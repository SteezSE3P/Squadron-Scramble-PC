using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class GroundExplosion
{
	private GameWorld g;

	private Vector2 position;

	private Vector2 origin;

	private bool explosionTriggered;

	private float animFrame;

	private Rectangle sourceRect;

	private bool facingRight;

	private int frameWidth = 96;

	private int frameHeight = 96;

	private int animFrames = 12;

	private int animSpeed = 15;

	private Texture2D mSpriteTexture;

	public GroundExplosion(GameWorld gw)
	{
		g = gw;
		position = new Vector2(100f, 100f);
		explosionTriggered = false;
		animFrame = 0f;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetGroundCrashTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
		Animate(theGameTime);
	}

	public void TriggerExplosion(GameTime theGameTime, Vector2 p, bool b)
	{
		explosionTriggered = true;
		g.theSoundManager.ExplosionSound();
		facingRight = b;
		animFrame = 0f;
		if (facingRight)
		{
			origin = new Vector2(28f, 48f);
		}
		else
		{
			origin = new Vector2(68f, 48f);
		}
		position = p;
	}

	public void Animate(GameTime theGameTime)
	{
		int num = ((!facingRight) ? 16 : 0);
		sourceRect = new Rectangle(((int)animFrame + num) % 8 * frameWidth, ((int)animFrame + num) / 8 * frameHeight, frameWidth, frameHeight);
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
		}
	}

	public Vector2 GetPosition()
	{
		return position;
	}
}
