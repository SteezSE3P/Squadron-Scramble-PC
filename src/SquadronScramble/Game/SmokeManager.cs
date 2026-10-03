using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class SmokeManager
{
	private Plane thePlane;

	private Smoke[] theSmoke;

	private bool active;

	private float puffTimer = 0f;

	private int smokeLimit = 12;

	private float puffSpeed = 0.075f;

	public SmokeManager(Plane p)
	{
		thePlane = p;
		theSmoke = new Smoke[smokeLimit];
		for (int i = 0; i < smokeLimit; i++)
		{
			theSmoke[i] = new Smoke(thePlane);
		}
	}

	public void LoadContent()
	{
		for (int i = 0; i < smokeLimit; i++)
		{
			theSmoke[i].LoadContent();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < smokeLimit; i++)
		{
			theSmoke[i].Draw(theSpriteBatch);
		}
	}

	public void Update(GameTime theGameTime)
	{
		for (int i = 0; i < smokeLimit; i++)
		{
			theSmoke[i].Update(theGameTime);
		}
		if (!active)
		{
			return;
		}
		puffTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (puffTimer >= puffSpeed)
		{
			puffTimer = 0f;
			int j;
			for (j = 0; theSmoke[j].GetActive() && j < theSmoke.Length - 1; j++)
			{
			}
			if (!theSmoke[j].GetActive())
			{
				theSmoke[j].SetActive(b: true);
			}
		}
	}

	public void SetActive(bool b)
	{
		active = b;
	}
}
