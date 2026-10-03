using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SquadronScramble;

public class FrontEnd
{
	private enum State
	{
		pressStart,
		mainMenu,
		editSquadrons,
		options,
		returnToArcade,
		credits
	}

	private enum SubOptionState
	{
		off,
		soundVolumeControl,
		musicVolumeControl,
		screenModeControl,
		checkQuit
	}

	private GameWorld g;

	private Video video;

	private VideoPlayer vidPlayer;

	private Texture2D videoTexture;

	private float videoTimer = 0f;

	private Vector2 OPTIONPOSITION = new Vector2(650f, 450f);

	private int optionSwitch = 0;

	private string gameTitleString1 = "SQUADRON";

	private string gameTitleString2 = "SCRAMBLE";

	private string gameTitleString3 = "1 - 8 PLAYER DOGFIGHTING";

	private string gameTitleString4 = "ON ONE CONSOLE";

	private string trialModeString = "TRIAL VERSION";

	private string titleString = "";

	private string pressStartString = "PRESS START";

	private float pressStartFlashTimer = 0f;

	private string[] optionString;

	private float[] optionSize;

	private int MAXOPTIONLIMIT = 5;

	private int currentOptionLimit = 0;

	private int selectingController = 0;

	private bool frontEndComplete = false;

	private bool controlsDisabled = true;

	private bool frontEndMusicOn = false;

	private float musicDelay = 0f;

	private float timeLimit = 0f;

	private bool loaded = false;

	private float loadTimer = 0f;

	private Color loadColor = new Color(0, 0, 0);

	private bool companyNameOn = false;

	private bool diskSoundPlayed = false;

	private string companyNameString = "DepthCharge Software";

	private string presentsString = "presents";

	private float titleSize = 0f;

	private float subSize = 0f;

	private bool titleOn = false;

	private bool subOn = false;

	private bool startOn = false;

	private float introTimer = 0f;

	private State currentState = State.pressStart;

	private SubOptionState currentSubOptionState = SubOptionState.off;

	private Texture2D backgroundTexture;

	private Texture2D companyTexture;

	private Texture2D loadingTexture;

	private Texture2D elementsTexture;

	private int NUMBEROFPLANES = 2;

	private float[] planeTimer;

	private Vector2[] planePosition;

	private FrontEndExhaustManager[] planeExhaust;

	private float[] direction;

	private float engineSpeed = 2000f;

	private Color[] planeColor;

	private int NUMBEROFPILOTS = 1;

	private float PILOTFALLINGSPEED = 200f;

	private float[] pilotTimer;

	private Vector2[] pilotPosition;

	private float[] pilotDirection;

	private float[] pilotRotation;

	private Color[] pilotColor;

	private float[] pilotRotationRockTimer;

	private float[] pilotRotationRock;

	private int NUMBEROFCLOUDS = 3;

	private Vector2[] cloudPosition;

	private float[] cloudSpeed;

	public FrontEnd(GameWorld gw)
	{
		g = gw;
		optionString = new string[MAXOPTIONLIMIT];
		optionSize = new float[MAXOPTIONLIMIT];
		planePosition = new Vector2[NUMBEROFPLANES];
		planeColor = new Color[NUMBEROFPLANES];
		planeTimer = new float[NUMBEROFPLANES];
		direction = new float[NUMBEROFPLANES];
		planeExhaust = new FrontEndExhaustManager[NUMBEROFPLANES];
		for (int i = 0; i < NUMBEROFPLANES; i++)
		{
			if (i == 0)
			{
				planeTimer[i] = General.GetNextRandom(2, 4);
			}
			else
			{
				planeTimer[i] = planeTimer[i - 1] + 2f;
			}
			ref Vector2 reference = ref planePosition[i];
			reference = new Vector2(-1000f, -1000f);
			ref Color reference2 = ref planeColor[i];
			reference2 = Color.White;
			direction[i] = (float)Math.PI;
			planeExhaust[i] = new FrontEndExhaustManager(planePosition[i], direction[i]);
		}
		pilotPosition = new Vector2[NUMBEROFPILOTS];
		pilotColor = new Color[NUMBEROFPILOTS];
		pilotTimer = new float[NUMBEROFPILOTS];
		pilotDirection = new float[NUMBEROFPILOTS];
		pilotRotation = new float[NUMBEROFPILOTS];
		pilotRotationRockTimer = new float[NUMBEROFPILOTS];
		pilotRotationRock = new float[NUMBEROFPILOTS];
		for (int i = 0; i < NUMBEROFPILOTS; i++)
		{
			if (i == 0)
			{
				pilotTimer[i] = General.GetNextRandom(15, 25);
			}
			else
			{
				pilotTimer[i] = pilotTimer[i - 1] + 2f;
			}
			ref Vector2 reference3 = ref pilotPosition[i];
			reference3 = new Vector2(-1000f, -1000f);
			ref Color reference4 = ref pilotColor[i];
			reference4 = Color.White;
			pilotDirection[i] = (float)Math.PI;
			pilotRotation[i] = 0f;
			pilotRotationRockTimer[i] = 0f;
			pilotRotationRock[i] = 0f;
		}
		cloudPosition = new Vector2[NUMBEROFCLOUDS];
		cloudSpeed = new float[NUMBEROFCLOUDS];
		ref Vector2 reference5 = ref cloudPosition[0];
		reference5 = new Vector2(200f, 150f);
		ref Vector2 reference6 = ref cloudPosition[1];
		reference6 = new Vector2(800f, 500f);
		ref Vector2 reference7 = ref cloudPosition[2];
		reference7 = new Vector2(-1500f, 850f);
		cloudSpeed[0] = 20f;
		cloudSpeed[1] = 40f;
		cloudSpeed[2] = 50f;
	}

