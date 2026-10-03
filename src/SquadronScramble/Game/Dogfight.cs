using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using SquadronScramble.Net;

namespace SquadronScramble;

public class Dogfight : Game
{
	// The Xbox game was authored for a fixed 1280x720 screen. On PC it is rendered
	// at that size and then scaled (letterboxed) to fit the window.
	public const int VirtualWidth = 1280;

	public const int VirtualHeight = 720;

	// Online play: at most this many simulation frames per update when catching up.
	private const int MaxCatchUpFrames = 6;

	private GraphicsDeviceManager graphics;

	private SpriteBatch spriteBatch;

	private GameWorld theGame;

	private GamerServicesComponent GSC;

	// Trial mode only existed on Xbox Live; kept so the game code compiles, always null on PC.
	public TrialModeCounter trialModeCounter;

	private RenderTarget2D screen;

	private KeyboardState previousKeys;

	private Point windowedSize = new Point(VirtualWidth, VirtualHeight);

	private readonly NetLauncherOptions launcherOptions;

	private NetLauncher launcher;

	// Online play session, or null for a local game.
	private NetSession net;

	private int stalledUpdates;

	private SpriteFont uiFont;

	private Texture2D pixel;

	public Dogfight(string[] args)
	{
		graphics = new GraphicsDeviceManager(this);
		base.Content = new CaseInsensitiveContentManager(base.Services, "Content");
		graphics.PreferredBackBufferWidth = VirtualWidth;
		graphics.PreferredBackBufferHeight = VirtualHeight;
		graphics.SynchronizeWithVerticalRetrace = true;
		graphics.HardwareModeSwitch = false;
		base.IsFixedTimeStep = true;
		base.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
		base.Window.Title = "Squadron Scramble";
		base.Window.AllowUserResizing = true;
		base.IsMouseVisible = false;
		if (args != null && Array.Exists(args, (string a) => a.Equals("--fullscreen", StringComparison.OrdinalIgnoreCase) || a.Equals("-f", StringComparison.OrdinalIgnoreCase)))
		{
			graphics.IsFullScreen = true;
		}
		launcherOptions = NetLauncherOptions.Parse(args);
		GSC = new GamerServicesComponent(this);
		base.Components.Add(GSC);
	}

	protected override void Initialize()
	{
		base.Initialize();
		if (graphics.IsFullScreen)
		{
			ApplyFullScreen(true);
		}
	}

