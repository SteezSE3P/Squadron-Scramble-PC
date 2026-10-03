using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class SplashPilot
{
	private GameWorld g;

	private Vector2 position;

	private bool explosionTriggered;

	private float animFrame;

	private Rectangle sourceRect;

	private float scale = 0f;

	private int frameWidth = 96;

	private int frameHeight = 96;

	private int animFrames = 12;

	private int animSpeed = 15;

	private Texture2D mSpriteTexture;

	public SplashPilot(GameWorld gw)
	{
		g = gw;
		position = new Vector2(100f, 100f);
		explosionTriggered = false;
		animFrame = 0f;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetSplashTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(origin: new Vector2(48f, 48f), texture: mSpriteTexture, position: position, sourceRectangle: sourceRect, color: Color.White, rotation: 0f, scale: scale, effects: SpriteEffects.None, layerDepth: 0f);
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
		Animate(theGameTime);
	}

	public void TriggerSplash(GameTime theGameTime, Vector2 p, float s, float t)
	{
		explosionTriggered = true;
		g.theSoundManager.SplashSound(t);
		animFrame = 0f;
		scale = s;
		position = p;
	}

	public void Animate(GameTime theGameTime)
	{
		int num = frameWidth;
		sourceRect = new Rectangle(y: (!(animFrame < 6f)) ? 96 : 0, x: (int)animFrame % 6 * num, width: frameWidth, height: frameHeight);
		animFrame += (float)animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (animFrame >= (float)animFrames)
		{
			explosionTriggered = false;
		}
	}

	public void CheckPosition(GameTime theGameTime)
	{
		if (!explosionTriggered)
		{
			position.X = -100f;
			position.Y = -100f;
			animFrame = 0f;
		}
	}
}
