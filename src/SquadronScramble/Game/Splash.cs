using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Splash
{
	private GameWorld g;

	private Plane thePlane;

	private Vector2 position;

	private Vector2 origin;

	public bool splashTriggered;

	public float animFrame;

	public Rectangle sourceRect;

	private bool facingRight;

	private bool isNew;

	private int frameWidth = 96;

	private int frameHeight = 96;

	private int animFrames = 12;

	private int animSpeed = 15;

	private float splashRadius = 30f;

	private Texture2D mSpriteTexture;

	public Splash(GameWorld gw, Plane p)
	{
		g = gw;
		thePlane = p;
		position = new Vector2(100f, 100f);
		splashTriggered = false;
		animFrame = 0f;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetGroundSplashTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		if (thePlane.GetThePilot().IsParticipating())
		{
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
		}
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
		Animate(theGameTime);
	}

	public void TriggerSplash(GameTime theGameTime, Vector2 p, bool b, float t)
	{
		splashTriggered = true;
		g.theSoundManager.SplashSound(t);
		facingRight = b;
		isNew = true;
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
		if (animFrame > 1f)
		{
			isNew = false;
		}
		animFrame += (float)animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (animFrame >= (float)animFrames)
		{
			splashTriggered = false;
		}
	}

	public void CheckPosition(GameTime theGameTime)
	{
		if (!splashTriggered)
		{
			position.X = -100f;
			position.Y = -100f;
		}
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public float GetSplashRadius()
	{
		return splashRadius;
	}

	public bool GetIsNew()
	{
		return isNew;
	}
}
