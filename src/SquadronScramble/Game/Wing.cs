using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Wing
{
	private GameWorld g;

	private bool active = false;

	private Vector2 position = new Vector2(-3000f, -3000f);

	private Vector2 momentum = new Vector2(0f, 0f);

	private Vector2 origin = new Vector2(12f, 12f);

	private float rotation = 0f;

	private float fallingSpeed = 0f;

	private float ROTATESPEED = 400f;

	private float TOPSPEED = 300f;

	private float SPINSPEED = 5f;

	private float DESCENDRATE = 400f;

	public float FALLRATE = 0.025f;

	private Texture2D mSpriteTexture;

	private WingDustCloud theDustCloud;

	private int frameWidth = 24;

	private int frameHeight = 24;

	private float animFrame = 0f;

	private Rectangle sourceRect;

	public Wing(GameWorld gw)
	{
		g = gw;
		position = new Vector2(-3000f, -3000f);
		active = false;
		theDustCloud = new WingDustCloud(g, this);
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetWingTexture();
		theDustCloud.LoadContent();
	}

	public void Draw(SpriteBatch theSpriteBatch, Plane p)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, p.GetThePilot().GetCurrentColor(), rotation, origin, 1.5f, SpriteEffects.None, 0f);
		theDustCloud.Draw(theSpriteBatch);
	}

	public void Update(GameTime theGameTime)
	{
		Animate(theGameTime);
		CheckPosition(theGameTime);
		theDustCloud.Update(theGameTime);
	}

	public void Animate(GameTime theGameTime)
	{
		if ((double)animFrame < 7.9)
		{
			animFrame += SPINSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		if ((double)animFrame >= 7.9)
		{
			animFrame = 0f;
		}
		sourceRect = new Rectangle((int)animFrame % 8 * frameWidth, (int)animFrame / 8 * frameHeight, frameWidth, frameHeight);
	}

	public void CheckPosition(GameTime theGameTime)
	{
		if (active)
		{
			if (momentum.X < 0f)
			{
				momentum.X += 0.01f;
			}
			else if (momentum.X > 0f)
			{
				momentum.X -= 0.01f;
			}
			else
			{
				momentum.X = 0f;
			}
			momentum.Y += FALLRATE;
			rotation = (rotation + ROTATESPEED * ((float)Math.PI / 180f * (float)theGameTime.ElapsedGameTime.TotalSeconds)) % ((float)Math.PI * 2f);
			fallingSpeed += DESCENDRATE * momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (fallingSpeed > TOPSPEED)
			{
				fallingSpeed = TOPSPEED;
			}
			position += momentum * fallingSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (position.Y >= Level.GetAbsoluteGroundY() + 5f)
			{
				HitGround(theGameTime);
				if (g.GetCurrentLevel() != 2)
				{
					g.theSoundManager.HitSound();
				}
			}
		}
		else
		{
			position.X = -3000f;
			position.Y = -3000f;
		}
	}

	public void HitGround(GameTime theGameTime)
	{
		if (momentum.X < 0f)
		{
			theDustCloud.TriggerDustCloud(theGameTime, position, b: false);
		}
		else
		{
			theDustCloud.TriggerDustCloud(theGameTime, position, b: true);
		}
		WingOff();
	}

	public void WingBroken(Plane p)
	{
		active = true;
		position = p.GetPosition();
		momentum = new Vector2(p.GetMomentum().X / 2f, p.GetMomentum().Y);
		fallingSpeed = p.GetEngineSpeed();
		rotation = p.GetDirection();
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public Vector2 GetMomentum()
	{
		return momentum;
	}

	public void WingOff()
	{
		active = false;
		momentum = new Vector2(0f, 0f);
		position.X = -3000f;
		position.Y = -3000f;
	}
}
