using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class SafeArea
{
	private GameWorld g;

	// PC port: default to the full screen area (the Xbox defaulted to a TV-safe border, mode 1).
	private int screenMode = 0;

	private int provScreenMode = 0;

	private float planeXMinLimit = 0f;

	private float planeXMaxLimit = 0f;

	private float pilotXMinLimit = 0f;

	private float pilotXMaxLimit = 0f;

	private float stallCeiling = 0f;

	private Texture2D backgroundTexture;

	private Texture2D background2Texture;

	private Vector2 screenPosition = new Vector2(0f, 0f);

	public void LoadContent(ContentManager theContentManager)
	{
		backgroundTexture = theContentManager.Load<Texture2D>("SafeArea");
		background2Texture = theContentManager.Load<Texture2D>("SafeAreaHalf");
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(0f, 0f);
		if (!g.theOptionsOverlay.GetIsAdjustingScreen())
		{
			if (screenMode == 0)
			{
				theSpriteBatch.Draw(backgroundTexture, screenPosition, null, new Color(0f, 0f, 0f, 0f), 0f, origin, 1f, SpriteEffects.None, 0f);
			}
			else if (screenMode == 1)
			{
				theSpriteBatch.Draw(background2Texture, screenPosition, null, new Color(0f, 0f, 0f, 1f), 0f, origin, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				theSpriteBatch.Draw(backgroundTexture, screenPosition, null, new Color(0f, 0f, 0f, 1f), 0f, origin, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public SafeArea(GameWorld gw)
	{
		g = gw;
	}

	public void Update(GameTime theGameTime)
	{
		if (screenMode == 0)
		{
			planeXMinLimit = -25f;
			planeXMaxLimit = 993f;
			pilotXMinLimit = 30f;
			pilotXMaxLimit = 940f;
			stallCeiling = 10f;
		}
		else if (screenMode == 1)
		{
			planeXMinLimit = -5f;
			planeXMaxLimit = 964f;
			pilotXMinLimit = 45f;
			pilotXMaxLimit = 907f;
			stallCeiling = 44f;
		}
		else
		{
			planeXMinLimit = 10f;
			planeXMaxLimit = 943f;
			pilotXMinLimit = 80f;
			pilotXMaxLimit = 875f;
			stallCeiling = 75f;
		}
	}

	public float GetPlaneXMinLimit()
	{
		return planeXMinLimit;
	}

	public float GetPlaneXMaxLimit()
	{
		return planeXMaxLimit;
	}

	public float GetPilotXMinLimit()
	{
		return pilotXMinLimit;
	}

	public float GetPilotXMaxLimit()
	{
		return pilotXMaxLimit;
	}

	public int GetScreenMode()
	{
		return screenMode;
	}

	public float GetStallCeiling()
	{
		return stallCeiling;
	}

	public void SetProvScreenMode(int i)
	{
		provScreenMode = i;
	}

	public void SetScreenMode(int i)
	{
		screenMode = i;
	}

	public int GetProvScreenMode()
	{
		return provScreenMode;
	}

	public void IncreaseProvScreenMode()
	{
		if (provScreenMode > 0)
		{
			provScreenMode--;
		}
	}

	public void DecreaseProvScreenMode()
	{
		if (provScreenMode < 2)
		{
			provScreenMode++;
		}
	}
}
