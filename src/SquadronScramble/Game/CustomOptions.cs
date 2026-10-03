using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public static class CustomOptions
{
	private static float gameSpeed = 0.8f;

	private static bool debugOn = false;

	public static int DEFAULTTROPHYLIMIT = 3;

	public static float DEFAULTTIMELIMIT = 2f;

	private static int DEFAULTSQUADRON0SECTIONSIZE = 4;

	private static int DEFAULTSQUADRON1SECTIONSIZE = 4;

	private static int DEFAULTSQUADRON2SECTIONSIZE = 4;

	private static int DEFAULTSQUADRON3SECTIONSIZE = 4;

	private static int DEFAULTBULLETLIMIT = 3;

	private static bool DEFAULTFRIENDLYFIREENABLED = false;

	private static bool DEFAULTLEVEL1ON = true;

	private static bool DEFAULTLEVEL2ON = true;

	private static bool DEFAULTLEVEL3ON = true;

	private static bool DEFAULTLEVEL4ON = true;

	private static bool DEFAULTLEVEL5ON = true;

	public static int SPECIALTROPHYLIMIT = 3;

	public static float SPECIALTIMELIMIT = 1f;

	private static int specialSquadron0SectionSize = 4;

	private static int specialSquadron1SectionSize = 4;

	private static int specialSquadron2SectionSize = 4;

	private static int specialSquadron3SectionSize = 4;

	private static int SPECIALBULLETLIMIT = 3;

	private static bool SPECIALFRIENDLYFIREENABLED = false;

	private static bool SPECIALLEVEL1ON = true;

	private static bool SPECIALLEVEL2ON = true;

	private static bool SPECIALLEVEL3ON = true;

	private static bool SPECIALLEVEL4ON = true;

	private static bool SPECIALLEVEL5ON = true;

	public static int SPECIAL2TROPHYLIMIT = 1;

	public static float SPECIAL2TIMELIMIT = 5f;

	private static int special2Squadron0SectionSize = 4;

	private static int special2Squadron1SectionSize = 4;

	private static int special2Squadron2SectionSize = 4;

	private static int special2Squadron3SectionSize = 4;

	private static int SPECIAL2BULLETLIMIT = 1;

	private static bool SPECIAL2FRIENDLYFIREENABLED = false;

	private static bool SPECIAL2LEVEL1ON = true;

	private static bool SPECIAL2LEVEL2ON = true;

	private static bool SPECIAL2LEVEL3ON = true;

	private static bool SPECIAL2LEVEL4ON = true;

	private static bool SPECIAL2LEVEL5ON = true;

	public static int customTrophyLimit = 2;

	public static float customTimeLimit = 1f;

	private static int customSquadron0SectionSize = 1;

	private static int customSquadron1SectionSize = 2;

	private static int customSquadron2SectionSize = 3;

	private static int customSquadron3SectionSize = 4;

	private static int customBulletLimit = 1;

	private static bool customFriendlyFireEnabled = false;

	private static bool customLevel1On = true;

	private static bool customLevel2On = true;

	private static bool customLevel3On = true;

	private static bool customLevel4On = true;

	private static bool customLevel5On = true;

	public static int currentCupOption = 2;

	public static int currentTimeOption = 2;

	public static int currentBulletOption = 2;

	public static int currentFriendlyFireOption = 1;

	public static int currentWhitePilotOption = 3;

	public static int currentYellowPilotOption = 3;

	public static int currentGreenPilotOption = 3;

	public static int currentBluePilotOption = 3;

	public static bool currentLevel1On = true;

	public static bool currentLevel2On = true;

	public static bool currentLevel3On = true;

	public static bool currentLevel4On = true;

	public static bool currentLevel5On = true;

	private static int squadron0SectionSize = 0;

	private static int squadron1SectionSize = 0;

	private static int squadron2SectionSize = 0;

	private static int squadron3SectionSize = 0;

	private static int bulletLimit = 0;

	private static bool friendlyFireEnabled = false;

	public static float timeLimit = 2f;

	public static int trophyLimit = 0;

	public static bool level1On = true;

	public static bool level2On = true;

	public static bool level3On = true;

	public static bool level4On = true;

	public static bool level5On = true;

	private static bool manualBailEnabled = false;

	private static Texture2D backgroundTexture;

	public static void LoadContent(ContentManager theContentManager)
	{
		backgroundTexture = TextureManager.GetScreenControllerSelectTexture();
	}

	public static void UseNormalGameOptions()
	{
		squadron0SectionSize = DEFAULTSQUADRON0SECTIONSIZE;
		squadron1SectionSize = DEFAULTSQUADRON1SECTIONSIZE;
		squadron2SectionSize = DEFAULTSQUADRON2SECTIONSIZE;
		squadron3SectionSize = DEFAULTSQUADRON3SECTIONSIZE;
		bulletLimit = DEFAULTBULLETLIMIT;
		friendlyFireEnabled = DEFAULTFRIENDLYFIREENABLED;
		timeLimit = DEFAULTTIMELIMIT;
		trophyLimit = DEFAULTTROPHYLIMIT;
		level1On = DEFAULTLEVEL1ON;
		level2On = DEFAULTLEVEL2ON;
		level3On = DEFAULTLEVEL3ON;
		level4On = DEFAULTLEVEL4ON;
		level5On = DEFAULTLEVEL5ON;
	}

	public static void UseSpecialGameOptions()
	{
		squadron0SectionSize = specialSquadron0SectionSize;
		squadron1SectionSize = specialSquadron1SectionSize;
		squadron2SectionSize = specialSquadron2SectionSize;
		squadron3SectionSize = specialSquadron3SectionSize;
		bulletLimit = SPECIALBULLETLIMIT;
		friendlyFireEnabled = SPECIALFRIENDLYFIREENABLED;
		timeLimit = SPECIALTIMELIMIT;
		trophyLimit = SPECIALTROPHYLIMIT;
		level1On = SPECIALLEVEL1ON;
		level2On = SPECIALLEVEL2ON;
		level3On = SPECIALLEVEL3ON;
		level4On = SPECIALLEVEL4ON;
		level5On = SPECIALLEVEL5ON;
	}

	public static void UseSpecialGame2Options()
	{
		squadron0SectionSize = special2Squadron0SectionSize;
		squadron1SectionSize = special2Squadron1SectionSize;
		squadron2SectionSize = special2Squadron2SectionSize;
		squadron3SectionSize = special2Squadron3SectionSize;
		bulletLimit = SPECIAL2BULLETLIMIT;
		friendlyFireEnabled = SPECIAL2FRIENDLYFIREENABLED;
		timeLimit = SPECIAL2TIMELIMIT;
		trophyLimit = SPECIAL2TROPHYLIMIT;
		level1On = SPECIAL2LEVEL1ON;
		level2On = SPECIAL2LEVEL2ON;
		level3On = SPECIAL2LEVEL3ON;
		level4On = SPECIAL2LEVEL4ON;
		level5On = SPECIAL2LEVEL5ON;
	}

	public static void UseCustomGameOptions()
	{
		squadron0SectionSize = customSquadron0SectionSize;
		squadron1SectionSize = customSquadron1SectionSize;
		squadron2SectionSize = customSquadron2SectionSize;
		squadron3SectionSize = customSquadron3SectionSize;
		bulletLimit = customBulletLimit;
		friendlyFireEnabled = customFriendlyFireEnabled;
		timeLimit = customTimeLimit;
		trophyLimit = customTrophyLimit;
		level1On = customLevel1On;
		level2On = customLevel2On;
		level3On = customLevel3On;
		level4On = customLevel4On;
		level5On = customLevel5On;
	}

	public static void ResetCustomGameOptions()
	{
		customSquadron0SectionSize = DEFAULTSQUADRON0SECTIONSIZE;
		customSquadron1SectionSize = DEFAULTSQUADRON1SECTIONSIZE;
		customSquadron2SectionSize = DEFAULTSQUADRON2SECTIONSIZE;
		customSquadron3SectionSize = DEFAULTSQUADRON3SECTIONSIZE;
		customBulletLimit = DEFAULTBULLETLIMIT;
		customFriendlyFireEnabled = DEFAULTFRIENDLYFIREENABLED;
		customTimeLimit = DEFAULTTIMELIMIT;
		customTrophyLimit = DEFAULTTROPHYLIMIT;
		customLevel1On = DEFAULTLEVEL1ON;
		customLevel2On = DEFAULTLEVEL2ON;
		customLevel3On = DEFAULTLEVEL3ON;
		customLevel4On = DEFAULTLEVEL4ON;
		customLevel5On = DEFAULTLEVEL5ON;
	}

	public static int GetSquadronSectionSize(int i)
	{
		return i switch
		{
			0 => squadron0SectionSize, 
			1 => squadron1SectionSize, 
			2 => squadron2SectionSize, 
			_ => squadron3SectionSize, 
		};
	}

	public static int GetBulletLimit()
	{
		return bulletLimit;
	}

	public static void SetBulletLimit(int i)
	{
		bulletLimit = i;
	}

	public static void SetCustomBulletLimit(int i)
	{
		customBulletLimit = i;
	}

	public static bool GetFriendlyFireEnabled()
	{
		return friendlyFireEnabled;
	}

	public static void SetFriendlyFireEnabled(bool b)
	{
		friendlyFireEnabled = b;
	}

	public static void SetCustomFriendlyFireEnabled(bool b)
	{
		customFriendlyFireEnabled = b;
	}

	public static float GetTimeLimit()
	{
		return timeLimit;
	}

	public static void SetTimeLimit(float f)
	{
		timeLimit = f;
	}

	public static void SetCustomTimeLimit(float f)
	{
		customTimeLimit = f;
	}

	public static int GetTrophyLimit()
	{
		return trophyLimit;
	}

	public static void SetTrophyLimit(int i)
	{
		trophyLimit = i;
	}

	public static void SetCustomTrophyLimit(int i)
	{
		customTrophyLimit = i;
	}

	public static void SetCustomLevelOn(int i, bool b)
	{
		switch (i)
		{
		case 0:
			customLevel1On = b;
			break;
		case 1:
			customLevel2On = b;
			break;
		case 2:
			customLevel3On = b;
			break;
		case 3:
			customLevel4On = b;
			break;
		case 4:
			customLevel5On = b;
			break;
		}
	}

	public static bool GetLevelOn(int i)
	{
		return i switch
		{
			1 => level1On, 
			2 => level2On, 
			3 => level3On, 
			4 => level4On, 
			_ => level5On, 
		};
	}

	public static void SetSpecialSquadronSectionSizes(int i, int j, int k, int l)
	{
		specialSquadron0SectionSize = i;
		specialSquadron1SectionSize = j;
		specialSquadron2SectionSize = k;
		specialSquadron3SectionSize = l;
	}

	public static void SetCustomSquadronSectionSizes(int i, int j, int k, int l)
	{
		customSquadron0SectionSize = i;
		customSquadron1SectionSize = j;
		customSquadron2SectionSize = k;
		customSquadron3SectionSize = l;
	}

	public static bool GetManualBailEnabled()
	{
		return manualBailEnabled;
	}

	public static void SetManualBailEnabled(bool b)
	{
		manualBailEnabled = b;
	}

	public static float GetGameSpeed()
	{
		return gameSpeed;
	}

	public static void SetGameSpeed(float f)
	{
		gameSpeed = f;
	}

	public static bool GetDebugOn()
	{
		return debugOn;
	}

	public static void SetDebugOn(bool b)
	{
		debugOn = b;
	}
}
