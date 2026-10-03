using Microsoft.Xna.Framework;

namespace SquadronScramble;

public class ScreenScore
{
	private GameWorld g;

	private Pilot thePilot;

	private Vector2 namePosition;

	private Vector2 nameScorePosition;

	private Vector2 scorePosition;

	private Vector2 nameOrigin;

	private Vector2 nameScoreOrigin;

	private Vector2 scoreOrigin;

	private string scoreString;

	private string nameString;

	private string nameScoreString;

	private float scoreTimer;

	private float nameTimer;

	private Color theNameColor;

	private Color theScoreColor;

	private bool needToFlyWarning = false;

	private bool noPilotsLeftWarning = false;

	private static float SCOREDISPLAYTIME = 1.5f;

	private static float NAMEDISPLAYTIME = 2.5f;

	private int lastScore = 0;

	private string lastName = "";

	private string lastNameString = "";

	private string lastNameScoreString = "";

	public ScreenScore(GameWorld gw, Pilot p)
	{
		g = gw;
		thePilot = p;
		namePosition = thePilot.GetPosition();
		nameScorePosition = thePilot.GetPosition();
		scorePosition = new Vector2(0f, 0f);
		nameOrigin = new Vector2(0f, 0f);
		nameScoreOrigin = new Vector2(0f, 0f);
		scoreOrigin = new Vector2(0f, 0f);
		scoreString = "";
		nameString = "";
		nameScoreString = "";
		theScoreColor = Color.White;
		theNameColor = Color.White;
	}

	public void Update(GameTime theGameTime)
	{
		bool flag = false;
		if (thePilot.GetInPlane() && thePilot.GetThePlane().GetActive())
		{
			namePosition = thePilot.GetThePlane().GetPosition();
			nameScorePosition = thePilot.GetThePlane().GetPosition();
			scorePosition = thePilot.GetThePlane().GetPosition();
			flag = true;
		}
		else if (!thePilot.IsOff())
		{
			namePosition = thePilot.GetPosition();
			nameScorePosition = thePilot.GetPosition();
			scorePosition = thePilot.GetPosition();
			flag = true;
		}
		if (flag)
		{
			scorePosition.X = scorePosition.X;
			scorePosition.Y -= 30f;
			namePosition.X = namePosition.X;
			namePosition.Y += 34f;
			nameScorePosition.Y += 34f;
		}
		if (namePosition.Y < Level.GetPilotGroundY() + 34f)
		{
			namePosition.Y = Level.GetPilotGroundY() + 34f;
		}
		if (nameScorePosition.Y < Level.GetPilotGroundY() + 34f)
		{
			nameScorePosition.Y = Level.GetPilotGroundY() + 34f;
		}
		if (thePilot.GetCurrentSquadronMember() == null)
		{
			return;
		}
		scoreTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		nameTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (nameTimer > 0f)
		{
			if (lastScore != thePilot.GetCurrentSquadronMember().GetScore() || lastName != thePilot.GetCurrentSquadronMember().GetName())
			{
				lastScore = thePilot.GetCurrentSquadronMember().GetScore();
				lastName = thePilot.GetCurrentSquadronMember().GetName();
				lastNameString = lastName;
				lastNameScoreString = lastScore.ToString();
			}
			nameString = lastNameString;
			nameScoreString = lastNameScoreString;
			if (needToFlyWarning)
			{
				nameString = "NEED TO FLY!";
				nameScoreString = "";
			}
			if (noPilotsLeftWarning)
			{
				nameString = "NO MORE PILOTS!";
				nameScoreString = "";
			}
			theNameColor = thePilot.GetCurrentSquadron().GetTheColor();
		}
		else
		{
			nameString = "";
			nameScoreString = "";
			nameTimer = 0f;
			needToFlyWarning = false;
			noPilotsLeftWarning = false;
		}
		if (g.theNamePlate.ControllerIconIsFlashing(thePilot))
		{
			theNameColor = Color.Red;
		}
		if (g.theNamePlate.ControllerIconIsFlashing(thePilot) && thePilot.GetCurrentSquadronMember().GetAlive())
		{
			theNameColor = thePilot.GetCurrentSquadron().GetTheColor();
			nameString = "";
			nameScoreString = "";
		}
		if (scoreTimer > 0f && nameTimer <= 0f)
		{
			scoreString = thePilot.GetCurrentSquadronMember().GetScore().ToString();
		}
		if (scoreTimer <= 0f || nameTimer > 0f)
		{
			scoreString = "";
			scoreTimer = 0f;
		}
		scoreOrigin = g.theFontManager.GetScreenScoreFont().MeasureString(scoreString) / 2f;
		if (needToFlyWarning || noPilotsLeftWarning)
		{
			nameOrigin = g.theFontManager.GetFont().MeasureString(nameString) / 2f;
			nameScoreOrigin = g.theFontManager.GetFont().MeasureString(nameScoreString) / 2f;
			return;
		}
		nameOrigin = new Vector2(g.theFontManager.GetFont().MeasureString(nameString).X, g.theFontManager.GetFont().MeasureString(nameString).Y / 2f) + new Vector2(g.theFontManager.GetFont().MeasureString("  ").X / 2f, 0f);
		nameScoreOrigin = new Vector2(0f, g.theFontManager.GetFont().MeasureString(nameScoreString).Y / 2f) - new Vector2(g.theFontManager.GetFont().MeasureString("  ").X / 2f, 0f);
		nameOrigin.X -= (g.theFontManager.GetFont().MeasureString(nameString).X - g.theFontManager.GetFont().MeasureString(nameScoreString).X) / 2f;
		nameScoreOrigin.X -= (g.theFontManager.GetFont().MeasureString(nameString).X - g.theFontManager.GetFont().MeasureString(nameScoreString).X) / 2f;
	}

