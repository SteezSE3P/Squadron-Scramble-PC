using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class GameWorld
{
	public enum State
	{
		frontEnd,
		editSquadronScreen,
		controllerSelectionScreen,
		AISelectionScreen,
		inGame,
		roundResultScreen,
		winningSquadronScreen,
		topPilotsScreen
	}

	public Dogfight d;

	public ContentManager directLinkToContentManager;

	private int NUMBEROFCONTROLLERS = 4;

	private int currentMenuController = 0;

	public FrontEnd theFrontEnd;

	public CollisionManager theCollisionManager;

	public FontManager theFontManager;

	public PauseManager thePauseManager;

	public RoundManager theRoundManager;

	public SoundManager theSoundManager;

	private bool[] activeController;

	public ControllerMenuManager[] theControllerMenuManager;

	public ControllerVibrationManager[] theControllerVibrationManager;

	public EditSquadronScreen theEditSquadronScreen;

	public ControllerSelectScreen theControllerSelectScreen;

	public AISelectScreen theAISelectScreen;

	public RoundResultScreen theRoundResultScreen;

	public TopPilotsScreen theTopPilotsScreen;

	public WinningSquadronScreen theWinningSquadronScreen;

	public ScreenFadeOverlay theScreenFadeOverlay;

	public ScreenControlDetails theScreenControlDetails;

	public OptionsOverlay theOptionsOverlay;

	public InGameScreenText theInGameScreenText;

	public SafeArea theSafeArea;

	private int currentLevel;

	public NamePlate theNamePlate;

	public State currentState = State.frontEnd;

	public SquadronNames NameList;

	public Slot[] theSlots = new Slot[4];

	public Slot[] activeSlots = new Slot[4];

	public Squadron[] squadrons = new Squadron[8];

	public Pilot[] pilots = new Pilot[8];

	public Player[] players = new Player[8];

	public AIPlayer[] AIplayers = new AIPlayer[8];

	public Plane[] planes = new Plane[16];

	public SpecialBonus theSpecialBonus;

	private HangarDoor hangar;

	private TowerDoor tower;

	public GameWorld(Dogfight df)
	{
		d = df;
		activeController = new bool[NUMBEROFCONTROLLERS];
		theControllerMenuManager = new ControllerMenuManager[NUMBEROFCONTROLLERS];
		theControllerVibrationManager = new ControllerVibrationManager[NUMBEROFCONTROLLERS];
		theFrontEnd = new FrontEnd(this);
		theNamePlate = new NamePlate(this);
		theCollisionManager = new CollisionManager(this);
		theFontManager = new FontManager(this);
		thePauseManager = new PauseManager(this);
		theRoundManager = new RoundManager(this, NUMBEROFCONTROLLERS);
		theSoundManager = new SoundManager(this);
		theControllerMenuManager[0] = new ControllerMenuManager(this, PlayerIndex.One);
		theControllerMenuManager[1] = new ControllerMenuManager(this, PlayerIndex.Two);
		theControllerMenuManager[2] = new ControllerMenuManager(this, PlayerIndex.Three);
		theControllerMenuManager[3] = new ControllerMenuManager(this, PlayerIndex.Four);
		theControllerVibrationManager[0] = new ControllerVibrationManager(this, PlayerIndex.One);
		theControllerVibrationManager[1] = new ControllerVibrationManager(this, PlayerIndex.Two);
		theControllerVibrationManager[2] = new ControllerVibrationManager(this, PlayerIndex.Three);
		theControllerVibrationManager[3] = new ControllerVibrationManager(this, PlayerIndex.Four);
		theSafeArea = new SafeArea(this);
		theInGameScreenText = new InGameScreenText(this);
		theScreenFadeOverlay = new ScreenFadeOverlay(this);
		theScreenControlDetails = new ScreenControlDetails(this);
		theOptionsOverlay = new OptionsOverlay(this);
		theEditSquadronScreen = new EditSquadronScreen(this);
		theControllerSelectScreen = new ControllerSelectScreen(this);
		theAISelectScreen = new AISelectScreen(this);
		theRoundResultScreen = new RoundResultScreen(this);
		RoundResultScenery.SetRoundResultScenery(this);
		theTopPilotsScreen = new TopPilotsScreen(this);
		theWinningSquadronScreen = new WinningSquadronScreen(this);
		theSpecialBonus = new SpecialBonus(this);
		ControlOverlay.SetGameWorld(this);
		currentLevel = 0;
		NameList = new SquadronNames(this);
		for (int i = 0; i < 8; i++)
		{
			players[i] = new Player(this);
			AIplayers[i] = new AIPlayer(this);
		}
		hangar = new HangarDoor(this);
		tower = new TowerDoor(this);
	}

	public void LoadContent(ContentManager theContentManager)
	{
		TextureManager.LoadContent(theContentManager);
		theFontManager.LoadContent(theContentManager);
		theSoundManager.LoadContent(theContentManager);
		theFrontEnd.LoadContent(theContentManager);
		theEditSquadronScreen.LoadContent(theContentManager);
		theControllerSelectScreen.LoadContent(theContentManager);
		theAISelectScreen.LoadContent(theContentManager);
		thePauseManager.LoadContent(theContentManager);
		theInGameScreenText.LoadContent();
		theRoundResultScreen.LoadContent(theContentManager);
		RoundResultScenery.LoadContent(theContentManager);
		theWinningSquadronScreen.LoadContent();
		theTopPilotsScreen.LoadContent(theContentManager);
		theSafeArea.LoadContent(theContentManager);
		Level.LoadContent();
		Level.CreateDropPlane(this);
		Level.CreateClouds();
		Level.CreateHeavyCloudsBack();
		Level.CreateHeavyCloudsFront();
		Level.CreateSnows();
		Level.CreateWaves();
		Level.CreateMiniWaves();
		ControlOverlay.LoadContent(theContentManager);
		for (int i = 0; i < Level.GetClouds().Length; i++)
		{
			Level.GetCloud(i).LoadContent(0);
		}
		for (int i = 0; i < Level.GetHeavyCloudsBack().Length; i++)
		{
			Level.GetHeavyCloudBack(i).LoadContent(1);
		}
		for (int i = 0; i < Level.GetHeavyCloudsFront().Length; i++)
		{
			Level.GetHeavyCloudFront(i).LoadContent(1);
		}
		for (int i = 0; i < Level.GetSnowsFront().Length; i++)
		{
			Level.GetSnowFront(i).LoadContent();
		}
		for (int i = 0; i < Level.GetSnowsBack().Length; i++)
		{
			Level.GetSnowBack(i).LoadContent();
		}
		hangar.LoadContent();
		tower.LoadContent();
		theNamePlate.LoadContent(theContentManager);
		theScreenFadeOverlay.LoadContent(theContentManager);
		theScreenControlDetails.LoadContent();
		theOptionsOverlay.LoadContent(theContentManager);
		theSpecialBonus.LoadContent();
		SetDirectLinkToContentManager(theContentManager);
		squadrons[0] = new Squadron(this, 0);
		squadrons[1] = new Squadron(this, 1);
		squadrons[2] = new Squadron(this, 2);
		squadrons[3] = new Squadron(this, 3);
		squadrons[4] = new Squadron(this, 4);
		squadrons[5] = new Squadron(this, 5);
		squadrons[6] = new Squadron(this, 6);
		squadrons[7] = new Squadron(this, 7);
		theSlots[0] = new Slot(squadrons[0], new Color(255, 255, 255), new Color(111, 111, 111), new Color(167, 167, 167), new Color(206, 206, 206), "WHITE", "WHITE SQUADRON");
		theSlots[1] = new Slot(squadrons[1], new Color(250, 250, 0), new Color(190, 190, 0), new Color(162, 119, 40), new Color(228, 150, 15), "YELLOW", "YELLOW SQUADRON");
		theSlots[2] = new Slot(squadrons[2], new Color(100, 255, 0), new Color(0, 138, 28), new Color(72, 181, 0), new Color(140, 200, 130), "GREEN", "GREEN SQUADRON");
		theSlots[3] = new Slot(squadrons[3], new Color(100, 173, 255), new Color(55, 55, 255), new Color(57, 230, 233), new Color(133, 125, 221), "BLUE", "BLUE SQUADRON");
	}

	public void LoadRoundContent()
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			pilots[i].LoadContent();
		}
		for (int i = 0; i < planes.Length; i++)
		{
			planes[i].LoadContent();
		}
		theSpecialBonus.LoadContent();
	}

	public void Update(GameTime theGameTime)
	{
		theSafeArea.Update(theGameTime);
		theSoundManager.Update(theGameTime);
		theOptionsOverlay.Update(theGameTime);
		General.Update(theGameTime);
		for (int i = 0; i < NUMBEROFCONTROLLERS; i++)
		{
			theControllerMenuManager[i].Update(theGameTime);
			theControllerVibrationManager[i].Update(theGameTime, activeController[i]);
		}
		if (!thePauseManager.GetGamePaused())
		{
			if (currentState == State.frontEnd)
			{
				theFrontEnd.Update(theGameTime);
			}
			else if (currentState == State.editSquadronScreen)
			{
				theEditSquadronScreen.Update(theGameTime);
			}
			else if (currentState == State.controllerSelectionScreen)
			{
				theControllerSelectScreen.Update(theGameTime);
				theInGameScreenText.Update(theGameTime);
			}
			else if (currentState == State.AISelectionScreen)
			{
				theAISelectScreen.Update(theGameTime);
				theInGameScreenText.Update(theGameTime);
			}
			else if (currentState == State.inGame)
			{
				theInGameScreenText.UpdateRoundText(theGameTime);
				theNamePlate.Update(theGameTime);
				if (!theRoundManager.GetRoundIsOver() && !theRoundManager.GetRoundIsPaused())
				{
					Level.Update(theGameTime, currentLevel);
					theRoundManager.Update(theGameTime);
					for (int i = 0; i < pilots.Length; i++)
					{
						pilots[i].Update(theGameTime, currentLevel);
					}
					for (int i = 0; i < planes.Length; i++)
					{
						planes[i].Update(theGameTime, currentLevel);
					}
					Level.UpdateLevel(theGameTime, currentLevel);
					tower.Update(theGameTime, currentLevel);
					hangar.Update(theGameTime, currentLevel);
					theCollisionManager.Update(theGameTime);
					theInGameScreenText.Update(theGameTime);
					theSpecialBonus.Update(theGameTime);
				}
			}
			else if (currentState == State.roundResultScreen)
			{
				theRoundResultScreen.Update(theGameTime);
			}
			else if (currentState == State.winningSquadronScreen)
			{
				theWinningSquadronScreen.Update(theGameTime);
			}
			else if (currentState == State.topPilotsScreen)
			{
				theTopPilotsScreen.Update(theGameTime);
			}
		}
		if ((currentState != State.inGame && !thePauseManager.GetGamePaused()) || !theRoundManager.GetRoundIsPlayable() || !theRoundManager.GetRoundIsOver())
		{
		}
		theScreenFadeOverlay.Update(theGameTime);
		thePauseManager.Update(theGameTime);
	}

	public void Draw(GameTime theGameTime, SpriteBatch theSpriteBatch)
	{
		if (currentState == State.frontEnd)
		{
			theFrontEnd.Draw(theSpriteBatch);
		}
		else if (currentState == State.editSquadronScreen)
		{
			theEditSquadronScreen.Draw(theSpriteBatch);
		}
		else if (currentState == State.controllerSelectionScreen)
		{
			theControllerSelectScreen.Draw(theSpriteBatch);
		}
		else if (currentState == State.AISelectionScreen)
		{
			theAISelectScreen.Draw(theSpriteBatch);
		}
		else if (currentState == State.inGame)
		{
			Level.DrawLevel(theSpriteBatch, currentLevel);
			if (currentLevel == 2)
			{
				DrawOffShipPilots(theSpriteBatch);
			}
			DrawShadows(theSpriteBatch);
			theSpecialBonus.DrawDepth0(theSpriteBatch);
			Level.DrawLevelElementsDepth0(theSpriteBatch, theGameTime, currentLevel);
			if (currentLevel == 1 || currentLevel == 3 || currentLevel == 4 || currentLevel == 5 || currentLevel == 10)
			{
				DrawPlanesTakingOff(theSpriteBatch);
			}
			if (currentLevel == 2)
			{
				DrawOnShipPilots(theSpriteBatch);
			}
			theSpecialBonus.DrawDepth1(theSpriteBatch);
			Level.DrawLevelElementsDepth1(theSpriteBatch, theGameTime, currentLevel);
			if (currentLevel == 2)
			{
				DrawPlanes(theSpriteBatch);
			}
			theSpecialBonus.DrawDepth2(theSpriteBatch);
			Level.DrawLevelElementsDepth2(theSpriteBatch, theGameTime, currentLevel);
			if (currentLevel == 1 || currentLevel == 3 || currentLevel == 4 || currentLevel == 5 || currentLevel == 10)
			{
				DrawPilots(theSpriteBatch);
			}
			DrawBullets(theSpriteBatch);
			theSpecialBonus.DrawDepth3(theSpriteBatch);
			Level.DrawLevelElementsDepth3(theSpriteBatch, theGameTime, currentLevel);
			if (currentLevel == 1 || currentLevel == 2 || currentLevel == 3 || currentLevel == 4 || currentLevel == 5 || currentLevel == 10)
			{
				DrawPlanesFlying(theSpriteBatch);
			}
			DrawExplosions(theSpriteBatch);
			Level.DrawLevelElementsDepth4(theSpriteBatch, theGameTime, currentLevel);
			theSpecialBonus.DrawDepth4(theSpriteBatch);
			DrawPlaneOutlines(theSpriteBatch);
			theInGameScreenText.DrawScreenScores(theSpriteBatch);
			theInGameScreenText.DrawScreenNames(theSpriteBatch);
			DrawTutorialText(theSpriteBatch);
			DrawFullGameText(theSpriteBatch);
			DrawNamePlate(theSpriteBatch);
			theInGameScreenText.DrawRound(theSpriteBatch);
		}
		else if (currentState == State.roundResultScreen)
		{
			theRoundResultScreen.Draw(theSpriteBatch);
		}
		else if (currentState == State.winningSquadronScreen)
		{
			theWinningSquadronScreen.Draw(theSpriteBatch);
		}
		else if (currentState == State.topPilotsScreen)
		{
			theTopPilotsScreen.Draw(theSpriteBatch);
		}
		theScreenFadeOverlay.Draw(theSpriteBatch);
		thePauseManager.Draw(theSpriteBatch);
		if (theFrontEnd.GetLoaded())
		{
			theSafeArea.Draw(theSpriteBatch);
		}
	}

	public void DrawTutorialText(SpriteBatch theSpriteBatch)
	{
		theInGameScreenText.DrawTutorialText(theSpriteBatch);
	}

	public void DrawFullGameText(SpriteBatch theSpriteBatch)
	{
		theInGameScreenText.DrawFullGameText(theSpriteBatch);
	}

	public void DrawNamePlate(SpriteBatch theSpriteBatch)
	{
		theNamePlate.Draw(theSpriteBatch);
		theInGameScreenText.DrawText(theSpriteBatch);
	}

	public void RoundOver()
	{
		theRoundManager.SetRoundIsOver(b: true);
		theInGameScreenText.InitialiseRoundOverText();
		theInGameScreenText.SetRoundTextOn(b: true);
	}

	public void SetDirectLinkToContentManager(ContentManager theContentManager)
	{
		directLinkToContentManager = theContentManager;
	}

	public void DrawBullets(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			planes[i].DrawBullets(theSpriteBatch);
		}
	}

	public void DrawExplosions(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			planes[i].DrawExplosions(theSpriteBatch);
		}
	}

	public void DrawShadows(SpriteBatch theSpriteBatch)
	{
		theSpecialBonus.DrawShadow(theSpriteBatch);
		if (currentLevel != 2)
		{
			for (int i = 0; i < planes.Length; i++)
			{
				planes[i].DrawShadow(theSpriteBatch);
			}
			for (int i = 0; i < pilots.Length; i++)
			{
				pilots[i].DrawShadow(theSpriteBatch);
			}
		}
	}

	public void DrawPlanes(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			planes[i].Draw(theSpriteBatch);
		}
	}

	public void DrawPlaneOutlines(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			planes[i].DrawOutline(theSpriteBatch);
		}
	}

	public void DrawPlanesTakingOff(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i].GetTakingOff())
			{
				planes[i].Draw(theSpriteBatch);
			}
		}
	}

	public void DrawPlanesFlying(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (!planes[i].GetTakingOff())
			{
				planes[i].Draw(theSpriteBatch);
			}
		}
	}

	public void DrawPilots(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			pilots[i].Draw(theSpriteBatch);
		}
	}

	public void DrawOffShipPilots(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			if (!General.isBetween(pilots[i].GetPosition().X, Level.GetLandingMinX(), Level.GetLandingMaxX()) || pilots[i].GetPosition().Y > Level.GetPilotGroundY() + 45f)
			{
				pilots[i].Draw(theSpriteBatch);
			}
		}
	}

	public void DrawOnShipPilots(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			if (General.isBetween(pilots[i].GetPosition().X, Level.GetLandingMinX() - 1f, Level.GetLandingMaxX() + 1f) && pilots[i].GetPosition().Y < Level.GetPilotGroundY() + 45f)
			{
				pilots[i].Draw(theSpriteBatch);
			}
		}
	}

	public void ResetAll()
	{
		for (int i = 0; i < activeSlots.Length; i++)
		{
			if (activeSlots[i] != null)
			{
				activeSlots[i].FullReset();
				activeSlots[i].GetCurrentSquadron().FullReset();
				activeSlots[i] = null;
			}
		}
		theRoundManager.FullReset();
	}

	public HangarDoor GetHangar()
	{
		return hangar;
	}

	public TowerDoor GetTower()
	{
		return tower;
	}

	public int GetCurrentMenuController()
	{
		return currentMenuController;
	}

	public int GetNumberOfControllers()
	{
		return NUMBEROFCONTROLLERS;
	}

	public void SetCurrentMenuController(int i)
	{
		currentMenuController = i;
	}

	public void StartTheGame()
	{
		currentState = State.inGame;
		theSoundManager.StopTitleMusic();
		theSoundManager.StartRound();
	}

	public void LoadData()
	{
		theEditSquadronScreen.InitiateLoad();
	}

	public void SaveData()
	{
		theEditSquadronScreen.InitiateSave();
	}

	public void GoToFrontEnd()
	{
		currentState = State.frontEnd;
		theFrontEnd.SetFrontEndComplete(b: false);
	}

	public void GoToRoundResultScreen()
	{
		currentState = State.roundResultScreen;
	}

	public void GoToWinningSquadronScreen()
	{
		currentState = State.winningSquadronScreen;
	}

	public void GoToEditSquadronScreen()
	{
		currentState = State.editSquadronScreen;
		theEditSquadronScreen.UpdateNames();
	}

	public void GoToControllerSelectionScreen()
	{
		theControllerSelectScreen.ResetScreen();
		currentState = State.controllerSelectionScreen;
	}

	public void GoToAISelectionScreen()
	{
		currentState = State.AISelectionScreen;
	}

	public void GoToTopPilotsScreen()
	{
		currentState = State.topPilotsScreen;
	}

	public Dogfight GetD()
	{
		return d;
	}

	public void CheckActiveControllers(GameTime theGameTime)
	{
		for (int i = 0; i < activeController.Length; i++)
		{
			if (activeController[i] && !theControllerMenuManager[i].GetIsConnected())
			{
				thePauseManager.ControllerDisconnectionDetected();
				thePauseManager.PausePressed(theGameTime, players[i]);
			}
		}
	}

	public void SetActiveController(int i, bool b)
	{
		activeController[i] = b;
	}

	public bool GetActiveController(int i)
	{
		return activeController[i];
	}

	public int GetCurrentLevel()
	{
		return currentLevel;
	}

	public void SetCurrentLevel(int i)
	{
		currentLevel = i;
	}
}
