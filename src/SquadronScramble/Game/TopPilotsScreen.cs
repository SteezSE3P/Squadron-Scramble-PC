using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class TopPilotsScreen
{
	private GameWorld g;

	private static SquadronMember[] allSquadronMembers;

	private static Squadron[] allSquadronMemberSquadrons;

	private static Slot[] allSquadronMemberSlots;

	private bool timerOn = false;

	private float timer = 0f;

	private float yReferencePoint = 750f;

	private int scorers = 0;

	private int TABLESIZE = 160;

	private int MEMBERLISTSIZE = 160;

	private float NAMESPEED = 40f;

	private float SEPARATION = 150f;

	private string tempString = "";

	private string titleString = "ROLL OF DISTINCTION";

	private SpriteFont font;

	private SpriteFont hiScoreFont;

	private SpriteFont hiScoreTitleFont;

	private Texture2D backgroundTexture;

	private Texture2D elementsTexture;

	private Vector2 screenPosition = new Vector2(0f, 0f);

	private Vector2 distinctionPosition = new Vector2(0f, 0f);

	private int NUMBEROFCLOUDS = 3;

	private Vector2[] cloudPosition;

	private float[] cloudSpeed;

	public TopPilotsScreen(GameWorld gw)
	{
		g = gw;
		allSquadronMembers = new SquadronMember[MEMBERLISTSIZE];
		allSquadronMemberSquadrons = new Squadron[MEMBERLISTSIZE];
		allSquadronMemberSlots = new Slot[MEMBERLISTSIZE];
		cloudPosition = new Vector2[NUMBEROFCLOUDS];
		cloudSpeed = new float[NUMBEROFCLOUDS];
		ref Vector2 reference = ref cloudPosition[0];
		reference = new Vector2(200f, 150f);
		ref Vector2 reference2 = ref cloudPosition[1];
		reference2 = new Vector2(800f, 750f);
		ref Vector2 reference3 = ref cloudPosition[2];
		reference3 = new Vector2(-1500f, 950f);
		cloudSpeed[0] = 20f;
		cloudSpeed[1] = 40f;
		cloudSpeed[2] = 50f;
	}

	public void LoadContent(ContentManager theContentManager)
	{
		backgroundTexture = theContentManager.Load<Texture2D>("ScreenTopPilots");
		elementsTexture = theContentManager.Load<Texture2D>("ElementsScreenTopPilots");
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(origin: new Vector2(0f, 0f), texture: backgroundTexture, position: screenPosition, sourceRectangle: null, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		DrawClouds(theSpriteBatch);
		DrawText(theSpriteBatch);
	}

	public void DrawClouds(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(200f, 170f);
		Rectangle value = new Rectangle(0, 200, 415, 175);
		for (int i = 0; i < NUMBEROFCLOUDS; i++)
		{
			theSpriteBatch.Draw(elementsTexture, cloudPosition[i], value, Color.White, 0f, origin, 3f, SpriteEffects.None, 0f);
		}
	}

	public void DrawText(SpriteBatch theSpriteBatch)
	{
		hiScoreFont = g.theFontManager.GetHiScoreFont();
		hiScoreTitleFont = g.theFontManager.GetHiScoreTitleFont();
		font = g.theFontManager.GetFont();
		float num = -23f;
		float num2 = -8f;
		float x = 640f;
		float num3 = 470f;
		float num4 = 300f;
		float num5 = -26f;
		float num6 = 350f;
		float num7 = 26f;
		Vector2 origin;
		for (int i = 0; i < TABLESIZE; i++)
		{
			if (allSquadronMembers[i] == null || allSquadronMembers[i].GetScore() <= 0)
			{
				continue;
			}
			Rectangle value = new Rectangle(0, 0, 393, 85);
			origin = new Vector2(0f, 0f);
			if (!(yReferencePoint + (float)i * SEPARATION > distinctionPosition.Y))
			{
				continue;
			}
			theSpriteBatch.Draw(elementsTexture, new Vector2(num3 + num, yReferencePoint + num2 + (float)i * SEPARATION), value, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(origin: new Vector2(font.MeasureString(allSquadronMemberSlots[i].GetSlotFullName()).X / 2f, 0f), spriteFont: font, text: allSquadronMemberSlots[i].GetSlotFullName(), position: new Vector2(x, yReferencePoint + (float)i * SEPARATION), color: allSquadronMemberSlots[i].GetSlotColorA(), rotation: 0f, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			origin = new Vector2(0f, 0f);
			tempString = allSquadronMembers[i].GetName();
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 - 1f, yReferencePoint - 1f + num7 + (float)i * SEPARATION), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 + 1f, yReferencePoint - 1f + num7 + (float)i * SEPARATION), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 - 1f, yReferencePoint + 1f + num7 + (float)i * SEPARATION), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 + 1f, yReferencePoint + 1f + num7 + (float)i * SEPARATION), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3, yReferencePoint + num7 + (float)i * SEPARATION), Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			if (!allSquadronMembers[i].GetAlive())
			{
				if (!allSquadronMembers[i].GetTaken())
				{
					theSpriteBatch.DrawString(font, "K.I.A.", new Vector2(num3 + num4, yReferencePoint + num5 + num7 + (float)i * SEPARATION), Color.Red, 0f, origin, 1f, SpriteEffects.None, 0f);
				}
				else
				{
					theSpriteBatch.DrawString(font, "M.I.A.", new Vector2(num3 + num4, yReferencePoint + num5 + num7 + (float)i * SEPARATION), Color.Yellow, 0f, origin, 1f, SpriteEffects.None, 0f);
				}
			}
			tempString = allSquadronMembers[i].GetScore().ToString();
			origin = new Vector2(hiScoreFont.MeasureString(tempString).X, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 - 1f + num6, yReferencePoint - 1f + num7 + (float)i * SEPARATION), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 + 1f + num6, yReferencePoint - 1f + num7 + (float)i * SEPARATION), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 - 1f + num6, yReferencePoint + 1f + num7 + (float)i * SEPARATION), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 + 1f + num6, yReferencePoint + 1f + num7 + (float)i * SEPARATION), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(hiScoreFont, tempString, new Vector2(num3 + num6, yReferencePoint + num7 + (float)i * SEPARATION), Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
		Rectangle value2 = new Rectangle(0, 185, 400, 10);
		origin = new Vector2(value2.Width / 2, 0f);
		for (int i = 10; i < 20; i++)
		{
			theSpriteBatch.Draw(elementsTexture, new Vector2(646f, i * value2.Height), value2, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
		value2 = new Rectangle(0, 185, 400, 2);
		origin = new Vector2(value2.Width / 2, 0f);
		for (int i = 0; i < 10; i++)
		{
			theSpriteBatch.Draw(elementsTexture, new Vector2(646f, 200 + i * value2.Height), value2, new Color(255f, 255f, 255f, 1f - 0.1f * (float)i), 0f, origin, 1f, SpriteEffects.None, 0f);
		}
		if (g.theSafeArea.GetScreenMode() == 2)
		{
			distinctionPosition = new Vector2(650f, 120f);
		}
		else
		{
			distinctionPosition = new Vector2(650f, 100f);
		}
		Rectangle value3 = new Rectangle(0, 90, 818, 91);
		theSpriteBatch.Draw(origin: new Vector2(value3.Width / 2, value3.Height / 2), texture: elementsTexture, position: distinctionPosition + new Vector2(1f, -4f), sourceRectangle: value3, color: Color.White, rotation: 0f, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		origin = new Vector2(hiScoreTitleFont.MeasureString(titleString).X / 2f, hiScoreTitleFont.MeasureString(titleString).Y / 2f);
		theSpriteBatch.DrawString(hiScoreTitleFont, titleString, distinctionPosition + new Vector2(-1f, -1f), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(hiScoreTitleFont, titleString, distinctionPosition + new Vector2(1f, -1f), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(hiScoreTitleFont, titleString, distinctionPosition + new Vector2(-1f, 1f), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(hiScoreTitleFont, titleString, distinctionPosition + new Vector2(1f, 1f), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(hiScoreTitleFont, titleString, distinctionPosition, Color.Gold, 0f, origin, 1f, SpriteEffects.None, 0f);
	}

	public void Update(GameTime theGameTime)
	{
		UpdateScenery(theGameTime);
		FadeControl(theGameTime);
		CheckControls(theGameTime);
	}

	public void UpdateScenery(GameTime theGameTime)
	{
		for (int i = 0; i < NUMBEROFCLOUDS; i++)
		{
			cloudPosition[i].X += cloudSpeed[i] * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (cloudPosition[i].X > 2000f)
			{
				cloudPosition[i].X = -700f;
			}
		}
	}

	public void CheckControls(GameTime theGameTime)
	{
		for (int i = 0; i < g.theControllerMenuManager.Length; i++)
		{
			if (g.theControllerMenuManager[i].CheckButtonStartPressed())
			{
				g.theScreenFadeOverlay.FadeScreen(1f);
				timerOn = true;
			}
		}
	}

	public void FadeControl(GameTime theGameTime)
	{
		if ((int)timer == 0)
		{
			SetAllSquadronMembers();
			if (scorers > 0)
			{
				g.theScreenFadeOverlay.UnFadeScreen(1f);
				timerOn = false;
				yReferencePoint = 750f;
				g.theSoundManager.StartRollCallMusic();
				timer = 1f;
			}
			else
			{
				timer = 5f;
			}
		}
		yReferencePoint = (yReferencePoint -= NAMESPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds);
		if (yReferencePoint < 200f - (float)scorers * SEPARATION && !timerOn)
		{
			g.theScreenFadeOverlay.FadeScreen(1f);
			timerOn = true;
		}
		if (timerOn)
		{
			timer = (timer += (float)theGameTime.ElapsedGameTime.TotalSeconds);
		}
		if (timer > 1f)
		{
			g.theSoundManager.SetMusicFadeSpeed(-0.4f);
		}
		if ((int)timer == 5)
		{
			timer = 0f;
			g.ResetAll();
			g.theScreenFadeOverlay.UnFadeScreen(1f);
			g.GoToFrontEnd();
			g.theSoundManager.StopRollCallMusic();
		}
	}

	public void SetAllSquadronMembers()
	{
		for (int i = 0; i < MEMBERLISTSIZE; i++)
		{
			allSquadronMembers[i] = null;
			allSquadronMemberSquadrons[i] = null;
		}
		int num = 0;
		scorers = 0;
		for (int j = 0; j < Squadron.GetSquadronSize(); j++)
		{
			for (int i = 0; i < g.activeSlots.Length; i++)
			{
				Slot slot = g.activeSlots[i];
				if (slot != null && slot.GetCurrentSquadron() != null)
				{
					Squadron currentSquadron = slot.GetCurrentSquadron();
					allSquadronMembers[num] = currentSquadron.GetMember(j);
					allSquadronMemberSquadrons[num] = currentSquadron;
					allSquadronMemberSlots[num] = slot;
					if (allSquadronMembers[num].GetScore() > 0)
					{
						scorers++;
					}
					num++;
				}
			}
		}
		SortAllSquadronMembers();
	}

	public void SortAllSquadronMembers()
	{
		for (int num = allSquadronMembers.Length - 1; num >= 0; num--)
		{
			for (int i = 1; i <= num; i++)
			{
				if (allSquadronMembers[i] != null && (allSquadronMembers[i - 1] == null || allSquadronMembers[i - 1].GetScore() < allSquadronMembers[i].GetScore()))
				{
					SquadronMember squadronMember = allSquadronMembers[i - 1];
					Squadron squadron = allSquadronMemberSquadrons[i - 1];
					Slot slot = allSquadronMemberSlots[i - 1];
					allSquadronMembers[i - 1] = allSquadronMembers[i];
					allSquadronMemberSquadrons[i - 1] = allSquadronMemberSquadrons[i];
					allSquadronMemberSlots[i - 1] = allSquadronMemberSlots[i];
					allSquadronMembers[i] = squadronMember;
					allSquadronMemberSquadrons[i] = squadron;
					allSquadronMemberSlots[i] = slot;
				}
			}
		}
	}
}
