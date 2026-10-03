using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class VictorySmokeManager
{
	private VictoryPlane theVictoryPlane;

	private VictorySmoke[] theVictorySmoke;

	private bool active;

	private float puffTimer = 0f;

	private Color theColor = Color.White;

	private int smokeLimit = 200;

	private float puffSpeed = 0.05f;

	public VictorySmokeManager(VictoryPlane p)
	{
		theVictoryPlane = p;
		theVictorySmoke = new VictorySmoke[smokeLimit];
		for (int i = 0; i < smokeLimit; i++)
		{
			theVictorySmoke[i] = new VictorySmoke(theVictoryPlane);
		}
	}

	public void LoadContent()
	{
		for (int i = 0; i < smokeLimit; i++)
		{
			theVictorySmoke[i].LoadContent();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < smokeLimit; i++)
		{
			theVictorySmoke[i].Draw(theSpriteBatch);
		}
	}

	public void Update(GameTime theGameTime)
	{
		if (active)
		{
			puffTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (puffTimer >= puffSpeed)
			{
				puffTimer = 0f;
				int i;
				for (i = 0; theVictorySmoke[i].GetActive() && i < theVictorySmoke.Length - 1; i++)
				{
				}
				if (!theVictorySmoke[i].GetActive())
				{
					theVictorySmoke[i].SetActive(b: true);
				}
			}
		}
		for (int j = 0; j < smokeLimit; j++)
		{
			theVictorySmoke[j].Update(theGameTime);
		}
	}

	public void SetActive(bool b)
	{
		active = b;
	}

	public void SetTheColor(Color c)
	{
		theColor = c;
		SetSmokeColor();
	}

	public void SetSmokeColor()
	{
		for (int i = 0; i < smokeLimit; i++)
		{
			theVictorySmoke[i].SetTheColor(theColor);
		}
	}

	public void Reset()
	{
		active = false;
		puffTimer = 0f;
		for (int i = 0; i < smokeLimit; i++)
		{
			theVictorySmoke[i].Reset();
		}
	}
}
