using Microsoft.Xna.Framework;

namespace SquadronScramble;

internal class Clock
{
	private GameWorld g;

	private int minutes = 0;

	private int seconds = 0;

	private float time = 0f;

	private int lastSecond = 0;

	private int lastMinute = 0;

	private string theTime = "";

	public Clock(GameWorld gw, float t)
	{
		g = gw;
		time = t * 60f;
		lastMinute = -1;
		lastSecond = -1;
	}

	public void Update(GameTime theGameTime)
	{
		if (g.theRoundManager.GetRoundIsPlayable() && time > -1f)
		{
			time -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		if (time <= 0f && time >= -1f)
		{
			time = -1f;
			if (g.theRoundManager.CheckIfRoundWinner())
			{
				g.RoundOver();
			}
		}
		minutes = (int)time / 60;
		seconds = (int)time % 60;
	}

	public bool GetIsTimeShown()
	{
		if (!g.theInGameScreenText.CheckIfSuddenDeath() || (g.theInGameScreenText.CheckIfSuddenDeath() && g.theRoundManager.CheckIfRoundWinner()))
		{
			return true;
		}
		return false;
	}

	public string GetMinuteString()
	{
		if (minutes == 0)
		{
			return "0";
		}
		if (minutes == 1)
		{
			return "1";
		}
		if (minutes == 2)
		{
			return "2";
		}
		if (minutes == 3)
		{
			return "3";
		}
		if (minutes == 4)
		{
			return "4";
		}
		if (minutes == 5)
		{
			return "5";
		}
		if (minutes == 6)
		{
			return "6";
		}
		if (minutes == 7)
		{
			return "7";
		}
		if (minutes == 8)
		{
			return "8";
		}
		return "9";
	}

	public string GetSecondString()
	{
		if (seconds == 0)
		{
			return "0";
		}
		if (seconds == 1)
		{
			return "1";
		}
		if (seconds == 2)
		{
			return "2";
		}
		if (seconds == 3)
		{
			return "3";
		}
		if (seconds == 4)
		{
			return "4";
		}
		if (seconds == 5)
		{
			return "5";
		}
		if (seconds == 6)
		{
			return "6";
		}
		if (seconds == 7)
		{
			return "7";
		}
		if (seconds == 8)
		{
			return "8";
		}
		if (seconds == 9)
		{
			return "9";
		}
		if (seconds == 10)
		{
			return "10";
		}
		if (seconds == 11)
		{
			return "11";
		}
		if (seconds == 12)
		{
			return "12";
		}
		if (seconds == 13)
		{
			return "13";
		}
		if (seconds == 14)
		{
			return "14";
		}
		if (seconds == 15)
		{
			return "15";
		}
		if (seconds == 16)
		{
			return "16";
		}
		if (seconds == 17)
		{
			return "17";
		}
		if (seconds == 18)
		{
			return "18";
		}
		if (seconds == 19)
		{
			return "19";
		}
		if (seconds == 20)
		{
			return "20";
		}
		if (seconds == 21)
		{
			return "21";
		}
		if (seconds == 22)
		{
			return "22";
		}
		if (seconds == 23)
		{
			return "23";
		}
		if (seconds == 24)
		{
			return "24";
		}
		if (seconds == 25)
		{
			return "25";
		}
		if (seconds == 26)
		{
			return "26";
		}
		if (seconds == 27)
		{
			return "27";
		}
		if (seconds == 28)
		{
			return "28";
		}
		if (seconds == 29)
		{
			return "29";
		}
		if (seconds == 30)
		{
			return "30";
		}
		if (seconds == 31)
		{
			return "31";
		}
		if (seconds == 32)
		{
			return "32";
		}
		if (seconds == 33)
		{
			return "33";
		}
		if (seconds == 34)
		{
			return "34";
		}
		if (seconds == 35)
		{
			return "35";
		}
		if (seconds == 36)
		{
			return "36";
		}
		if (seconds == 37)
		{
			return "37";
		}
		if (seconds == 38)
		{
			return "38";
		}
		if (seconds == 39)
		{
			return "39";
		}
		if (seconds == 40)
		{
			return "40";
		}
		if (seconds == 41)
		{
			return "41";
		}
		if (seconds == 42)
		{
			return "42";
		}
		if (seconds == 43)
		{
			return "43";
		}
		if (seconds == 44)
		{
			return "44";
		}
		if (seconds == 45)
		{
			return "45";
		}
		if (seconds == 46)
		{
			return "46";
		}
		if (seconds == 47)
		{
			return "47";
		}
		if (seconds == 48)
		{
			return "48";
		}
		if (seconds == 49)
		{
			return "49";
		}
		if (seconds == 50)
		{
			return "50";
		}
		if (seconds == 51)
		{
			return "51";
		}
		if (seconds == 52)
		{
			return "52";
		}
		if (seconds == 53)
		{
			return "53";
		}
		if (seconds == 54)
		{
			return "54";
		}
		if (seconds == 55)
		{
			return "55";
		}
		if (seconds == 56)
		{
			return "56";
		}
		if (seconds == 57)
		{
			return "57";
		}
		if (seconds == 58)
		{
			return "58";
		}
		if (seconds == 59)
		{
			return "59";
		}
		return "";
	}

	public float GetTime()
	{
		return time;
	}

	public int GetSeconds()
	{
		return seconds;
	}
}
