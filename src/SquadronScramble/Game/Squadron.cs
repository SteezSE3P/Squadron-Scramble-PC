using Microsoft.Xna.Framework;

namespace SquadronScramble;

public class Squadron
{
	private GameWorld g;

	private string name;

	private int squadronNumber;

	private SquadronMember[] members;

	private SquadronMember[] combatants;

	private Color theColor;

	private int trophies = 0;

	private static int squadronSize = 36;

	private int squadronSectionSize = 4;

	public Squadron(GameWorld gw, int s)
	{
		squadronNumber = s;
		g = gw;
		theColor = Color.Black;
		members = new SquadronMember[squadronSize];
		combatants = new SquadronMember[4];
		SetName();
		for (int i = 0; i < squadronSize; i++)
		{
			members[i] = new SquadronMember(g);
		}
		SetMemberNames();
		SquadronMember[] array = members;
		foreach (SquadronMember squadronMember in array)
		{
			squadronMember.SetScore(0);
		}
	}

	public void SelectFirstCombatants()
	{
		bool flag = false;
		for (int i = 0; i < squadronSectionSize; i++)
		{
			do
			{
				flag = false;
				combatants[i] = members[General.GetNextRandom(0, squadronSize)];
				for (int j = 0; j < i; j++)
				{
					if (combatants[j] == combatants[i])
					{
						flag = true;
					}
				}
			}
			while (flag);
		}
	}

	public void SelectNewReplacements()
	{
		bool flag = false;
		for (int i = 0; i < squadronSectionSize; i++)
		{
			if (combatants[i].GetAlive())
			{
				continue;
			}
			do
			{
				flag = false;
				combatants[i] = members[General.GetNextRandom(0, squadronSize)];
				for (int j = 0; j < squadronSectionSize; j++)
				{
					if (j != i && combatants[j] == combatants[i])
					{
						flag = true;
					}
				}
			}
			while (flag || !combatants[i].GetAlive());
		}
	}

	public void ResetActivePilots()
	{
		for (int i = 0; i < squadronSectionSize; i++)
		{
			combatants[i].SetInAction(b: false);
		}
	}

	public SquadronMember SelectOnDuty(SquadronMember current)
	{
		if (!PilotIsAvailable())
		{
			return null;
		}
		int nextRandom;
		do
		{
			nextRandom = General.GetNextRandom(0, squadronSectionSize);
		}
		while (!combatants[nextRandom].GetAlive() || combatants[nextRandom].GetInAction() || combatants[nextRandom] == current);
		current?.SetInAction(b: false);
		combatants[nextRandom].SetInAction(b: true);
		return combatants[nextRandom];
	}

	public void RoundReset()
	{
	}

	public void FullReset()
	{
		trophies = 0;
		for (int i = 0; i < squadronSize; i++)
		{
			GetMember(i).FullReset();
		}
	}

	public void SetName()
	{
		name = g.NameList.GetSquadronName(squadronNumber);
	}

	public bool GetAllPilotsDead()
	{
		bool result = true;
		for (int i = 0; i < squadronSectionSize; i++)
		{
			if (combatants[i].GetAlive())
			{
				result = false;
			}
		}
		return result;
	}

	public bool CombatantIsInSection(SquadronMember s)
	{
		bool result = false;
		for (int i = 0; i < squadronSectionSize; i++)
		{
			if (combatants[i] == s)
			{
				result = true;
			}
		}
		return result;
	}

	public string GetName()
	{
		return name;
	}

	public void IncrementTrophies(int i)
	{
		trophies += i;
		g.theSoundManager.CupAwardedSound();
	}

	public SquadronMember GetCombatant(int i)
	{
		return combatants[i];
	}

	public void SetTheColor(Color c)
	{
		theColor = c;
	}

	public Color GetTheColor()
	{
		return theColor;
	}

	public SquadronMember GetMember(int i)
	{
		return members[i];
	}

	public static int GetSquadronSize()
	{
		return squadronSize;
	}

	public int GetSquadronNumber()
	{
		return squadronNumber;
	}

	public bool PilotIsAvailable()
	{
		int num = 0;
		for (int i = 0; i < squadronSectionSize; i++)
		{
			if (combatants[i].GetAlive() && !combatants[i].GetInAction())
			{
				num++;
			}
		}
		if (num > 0)
		{
			return true;
		}
		return false;
	}

	public bool SquadronIsActive()
	{
		for (int i = 0; i < squadronSectionSize; i++)
		{
			if (combatants[i].GetAlive())
			{
				return true;
			}
		}
		return false;
	}

	public bool SquadronIsInService()
	{
		bool result = false;
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetCurrentSquadron() == this)
			{
				result = true;
			}
		}
		return result;
	}

	public bool LowerScorePilotIsAvailable(Pilot p)
	{
		bool result = false;
		for (int i = 0; i < squadronSectionSize; i++)
		{
			if (combatants[i] != p.GetCurrentSquadronMember() && !combatants[i].GetInAction() && combatants[i].GetAlive() && combatants[i].GetScore() < p.GetCurrentSquadronMember().GetScore())
			{
				result = true;
			}
		}
		return result;
	}

	public int GetSquadronSectionSize()
	{
		return squadronSectionSize;
	}

	public void SetSquadronSectionSize(int i)
	{
		squadronSectionSize = i;
	}

	public int GetScore()
	{
		int num = 0;
		for (int i = 0; i < squadronSectionSize; i++)
		{
			if (combatants[i].GetAlive())
			{
				num += combatants[i].GetScore();
			}
		}
		if (num > 999)
		{
			num = 999;
		}
		if (GetAllPilotsDead())
		{
			num = -1;
		}
		return num;
	}

	public int GetTrophies()
	{
		return trophies;
	}

	public void SetMemberNames()
	{
		for (int i = 0; i < squadronSize; i++)
		{
			members[i].SetName(squadronNumber, i);
		}
	}

	public void ResetMemberNames()
	{
		for (int i = 0; i < members.Length; i++)
		{
			members[i].ResetOriginalName(squadronNumber, i);
		}
	}
}
