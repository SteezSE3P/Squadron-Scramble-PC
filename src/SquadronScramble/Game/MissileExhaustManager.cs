using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class MissileExhaustManager
{
	private Missile theMissile;

	private MissileFume[] theFumes;

	private bool active;

	private float puffTimer = 0f;

	private int fumeLimit = 50;

	private float puffSpeed = 0.025f;

	private float shade = 255f;

	private float SHADERECOVERY = 500f;

	public MissileExhaustManager(Missile m)
	{
		theMissile = m;
		theFumes = new MissileFume[fumeLimit];
		for (int i = 0; i < fumeLimit; i++)
		{
			theFumes[i] = new MissileFume(theMissile);
		}
	}

	public void LoadContent()
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theFumes[i].LoadContent();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theFumes[i].Draw(theSpriteBatch);
		}
	}

	public void Update(GameTime theGameTime)
	{
		for (int i = 0; i < fumeLimit; i++)
		{
			theFumes[i].Update(theGameTime);
		}
		if (shade < 50f)
		{
			shade += SHADERECOVERY * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else
		{
			shade = 50f;
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
			for (j = 0; theFumes[j].GetActive() && j < theFumes.Length - 1; j++)
			{
			}
			if (!theFumes[j].GetActive())
			{
				theFumes[j].SetActive(b: true, (int)shade);
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
}
