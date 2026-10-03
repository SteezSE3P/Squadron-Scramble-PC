using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class HangarDoor
{
	private GameWorld g;

	private Vector2 position;

	private int doorWidth;

	private bool busy;

	private int frameWidth = 72;

	private int frameHeight = 72;

	private float animFrame = 0f;

	private float frameOffset = 0f;

	private int doorOpenSpeed = 16;

	private Texture2D mSpriteTexture;

	public HangarDoor(GameWorld gw)
	{
		g = gw;
		position = new Vector2(0f, 0f);
		busy = false;
		doorWidth = 20;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetDoorTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(frameWidth / 2, frameHeight / 2);
		int num = frameWidth;
		int num2 = frameHeight;
		Rectangle value = new Rectangle((int)(animFrame + frameOffset) % 8 * num, (int)(animFrame + frameOffset) / 8 * num2, frameWidth, frameHeight);
		if (g.GetCurrentLevel() != 2)
		{
			theSpriteBatch.Draw(mSpriteTexture, position, value, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
	}

	public void Update(GameTime theGameTime, int i)
	{
		if (i == 1)
		{
			doorWidth = 20;
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(65f, 675f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(95f, 642f);
			}
			else
			{
				position = new Vector2(122f, 610f);
			}
			frameOffset = 0f;
		}
		if (i == 2)
		{
			doorWidth = 70;
			position = new Vector2(Level.GetCarrierPosition().X + 260f, Level.GetCarrierPosition().Y - 10f);
			frameOffset = 0f;
		}
		if (i == 3)
		{
			doorWidth = 20;
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(912f, 674f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(882f, 642f);
			}
			else
			{
				position = new Vector2(852f, 610f);
			}
			frameOffset = 0f;
		}
		if (i == 4)
		{
			doorWidth = 20;
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(910f, 674f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(880f, 641f);
			}
			else
			{
				position = new Vector2(850f, 609f);
			}
			frameOffset = 0f;
		}
		if (i == 5)
		{
			doorWidth = 20;
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(73f, 674f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(103f, 641f);
			}
			else
			{
				position = new Vector2(130f, 609f);
			}
			frameOffset = 0f;
		}
		if (i == 10)
		{
			doorWidth = 20;
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(75f, 675f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(104f, 642f);
			}
			else
			{
				position = new Vector2(133f, 610f);
			}
			frameOffset = 0f;
		}
	}

	public void SetBusy(bool b)
	{
		busy = b;
	}

	public float GetPositionX()
	{
		return position.X;
	}

	public float GetPositionY()
	{
		return position.Y;
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public int GetDoorWidth()
	{
		return doorWidth;
	}

	public int GetDoorOpenSpeed()
	{
		return doorOpenSpeed;
	}

	public bool GetBusy()
	{
		return busy;
	}

	public void SetAnimFrame(float f)
	{
		animFrame = f;
	}
}
