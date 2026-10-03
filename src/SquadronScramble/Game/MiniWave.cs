using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class MiniWave
{
	private bool active = false;

	private Vector2 position = new Vector2(0f, 0f);

	private float speed = 0f;

	private float scale = 0f;

	private float animFrame = 0f;

	private Rectangle sourceRect;

	private float inactiveLimit = 0f;

	private float inactiveTimer = 0f;

	private int frameWidth = 40;

	private int frameHeight = 14;

	private int animFrames = 7;

	private int animSpeed = 8;

	private Texture2D mSpriteTexture;

	public MiniWave(float x, float y, float s, float f, float i)
	{
		position.X = x;
		position.Y = y;
		speed = s;
		scale = f;
		inactiveLimit = i;
		inactiveTimer = inactiveLimit;
	}

	public void LoadContent(Texture2D theTexture)
	{
		mSpriteTexture = theTexture;
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(20f, 7f);
		if (active)
		{
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
		}
	}

	public void Animate(GameTime theGameTime)
	{
		int num = frameWidth;
		sourceRect = new Rectangle((int)animFrame % animFrames * num, 0, frameWidth, frameHeight);
		animFrame += (float)animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (animFrame >= (float)animFrames)
		{
			active = false;
		}
	}

	public void Update(GameTime theGameTime)
	{
		if (inactiveTimer > 0f)
		{
			inactiveTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		if (inactiveTimer <= 0f)
		{
			active = true;
		}
		if (active)
		{
			Animate(theGameTime);
			CheckPosition(theGameTime);
		}
		if (!active && inactiveTimer <= 0f)
		{
			SetRandomPosition();
		}
	}

	public void CheckPosition(GameTime theGameTime)
	{
		position.X += speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (position.X < -300f)
		{
			position.X = 1700f;
		}
		if (position.X > 1700f)
		{
			position.X = -300f;
		}
	}

	public void SetRandomPosition()
	{
		animFrame = 0f;
		sourceRect = new Rectangle(0, 0, frameWidth, frameHeight);
		position.X = General.GetNextRandom(0, 2560);
		inactiveTimer = inactiveLimit;
	}

	public void SetMiniWaveY(float y)
	{
		position.Y = y;
	}
}
