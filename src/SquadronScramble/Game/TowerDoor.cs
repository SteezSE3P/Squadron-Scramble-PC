using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class TowerDoor
{
	private GameWorld g;

	private Vector2 position = new Vector2(0f, 0f);

	private int doorWidth;

	private bool busy;

	private int currentPilot = -1;

	private int[] startQueue;

	private int frameWidth = 72;

	private int frameHeight = 72;

	private float animFrame = 0f;

	private float frameOffset = 0f;

	private int doorOpenSpeed = 16;

	private Texture2D mSpriteTexture;

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetDoorTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(frameWidth / 2, frameHeight / 2);
		int num = frameWidth;
		int num2 = frameHeight;
		theSpriteBatch.Draw(sourceRectangle: new Rectangle((int)(animFrame + frameOffset) % 8 * num, (int)(animFrame + frameOffset) / 8 * num2, frameWidth, frameHeight), texture: mSpriteTexture, position: position, color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
	}

	public TowerDoor(GameWorld gw)
	{
		g = gw;
		busy = false;
		doorWidth = 20;
		startQueue = new int[g.pilots.Length];
		for (int i = 0; i < startQueue.Length; i++)
		{
			startQueue[i] = -1;
		}
	}

	public void Update(GameTime theGameTime, int l)
	{
		UpdateDoorPosition(theGameTime, l);
		RemoveFromStartQueue();
	}

	public void UpdateDoorPosition(GameTime theGameTime, int l)
	{
		if (l == 1)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(891f, 674f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(855f, 641f);
			}
			else
			{
				position = new Vector2(821f, 609f);
			}
			frameOffset = 0f;
		}
		if (l == 2)
		{
			position.X = Level.GetCarrierPosition().X - 40f;
			position.Y = Level.GetCurrentCarrierPositionOffset().Y - 31f;
			frameOffset = 0f;
		}
		if (l == 3)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(80f, 673f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(110f, 640f);
			}
			else
			{
				position = new Vector2(140f, 608f);
			}
			frameOffset = 16f;
		}
		if (l == 4)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(47f, 674f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(77f, 641f);
			}
			else
			{
				position = new Vector2(107f, 609f);
			}
			frameOffset = 0f;
		}
		if (l == 5)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				position = new Vector2(849f, 674f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				position = new Vector2(813f, 641f);
			}
			else
			{
				position = new Vector2(779f, 609f);
			}
			frameOffset = 16f;
		}
		if (l == 10)
		{
			position = new Vector2(20000f, 20000f);
			position = Level.GetDropPlane().GetPosition();
			frameOffset = 0f;
		}
	}

	public void AddToStartQueue(int i)
	{
		bool flag = false;
		for (int j = 0; j < startQueue.Length; j++)
		{
			if (startQueue[j] == i)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			int k;
			for (k = 0; startQueue[k] != -1; k++)
			{
			}
			startQueue[k] = i;
		}
	}

	public void RemoveFromStartQueue()
	{
		if (!busy && startQueue[0] != -1)
		{
			g.pilots[startQueue[0]].SetStateRequestExitingTower();
			currentPilot = startQueue[0];
			busy = true;
			for (int i = 0; i < startQueue.Length - 1; i++)
			{
				startQueue[i] = startQueue[i + 1];
			}
			startQueue[startQueue.Length - 1] = -1;
		}
	}

	public void SetBusy(bool b)
	{
		busy = b;
	}

	public void SetCurrentPilot(int i)
	{
		currentPilot = i;
	}

	public int GetCurrentPilot()
	{
		return currentPilot;
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

	public void Reset()
	{
		for (int i = 0; i < startQueue.Length - 1; i++)
		{
			startQueue[i] = -1;
		}
	}

	public void SetAnimFrame(float f)
	{
		animFrame = f;
	}
}
