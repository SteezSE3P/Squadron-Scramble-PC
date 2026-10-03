using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class DropPlane
{
	private GameWorld g;

	private bool active = true;

	private bool launched = false;

	private bool flightComplete = false;

	private Vector2 momentum = new Vector2(0f, 0f);

	private Vector2 origin = new Vector2(0f, 0f);

	private Vector2 position = new Vector2(-7000f, -7000f);

	private float SPEED = 200f;

	private Texture2D mSpriteTexture;

	private Rectangle sourceRect;

	private float animFrame = 0f;

	private float animTimer = 0f;

	private float ANIMSPEED = 0.1f;

	public DropPlane(GameWorld gw)
	{
		g = gw;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetDropPlaneTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		if (active)
		{
			origin = new Vector2(50f, sourceRect.Height / 2);
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
		}
	}

	public void Update(GameTime theGameTime)
	{
		Animate(theGameTime);
		CheckPosition(theGameTime);
	}

	public void Animate(GameTime theGameTime)
	{
		if (animTimer < ANIMSPEED)
		{
			animFrame = 0f;
		}
		else
		{
			animFrame = 1f;
		}
		animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (animTimer > ANIMSPEED * 2f)
		{
			animTimer = 0f;
		}
		if (animFrame == 0f)
		{
			sourceRect = new Rectangle(0, 0, 110, 45);
		}
		else
		{
			sourceRect = new Rectangle(0, 43, 110, 45);
		}
	}

	public void CheckPosition(GameTime theGameTime)
	{
		if (active)
		{
			if (!launched)
			{
				position = new Vector2(-150f, 100f);
				momentum = new Vector2(1f, 0f);
				launched = true;
			}
		}
		else
		{
			position = new Vector2(-7000f, -7000f);
			momentum = new Vector2(0f, 0f);
		}
		if (!flightComplete)
		{
			position.X += SPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else
		{
			position.X = General.GetNextRandom(50, 900);
			position.Y = -50f;
		}
		if (position.X > 1200f)
		{
			flightComplete = true;
		}
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public void SetActive(bool b)
	{
		active = true;
	}

	public void Reset()
	{
		active = false;
		launched = false;
		flightComplete = false;
	}
}