	public void SetTheScoreColor(Color c)
	{
		theScoreColor = c;
	}

	public void SetTheNameColor(Color c)
	{
		theNameColor = c;
	}

	public string GetScoreString()
	{
		return scoreString;
	}

	public string GetNameString()
	{
		return nameString;
	}

	public string GetNameScoreString()
	{
		return nameScoreString;
	}

	public Vector2 GetNamePosition()
	{
		return namePosition;
	}

	public Vector2 GetNameScorePosition()
	{
		return nameScorePosition;
	}

	public void SetNamePosition(Vector2 v)
	{
		namePosition = v;
	}

	public void SetNameScorePosition(Vector2 v)
	{
		nameScorePosition = v;
	}

	public Vector2 GetScorePosition()
	{
		return scorePosition;
	}

	public Vector2 GetScoreOrigin()
	{
		return scoreOrigin;
	}

	public Vector2 GetNameOrigin()
	{
		return nameOrigin;
	}

	public Vector2 GetNameScoreOrigin()
	{
		return nameScoreOrigin;
	}

	public Color GetTheScoreColor()
	{
		return theScoreColor;
	}

	public Color GetTheNameColor()
	{
		return theNameColor;
	}

	public void DisplayScreenScore()
	{
		scoreTimer = SCOREDISPLAYTIME;
	}

	public void ScreenScoreOff()
	{
		scoreTimer = 0f;
	}

	public void DisplayScreenName()
	{
		needToFlyWarning = false;
		noPilotsLeftWarning = false;
		nameTimer = NAMEDISPLAYTIME;
	}

	public void DisplayNeedToFlyWarning()
	{
		nameTimer = NAMEDISPLAYTIME;
		needToFlyWarning = true;
	}

	public void DisplayNoPilotsLeftWarning()
	{
		nameTimer = NAMEDISPLAYTIME;
		noPilotsLeftWarning = true;
	}

	public void ScreenNameOff()
	{
		nameTimer = 0f;
	}
}
