using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Snow
{
	private bool active = true;

	private Vector2 position = new Vector2(0f, 0f);

	private Color theColor = Color.White;

	private float speed = 0f;

	private float scale = 0f;

	private float timer = 0f;

	private bool isFlipped = false;

	private Texture2D mSpriteTexture;

	public Snow(float x, float y, float s, float sc, Color c, bool f)
	{
		position.X = x;
		position.Y = y;
		speed = s;
		scale = sc;
		theColor = c;
		isFlipped = f;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetSnowTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(0f, 0f);
		if (!isFlipped)
		{
			theSpriteBatch.Draw(mSpriteTexture, position, null, theColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}
		else
		{
			theSpriteBatch.Draw(mSpriteTexture, position, null, theColor, 0f, origin, scale, SpriteEffects.FlipHorizontally, 0f);
		}
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
	}

	public void CheckPosition(GameTime theGameTime)
	{
		float num = 200f;
		position.Y += speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (position.Y > Level.GetAbsoluteGroundY() + num)
		{
			position.Y = Level.GetAbsoluteGroundY() + num - 1000f;
		}
		timer += speed / 50f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		float num2 = (float)Math.Sin(timer) / 10f;
		position.X += num2;
		if (timer >= (float)Math.PI * 2f)
		{
			timer = 0f;
		}
	}

	public void SetUp(bool b, float sp, float sc)
	{
		active = b;
		speed = sp;
		scale = sc;
	}
}
