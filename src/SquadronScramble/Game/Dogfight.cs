using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SquadronScramble;

public class Dogfight : Game
{
	// The Xbox game was authored for a fixed 1280x720 screen. On PC it is rendered
	// at that size and then scaled (letterboxed) to fit the window.
	public const int VirtualWidth = 1280;

	public const int VirtualHeight = 720;

	private GraphicsDeviceManager graphics;

	private SpriteBatch spriteBatch;

	private GameWorld theGame;

	private GamerServicesComponent GSC;

	// Trial mode only existed on Xbox Live; kept so the game code compiles, always null on PC.
	public TrialModeCounter trialModeCounter;

	private RenderTarget2D screen;

	private KeyboardState previousKeys;

	private Point windowedSize = new Point(VirtualWidth, VirtualHeight);

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
		GSC = new GamerServicesComponent(this);
		base.Components.Add(GSC);
	}

	protected override void Initialize()
	{
		theGame = new GameWorld(this);
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
		theGame.LoadContent(base.Content);
	}

	protected override void UnloadContent()
	{
		screen?.Dispose();
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
		previousKeys = keys;
		theGame.Update(gameTime);
		base.Update(gameTime);
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
		spriteBatch.Begin();
		theGame.Draw(gameTime, spriteBatch);
		spriteBatch.End();
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

	public TrialModeCounter GetTrialModeCounter()
	{
		return trialModeCounter;
	}
}
