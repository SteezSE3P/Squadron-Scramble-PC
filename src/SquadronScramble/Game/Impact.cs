using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Impact
{
	private Vector2 position = new Vector2(-200f, -200f);

	private Vector2 origin = new Vector2(24f, 24f);

	private bool active;

	private string impactType = "";

	private float rotation = 0f;

	private int shade = 255;

	private int frameWidth = 48;

	private int frameHeight = 48;

	private float animFrame = 0f;

	private float animTimer = 0f;

	private Rectangle sourceRect;

	private float animSpeed = 12f;

	private float rotationSpeed = (float)Math.PI / 100f;

	private Texture2D mSpriteTexture;

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetImpactTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, new Color(shade, shade, shade), rotation, origin, 1.5f, SpriteEffects.None, 0f);
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
			if (animTimer == 0f)
			{
				if (impactType != "GROUND")
				{
					rotation = (float)Math.PI / (float)nextRandom;
				}
				else
				{
					rotation = 0f;
				}
			}
			if (animTimer < 1f)
			{
				animTimer += animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				rotation += rotationSpeed;
			}
			if (animTimer >= 1f)
			{
				animFrame = 0f;
				animTimer = 0f;
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

	public void SetActive(bool b, Vector2 v, string st, int s)
	{
		active = b;
		position = v;
		shade = s;
		impactType = st;
		if (impactType == "SHOT")
		{
			animFrame = 0f;
		}
		if (impactType == "GROUND")
		{
			animFrame = 8f;
		}
		animTimer = 0f;
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
