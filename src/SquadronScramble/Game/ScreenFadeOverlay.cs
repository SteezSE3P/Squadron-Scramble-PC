using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class ScreenFadeOverlay
{
	private enum State
	{
		fadedUp,
		fadingUp,
		fadingDown,
		fadedDown
	}

	private GameWorld g;

	private Vector2 position = new Vector2(0f, 0f);

	private float alphaValue = 0f;

	private float fadeRate = 1f;

	private State currentState = State.fadedUp;

	private Texture2D screenFadeOverlayTexture;

	public ScreenFadeOverlay(GameWorld gw)
	{
		g = gw;
	}

	public void Update(GameTime theGameTime)
	{
		float num = 1f;
		if (currentState == State.fadingUp)
		{
			alphaValue -= num * (float)theGameTime.ElapsedGameTime.TotalSeconds * fadeRate;
			if (alphaValue <= 0f)
			{
				alphaValue = 0f;
				currentState = State.fadedUp;
			}
		}
		if (currentState == State.fadingDown)
		{
			alphaValue += num * (float)theGameTime.ElapsedGameTime.TotalSeconds * fadeRate;
			if (alphaValue >= 1f)
			{
				alphaValue = 1f;
				currentState = State.fadedDown;
			}
		}
	}

	public void LoadContent(ContentManager theContentManager)
	{
		screenFadeOverlayTexture = theContentManager.Load<Texture2D>("ScreenFadeOverlay");
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(origin: new Vector2(0f, 0f), color: new Color(0f, 0f, 0f, alphaValue), texture: screenFadeOverlayTexture, position: position, sourceRectangle: null, rotation: 0f, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
	}

	public void FadeScreen(float f)
	{
		currentState = State.fadingDown;
		fadeRate = f;
	}

	public void UnFadeScreen(float f)
	{
		currentState = State.fadingUp;
		fadeRate = f;
	}

	public bool StateisFadedDown()
	{
		if (currentState == State.fadedDown)
		{
			return true;
		}
		return false;
	}

	public bool StateisFadedUp()
	{
		if (currentState == State.fadedUp)
		{
			return true;
		}
		return false;
	}

	public void SetAlphaValue(float f)
	{
		alphaValue = f;
	}
}
