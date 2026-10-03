using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class VictoryExhaustManager
{
	private VictoryPlane theVictoryPlane;

	private VictoryFume[] theVictoryFume;

	private bool active;

	private float puffTimer = 0f;

	private int fumeLimit = 50;

	private float puffSpeed = 0.025f;

	public VictoryExhaustManager(VictoryPlane p)
	{
		theVictoryPlane = p;
		theVictoryFume = new VictoryFume[fumeLimit];
		for (int i = 0; i < fumeLimit; i++)
		{
			theVictoryFume[i] = new VictoryFume(theVictoryPlane);
		}
	}

	public void LoadContent()
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theVictoryFume[i].LoadContent();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theVictoryFume[i].Draw(theSpriteBatch);
		}
	}

	public void Update(GameTime theGameTime)
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theVictoryFume[i].Update(theGameTime);
		}
		if (!active)
		{
			return;
		}
		puffTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (puffTimer >= puffSpeed)
		{
			puffTimer -= puffSpeed;
			int j;
			for (j = 0; theVictoryFume[j].GetActive() && j < theVictoryFume.Length - 1; j++)
			{
			}
			if (!theVictoryFume[j].GetActive())
			{
				theVictoryFume[j].SetActive(b: true);
			}
		}
	}

	public void SetActive(bool b)
	{
		active = b;
	}
}
