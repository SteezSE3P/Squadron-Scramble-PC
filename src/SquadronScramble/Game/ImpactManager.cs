using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class ImpactManager
{
	private Impact[] theImpact;

	private bool active;

	private Vector2 impactLocation = new Vector2(0f, 0f);

	private string impactType = "";

	private int impactLimit = 5;

	private float impactSpeed = 0.025f;

	private float shade = 255f;

	private float SHADERECOVERY = 500f;

	public ImpactManager()
	{
		theImpact = new Impact[impactLimit];
		for (int i = 0; i < impactLimit; i++)
		{
			theImpact[i] = new Impact();
		}
	}

	public void LoadContent()
	{
		for (int i = 0; i < impactLimit; i++)
		{
			theImpact[i].LoadContent();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < impactLimit; i++)
		{
			theImpact[i].Draw(theSpriteBatch);
		}
	}

	public void Update(GameTime theGameTime)
	{
		for (int i = 0; i < impactLimit; i++)
		{
			theImpact[i].Update(theGameTime);
		}
		if (shade < 255f)
		{
			shade += SHADERECOVERY * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else
		{
			shade = 255f;
		}
		if (active)
		{
			int j;
			for (j = 0; theImpact[j].GetActive() && j < theImpact.Length - 1; j++)
			{
			}
			if (!theImpact[j].GetActive())
			{
				theImpact[j].SetActive(b: true, impactLocation, impactType, (int)shade);
			}
		}
		active = false;
	}

	public void SetActive(bool b, Vector2 v, string st)
	{
		active = b;
		impactLocation = v;
		impactType = st;
	}

	public void SetShade(int i)
	{
		shade = i;
	}
}
