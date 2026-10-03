using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class ControllerSelectScreen
{
	private GameWorld g;

	private Vector2 screenPosition = new Vector2(0f, 0f);

	private int phase = 0;

	private float arrowFlashTimer = 0f;

	private int CONTROLLERLIMIT = 4;

	private int PARTICIPANTLIMIT = 8;

	private float NEUTRALPOSITIONX = 300f;

	private float NEUTRALPOSITIONY = 170f;

	private float CONTROLLERXOFFSET = 250f;

	private float CONTROLLERYOFFSET = 53f;

	private float ANALOGUESELECT = 0.8f;

	private float ANALOGUEDESELECT = 0.2f;

	private int ICONDISTANCEX = 86;

	private int ICONDISTANCEY = 53;

	private int ICONWIDTH = 82;

	private int ICONHEIGHT = 48;

	private Vector2 cloudPosition = new Vector2(500f, 100f);

	private float CLOUDSPEED = 20f;

	private bool[] controllerSplit;

	private bool[] controllerFull;

	private int[] controllerSquadron;

	private int[] altControllerSquadron;

	private Vector2[] controllerPosition;

	private Vector2[] altControllerPosition;

	private bool[] isAIPosition = new bool[8];

	private int controllerAIPosition = 0;

	private Texture2D backgroundTexture;

	private Texture2D backShadeTexture;

	private Texture2D framesTexture;

	private Texture2D elementsTexture;

	private Texture2D cloudTexture;

	private Texture2D customOptionsBackgroundTexture;

	private Color fadeColor = new Color(255f, 255f, 255f, 0.6f);

	private bool timerOn = false;

	private float timer = 0f;

	private bool controlsDisabled = false;

	private bool gameStarting = false;

	private int gameType = 0;

	private int customOptionRowPosition = 0;

	private int CUSTOMOPTIONROWPOSITIONLIMIT = 5;

	private int customOptionColumnPosition = 0;

	private int CUSTOMOPTIONCOLUMNPOSITIONLIMIT = 5;

	private int customCupOption = 0;

	private int CUSTOMCUPOPTIONLIMIT = 2;

	private int customTimeOption = 0;

	private int CUSTOMTIMEOPTIONLIMIT = 4;

	private int customBulletOption = 0;

	private int CUSTOMBULLETOPTIONLIMIT = 4;

	private int customFriendlyFireOption = 0;

	private int CUSTOMFRIENDLYFIREOPTIONLIMIT = 1;

	private int customWhitePilotOption = 0;

	private int CUSTOMWHITEPILOTOPTIONLIMIT = 3;

	private int customYellowPilotOption = 0;

	private int CUSTOMYELLOWPILOTOPTIONLIMIT = 3;

	private int customGreenPilotOption = 0;

	private int CUSTOMGREENPILOTOPTIONLIMIT = 3;

	private int customBluePilotOption = 0;

	private int CUSTOMBLUEPILOTOPTIONLIMIT = 3;

	private int customLevelOption = 0;

	private int CUSTOMLEVELOPTIONLIMIT = 4;

	private bool customLevel1On = true;

	private bool customLevel2On = true;

	private bool customLevel3On = true;

	private bool customLevel4On = true;

	private bool customLevel5On = true;

	private bool optionSelected = false;

	private int controllerInControl = -1;

	private bool[] isActive;

	private int[] squadCount;

	public ControllerSelectScreen(GameWorld gw)
	{
		g = gw;
		controllerSplit = new bool[CONTROLLERLIMIT];
		controllerFull = new bool[CONTROLLERLIMIT];
		controllerSquadron = new int[CONTROLLERLIMIT];
		altControllerSquadron = new int[CONTROLLERLIMIT];
		controllerPosition = new Vector2[CONTROLLERLIMIT];
		altControllerPosition = new Vector2[CONTROLLERLIMIT];
		isActive = new bool[g.theSlots.Length];
		squadCount = new int[g.theSlots.Length];
		ResetScreen();
	}

	public void LoadContent(ContentManager theContentManager)
	{
		backgroundTexture = TextureManager.GetScreenControllerSelectTexture();
		backShadeTexture = TextureManager.GetScreenFadeOverlayTexture();
		framesTexture = theContentManager.Load<Texture2D>("ScreenControllerSelectFrames");
		elementsTexture = theContentManager.Load<Texture2D>("ElementsControllerSelect");
		cloudTexture = TextureManager.GetCloudTexture();
		customOptionsBackgroundTexture = TextureManager.GetScreenCustomOptionsBackgroundTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(origin: new Vector2(0f, 0f), texture: backgroundTexture, position: screenPosition, sourceRectangle: null, color: Color.White, rotation: 0f, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		theSpriteBatch.Draw(origin: new Vector2(200f, 85f), sourceRectangle: new Rectangle(0, 0, 408, 171), texture: cloudTexture, position: cloudPosition, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		if (phase == 0 || phase == 1)
		{
			theSpriteBatch.Draw(origin: new Vector2(0f, 0f), texture: framesTexture, position: new Vector2(screenPosition.X, screenPosition.Y + 50f), sourceRectangle: null, color: fadeColor, rotation: 0f, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			DrawScreenText(theSpriteBatch);
			DrawScreenElements(theSpriteBatch);
		}
		DrawGameModeText(theSpriteBatch);
		if (phase == 3)
		{
			DrawCustomOptions(theSpriteBatch);
		}
		ControlOverlay.DrawAB(theSpriteBatch);
	}

	public void DrawScreenElements(SpriteBatch theSpriteBatch)
	{
		if (phase != 0 && phase != 1)
		{
			return;
		}
		Vector2 origin = new Vector2(0f, 0f);
		float num = (float)Math.Sin(arrowFlashTimer * 4f);
		if (num <= 0f)
		{
			arrowFlashTimer = 0f;
		}
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			Vector2 vector = new Vector2(0f, 0f);
			Vector2 vector2 = new Vector2(0f, 0f);
			if (controllerSquadron[i] == -1)
			{
				vector = new Vector2(19f, 0f);
			}
			if (altControllerSquadron[i] == -1)
			{
				vector2 = new Vector2(19f, 0f);
			}
			if (isAIPosition[i * 2] && (phase == 1 || controllerSquadron[i] > -1))
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, ICONDISTANCEY * 4, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: controllerPosition[i] + vector, color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			if (isAIPosition[i * 2 + 1] && (phase == 1 || altControllerSquadron[i] > -1))
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, ICONDISTANCEY * 4, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: altControllerPosition[i] + vector2, color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			if (phase == 1 && i == controllerAIPosition / 2)
			{
				if (controllerAIPosition % 2 == 0)
				{
					theSpriteBatch.Draw(sourceRectangle: new Rectangle(ICONDISTANCEX, ICONDISTANCEY * 4, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: controllerPosition[i] + vector, color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
					if (controllerSquadron[i] > -1)
					{
						theSpriteBatch.Draw(sourceRectangle: new Rectangle(ICONDISTANCEX, ICONDISTANCEY * 5, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: new Vector2(controllerPosition[i].X - (float)ICONDISTANCEX, controllerPosition[i].Y), color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
					}
					if (controllerSquadron[i] < 3)
					{
						theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, ICONDISTANCEY * 5, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: new Vector2(controllerPosition[i].X + (float)ICONDISTANCEX, controllerPosition[i].Y), color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
					}
				}
				else
				{
					theSpriteBatch.Draw(sourceRectangle: new Rectangle(ICONDISTANCEX, ICONDISTANCEY * 4, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: altControllerPosition[i] + vector2, color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
					if (altControllerSquadron[i] > -1)
					{
						theSpriteBatch.Draw(sourceRectangle: new Rectangle(ICONDISTANCEX, ICONDISTANCEY * 5, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: new Vector2(altControllerPosition[i].X - (float)ICONDISTANCEX, altControllerPosition[i].Y), color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
					}
					if (altControllerSquadron[i] < 3)
					{
						theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, ICONDISTANCEY * 5, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: new Vector2(altControllerPosition[i].X + (float)ICONDISTANCEX, altControllerPosition[i].Y), color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
					}
				}
			}
			float num2 = 1f;
			if (!g.theControllerMenuManager[i].GetIsConnected())
			{
				num2 = 0.15f;
			}
			if (!controllerSplit[i] && !isAIPosition[i * 2])
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, ICONDISTANCEY * i, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: controllerPosition[i] + vector, color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			if (controllerSplit[i])
			{
				if (!isAIPosition[i * 2])
				{
					Rectangle value = new Rectangle(0, ICONDISTANCEY * i, ICONWIDTH, ICONHEIGHT);
					Rectangle value2 = new Rectangle(0, ICONDISTANCEY * 6, ICONWIDTH, ICONHEIGHT);
					theSpriteBatch.Draw(elementsTexture, controllerPosition[i] + vector, value, Color.White * num2, 0f, origin, 1f, SpriteEffects.None, 0f);
					if (g.theControllerMenuManager[i].GetIsConnected())
					{
						theSpriteBatch.Draw(elementsTexture, controllerPosition[i] + vector, value2, Color.White * (Math.Abs(num) / 1.5f), 0f, origin, 1f, SpriteEffects.None, 0f);
						if (phase == 0)
						{
							if (controllerSquadron[i] > -1)
							{
								theSpriteBatch.Draw(sourceRectangle: new Rectangle(ICONDISTANCEX, ICONDISTANCEY * 5, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: new Vector2(controllerPosition[i].X - (float)ICONDISTANCEX, controllerPosition[i].Y), color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
							}
							if (controllerSquadron[i] < 3)
							{
								theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, ICONDISTANCEY * 5, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: new Vector2(controllerPosition[i].X + (float)ICONDISTANCEX, controllerPosition[i].Y), color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
							}
						}
					}
				}
				if (!isAIPosition[i * 2 + 1])
				{
					Rectangle value = new Rectangle(ICONDISTANCEX, ICONDISTANCEY * i, ICONWIDTH, ICONHEIGHT);
					Rectangle value2 = new Rectangle(ICONDISTANCEX, ICONDISTANCEY * 6, ICONWIDTH, ICONHEIGHT);
					theSpriteBatch.Draw(elementsTexture, altControllerPosition[i] + vector2, value, Color.White * num2, 0f, origin, 1f, SpriteEffects.None, 0f);
					if (g.theControllerMenuManager[i].GetIsConnected())
					{
						theSpriteBatch.Draw(elementsTexture, altControllerPosition[i] + vector2, value2, Color.White * (Math.Abs(num) / 1.5f), 0f, origin, 1f, SpriteEffects.None, 0f);
						if (phase == 0)
						{
							if (altControllerSquadron[i] > -1)
							{
								theSpriteBatch.Draw(sourceRectangle: new Rectangle(ICONDISTANCEX, ICONDISTANCEY * 5, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: new Vector2(altControllerPosition[i].X - (float)ICONDISTANCEX, altControllerPosition[i].Y), color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
							}
							if (altControllerSquadron[i] < 3)
							{
								theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, ICONDISTANCEY * 5, ICONWIDTH, ICONHEIGHT), texture: elementsTexture, position: new Vector2(altControllerPosition[i].X + (float)ICONDISTANCEX, altControllerPosition[i].Y), color: Color.White * (1f - Math.Abs(num) / 1.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
							}
						}
					}
				}
			}
			for (int j = 0; j < g.theSlots.Length; j++)
			{
				int num3 = 0;
				for (int k = 0; k < controllerSquadron.Length; k++)
				{
					if (controllerSquadron[k] == j || altControllerSquadron[k] == j)
					{
						num3++;
					}
				}
				if (num3 == 0)
				{
					theSpriteBatch.Draw(sourceRectangle: new Rectangle(181, 1, 248, 470), texture: elementsTexture, position: new Vector2(215 + 251 * j, 127f), color: new Color(0f, 0f, 0f, 0.25f), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				}
			}
		}
		if (phase == 0 && ((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) && CountNumberOfHumanPlayers() > 2)
		{
			SpriteFont roundFont = g.theFontManager.GetRoundFont();
			string text = "FULL GAME REQUIRED FOR\n            3 TO 8 PLAYERS";
			Vector2 v = new Vector2(640f, 360f);
			Vector2 o = roundFont.MeasureString(text) / 2f;
			General.DrawEmbossedString(theSpriteBatch, roundFont, text, v, new Color(225, 0, 0), new Color(255, 0, 0), new Color(195, 0, 0), 0f, o, 1f, 1, 1f);
		}
		if (phase == 1 && ((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) && CountNumberOfAIPlayers() > 1)
		{
			SpriteFont roundFont = g.theFontManager.GetRoundFont();
			string text = "  FULL GAME REQUIRED FOR\n MORE THAN 1 A.I. OPPONENT";
			Vector2 v = new Vector2(640f, 360f);
			Vector2 o = roundFont.MeasureString(text) / 2f;
			General.DrawEmbossedString(theSpriteBatch, roundFont, text, v, new Color(225, 0, 0), new Color(255, 0, 0), new Color(195, 0, 0), 0f, o, 1f, 1, 1f);
		}
	}

	public void DrawScreenText(SpriteBatch theSpriteBatch)
	{
		SpriteFont spriteFont;
		Vector2 v;
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			spriteFont = g.theFontManager.GetFontGameHeading3();
			v = new Vector2(717f, 70f);
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			spriteFont = g.theFontManager.GetFontGameHeading3();
			v = new Vector2(717f, 85f);
		}
		else
		{
			spriteFont = g.theFontManager.GetFontGameHeading2();
			v = new Vector2(717f, 100f);
		}
		Vector2 vector = new Vector2(0f, 0f);
		Color white = Color.White;
		string text = "";
		text = ((phase == 0) ? "SELECT PLAYERS" : ((phase != 1) ? "" : "SELECT A.I. OPPONENTS"));
		General.DrawEmbossedString(o: new Vector2(spriteFont.MeasureString(text).X / 2f, spriteFont.MeasureString(text).Y / 2f), theSpriteBatch: theSpriteBatch, f: spriteFont, s: text, v: v, c: new Color(225, 225, 225), ct: new Color(255, 255, 255), cb: new Color(195, 195, 195), r: 0f, sc: 1f, b: 1, fade: 1f);
		for (int i = 0; i < g.theSlots.Length; i++)
		{
			Slot slot = g.theSlots[i];
			Vector2 origin = g.theFontManager.GetFont().MeasureString(slot.GetSlotFullName()) / 2f;
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), slot.GetSlotFullName(), new Vector2(339 + 250 * i, 149f), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), slot.GetSlotFullName(), new Vector2(341 + 250 * i, 149f), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), slot.GetSlotFullName(), new Vector2(339 + 250 * i, 151f), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), slot.GetSlotFullName(), new Vector2(341 + 250 * i, 151f), Color.Black, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), slot.GetSlotFullName(), new Vector2(340 + 250 * i, 150f), slot.GetSlotColorA(), 0f, origin, 1f, SpriteEffects.None, 0f);
		}
		string text2 = "";
		Color ct = Color.White;
		if (CountActiveSquadrons() == 2)
		{
			text2 = "CLASSIC DOGFIGHT";
		}
		if (CountActiveSquadrons() == 3)
		{
			text2 = "MINOR SKIRMISH";
		}
		if (CountActiveSquadrons() == 4)
		{
			text2 = "MAJOR SKIRMISH";
		}
		if (CountActiveSquadrons() == 2 && (CountSquadronParticipants(0) > 1 || CountSquadronParticipants(1) > 1 || CountSquadronParticipants(2) > 1 || CountSquadronParticipants(3) > 1))
		{
			text2 = "CLASSIC DOGFIGHT (TEAM)";
		}
		if (CountActiveSquadrons() == 3 && (CountSquadronParticipants(0) > 1 || CountSquadronParticipants(1) > 1 || CountSquadronParticipants(2) > 1 || CountSquadronParticipants(3) > 1))
		{
			text2 = "MINOR SKIRMISH (TEAM)";
		}
		if (CountActiveSquadrons() == 4 && (CountSquadronParticipants(0) > 1 || CountSquadronParticipants(1) > 1 || CountSquadronParticipants(2) > 1 || CountSquadronParticipants(3) > 1))
		{
			text2 = "MAJOR SKIRMISH (TEAM)";
		}
		if (CountActiveSquadrons() == 2 && (CountSquadronParticipants(-1) == 0 || CountSquadronParticipants(-1) == 0 || CountSquadronParticipants(-1) == 0 || CountSquadronParticipants(-1) == 0))
		{
			text2 = "FULL SQUADRON SCRAMBLE";
		}
		if (phase == 1 && CountActiveSquadrons() == 0)
		{
			text2 = "2 MORE SQUADRONS REQUIRED";
			ct = Color.Red;
		}
		if (phase == 1 && CountActiveSquadrons() == 1)
		{
			text2 = "1 MORE SQUADRON REQUIRED";
			ct = Color.Red;
		}
		General.DrawEmbossedString(o: new Vector2(0f, g.theFontManager.GetHiScoreFont().MeasureString(text2).Y / 2f), theSpriteBatch: theSpriteBatch, f: g.theFontManager.GetHiScoreFont(), s: text2, v: new Vector2(250f, 627f), c: new Color(ct.R - 25, ct.G - 25, ct.B - 25), ct: ct, cb: new Color(ct.R - 50, ct.G - 50, ct.B - 50), r: 0f, sc: 1f, b: 1, fade: 1f);
	}

	public void DrawGameModeText(SpriteBatch theSpriteBatch)
	{
		if (phase != 2)
		{
			return;
		}
		theSpriteBatch.Draw(backShadeTexture, screenPosition, null, Color.White * 0.8f, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
		SpriteFont spriteFont;
		Vector2 v;
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			spriteFont = g.theFontManager.GetFontGameHeading3();
			v = new Vector2(640f, 70f);
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			spriteFont = g.theFontManager.GetFontGameHeading3();
			v = new Vector2(640f, 85f);
		}
		else
		{
			spriteFont = g.theFontManager.GetFontGameHeading2();
			v = new Vector2(640f, 100f);
		}
		Vector2 vector = new Vector2(0f, 0f);
		Color white = Color.White;
		string text = "SELECT GAME MODE";
		General.DrawEmbossedString(o: new Vector2(spriteFont.MeasureString(text).X / 2f, spriteFont.MeasureString(text).Y / 2f), theSpriteBatch: theSpriteBatch, f: spriteFont, s: text, v: v, c: new Color(225, 225, 225), ct: new Color(255, 255, 255), cb: new Color(195, 195, 195), r: 0f, sc: 1f, b: 1, fade: 1f);
		string text2 = "NORMAL MODE";
		string text3 = "SINGLE PILOT MODE";
		string text4 = "SINGLE SHOT MODE";
		string text5 = "CUSTOM MODE";
		string text6 = "FULL GAME REQUIRED FOR THIS MODE";
		float num = 1f;
		float num2 = 1f;
		float num3 = 1f;
		float num4 = 1f;
		Vector2 vector2 = new Vector2(640f, 250f);
		Vector2 vector3 = new Vector2(640f, 325f);
		Vector2 vector4 = new Vector2(640f, 400f);
		Vector2 vector5 = new Vector2(640f, 475f);
		Vector2 o2 = g.theFontManager.GetRoundFont().MeasureString(text2) / 2f;
		Vector2 o3 = g.theFontManager.GetRoundFont().MeasureString(text3) / 2f;
		Vector2 o4 = g.theFontManager.GetRoundFont().MeasureString(text4) / 2f;
		Vector2 o5 = g.theFontManager.GetRoundFont().MeasureString(text5) / 2f;
		Color ct;
		Color ct2;
		Color ct3;
		Color ct4;
		Vector2 position;
		if (gameType == 0)
		{
			ct = new Color((int)(255f * General.GetOptionThrobValue()), 0, 0);
			ct2 = Color.White;
			ct3 = Color.White;
			ct4 = Color.White;
			num = 0.75f;
			num2 = 0.4f;
			num3 = 0.4f;
			num4 = 0.4f;
			position = vector2;
		}
		else if (gameType == 1)
		{
			ct = Color.White;
			ct2 = new Color((int)(255f * General.GetOptionThrobValue()), 0, 0);
			ct3 = Color.White;
			ct4 = Color.White;
			num = 0.4f;
			num2 = 0.75f;
			num3 = 0.4f;
			num4 = 0.4f;
			position = vector3;
			if ((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode)
			{
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), text6, new Vector2(640f, 525f), Color.Red, 0f, g.theFontManager.GetHiScoreFont().MeasureString(text6) / 2f, 1f, 1, 1f);
			}
		}
		else if (gameType == 2)
		{
			ct = Color.White;
			ct2 = Color.White;
			ct3 = new Color((int)(255f * General.GetOptionThrobValue()), 0, 0);
			ct4 = Color.White;
			num = 0.4f;
			num2 = 0.4f;
			num3 = 0.75f;
			num4 = 0.4f;
			position = vector4;
			if ((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode)
			{
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), text6, new Vector2(640f, 525f), Color.Red, 0f, g.theFontManager.GetHiScoreFont().MeasureString(text6) / 2f, 1f, 1, 1f);
			}
		}
		else
		{
			ct = Color.White;
			ct2 = Color.White;
			ct3 = Color.White;
			ct4 = new Color((int)(255f * General.GetOptionThrobValue()), 0, 0);
			num = 0.4f;
			num2 = 0.4f;
			num3 = 0.4f;
			num4 = 0.75f;
			position = vector5;
			if ((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode)
			{
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), text6, new Vector2(640f, 525f), Color.Red, 0f, g.theFontManager.GetHiScoreFont().MeasureString(text6) / 2f, 1f, 1, 1f);
			}
		}
		if ((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode)
		{
			ct2 = new Color(75, 75, 75);
			ct3 = new Color(75, 75, 75);
			ct4 = new Color(75, 75, 75);
		}
		Rectangle value = new Rectangle(0, 808, 394, 41);
		theSpriteBatch.Draw(origin: new Vector2(197f, 23f), texture: elementsTexture, position: position, sourceRectangle: value, color: new Color(200, 200, 255) * 0.75f, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetRoundFont(), text2, vector2, new Color(ct.R - 25, ct.G - 25, ct.B - 25), ct, new Color(ct.R - 50, ct.G - 50, ct.B - 50), 0f, o2, num, 1, 1f);
		General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetRoundFont(), text3, vector3, new Color(ct2.R - 25, ct2.G - 25, ct2.B - 25), ct2, new Color(ct2.R - 50, ct2.G - 50, ct2.B - 50), 0f, o3, num2, 1, 1f);
		General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetRoundFont(), text4, vector4, new Color(ct3.R - 25, ct3.G - 25, ct3.B - 25), ct3, new Color(ct3.R - 50, ct3.G - 50, ct3.B - 50), 0f, o4, num3, 1, 1f);
		General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetRoundFont(), text5, vector5, new Color(ct4.R - 25, ct4.G - 25, ct4.B - 25), ct4, new Color(ct4.R - 50, ct4.G - 50, ct4.B - 50), 0f, o5, num4, 1, 1f);
		SpriteFont font = g.theFontManager.GetFont();
		string text7 = "";
		Vector2 v2 = new Vector2(640f, 575f);
		text7 = ((gameType == 0) ? "EACH SQUADRON HAS 4 PILOTS - 2 MINUTE ROUNDS - FIRST SQUADRON WITH 3 CUPS WINS" : ((gameType == 1) ? "EACH PLAYER HAS A SINGLE PILOT - 1 MINUTE ROUNDS - FIRST SQUADRON WITH 3 CUPS WINS" : ((gameType != 2) ? "CUSTOMISE THE GAME - TURN ON/OFF FRIENDLY FIRE - LIMIT THE NUMBER OF ACTIVE BULLETS" : "ONLY ONE BULLET CAN BE FIRED AT A TIME - 5 MINUTE ROUND - TOP SCORING SQUADRON WINS")));
		Vector2 o6 = font.MeasureString(text7) / 2f;
		General.DrawOutlineString(theSpriteBatch, font, text7, v2, Color.White, 0f, o6, 1f, 1, 1f);
		DrawControllerIcon(theSpriteBatch);
	}

	public void DrawCustomOptions(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(0f, 0f);
		theSpriteBatch.Draw(backShadeTexture, screenPosition, null, Color.White * 0.8f, 0f, origin, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.Draw(customOptionsBackgroundTexture, screenPosition, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		SpriteFont fontGameHeading = g.theFontManager.GetFontGameHeading1();
		SpriteFont font = g.theFontManager.GetFont();
		SpriteFont font2 = g.theFontManager.GetFont();
		string text = "CUSTOM MODE OPTIONS";
		Vector2 v = new Vector2(640f, 118f);
		Color white = Color.White;
		Vector2 o = fontGameHeading.MeasureString(text) / 2f;
		General.DrawEmbossedString(theSpriteBatch, fontGameHeading, text, v, new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, o, 1f, 1, 1f);
		if (customOptionRowPosition < 4)
		{
			if (optionSelected)
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 558, 518, 75), texture: elementsTexture, position: new Vector2(103 + customOptionColumnPosition * 552, 152 + customOptionRowPosition * 74), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			else
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 482, 518, 75), texture: elementsTexture, position: new Vector2(103 + customOptionColumnPosition * 552, 152 + customOptionRowPosition * 74), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
		}
		Color white2 = Color.White;
		Color color = new Color(40, 40, 40);
		Color black = Color.Black;
		Color color2 = Color.Red * General.GetOptionThrobValue();
		string text2 = "CUPS TO WIN";
		Vector2 v2 = new Vector2(340f, 175f);
		Color white3 = Color.White;
		Vector2 o2 = font.MeasureString(text2) / 2f;
		string text3 = "1";
		string text4 = "2";
		string text5 = "3";
		Color c = color;
		Color c2 = color;
		Color c3 = color;
		Vector2 v3 = new Vector2(240f, 200f);
		Vector2 v4 = new Vector2(340f, 200f);
		Vector2 v5 = new Vector2(440f, 200f);
		Vector2 o3 = font2.MeasureString(text3) / 2f;
		Vector2 o4 = font2.MeasureString(text4) / 2f;
		Vector2 o5 = font2.MeasureString(text5) / 2f;
		Color color3 = white2;
		if (optionSelected && customOptionRowPosition == 0 && customOptionColumnPosition == 0)
		{
			color3 = color2;
		}
		if (customCupOption == 0)
		{
			c = color3;
		}
		else if (customCupOption == 1)
		{
			c2 = color3;
		}
		else if (customCupOption == 2)
		{
			c3 = color3;
		}
		if (customOptionRowPosition == 0 && customOptionColumnPosition == 0 && !optionSelected)
		{
			General.DrawOutlineString(theSpriteBatch, font, text2, v2, Color.Red * General.GetOptionThrobValue(), 0f, o2, 1f, 1, 1f);
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, font, text2, v2, white3, 0f, o2, 1f, 1, 1f);
		}
		General.DrawOutlineString(theSpriteBatch, font2, text3, v3, c, 0f, o3, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text4, v4, c2, 0f, o4, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text5, v5, c3, 0f, o5, 1f, 1, 1f);
		string text6 = "TIME LIMIT (MINUTES)";
		Vector2 v6 = new Vector2(940f, 175f);
		Color white4 = Color.White;
		Vector2 o6 = font.MeasureString(text6) / 2f;
		string text7 = "NONE";
		string text8 = "1";
		string text9 = "2";
		string text10 = "3";
		string text11 = "5";
		Color c4 = color;
		Color c5 = color;
		Color c6 = color;
		Color c7 = color;
		Color c8 = color;
		Vector2 v7 = new Vector2(740f, 200f);
		Vector2 v8 = new Vector2(840f, 200f);
		Vector2 v9 = new Vector2(940f, 200f);
		Vector2 v10 = new Vector2(1040f, 200f);
		Vector2 v11 = new Vector2(1140f, 200f);
		Vector2 o7 = font2.MeasureString(text7) / 2f;
		Vector2 o8 = font2.MeasureString(text8) / 2f;
		Vector2 o9 = font2.MeasureString(text9) / 2f;
		Vector2 o10 = font2.MeasureString(text10) / 2f;
		Vector2 o11 = font2.MeasureString(text11) / 2f;
		Color color4 = white2;
		if (optionSelected && customOptionRowPosition == 0 && customOptionColumnPosition == 1)
		{
			color4 = color2;
		}
		if (customTimeOption == 0)
		{
			c4 = color4;
		}
		else if (customTimeOption == 1)
		{
			c5 = color4;
		}
		else if (customTimeOption == 2)
		{
			c6 = color4;
		}
		else if (customTimeOption == 3)
		{
			c7 = color4;
		}
		else if (customTimeOption == 4)
		{
			c8 = color4;
		}
		if (customOptionRowPosition == 0 && customOptionColumnPosition == 1 && !optionSelected)
		{
			General.DrawOutlineString(theSpriteBatch, font, text6, v6, Color.Red * General.GetOptionThrobValue(), 0f, o6, 1f, 1, 1f);
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, font, text6, v6, white4, 0f, o6, 1f, 1, 1f);
		}
		General.DrawOutlineString(theSpriteBatch, font2, text7, v7, c4, 0f, o7, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text8, v8, c5, 0f, o8, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text9, v9, c6, 0f, o9, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text10, v10, c7, 0f, o10, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text11, v11, c8, 0f, o11, 1f, 1, 1f);
		string text12 = "BULLET LIMIT";
		Vector2 v12 = new Vector2(340f, 250f);
		Color white5 = Color.White;
		Vector2 o12 = font.MeasureString(text12) / 2f;
		string text13 = "1";
		string text14 = "2";
		string text15 = "3";
		string text16 = "4";
		string text17 = "5";
		Color c9 = color;
		Color c10 = color;
		Color c11 = color;
		Color c12 = color;
		Color c13 = color;
		Vector2 v13 = new Vector2(140f, 275f);
		Vector2 v14 = new Vector2(240f, 275f);
		Vector2 v15 = new Vector2(340f, 275f);
		Vector2 v16 = new Vector2(440f, 275f);
		Vector2 v17 = new Vector2(540f, 275f);
		Vector2 o13 = font2.MeasureString(text13) / 2f;
		Vector2 o14 = font2.MeasureString(text14) / 2f;
		Vector2 o15 = font2.MeasureString(text15) / 2f;
		Vector2 o16 = font2.MeasureString(text16) / 2f;
		Vector2 o17 = font2.MeasureString(text17) / 2f;
		Color color5 = white2;
		if (optionSelected && customOptionRowPosition == 1 && customOptionColumnPosition == 0)
		{
			color5 = color2;
		}
		if (customBulletOption == 0)
		{
			c9 = color5;
		}
		else if (customBulletOption == 1)
		{
			c10 = color5;
		}
		else if (customBulletOption == 2)
		{
			c11 = color5;
		}
		else if (customBulletOption == 3)
		{
			c12 = color5;
		}
		else if (customBulletOption == 4)
		{
			c13 = color5;
		}
		if (customOptionRowPosition == 1 && customOptionColumnPosition == 0 && !optionSelected)
		{
			General.DrawOutlineString(theSpriteBatch, font, text12, v12, Color.Red * General.GetOptionThrobValue(), 0f, o12, 1f, 1, 1f);
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, font, text12, v12, white5, 0f, o12, 1f, 1, 1f);
		}
		General.DrawOutlineString(theSpriteBatch, font2, text13, v13, c9, 0f, o13, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text14, v14, c10, 0f, o14, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text15, v15, c11, 0f, o15, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text16, v16, c12, 0f, o16, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text17, v17, c13, 0f, o17, 1f, 1, 1f);
		string text18 = "FRIENDLY FIRE";
		Vector2 v18 = new Vector2(940f, 250f);
		Color white6 = Color.White;
		Vector2 o18 = font.MeasureString(text18) / 2f;
		string text19 = "ON";
		string text20 = "OFF";
		Color c14 = color;
		Color c15 = color;
		Vector2 v19 = new Vector2(890f, 275f);
		Vector2 v20 = new Vector2(990f, 275f);
		Vector2 o19 = font2.MeasureString(text19) / 2f;
		Vector2 o20 = font2.MeasureString(text20) / 2f;
		Color color6 = white2;
		if (optionSelected && customOptionRowPosition == 1 && customOptionColumnPosition == 1)
		{
			color6 = color2;
		}
		if (customFriendlyFireOption == 0)
		{
			c14 = color6;
		}
		else if (customFriendlyFireOption == 1)
		{
			c15 = color6;
		}
		if (customOptionRowPosition == 1 && customOptionColumnPosition == 1 && !optionSelected)
		{
			General.DrawOutlineString(theSpriteBatch, font, text18, v18, Color.Red * General.GetOptionThrobValue(), 0f, o18, 1f, 1, 1f);
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, font, text18, v18, white6, 0f, o18, 1f, 1, 1f);
		}
		General.DrawOutlineString(theSpriteBatch, font2, text19, v19, c14, 0f, o19, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, font2, text20, v20, c15, 0f, o20, 1f, 1, 1f);
		string text21 = "WHITE SQUADRON PILOT LIMIT";
		Vector2 v21 = new Vector2(340f, 325f);
		Color white7 = Color.White;
		Vector2 o21 = font.MeasureString(text21) / 2f;
		string text22 = "1";
		string text23 = "2";
		string text24 = "3";
		string text25 = "4";
		Color c16 = color;
		Color c17 = color;
		Color c18 = color;
		Color c19 = color;
		Vector2 v22 = new Vector2(190f, 350f);
		Vector2 v23 = new Vector2(290f, 350f);
		Vector2 v24 = new Vector2(390f, 350f);
		Vector2 v25 = new Vector2(490f, 350f);
		Vector2 o22 = font2.MeasureString(text22) / 2f;
		Vector2 o23 = font2.MeasureString(text23) / 2f;
		Vector2 o24 = font2.MeasureString(text24) / 2f;
		Vector2 o25 = font2.MeasureString(text25) / 2f;
		Color color7 = white2;
		if (optionSelected && customOptionRowPosition == 2 && customOptionColumnPosition == 0)
		{
			color7 = color2;
		}
		if (customWhitePilotOption == 0)
		{
			c16 = color7;
		}
		else if (customWhitePilotOption == 1)
		{
			c17 = color7;
		}
		else if (customWhitePilotOption == 2)
		{
			c18 = color7;
		}
		else if (customWhitePilotOption == 3)
		{
			c19 = color7;
		}
		if (GetSquadronParticipantNumber(0) > 0)
		{
			General.DrawOutlineString(theSpriteBatch, font2, text22, v22, c16, 0f, o22, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(0), 1));
			General.DrawOutlineString(theSpriteBatch, font2, text23, v23, c17, 0f, o23, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(0), 2));
			General.DrawOutlineString(theSpriteBatch, font2, text24, v24, c18, 0f, o24, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(0), 3));
			General.DrawOutlineString(theSpriteBatch, font2, text25, v25, c19, 0f, o25, 1f, 1, 1f);
			if (customOptionRowPosition == 2 && customOptionColumnPosition == 0 && !optionSelected)
			{
				General.DrawOutlineString(theSpriteBatch, font, text21, v21, Color.Red * General.GetOptionThrobValue(), 0f, o21, 1f, 1, 1f);
			}
			else
			{
				General.DrawOutlineString(theSpriteBatch, font, text21, v21, white7, 0f, o21, 1f, 1, 1f);
			}
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, font, text21, v21, white7, 0f, o21, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text22, v22, c16, 0f, o22, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text23, v23, c17, 0f, o23, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text24, v24, c18, 0f, o24, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text25, v25, c19, 0f, o25, 1f, 1, 0.05f);
		}
		string text26 = "YELLOW SQUADRON PILOT LIMIT";
		Vector2 v26 = new Vector2(940f, 325f);
		Color yellow = Color.Yellow;
		Vector2 o26 = font.MeasureString(text26) / 2f;
		string text27 = "1";
		string text28 = "2";
		string text29 = "3";
		string text30 = "4";
		Color c20 = color;
		Color c21 = color;
		Color c22 = color;
		Color c23 = color;
		Vector2 v27 = new Vector2(790f, 350f);
		Vector2 v28 = new Vector2(890f, 350f);
		Vector2 v29 = new Vector2(990f, 350f);
		Vector2 v30 = new Vector2(1090f, 350f);
		Vector2 o27 = font2.MeasureString(text27) / 2f;
		Vector2 o28 = font2.MeasureString(text28) / 2f;
		Vector2 o29 = font2.MeasureString(text29) / 2f;
		Vector2 o30 = font2.MeasureString(text30) / 2f;
		Color color8 = white2;
		if (optionSelected && customOptionRowPosition == 2 && customOptionColumnPosition == 1)
		{
			color8 = color2;
		}
		if (customYellowPilotOption == 0)
		{
			c20 = color8;
		}
		else if (customYellowPilotOption == 1)
		{
			c21 = color8;
		}
		else if (customYellowPilotOption == 2)
		{
			c22 = color8;
		}
		else if (customYellowPilotOption == 3)
		{
			c23 = color8;
		}
		if (GetSquadronParticipantNumber(1) > 0)
		{
			General.DrawOutlineString(theSpriteBatch, font2, text27, v27, c20, 0f, o27, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(1), 1));
			General.DrawOutlineString(theSpriteBatch, font2, text28, v28, c21, 0f, o28, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(1), 2));
			General.DrawOutlineString(theSpriteBatch, font2, text29, v29, c22, 0f, o29, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(1), 3));
			General.DrawOutlineString(theSpriteBatch, font2, text30, v30, c23, 0f, o30, 1f, 1, 1f);
			if (customOptionRowPosition == 2 && customOptionColumnPosition == 1 && !optionSelected)
			{
				General.DrawOutlineString(theSpriteBatch, font, text26, v26, Color.Red * General.GetOptionThrobValue(), 0f, o26, 1f, 1, 1f);
			}
			else
			{
				General.DrawOutlineString(theSpriteBatch, font, text26, v26, yellow, 0f, o26, 1f, 1, 1f);
			}
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, font, text26, v26, yellow, 0f, o26, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text27, v27, c20, 0f, o27, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text28, v28, c21, 0f, o28, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text29, v29, c22, 0f, o29, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text30, v30, c23, 0f, o30, 1f, 1, 0.05f);
		}
		string text31 = "GREEN SQUADRON PILOT LIMIT";
		Vector2 v31 = new Vector2(340f, 400f);
		Color green = Color.Green;
		Vector2 o31 = font.MeasureString(text31) / 2f;
		string text32 = "1";
		string text33 = "2";
		string text34 = "3";
		string text35 = "4";
		Color c24 = color;
		Color c25 = color;
		Color c26 = color;
		Color c27 = color;
		Vector2 v32 = new Vector2(190f, 425f);
		Vector2 v33 = new Vector2(290f, 425f);
		Vector2 v34 = new Vector2(390f, 425f);
		Vector2 v35 = new Vector2(490f, 425f);
		Vector2 o32 = font2.MeasureString(text32) / 2f;
		Vector2 o33 = font2.MeasureString(text33) / 2f;
		Vector2 o34 = font2.MeasureString(text34) / 2f;
		Vector2 o35 = font2.MeasureString(text35) / 2f;
		Color color9 = white2;
		if (optionSelected && customOptionRowPosition == 3 && customOptionColumnPosition == 0)
		{
			color9 = color2;
		}
		if (customGreenPilotOption == 0)
		{
			c24 = color9;
		}
		else if (customGreenPilotOption == 1)
		{
			c25 = color9;
		}
		else if (customGreenPilotOption == 2)
		{
			c26 = color9;
		}
		else if (customGreenPilotOption == 3)
		{
			c27 = color9;
		}
		if (GetSquadronParticipantNumber(2) > 0)
		{
			General.DrawOutlineString(theSpriteBatch, font2, text32, v32, c24, 0f, o32, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(2), 1));
			General.DrawOutlineString(theSpriteBatch, font2, text33, v33, c25, 0f, o33, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(2), 2));
			General.DrawOutlineString(theSpriteBatch, font2, text34, v34, c26, 0f, o34, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(2), 3));
			General.DrawOutlineString(theSpriteBatch, font2, text35, v35, c27, 0f, o35, 1f, 1, 1f);
			if (customOptionRowPosition == 3 && customOptionColumnPosition == 0 && !optionSelected)
			{
				General.DrawOutlineString(theSpriteBatch, font, text31, v31, Color.Red * General.GetOptionThrobValue(), 0f, o31, 1f, 1, 1f);
			}
			else
			{
				General.DrawOutlineString(theSpriteBatch, font, text31, v31, green, 0f, o31, 1f, 1, 1f);
			}
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, font, text31, v31, green, 0f, o31, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text32, v32, c24, 0f, o32, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text33, v33, c25, 0f, o33, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text34, v34, c26, 0f, o34, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text35, v35, c27, 0f, o35, 1f, 1, 0.05f);
		}
		string text36 = "BLUE SQUADRON PILOT LIMIT";
		Vector2 v36 = new Vector2(940f, 400f);
		Color blue = Color.Blue;
		Vector2 o36 = font.MeasureString(text36) / 2f;
		string text37 = "1";
		string text38 = "2";
		string text39 = "3";
		string text40 = "4";
		Color c28 = color;
		Color c29 = color;
		Color c30 = color;
		Color c31 = color;
		Vector2 v37 = new Vector2(790f, 425f);
		Vector2 v38 = new Vector2(890f, 425f);
		Vector2 v39 = new Vector2(990f, 425f);
		Vector2 v40 = new Vector2(1090f, 425f);
		Vector2 o37 = font2.MeasureString(text37) / 2f;
		Vector2 o38 = font2.MeasureString(text38) / 2f;
		Vector2 o39 = font2.MeasureString(text39) / 2f;
		Vector2 o40 = font2.MeasureString(text40) / 2f;
		Color color10 = white2;
		if (optionSelected && customOptionRowPosition == 3 && customOptionColumnPosition == 1)
		{
			color10 = color2;
		}
		if (customBluePilotOption == 0)
		{
			c28 = color10;
		}
		else if (customBluePilotOption == 1)
		{
			c29 = color10;
		}
		else if (customBluePilotOption == 2)
		{
			c30 = color10;
		}
		else if (customBluePilotOption == 3)
		{
			c31 = color10;
		}
		if (GetSquadronParticipantNumber(3) > 0)
		{
			General.DrawOutlineString(theSpriteBatch, font2, text37, v37, c28, 0f, o37, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(3), 1));
			General.DrawOutlineString(theSpriteBatch, font2, text38, v38, c29, 0f, o38, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(3), 2));
			General.DrawOutlineString(theSpriteBatch, font2, text39, v39, c30, 0f, o39, 1f, 1, GetTextFadeValue(GetSquadronParticipantNumber(3), 3));
			General.DrawOutlineString(theSpriteBatch, font2, text40, v40, c31, 0f, o40, 1f, 1, 1f);
			if (customOptionRowPosition == 3 && customOptionColumnPosition == 1 && !optionSelected)
			{
				General.DrawOutlineString(theSpriteBatch, font, text36, v36, Color.Red * General.GetOptionThrobValue(), 0f, o36, 1f, 1, 1f);
			}
			else
			{
				General.DrawOutlineString(theSpriteBatch, font, text36, v36, blue, 0f, o36, 1f, 1, 1f);
			}
		}
		else
		{
			General.DrawOutlineString(theSpriteBatch, font, text36, v36, blue, 0f, o36, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text37, v37, c28, 0f, o37, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text38, v38, c29, 0f, o38, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text39, v39, c30, 0f, o39, 1f, 1, 0.05f);
			General.DrawOutlineString(theSpriteBatch, font2, text40, v40, c31, 0f, o40, 1f, 1, 0.05f);
		}
		if (!customLevel1On)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(185, 634, 184, 121), texture: elementsTexture, position: new Vector2(134f, 458f), color: Color.White * 0.9f, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			if (customOptionRowPosition != 4 || customLevelOption != 0)
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(261, 756, 36, 30), texture: elementsTexture, position: new Vector2(282f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			else
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(335, 756, 36, 30), texture: elementsTexture, position: new Vector2(282f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
		}
		else if (customOptionRowPosition == 4 && customLevelOption == 0)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(298, 756, 36, 30), texture: elementsTexture, position: new Vector2(282f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (!customLevel2On)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(185, 634, 184, 121), texture: elementsTexture, position: new Vector2(340f, 458f), color: Color.White * 0.9f, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			if (customOptionRowPosition != 4 || customLevelOption != 1)
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(261, 756, 36, 30), texture: elementsTexture, position: new Vector2(488f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			else
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(335, 756, 36, 30), texture: elementsTexture, position: new Vector2(488f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
		}
		else if (customOptionRowPosition == 4 && customLevelOption == 1)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(298, 756, 36, 30), texture: elementsTexture, position: new Vector2(488f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (!customLevel3On)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(185, 634, 184, 121), texture: elementsTexture, position: new Vector2(546f, 458f), color: Color.White * 0.9f, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			if (customOptionRowPosition != 4 || customLevelOption != 2)
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(261, 756, 36, 30), texture: elementsTexture, position: new Vector2(694f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			else
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(335, 756, 36, 30), texture: elementsTexture, position: new Vector2(694f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
		}
		else if (customOptionRowPosition == 4 && customLevelOption == 2)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(298, 756, 36, 30), texture: elementsTexture, position: new Vector2(694f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (!customLevel4On)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(185, 634, 184, 121), texture: elementsTexture, position: new Vector2(752f, 458f), color: Color.White * 0.9f, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			if (customOptionRowPosition != 4 || customLevelOption != 3)
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(261, 756, 36, 30), texture: elementsTexture, position: new Vector2(900f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			else
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(335, 756, 36, 30), texture: elementsTexture, position: new Vector2(900f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
		}
		else if (customOptionRowPosition == 4 && customLevelOption == 3)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(298, 756, 36, 30), texture: elementsTexture, position: new Vector2(900f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (!customLevel5On)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(185, 634, 184, 121), texture: elementsTexture, position: new Vector2(958f, 458f), color: Color.White * 0.9f, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			if (customOptionRowPosition != 4 || customLevelOption != 4)
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(261, 756, 36, 30), texture: elementsTexture, position: new Vector2(1106f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			else
			{
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(335, 756, 36, 30), texture: elementsTexture, position: new Vector2(1106f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
		}
		else if (customOptionRowPosition == 4 && customLevelOption == 4)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(298, 756, 36, 30), texture: elementsTexture, position: new Vector2(1106f, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (customOptionRowPosition == 4)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 634, 184, 121), texture: elementsTexture, position: new Vector2(134 + customLevelOption * 206, 458f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		if (customOptionRowPosition == 5)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 756, 260, 50), texture: elementsTexture, position: new Vector2(508f, 586f), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		string text41 = "START GAME";
		Vector2 v41 = new Vector2(640f, 613f);
		Color white8 = Color.White;
		Vector2 vector = font.MeasureString(text41) / 2f;
		if (customOptionRowPosition == 5 && !optionSelected)
		{
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), text41, v41, new Color((int)(225f * General.GetOptionThrobValue()), 0, 0), new Color((int)(255f * General.GetOptionThrobValue()), 0, 0), new Color((int)(195f * General.GetOptionThrobValue()), 0, 0), 0f, g.theFontManager.GetHiScoreFont().MeasureString(text41) / 2f, 1f, 1, 1f);
		}
		else
		{
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), text41, v41, new Color(white8.R - 25, white8.G - 25, white8.B - 25), white8, new Color(white8.R - 50, white8.G - 50, white8.B - 50), 0f, g.theFontManager.GetHiScoreFont().MeasureString(text41) / 2f, 1f, 1, 1f);
		}
		DrawControllerIcon(theSpriteBatch);
	}

	private void DrawControllerIcon(SpriteBatch theSpriteBatch)
	{
		Vector2 position = new Vector2(0f, 0f);
		if (phase == 2)
		{
			position = ((g.theSafeArea.GetScreenMode() == 0) ? new Vector2(260f, 65f) : ((g.theSafeArea.GetScreenMode() != 1) ? new Vector2(320f, 95f) : new Vector2(260f, 80f)));
		}
		if (phase == 3)
		{
			position = new Vector2(310f, 114f);
		}
		Rectangle value = new Rectangle(4 + 51 * controllerInControl, 863, 51, 51);
		Rectangle value2 = new Rectangle(208, 863, 51, 51);
		float num = (float)Math.Sin(arrowFlashTimer * 2f);
		if (num <= 0f)
		{
			arrowFlashTimer = 0f;
		}
		theSpriteBatch.Draw(elementsTexture, position, value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.Draw(elementsTexture, position, value2, Color.White * (Math.Abs(num) / 2f), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
	}

	public float GetTextFadeValue(int i, int n)
	{
		if (i > n)
		{
			return 0.05f;
		}
		return 1f;
	}

	public void Update(GameTime theGameTime)
	{
		if (!Guide.IsVisible)
		{
			if (phase == 0)
			{
				CheckInputPlayerPhase(theGameTime);
			}
			if (phase == 1)
			{
				CheckInputAIPhase(theGameTime);
			}
			if (phase == 2)
			{
				CheckInputGameMode(theGameTime);
			}
			if (phase == 3)
			{
				CheckInputCustomOptions(theGameTime);
			}
		}
		UpdatePositions();
		UpdateCloud(theGameTime);
		FadeControl(theGameTime);
		arrowFlashTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		g.theFrontEnd.UpdateScenery(theGameTime);
	}

	public void FadeControl(GameTime theGameTime)
	{
		if (!timerOn)
		{
			return;
		}
		if (timer == 0f)
		{
			g.theScreenFadeOverlay.FadeScreen(4f);
			if (gameStarting)
			{
				g.theSoundManager.SetMusicFadeSpeed(-0.4f);
			}
		}
		timer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (gameStarting)
		{
			if ((int)timer == 3)
			{
				timerOn = false;
				controlsDisabled = false;
				gameStarting = false;
				timer = 0f;
				phase = 0;
				g.theRoundManager.StartNewRound();
			}
		}
		else if ((int)timer == 1)
		{
			timerOn = false;
			controlsDisabled = false;
			gameStarting = false;
			timer = 0f;
			phase = 0;
			g.theScreenFadeOverlay.UnFadeScreen(1f);
			g.GoToFrontEnd();
		}
	}

	public void UpdateCloud(GameTime theGameTime)
	{
		cloudPosition.X += CLOUDSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (cloudPosition.X > 1700f)
		{
			cloudPosition.X = -300f;
		}
	}

	public void CheckInputAIPhase(GameTime theGameTime)
	{
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			if (controlsDisabled)
			{
				continue;
			}
			if (ControllerIsParticipating(i))
			{
				if (g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed())
				{
					if (controllerAIPosition % 2 == 0)
					{
						controllerSquadron[controllerAIPosition / 2] = MoveLeft(theGameTime, controllerSquadron[controllerAIPosition / 2]);
					}
					else
					{
						altControllerSquadron[controllerAIPosition / 2] = MoveLeft(theGameTime, altControllerSquadron[controllerAIPosition / 2]);
					}
				}
				if (g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed())
				{
					if (controllerAIPosition % 2 == 0)
					{
						controllerSquadron[controllerAIPosition / 2] = MoveRight(theGameTime, controllerSquadron[controllerAIPosition / 2]);
					}
					else
					{
						altControllerSquadron[controllerAIPosition / 2] = MoveRight(theGameTime, altControllerSquadron[controllerAIPosition / 2]);
					}
				}
				if (g.theControllerMenuManager[i].CheckLeftUpPressed() || g.theControllerMenuManager[i].CheckRightUpPressed())
				{
					int num = controllerAIPosition;
					do
					{
						controllerAIPosition--;
					}
					while (controllerAIPosition > 0 && !isAIPosition[controllerAIPosition]);
					if (controllerAIPosition < 0)
					{
						controllerAIPosition = 0;
					}
					if (!isAIPosition[controllerAIPosition])
					{
						do
						{
							controllerAIPosition++;
						}
						while (!isAIPosition[controllerAIPosition]);
					}
					if (controllerAIPosition != num)
					{
						g.theSoundManager.MenuSwitchSound();
					}
				}
				if (g.theControllerMenuManager[i].CheckLeftDownPressed() || g.theControllerMenuManager[i].CheckRightDownPressed())
				{
					int num = controllerAIPosition;
					do
					{
						controllerAIPosition++;
					}
					while (controllerAIPosition < PARTICIPANTLIMIT && !isAIPosition[controllerAIPosition]);
					if (controllerAIPosition >= PARTICIPANTLIMIT)
					{
						controllerAIPosition = PARTICIPANTLIMIT - 1;
					}
					if (!isAIPosition[controllerAIPosition])
					{
						do
						{
							controllerAIPosition--;
						}
						while (!isAIPosition[controllerAIPosition]);
					}
					if (controllerAIPosition != num)
					{
						g.theSoundManager.MenuSwitchSound();
					}
				}
				if ((g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed()) && (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) || CountNumberOfAIPlayers() < 2) && CountActiveSquadrons() > 1)
				{
					g.theSoundManager.MenuSelectSound();
					phase = 2;
					controllerInControl = i;
				}
			}
			if (!g.theControllerMenuManager[i].CheckButtonBPressed() && !g.theControllerMenuManager[i].CheckButtonBackPressed())
			{
				continue;
			}
			g.theSoundManager.MenuSelectSound();
			for (int j = 0; j < isAIPosition.Length; j++)
			{
				if (isAIPosition[j])
				{
					if (j % 2 == 0)
					{
						controllerSquadron[j / 2] = -1;
					}
					else
					{
						altControllerSquadron[j / 2] = -1;
					}
					isAIPosition[j] = false;
				}
			}
			phase = 0;
		}
	}

	public void CheckInputPlayerPhase(GameTime theGameTime)
	{
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			if (controlsDisabled)
			{
				continue;
			}
			if (g.theControllerMenuManager[i].CheckLeftLeftPressed())
			{
				controllerSquadron[i] = MoveLeft(theGameTime, controllerSquadron[i]);
			}
			if (g.theControllerMenuManager[i].CheckLeftRightPressed())
			{
				controllerSquadron[i] = MoveRight(theGameTime, controllerSquadron[i]);
			}
			if (ControllerIsParticipating(i) && (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed()) && (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) || CountNumberOfHumanPlayers() < 3))
			{
				bool flag = false;
				for (int j = 0; j < CONTROLLERLIMIT; j++)
				{
					if (controllerSquadron[j] > -1 || altControllerSquadron[j] > -1)
					{
						g.theSoundManager.MenuSelectSound();
						flag = true;
						controllerInControl = i;
					}
				}
				if (flag)
				{
					for (int k = 0; k < CONTROLLERLIMIT; k++)
					{
						if (controllerSquadron[k] > -1 && altControllerSquadron[k] > -1)
						{
							controllerFull[k] = true;
						}
						else
						{
							controllerFull[k] = false;
						}
					}
					for (int j = 0; j < CONTROLLERLIMIT; j++)
					{
						if (controllerSquadron[j] == -1)
						{
							isAIPosition[j * 2] = true;
						}
						if (altControllerSquadron[j] == -1)
						{
							isAIPosition[j * 2 + 1] = true;
						}
					}
					phase = 1;
					controllerAIPosition = 0;
					while (controllerAIPosition < PARTICIPANTLIMIT && !isAIPosition[controllerAIPosition])
					{
						controllerAIPosition++;
					}
					if (controllerAIPosition >= PARTICIPANTLIMIT)
					{
						phase = 2;
					}
				}
			}
			if (!controlsDisabled)
			{
				if (g.theControllerMenuManager[i].CheckRightLeftPressed())
				{
					altControllerSquadron[i] = MoveLeft(theGameTime, altControllerSquadron[i]);
				}
				if (g.theControllerMenuManager[i].CheckRightRightPressed())
				{
					altControllerSquadron[i] = MoveRight(theGameTime, altControllerSquadron[i]);
				}
				if (g.theControllerMenuManager[i].CheckButtonBPressed() || g.theControllerMenuManager[i].CheckButtonBackPressed())
				{
					g.theSoundManager.MenuSelectSound();
					timerOn = true;
					controlsDisabled = true;
					gameStarting = false;
				}
				if (phase == 0 && !g.theControllerMenuManager[i].GetIsConnected())
				{
					controllerSquadron[i] = -1;
					altControllerSquadron[i] = -1;
				}
			}
		}
	}

	public void CheckInputGameMode(GameTime theGameTime)
	{
		int num = 3;
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			if (controlsDisabled)
			{
				continue;
			}
			if (ControllerIsParticipating(i) && controllerInControl == i)
			{
				if (g.theControllerMenuManager[i].CheckLeftUpPressed() || g.theControllerMenuManager[i].CheckRightUpPressed())
				{
					if (gameType > 0)
					{
						gameType--;
					}
					else
					{
						gameType = num;
					}
					g.theSoundManager.MenuSwitchSound();
				}
				if (g.theControllerMenuManager[i].CheckLeftDownPressed() || g.theControllerMenuManager[i].CheckRightDownPressed())
				{
					if (gameType < num)
					{
						gameType++;
					}
					else
					{
						gameType = 0;
					}
					g.theSoundManager.MenuSwitchSound();
				}
				if ((g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed()) && (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) || CountNumberOfAIPlayers() < 2) && CountActiveSquadrons() > 1)
				{
					g.theSoundManager.MenuSelectSound();
					if (gameType == 0)
					{
						CustomOptions.UseNormalGameOptions();
						timerOn = true;
						controlsDisabled = true;
						gameStarting = true;
					}
					else if (gameType == 1)
					{
						if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode))
						{
							CustomOptions.SetSpecialSquadronSectionSizes(GetSquadronParticipantNumber(0), GetSquadronParticipantNumber(1), GetSquadronParticipantNumber(2), GetSquadronParticipantNumber(3));
							CustomOptions.UseSpecialGameOptions();
							timerOn = true;
							controlsDisabled = true;
							gameStarting = true;
						}
					}
					else if (gameType == 2)
					{
						if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode))
						{
							CustomOptions.UseSpecialGame2Options();
							timerOn = true;
							controlsDisabled = true;
							gameStarting = true;
						}
					}
					else if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode))
					{
						phase = 3;
						GetCustomGameOptions();
					}
				}
			}
			if (g.theControllerMenuManager[i].CheckButtonBPressed() || g.theControllerMenuManager[i].CheckButtonBackPressed())
			{
				g.theSoundManager.MenuSelectSound();
				if (controllerAIPosition >= PARTICIPANTLIMIT)
				{
					phase = 0;
				}
				else
				{
					phase = 1;
				}
			}
		}
	}

	public void GetCustomGameOptions()
	{
		customCupOption = CustomOptions.currentCupOption;
		customTimeOption = CustomOptions.currentTimeOption;
		customBulletOption = CustomOptions.currentBulletOption;
		customFriendlyFireOption = CustomOptions.currentFriendlyFireOption;
		customWhitePilotOption = CustomOptions.currentWhitePilotOption;
		customYellowPilotOption = CustomOptions.currentYellowPilotOption;
		customGreenPilotOption = CustomOptions.currentGreenPilotOption;
		customBluePilotOption = CustomOptions.currentBluePilotOption;
		customLevel1On = CustomOptions.currentLevel1On;
		customLevel2On = CustomOptions.currentLevel1On;
		customLevel3On = CustomOptions.currentLevel1On;
		customLevel4On = CustomOptions.currentLevel1On;
		customLevel5On = CustomOptions.currentLevel1On;
	}

	public int GetSquadronParticipantNumber(int i)
	{
		int num = 0;
		for (int j = 0; j < CONTROLLERLIMIT; j++)
		{
			if (controllerSquadron[j] == i)
			{
				num++;
			}
			if (altControllerSquadron[j] == i)
			{
				num++;
			}
		}
		return num;
	}

	public void CheckInputCustomOptions(GameTime theGameTime)
	{
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			if (controlsDisabled)
			{
				continue;
			}
			if (ControllerIsParticipating(i) && controllerInControl == i)
			{
				if (customOptionRowPosition < 4)
				{
					if (!optionSelected && (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed()))
					{
						if (customOptionRowPosition == 2 && customOptionColumnPosition == 0 && GetSquadronParticipantNumber(0) > 0)
						{
							g.theSoundManager.MenuSelectSound();
							optionSelected = true;
						}
						if (customOptionRowPosition == 2 && customOptionColumnPosition == 1 && GetSquadronParticipantNumber(1) > 0)
						{
							g.theSoundManager.MenuSelectSound();
							optionSelected = true;
						}
						if (customOptionRowPosition == 3 && customOptionColumnPosition == 0 && GetSquadronParticipantNumber(2) > 0)
						{
							g.theSoundManager.MenuSelectSound();
							optionSelected = true;
						}
						if (customOptionRowPosition == 3 && customOptionColumnPosition == 1 && GetSquadronParticipantNumber(3) > 0)
						{
							g.theSoundManager.MenuSelectSound();
							optionSelected = true;
						}
						if (customOptionRowPosition < 2)
						{
							g.theSoundManager.MenuSelectSound();
							optionSelected = true;
						}
					}
					if (optionSelected && (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed()))
					{
						g.theSoundManager.MenuSelectSound();
						optionSelected = false;
					}
				}
				if (!optionSelected)
				{
					if (g.theControllerMenuManager[i].CheckLeftUpPressed() || g.theControllerMenuManager[i].CheckRightUpPressed())
					{
						if (customOptionRowPosition == 0)
						{
							g.theSoundManager.MenuSwitchSound();
							customOptionRowPosition = CUSTOMOPTIONROWPOSITIONLIMIT;
							customOptionColumnPosition = 0;
						}
						else if (customOptionRowPosition > 0)
						{
							g.theSoundManager.MenuSwitchSound();
							customOptionRowPosition--;
						}
					}
					if (g.theControllerMenuManager[i].CheckLeftDownPressed() || g.theControllerMenuManager[i].CheckRightDownPressed())
					{
						if (customOptionRowPosition == CUSTOMOPTIONROWPOSITIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customOptionRowPosition = 0;
							customOptionColumnPosition = 0;
						}
						else if (customOptionRowPosition < CUSTOMOPTIONROWPOSITIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customOptionRowPosition++;
						}
					}
					if (customOptionRowPosition < 4)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customOptionColumnPosition == 1)
						{
							g.theSoundManager.MenuSwitchSound();
							customOptionColumnPosition = 0;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customOptionColumnPosition == 0)
						{
							g.theSoundManager.MenuSwitchSound();
							customOptionColumnPosition = 1;
						}
					}
				}
				if (optionSelected)
				{
					if (customOptionRowPosition == 0 && customOptionColumnPosition == 0)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customCupOption > 0)
						{
							g.theSoundManager.MenuSwitchSound();
							customCupOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customCupOption < CUSTOMCUPOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customCupOption++;
						}
					}
					if (customOptionRowPosition == 0 && customOptionColumnPosition == 1)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customTimeOption > 0)
						{
							g.theSoundManager.MenuSwitchSound();
							customTimeOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customTimeOption < CUSTOMTIMEOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customTimeOption++;
						}
					}
					if (customOptionRowPosition == 1 && customOptionColumnPosition == 0)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customBulletOption > 0)
						{
							g.theSoundManager.MenuSwitchSound();
							customBulletOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customBulletOption < CUSTOMBULLETOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customBulletOption++;
						}
					}
					if (customOptionRowPosition == 1 && customOptionColumnPosition == 1)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customFriendlyFireOption > 0)
						{
							g.theSoundManager.MenuSwitchSound();
							customFriendlyFireOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customFriendlyFireOption < CUSTOMFRIENDLYFIREOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customFriendlyFireOption++;
						}
					}
					if (customOptionRowPosition == 2 && customOptionColumnPosition == 0)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customWhitePilotOption > 0 && customWhitePilotOption >= GetSquadronParticipantNumber(0))
						{
							g.theSoundManager.MenuSwitchSound();
							customWhitePilotOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customWhitePilotOption < CUSTOMWHITEPILOTOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customWhitePilotOption++;
						}
					}
					if (customOptionRowPosition == 2 && customOptionColumnPosition == 1)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customYellowPilotOption > 0 && customYellowPilotOption >= GetSquadronParticipantNumber(1))
						{
							g.theSoundManager.MenuSwitchSound();
							customYellowPilotOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customYellowPilotOption < CUSTOMYELLOWPILOTOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customYellowPilotOption++;
						}
					}
					if (customOptionRowPosition == 3 && customOptionColumnPosition == 0)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customGreenPilotOption > 0 && customGreenPilotOption >= GetSquadronParticipantNumber(2))
						{
							g.theSoundManager.MenuSwitchSound();
							customGreenPilotOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customGreenPilotOption < CUSTOMGREENPILOTOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customGreenPilotOption++;
						}
					}
					if (customOptionRowPosition == 3 && customOptionColumnPosition == 1)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customBluePilotOption > 0 && customBluePilotOption >= GetSquadronParticipantNumber(3))
						{
							g.theSoundManager.MenuSwitchSound();
							customBluePilotOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customBluePilotOption < CUSTOMBLUEPILOTOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customBluePilotOption++;
						}
					}
				}
				if (!optionSelected)
				{
					if (customOptionRowPosition == 4)
					{
						if ((g.theControllerMenuManager[i].CheckLeftLeftPressed() || g.theControllerMenuManager[i].CheckRightLeftPressed()) && customLevelOption > 0)
						{
							g.theSoundManager.MenuSwitchSound();
							customLevelOption--;
						}
						if ((g.theControllerMenuManager[i].CheckLeftRightPressed() || g.theControllerMenuManager[i].CheckRightRightPressed()) && customLevelOption < CUSTOMLEVELOPTIONLIMIT)
						{
							g.theSoundManager.MenuSwitchSound();
							customLevelOption++;
						}
						if (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed())
						{
							if (customLevelOption == 0)
							{
								if (!customLevel1On)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel1On = true;
								}
								else if (CustomLevelCount() > 1)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel1On = false;
								}
							}
							else if (customLevelOption == 1)
							{
								if (!customLevel2On)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel2On = true;
								}
								else if (CustomLevelCount() > 1)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel2On = false;
								}
							}
							else if (customLevelOption == 2)
							{
								if (!customLevel3On)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel3On = true;
								}
								else if (CustomLevelCount() > 1)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel3On = false;
								}
							}
							else if (customLevelOption == 3)
							{
								if (!customLevel4On)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel4On = true;
								}
								else if (CustomLevelCount() > 1)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel4On = false;
								}
							}
							else if (customLevelOption == 4)
							{
								if (!customLevel5On)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel5On = true;
								}
								else if (CustomLevelCount() > 1)
								{
									g.theSoundManager.MenuSelectSound();
									customLevel5On = false;
								}
							}
						}
					}
					if (customOptionRowPosition == 5 && (g.theControllerMenuManager[i].CheckButtonAPressed() || g.theControllerMenuManager[i].CheckButtonStartPressed()))
					{
						g.theSoundManager.MenuSelectSound();
						SetCustomOptions();
						CustomOptions.UseCustomGameOptions();
						timerOn = true;
						controlsDisabled = true;
						gameStarting = true;
					}
				}
			}
			if (optionSelected && (g.theControllerMenuManager[i].CheckButtonBPressed() || g.theControllerMenuManager[i].CheckButtonBackPressed()))
			{
				g.theSoundManager.MenuSelectSound();
				optionSelected = false;
			}
			if (!optionSelected && (g.theControllerMenuManager[i].CheckButtonBPressed() || g.theControllerMenuManager[i].CheckButtonBackPressed()))
			{
				g.theSoundManager.MenuSelectSound();
				phase = 2;
			}
		}
	}

	public int CustomLevelCount()
	{
		int num = 0;
		if (customLevel1On)
		{
			num++;
		}
		if (customLevel2On)
		{
			num++;
		}
		if (customLevel3On)
		{
			num++;
		}
		if (customLevel4On)
		{
			num++;
		}
		if (customLevel5On)
		{
			num++;
		}
		return num;
	}

	public void SetCustomOptions()
	{
		CustomOptions.SetCustomSquadronSectionSizes(customWhitePilotOption + 1, customYellowPilotOption + 1, customGreenPilotOption + 1, customBluePilotOption + 1);
		CustomOptions.SetCustomBulletLimit(customBulletOption + 1);
		CustomOptions.SetCustomTrophyLimit(customCupOption + 1);
		if (customFriendlyFireOption == 0)
		{
			CustomOptions.SetCustomFriendlyFireEnabled(b: true);
		}
		else
		{
			CustomOptions.SetCustomFriendlyFireEnabled(b: false);
		}
		int num = ((customTimeOption == 0) ? (-1) : ((customTimeOption == 1) ? 1 : ((customTimeOption == 2) ? 2 : ((customTimeOption != 3) ? 5 : 3))));
		CustomOptions.SetCustomTimeLimit(num);
		CustomOptions.SetCustomLevelOn(0, customLevel1On);
		CustomOptions.SetCustomLevelOn(1, customLevel2On);
		CustomOptions.SetCustomLevelOn(2, customLevel3On);
		CustomOptions.SetCustomLevelOn(3, customLevel4On);
		CustomOptions.SetCustomLevelOn(4, customLevel5On);
	}

	public void UpdatePositions()
	{
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			controllerPosition[i].X = NEUTRALPOSITIONX + CONTROLLERXOFFSET * (float)controllerSquadron[i];
			altControllerPosition[i].X = NEUTRALPOSITIONX + CONTROLLERXOFFSET * (float)altControllerSquadron[i];
		}
	}

	public int MoveLeft(GameTime theGameTime, int i)
	{
		int j = i;
		do
		{
			j--;
		}
		while (j > -1 && CountSquadronParticipants(j) == 4);
		if (j <= -1)
		{
			j = -1;
		}
		if (j > -1)
		{
			for (; j != i && CountSquadronParticipants(j) + 1 > 4; j++)
			{
			}
		}
		if (j != i)
		{
			g.theSoundManager.MenuSwitchSound();
		}
		return j;
	}

	public int MoveRight(GameTime theGameTime, int i)
	{
		int num = i;
		do
		{
			num++;
		}
		while (num < 3 && CountSquadronParticipants(num) == 4);
		if (num >= 3)
		{
			num = 3;
		}
		while (num != i && CountSquadronParticipants(num) + 1 > 4)
		{
			num--;
		}
		if (num != i)
		{
			g.theSoundManager.MenuSwitchSound();
		}
		return num;
	}

	public void SplitController(int i)
	{
		if (!controllerSplit[i])
		{
			controllerSplit[i] = true;
		}
		else
		{
			controllerSplit[i] = false;
		}
	}

	public int SetAltControllerSquadron(int i)
	{
		return controllerSquadron[i];
	}

	public void SetSquadrons()
	{
		int num = 0;
		for (int i = 0; i < g.theSlots.Length; i++)
		{
			isActive[i] = false;
			for (int j = 0; j < CONTROLLERLIMIT; j++)
			{
				if (controllerSquadron[j] == i || altControllerSquadron[j] == i)
				{
					isActive[i] = true;
				}
			}
			if (isActive[i])
			{
				g.activeSlots[num] = g.theSlots[i];
				g.activeSlots[num].GetCurrentSquadron().SetSquadronSectionSize(CustomOptions.GetSquadronSectionSize(i));
				num++;
			}
		}
	}

	public void SetParticipants()
	{
		int num = 0;
		for (int i = 0; i < squadCount.Length; i++)
		{
			squadCount[i] = 0;
		}
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			if (controllerSquadron[i] > -1)
			{
				if (!isAIPosition[i * 2])
				{
					g.pilots[num].SetParticipant(g.players[i]);
					g.pilots[num].SetCurrentSquadron(g.theSlots[controllerSquadron[i]].GetCurrentSquadron().GetSquadronNumber(), AssignPilotColor(controllerSquadron[i], squadCount[controllerSquadron[i]]));
					g.players[i].SetCurrentPlayerIndex(i);
					if (isAIPosition[i * 2 + 1])
					{
						g.players[i].SetCurrentControllerPosition(0);
					}
					else
					{
						g.players[i].SetCurrentControllerPosition(1);
					}
					g.pilots[num].SetParticipating(b: true);
					if (g.pilots[num].GetCurrentSquadronMember() == null)
					{
						g.pilots[num].SelectCurrentSquadronMember();
					}
					else
					{
						g.pilots[num].GetCurrentSquadronMember().SetInAction(b: true);
					}
					g.players[i].SetParticipating(b: true);
					g.SetActiveController(i, b: true);
					num++;
					squadCount[controllerSquadron[i]]++;
				}
				else
				{
					g.pilots[num].SetParticipant(g.AIplayers[i]);
					g.pilots[num].SetCurrentSquadron(g.theSlots[controllerSquadron[i]].GetCurrentSquadron().GetSquadronNumber(), AssignPilotColor(controllerSquadron[i], squadCount[controllerSquadron[i]]));
					g.pilots[num].SetParticipating(b: true);
					if (g.pilots[num].GetCurrentSquadronMember() == null)
					{
						g.pilots[num].SelectCurrentSquadronMember();
					}
					else
					{
						g.pilots[num].GetCurrentSquadronMember().SetInAction(b: true);
					}
					g.AIplayers[i].SetParticipating(b: true);
					num++;
					squadCount[controllerSquadron[i]]++;
				}
			}
			if (altControllerSquadron[i] <= -1)
			{
				continue;
			}
			if (!isAIPosition[i * 2 + 1])
			{
				g.pilots[num].SetParticipant(g.players[i + 4]);
				g.pilots[num].SetCurrentSquadron(g.theSlots[altControllerSquadron[i]].GetCurrentSquadron().GetSquadronNumber(), AssignPilotColor(altControllerSquadron[i], squadCount[altControllerSquadron[i]]));
				g.players[i + 4].SetCurrentPlayerIndex(i);
				if (isAIPosition[i * 2])
				{
					g.players[i + 4].SetCurrentControllerPosition(0);
				}
				else
				{
					g.players[i + 4].SetCurrentControllerPosition(2);
				}
				g.pilots[num].SetParticipating(b: true);
				if (g.pilots[num].GetCurrentSquadronMember() == null)
				{
					g.pilots[num].SelectCurrentSquadronMember();
				}
				else
				{
					g.pilots[num].GetCurrentSquadronMember().SetInAction(b: true);
				}
				g.players[i + 4].SetParticipating(b: true);
				g.SetActiveController(i, b: true);
				num++;
				squadCount[altControllerSquadron[i]]++;
			}
			else
			{
				g.pilots[num].SetParticipant(g.AIplayers[i + 4]);
				g.pilots[num].SetCurrentSquadron(g.theSlots[altControllerSquadron[i]].GetCurrentSquadron().GetSquadronNumber(), AssignPilotColor(altControllerSquadron[i], squadCount[altControllerSquadron[i]]));
				g.pilots[num].SetParticipating(b: true);
				if (g.pilots[num].GetCurrentSquadronMember() == null)
				{
					g.pilots[num].SelectCurrentSquadronMember();
				}
				else
				{
					g.pilots[num].GetCurrentSquadronMember().SetInAction(b: true);
				}
				g.AIplayers[i + 4].SetParticipating(b: true);
				num++;
				squadCount[altControllerSquadron[i]]++;
			}
		}
	}

	public Color AssignPilotColor(int squad, int pos)
	{
		Color result = Color.Black;
		for (int i = 0; i < g.theSlots.Length; i++)
		{
			if (squad == i)
			{
				switch (pos)
				{
				case 0:
					result = g.theSlots[i].GetSlotColorA();
					break;
				case 1:
					result = g.theSlots[i].GetSlotColorB();
					break;
				case 2:
					result = g.theSlots[i].GetSlotColorC();
					break;
				case 3:
					result = g.theSlots[i].GetSlotColorD();
					break;
				}
			}
		}
		return result;
	}

	public bool GetIsAIPosition(int i)
	{
		return isAIPosition[i];
	}

	public bool GetControllerFull(int i)
	{
		return controllerFull[i];
	}

	public int CountActiveSquadrons()
	{
		int[] array = new int[g.theSlots.Length];
		int num = 0;
		for (int i = 0; i < g.theSlots.Length; i++)
		{
			array[i] = CountSquadronParticipants(i);
		}
		for (int i = 0; i < g.theSlots.Length; i++)
		{
			if (array[i] > 0)
			{
				num++;
			}
		}
		return num;
	}

	public int CountNumberOfHumanPlayers()
	{
		int num = 0;
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			if (controllerSquadron[i] > -1 && !isAIPosition[i * 2])
			{
				num++;
			}
			if (altControllerSquadron[i] > -1 && !isAIPosition[i * 2 + 1])
			{
				num++;
			}
		}
		return num;
	}

	public int CountNumberOfAIPlayers()
	{
		int num = 0;
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			if (controllerSquadron[i] > -1 && isAIPosition[i * 2])
			{
				num++;
			}
			if (altControllerSquadron[i] > -1 && isAIPosition[i * 2 + 1])
			{
				num++;
			}
		}
		return num;
	}

	public int CountSquadronParticipants(int i)
	{
		int num = 0;
		for (int j = 0; j < 8; j++)
		{
			if (j % 2 == 0)
			{
				if (controllerSquadron[j / 2] == i)
				{
					num++;
				}
			}
			else if (altControllerSquadron[j / 2] == i)
			{
				num++;
			}
		}
		return num;
	}

	public bool ControllerIsParticipating(int i)
	{
		if (controllerSquadron[i] > -1 || altControllerSquadron[i] > -1)
		{
			return true;
		}
		return false;
	}

	public void ResetScreen()
	{
		for (int i = 0; i < CONTROLLERLIMIT; i++)
		{
			controllerSplit[i] = true;
			controllerSquadron[i] = -1;
			altControllerSquadron[i] = -1;
			controllerPosition[i].X = NEUTRALPOSITIONX;
			controllerPosition[i].Y = NEUTRALPOSITIONY + CONTROLLERYOFFSET * (float)(i * 2);
			altControllerPosition[i].X = NEUTRALPOSITIONX;
			altControllerPosition[i].Y = NEUTRALPOSITIONY + CONTROLLERYOFFSET * (float)(i * 2 + 1);
		}
		for (int j = 0; j < isAIPosition.Length; j++)
		{
			isAIPosition[j] = false;
		}
	}
}
