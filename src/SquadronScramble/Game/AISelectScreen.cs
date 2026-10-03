using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class AISelectScreen
{
	private GameWorld g;

	private Vector2 screenPosition = new Vector2(0f, 0f);

	private Rectangle[,] participantPositions = new Rectangle[4, 4];

	private Texture2D backgroundTexture;

	private Texture2D elementsTexture;

	public AISelectScreen(GameWorld gw)
	{
		g = gw;
	}

	public void LoadContent(ContentManager theContentManager)
	{
		backgroundTexture = theContentManager.Load<Texture2D>("ScreenAISelect");
		elementsTexture = theContentManager.Load<Texture2D>("ElementsAISelect");
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(origin: new Vector2(0f, 0f), texture: backgroundTexture, position: screenPosition, sourceRectangle: null, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		DrawLevelElements(theSpriteBatch);
	}

	public void DrawLevelElements(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(0f, 0f);
		for (int i = 0; i < g.theSlots.Length; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(200 + 200 * i + 50 * (j % 2), 200 + 50 * (j / 2)), participantPositions[i, j], Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public void Update(GameTime theGameTime)
	{
		CheckInput(theGameTime);
		UpdatePositions();
	}

	public void CheckInput(GameTime theGameTime)
	{
		for (int i = 0; i < g.GetNumberOfControllers(); i++)
		{
			if (g.theControllerMenuManager[i].CheckButtonStartPressed())
			{
				g.StartTheGame();
			}
		}
	}

	public void UpdatePositions()
	{
		for (int i = 0; i < g.theSlots.Length; i++)
		{
			int num = 0;
			for (int j = 0; j < g.pilots.Length; j++)
			{
				if (g.pilots[j].GetParticipant() != null)
				{
					if (g.pilots[j].GetCurrentSquadron() == g.theSlots[i].GetCurrentSquadron() && g.pilots[j].GetParticipant() is Player)
					{
						ref Rectangle reference = ref participantPositions[i, num];
						reference = new Rectangle(g.pilots[j].GetParticipant().GetCurrentControllerPosition() * 32, g.pilots[j].GetParticipant().GetCurrentPlayer() * 32, 32, 32);
						num++;
					}
					if (g.pilots[j].GetCurrentSquadron() == g.theSlots[i].GetCurrentSquadron() && g.pilots[j].GetParticipant() is AIPlayer)
					{
						ref Rectangle reference2 = ref participantPositions[i, num];
						reference2 = new Rectangle(g.pilots[j].GetParticipant().GetCurrentControllerPosition() * 32, 128, 32, 32);
						num++;
					}
				}
			}
		}
	}
}
