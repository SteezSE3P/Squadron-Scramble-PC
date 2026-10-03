using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Bomb
{
	private GameWorld g;

	private bool active = false;

	private Vector2 position = new Vector2(-300f, -300f);

	private Vector2 momentum = new Vector2(0f, 0f);

	private Vector2 origin = new Vector2(12f, 12f);

	private float rotation = 0f;

	private float fallingSpeed = 0f;

	private float ROTATESPEED = 500f;

	private float TOPSPEED = 300f;

	private float SPINSPEED = 7f;

	private float DESCENDRATE = 400f;

	public float FALLRATE = 0.025f;

	private Texture2D mSpriteTexture;

	private GroundExplosion theGroundExplosion;

	private int frameWidth = 24;

	private int frameHeight = 24;

	private float animFrame = 0f;

	private Rectangle sourceRect;

	public Bomb(GameWorld gw)
	{
		g = gw;
		theGroundExplosion = new GroundExplosion(g);
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetBombTexture();
		theGroundExplosion.LoadContent();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, rotation, origin, 1.25f, SpriteEffects.None, 0f);
		theGroundExplosion.Draw(theSpriteBatch);
	}

	public void Update(GameTime theGameTime)
	{
		Animate(theGameTime);
		CheckPosition(theGameTime);
		theGroundExplosion.Update(theGameTime);
	}

	public void Animate(GameTime theGameTime)
	{
		if ((double)animFrame < 7.9)
		{
			animFrame += SPINSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
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
			fallingSpeed += DESCENDRATE * momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (fallingSpeed > TOPSPEED)
			{
				fallingSpeed = TOPSPEED;
			}
			position += momentum * fallingSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (position.Y >= Level.GetAbsoluteGroundY())
			{
				active = false;
				if (momentum.X > 0f)
				{
					theGroundExplosion.TriggerExplosion(theGameTime, position, b: true);
				}
				else
				{
					theGroundExplosion.TriggerExplosion(theGameTime, position, b: false);
				}
			}
		}
		else
		{
			position.X = -300f;
			position.Y = -300f;
		}
	}

	public void BombDropped(SpecialBonus s)
	{
		active = true;
		position = s.GetPosition() + new Vector2(0f, 10f);
		momentum = new Vector2(s.GetMomentum().X, 0.5f);
		fallingSpeed = 100f;
		rotation = 0f;
	}

	public GroundExplosion GetTheGroundExplosion()
	{
		return theGroundExplosion;
	}

	public Vector2 GetMomentum()
	{
		return momentum;
	}
}
