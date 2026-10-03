using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public static class Level
{
	private static GameWorld g;

	private static DropPlane theDropPlane;

	private static Vector2 position = new Vector2(0f, 0f);

	private static Cloud[] clouds = new Cloud[3];

	private static Cloud[] heavyCloudsBack = new Cloud[14];

	private static Cloud[] heavyCloudsFront = new Cloud[6];

	private static Snow[] snowsFront = new Snow[10];

	private static Snow[] snowsBack = new Snow[10];

	private static Wave[] waves = new Wave[12];

	private static MiniWave[] miniWaves = new MiniWave[3];

	private static Vector2 backgroundPosition = new Vector2(0f, 0f);

	private static float takeOffY = 0f;

	private static float groundY = 0f;

	private static float pilotGroundY = 0f;

	private static float absoluteGroundY = 0f;

	private static float landingMinX = 0f;

	private static float landingMaxX = 0f;

	private static float airfieldAnimCounter = 0f;

	private static Vector2 hangarPosition = new Vector2(0f, 0f);

	private static bool lightOn = false;

	private static float carrierAnimCounter = 0f;

	private static bool carrierLeft = false;

	private static float CARRIERHEIGHT = 625f;

	private static Vector2 carrierPosition = new Vector2(500f, CARRIERHEIGHT);

	private static float WAVEHEIGHT = 3f;

	private static float waveSinValue = 0f;

	private static float carrierSpeedX = 0f;

	private static float CARRIERSPEEDX = 20f;

	private static float CARRIERSPEEDY = 1f;

	private static float CARRIERTURNSPEED = 10f;

	private static float carrierMinLimitX = 390f;

	private static float carrierMaxLimitX = 620f;

	private static Vector2 currentCarrierPositionOffset = new Vector2(0f, 0f);

	private static Vector2 carrierLiftPosition = new Vector2(0f, 0f);

	private static Vector2 carrierLiftCoverAPosition = new Vector2(0f, 0f);

	private static Vector2 carrierLiftCoverBPosition = new Vector2(0f, 0f);

	private static Vector2 carrierLiftPositionOffset = new Vector2(267f, 2f);

	private static bool carrierLiftActive = false;

	private static bool carrierLiftInitialised = false;

	private static bool carrierLiftGoingUp = false;

	private static bool carrierLiftGoingDown = false;

	private static float CARRIERLIFTSPEEDY = 100f;

	private static float CARRIERLIFTLOWLIMITY = 55f;

	private static float carrierLiftLoweredOffset = 0f;

	private static float carrierLiftWaitTimer = 0f;

	private static float carrierRadarTimer = 0f;

	private static float carrierRadarSpeed = 10f;

	private static Rectangle carrierRadarRect = new Rectangle(0, 0, 0, 0);

	private static float[] searchlightRotation = new float[4];

	private static float[] searchlightCounter = new float[4];

	private static bool frontSearchLightsOn = false;

	private static float frontSearchLightTimer = 0f;

	private static float FRONTSEARCHLIGHTTIMERLIMIT = 25f;

	private static float windTimer = 0f;

	private static bool windHangarFull = false;

	private static bool windTentFull = false;

	private static Texture2D background1Texture;

	private static Texture2D background2Texture;

	private static Texture2D background3Texture;

	private static Texture2D background4Texture;

	private static Texture2D background5Texture;

	private static Texture2D background10Texture;

	private static Texture2D elementsTexture;

	private static Texture2D miniWaveTexture;

	public static void LoadContent()
	{
		background1Texture = TextureManager.GetBackground1Texture();
		background2Texture = TextureManager.GetBackground2Texture();
		background3Texture = TextureManager.GetBackground3Texture();
		background4Texture = TextureManager.GetBackground4Texture();
		background5Texture = TextureManager.GetBackground5Texture();
		background10Texture = TextureManager.GetBackground10Texture();
		elementsTexture = TextureManager.GetElementsTexture();
		miniWaveTexture = TextureManager.GetMiniWaveTexture();
	}

	public static void DrawLevel(SpriteBatch theSpriteBatch, int i)
	{
		Vector2 origin = new Vector2(0f, 0f);
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			backgroundPosition = new Vector2(0f, 0f);
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			backgroundPosition = new Vector2(0f, -33f);
		}
		else
		{
			backgroundPosition = new Vector2(0f, -65f);
		}
		switch (i)
		{
		case 1:
			theSpriteBatch.Draw(background1Texture, backgroundPosition, null, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
			break;
		case 2:
			theSpriteBatch.Draw(background2Texture, backgroundPosition, null, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
			break;
		case 3:
			theSpriteBatch.Draw(background3Texture, backgroundPosition, null, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(765, 310, 323, 18), texture: elementsTexture, position: new Vector2(hangarPosition.X - 360f, hangarPosition.Y + 89f), color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			break;
		case 4:
			theSpriteBatch.Draw(background4Texture, backgroundPosition, null, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(origin: new Vector2(33f, 375f), sourceRectangle: new Rectangle(709, 0, 60, 385), texture: elementsTexture, position: new Vector2(backgroundPosition.X + 350f, backgroundPosition.Y + 600f), color: new Color(255f, 255f, 255f, 0.5f), rotation: searchlightRotation[0], scale: 2f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(origin: new Vector2(33f, 375f), sourceRectangle: new Rectangle(709, 0, 60, 385), texture: elementsTexture, position: new Vector2(backgroundPosition.X + 700f, backgroundPosition.Y + 600f), color: new Color(255f, 255f, 255f, 0.5f), rotation: searchlightRotation[1], scale: 2f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(0, 425, 991, 165), texture: elementsTexture, position: new Vector2(backgroundPosition.X, backgroundPosition.Y + 391f), color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			if (frontSearchLightsOn)
			{
				theSpriteBatch.Draw(origin: new Vector2(33f, 375f), sourceRectangle: new Rectangle(709, 0, 60, 385), texture: elementsTexture, position: new Vector2(backgroundPosition.X + 250f, backgroundPosition.Y + 650f), color: new Color(255f, 255f, 255f, 0.5f), rotation: searchlightRotation[2], scale: 4f, effects: SpriteEffects.None, layerDepth: 0f);
				if (frontSearchLightTimer > 0.1f)
				{
					theSpriteBatch.Draw(origin: new Vector2(33f, 375f), sourceRectangle: new Rectangle(709, 0, 60, 385), texture: elementsTexture, position: new Vector2(backgroundPosition.X + 800f, backgroundPosition.Y + 650f), color: new Color(255f, 255f, 255f, 0.5f), rotation: searchlightRotation[3], scale: 4f, effects: SpriteEffects.None, layerDepth: 0f);
				}
			}
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(765, 310, 323, 18), texture: elementsTexture, position: new Vector2(hangarPosition.X - 250f, hangarPosition.Y + 87f), color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			break;
		case 5:
			theSpriteBatch.Draw(background5Texture, backgroundPosition, null, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
			break;
		case 10:
			theSpriteBatch.Draw(background10Texture, backgroundPosition, null, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
			break;
		}
	}

	public static void DrawLevelElementsDepth0(SpriteBatch theSpriteBatch, GameTime theGameTime, int theLevel)
	{
		if (theLevel == 1)
		{
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(152, 22, 17, 59), texture: elementsTexture, position: new Vector2(hangarPosition.X + 227f, hangarPosition.Y), color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (theLevel == 2)
		{
			Rectangle value = new Rectangle(10, 206, 535, 122);
			theSpriteBatch.Draw(origin: new Vector2(value.Width / 2, value.Height / 2), texture: elementsTexture, position: carrierPosition, sourceRectangle: value, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			value = new Rectangle(370, 119, 114, 20);
			theSpriteBatch.Draw(origin: new Vector2(value.Width / 2, value.Height / 2), texture: elementsTexture, position: carrierLiftPosition, sourceRectangle: value, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(origin: new Vector2(value.Width / 2, value.Height / 2), texture: elementsTexture, position: new Vector2(carrierPosition.X + 6f, carrierPosition.Y - 104f), sourceRectangle: carrierRadarRect, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			g.GetHangar().Draw(theSpriteBatch);
			g.GetTower().Draw(theSpriteBatch);
		}
		if (theLevel == 3)
		{
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(791, 225, 17, 58), texture: elementsTexture, position: new Vector2(hangarPosition.X, hangarPosition.Y + 5f), color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			DrawHeavyCloudsBack(theSpriteBatch);
		}
		if (theLevel == 4)
		{
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(791, 225, 17, 58), texture: elementsTexture, position: new Vector2(hangarPosition.X - 1f, hangarPosition.Y + 5f), color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (theLevel == 5)
		{
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(152, 22, 17, 59), texture: elementsTexture, position: new Vector2(hangarPosition.X + 255f, hangarPosition.Y), color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (theLevel == 10)
		{
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(152, 22, 17, 59), texture: elementsTexture, position: new Vector2(hangarPosition.X + 255f, hangarPosition.Y), color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		}
	}

	public static void DrawLevelElementsDepth1(SpriteBatch theSpriteBatch, GameTime theGameTime, int theLevel)
	{
		if (theLevel == 1)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				hangarPosition = new Vector2(28f, 583f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				hangarPosition = new Vector2(58f, 550f);
			}
			else
			{
				hangarPosition = new Vector2(85f, 518f);
			}
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(27, 108, 159, 76), texture: elementsTexture, position: hangarPosition, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			Vector2 vector = ((g.theSafeArea.GetScreenMode() == 0) ? new Vector2(831f, 427f) : ((g.theSafeArea.GetScreenMode() != 1) ? new Vector2(761f, 362f) : new Vector2(795f, 394f)));
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(216, 5, 90, 183), texture: elementsTexture, position: vector, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			vector = new Vector2(vector.X + 50f, vector.Y - 1f);
			Vector2 origin3 = new Vector2(0f, 0f);
			Rectangle value = new Rectangle(102, 33, 10, 32);
			if (airfieldAnimCounter > 2.5f)
			{
				if (!lightOn)
				{
					lightOn = true;
					g.theSoundManager.StartElectricSound();
				}
				else
				{
					lightOn = false;
					g.theSoundManager.StopElectricSound();
				}
				airfieldAnimCounter = 0f;
			}
			if (lightOn)
			{
				theSpriteBatch.Draw(elementsTexture, vector, value, Color.White, 0f, origin3, 1.5f, SpriteEffects.None, 0f);
			}
		}
		if (theLevel == 2)
		{
			Rectangle value = new Rectangle(336, 119, 22, 22);
			theSpriteBatch.Draw(origin: new Vector2(value.Width / 2, value.Height / 2), texture: elementsTexture, position: carrierLiftCoverBPosition, sourceRectangle: value, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (theLevel == 3)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				hangarPosition = new Vector2(700f, 578f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				hangarPosition = new Vector2(670f, 545f);
			}
			else
			{
				hangarPosition = new Vector2(640f, 513f);
			}
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(1030, 221, 170, 82), texture: elementsTexture, position: hangarPosition, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(position: (g.theSafeArea.GetScreenMode() == 0) ? new Vector2(0f, 600f) : ((g.theSafeArea.GetScreenMode() != 1) ? new Vector2(60f, 535f) : new Vector2(30f, 567f)), origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(1031, 65, 106, 67), texture: elementsTexture, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (theLevel == 4)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				hangarPosition = new Vector2(700f, 583f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				hangarPosition = new Vector2(670f, 550f);
			}
			else
			{
				hangarPosition = new Vector2(640f, 518f);
			}
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(840, 224, 170, 86), texture: elementsTexture, position: hangarPosition, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			Vector2 vector = ((g.theSafeArea.GetScreenMode() == 0) ? new Vector2(20f, 391f) : ((g.theSafeArea.GetScreenMode() != 1) ? new Vector2(80f, 326f) : new Vector2(50f, 358f)));
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(827, 0, 178, 205), texture: elementsTexture, position: vector, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			vector = new Vector2(vector.X + 178f, vector.Y + 18f);
			Vector2 origin3 = new Vector2(0f, 0f);
			Rectangle value = new Rectangle(102, 33, 10, 32);
			if (airfieldAnimCounter > 2.5f)
			{
				if (!lightOn)
				{
					lightOn = true;
					g.theSoundManager.StartElectricSound();
				}
				else
				{
					lightOn = false;
					g.theSoundManager.StopElectricSound();
				}
				airfieldAnimCounter = 0f;
			}
			if (lightOn)
			{
				theSpriteBatch.Draw(elementsTexture, vector, value, Color.White, 0f, origin3, 1.5f, SpriteEffects.None, 0f);
			}
		}
		if (theLevel == 5)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				hangarPosition = new Vector2(8f, 583f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				hangarPosition = new Vector2(38f, 550f);
			}
			else
			{
				hangarPosition = new Vector2(65f, 518f);
			}
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: (!windHangarFull) ? new Rectangle(198, 602, 180, 80) : new Rectangle(5, 602, 180, 80), texture: elementsTexture, position: hangarPosition, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(position: (g.theSafeArea.GetScreenMode() == 0) ? new Vector2(731f, 615f) : ((g.theSafeArea.GetScreenMode() != 1) ? new Vector2(661f, 550f) : new Vector2(695f, 582f)), origin: new Vector2(0f, 0f), sourceRectangle: (!windTentFull) ? new Rectangle(542, 611, 158, 62) : new Rectangle(387, 611, 158, 62), texture: elementsTexture, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (theLevel == 10)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				hangarPosition = new Vector2(8f, 583f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				hangarPosition = new Vector2(38f, 550f);
			}
			else
			{
				hangarPosition = new Vector2(65f, 518f);
			}
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), sourceRectangle: new Rectangle(772, 364, 180, 80), texture: elementsTexture, position: hangarPosition, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
			theDropPlane.Draw(theSpriteBatch);
		}
		if (theLevel != 2)
		{
			g.GetHangar().Draw(theSpriteBatch);
			g.GetTower().Draw(theSpriteBatch);
		}
	}

	public static void DrawLevelElementsDepth2(SpriteBatch theSpriteBatch, GameTime theGameTime, int theLevel)
	{
		if (theLevel == 2)
		{
			Rectangle value = new Rectangle(370, 142, 116, 39);
			theSpriteBatch.Draw(origin: new Vector2(value.Width / 2, value.Height / 2), texture: elementsTexture, position: carrierLiftCoverAPosition, sourceRectangle: value, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		}
	}

	public static void DrawLevelElementsDepth3(SpriteBatch theSpriteBatch, GameTime theGameTime, int theLevel)
	{
	}

	public static void DrawLevelElementsDepth4(SpriteBatch theSpriteBatch, GameTime theGameTime, int theLevel)
	{
		if (theLevel != 3)
		{
			DrawClouds(theSpriteBatch);
		}
		if (theLevel == 3)
		{
			DrawSnows(theSpriteBatch);
			DrawHeavyCloudsFront(theSpriteBatch);
		}
		if (theLevel == 2)
		{
			DrawWaves(theSpriteBatch);
			DrawMiniWaves(theSpriteBatch);
		}
		if (theLevel == 5)
		{
			DrawHeavyCloudsBack(theSpriteBatch);
			DrawHeavyCloudsFront(theSpriteBatch);
		}
	}

	public static void LoadWaves()
	{
		for (int i = 0; i < waves.Length; i++)
		{
			if (waves[i] != null)
			{
				waves[i].LoadContent(elementsTexture);
			}
		}
	}

	public static void LoadMiniWaves()
	{
		for (int i = 0; i < miniWaves.Length; i++)
		{
			if (miniWaves[i] != null)
			{
				miniWaves[i].LoadContent(miniWaveTexture);
			}
		}
	}

	public static void CreateDropPlane(GameWorld gw)
	{
		g = gw;
		theDropPlane = new DropPlane(g);
		theDropPlane.LoadContent();
	}

	public static void CreateClouds()
	{
		clouds[0] = new Cloud(300f, 150f, 10f, 0.75f);
		clouds[1] = new Cloud(100f, 200f, 20f, 0.75f);
		clouds[2] = new Cloud(700f, 300f, 30f, 0.75f);
	}

	public static void CreateHeavyCloudsBack()
	{
		for (int i = 0; i < heavyCloudsBack.Length; i++)
		{
			heavyCloudsBack[i] = new Cloud(0f, 0f, 0f, 0.75f);
		}
	}

	public static void CreateHeavyCloudsFront()
	{
		for (int i = 0; i < heavyCloudsFront.Length; i++)
		{
			heavyCloudsFront[i] = new Cloud(0f, 0f, 0f, 0.75f);
		}
	}

	public static void CreateSnows()
	{
		for (int i = 0; i < snowsFront.Length; i++)
		{
			snowsFront[i] = new Snow(0f, i * 130, 30f, 1.5f, Color.White, f: false);
		}
		for (int i = 0; i < snowsBack.Length; i++)
		{
			snowsBack[i] = new Snow(-30f, i * 130, 20f, 1.5f, Color.White, f: true);
		}
	}

	public static void CreateWaves()
	{
		waves[0] = new Wave(200f, 676f, 40f, 0f, g);
		waves[1] = new Wave(220f, 676f, 40f, 0.1f, g);
		waves[2] = new Wave(240f, 676f, 40f, 0.2f, g);
		waves[3] = new Wave(260f, 676f, 40f, 0.3f, g);
		waves[4] = new Wave(280f, 676f, 40f, 0.4f, g);
		waves[5] = new Wave(300f, 676f, 40f, 0.5f, g);
		waves[6] = new Wave(320f, 676f, 40f, 0.6f, g);
		waves[7] = new Wave(340f, 676f, 40f, 0.5f, g);
		waves[8] = new Wave(360f, 676f, 40f, 0.4f, g);
		waves[9] = new Wave(380f, 676f, 40f, 0.3f, g);
		waves[10] = new Wave(400f, 676f, 40f, 0.2f, g);
		waves[11] = new Wave(420f, 676f, 40f, 0.1f, g);
		LoadWaves();
	}

	public static void CreateMiniWaves()
	{
		miniWaves[0] = new MiniWave(2000f, 670f, 30f, 1.5f, 2f);
		miniWaves[1] = new MiniWave(2000f, 680f, 35f, 1.5f, 2.2f);
		miniWaves[2] = new MiniWave(2000f, 690f, 40f, 1.5f, 2.4f);
		LoadMiniWaves();
	}

	public static void DrawClouds(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < clouds.Length; i++)
		{
			if (clouds[i] != null)
			{
				clouds[i].Draw(theSpriteBatch);
			}
		}
	}

	public static void DrawHeavyCloudsBack(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < heavyCloudsBack.Length; i++)
		{
			if (heavyCloudsBack[i] != null)
			{
				heavyCloudsBack[i].Draw(theSpriteBatch);
			}
		}
	}

	public static void DrawHeavyCloudsFront(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < heavyCloudsFront.Length; i++)
		{
			if (heavyCloudsFront[i] != null)
			{
				heavyCloudsFront[i].Draw(theSpriteBatch);
			}
		}
	}

	public static void DrawSnows(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < snowsFront.Length; i++)
		{
			if (snowsFront[i] != null)
			{
				snowsFront[i].Draw(theSpriteBatch);
			}
		}
		for (int i = 0; i < snowsBack.Length; i++)
		{
			if (snowsBack[i] != null)
			{
				snowsBack[i].Draw(theSpriteBatch);
			}
		}
	}

	public static void DrawWaves(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < waves.Length; i++)
		{
			if (waves[i] != null)
			{
				waves[i].Draw(theSpriteBatch);
			}
		}
	}

	public static void DrawMiniWaves(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < miniWaves.Length; i++)
		{
			if (miniWaves[i] != null)
			{
				miniWaves[i].Draw(theSpriteBatch);
			}
		}
	}

	public static void Update(GameTime theGameTime, int c)
	{
		UpdateEnvironment(theGameTime, c);
		UpdateAnimCounters(theGameTime);
		UpdateClouds(theGameTime);
		UpdateSnows(theGameTime);
		UpdateMiniWaves(theGameTime);
		UpdateHeavyCloudsBack(theGameTime, c);
		UpdateHeavyCloudsFront(theGameTime, c);
	}

	public static void UpdateEnvironment(GameTime theGameTime, int c)
	{
		if (c != 5)
		{
			return;
		}
		windTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (!(windTimer >= 0.02f))
		{
			return;
		}
		if (General.GetNextRandom(0, 10) == 0)
		{
			if (!windHangarFull)
			{
				windHangarFull = true;
			}
			else
			{
				windHangarFull = false;
			}
		}
		if (General.GetNextRandom(0, 10) == 0)
		{
			if (!windTentFull)
			{
				windTentFull = true;
			}
			else
			{
				windTentFull = false;
			}
		}
		windTimer = 0f;
	}

	public static void SetLevel(int c)
	{
		if (c == 1)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				takeOffY = 671f;
				pilotGroundY = 675f;
				groundY = 700f;
				absoluteGroundY = 700f;
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				takeOffY = 638f;
				pilotGroundY = 642f;
				groundY = 660f;
				absoluteGroundY = 660f;
			}
			else
			{
				takeOffY = 606f;
				pilotGroundY = 610f;
				groundY = 628f;
				absoluteGroundY = 628f;
			}
			landingMinX = -2000f;
			landingMaxX = 2000f;
		}
		if (c == 2)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				absoluteGroundY = 675f;
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				absoluteGroundY = 642f;
			}
			else
			{
				absoluteGroundY = 610f;
			}
			groundY = carrierPosition.Y;
			takeOffY = carrierPosition.Y;
			pilotGroundY = currentCarrierPositionOffset.Y - 31f;
			landingMinX = GetCarrierPosition().X - 330f;
			landingMaxX = GetCarrierPosition().X + 330f;
		}
		if (c == 3)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				takeOffY = 672f;
				pilotGroundY = 675f;
				groundY = 700f;
				absoluteGroundY = 700f;
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				takeOffY = 639f;
				pilotGroundY = 642f;
				groundY = 660f;
				absoluteGroundY = 660f;
			}
			else
			{
				takeOffY = 607f;
				pilotGroundY = 610f;
				groundY = 628f;
				absoluteGroundY = 628f;
			}
			landingMinX = -2000f;
			landingMaxX = 2000f;
		}
		if (c == 4)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				takeOffY = 675f;
				pilotGroundY = 675f;
				groundY = 700f;
				absoluteGroundY = 700f;
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				takeOffY = 642f;
				pilotGroundY = 642f;
				groundY = 660f;
				absoluteGroundY = 660f;
			}
			else
			{
				takeOffY = 610f;
				pilotGroundY = 610f;
				groundY = 628f;
				absoluteGroundY = 628f;
			}
			landingMinX = -2000f;
			landingMaxX = 2000f;
		}
		if (c == 5)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				takeOffY = 672f;
				pilotGroundY = 675f;
				groundY = 700f;
				absoluteGroundY = 700f;
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				takeOffY = 639f;
				pilotGroundY = 642f;
				groundY = 660f;
				absoluteGroundY = 660f;
			}
			else
			{
				takeOffY = 607f;
				pilotGroundY = 610f;
				groundY = 628f;
				absoluteGroundY = 628f;
			}
			landingMinX = -2000f;
			landingMaxX = 2000f;
		}
		if (c == 10)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				takeOffY = 675f;
				pilotGroundY = 675f;
				groundY = 685f;
				absoluteGroundY = 685f;
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				takeOffY = 642f;
				pilotGroundY = 642f;
				groundY = 652f;
				absoluteGroundY = 652f;
			}
			else
			{
				takeOffY = 610f;
				pilotGroundY = 610f;
				groundY = 620f;
				absoluteGroundY = 620f;
			}
			landingMinX = -2000f;
			landingMaxX = 2000f;
			theDropPlane.SetActive(b: true);
		}
	}

	public static void UpdateLevel(GameTime theGameTime, int c)
	{
		if (c == 2)
		{
			groundY = carrierPosition.Y;
			takeOffY = carrierPosition.Y;
			pilotGroundY = currentCarrierPositionOffset.Y - 31f;
			UpdateCarrier(theGameTime);
			UpdateCarrierLift(theGameTime);
			UpdateCarrierRadar(theGameTime);
			UpdateWaves(theGameTime);
			landingMinX = GetCarrierPosition().X - 330f;
			landingMaxX = GetCarrierPosition().X + 330f;
		}
		if (c == 4)
		{
			UpdateSearchlights(theGameTime);
		}
		if (c == 10)
		{
			theDropPlane.SetActive(b: true);
			UpdateDropPlane(theGameTime);
		}
	}

	public static void UpdateAnimCounters(GameTime theGameTime)
	{
		airfieldAnimCounter += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		carrierAnimCounter += (float)theGameTime.ElapsedGameTime.TotalSeconds;
	}

	public static void UpdateClouds(GameTime theGameTime)
	{
		for (int i = 0; i < clouds.Length; i++)
		{
			if (clouds[i] != null)
			{
				clouds[i].Update(theGameTime);
			}
		}
	}

	public static void UpdateHeavyCloudsBack(GameTime theGameTime, int c)
	{
		for (int i = 0; i < heavyCloudsBack.Length; i++)
		{
			if (heavyCloudsBack[i] != null)
			{
				if (c == 3)
				{
					heavyCloudsBack[i].SetColor(Color.White, 1f);
				}
				else
				{
					heavyCloudsBack[i].SetColor(new Color(221, 178, 127), 0.4f);
				}
				heavyCloudsBack[i].Update(theGameTime);
			}
		}
	}

	public static void UpdateHeavyCloudsFront(GameTime theGameTime, int c)
	{
		for (int i = 0; i < heavyCloudsFront.Length; i++)
		{
			if (heavyCloudsFront[i] != null)
			{
				if (c == 3)
				{
					heavyCloudsFront[i].SetColor(Color.White, 1f);
				}
				else
				{
					heavyCloudsFront[i].SetColor(new Color(221, 178, 127), 0.4f);
				}
				heavyCloudsFront[i].Update(theGameTime);
			}
		}
	}

	public static void UpdateSnows(GameTime theGameTime)
	{
		for (int i = 0; i < snowsFront.Length; i++)
		{
			if (snowsFront[i] != null)
			{
				snowsFront[i].Update(theGameTime);
			}
		}
		for (int i = 0; i < snowsBack.Length; i++)
		{
			if (snowsBack[i] != null)
			{
				snowsBack[i].Update(theGameTime);
			}
		}
	}

	public static void UpdateMiniWaves(GameTime theGameTime)
	{
		float num = absoluteGroundY + 20f;
		miniWaves[0].SetMiniWaveY(num - 10f);
		miniWaves[1].SetMiniWaveY(num);
		miniWaves[2].SetMiniWaveY(num + 10f);
		for (int i = 0; i < miniWaves.Length; i++)
		{
			miniWaves[i].Update(theGameTime);
		}
	}

	public static void UpdateDropPlane(GameTime theGameTime)
	{
		theDropPlane.Update(theGameTime);
	}

	public static void UpdateWaves(GameTime theGameTime)
	{
		for (int i = 0; i < waves.Length; i++)
		{
			if (waves[i] != null)
			{
				waves[i].Update(theGameTime);
			}
		}
	}

	public static void UpdateCarrier(GameTime theGameTime)
	{
		if (carrierPosition.X >= carrierMaxLimitX)
		{
			carrierLeft = true;
		}
		if (carrierPosition.X <= carrierMinLimitX)
		{
			carrierLeft = false;
		}
		if (carrierLeft)
		{
			if (carrierSpeedX > 0f - CARRIERSPEEDX)
			{
				carrierSpeedX -= CARRIERTURNSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
		}
		else if (carrierSpeedX < CARRIERSPEEDX)
		{
			carrierSpeedX += CARRIERTURNSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		waveSinValue += CARRIERSPEEDY * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (waveSinValue >= (float)Math.PI * 2f)
		{
			waveSinValue = 0f;
		}
		currentCarrierPositionOffset.X = carrierSpeedX * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			carrierMinLimitX = 380f;
			carrierMaxLimitX = 590f;
			currentCarrierPositionOffset.Y = CARRIERHEIGHT + (float)((double)WAVEHEIGHT * Math.Sin(waveSinValue));
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			carrierMinLimitX = 413f;
			carrierMaxLimitX = 557f;
			currentCarrierPositionOffset.Y = CARRIERHEIGHT + (float)((double)WAVEHEIGHT * Math.Sin(waveSinValue)) - 33f;
		}
		else
		{
			carrierMinLimitX = 436f;
			carrierMaxLimitX = 520f;
			currentCarrierPositionOffset.Y = CARRIERHEIGHT + (float)((double)WAVEHEIGHT * Math.Sin(waveSinValue)) - 66f;
		}
		carrierPosition.X += currentCarrierPositionOffset.X;
		carrierPosition.Y = currentCarrierPositionOffset.Y;
	}

	public static void UpdateCarrierLift(GameTime theGameTime)
	{
		carrierLiftPosition.X = carrierPosition.X + carrierLiftPositionOffset.X;
		carrierLiftPosition.Y = carrierPosition.Y + carrierLiftPositionOffset.Y + carrierLiftLoweredOffset;
		carrierLiftCoverAPosition.X = carrierPosition.X + carrierLiftPositionOffset.X;
		carrierLiftCoverAPosition.Y = carrierPosition.Y + carrierLiftPositionOffset.Y + 40f;
		carrierLiftCoverBPosition.X = carrierPosition.X + carrierLiftPositionOffset.X - 71f;
		carrierLiftCoverBPosition.Y = carrierPosition.Y + carrierLiftPositionOffset.Y + 1f;
		if (!carrierLiftActive)
		{
			return;
		}
		if (!carrierLiftInitialised)
		{
			carrierLiftGoingDown = true;
			carrierLiftGoingUp = false;
			carrierLiftWaitTimer = 0.25f;
			carrierLiftInitialised = true;
			g.theSoundManager.CarrierLiftSound(0.7f);
		}
		if (carrierLiftGoingDown)
		{
			carrierLiftLoweredOffset += CARRIERLIFTSPEEDY * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (carrierLiftLoweredOffset >= CARRIERLIFTLOWLIMITY)
			{
				carrierLiftLoweredOffset = CARRIERLIFTLOWLIMITY;
				carrierLiftWaitTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (carrierLiftWaitTimer <= 0f)
			{
				carrierLiftWaitTimer = 0f;
				carrierLiftGoingDown = false;
				carrierLiftGoingUp = true;
				g.theSoundManager.CarrierLiftSound(0.3f);
			}
		}
		if (carrierLiftGoingUp)
		{
			carrierLiftLoweredOffset -= CARRIERLIFTSPEEDY * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (carrierLiftLoweredOffset <= 0f)
			{
				carrierLiftLoweredOffset = 0f;
				carrierLiftGoingDown = false;
				carrierLiftGoingUp = false;
				carrierLiftInitialised = false;
				carrierLiftActive = false;
			}
		}
	}

	public static void UpdateCarrierRadar(GameTime theGameTime)
	{
		carrierRadarTimer += carrierRadarSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (carrierRadarTimer > 12f)
		{
			carrierRadarTimer -= 12f;
		}
		carrierRadarRect = new Rectangle(326 + (int)carrierRadarTimer % 6 * 51, 6 + (int)carrierRadarTimer / 6 * 34, 50, 34);
	}

	public static void UpdateSearchlights(GameTime theGameTime)
	{
		for (int i = 0; i < searchlightRotation.Length; i++)
		{
			searchlightCounter[i] += (0.25f + (float)i * 0.1f) * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			searchlightRotation[i] = (float)Math.Sin(searchlightCounter[i]) / (float)(2 + i);
		}
		if (frontSearchLightsOn)
		{
			frontSearchLightTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (frontSearchLightTimer >= FRONTSEARCHLIGHTTIMERLIMIT)
			{
				frontSearchLightsOn = false;
				frontSearchLightTimer = 0f;
			}
		}
	}

	public static void ResetLevels(int c)
	{
		theDropPlane.Reset();
		SetLevel(c);
		SetClouds();
		SetHeavyClouds(c);
		frontSearchLightTimer = 0f;
		frontSearchLightsOn = false;
	}

	public static void SetHeavyClouds(int c)
	{
		if (c == 3)
		{
			SetHeavyCloudsSnow();
		}
		if (c == 5)
		{
			SetHeavyCloudsSand();
		}
	}

	public static void SetClouds()
	{
		for (int i = 0; i < heavyCloudsFront.Length; i++)
		{
			heavyCloudsFront[i].SetActive(b: false);
		}
		for (int i = 0; i < heavyCloudsBack.Length; i++)
		{
			heavyCloudsBack[i].SetActive(b: false);
		}
		if (g.GetCurrentLevel() == 1)
		{
			clouds[0].SetUp(b: true, 10f, 0.75f);
			clouds[1].SetUp(b: true, 20f, 0.75f);
			clouds[2].SetUp(b: true, 30f, 0.75f);
		}
		if (g.GetCurrentLevel() == 2)
		{
			clouds[0].SetUp(b: false, 20f, 0.75f);
			clouds[1].SetUp(b: false, 30f, 0.75f);
			clouds[2].SetUp(b: false, 40f, 0.75f);
		}
		if (g.GetCurrentLevel() == 3)
		{
			clouds[0].SetUp(b: false, 20f, 0.75f);
			clouds[1].SetUp(b: false, 30f, 0.75f);
			clouds[2].SetUp(b: false, 40f, 0.75f);
		}
		if (g.GetCurrentLevel() == 4)
		{
			clouds[0].SetUp(b: true, 10f, 0.75f);
			clouds[1].SetUp(b: true, 20f, 0.75f);
			clouds[2].SetUp(b: true, 30f, 0.75f);
		}
		if (g.GetCurrentLevel() == 5)
		{
			clouds[0].SetUp(b: false, 10f, 0.75f);
			clouds[1].SetUp(b: true, 20f, 0.5f);
			clouds[2].SetUp(b: false, 30f, 0.75f);
		}
		if (g.GetCurrentLevel() == 10)
		{
			clouds[0].SetUp(b: false, 10f, 0.75f);
			clouds[1].SetUp(b: true, 20f, 0.5f);
			clouds[2].SetUp(b: false, 30f, 0.75f);
		}
		for (int j = 0; j < clouds.Length; j++)
		{
			clouds[j].SetRandomPosition();
		}
	}

	public static void SetHeavyCloudsSnow()
	{
		float num = g.theSafeArea.GetStallCeiling() + 0f;
		heavyCloudsBack[0].SetHeavyCloud(-300f, num, 2.5f);
		heavyCloudsBack[1].SetHeavyCloud(-50f, num, 2.5f);
		heavyCloudsBack[2].SetHeavyCloud(200f, num, 2.5f);
		heavyCloudsBack[3].SetHeavyCloud(450f, num, 2.5f);
		heavyCloudsBack[4].SetHeavyCloud(700f, num, 2.5f);
		heavyCloudsBack[5].SetHeavyCloud(950f, num, 2.5f);
		heavyCloudsBack[6].SetHeavyCloud(1200f, num, 2.5f);
		heavyCloudsBack[10].SetHeavyCloud(-175f, num + 20f, 2.5f);
		heavyCloudsBack[8].SetHeavyCloud(75f, num + 20f, 2.5f);
		heavyCloudsBack[9].SetHeavyCloud(325f, num + 20f, 2.5f);
		heavyCloudsBack[10].SetHeavyCloud(575f, num + 20f, 2.5f);
		heavyCloudsBack[11].SetHeavyCloud(825f, num + 20f, 2.5f);
		heavyCloudsBack[12].SetHeavyCloud(1075f, num + 20f, 2.5f);
		heavyCloudsBack[13].SetHeavyCloud(1325f, num + 20f, 2.5f);
		heavyCloudsFront[0].SetHeavyCloud(-200f, num + 10f, 3f);
		heavyCloudsFront[1].SetHeavyCloud(250f, num - 2f, 4f);
		heavyCloudsFront[2].SetHeavyCloud(600f, num + 7f, 5f);
		heavyCloudsFront[3].SetHeavyCloud(1000f, num + 20f, 6f);
		heavyCloudsFront[4].SetHeavyCloud(1400f, num + 15f, 7f);
		heavyCloudsFront[5].SetHeavyCloud(750f, num - 15f, 8f);
	}

	public static void SetHeavyCloudsSand()
	{
		float num = absoluteGroundY + 0f;
		heavyCloudsBack[0].SetHeavyCloud(-300f, num, 20f);
		heavyCloudsBack[1].SetHeavyCloud(-50f, num, 20f);
		heavyCloudsBack[2].SetHeavyCloud(200f, num, 20f);
		heavyCloudsBack[3].SetHeavyCloud(450f, num, 20f);
		heavyCloudsBack[4].SetHeavyCloud(700f, num, 20f);
		heavyCloudsBack[5].SetHeavyCloud(950f, num, 20f);
		heavyCloudsBack[6].SetHeavyCloud(1200f, num, 20f);
		heavyCloudsBack[7].SetHeavyCloud(-200f, num + 30f, 20f);
		heavyCloudsBack[8].SetHeavyCloud(50f, num + 30f, 20f);
		heavyCloudsBack[9].SetHeavyCloud(300f, num + 30f, 20f);
		heavyCloudsBack[10].SetHeavyCloud(550f, num + 30f, 20f);
		heavyCloudsBack[11].SetHeavyCloud(800f, num + 30f, 20f);
		heavyCloudsBack[12].SetHeavyCloud(1050f, num + 30f, 20f);
		heavyCloudsBack[13].SetHeavyCloud(1300f, num + 30f, 20f);
		heavyCloudsFront[0].SetHeavyCloud(-200f, num + 42f, 24f);
		heavyCloudsFront[1].SetHeavyCloud(200f, num - 12f, 32f);
		heavyCloudsFront[2].SetHeavyCloud(600f, num - 7f, 40f);
		heavyCloudsFront[3].SetHeavyCloud(1000f, num + 30f, 48f);
		heavyCloudsFront[4].SetHeavyCloud(1400f, num + 25f, 56f);
		heavyCloudsFront[5].SetHeavyCloud(750f, num - 25f, 64f);
	}

	public static void SetCarrierLiftActive(bool b)
	{
		carrierLiftActive = b;
	}

	public static bool GetCarrierLiftActive()
	{
		return carrierLiftActive;
	}

	public static bool GetCarrierLiftGoingUp()
	{
		return carrierLiftGoingUp;
	}

	public static Cloud GetCloud(int i)
	{
		return clouds[i];
	}

	public static Cloud GetHeavyCloudBack(int i)
	{
		return heavyCloudsBack[i];
	}

	public static Cloud GetHeavyCloudFront(int i)
	{
		return heavyCloudsFront[i];
	}

	public static Snow GetSnowFront(int i)
	{
		return snowsFront[i];
	}

	public static Snow GetSnowBack(int i)
	{
		return snowsBack[i];
	}

	public static Cloud[] GetClouds()
	{
		return clouds;
	}

	public static Cloud[] GetHeavyCloudsBack()
	{
		return heavyCloudsBack;
	}

	public static Cloud[] GetHeavyCloudsFront()
	{
		return heavyCloudsFront;
	}

	public static Snow[] GetSnowsFront()
	{
		return snowsFront;
	}

	public static Snow[] GetSnowsBack()
	{
		return snowsBack;
	}

	public static DropPlane GetDropPlane()
	{
		return theDropPlane;
	}

	public static Vector2 GetHangarPosition()
	{
		return hangarPosition;
	}

	public static Vector2 GetCarrierPosition()
	{
		return carrierPosition;
	}

	public static Vector2 GetCarrierLiftPosition()
	{
		return carrierLiftPosition;
	}

	public static Vector2 GetCurrentCarrierPositionOffset()
	{
		return currentCarrierPositionOffset;
	}

	public static float GetTakeOffY()
	{
		return takeOffY;
	}

	public static float GetPilotGroundY()
	{
		return pilotGroundY;
	}

	public static float GetGroundY()
	{
		return groundY;
	}

	public static float GetAbsoluteGroundY()
	{
		return absoluteGroundY;
	}

	public static float GetLandingMinX()
	{
		return landingMinX;
	}

	public static float GetLandingMaxX()
	{
		return landingMaxX;
	}

	public static void SetFrontSearchLightsOn(bool b)
	{
		frontSearchLightsOn = b;
		g.theSoundManager.StartSirenCloseSound();
	}

	public static bool GetFrontSearchLightsOn()
	{
		return frontSearchLightsOn;
	}

	public static void ResetCarrierLift()
	{
		carrierLiftPosition = new Vector2(0f, 0f);
		carrierLiftCoverAPosition = new Vector2(0f, 0f);
		carrierLiftCoverBPosition = new Vector2(0f, 0f);
		carrierLiftPositionOffset = new Vector2(267f, 2f);
		carrierLiftActive = false;
		carrierLiftInitialised = false;
		carrierLiftGoingUp = false;
		carrierLiftGoingDown = false;
		carrierLiftLoweredOffset = 0f;
		carrierLiftWaitTimer = 0f;
	}
}
