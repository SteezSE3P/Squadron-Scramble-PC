using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Flames
{
	private enum Mode
	{
		sideFlames,
		backFlame
	}

	private Plane thePlane;

	private bool active;

	private Vector2 position;

	private float direction;

	private Texture2D mSpriteTexture;

	private int frameWidth = 48;

	private int frameHeight = 48;

	private float animFrame = 0f;

	private float animTimer = 0f;

	private float animSpeed = 0.1f;

	private Mode currentMode = Mode.backFlame;

	private Vector2 origin = new Vector2(24f, 24f);

	public Rectangle sourceRect;

	public Flames(Plane p)
	{
		thePlane = p;
		direction = thePlane.GetDirection();
		position = thePlane.GetPosition();
		animFrame = (int)thePlane.GetAnimFrame();
		sourceRect = new Rectangle(0, 0, frameWidth, frameHeight);
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetFlamesTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, direction, origin, 1.5f, SpriteEffects.None, 0f);
	}

	public void Update(GameTime theGameTime)
	{
		if (active)
		{
			position = thePlane.GetPosition();
			direction = thePlane.GetDirection();
			if (currentMode == Mode.sideFlames)
			{
				animFrame = (int)thePlane.GetAnimFrame();
			}
		}
		else
		{
			position.X = -1000f;
			position.Y = -1000f;
		}
		Animate(theGameTime);
	}

	public void Animate(GameTime theGameTime)
	{
		int num = frameWidth;
		int num2 = frameHeight;
		if (currentMode == Mode.sideFlames)
		{
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer >= animSpeed * 4f)
			{
				animTimer = 0f;
			}
			if (animTimer < animSpeed)
			{
				animFrame = animFrame;
			}
			else if (animTimer < animSpeed * 2f)
			{
				animFrame += 8f;
			}
			else if (animTimer < animSpeed * 3f)
			{
				animFrame += 16f;
			}
			else if (animTimer < animSpeed * 4f)
			{
				animFrame += 24f;
			}
			origin = new Vector2(24f, 24f);
		}
		if (currentMode == Mode.backFlame)
		{
			if (animFrame < 32f || animFrame > 36f)
			{
				animFrame = 32f;
			}
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer > animSpeed)
			{
				animFrame += 1f;
				if (animFrame > 36f)
				{
					animFrame = 32f;
				}
				animTimer = 0f;
			}
			origin = new Vector2(56f, 24f);
		}
		sourceRect = new Rectangle((int)animFrame % 8 * num, (int)animFrame / 8 * num2, frameWidth, frameHeight);
	}

	public bool GetActive()
	{
		return active;
	}

	public void SetActive(bool b)
	{
		active = b;
	}

	public void SetMode(int i)
	{
		if (i == 0)
		{
			currentMode = Mode.sideFlames;
		}
		if (i == 1)
		{
			currentMode = Mode.backFlame;
		}
	}
}
