using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class DustCloud
{
	private GameWorld g;

	private Plane thePlane;

	private Vector2 position;

	private Vector2 origin;

	public bool dustCloudTriggered;

	public float animFrame;

	public Rectangle sourceRect;

	private bool facingRight;

	private bool isNew;

	private int frameWidth = 96;

	private int frameHeight = 96;

	private int animFrames = 12;

	private Color dustColor = Color.White;

	private int animSpeed = 15;

	private float dustCloudRadius = 30f;

	private Texture2D mSpriteTexture;

	public DustCloud(GameWorld gw, Plane p)
	{
		g = gw;
		thePlane = p;
		position = new Vector2(100f, 100f);
		dustCloudTriggered = false;
		animFrame = 0f;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetDustCloudTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		if (thePlane.GetThePilot().IsParticipating())
		{
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, dustColor * 0.75f, 0f, origin, 0.75f, SpriteEffects.None, 0f);
		}
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
		Animate(theGameTime);
	}

	public void TriggerDustCloud(GameTime theGameTime, Vector2 p, bool b)
	{
		dustCloudTriggered = true;
		facingRight = b;
		isNew = true;
		if (g.GetCurrentLevel() == 1)
		{
			dustColor = new Color(80, 70, 52);
		}
		else if (g.GetCurrentLevel() == 2)
		{
			dustColor = new Color(70, 70, 70);
		}
		else if (g.GetCurrentLevel() == 3)
		{
			dustColor = new Color(255, 255, 255);
		}
		else if (g.GetCurrentLevel() == 4)
		{
			dustColor = new Color(80, 70, 52);
		}
		else
		{
			dustColor = new Color(200, 150, 100);
		}
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
			dustCloudTriggered = false;
		}
	}

	public void CheckPosition(GameTime theGameTime)
	{
		if (!dustCloudTriggered)
		{
			position.X = -100f;
			position.Y = -100f;
		}
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public float GetDustCloudRadius()
	{
		return dustCloudRadius;
	}

	public bool GetIsNew()
	{
		return isNew;
	}
}
