using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class RoundResultScreen
{
	private enum State
	{
		off,
		fadingUp,
		displayingResults,
		fadingDown
	}

	private GameWorld g;

	private Vector2 screenPosition = new Vector2(0f, 0f);

	private float timer = 0f;

	private Texture2D backgroundTexture;

	private Texture2D elementsTexture;

	private Color backgroundColor = Color.White;

	public Vector2[] squadronPlatePositionA;

	public Vector2[] squadronPlatePositionB;

	public Rectangle squadronPlateSourceRectA = new Rectangle(0, 0, 509, 132);

	public Rectangle squadronPlateSourceRectB = new Rectangle(510, 0, 507, 132);

	public Vector2[,] squadronTrophyPositions;

	public Vector2 squadronTrophyOrigin = new Vector2(48f, 48f);

	public float[,] squadronTrophyScales;

	public Rectangle squadronTrophySourceRect = new Rectangle(0, 134, 96, 100);

	public Vector2 squadronTrophyEmptyOrigin = new Vector2(38f, 53f);

	public Rectangle squadronTrophyEmptySourceRect = new Rectangle(146, 171, 80, 110);

	public Vector2 squadronTrophyShinePosition = new Vector2(0f, 0f);

	public Vector2 squadronTrophyShineOrigin = new Vector2(18f, 18f);

	public float squadronTrophyShineTimer = 0f;

	public float squadronTrophyShineScale = 0f;

	public float squadronTrophyShineRotation = 0f;

	public Rectangle squadronTrophyShineSourceRect = new Rectangle(105, 185, 37, 37);

	private SpriteFont screenTitleFont;

	private string screenTitleText;

	private Color screenTitleColor;

	private Vector2 screenTitlePosition;

	private Vector2 screenTitleOrigin;

	private float screenTitleScale;

	private SpriteFont squadronFont;

	private string[] squadronNameText;

	private Color[] squadronColor;

	private Vector2[] squadronNamePosition;

	private Vector2[] squadronNameOrigin;

	private float[] squadronNameScale;

	private string[] squadronScoreText;

	private Vector2[] squadronScorePosition;

	private Vector2[] squadronScoreOrigin;

	private float[] squadronScoreScale;

	private SpriteFont combatantFont;

	private string[,] squadronCombatantNameText;

	private Color[,] squadronCombatantNameColor;

	private Vector2[,] squadronCombatantNamePosition;

	private Vector2[,] squadronCombatantNameOrigin;

	private float[,] squadronCombatantNameScale;

	private string[,] squadronCombatantScoreText;

	private Vector2[,] squadronCombatantScorePosition;

	private Vector2[,] squadronCombatantScoreOrigin;

	private float[,] squadronCombatantScoreScale;

	private string[,] squadronCombatantKIAText;

	private Vector2[,] squadronCombatantKIAPosition;

	private Color[,] squadronCombatantKIAColor;

	private bool trophiesAwarded = false;

	private bool initialisePodiums = true;

	private int podiumScoreMovingNow;

	private float podiumXLimit;

	private SpriteFont podiumFont;

	private string[] squadronPodiumText;

	private Color[] squadronPodiumColor;

	private Vector2[] squadronPodiumPosition;

	private Vector2[] squadronPodiumOrigin;

	private float[] squadronPodiumScale;

	private bool tipOn = false;

	private string[] tipString = new string[12];

	private int tipNumber = 0;

	private Vector2 tipPosition = new Vector2(0f, 0f);

	private Vector2 tipOrigin = new Vector2(0f, 0f);

	private SpriteFont tipFont;

	private float tipStartPositionY = 0f;

	private float tipEndPositionY = 0f;

	private float textThrob = 0f;

	private State currentState = State.off;

	private int[] theScores;

	public RoundResultScreen(GameWorld gw)
	{
		g = gw;
		squadronPlatePositionA = new Vector2[g.activeSlots.Length];
		squadronPlatePositionB = new Vector2[g.activeSlots.Length];
		squadronTrophyPositions = new Vector2[g.activeSlots.Length, 3];
		squadronTrophyScales = new float[g.activeSlots.Length, 3];
		squadronNameText = new string[g.activeSlots.Length];
		squadronColor = new Color[g.activeSlots.Length];
		squadronNamePosition = new Vector2[g.activeSlots.Length];
		squadronNameOrigin = new Vector2[g.activeSlots.Length];
		squadronNameScale = new float[g.activeSlots.Length];
		squadronScoreText = new string[g.activeSlots.Length];
		squadronScorePosition = new Vector2[g.activeSlots.Length];
		squadronScoreOrigin = new Vector2[g.activeSlots.Length];
		squadronScoreScale = new float[g.activeSlots.Length];
		squadronCombatantNameText = new string[g.activeSlots.Length, 4];
		squadronCombatantNameColor = new Color[g.activeSlots.Length, 4];
		squadronCombatantNamePosition = new Vector2[g.activeSlots.Length, 4];
		squadronCombatantNameOrigin = new Vector2[g.activeSlots.Length, 4];
		squadronCombatantNameScale = new float[g.activeSlots.Length, 4];
		squadronCombatantScoreText = new string[g.activeSlots.Length, 4];
		squadronCombatantScorePosition = new Vector2[g.activeSlots.Length, 4];
		squadronCombatantScoreOrigin = new Vector2[g.activeSlots.Length, 4];
		squadronCombatantScoreScale = new float[g.activeSlots.Length, 4];
		squadronCombatantKIAText = new string[g.activeSlots.Length, 4];
		squadronCombatantKIAPosition = new Vector2[g.activeSlots.Length, 4];
		squadronCombatantKIAColor = new Color[g.activeSlots.Length, 4];
		podiumXLimit = 135f;
		squadronPodiumText = new string[g.activeSlots.Length];
		squadronPodiumColor = new Color[g.activeSlots.Length];
		squadronPodiumPosition = new Vector2[g.activeSlots.Length];
		squadronPodiumOrigin = new Vector2[g.activeSlots.Length];
		squadronPodiumScale = new float[g.activeSlots.Length];
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			squadronPodiumText[i] = "";
			ref Color reference = ref squadronPodiumColor[i];
			reference = Color.Black;
		}
		tipString[0] = "TIP: Stalled? Point your plane downwards to restart its engine!";
		tipString[1] = "TIP: Plane out of control? Press fire to eject!";
		tipString[2] = "TIP: Keep your pilots alive! Scores carry across sorties.";
		tipString[3] = "TIP: Get altitude! Planes cannot shoot when their wheels are down.";
		tipString[4] = "TIP: Enemies on your tail? Use the clouds to lose them!";
		tipString[5] = "TIP: Aim carefully! Only so many bullets can be fired at once.";
		tipString[6] = "TIP: Do your duty! Pilots MUST fly before returning to barracks.";
		tipString[7] = "TIP: Look out! Falling debris can harm both planes and pilots.";
		tipString[8] = "TIP: High scoring pilots are safest kept in the barracks.";
		tipString[9] = "TIP: Press fire to exit the barracks at the start of a sortie.";
		tipString[10] = "TIP: Get altitude! Planes cannot shoot when their wheels are down.";
		tipString[11] = "TIP: Aim carefully! Only so many bullets can be fired at once.";
		tipFont = g.theFontManager.GetHiScoreFont();
		theScores = new int[g.activeSlots.Length];
	}

	public void LoadContent(ContentManager theContentManager)
	{
		backgroundTexture = TextureManager.GetScreenRoundResultTexture();
		elementsTexture = TextureManager.GetElementsRoundResultTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(origin: new Vector2(0f, 0f), texture: backgroundTexture, position: screenPosition, sourceRectangle: null, color: backgroundColor, rotation: 0f, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		RoundResultScenery.Draw(theSpriteBatch);
		DrawText(theSpriteBatch);
	}

	public void DrawText(SpriteBatch theSpriteBatch)
	{
		SetMainTextPositions();
		Vector2 vector = new Vector2(0f, 0f);
		Color color = new Color(100, 100, 100);
		General.DrawEmbossedString(theSpriteBatch, screenTitleFont, screenTitleText, screenTitlePosition, new Color(screenTitleColor.R - 25, screenTitleColor.G - 25, screenTitleColor.B - 25), screenTitleColor, new Color(screenTitleColor.R - 50, screenTitleColor.G - 50, screenTitleColor.B - 50), 0f, screenTitleOrigin, screenTitleScale, 1, 1f);
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] == null)
			{
				continue;
			}
			theSpriteBatch.Draw(elementsTexture, squadronPlatePositionB[i], squadronPlateSourceRectB, Color.White, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(elementsTexture, squadronPlatePositionA[i], squadronPlateSourceRectA, Color.White, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
			General.DrawEmbossedString(theSpriteBatch, squadronFont, squadronNameText[i], squadronNamePosition[i], new Color(squadronColor[i].R - 25, squadronColor[i].G - 25, squadronColor[i].B - 25), squadronColor[i], new Color(squadronColor[i].R - 50, squadronColor[i].G - 50, squadronColor[i].B - 50), 0f, squadronNameOrigin[i], squadronNameScale[i], 1, 1f);
			General.DrawEmbossedString(theSpriteBatch, squadronFont, squadronScoreText[i], squadronScorePosition[i], new Color(squadronColor[i].R - 25, squadronColor[i].G - 25, squadronColor[i].B - 25), squadronColor[i], new Color(squadronColor[i].R - 50, squadronColor[i].G - 50, squadronColor[i].B - 50), 0f, squadronScoreOrigin[i], squadronScoreScale[i], 1, 1f);
			General.DrawEmbossedString(theSpriteBatch, podiumFont, squadronPodiumText[i], squadronPodiumPosition[i], new Color(squadronPodiumColor[i].R - 25, squadronPodiumColor[i].G - 25, squadronPodiumColor[i].B - 25), squadronPodiumColor[i], new Color(squadronPodiumColor[i].R - 50, squadronPodiumColor[i].G - 50, squadronPodiumColor[i].B - 50), 0f, squadronPodiumOrigin[i], squadronPodiumScale[i], 1, 1f);
			for (int j = 0; j < CustomOptions.GetTrophyLimit(); j++)
			{
				if (g.activeSlots[i].GetCurrentSquadron().GetTrophies() < j + 1)
				{
					theSpriteBatch.Draw(elementsTexture, squadronTrophyPositions[i, j], squadronTrophyEmptySourceRect, Color.White * 0.15f, 0f, squadronTrophyEmptyOrigin, 1f, SpriteEffects.None, 0f);
				}
			}
			if (g.activeSlots[i].GetCurrentSquadron().GetTrophies() > 0)
			{
				for (int j = 0; j < g.activeSlots[i].GetCurrentSquadron().GetTrophies(); j++)
				{
					theSpriteBatch.Draw(elementsTexture, squadronTrophyPositions[i, j], squadronTrophySourceRect, Color.White, 0f, squadronTrophyOrigin, squadronTrophyScales[i, j], SpriteEffects.None, 0f);
				}
				theSpriteBatch.Draw(elementsTexture, squadronTrophyShinePosition, squadronTrophyShineSourceRect, Color.White, squadronTrophyShineRotation, squadronTrophyShineOrigin, squadronTrophyShineScale, SpriteEffects.None, 0f);
			}
			for (int k = 0; k < g.activeSlots[i].GetCurrentSquadron().GetSquadronSectionSize(); k++)
			{
				if (squadronCombatantKIAText[i, k] == "")
				{
					if (k % 2 == 0)
					{
						theSpriteBatch.Draw(elementsTexture, new Vector2(squadronCombatantNamePosition[i, k].X - 8f, squadronCombatantNamePosition[i, k].Y - 6f), new Rectangle(101, 136, 250, 35), new Color(127, 127, 127), 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
					}
					else
					{
						theSpriteBatch.Draw(elementsTexture, new Vector2(squadronCombatantNamePosition[i, k].X - 7f, squadronCombatantNamePosition[i, k].Y - 6f), new Rectangle(101, 136, 250, 35), new Color(127, 127, 127), 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
					}
				}
				theSpriteBatch.DrawString(combatantFont, squadronCombatantNameText[i, k], new Vector2(squadronCombatantNamePosition[i, k].X - 1f, squadronCombatantNamePosition[i, k].Y - 1f), Color.Black, 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantNameText[i, k], new Vector2(squadronCombatantNamePosition[i, k].X + 1f, squadronCombatantNamePosition[i, k].Y - 1f), Color.Black, 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantNameText[i, k], new Vector2(squadronCombatantNamePosition[i, k].X - 1f, squadronCombatantNamePosition[i, k].Y + 1f), Color.Black, 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantNameText[i, k], new Vector2(squadronCombatantNamePosition[i, k].X + 1f, squadronCombatantNamePosition[i, k].Y + 1f), Color.Black, 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantNameText[i, k], squadronCombatantNamePosition[i, k], squadronCombatantNameColor[i, k], 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantKIAText[i, k], new Vector2(squadronCombatantKIAPosition[i, k].X - 1f, squadronCombatantKIAPosition[i, k].Y - 1f), Color.Black, 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantKIAText[i, k], new Vector2(squadronCombatantKIAPosition[i, k].X + 1f, squadronCombatantKIAPosition[i, k].Y - 1f), Color.Black, 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantKIAText[i, k], new Vector2(squadronCombatantKIAPosition[i, k].X - 1f, squadronCombatantKIAPosition[i, k].Y + 1f), Color.Black, 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantKIAText[i, k], new Vector2(squadronCombatantKIAPosition[i, k].X + 1f, squadronCombatantKIAPosition[i, k].Y + 1f), Color.Black, 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantKIAText[i, k], squadronCombatantKIAPosition[i, k], squadronCombatantKIAColor[i, k], 0f, squadronCombatantNameOrigin[i, k], squadronCombatantNameScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantScoreText[i, k], new Vector2(squadronCombatantScorePosition[i, k].X - 1f, squadronCombatantScorePosition[i, k].Y - 1f), Color.Black, 0f, squadronCombatantScoreOrigin[i, k], squadronCombatantScoreScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantScoreText[i, k], new Vector2(squadronCombatantScorePosition[i, k].X + 1f, squadronCombatantScorePosition[i, k].Y - 1f), Color.Black, 0f, squadronCombatantScoreOrigin[i, k], squadronCombatantScoreScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantScoreText[i, k], new Vector2(squadronCombatantScorePosition[i, k].X - 1f, squadronCombatantScorePosition[i, k].Y + 1f), Color.Black, 0f, squadronCombatantScoreOrigin[i, k], squadronCombatantScoreScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantScoreText[i, k], new Vector2(squadronCombatantScorePosition[i, k].X + 1f, squadronCombatantScorePosition[i, k].Y + 1f), Color.Black, 0f, squadronCombatantScoreOrigin[i, k], squadronCombatantScoreScale[i, k], SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(combatantFont, squadronCombatantScoreText[i, k], squadronCombatantScorePosition[i, k], squadronCombatantNameColor[i, k], 0f, squadronCombatantScoreOrigin[i, k], squadronCombatantScoreScale[i, k], SpriteEffects.None, 0f);
			}
		}
		if (g.theSafeArea.GetScreenMode() == 0 || g.theSafeArea.GetScreenMode() == 1)
		{
			tipFont = g.theFontManager.GetHiScoreFont();
		}
		else
		{
			tipFont = g.theFontManager.GetFont();
		}
		tipOrigin = tipFont.MeasureString(tipString[tipNumber]) / 2f;
		if (g.theSafeArea.GetScreenMode() == 0 || g.theSafeArea.GetScreenMode() == 1)
		{
			General.DrawEmbossedString(theSpriteBatch, tipFont, tipString[tipNumber], tipPosition, new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, tipOrigin, 1f, 1, 1f);
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, tipFont, tipString[tipNumber], tipPosition, new Color(255, 255, 255), 0f, tipOrigin, 1f, 1, 1f);
		}
		if (!tipOn)
		{
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				ControlOverlay.DrawAShowTip(theSpriteBatch, 190f, 50f);
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				ControlOverlay.DrawAShowTip(theSpriteBatch, 170f, 30f);
			}
			else
			{
				ControlOverlay.DrawAShowTip(theSpriteBatch, 150f, 0f);
			}
		}
	}

	public void Update(GameTime theGameTime)
	{
		CheckLastGame();
		SetTrophyPositions();
		SetPodiumTextPositions(theGameTime);
		SetTipPosition(theGameTime);
		FadeControl(theGameTime);
		RoundResultScenery.Update(theGameTime);
		CheckControls(theGameTime);
	}

	public void FadeControl(GameTime theGameTime)
	{
		if (timer == 0f)
		{
			SetRoundResultScreen();
			tipOn = false;
			tipNumber = General.GetNextRandom(0, 12);
			if (g.theSafeArea.GetScreenMode() == 0)
			{
				tipStartPositionY = 884f;
				tipEndPositionY = 684f;
			}
			else if (g.theSafeArea.GetScreenMode() == 1)
			{
				tipStartPositionY = 867f;
				tipEndPositionY = 667f;
			}
			else
			{
				tipStartPositionY = 843f;
				tipEndPositionY = 643f;
			}
			tipPosition = new Vector2(640f, tipStartPositionY);
		}
		timer = (timer += (float)theGameTime.ElapsedGameTime.TotalSeconds);
		if ((int)timer == 0)
		{
			g.theScreenFadeOverlay.UnFadeScreen(1f);
		}
		if ((int)timer == 9)
		{
			g.theScreenFadeOverlay.FadeScreen(1f);
			g.theSoundManager.SetMusicFadeSpeed(-0.4f);
		}
		if ((int)timer == 12)
		{
			g.theSoundManager.StopSortieReportMusic();
			timer = 0f;
			textThrob = 0f;
			initialisePodiums = true;
			trophiesAwarded = false;
			CheckForWinner();
		}
	}

	public void CheckLastGame()
	{
		if (!g.theRoundManager.GetIsFinal())
		{
			return;
		}
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetParticipating() && !g.activeSlots[i].GetCurrentSquadron().GetAllPilotsDead())
			{
				g.theWinningSquadronScreen.SetWinningSquadron(g.activeSlots[i].GetCurrentSquadron());
				g.theWinningSquadronScreen.SetWinningSlot(g.activeSlots[i]);
			}
		}
		g.GoToWinningSquadronScreen();
	}

	public void CheckForWinner()
	{
		int num = 0;
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetCurrentSquadron().GetTrophies() == CustomOptions.GetTrophyLimit())
			{
				num++;
				g.theWinningSquadronScreen.SetWinningSquadron(g.activeSlots[i].GetCurrentSquadron());
				g.theWinningSquadronScreen.SetWinningSlot(g.activeSlots[i]);
			}
		}
		if (num == 1)
		{
			g.GoToWinningSquadronScreen();
		}
		else if (num > 1)
		{
			g.theRoundManager.SetIsFinal(b: true);
			g.theRoundManager.StartNewRound();
		}
		else
		{
			g.theRoundManager.StartNewRound();
		}
	}

	public void SetMainTextPositions()
	{
		podiumFont = g.theFontManager.GetRoundFont();
		screenTitleFont = g.theFontManager.GetFontGameHeading2();
		screenTitleText = "SORTIE REPORT";
		screenTitleColor = Color.White;
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			screenTitleFont = g.theFontManager.GetFontGameHeading3();
			screenTitlePosition = new Vector2(640f, 63f);
			screenTitleScale = 1f;
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			screenTitleFont = g.theFontManager.GetFontGameHeading3();
			screenTitlePosition = new Vector2(640f, 78f);
			screenTitleScale = 1f;
		}
		else
		{
			screenTitleFont = g.theFontManager.GetFontGameHeading2();
			screenTitlePosition = new Vector2(640f, 86f);
			screenTitleScale = 0f;
		}
		screenTitleOrigin = screenTitleFont.MeasureString(screenTitleText) / 2f;
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null)
			{
				Squadron currentSquadron = g.activeSlots[i].GetCurrentSquadron();
				squadronFont = g.theFontManager.GetHiScoreFont();
				combatantFont = g.theFontManager.GetFont();
				float num = 484f;
				float num2 = 133f;
				float num3 = 235f;
				float num4 = 248f;
				float num5 = 35f;
				float num6 = ((g.theSafeArea.GetScreenMode() != 0 && g.theSafeArea.GetScreenMode() != 1) ? 107f : 123f);
				float num7;
				if (CustomOptions.GetTrophyLimit() == 3)
				{
					num7 = 215f;
					ref Vector2 reference = ref squadronPlatePositionA[i];
					reference = new Vector2(num7 - 16f, num6 - 8f + (float)i * num2);
					ref Vector2 reference2 = ref squadronPlatePositionB[i];
					reference2 = new Vector2(num7 - 16f + 507f, num6 - 8f + (float)i * num2);
				}
				else if (CustomOptions.GetTrophyLimit() == 2)
				{
					num7 = 265f;
					ref Vector2 reference3 = ref squadronPlatePositionA[i];
					reference3 = new Vector2(num7 - 16f, num6 - 8f + (float)i * num2);
					ref Vector2 reference4 = ref squadronPlatePositionB[i];
					reference4 = new Vector2(num7 - 16f + 407f, num6 - 8f + (float)i * num2);
				}
				else
				{
					num7 = 315f;
					ref Vector2 reference5 = ref squadronPlatePositionA[i];
					reference5 = new Vector2(num7 - 16f, num6 - 8f + (float)i * num2);
					ref Vector2 reference6 = ref squadronPlatePositionB[i];
					reference6 = new Vector2(num7 - 16f + 316f, num6 - 8f + (float)i * num2);
				}
				squadronNameText[i] = g.activeSlots[i].GetSlotFullName();
				ref Color reference7 = ref squadronColor[i];
				reference7 = currentSquadron.GetTheColor();
				ref Vector2 reference8 = ref squadronNamePosition[i];
				reference8 = new Vector2(num7, num6 + 3f + (float)i * num2);
				ref Vector2 reference9 = ref squadronNameOrigin[i];
				reference9 = new Vector2(0f, 0f);
				squadronNameScale[i] = 1f;
				squadronScoreText[i] = currentSquadron.GetScore().ToString();
				if (squadronScoreText[i] == "-1")
				{
					squadronScoreText[i] = "OUT";
				}
				ref Vector2 reference10 = ref squadronScorePosition[i];
				reference10 = new Vector2(num7 + num, num6 + 3f + (float)i * num2);
				Vector2 vector = squadronFont.MeasureString(squadronScoreText[i]);
				ref Vector2 reference11 = ref squadronScoreOrigin[i];
				reference11 = new Vector2(0f + vector.X, 0f);
				squadronScoreScale[i] = 1f;
				for (int j = 0; j < g.activeSlots[i].GetCurrentSquadron().GetSquadronSectionSize(); j++)
				{
					squadronCombatantNameText[i, j] = currentSquadron.GetCombatant(j).GetName();
					if (currentSquadron.GetCombatant(j).GetAlive())
					{
						ref Color reference12 = ref squadronCombatantNameColor[i, j];
						reference12 = Color.White;
						squadronCombatantKIAText[i, j] = "";
						ref Color reference13 = ref squadronCombatantKIAColor[i, j];
						reference13 = Color.White;
					}
					else
					{
						ref Color reference14 = ref squadronCombatantNameColor[i, j];
						reference14 = new Color(30, 30, 30);
						if (!currentSquadron.GetCombatant(j).GetTaken())
						{
							squadronCombatantKIAText[i, j] = "K.I.A.";
							ref Color reference15 = ref squadronCombatantKIAColor[i, j];
							reference15 = Color.Red;
						}
						else
						{
							squadronCombatantKIAText[i, j] = "M.I.A.";
							ref Color reference16 = ref squadronCombatantKIAColor[i, j];
							reference16 = Color.Yellow;
						}
					}
					ref Vector2 reference17 = ref squadronCombatantNamePosition[i, j];
					reference17 = new Vector2(num7 + num4 * (float)(j % 2), num6 + 51f + (float)i * num2 + num5 * (float)(j / 2));
					ref Vector2 reference18 = ref squadronCombatantKIAPosition[i, j];
					reference18 = new Vector2(squadronCombatantNamePosition[i, j].X + 130f, squadronCombatantNamePosition[i, j].Y);
					ref Vector2 reference19 = ref squadronCombatantNameOrigin[i, j];
					reference19 = new Vector2(0f, 0f);
					squadronCombatantNameScale[i, j] = 1f;
					squadronCombatantScoreText[i, j] = currentSquadron.GetCombatant(j).GetScore().ToString();
					ref Vector2 reference20 = ref squadronCombatantScorePosition[i, j];
					reference20 = new Vector2(num7 + num4 * (float)(j % 2) + num3, num6 + 51f + (float)i * num2 + num5 * (float)(j / 2));
					vector = combatantFont.MeasureString(squadronCombatantScoreText[i, j]);
					ref Vector2 reference21 = ref squadronCombatantScoreOrigin[i, j];
					reference21 = new Vector2(0f + vector.X, 0f);
					squadronCombatantScoreScale[i, j] = 1f;
				}
			}
			else
			{
				squadronNameText[i] = "";
				ref Color reference22 = ref squadronColor[i];
				reference22 = Color.White;
				ref Vector2 reference23 = ref squadronNamePosition[i];
				reference23 = new Vector2(-1000f, -1000f);
				ref Vector2 reference24 = ref squadronNameOrigin[i];
				reference24 = new Vector2(0f, 0f);
				squadronNameScale[i] = 0.5f;
			}
		}
	}

	public void SetTrophyPositions()
	{
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null)
			{
				int num = 3;
				for (int j = 0; j < num; j++)
				{
					ref Vector2 reference = ref squadronTrophyPositions[i, j];
					reference = new Vector2(squadronNamePosition[i].X + 568f + (float)(96 * j), squadronNamePosition[i].Y + 54f);
					squadronTrophyScales[i, j] = 1f;
				}
			}
		}
	}

	public void SetPodiumTextPositions(GameTime theGameTime)
	{
		float num = 320f;
		float x = -100f;
		if (initialisePodiums)
		{
			for (int i = 0; i < g.activeSlots.Length; i++)
			{
				ref Vector2 reference = ref squadronPodiumPosition[i];
				reference = new Vector2(x, squadronNamePosition[i].Y + 60f);
			}
			podiumScoreMovingNow = -10;
			initialisePodiums = false;
		}
		for (int i = 0; i < theScores.Length; i++)
		{
			theScores[i] = -1;
		}
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = -1;
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null)
			{
				theScores[i] = g.activeSlots[i].GetCurrentSquadron().GetScore();
			}
			else
			{
				theScores[i] = -1;
			}
		}
		for (int i = 0; i < theScores.Length; i++)
		{
			bool flag = true;
			for (int j = 0; j < theScores.Length; j++)
			{
				if (theScores[i] < theScores[j])
				{
					flag = false;
				}
			}
			if (flag)
			{
				num2 = theScores[i];
			}
		}
		for (int i = 0; i < theScores.Length; i++)
		{
			bool flag2 = true;
			for (int j = 0; j < theScores.Length; j++)
			{
				if (theScores[i] < theScores[j] && theScores[j] != num2)
				{
					flag2 = false;
				}
			}
			if (flag2 && theScores[i] != num2)
			{
				num3 = theScores[i];
			}
		}
		for (int i = 0; i < theScores.Length; i++)
		{
			bool flag3 = true;
			for (int j = 0; j < theScores.Length; j++)
			{
				if (theScores[i] < theScores[j] && theScores[j] != num2 && theScores[j] != num3)
				{
					flag3 = false;
				}
			}
			if (flag3 && theScores[i] != num2 && theScores[i] != num3)
			{
				num4 = theScores[i];
			}
		}
		for (int i = 0; i < theScores.Length; i++)
		{
			bool flag4 = true;
			for (int j = 0; j < theScores.Length; j++)
			{
				if (theScores[i] < theScores[j] && theScores[j] != num2 && theScores[j] != num3 && theScores[j] != num4)
				{
					flag4 = false;
				}
			}
			if (flag4 && theScores[i] != num2 && theScores[i] != num3 && theScores[i] != num4)
			{
				num5 = theScores[i];
			}
		}
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null)
			{
				if (g.activeSlots[i].GetCurrentSquadron().GetScore() == num2)
				{
					squadronPodiumText[i] = "1st";
					ref Color reference2 = ref squadronPodiumColor[i];
					reference2 = Color.Red;
				}
				else if (g.activeSlots[i].GetCurrentSquadron().GetScore() == num3)
				{
					squadronPodiumText[i] = "2nd";
					ref Color reference3 = ref squadronPodiumColor[i];
					reference3 = Color.White;
				}
				else if (g.activeSlots[i].GetCurrentSquadron().GetScore() == num4)
				{
					squadronPodiumText[i] = "3rd";
					ref Color reference4 = ref squadronPodiumColor[i];
					reference4 = Color.White;
				}
				else
				{
					squadronPodiumText[i] = "4th";
					ref Color reference5 = ref squadronPodiumColor[i];
					reference5 = Color.White;
				}
				ref Vector2 reference6 = ref squadronPodiumOrigin[i];
				reference6 = g.theFontManager.GetRoundFont().MeasureString(squadronPodiumText[i]) / 2f;
				squadronPodiumScale[i] = 0.75f;
			}
		}
		if (podiumScoreMovingNow == -10 && !g.theRoundManager.GetIsFinal())
		{
			podiumScoreMovingNow = num5;
			g.theSoundManager.PodiumResultSound();
		}
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetCurrentSquadron().GetScore() == podiumScoreMovingNow)
			{
				flag6 = true;
				squadronPodiumPosition[i].X += num * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				if (squadronPodiumPosition[i].X >= podiumXLimit)
				{
					squadronPodiumPosition[i].X = podiumXLimit;
					flag5 = true;
				}
			}
		}
		if ((flag5 || !flag6) && !g.theRoundManager.GetIsFinal())
		{
			if (podiumScoreMovingNow == num2)
			{
				flag7 = true;
			}
			if (podiumScoreMovingNow == num3)
			{
				podiumScoreMovingNow = num2;
			}
			if (podiumScoreMovingNow == num4)
			{
				podiumScoreMovingNow = num3;
			}
			if (podiumScoreMovingNow == num5)
			{
				podiumScoreMovingNow = num4;
			}
			if (!flag7)
			{
				g.theSoundManager.PodiumResultSound();
			}
		}
		if (flag7)
		{
			textThrob += 0.2f;
		}
		if (textThrob > (float)Math.PI * 2f)
		{
			textThrob = 0f;
		}
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetCurrentSquadron().GetScore() == num2)
			{
				squadronPodiumScale[i] = 0.75f + (float)Math.Sin(textThrob) / 20f;
				squadronTrophyShineTimer += 3.5f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				if (squadronTrophyShineTimer > (float)Math.PI)
				{
					squadronTrophyShineTimer = (float)Math.PI;
				}
				squadronTrophyShineScale = (float)Math.Sin(squadronTrophyShineTimer) * 1.5f;
				if (squadronTrophyShineScale <= 0f)
				{
					squadronTrophyShineScale = 0f;
				}
				squadronTrophyShineRotation = squadronTrophyShineTimer * 20f / ((float)Math.PI * 2f);
				if (g.activeSlots[i].GetCurrentSquadron().GetTrophies() > 0)
				{
					squadronTrophyScales[i, g.activeSlots[i].GetCurrentSquadron().GetTrophies() - 1] += (float)Math.Sin(textThrob) / 20f;
					squadronTrophyShinePosition = squadronTrophyPositions[i, g.activeSlots[i].GetCurrentSquadron().GetTrophies() - 1] + new Vector2(20f, -20f);
				}
				if (!trophiesAwarded && flag7)
				{
					g.activeSlots[i].GetCurrentSquadron().IncrementTrophies(1);
					squadronTrophyShineTimer = 0f;
				}
			}
		}
		if (flag7)
		{
			trophiesAwarded = true;
		}
	}

	public void SetTipPosition(GameTime theGameTime)
	{
		if (tipOn && tipPosition.Y > tipEndPositionY)
		{
			tipPosition.Y -= 200f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (tipPosition.Y <= tipEndPositionY)
			{
				tipPosition.Y = tipEndPositionY;
			}
		}
	}

	public void CheckControls(GameTime theGameTime)
	{
		if (Guide.IsVisible || tipOn)
		{
			return;
		}
		for (int i = 0; i < g.GetNumberOfControllers(); i++)
		{
			if (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed())
			{
				g.theSoundManager.MenuSelectSound();
				tipOn = true;
			}
		}
	}

	public void SetRoundResultScreen()
	{
		if (g.GetCurrentLevel() == 1)
		{
			backgroundColor = new Color(82, 85, 255);
		}
		if (g.GetCurrentLevel() == 2 || g.GetCurrentLevel() == 10)
		{
			backgroundColor = new Color(180, 100, 0);
		}
		if (g.GetCurrentLevel() == 3)
		{
			backgroundColor = new Color(131, 185, 252);
		}
		if (g.GetCurrentLevel() == 4)
		{
			backgroundColor = new Color(39, 39, 39);
		}
		if (g.GetCurrentLevel() == 5 || g.GetCurrentLevel() == 10)
		{
			backgroundColor = new Color(108, 174, 255);
		}
		if (g.GetCurrentLevel() != 10)
		{
			g.theSoundManager.StartSortieReportMusic();
		}
	}

	public void Reset()
	{
		timer = 0f;
	}
}
