using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class VictoryFume
{
	private VictoryPlane theVictoryPlane;

	private Vector2 position = new Vector2(-200f, -200f);

	private Vector2 origin = new Vector2(24f, 24f);

	private bool active;

	private float rotation = 0f;

	private int frameWidth = 48;

	private int frameHeight = 48;

	private float animFrame = 0f;

	private Rectangle sourceRect;

	private float animSpeed = 8f;

	private float rotationSpeed = (float)Math.PI / 100f;

	private Texture2D mSpriteTexture;

	public VictoryFume(VictoryPlane p)
	{
		theVictoryPlane = p;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetSmokeTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, new Color(255, 255, 255) * 0.05f, rotation, origin, 0.5f, SpriteEffects.None, 0f);
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
				position = new Vector2(theVictoryPlane.GetPosition().X + (float)Math.Cos(theVictoryPlane.GetDirection()) * -35f, theVictoryPlane.GetPosition().Y + (float)Math.Sin(theVictoryPlane.GetDirection()) * -35f);
				rotation = (float)Math.PI / (float)nextRandom;
			}
			if (animFrame < 9f)
			{
				animFrame += animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				rotation += rotationSpeed;
			}
			if (animFrame >= 9f)
			{
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
}
