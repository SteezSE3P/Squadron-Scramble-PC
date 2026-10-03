using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace SquadronScramble;

public class RoundManager
{
	private GameWorld g;

	private int numberOfControllers;

	private int roundNumber = 0;

	private bool roundIsPlayable = false;

	private bool roundIsPaused = false;

	private bool roundIsOver = false;

	private bool isFinal = false;

	private bool keepSquadronMembers = true;

	public RoundManager(GameWorld gw, int i)
	{
		g = gw;
		numberOfControllers = i;
	}

	public void Update(GameTime theGameTime)
	{
		CheckSquadronsCompeting(theGameTime);
	}

	public void CheckSquadronsCompeting(GameTime theGameTime)
	{
		if (!SquadronsAreCompeting())
		{
			g.RoundOver();
		}
	}

	public void StartNewRound()
	{
		roundNumber++;
		SelectLevel();
		SquadronRoundReset();
		AIPlayerReset();
		roundIsOver = false;
		roundIsPlayable = false;
		g.theInGameScreenText.SetRoundTextOn(b: true);
		g.thePauseManager.ResetPause();
		g.theInGameScreenText.ResetClock();
		g.GetTower().SetBusy(b: false);
		g.GetHangar().SetBusy(b: false);
		if (isFinal)
		{
			g.SetCurrentLevel(10);
		}
		Level.ResetLevels(g.GetCurrentLevel());
		GC.Collect();
		for (int i = 0; i < g.planes.Length; i++)
		{
			g.planes[i] = new Plane(g);
		}
		g.theControllerSelectScreen.SetSquadrons();
		FlushPilots();
		SelectSquadronPilots();
		g.theInGameScreenText.ClearNamesAndScores();
		g.theSpecialBonus = new SpecialBonus(g);
		g.theSoundManager.StopSirenCloseSound();
		DoorsReset();
		g.LoadRoundContent();
		g.theControllerSelectScreen.SetParticipants();
		CheckIfFinal();
		g.theScreenFadeOverlay.UnFadeScreen(1f);
		g.theInGameScreenText.Change();
		g.StartTheGame();
	}

	public void FlushPilots()
	{
		SquadronMember squadronMember = null;
		for (int i = 0; i < g.pilots.Length; i++)
		{
			squadronMember = null;
			if (keepSquadronMembers)
			{
				if (g.pilots[i] != null && g.pilots[i].IsParticipating() && g.pilots[i].GetCurrentSquadronMember() != null)
				{
					if (g.pilots[i].GetCurrentSquadronMember().GetAlive())
					{
						squadronMember = g.pilots[i].GetCurrentSquadronMember();
					}
					else
					{
						g.pilots[i].GetCurrentSquadronMember().SetInAction(b: false);
					}
				}
				g.pilots[i] = new Pilot(g, i, g.planes[i], g.planes[i + 8]);
				if (squadronMember != null)
				{
					g.pilots[i].SetCurrentSquadronMember(squadronMember);
				}
			}
			else
			{
				g.pilots[i] = new Pilot(g, i, g.planes[i], g.planes[i + 8]);
				ResetSquadronPilots();
			}
		}
	}

	public void ResetSquadronPilots()
	{
		if (roundNumber != 1)
		{
			for (int i = 0; i < g.theSlots.Length; i++)
			{
				g.theSlots[i].GetCurrentSquadron().ResetActivePilots();
			}
		}
	}

	public void SelectSquadronPilots()
	{
		if (roundNumber == 1)
		{
			for (int i = 0; i < g.theSlots.Length; i++)
			{
				g.theSlots[i].GetCurrentSquadron().SelectFirstCombatants();
			}
		}
		else
		{
			for (int i = 0; i < g.theSlots.Length; i++)
			{
				g.theSlots[i].GetCurrentSquadron().SelectNewReplacements();
			}
		}
	}

