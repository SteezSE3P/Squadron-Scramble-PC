using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Cloud
{
	private bool active = false;

	private Vector2 position = new Vector2(0f, 0f);

	private Vector2 centreColPosition = new Vector2(-10000f, -10000f);

	private Vector2 frontColPosition = new Vector2(-10000f, -10000f);

	private Vector2 rearColPosition = new Vector2(-10000f, -10000f);

	private Vector2 frontTipColPosition = new Vector2(-10000f, -10000f);

	private Vector2 rearTipColPosition = new Vector2(-10000f, -10000f);

	private float centreColSize = 50f;

	private float frontColSize = 45f;

	private float rearColSize = 45f;

	private float frontTipColSize = 25f;

	private float rearTipColSize = 25f;

	private float speed = 0f;

	private float scale = 0f;

	private Color theColor = Color.White;

	private float opacity = 1f;

	private float xLimit = 0f;

	private float xStart = 0f;

	private Texture2D mSpriteTexture;

	public Cloud(float x, float y, float s, float f)
	{
		position.X = x;
		position.Y = y;
		speed = s;
		scale = f;
	}

	public void LoadContent(int i)
	{
		if (i == 0)
		{
			mSpriteTexture = TextureManager.GetCloudTexture();
		}
		else
		{
			mSpriteTexture = TextureManager.GetSnowCloudTexture();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(200f, 100f);
		if (active)
		{
			theSpriteBatch.Draw(mSpriteTexture, position, null, theColor * opacity, 0f, origin, scale, SpriteEffects.None, 0f);
		}
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
	}

	public void CheckPosition(GameTime theGameTime)
	{
		position.X += speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (position.X < xStart)
		{
			position.X = xLimit;
		}
		if (position.X > xLimit)
		{
			position.X = xStart;
		}
		if (scale <= 0.6f)
		{
			centreColSize = 33.333332f;
			frontColSize = 30f;
			rearColSize = 30f;
			frontTipColSize = 16.666666f;
			rearTipColSize = 16.666666f;
			centreColPosition = position + new Vector2(0f, -10f);
			frontColPosition = position + new Vector2(30f, -3.3333333f);
			rearColPosition = position + new Vector2(-30f, -3.3333333f);
			frontTipColPosition = position + new Vector2(66.666664f, 13.333333f);
			rearTipColPosition = position + new Vector2(-66.666664f, 13.333333f);
		}
		else
		{
			centreColSize = 50f;
			frontColSize = 45f;
			rearColSize = 45f;
			frontTipColSize = 25f;
			rearTipColSize = 25f;
			centreColPosition = position + new Vector2(0f, -15f);
			frontColPosition = position + new Vector2(45f, -5f);
			rearColPosition = position + new Vector2(-45f, -5f);
			frontTipColPosition = position + new Vector2(100f, 20f);
			rearTipColPosition = position + new Vector2(-100f, 20f);
		}
	}

	public void SetUp(bool b, float sp, float sc)
	{
		active = b;
		speed = sp;
		scale = sc;
		xLimit = 1700f;
		xStart = -300f;
	}

	public void SetHeavyCloud(float x, float y, float s)
	{
		position.X = x;
		position.Y = y;
		speed = s;
		active = true;
		xLimit = 1550f;
		xStart = -300f;
	}

	public void SetRandomPosition()
	{
		position.X = General.GetNextRandom(0, 1280);
	}

	public void SetColor(Color c, float f)
	{
		theColor = c;
		opacity = f;
	}

	public bool CheckCollisionBoxes(Vector2 p)
	{
		if (active)
		{
			if (General.CheckDistance(p, centreColPosition) < centreColSize)
			{
				return true;
			}
			if (General.CheckDistance(p, frontColPosition) < frontColSize)
			{
				return true;
			}
			if (General.CheckDistance(p, rearColPosition) < rearColSize)
			{
				return true;
			}
			if (General.CheckDistance(p, frontTipColPosition) < frontTipColSize)
			{
				return true;
			}
			if (General.CheckDistance(p, rearTipColPosition) < rearTipColSize)
			{
				return true;
			}
		}
		return false;
	}

	public void SetActive(bool b)
	{
		active = b;
	}
}
