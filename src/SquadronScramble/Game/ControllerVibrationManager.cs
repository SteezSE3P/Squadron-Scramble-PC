using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SquadronScramble;

public class ControllerVibrationManager
{
	private GameWorld g;

	private PlayerIndex thePlayerIndex;

	private float vibrationTimer = 0f;

	private float vibrationLevel = 0f;

	private bool vibrationOn = true;

	public ControllerVibrationManager(GameWorld gw, PlayerIndex pi)
	{
		g = gw;
		thePlayerIndex = pi;
	}

	public void Update(GameTime theGameTime, bool active)
	{
		vibrationTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (vibrationTimer <= 0f || !vibrationOn)
		{
			vibrationTimer = 0f;
			vibrationLevel = 0f;
		}
		GamePad.SetVibration(thePlayerIndex, vibrationLevel, vibrationLevel);
	}

	public void SetVibration(float vl, float vt)
	{
		if (vl >= vibrationLevel)
		{
			vibrationLevel = vl;
			vibrationTimer = vt;
		}
	}

	public void StopVibration()
	{
		vibrationLevel = 0f;
		vibrationTimer = 0f;
	}

	public void SetVibrationOn(bool b)
	{
		vibrationOn = b;
	}

	public bool GetVibrationOn()
	{
		return vibrationOn;
	}
}
