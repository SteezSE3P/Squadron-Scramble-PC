using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Ignition
{
	private Plane thePlane;

	private Vector2 position = new Vector2(-200f, -200f);

	private Vector2 origin = new Vector2(24f, 24f);

	private bool active;

	private int frameWidth = 48;

	private int frameHeight = 48;

	private float animFrame = 0f;

	private Rectangle sourceRect;

	private float animSpeed = 35f;

	private Texture2D mSpriteTexture;

	public Ignition(Plane p)
	{
		thePlane = p;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetIgnitionTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, new Color(255, 255, 255), thePlane.GetDirection(), origin, 1.5f, SpriteEffects.None, 0f);
	}

	public void Update(GameTime theGameTime)
	{
		Animate(theGameTime);
	}

	public void Animate(GameTime theGameTime)
	{
		if (active)
		{
			position = new Vector2(thePlane.GetPosition().X + (float)Math.Cos(thePlane.GetDirection()) * -38f, thePlane.GetPosition().Y + (float)Math.Sin(thePlane.GetDirection()) * -38f);
			if (animFrame < 5f)
			{
				animFrame += animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (animFrame >= 5f)
			{
				animFrame = 0f;
				position.X = -200f;
				position.Y = -200f;
				active = false;
			}
		}
		else
		{
			animFrame = 0f;
			position.X = -200f;
			position.Y = -200f;
			active = false;
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
}
