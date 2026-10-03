using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class ExhaustManager
{
	private Plane thePlane;

	private Fume[] theFume;

	private bool active;

	private float puffTimer = 0f;

	private int fumeLimit = 20;

	private float puffSpeed = 0.035f;

	private float shade = 255f;

	private float SHADERECOVERY = 500f;

	private float density = 0.05f;

	private float DENSITYRECOVERY = 1.5f;

	public ExhaustManager(Plane p)
	{
		thePlane = p;
		theFume = new Fume[fumeLimit];
		for (int i = 0; i < fumeLimit; i++)
		{
			theFume[i] = new Fume(thePlane);
		}
	}

	public void LoadContent()
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theFume[i].LoadContent();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theFume[i].Draw(theSpriteBatch);
		}
	}

	public void Update(GameTime theGameTime)
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theFume[i].Update(theGameTime);
		}
		if (shade < 255f)
		{
			shade += SHADERECOVERY * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else
		{
			shade = 255f;
		}
		if (density > 0.075f)
		{
			density -= DENSITYRECOVERY * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else
		{
			density = 0.075f;
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
			for (j = 0; theFume[j].GetActive() && j < theFume.Length - 1; j++)
			{
			}
			if (!theFume[j].GetActive())
			{
				theFume[j].SetActive(b: true, (int)shade, density);
			}
		}
	}

	public void SetActive(bool b)
	{
		active = b;
	}

	public void SetShade(int i)
	{
		shade = i;
	}

	public void SetDensity(float d)
	{
		density = d;
	}
}
