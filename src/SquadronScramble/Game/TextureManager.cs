using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public static class TextureManager
{
	private static Texture2D background1Texture;

	private static Texture2D background2Texture;

	private static Texture2D background3Texture;

	private static Texture2D background4Texture;

	private static Texture2D background5Texture;

	private static Texture2D background10Texture;

	private static Texture2D elementsControlOverlayTexture;

	private static Texture2D elementsTexture;

	private static Texture2D elementsFrontEndTexture;

	private static Texture2D elementsOptionsOverlayTexture;

	private static Texture2D elementsRoundResultTexture;

	private static Texture2D screenRoundResultTexture;

	private static Texture2D screenWinningSquadronTexture;

	private static Texture2D screenControlDetailsTexture;

	private static Texture2D screenControllerSelectTexture;

	private static Texture2D screenCustomOptionsBackgroundTexture;

	private static Texture2D screenFadeOverlayTexture;

	private static Texture2D subCarrierTexture;

	private static Texture2D subDesertTexture;

	private static Texture2D subSnowTexture;

	private static Texture2D bombTexture;

	private static Texture2D bulletTexture;

	private static Texture2D cloudTexture;

	private static Texture2D doorTexture;

	private static Texture2D dropPlaneTexture;

	private static Texture2D dustCloudTexture;

	private static Texture2D explosionTexture;

	private static Texture2D flamesTexture;

	private static Texture2D groundCrashTexture;

	private static Texture2D groundSplashTexture;

	private static Texture2D ignitionTexture;

	private static Texture2D impactTexture;

	private static Texture2D miniWaveTexture;

	private static Texture2D missileTexture;

	private static Texture2D pilotTexture;

	private static Texture2D planeTexture;

	private static Texture2D planeOutlineTexture;

	private static Texture2D smokeTexture;

	private static Texture2D snowTexture;

	private static Texture2D snowCloudTexture;

	private static Texture2D specialsTexture;

	private static Texture2D splashTexture;

	private static Texture2D wingTexture;

	public static void LoadContent(ContentManager theContentManager)
	{
		background1Texture = theContentManager.Load<Texture2D>("Environment1GradientFull");
		background2Texture = theContentManager.Load<Texture2D>("Environment2Smooth");
		background3Texture = theContentManager.Load<Texture2D>("Environment3");
		background4Texture = theContentManager.Load<Texture2D>("Environment4");
		background5Texture = theContentManager.Load<Texture2D>("Environment10");
		background10Texture = theContentManager.Load<Texture2D>("Environment10");
		elementsControlOverlayTexture = theContentManager.Load<Texture2D>("ElementsControlOverlay");
		elementsTexture = theContentManager.Load<Texture2D>("EnvironmentElementsSmoothFull");
		elementsFrontEndTexture = theContentManager.Load<Texture2D>("ElementsFrontEnd");
		elementsOptionsOverlayTexture = theContentManager.Load<Texture2D>("ElementsOptionsOverlay");
		elementsRoundResultTexture = theContentManager.Load<Texture2D>("ElementsRoundResult");
		screenRoundResultTexture = theContentManager.Load<Texture2D>("ScreenRoundResult");
		screenWinningSquadronTexture = theContentManager.Load<Texture2D>("ScreenWinningSquadron");
		screenControlDetailsTexture = theContentManager.Load<Texture2D>("ScreenControlDetails");
		screenControllerSelectTexture = theContentManager.Load<Texture2D>("HangarScreenBlurred");
		screenCustomOptionsBackgroundTexture = theContentManager.Load<Texture2D>("ScreenCustomOptions");
		screenFadeOverlayTexture = theContentManager.Load<Texture2D>("ScreenFadeOverlay");
		subCarrierTexture = theContentManager.Load<Texture2D>("SubCarrier");
		subDesertTexture = theContentManager.Load<Texture2D>("SubDesert");
		subSnowTexture = theContentManager.Load<Texture2D>("SubSnow");
		bombTexture = theContentManager.Load<Texture2D>("Bomb");
		bulletTexture = theContentManager.Load<Texture2D>("Bullet");
		doorTexture = theContentManager.Load<Texture2D>("Door");
		cloudTexture = theContentManager.Load<Texture2D>("Clouds");
		dropPlaneTexture = theContentManager.Load<Texture2D>("DropPlane");
		dustCloudTexture = theContentManager.Load<Texture2D>("DustCloud");
		explosionTexture = theContentManager.Load<Texture2D>("Explosion");
		flamesTexture = theContentManager.Load<Texture2D>("Flames_New");
		groundCrashTexture = theContentManager.Load<Texture2D>("GroundCrash");
		groundSplashTexture = theContentManager.Load<Texture2D>("GroundSplash");
		ignitionTexture = theContentManager.Load<Texture2D>("Ignition");
		impactTexture = theContentManager.Load<Texture2D>("Impact");
		miniWaveTexture = theContentManager.Load<Texture2D>("MiniWave");
		missileTexture = theContentManager.Load<Texture2D>("Missile");
		pilotTexture = theContentManager.Load<Texture2D>("PilotHiRes");
		planeTexture = theContentManager.Load<Texture2D>("PlaneNoCockpit");
		planeOutlineTexture = theContentManager.Load<Texture2D>("PlaneOutline");
		smokeTexture = theContentManager.Load<Texture2D>("Smoke");
		snowTexture = theContentManager.Load<Texture2D>("Snow");
		snowCloudTexture = theContentManager.Load<Texture2D>("SnowCloud");
		specialsTexture = theContentManager.Load<Texture2D>("Specials");
		splashTexture = theContentManager.Load<Texture2D>("Splash");
		wingTexture = theContentManager.Load<Texture2D>("Wing");
	}

	public static Texture2D GetBackground1Texture()
	{
		return background1Texture;
	}

	public static Texture2D GetBackground2Texture()
	{
		return background2Texture;
	}

	public static Texture2D GetBackground3Texture()
	{
		return background3Texture;
	}

	public static Texture2D GetBackground4Texture()
	{
		return background4Texture;
	}

	public static Texture2D GetBackground5Texture()
	{
		return background5Texture;
	}

	public static Texture2D GetBackground10Texture()
	{
		return background10Texture;
	}

	public static Texture2D GetElementsControlOverlayTexture()
	{
		return elementsControlOverlayTexture;
	}

	public static Texture2D GetElementsTexture()
	{
		return elementsTexture;
	}

	public static Texture2D GetElementsFrontEndTexture()
	{
		return elementsFrontEndTexture;
	}

	public static Texture2D GetElementsOptionsOverlayTexture()
	{
		return elementsOptionsOverlayTexture;
	}

	public static Texture2D GetElementsRoundResultTexture()
	{
		return elementsRoundResultTexture;
	}

	public static Texture2D GetScreenRoundResultTexture()
	{
		return screenRoundResultTexture;
	}

	public static Texture2D GetScreenWinningSquadronTexture()
	{
		return screenWinningSquadronTexture;
	}

	public static Texture2D GetScreenControlDetailsTexture()
	{
		return screenControlDetailsTexture;
	}

	public static Texture2D GetScreenControllerSelectTexture()
	{
		return screenControllerSelectTexture;
	}

	public static Texture2D GetScreenCustomOptionsBackgroundTexture()
	{
		return screenCustomOptionsBackgroundTexture;
	}

	public static Texture2D GetScreenFadeOverlayTexture()
	{
		return screenFadeOverlayTexture;
	}

	public static Texture2D GetSubCarrierTexture()
	{
		return subCarrierTexture;
	}

	public static Texture2D GetSubDesertTexture()
	{
		return subDesertTexture;
	}

	public static Texture2D GetSubSnowTexture()
	{
		return subSnowTexture;
	}

	public static Texture2D GetBombTexture()
	{
		return bombTexture;
	}

	public static Texture2D GetBulletTexture()
	{
		return bulletTexture;
	}

	public static Texture2D GetDoorTexture()
	{
		return doorTexture;
	}

	public static Texture2D GetCloudTexture()
	{
		return cloudTexture;
	}

	public static Texture2D GetDropPlaneTexture()
	{
		return dropPlaneTexture;
	}

	public static Texture2D GetDustCloudTexture()
	{
		return dustCloudTexture;
	}

	public static Texture2D GetExplosionTexture()
	{
		return explosionTexture;
	}

	public static Texture2D GetFlamesTexture()
	{
		return flamesTexture;
	}

	public static Texture2D GetIgnitionTexture()
	{
		return ignitionTexture;
	}

	public static Texture2D GetImpactTexture()
	{
		return impactTexture;
	}

	public static Texture2D GetMiniWaveTexture()
	{
		return miniWaveTexture;
	}

	public static Texture2D GetMissileTexture()
	{
		return missileTexture;
	}

	public static Texture2D GetGroundCrashTexture()
	{
		return groundCrashTexture;
	}

	public static Texture2D GetGroundSplashTexture()
	{
		return groundSplashTexture;
	}

	public static Texture2D GetPilotTexture()
	{
		return pilotTexture;
	}

	public static Texture2D GetPlaneTexture()
	{
		return planeTexture;
	}

	public static Texture2D GetPlaneOutlineTexture()
	{
		return planeOutlineTexture;
	}

	public static Texture2D GetSmokeTexture()
	{
		return smokeTexture;
	}

	public static Texture2D GetSnowTexture()
	{
		return snowTexture;
	}

	public static Texture2D GetSnowCloudTexture()
	{
		return snowCloudTexture;
	}

	public static Texture2D GetSpecialsTexture()
	{
		return specialsTexture;
	}

	public static Texture2D GetSplashTexture()
	{
		return splashTexture;
	}

	public static Texture2D GetWingTexture()
	{
		return wingTexture;
	}
}
