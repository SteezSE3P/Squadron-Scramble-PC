namespace SquadronScramble;

public class SquadronMember
{
	private GameWorld g;

	private string name;

	private int score;

	private bool alive = true;

	private bool dying = false;

	private bool inAction = false;

	private bool taken = false;

	public SquadronMember(GameWorld gw)
	{
		g = gw;
	}

	public string GetName()
	{
		return name;
	}

	public void SetName(int i, int s)
	{
		name = g.NameList.GetName(i, s);
	}

	public void ResetOriginalName(int i, int s)
	{
		name = g.NameList.ResetOriginalName(i, s);
	}

	public void SetCustomName(string s)
	{
		name = s;
	}

	public void FullReset()
	{
		alive = true;
		dying = false;
		taken = false;
		inAction = false;
		score = 0;
	}

	public int GetScore()
	{
		return score;
	}

	public void SetScore(int i)
	{
		score = i;
		if (score > 999)
		{
			score = 999;
		}
	}

	public void IncrementScore(int i)
	{
		score += i;
		if (score > 999)
		{
			score = 999;
		}
	}

	public void DecrementScore(int i)
	{
		if (score > 0)
		{
			score -= i;
		}
		g.theSoundManager.BuzzerSound();
	}

	public bool GetAlive()
	{
		return alive;
	}

	public void SetAlive(bool b)
	{
		alive = b;
	}

	public bool GetTaken()
	{
		return taken;
	}

	public void SetTaken(bool b)
	{
		taken = b;
	}

	public bool GetDying()
	{
		return dying;
	}

	public void SetDying(bool b)
	{
		dying = b;
	}

	public bool GetInAction()
	{
		return inAction;
	}

	public void SetInAction(bool b)
	{
		inAction = b;
	}

	public bool CheckColor(int c)
	{
		switch (c)
		{
		case 0:
			if (alive && !inAction)
			{
				return true;
			}
			return false;
		case 1:
			if (alive && inAction && !dying)
			{
				return true;
			}
			return false;
		case 2:
			if (!alive && !dying)
			{
				return true;
			}
			return false;
		case 3:
			if (!alive && dying)
			{
				return true;
			}
			return false;
		default:
			return false;
		}
	}
}
