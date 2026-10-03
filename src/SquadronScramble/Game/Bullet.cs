using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Bullet
{
	private GameWorld g;

	private Vector2 position = new Vector2(100f, 100f);

	private Vector2 origin = new Vector2(5f, 5f);

	private float direction = 0f;

	private bool bulletFired = false;

	private bool bulletActive = false;

	private float bulletLife = 0f;

	private Vector2 momentum = new Vector2(0f, 0f);

	private float bulletSpeed = 600f;

	private float lifeSpan = 1.8f;

	private Texture2D mSpriteTexture;

	private BulletDustCloud theDustCloud;

	public Bullet(GameWorld gw)
	{
		g = gw;
		position = new Vector2(-100f, -100f);
		bulletFired = false;
		bulletActive = false;
		theDustCloud = new BulletDustCloud(g, this);
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetBulletTexture();
		theDustCloud.LoadContent();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, null, Color.White, 1f, origin, 1f, SpriteEffects.None, 0f);
		theDustCloud.Draw(theSpriteBatch);
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
		theDustCloud.Update(theGameTime);
	}

	public void FireBullet(GameTime theGameTime, float d, Vector2 p)
	{
		bulletFired = true;
		bulletActive = true;
		bulletLife = lifeSpan;
		position = p;
		direction = d;
		g.theSoundManager.BulletSound();
	}

	public void CheckPosition(GameTime theGameTime)
	{
		if (bulletFired)
		{
			bulletLife -= (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		if (!bulletActive)
		{
			position.X = -100f;
			position.Y = -100f;
		}
		else
		{
			momentum.X = (float)Math.Cos(direction);
			momentum.Y = (float)Math.Sin(direction);
			position += momentum * bulletSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		if (position.Y >= Level.GetAbsoluteGroundY() + 5f)
		{
			HitGround(theGameTime);
			g.theSoundManager.BulletHitGroundSound();
		}
		if (bulletLife <= 0f)
		{
			bulletFired = false;
			bulletActive = false;
			position.X = -100f;
			position.Y = -100f;
		}
	}

	public void HitGround(GameTime theGameTime)
	{
		if (direction > (float)Math.PI / 2f && (double)direction < 4.71238911151886)
		{
			theDustCloud.TriggerDustCloud(theGameTime, position, b: false);
		}
		else
		{
			theDustCloud.TriggerDustCloud(theGameTime, position, b: true);
		}
		BulletOff();
	}

	public void BulletOff()
	{
		bulletActive = false;
		momentum = new Vector2(0f, 0f);
		position.X = -100f;
		position.Y = -100f;
	}

	public bool getBulletFired()
	{
		return bulletFired;
	}

	public bool getBulletActive()
	{
		return bulletActive;
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public Texture2D GetMSpriteTexture()
	{
		return mSpriteTexture;
	}

	public Vector2 GetMomentum()
	{
		return momentum;
	}
}
