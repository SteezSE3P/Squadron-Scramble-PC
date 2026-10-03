using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class VictorySmoke
{
	private VictoryPlane theVictoryPlane;

	private Vector2 position = new Vector2(-200f, -200f);

	private Vector2 origin = new Vector2(24f, 24f);

	private Color theColor = Color.White;

	private bool active;

	private float rotation = 0f;

	private float alpha = 1f;

	private float alphaSpeed = 0.1f;

	private int frameWidth = 48;

	private int frameHeight = 48;

	private float animFrame = 0f;

	private Rectangle sourceRect;

	private float animSpeed = 13f;

	private float rotationSpeed = (float)Math.PI / 100f;

	private Texture2D mSpriteTexture;

	public VictorySmoke(VictoryPlane p)
	{
		theVictoryPlane = p;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetSmokeTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, theColor * alpha, rotation, origin, 1.5f, SpriteEffects.None, 0f);
	}

	public void Update(GameTime theGameTime)
	{
		Animate(theGameTime);
	}

	public void Animate(GameTime theGameTime)
	{
		int nextRandom = General.GetNextRandom(0, 9);
		if (active)
		{
			if (animFrame == 0f)
			{
				position = theVictoryPlane.GetPosition() + new Vector2((float)Math.Cos(theVictoryPlane.GetDirection()) * -35f, (float)Math.Sin(theVictoryPlane.GetDirection()) * -35f);
				rotation = (float)Math.PI / (float)nextRandom;
				alpha = 0.75f;
			}
			if (animFrame < 4f)
			{
				animFrame += animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				rotation += rotationSpeed;
			}
			if (animFrame >= 4f)
			{
				alpha -= alphaSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				rotation += rotationSpeed;
			}
			if (alpha <= 0f)
			{
				alpha = 0f;
				animFrame = 0f;
				position.X = -200f;
				position.Y = -200f;
				active = false;
			}
		}
		sourceRect = new Rectangle((int)animFrame % 8 * frameWidth, (int)animFrame / 8 * frameHeight, frameWidth, frameHeight);
	}

	public bool GetActive()
	{
		return active;
	}

	public void SetActive(bool b)
	{
		active = b;
	}

	public void SetPositionX(float f)
	{
		position.X = f;
	}

	public void SetPositionY(float f)
	{
		position.Y = f;
	}

	public void SetTheColor(Color c)
	{
		theColor = c;
	}

	public void Reset()
	{
		alpha = 0f;
		animFrame = 0f;
		position.X = -200f;
		position.Y = -200f;
		active = false;
	}
}
