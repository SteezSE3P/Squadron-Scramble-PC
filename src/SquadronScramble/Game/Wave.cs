using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Wave
{
	private GameWorld g;

	private Vector2 position = new Vector2(0f, 0f);

	private float positionOffsetY = 0f;

	private float rootY = 0f;

	private float speedX = 0f;

	private float waveSinValue = 0f;

	private float WAVESPEED = 0.7f;

	private float WAVEHEIGHT = 8f;

	private Texture2D mSpriteTexture;

	private Rectangle sourceRect = new Rectangle(0, 0, 0, 0);

	private Vector2 origin = new Vector2(0f, 0f);

	public Wave(float x, float y, float dx, float v, GameWorld gw)
	{
		g = gw;
		position.X = x;
		rootY = y;
		speedX = dx;
		waveSinValue = v;
	}

	public void LoadContent(Texture2D theTexture)
	{
		mSpriteTexture = theTexture;
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		sourceRect = new Rectangle(0, 327, 702, 86);
		origin = new Vector2(0f, 0f);
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			theSpriteBatch.Draw(mSpriteTexture, new Vector2(position.X - (float)sourceRect.Width, position.Y), sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(mSpriteTexture, new Vector2(position.X + (float)sourceRect.Width, position.Y), sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			theSpriteBatch.Draw(mSpriteTexture, new Vector2(position.X - (float)sourceRect.Width, position.Y - 33f), sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(mSpriteTexture, new Vector2(position.X, position.Y - 33f), sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(mSpriteTexture, new Vector2(position.X + (float)sourceRect.Width, position.Y - 33f), sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
		else
		{
			theSpriteBatch.Draw(mSpriteTexture, new Vector2(position.X - (float)sourceRect.Width, position.Y - 66f), sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(mSpriteTexture, new Vector2(position.X, position.Y - 66f), sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(mSpriteTexture, new Vector2(position.X + (float)sourceRect.Width, position.Y - 66f), sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
	}

	public void CheckPosition(GameTime theGameTime)
	{
		position.X += speedX * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (position.X > (float)sourceRect.Width)
		{
			position.X -= (float)sourceRect.Width;
		}
		waveSinValue += WAVESPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (waveSinValue >= (float)Math.PI * 2f)
		{
			waveSinValue -= (float)Math.PI * 2f;
		}
		positionOffsetY = rootY + (float)((double)WAVEHEIGHT * Math.Sin(waveSinValue));
		position.Y = positionOffsetY;
	}
}
