using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class NamePlate
{
	private GameWorld g;

	private Vector2 position = new Vector2(0f, 0f);

	private float animFrame;

	public Rectangle sourceRect;

	private float[] controllerIconFlash;

	private float flashTimer = 0f;

	private float FLASHTIME = 50f;

	private Texture2D mNamePlateTexture;

	private Texture2D mNamePlateElementsTexture;

	public NamePlate(GameWorld gw)
	{
		g = gw;
		controllerIconFlash = new float[g.pilots.Length];
	}

	public void LoadContent(ContentManager theContentManager)
	{
		mNamePlateTexture = theContentManager.Load<Texture2D>("NamePlateTexturedBlurred");
		mNamePlateElementsTexture = theContentManager.Load<Texture2D>("ElementsNamePlate");
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			position = new Vector2(962f, 0f);
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			position = new Vector2(929f, 0f);
		}
		else
		{
			position = new Vector2(892f, -30f);
		}
		theSpriteBatch.Draw(origin: new Vector2(0f, 0f), texture: mNamePlateTexture, position: position, sourceRectangle: null, color: Color.White, rotation: 0f, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		DrawClockBox(theSpriteBatch);
		DrawSlotBoxes(theSpriteBatch);
		DrawControllerIcons(theSpriteBatch);
		DrawTrophies(theSpriteBatch);
	}

	public void Update(GameTime theGameTime)
	{
		UpdateControllerFlashes(theGameTime);
	}

	public void DrawClockBox(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(0f, 0f);
		Vector2 vector = new Vector2(position.X + 2f, position.Y + 1f);
		sourceRect = new Rectangle(111, 179, 319, 90);
		Color clockColor = g.theInGameScreenText.GetClockColor();
		theSpriteBatch.Draw(mNamePlateElementsTexture, vector, sourceRect, clockColor, 0f, origin, 1f, SpriteEffects.None, 0f);
	}

	public void DrawSlotBoxes(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(0f, 0f);
		Vector2 vector = new Vector2(0f, 0f);
		sourceRect = new Rectangle(111, 0, 318, 151);
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetParticipating())
			{
				theSpriteBatch.Draw(position: new Vector2(position.X + 0f, position.Y + 91f + (float)(i * 149)), texture: mNamePlateElementsTexture, sourceRectangle: sourceRect, color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
		}
	}

	public void DrawControllerIcons(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(0f, 0f);
		Color color = new Color(60, 60, 60);
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] == null || !g.activeSlots[i].GetParticipating())
			{
				continue;
			}
			for (int j = 0; j < g.activeSlots[i].GetCurrentSquadron().GetSquadronSectionSize(); j++)
			{
				Vector2 vector = new Vector2(position.X + 10f, position.Y + 124f + (float)(i * 149) + (float)(j * 27));
				if (g.activeSlots[i].GetCurrentSquadron().GetCombatant(j).GetAlive())
				{
					sourceRect = new Rectangle(121, 151, 298, 27);
					if (!g.activeSlots[i].GetCurrentSquadron().GetCombatant(j).GetDying())
					{
						theSpriteBatch.Draw(mNamePlateElementsTexture, vector, sourceRect, Color.Gray, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
				}
				for (int k = 0; k < g.pilots.Length; k++)
				{
					if (g.pilots[k].GetCurrentSquadron() == null || g.pilots[k].GetCurrentSquadronMember() != g.activeSlots[i].GetCurrentSquadron().GetCombatant(j))
					{
						continue;
					}
					if (g.activeSlots[i].GetCurrentSquadron().GetCombatant(j).GetAlive())
					{
						sourceRect = new Rectangle(121, 151, 298, 27);
						theSpriteBatch.Draw(mNamePlateElementsTexture, vector, sourceRect, g.pilots[k].GetCurrentColor(), 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					if (g.activeSlots[i].GetCurrentSquadron().GetCombatant(j).GetDying())
					{
						sourceRect = new Rectangle(121, 151, 298, 27);
						if ((int)controllerIconFlash[k] % 2 == 0)
						{
							theSpriteBatch.Draw(mNamePlateElementsTexture, vector, sourceRect, Color.Red, 0f, origin, 1f, SpriteEffects.None, 0f);
						}
					}
					if (g.activeSlots[i].GetCurrentSquadron().GetCombatant(j).GetAlive() || g.activeSlots[i].GetCurrentSquadron().GetCombatant(j).GetDying())
					{
						if (g.pilots[k].GetParticipantAI() != null)
						{
							sourceRect = new Rectangle(0, 128, 32, 32);
						}
						else
						{
							sourceRect = new Rectangle(g.pilots[k].GetParticipant().GetCurrentControllerPosition() * 32, g.pilots[k].GetParticipant().GetCurrentPlayer() * 32, 32, 32);
						}
						Vector2 vector2 = new Vector2(position.X + 15f, position.Y + 121f + (float)(i * 149) + (float)(j * 27));
						if ((int)controllerIconFlash[k] % 2 == 0)
						{
							theSpriteBatch.Draw(mNamePlateElementsTexture, vector2, sourceRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}
	}

	public void DrawTrophies(SpriteBatch theSpriteBatch)
	{
		Vector2 vector = new Vector2(position.X + 17f, position.Y + 100f);
		Vector2 origin = new Vector2(0f, 0f);
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] == null || !g.activeSlots[i].GetParticipating())
			{
				continue;
			}
			int trophies = g.activeSlots[i].GetCurrentSquadron().GetTrophies();
			for (int j = 0; j < CustomOptions.GetTrophyLimit(); j++)
			{
				Rectangle value = new Rectangle(6, 177, 19, 21);
				if (trophies < j + 1)
				{
					value = new Rectangle(37, 177, 19, 21);
				}
				theSpriteBatch.Draw(mNamePlateElementsTexture, vector + new Vector2(26 * j, 149 * i), value, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public void UpdateControllerFlashes(GameTime theGameTime)
	{
		for (int i = 0; i < g.pilots.Length; i++)
		{
			controllerIconFlash[i] -= 6f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (controllerIconFlash[i] < 0f)
			{
				controllerIconFlash[i] = 0f;
			}
		}
	}

	public void SetControllerFlashTime(int p, float f)
	{
		controllerIconFlash[p] = f;
	}

	public bool ControllerIconIsFlashing(Pilot p)
	{
		int num = 0;
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (g.pilots[i] != null && g.pilots[i] == p)
			{
				num = i;
			}
		}
		if ((int)controllerIconFlash[num] % 2 == 0)
		{
			return false;
		}
		return true;
	}

	public Vector2 GetPosition()
	{
		return position;
	}
}
