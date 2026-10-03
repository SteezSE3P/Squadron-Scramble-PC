using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class FrontEndExhaustManager
{
	private Vector2 thePosition;

	private float theDirection;

	private FrontEndFume[] theFume;

	private bool active = true;

	private float puffTimer = 0f;

	private int fumeLimit = 50;

	private float puffSpeed = 0.025f;

	private float shade = 255f;

	private float SHADERECOVERY = 500f;

	public FrontEndExhaustManager(Vector2 v, float f)
	{
		thePosition = v;
		theDirection = f;
		theFume = new FrontEndFume[fumeLimit];
		for (int i = 0; i < fumeLimit; i++)
		{
			theFume[i] = new FrontEndFume();
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

	public void Update(GameTime theGameTime, Vector2 v, float f)
	{
		thePosition = v;
		theDirection = f;
		for (int i = 0; i < fumeLimit; i++)
		{
			theFume[i].Update(theGameTime, thePosition, theDirection);
		}
		if (shade < 255f)
		{
			shade += SHADERECOVERY * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else
		{
			shade = 255f;
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
				theFume[j].SetActive(b: true, (int)shade);
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
