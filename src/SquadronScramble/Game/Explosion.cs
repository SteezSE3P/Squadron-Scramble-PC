using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Explosion
{
	private GameWorld g;

	private Vector2 position;

	private bool explosionTriggered;

	private float animFrame;

	private Rectangle sourceRect;

	private float scale;

	private int frameWidth = 96;

	private int frameHeight = 96;

	private int animFrames = 12;

	private int animSpeed = 15;

	private Texture2D mSpriteTexture;

	public Explosion(GameWorld gw)
	{
		g = gw;
		position = new Vector2(100f, 100f);
		explosionTriggered = false;
		animFrame = 0f;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetExplosionTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(origin: new Vector2(48f, 48f), texture: mSpriteTexture, position: position, sourceRectangle: sourceRect, color: Color.White, rotation: 0f, scale: scale, effects: SpriteEffects.None, layerDepth: 0f);
	}

	public void Update(GameTime theGameTime)
	{
		CheckPosition(theGameTime);
		Animate(theGameTime);
	}

	public void TriggerExplosion(GameTime theGameTime, Vector2 p, float s)
	{
		explosionTriggered = true;
		g.theSoundManager.ExplosionSound();
		animFrame = 0f;
		position = p;
		scale = s;
	}

	public void Animate(GameTime theGameTime)
	{
		int num = frameWidth;
		sourceRect = new Rectangle(y: (!(animFrame < 6f)) ? 96 : 0, x: (int)animFrame % 6 * num, width: frameWidth, height: frameHeight);
		animFrame += (float)animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (animFrame >= (float)animFrames)
		{
			explosionTriggered = false;
		}
	}

	public void CheckPosition(GameTime theGameTime)
	{
		if (!explosionTriggered)
		{
			position.X = -100f;
			position.Y = -100f;
			animFrame = 0f;
		}
	}
}
