using Microsoft.Xna.Framework;

namespace SquadronScramble;

public class ScreenScoreBonus
{
	private GameWorld g;

	private SpecialBonus theSpecialBonus;

	private Vector2 position;

	private Vector2 origin;

	private string scoreString;

	private Color theColor;

	private float timer;

	private static int DISPLAYTIME = 2;

	public ScreenScoreBonus(GameWorld gw, SpecialBonus sb)
	{
		g = gw;
		theSpecialBonus = sb;
		origin = new Vector2(0f, 0f);
		scoreString = "";
		theColor = Color.LightGreen;
	}

	public void Update(GameTime theGameTime)
	{
		position.Y -= 10f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		timer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (timer > 0f)
		{
			scoreString = "+3";
		}
		else
		{
			scoreString = "";
			timer = 0f;
		}
		origin = g.theFontManager.GetScreenScoreFont().MeasureString(scoreString) / 2f;
	}

	public void SetTheColor(Color c)
	{
		theColor = c;
	}

	public string GetScoreString()
	{
		return scoreString;
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public Vector2 GetOrigin()
	{
		return origin;
	}

	public Color GetTheColor()
	{
		return theColor;
	}

	public void DisplayScreenScoreBonus(Color c)
	{
		timer = DISPLAYTIME;
		position = theSpecialBonus.GetPosition();
		position.Y -= 20f;
		theColor = c;
	}

	public void ScreenScoreOff()
	{
		timer = 0f;
	}
}
