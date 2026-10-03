using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class VictoryPlane
{
	private GameWorld g;

	private Vector2 position = new Vector2(10000f, 10000f);

	private Vector2 origin = new Vector2(29f, 24f);

	private Vector2 momentum = new Vector2(0f, 0f);

	private Color theColor = Color.White;

	private float direction = 0f;

	private float turnRate = 0f;

	private float fallingSpeed = 0f;

	private float engineSpeed = 0f;

	private float topSpeed = 500f;

	private float topTurnRate = 300f;

	private float TURNINCREMENT = 40f;

	private float ASCENDRATE = 100f;

	private float DESCENDRATE = 400f;

	private float currentAnalogueInput = 0f;

	private int frameWidth = 48;

	private int frameHeight = 48;

	private float animFrame = 0f;

	private float targetAnimFrame = 0f;

	private float animSpeed = 0f;

	private float DEFAULTANIMSPEED = 7f;

	public Rectangle sourceRect;

	public Rectangle cockpitSourceRect;

	public VictorySmokeManager theVictorySmokeManager;

	public VictoryExhaustManager theVictoryExhaustManager;

	private Texture2D mSpriteTexture;

	public VictoryPlane(GameWorld gw)
	{
		g = gw;
		theVictorySmokeManager = new VictorySmokeManager(this);
		theVictoryExhaustManager = new VictoryExhaustManager(this);
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetPlaneTexture();
		theVictorySmokeManager.LoadContent();
		theVictoryExhaustManager.LoadContent();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theVictoryExhaustManager.Draw(theSpriteBatch);
		theVictorySmokeManager.Draw(theSpriteBatch);
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, theColor, direction, origin, 1.5f, SpriteEffects.None, 0f);
		theSpriteBatch.Draw(mSpriteTexture, position, cockpitSourceRect, theColor, direction, origin, 1.5f, SpriteEffects.None, 0f);
	}

	public void Animate(GameTime theGameTime)
	{
		int num = frameWidth;
		int num2 = frameHeight;
		int num3 = 0;
		int num4 = 16;
		if (animFrame < targetAnimFrame)
		{
			animFrame += animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animFrame >= targetAnimFrame)
			{
				animFrame = targetAnimFrame;
			}
		}
		if (animFrame > targetAnimFrame)
		{
			animFrame -= animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animFrame <= targetAnimFrame)
			{
				animFrame = targetAnimFrame;
			}
		}
		sourceRect = new Rectangle(((int)animFrame + num3) % 8 * num, ((int)animFrame + num3) / 8 * num2, frameWidth, frameHeight);
		cockpitSourceRect = new Rectangle(((int)animFrame + num4) % 8 * num, ((int)animFrame + num4) / 8 * num2, frameWidth, frameHeight);
	}

	public void Update(GameTime theGameTime)
	{
		RunEngine(theGameTime);
		Animate(theGameTime);
		UpdatePosition(theGameTime);
		UpdateTurn(theGameTime);
		UpdateDirection(theGameTime);
		theVictorySmokeManager.Update(theGameTime);
		theVictoryExhaustManager.Update(theGameTime);
	}

	public void RunEngine(GameTime theGameTime)
	{
		if (momentum.Y < 0f)
		{
			engineSpeed += ASCENDRATE * momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else
		{
			engineSpeed += DESCENDRATE * momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		if (engineSpeed > topSpeed)
		{
			engineSpeed = topSpeed;
		}
	}

	public void UpdatePosition(GameTime theGameTime)
	{
		momentum.X = (float)Math.Cos(direction);
		momentum.Y = (float)Math.Sin(direction);
		position.Y += momentum.Y * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		position.X += momentum.X * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
	}

	public void UpdateTurn(GameTime theGameTime)
	{
		float num = currentAnalogueInput * topTurnRate;
		if (turnRate > num)
		{
			turnRate -= TURNINCREMENT;
			if (turnRate <= num)
			{
				turnRate = num;
			}
		}
		if (turnRate < num)
		{
			turnRate += TURNINCREMENT;
			if (turnRate >= num)
			{
				turnRate = num;
			}
		}
		if (num == 0f)
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
	}

	public void UpdateDirection(GameTime theGameTime)
	{
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

	public void SetStartValues(Vector2 p, float d, Color c)
	{
		position = p;
		direction = d;
		theColor = c;
		engineSpeed = topSpeed;
		theVictorySmokeManager.Reset();
		theVictorySmokeManager.SetTheColor(theColor);
		SmokeActivated(b: false);
		theVictoryExhaustManager.SetActive(b: true);
		animSpeed = DEFAULTANIMSPEED;
		animFrame = 0f;
		currentAnalogueInput = 0f;
		targetAnimFrame = 0f;
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public float GetDirection()
	{
		return direction;
	}

	public void SmokeActivated(bool b)
	{
		theVictorySmokeManager.SetActive(b);
	}

	public void SetSmokeColor(Color c)
	{
		theVictorySmokeManager.SetTheColor(c);
	}

	public void SetCurrentAnalogueInput(float f)
	{
		currentAnalogueInput = f;
	}

	public void SetAnimFrame(int i)
	{
		animFrame = i;
		targetAnimFrame = i;
	}

	public void SetTargetAnimFrame(int i)
	{
		targetAnimFrame = i;
	}

	public void SetAnimSpeed(float f)
	{
		animSpeed = f;
	}

	public void ShiftPositionY(float f)
	{
		position.Y += f;
	}

	public void SetPositionY(float f)
	{
		position.Y = f;
	}
}
