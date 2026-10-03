using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class ScreenControlDetails
{
	private GameWorld g;

	private Vector2 position = new Vector2(0f, 0f);

	private bool active = false;

	private Texture2D screenControlDetailsTexture;

	public ScreenControlDetails(GameWorld gw)
	{
		g = gw;
	}

	public void Update(GameTime theGameTime)
	{
	}

	public void LoadContent()
	{
		screenControlDetailsTexture = TextureManager.GetScreenControlDetailsTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(0f, 0f);
		if (active)
		{
			theSpriteBatch.Draw(screenControlDetailsTexture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
	}

	public void SetActive(bool b)
	{
		active = b;
	}
}