	protected override void LoadContent()
	{
		spriteBatch = new SpriteBatch(base.GraphicsDevice);
		screen = new RenderTarget2D(base.GraphicsDevice, VirtualWidth, VirtualHeight, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
		pixel = new Texture2D(base.GraphicsDevice, 1, 1);
		pixel.SetData(new Color[1] { Color.White });
		uiFont = base.Content.Load<SpriteFont>("mySpriteFont1");
		launcher = new NetLauncher(launcherOptions, uiFont, base.Content.Load<SpriteFont>("fontGameHeading2"), base.Content.Load<Texture2D>("HangarScreenBlurred"), pixel);
	}

	protected override void UnloadContent()
	{
		screen?.Dispose();
		pixel?.Dispose();
	}

	/// <summary>Creates the game itself, once the player has chosen local or online play.</summary>
	private void StartGame(NetSession session)
	{
		launcher = null;
		net = session;
		if (net != null)
		{
			// Every PC must start from the same state: same random seed and the host's save data.
			General.SeedRandom(net.Seed);
			StorageDevice.UseVirtualFiles(net.SaveFiles, net.IsHost);
			net.BeginGame();
			// Keep simulating at full speed when the window is in the background, or everyone waits.
			base.InactiveSleepTime = TimeSpan.Zero;
		}
		theGame = new GameWorld(this);
		theGame.LoadContent(base.Content);
	}

	protected override void Update(GameTime gameTime)
	{
		KeyboardState keys = Keyboard.GetState();
		bool altEnter = keys.IsKeyDown(Keys.Enter) && (keys.IsKeyDown(Keys.LeftAlt) || keys.IsKeyDown(Keys.RightAlt)) && !previousKeys.IsKeyDown(Keys.Enter);
		bool f11 = keys.IsKeyDown(Keys.F11) && !previousKeys.IsKeyDown(Keys.F11);
		if (altEnter || f11)
		{
			ApplyFullScreen(!graphics.IsFullScreen);
		}
		bool escapePressed = keys.IsKeyDown(Keys.Escape) && !previousKeys.IsKeyDown(Keys.Escape);
		previousKeys = keys;

		if (launcher != null)
		{
			launcher.Update();
			switch (launcher.Result)
			{
			case NetLauncher.Outcome.Local:
				StartGame(null);
				break;
			case NetLauncher.Outcome.Online:
				StartGame(launcher.Session);
				break;
			case NetLauncher.Outcome.Quit:
				Exit();
				break;
			}
		}
		else if (net == null)
		{
			theGame.Update(gameTime);
		}
		else
		{
			UpdateOnline();
			if (net.Error != null && escapePressed)
			{
				Exit();
			}
		}
		base.Update(gameTime);
	}

	/// <summary>Online play: run as many lockstep frames as have arrived (normally exactly one).</summary>
	private void UpdateOnline()
	{
		net.Poll();
		net.PumpInput();
		int steps = 0;
		while (steps < MaxCatchUpFrames && net.TryBeginFrame(base.TargetElapsedTime, out GameTime frameTime))
		{
			theGame.Update(frameTime);
			net.EndFrame();
			net.PumpInput();
			steps++;
		}
		stalledUpdates = steps == 0 ? stalledUpdates + 1 : 0;
	}

	private void ApplyFullScreen(bool fullScreen)
	{
		if (fullScreen)
		{
			if (!graphics.IsFullScreen)
			{
				windowedSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			}
			DisplayMode mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			graphics.PreferredBackBufferWidth = mode.Width;
			graphics.PreferredBackBufferHeight = mode.Height;
			graphics.IsFullScreen = true;
		}
		else
		{
			graphics.PreferredBackBufferWidth = windowedSize.X;
			graphics.PreferredBackBufferHeight = windowedSize.Y;
			graphics.IsFullScreen = false;
		}
		graphics.ApplyChanges();
	}

	protected override void Draw(GameTime gameTime)
	{
		base.GraphicsDevice.SetRenderTarget(screen);
		base.GraphicsDevice.Clear(Color.CornflowerBlue);
		if (launcher != null)
		{
			launcher.Draw(spriteBatch, gameTime);
		}
		else
		{
			spriteBatch.Begin();
			theGame.Draw(gameTime, spriteBatch);
			spriteBatch.End();
			if (net != null)
			{
				DrawOnlineStatus();
			}
		}
		base.Draw(gameTime);
		base.GraphicsDevice.SetRenderTarget(null);

		Rectangle bounds = base.GraphicsDevice.PresentationParameters.Bounds;
		float scale = Math.Min((float)bounds.Width / VirtualWidth, (float)bounds.Height / VirtualHeight);
		int width = (int)(VirtualWidth * scale);
		int height = (int)(VirtualHeight * scale);
		Rectangle destination = new Rectangle((bounds.Width - width) / 2, (bounds.Height - height) / 2, width, height);
		base.GraphicsDevice.Clear(Color.Black);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp);
		spriteBatch.Draw(screen, destination, Color.White);
		spriteBatch.End();
	}

	private void DrawOnlineStatus()
	{
		string center = null;
		if (net.Error != null)
		{
			center = net.Error + "   [Esc] Quit";
		}
		else if (stalledUpdates > 30)
		{
			center = net.WaitingFor != null ? ("Waiting for " + net.WaitingFor + "...") : "Waiting for the other players...";
		}
		spriteBatch.Begin();
		if (net.Warning != null)
		{
			DrawBanner(net.Warning, 8f, new Color(255, 200, 80));
		}
		if (center != null)
		{
			DrawBanner(center, VirtualHeight / 2 - 20, Color.White);
		}
		spriteBatch.End();
	}

	private void DrawBanner(string text, float y, Color color)
	{
		text = new string(text.Select((char c) => uiFont.Characters.Contains(c) ? c : '?').ToArray());
		Vector2 size = uiFont.MeasureString(text);
		float scale = Math.Min(1f, (VirtualWidth - 40) / Math.Max(1f, size.X));
		size *= scale;
		Rectangle r = new Rectangle((int)(VirtualWidth / 2 - size.X / 2 - 16), (int)y - 8, (int)size.X + 32, (int)size.Y + 16);
		spriteBatch.Draw(pixel, r, Color.Black * 0.75f);
		spriteBatch.DrawString(uiFont, text, new Vector2(r.X + 16, y), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
	}

	protected override void OnExiting(object sender, EventArgs args)
	{
		net?.Dispose();
		launcher?.Session?.Dispose();
		base.OnExiting(sender, args);
	}

	public TrialModeCounter GetTrialModeCounter()
	{
		return trialModeCounter;
	}
}