	public void LoadContent(ContentManager theContentManager)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		vidPlayer = new VideoPlayer();
		backgroundTexture = theContentManager.Load<Texture2D>("ScreenFrontEnd");
		companyTexture = theContentManager.Load<Texture2D>("ScreenCompany");
		loadingTexture = theContentManager.Load<Texture2D>("ScreenRoundResult");
		elementsTexture = TextureManager.GetElementsFrontEndTexture();
		for (int i = 0; i < NUMBEROFPLANES; i++)
		{
			planeExhaust[i].LoadContent();
		}
	}

	public void Update(GameTime theGameTime)
	{
		if (!loaded)
		{
			LoadSequence(theGameTime);
		}
		else
		{
			CheckControls(theGameTime);
			CheckMusic(theGameTime);
		}
		UpdateAllOptions(theGameTime);
		UpdateScenery(theGameTime);
		g.theEditSquadronScreen.UpdateDiskCounter(theGameTime);
	}

	public void CheckMusic(GameTime theGameTime)
	{
		if (musicDelay > 0f)
		{
			musicDelay -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (musicDelay < 0f)
			{
				musicDelay = 0f;
			}
		}
		if (vidPlayer.State == MediaState.Playing)
		{
			if (frontEndMusicOn)
			{
				g.theSoundManager.StopTitleMusic();
			}
		}
		else if (!frontEndMusicOn && musicDelay == 0f)
		{
			g.theSoundManager.StartTitleMusic();
		}
	}

	public void CheckVideo(GameTime theGameTime)
	{
		if (vidPlayer.State == MediaState.Playing)
		{
			videoTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else if (videoTimer > 0f)
		{
			controlsDisabled = false;
			videoTexture = null;
			musicDelay = 0.25f;
			g.theScreenFadeOverlay.SetAlphaValue(1f);
			g.theScreenFadeOverlay.UnFadeScreen(0.75f);
			videoTimer = 0f;
		}
	}

	public void FrontEndMusicOn(bool b)
	{
		frontEndMusicOn = b;
	}

	public void LoadSequence(GameTime theGameTime)
	{
		if (loadTimer == 0f)
		{
			loadTimer = 10f;
			diskSoundPlayed = true;
		}
		loadTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (loadTimer >= 0f)
		{
			loadColor = new Color(0, 0, 0);
		}
		if (loadTimer >= 1.5f)
		{
			loadColor = new Color(63, 63, 63);
		}
		if (loadTimer >= 3f)
		{
			loadColor = new Color(126, 126, 126);
		}
		if (loadTimer >= 4.5f)
		{
			loadColor = new Color(254, 254, 254);
		}
		if (loadTimer >= 7f)
		{
			if (!diskSoundPlayed)
			{
				diskSoundPlayed = true;
			}
			loadColor = new Color(0, 0, 0);
		}
		if (loadTimer >= 11f)
		{
			if (!companyNameOn && loadTimer < 15f)
			{
				g.theSoundManager.SonarSound();
			}
			companyNameOn = true;
		}
		if (loadTimer >= 15f)
		{
			companyNameOn = false;
		}
		if (loadTimer >= 17f)
		{
			controlsDisabled = false;
			loaded = true;
			companyNameOn = false;
			g.GoToFrontEnd();
		}
	}

	public void UpdateScenery(GameTime theGameTime)
	{
		for (int i = 0; i < NUMBEROFPLANES; i++)
		{
			Vector2 vector = new Vector2(0f, 0f);
			vector.X = (float)Math.Cos(direction[i]);
			vector.Y = (float)Math.Sin(direction[i]);
			planePosition[i] += vector * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			planeExhaust[i].Update(theGameTime, planePosition[i], direction[i]);
			if (loaded)
			{
				planeTimer[i] -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (planeTimer[i] <= 0f)
			{
				direction[i] = (float)General.GetNextRandom(1, 360) * ((float)Math.PI / 180f);
				vector.X = (float)Math.Cos(direction[i]);
				vector.Y = (float)Math.Sin(direction[i]);
				ref Vector2 reference = ref planePosition[i];
				reference = new Vector2(500f - vector.X * 1500f + (float)(-200 + 100 * General.GetNextRandom(0, 4)), 360f - vector.Y * 1500f);
				planeTimer[i] = General.GetNextRandom(6, 10);
				switch (General.GetNextRandom(0, 4))
				{
				case 0:
				{
					ref Color reference5 = ref planeColor[i];
					reference5 = new Color(255, 255, 255);
					break;
				}
				case 1:
				{
					ref Color reference4 = ref planeColor[i];
					reference4 = new Color(250, 250, 0);
					break;
				}
				case 2:
				{
					ref Color reference3 = ref planeColor[i];
					reference3 = new Color(44, 210, 52);
					break;
				}
				default:
				{
					ref Color reference2 = ref planeColor[i];
					reference2 = new Color(151, 176, 227);
					break;
				}
				}
			}
		}
		for (int i = 0; i < NUMBEROFPILOTS; i++)
		{
			Vector2 vector = new Vector2(0f, 0f);
			vector.X = (float)Math.Cos(pilotDirection[i]);
			vector.Y = (float)Math.Sin(pilotDirection[i]);
			pilotPosition[i] += new Vector2(vector.X * 100f, PILOTFALLINGSPEED) * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			float num = 3f;
			float num2 = 200f;
			pilotRotationRockTimer[i] += num * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			pilotRotationRock[i] = (float)Math.Cos(pilotRotationRockTimer[i]);
			pilotRotation[i] += pilotRotationRock[i] / num2;
			if (loaded)
			{
				pilotTimer[i] -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (pilotTimer[i] <= 0f)
			{
				pilotDirection[i] = (float)Math.PI * 3f / 8f + (float)General.GetNextRandom(0, 3) * ((float)Math.PI / 8f);
				vector.X = (float)Math.Cos(pilotDirection[i]);
				vector.Y = (float)Math.Sin(pilotDirection[i]);
				ref Vector2 reference6 = ref pilotPosition[i];
				reference6 = new Vector2(100 + General.GetNextRandom(0, 1000), -200f);
				pilotTimer[i] = General.GetNextRandom(15, 25);
				switch (General.GetNextRandom(0, 4))
				{
				case 0:
				{
					ref Color reference10 = ref pilotColor[i];
					reference10 = new Color(255, 255, 255);
					break;
				}
				case 1:
				{
					ref Color reference9 = ref pilotColor[i];
					reference9 = new Color(250, 250, 0);
					break;
				}
				case 2:
				{
					ref Color reference8 = ref pilotColor[i];
					reference8 = new Color(44, 210, 52);
					break;
				}
				default:
				{
					ref Color reference7 = ref pilotColor[i];
					reference7 = new Color(151, 176, 227);
					break;
				}
				}
			}
		}
		for (int i = 0; i < NUMBEROFCLOUDS; i++)
		{
			cloudPosition[i].X += cloudSpeed[i] * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (cloudPosition[i].X > 2000f)
			{
				cloudPosition[i].X = -700f;
			}
		}
	}

	public void DrawScenery(SpriteBatch theSpriteBatch)
	{
		if (!loaded)
		{
			Vector2 origin = new Vector2(0f, 0f);
			theSpriteBatch.Draw(loadingTexture, new Vector2(0f, 0f), null, loadColor, 0f, origin, 1f, SpriteEffects.None, 0f);
			if (companyNameOn)
			{
				theSpriteBatch.Draw(companyTexture, new Vector2(0f, 0f), null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			}
		}
		else
		{
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), texture: backgroundTexture, position: new Vector2(0f, 0f), sourceRectangle: null, color: Color.White, rotation: 0f, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			DrawClouds(theSpriteBatch);
			DrawExhausts(theSpriteBatch);
			DrawPlanes(theSpriteBatch);
			DrawPilots(theSpriteBatch);
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		DrawScenery(theSpriteBatch);
		if (loaded)
		{
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameTitle(), gameTitleString1, new Vector2(OPTIONPOSITION.X, 135f), new Color(225, 0, 0), new Color(255, 0, 0), new Color(155, 0, 0), 0f, g.theFontManager.GetFontGameTitle().MeasureString(gameTitleString1) / 2f, titleSize, 1, 1f);
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameTitle(), gameTitleString2, new Vector2(OPTIONPOSITION.X, 265f), new Color(225, 0, 0), new Color(255, 0, 0), new Color(155, 0, 0), 0f, g.theFontManager.GetFontGameTitle().MeasureString(gameTitleString2) / 2f, titleSize, 1, 1f);
			if (currentState == State.pressStart)
			{
				General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), gameTitleString3, new Vector2(OPTIONPOSITION.X, 400f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, g.theFontManager.GetHiScoreFont().MeasureString(gameTitleString3) / 2f, subSize, 1, 1f);
				General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), gameTitleString4, new Vector2(OPTIONPOSITION.X, 450f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, g.theFontManager.GetHiScoreFont().MeasureString(gameTitleString4) / 2f, subSize, 1, 1f);
				float num = (float)Math.Sin(pressStartFlashTimer * 1.5f);
				if (num >= (float)Math.PI * 2f)
				{
					pressStartFlashTimer = 0f;
				}
				if (startOn)
				{
					theSpriteBatch.DrawString(g.theFontManager.GetHiScoreFont(), pressStartString, new Vector2(OPTIONPOSITION.X + 3f, 603f), Color.Black * (1f - Math.Abs(num) / 1.25f), 0f, g.theFontManager.GetHiScoreFont().MeasureString(pressStartString) / 2f, 1f, SpriteEffects.None, 0f);
					theSpriteBatch.DrawString(g.theFontManager.GetHiScoreFont(), pressStartString, new Vector2(OPTIONPOSITION.X, 600f), Color.White * (1f - Math.Abs(num) / 1.25f), 0f, g.theFontManager.GetHiScoreFont().MeasureString(pressStartString) / 2f, 1f, SpriteEffects.None, 0f);
				}
			}
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameHeading1(), titleString, new Vector2(OPTIONPOSITION.X, 375f), new Color(225, 225, 0), new Color(255, 255, 0), new Color(195, 195, 0), 0f, g.theFontManager.GetFontGameHeading1().MeasureString(titleString) / 2f, 1f, 1, 1f);
			Rectangle value = new Rectangle(4, 437, 218, 30);
			if (currentSubOptionState == SubOptionState.off && currentState != State.credits)
			{
				if (currentState != 0)
				{
					theSpriteBatch.Draw(elementsTexture, new Vector2(OPTIONPOSITION.X, OPTIONPOSITION.Y + (float)(50 * optionSwitch) - 3f), value, new Color(200, 200, 255) * 0.75f, 0f, new Vector2(value.Width / 2, value.Height / 2), 1.5f, SpriteEffects.None, 0f);
				}
				for (int i = 0; i < MAXOPTIONLIMIT; i++)
				{
					General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), optionString[i], new Vector2(OPTIONPOSITION.X, OPTIONPOSITION.Y + (float)(50 * i)), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, g.theFontManager.GetHiScoreFont().MeasureString(optionString[i]) / 2f, optionSize[i], 1, 1f);
				}
				General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), optionString[optionSwitch], new Vector2(OPTIONPOSITION.X, OPTIONPOSITION.Y + (float)(50 * optionSwitch)), new Color((int)(225f * General.GetOptionThrobValue()), 0, 0), new Color((int)(255f * General.GetOptionThrobValue()), 0, 0), new Color((int)(195f * General.GetOptionThrobValue()), 0, 0), 0f, g.theFontManager.GetHiScoreFont().MeasureString(optionString[optionSwitch]) / 2f, optionSize[optionSwitch], 1, 1f);
			}
			if (currentSubOptionState == SubOptionState.soundVolumeControl)
			{
				g.theOptionsOverlay.DrawVolumeControl(theSpriteBatch, new Vector2(650f, 510f), "ADJUST SOUND VOLUME");
			}
			if (currentSubOptionState == SubOptionState.musicVolumeControl)
			{
				g.theOptionsOverlay.DrawVolumeControl(theSpriteBatch, new Vector2(650f, 510f), "ADJUST MUSIC VOLUME");
			}
			if (currentSubOptionState == SubOptionState.screenModeControl)
			{
				g.theOptionsOverlay.DrawScreenModeControl(theSpriteBatch, new Vector2(650f, 510f), "SELECT SCREEN AREA");
			}
			if (currentSubOptionState == SubOptionState.checkQuit)
			{
				g.theOptionsOverlay.DrawYesNoOption(theSpriteBatch, new Vector2(650f, 510f), "EXIT GAME?", "YES", "NO");
			}
			if (currentState != 0 && currentState != State.credits)
			{
				ControlOverlay.DrawAB(theSpriteBatch);
			}
			if (currentState == State.options && currentSubOptionState == SubOptionState.off)
			{
				ControlOverlay.DrawYCredits(theSpriteBatch);
			}
			if (currentState == State.credits)
			{
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), "CREATED BY", OPTIONPOSITION + new Vector2(0f, -15f), Color.White, 0f, g.theFontManager.GetHiScoreFont().MeasureString("CREATED BY") / 2f, 1f, 1, 1f);
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), "Daniel Chequer", OPTIONPOSITION + new Vector2(0f, 15f), Color.White, 0f, g.theFontManager.GetFont().MeasureString("Daniel Chequer") / 2f, 1f, 1, 1f);
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), "'WARMONGER BB' font licensed from Nate Piekos (Blambot.com)", OPTIONPOSITION + new Vector2(0f, 50f), Color.White, 0f, g.theFontManager.GetFont().MeasureString("'WARMONGER BB' font licensed from Nate Piekos (Blambot.com)") / 2f, 1f, 1, 1f);
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), "Additional music & sound effects licensed from 1SoundFX.com", OPTIONPOSITION + new Vector2(0f, 85f), Color.White, 0f, g.theFontManager.GetFont().MeasureString("Additional music & sound effects licensed from 1SoundFX.com") / 2f, 1f, 1, 1f);
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), "SPECIAL THANKS TO", OPTIONPOSITION + new Vector2(0f, 125f), Color.White, 0f, g.theFontManager.GetHiScoreFont().MeasureString("SPECIAL THANKS TO") / 2f, 1f, 1, 1f);
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), "Vanessa Dalby", OPTIONPOSITION + new Vector2(0f, 155f), Color.White, 0f, g.theFontManager.GetFont().MeasureString("Vanessa Dalby") / 2f, 1f, 1, 1f);
				ControlOverlay.DrawB(theSpriteBatch);
			}
			if (g.theEditSquadronScreen.GetIsSaving())
			{
				ControlOverlay.DrawSaving(theSpriteBatch);
			}
			if (g.theEditSquadronScreen.GetIsLoading())
			{
				ControlOverlay.DrawLoading(theSpriteBatch);
			}
			if (((g.d.GetTrialModeCounter() != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) && startOn)
			{
				Vector2 vector = new Vector2(OPTIONPOSITION.X + 164f, 325f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), trialModeString, vector + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(trialModeString) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), trialModeString, vector + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(trialModeString) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), trialModeString, vector + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(trialModeString) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), trialModeString, vector + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(trialModeString) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), trialModeString, vector, Color.Silver, 0f, g.theFontManager.GetFont().MeasureString(trialModeString) / 2f, 1f, SpriteEffects.None, 0f);
			}
		}
		if (vidPlayer.State != 0 && videoTimer > 0.5f)
		{
			g.theScreenFadeOverlay.SetAlphaValue(0f);
			videoTexture = vidPlayer.GetTexture();
			Rectangle destinationRectangle = new Rectangle(0, 0, 1280, 720);
			if (videoTexture != null)
			{
				theSpriteBatch.Draw(videoTexture, destinationRectangle, Color.White);
			}
		}
	}

	public void DrawPlanes(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(200f, 140f);
		Rectangle value = new Rectangle(0, 0, 410, 280);
		for (int i = 0; i < NUMBEROFPLANES; i++)
		{
			theSpriteBatch.Draw(elementsTexture, planePosition[i], value, planeColor[i], direction[i], origin, 1f, SpriteEffects.None, 0f);
		}
	}

	public void DrawPilots(SpriteBatch theSpriteBatch)
	{
		Rectangle rectangle = new Rectangle(850, 0, 138, 208);
		for (int i = 0; i < NUMBEROFPILOTS; i++)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(850, 0, 138, 208), texture: elementsTexture, position: pilotPosition[i], color: pilotColor[i], rotation: pilotRotation[i], origin: new Vector2(69f, 104f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(850, 210, 138, 60), texture: elementsTexture, position: pilotPosition[i], color: Color.White, rotation: pilotRotation[i], origin: new Vector2(70f, 5f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
	}

	public void DrawExhausts(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < NUMBEROFPLANES; i++)
		{
			planeExhaust[i].Draw(theSpriteBatch);
		}
	}

	public void DrawClouds(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(200f, 170f);
		Rectangle value = new Rectangle(447, 256, 417, 177);
		for (int i = 0; i < NUMBEROFCLOUDS; i++)
		{
			theSpriteBatch.Draw(elementsTexture, cloudPosition[i], value, Color.White, 0f, origin, 3f, SpriteEffects.None, 0f);
		}
	}

	public void UpdateAllOptions(GameTime theGameTime)
	{
		if (!Guide.IsVisible)
		{
			if (currentState == State.pressStart)
			{
				PressStartOptions(theGameTime);
			}
			if (currentState == State.mainMenu)
			{
				MainMenuOptions(theGameTime);
			}
			else if (currentState == State.options)
			{
				Options(theGameTime);
			}
			else if (currentState == State.credits)
			{
				Credits(theGameTime);
			}
		}
	}

	public void PressStartOptions(GameTime theGameTime)
	{
		if (loaded)
		{
			introTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		if (introTimer > 0f)
		{
			titleOn = true;
		}
		if (introTimer > 1f)
		{
			subOn = true;
		}
		if (introTimer > 2f)
		{
			if (!startOn)
			{
			}
			startOn = true;
			introTimer = 2f;
		}
		if (titleOn)
		{
			if (titleSize < 1f)
			{
				titleSize += 3f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (titleSize >= 1f)
			{
				titleSize = 1f;
			}
		}
		if (subOn)
		{
			if (subSize < 1f)
			{
				subSize += 4f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (subSize >= 1f)
			{
				subSize = 1f;
			}
		}
		pressStartFlashTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		currentOptionLimit = 0;
		titleString = "";
		optionString[0] = "";
		optionString[1] = "";
		optionString[2] = "";
		optionString[3] = "";
		optionString[4] = "";
	}

	public void MainMenuOptions(GameTime theGameTime)
	{
		currentOptionLimit = 3;
		titleString = "MAIN MENU";
		optionString[0] = "PLAY";
		optionString[1] = "EDIT PILOTS";
		optionString[2] = "OPTIONS";
		optionString[3] = "EXIT";
		optionString[4] = "";
		optionSize[0] = 0.75f;
		optionSize[1] = 0.75f;
		optionSize[2] = 0.75f;
		optionSize[3] = 0.75f;
		optionSize[4] = 0.75f;
		float num = 1f;
		switch (optionSwitch)
		{
		case 0:
			optionSize[0] = num;
			if (IsValidOption())
			{
				frontEndComplete = true;
				controlsDisabled = true;
				g.theScreenFadeOverlay.FadeScreen(3f);
				timeLimit = 1f;
			}
			break;
		case 1:
			optionSize[1] = num;
			if (IsValidOption())
			{
				g.SetCurrentMenuController(selectingController);
				g.theEditSquadronScreen.UpdateNames();
				g.GoToEditSquadronScreen();
			}
			break;
		case 2:
			optionSize[2] = num;
			if (IsValidOption())
			{
				currentState = State.options;
				optionSwitch = 0;
			}
			break;
		case 3:
			optionSize[3] = num;
			if (IsValidOption())
			{
				g.theOptionsOverlay.SetOptionBooleanIsYes(b: false);
				currentSubOptionState = SubOptionState.checkQuit;
				optionSwitch = 0;
			}
			break;
		}
		if (frontEndComplete)
		{
			timeLimit -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (timeLimit <= 0f)
			{
				timeLimit = 0f;
				g.theScreenFadeOverlay.UnFadeScreen(3f);
				controlsDisabled = false;
				g.GoToControllerSelectionScreen();
			}
		}
	}

	public void Options(GameTime theGameTime)
	{
		currentOptionLimit = 3;
		titleString = "OPTIONS";
		optionString[0] = "SOUND VOLUME";
		optionString[1] = "MUSIC VOLUME";
		optionString[2] = "ADJUST SCREEN";
		optionString[3] = "DONE";
		optionString[4] = "";
		optionSize[0] = 0.75f;
		optionSize[1] = 0.75f;
		optionSize[2] = 0.75f;
		optionSize[3] = 0.75f;
		optionSize[4] = 0.75f;
		float num = 1f;
		switch (optionSwitch)
		{
		case 0:
			optionSize[0] = num;
			if (IsValidOption())
			{
				g.theSoundManager.SetProvVolume(g.theSoundManager.GetVolumeSFX());
				currentSubOptionState = SubOptionState.soundVolumeControl;
			}
			break;
		case 1:
			optionSize[1] = num;
			if (IsValidOption())
			{
				g.theSoundManager.SetProvVolume(g.theSoundManager.GetVolumeMusic());
				currentSubOptionState = SubOptionState.musicVolumeControl;
			}
			break;
		case 2:
			optionSize[2] = num;
			if (IsValidOption())
			{
				g.theSafeArea.SetProvScreenMode(g.theSafeArea.GetScreenMode());
				g.theOptionsOverlay.SetIsAdjustingScreen(b: true);
				currentSubOptionState = SubOptionState.screenModeControl;
			}
			break;
		case 3:
			optionSize[3] = num;
			if (IsValidOption())
			{
				currentState = State.mainMenu;
				optionSwitch = 2;
				if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode))
				{
					g.SaveData();
				}
			}
			break;
		}
	}

	public void Credits(GameTime theGameTime)
	{
		currentOptionLimit = 3;
		titleString = "CREDITS";
		optionString[0] = "";
		optionString[1] = "";
		optionString[2] = "";
		optionString[3] = "";
		optionString[4] = "";
		optionSize[0] = 0.75f;
		optionSize[1] = 0.75f;
		optionSize[2] = 0.75f;
		optionSize[3] = 0.75f;
		optionSize[4] = 0.75f;
		float num = 1f;
	}

	public void CheckControls(GameTime theGameTime)
	{
		if (Guide.IsVisible)
		{
			return;
		}
		if (!controlsDisabled && currentSubOptionState == SubOptionState.off)
		{
			for (int i = 0; i < g.GetNumberOfControllers(); i++)
			{
				if (currentState != State.credits)
				{
					if (g.theControllerMenuManager[i].CheckLeftUpPressed() && currentState != 0)
					{
						OptionUp();
					}
					if (g.theControllerMenuManager[i].CheckLeftDownPressed() && currentState != 0)
					{
						OptionDown();
					}
					if (g.theControllerMenuManager[i].CheckButtonBPressed() || g.theControllerMenuManager[i].CheckButtonBackPressed())
					{
						if (currentState == State.mainMenu)
						{
							currentState = State.pressStart;
							g.theSoundManager.MenuSelectSound();
						}
						if (currentState == State.options)
						{
							currentState = State.mainMenu;
							g.theSoundManager.MenuSelectSound();
							if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode))
							{
								g.SaveData();
							}
						}
					}
					if (currentState == State.pressStart && g.theControllerMenuManager[i].CheckButtonStartPressed() && startOn)
					{
						g.theSoundManager.MenuSelectSound();
						currentState = State.mainMenu;
						g.theEditSquadronScreen.ResetAllNames();
						g.theEditSquadronScreen.SetSavingEnabled(b: false);
						g.LoadData();
					}
					if (currentState == State.options && currentSubOptionState == SubOptionState.off && g.theControllerMenuManager[i].CheckButtonYPressed())
					{
						g.theSoundManager.MenuSelectSound();
						currentState = State.credits;
					}
				}
				if (currentState == State.credits && g.theControllerMenuManager[i].CheckButtonBPressed())
				{
					g.theSoundManager.MenuSelectSound();
					currentState = State.options;
				}
			}
		}
		if (!controlsDisabled && currentSubOptionState == SubOptionState.soundVolumeControl)
		{
			for (int i = 0; i < g.GetNumberOfControllers(); i++)
			{
				if (g.theControllerMenuManager[i].CheckLeftRightPressed())
				{
					g.theSoundManager.IncrementProvSFXVolume();
				}
				if (g.theControllerMenuManager[i].CheckLeftLeftPressed())
				{
					g.theSoundManager.DecrementProvSFXVolume();
				}
				if (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed())
				{
					g.theSoundManager.MenuSelectSound();
					g.theSoundManager.SetVolumeSFX(g.theSoundManager.GetProvVolume());
					currentSubOptionState = SubOptionState.off;
				}
				if (g.theControllerMenuManager[i].CheckButtonBPressed() || g.theControllerMenuManager[i].CheckButtonBackPressed())
				{
					g.theSoundManager.MenuSelectSound();
					currentSubOptionState = SubOptionState.off;
				}
			}
		}
		if (!controlsDisabled && currentSubOptionState == SubOptionState.musicVolumeControl)
		{
			g.theSoundManager.SetCurrentStateSettingMusic();
			for (int i = 0; i < g.GetNumberOfControllers(); i++)
			{
				if (g.theControllerMenuManager[i].CheckLeftRightPressed())
				{
					g.theSoundManager.IncrementProvMusicVolume();
				}
				if (g.theControllerMenuManager[i].CheckLeftLeftPressed())
				{
					g.theSoundManager.DecrementProvMusicVolume();
				}
				if (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed())
				{
					g.theSoundManager.MenuSelectSound();
					g.theSoundManager.SetVolumeMusic(g.theSoundManager.GetProvVolume());
					currentSubOptionState = SubOptionState.off;
					g.theSoundManager.SetCurrentStateNormal();
				}
				if (g.theControllerMenuManager[i].CheckButtonBPressed() || g.theControllerMenuManager[i].CheckButtonBackPressed())
				{
					g.theSoundManager.MenuSelectSound();
					g.theSoundManager.SetVolumeMusic(g.theSoundManager.GetVolumeMusic());
					currentSubOptionState = SubOptionState.off;
					g.theSoundManager.SetCurrentStateNormal();
				}
			}
		}
		if (!controlsDisabled && currentSubOptionState == SubOptionState.screenModeControl)
		{
			for (int i = 0; i < g.GetNumberOfControllers(); i++)
			{
				if (g.theControllerMenuManager[i].CheckLeftRightPressed())
				{
					g.theSoundManager.MenuSwitchSound();
					g.theSafeArea.IncreaseProvScreenMode();
				}
				if (g.theControllerMenuManager[i].CheckLeftLeftPressed())
				{
					g.theSoundManager.MenuSwitchSound();
					g.theSafeArea.DecreaseProvScreenMode();
				}
				if (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed())
				{
					g.theSoundManager.MenuSelectSound();
					g.theSafeArea.SetScreenMode(g.theSafeArea.GetProvScreenMode());
					g.theOptionsOverlay.SetIsAdjustingScreen(b: false);
					currentSubOptionState = SubOptionState.off;
				}
				if (g.theControllerMenuManager[i].CheckButtonBPressed() || g.theControllerMenuManager[i].CheckButtonBackPressed())
				{
					g.theSoundManager.MenuSelectSound();
					g.theOptionsOverlay.SetIsAdjustingScreen(b: false);
					currentSubOptionState = SubOptionState.off;
				}
			}
		}
		if (currentSubOptionState != SubOptionState.checkQuit)
		{
			return;
		}
		for (int i = 0; i < g.GetNumberOfControllers(); i++)
		{
			if (g.theControllerMenuManager[i].CheckLeftRightPressed() && g.theOptionsOverlay.GetOptionBooleanIsYes())
			{
				g.theOptionsOverlay.SetOptionBooleanIsYes(b: false);
			}
			if (g.theControllerMenuManager[i].CheckLeftLeftPressed() && !g.theOptionsOverlay.GetOptionBooleanIsYes())
			{
				g.theOptionsOverlay.SetOptionBooleanIsYes(b: true);
			}
			if (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed())
			{
				if (!g.theOptionsOverlay.GetOptionBooleanIsYes())
				{
					g.theSoundManager.MenuSelectSound();
					currentState = State.mainMenu;
					currentSubOptionState = SubOptionState.off;
					optionSwitch = 3;
				}
				if (g.theOptionsOverlay.GetOptionBooleanIsYes())
				{
					g.theSoundManager.MenuSelectSound();
					g.GetD().Exit();
				}
			}
			if (g.theControllerMenuManager[i].CheckButtonBPressed())
			{
				g.theSoundManager.MenuSelectSound();
				currentState = State.mainMenu;
				currentSubOptionState = SubOptionState.off;
				optionSwitch = 3;
			}
		}
	}

	public void PlayVideo()
	{
		if (vidPlayer.State == MediaState.Stopped)
		{
			vidPlayer.IsLooped = false;
			vidPlayer.Play(video);
		}
	}

	public void SetFrontEndComplete(bool b)
	{
		frontEndComplete = b;
	}

	public void OptionUp()
	{
		if (optionSwitch > 0)
		{
			optionSwitch--;
		}
		else
		{
			optionSwitch = currentOptionLimit;
		}
		g.theSoundManager.MenuSwitchSound();
	}

	public void OptionDown()
	{
		if (optionSwitch < currentOptionLimit)
		{
			optionSwitch++;
		}
		else
		{
			optionSwitch = 0;
		}
		g.theSoundManager.MenuSwitchSound();
	}

	public bool IsValidOption()
	{
		for (int i = 0; i < g.GetNumberOfControllers(); i++)
		{
			if ((g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed()) && !controlsDisabled)
			{
				g.theSoundManager.MenuSelectSound();
				selectingController = i;
				return true;
			}
		}
		return false;
	}

	public bool GetLoaded()
	{
		return loaded;
	}
}
