using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Missile
{
	private GameWorld g;

	private Vector2 position = new Vector2(-100f, -100f);

	private Vector2 momentum = new Vector2(0f, 0f);

	private float direction = 0f;

	private float missileLife;

	private bool missileFired;

	private float animTimer;

	private int animFrame;

	private Rectangle sourceRect;

	private int frameWidth = 20;

	private int frameHeight = 20;

	private Vector2 origin = new Vector2(10f, 10f);

	public static float TOPTURNRATE = 300f;

	public float TURNINCREMENT = 30f;

	private float turnRate = 0f;

	private Plane planeTarget;

	public float missileSpeed = 440f;

	public float lifeSpan = 10f;

	private MissileExhaustManager theMissileExhaustManager;

	private Explosion theExplosion;

	private Texture2D mSpriteTexture;

	public Missile(GameWorld gw)
	{
		g = gw;
		position = new Vector2(-100f, -100f);
		missileFired = false;
		theMissileExhaustManager = new MissileExhaustManager(this);
		theExplosion = new Explosion(g);
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetMissileTexture();
		theMissileExhaustManager.LoadContent();
		theExplosion.LoadContent();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theMissileExhaustManager.Draw(theSpriteBatch);
		theExplosion.Draw(theSpriteBatch);
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, direction, origin, 1.5f, SpriteEffects.None, 0f);
	}

	public void Animate(GameTime theGameTime)
	{
		float num = 0.25f;
		animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (animTimer < num)
		{
			animFrame = 0;
		}
		else
		{
			animFrame = 1;
		}
		if (animTimer >= num * 2f)
		{
			animFrame = 0;
			animTimer = 0f;
		}
		sourceRect = new Rectangle(animFrame * frameWidth, 0, frameWidth, frameHeight);
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
		Animate(theGameTime);
		theMissileExhaustManager.Update(theGameTime);
		theExplosion.Update(theGameTime);
	}

	public void AnalogueLeft(float f, GameTime theGameTime)
	{
		float num = f * TOPTURNRATE;
		if (turnRate > num)
		{
			turnRate -= TURNINCREMENT;
		}
	}

	public void AnalogueRight(float f, GameTime theGameTime)
	{
		float num = f * TOPTURNRATE;
		if (turnRate < num)
		{
			turnRate += TURNINCREMENT;
		}
	}

	public void NoTurn(GameTime theGameTime)
	{
		if (turnRate < 0f)
		{
			turnRate += TURNINCREMENT;
		}
		if (turnRate > 0f)
		{
			turnRate -= TURNINCREMENT;
		}
		if (turnRate > 0f - TURNINCREMENT && turnRate < TURNINCREMENT)
		{
			turnRate = 0f;
		}
	}

	public void FireMissile(GameTime theGameTime, float d, Vector2 p)
	{
		missileFired = true;
		missileLife = lifeSpan;
		position = p + new Vector2(0f, -18f);
		direction = d;
		g.theSoundManager.MissileLaunchSound();
		g.theSoundManager.missilePitch(this, 1f);
		g.theSoundManager.StartMissile(this);
		theMissileExhaustManager.SetActive(b: true);
	}

	public void CheckPosition(GameTime theGameTime)
	{
		if (missileFired)
		{
			SelectPlaneTarget(theGameTime);
			AimAtTarget(theGameTime);
			momentum.X = (float)Math.Cos(direction);
			momentum.Y = (float)Math.Sin(direction);
			position += momentum * missileSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			missileLife -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (missileLife <= 0f || position.Y > Level.GetAbsoluteGroundY())
			{
				theExplosion.TriggerExplosion(theGameTime, position, 0.5f);
				MissileOff();
			}
			if (position.X < g.theSafeArea.GetPlaneXMinLimit() - 500f || position.X > g.theSafeArea.GetPlaneXMaxLimit() + 500f || position.Y < g.theSafeArea.GetStallCeiling() - 500f)
			{
				MissileOff();
			}
			direction = (direction + turnRate * ((float)Math.PI / 180f * (float)theGameTime.ElapsedGameTime.TotalSeconds)) % ((float)Math.PI * 2f);
			if (direction < 0f)
			{
				direction += (float)Math.PI * 2f;
			}
			if (direction > (float)Math.PI * 2f)
			{
				direction -= (float)Math.PI * 2f;
			}
		}
	}

	public void MissileOff()
	{
		missileLife = 0f;
		missileFired = false;
		position.X = -100f;
		position.Y = -100f;
		theMissileExhaustManager.SetActive(b: false);
		g.theSoundManager.StopMissile(this);
	}

	public void AimAtTarget(GameTime theGameTime)
	{
		if (planeTarget != null)
		{
			if (!General.IsDirectionSideClockwise(position, GetTargetPosition(theGameTime), direction, 3.1415927410125732))
			{
				AnalogueLeft((float)(0.0 - General.RelativeAgnosticNormalisedAngle(position, GetTargetPosition(theGameTime), direction, 1.5707963705062866)), theGameTime);
			}
			if (General.IsDirectionSideClockwise(position, GetTargetPosition(theGameTime), direction, 3.1415927410125732))
			{
				AnalogueRight((float)General.RelativeAgnosticNormalisedAngle(position, GetTargetPosition(theGameTime), direction, 1.5707963705062866), theGameTime);
			}
		}
		else
		{
			NoTurn(theGameTime);
		}
	}

	public Vector2 GetTargetPosition(GameTime theGameTime)
	{
		Vector2 result = new Vector2(0f, 0f);
		if (planeTarget != null)
		{
			return planeTarget.GetPosition();
		}
		return result;
	}

	public void SelectPlaneTarget(GameTime theGameTime)
	{
		Plane plane = null;
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (g.pilots[i].GetParticipant() == null && g.pilots[i].GetParticipantAI() == null)
			{
				continue;
			}
			if (g.pilots[i].GetAvailablePlane(0).GetActive())
			{
				if (plane == null)
				{
					plane = g.pilots[i].GetAvailablePlane(0);
				}
				if (General.CheckDistance(position, g.pilots[i].GetAvailablePlane(0).GetPosition()) < General.CheckDistance(position, plane.GetPosition()))
				{
					plane = g.pilots[i].GetAvailablePlane(0);
				}
			}
			if (g.pilots[i].GetAvailablePlane(1).GetActive())
			{
				if (plane == null)
				{
					plane = g.pilots[i].GetAvailablePlane(1);
				}
				if (General.CheckDistance(position, g.pilots[i].GetAvailablePlane(1).GetPosition()) < General.CheckDistance(position, plane.GetPosition()))
				{
					plane = g.pilots[i].GetAvailablePlane(1);
				}
			}
		}
		planeTarget = plane;
	}

	public bool GetMissileFired()
	{
		return missileFired;
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public Vector2 GetMomentum()
	{
		return momentum;
	}

	public float GetDirection()
	{
		return direction;
	}

	public Explosion GetTheExplosion()
	{
		return theExplosion;
	}
}