	public bool SquadronsAreCompeting()
	{
		int num = 0;
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetParticipating() && !g.activeSlots[i].GetCurrentSquadron().GetAllPilotsDead())
			{
				num++;
			}
		}
		if (num > 1)
		{
			return true;
		}
		return false;
	}

	public bool CheckIfRoundWinner()
	{
		int num = -1;
		bool result = true;
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null)
			{
				if (g.activeSlots[i].GetCurrentSquadron().GetScore() > num)
				{
					num = g.activeSlots[i].GetCurrentSquadron().GetScore();
					result = true;
				}
				else if (g.activeSlots[i].GetCurrentSquadron().GetScore() == num)
				{
					result = false;
				}
			}
		}
		return result;
	}

	public int CheckTopScore()
	{
		int num = 0;
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetCurrentSquadron().GetScore() > num)
			{
				num = g.activeSlots[i].GetCurrentSquadron().GetScore();
			}
		}
		return num;
	}

	public void CheckIfFinal()
	{
		if (!isFinal)
		{
			return;
		}
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (g.pilots[i].GetCurrentSquadron() != null && g.pilots[i].GetCurrentSquadron().GetTrophies() != CustomOptions.GetTrophyLimit())
			{
				g.pilots[i].SetParticipating(b: false);
			}
			if (g.pilots[i].GetCurrentSquadron() != null && g.pilots[i].GetCurrentSquadron().GetTrophies() == CustomOptions.GetTrophyLimit() && g.pilots[i].IsParticipating() && !g.pilots[i].GetCurrentSquadron().CombatantIsInSection(g.pilots[i].GetCurrentSquadronMember()))
			{
				g.pilots[i].GetCurrentSquadronMember().SetInAction(b: false);
				g.pilots[i].SelectCurrentSquadronMember();
			}
		}
	}

	public void SquadronRoundReset()
	{
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null)
			{
				g.activeSlots[i].GetCurrentSquadron().RoundReset();
			}
		}
	}

	public void DoorsReset()
	{
		g.GetHangar().SetAnimFrame(0f);
		g.GetTower().SetAnimFrame(0f);
		Level.ResetCarrierLift();
	}

	public void AIPlayerReset()
	{
		for (int i = 0; i < g.AIplayers.Length; i++)
		{
			if (g.AIplayers[i] != null)
			{
				g.AIplayers[i].Reset();
			}
		}
	}

	public void FullReset()
	{
		roundNumber = 0;
		isFinal = false;
		for (int i = 0; i < numberOfControllers; i++)
		{
			g.SetActiveController(i, b: false);
		}
		for (int i = 0; i < g.pilots.Length; i++)
		{
			g.pilots[i].SetCurrentSquadronMember(null);
		}
		g.GetTower().Reset();
		g.theRoundResultScreen.Reset();
	}

	public bool GetIsFinal()
	{
		return isFinal;
	}

	public int GetRoundNumber()
	{
		return roundNumber;
	}

	public void SetRoundIsOver(bool b)
	{
		roundIsOver = b;
	}

	public void SetRoundIsPaused(bool b)
	{
		roundIsPaused = b;
	}

	public void SetRoundIsPlayable(bool b)
	{
		roundIsPlayable = b;
	}

	public void SetIsFinal(bool b)
	{
		isFinal = b;
	}

	public bool GetRoundIsOver()
	{
		return roundIsOver;
	}

	public bool GetRoundIsPaused()
	{
		return roundIsPaused;
	}

	public bool GetRoundIsPlayable()
	{
		return roundIsPlayable;
	}

	public void SelectLevel()
	{
		int currentLevel = g.GetCurrentLevel();
		int num = 0;
		if (CustomOptions.level1On)
		{
			num++;
		}
		if (CustomOptions.level2On)
		{
			num++;
		}
		if (CustomOptions.level3On)
		{
			num++;
		}
		if (CustomOptions.level4On)
		{
			num++;
		}
		if (CustomOptions.level5On)
		{
			num++;
		}
		if (num > 1)
		{
			do
			{
				int nextRandom = General.GetNextRandom(0, 5);
				if (roundNumber == 1)
				{
					if (nextRandom == 0 && CustomOptions.level1On)
					{
						g.SetCurrentLevel(1);
					}
					if (nextRandom == 1 && CustomOptions.level2On)
					{
						g.SetCurrentLevel(2);
					}
					if (nextRandom == 2 && CustomOptions.level3On)
					{
						g.SetCurrentLevel(3);
					}
					if (nextRandom == 3 && CustomOptions.level4On)
					{
						g.SetCurrentLevel(4);
					}
					if (nextRandom == 4 && CustomOptions.level5On)
					{
						g.SetCurrentLevel(5);
					}
				}
				if (roundNumber == 2)
				{
					if (nextRandom == 0 && CustomOptions.level1On)
					{
						g.SetCurrentLevel(1);
					}
					if (nextRandom == 1 && CustomOptions.level2On)
					{
						g.SetCurrentLevel(2);
					}
					if (nextRandom == 2 && CustomOptions.level3On)
					{
						g.SetCurrentLevel(3);
					}
					if (nextRandom == 3 && CustomOptions.level4On)
					{
						g.SetCurrentLevel(4);
					}
					if (nextRandom == 4 && CustomOptions.level5On)
					{
						g.SetCurrentLevel(5);
					}
				}
				if (roundNumber >= 3)
				{
					if (nextRandom == 0 && CustomOptions.level1On)
					{
						g.SetCurrentLevel(1);
					}
					if (nextRandom == 1 && CustomOptions.level2On)
					{
						g.SetCurrentLevel(2);
					}
					if (nextRandom == 2 && CustomOptions.level3On)
					{
						g.SetCurrentLevel(3);
					}
					if (nextRandom == 3 && CustomOptions.level4On)
					{
						g.SetCurrentLevel(4);
					}
					if (nextRandom == 4 && CustomOptions.level5On)
					{
						g.SetCurrentLevel(5);
					}
				}
			}
			while ((!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) && g.GetCurrentLevel() == currentLevel) || (((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) && (g.GetCurrentLevel() == currentLevel || (g.GetCurrentLevel() != 1 && g.GetCurrentLevel() != 4))));
		}
		else
		{
			if (CustomOptions.level1On)
			{
				g.SetCurrentLevel(1);
			}
			if (CustomOptions.level2On)
			{
				g.SetCurrentLevel(2);
			}
			if (CustomOptions.level3On)
			{
				g.SetCurrentLevel(3);
			}
			if (CustomOptions.level4On)
			{
				g.SetCurrentLevel(4);
			}
			if (CustomOptions.level5On)
			{
				g.SetCurrentLevel(5);
			}
		}
		if (currentLevel == 0)
		{
			g.theInGameScreenText.ShowTutorial();
			if (CustomOptions.level1On)
			{
				g.SetCurrentLevel(1);
			}
		}
		else if ((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode)
		{
			g.theInGameScreenText.ShowFullGameText();
		}
		else
		{
			g.theInGameScreenText.InitialiseRoundText();
		}
	}
}
