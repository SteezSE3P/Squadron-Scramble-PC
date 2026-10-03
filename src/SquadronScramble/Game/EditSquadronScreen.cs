using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace SquadronScramble;

public class EditSquadronScreen
{
	public struct SaveGameData
	{
		public string nameA00;

		public string nameA01;

		public string nameA02;

		public string nameA03;

		public string nameA04;

		public string nameA05;

		public string nameA06;

		public string nameA07;

		public string nameA08;

		public string nameA09;

		public string nameA10;

		public string nameA11;

		public string nameA12;

		public string nameA13;

		public string nameA14;

		public string nameA15;

		public string nameA16;

		public string nameA17;

		public string nameA18;

		public string nameA19;

		public string nameA20;

		public string nameA21;

		public string nameA22;

		public string nameA23;

		public string nameA24;

		public string nameA25;

		public string nameA26;

		public string nameA27;

		public string nameA28;

		public string nameA29;

		public string nameA30;

		public string nameA31;

		public string nameA32;

		public string nameA33;

		public string nameA34;

		public string nameA35;

		public string nameB00;

		public string nameB01;

		public string nameB02;

		public string nameB03;

		public string nameB04;

		public string nameB05;

		public string nameB06;

		public string nameB07;

		public string nameB08;

		public string nameB09;

		public string nameB10;

		public string nameB11;

		public string nameB12;

		public string nameB13;

		public string nameB14;

		public string nameB15;

		public string nameB16;

		public string nameB17;

		public string nameB18;

		public string nameB19;

		public string nameB20;

		public string nameB21;

		public string nameB22;

		public string nameB23;

		public string nameB24;

		public string nameB25;

		public string nameB26;

		public string nameB27;

		public string nameB28;

		public string nameB29;

		public string nameB30;

		public string nameB31;

		public string nameB32;

		public string nameB33;

		public string nameB34;

		public string nameB35;

		public string nameC00;

		public string nameC01;

		public string nameC02;

		public string nameC03;

		public string nameC04;

		public string nameC05;

		public string nameC06;

		public string nameC07;

		public string nameC08;

		public string nameC09;

		public string nameC10;

		public string nameC11;

		public string nameC12;

		public string nameC13;

		public string nameC14;

		public string nameC15;

		public string nameC16;

		public string nameC17;

		public string nameC18;

		public string nameC19;

		public string nameC20;

		public string nameC21;

		public string nameC22;

		public string nameC23;

		public string nameC24;

		public string nameC25;

		public string nameC26;

		public string nameC27;

		public string nameC28;

		public string nameC29;

		public string nameC30;

		public string nameC31;

		public string nameC32;

		public string nameC33;

		public string nameC34;

		public string nameC35;

		public string nameD00;

		public string nameD01;

		public string nameD02;

		public string nameD03;

		public string nameD04;

		public string nameD05;

		public string nameD06;

		public string nameD07;

		public string nameD08;

		public string nameD09;

		public string nameD10;

		public string nameD11;

		public string nameD12;

		public string nameD13;

		public string nameD14;

		public string nameD15;

		public string nameD16;

		public string nameD17;

		public string nameD18;

		public string nameD19;

		public string nameD20;

		public string nameD21;

		public string nameD22;

		public string nameD23;

		public string nameD24;

		public string nameD25;

		public string nameD26;

		public string nameD27;

		public string nameD28;

		public string nameD29;

		public string nameD30;

		public string nameD31;

		public string nameD32;

		public string nameD33;

		public string nameD34;

		public string nameD35;

		public string nameE00;

		public string nameE01;

		public string nameE02;

		public string nameE03;

		public string nameE04;

		public string nameE05;

		public string nameE06;

		public string nameE07;

		public string nameE08;

		public string nameE09;

		public string nameE10;

		public string nameE11;

		public string nameE12;

		public string nameE13;

		public string nameE14;

		public string nameE15;

		public string nameE16;

		public string nameE17;

		public string nameE18;

		public string nameE19;

		public string nameE20;

		public string nameE21;

		public string nameE22;

		public string nameE23;

		public string nameE24;

		public string nameE25;

		public string nameE26;

		public string nameE27;

		public string nameE28;

		public string nameE29;

		public string nameE30;

		public string nameE31;

		public string nameE32;

		public string nameE33;

		public string nameE34;

		public string nameE35;

		public string nameF00;

		public string nameF01;

		public string nameF02;

		public string nameF03;

		public string nameF04;

		public string nameF05;

		public string nameF06;

		public string nameF07;

		public string nameF08;

		public string nameF09;

		public string nameF10;

		public string nameF11;

		public string nameF12;

		public string nameF13;

		public string nameF14;

		public string nameF15;

		public string nameF16;

		public string nameF17;

		public string nameF18;

		public string nameF19;

		public string nameF20;

		public string nameF21;

		public string nameF22;

		public string nameF23;

		public string nameF24;

		public string nameF25;

		public string nameF26;

		public string nameF27;

		public string nameF28;

		public string nameF29;

		public string nameF30;

		public string nameF31;

		public string nameF32;

		public string nameF33;

		public string nameF34;

		public string nameF35;

		public string nameG00;

		public string nameG01;

		public string nameG02;

		public string nameG03;

		public string nameG04;

		public string nameG05;

		public string nameG06;

		public string nameG07;

		public string nameG08;

		public string nameG09;

		public string nameG10;

		public string nameG11;

		public string nameG12;

		public string nameG13;

		public string nameG14;

		public string nameG15;

		public string nameG16;

		public string nameG17;

		public string nameG18;

		public string nameG19;

		public string nameG20;

		public string nameG21;

		public string nameG22;

		public string nameG23;

		public string nameG24;

		public string nameG25;

		public string nameG26;

		public string nameG27;

		public string nameG28;

		public string nameG29;

		public string nameG30;

		public string nameG31;

		public string nameG32;

		public string nameG33;

		public string nameG34;

		public string nameG35;

		public string nameH00;

		public string nameH01;

		public string nameH02;

		public string nameH03;

		public string nameH04;

		public string nameH05;

		public string nameH06;

		public string nameH07;

		public string nameH08;

		public string nameH09;

		public string nameH10;

		public string nameH11;

		public string nameH12;

		public string nameH13;

		public string nameH14;

		public string nameH15;

		public string nameH16;

		public string nameH17;

		public string nameH18;

		public string nameH19;

		public string nameH20;

		public string nameH21;

		public string nameH22;

		public string nameH23;

		public string nameH24;

		public string nameH25;

		public string nameH26;

		public string nameH27;

		public string nameH28;

		public string nameH29;

		public string nameH30;

		public string nameH31;

		public string nameH32;

		public string nameH33;

		public string nameH34;

		public string nameH35;

		public int slot0Squadron;

		public int slot1Squadron;

		public int slot2Squadron;

		public int slot3Squadron;

		public int soundVolume;

		public int musicVolume;

		public int screenMode;
	}

	private enum State
	{
		squadronSelect,
		assigningSquadron,
		pilotSelect,
		resettingGroupNames
	}

	private GameWorld g;

	private StorageDevice device;

	private string containerName = "MyGamesStorage";

	private string filename = "mysave.sav";

	private bool savingEnabled = false;

	private bool isSaving = false;

	private bool isLoading = false;

	private float diskCounter = 0f;

	private bool errorSaving = false;

	private bool errorSavingMessageShown = false;

	private string tempString = "";

	private string doneString = "DONE";

	private string[] groupNames;

	private string[] squadronNames;

	private string[,] pilotNames;

	private Texture2D backgroundTexture;

	private Texture2D elementsTexture;

	private Vector2 screenPosition = new Vector2(0f, 0f);

	private int optionPosition = 0;

	private int currentSquadron = -1;

	private int slotSelectPosition = 0;

	private bool[] squadronIsActive;

	private Vector2 SQUADRONNAMEPOSITION = new Vector2(385f, 200f);

	private float SQUADRONXGAP = 500f;

	private float SQUADRONYGAP = 100f;

	private Vector2 PILOTNAMEPOSITION = new Vector2(225f, 151f);

	private float PILOTXGAP = 279f;

	private float PILOTYGAP = 50f;

	private float NAMEPLATEOFFSETX = 88f;

	private float NAMEPLATEOFFSETY = 124f;

	private float NAMEPLATEDISTANCEX = 280f;

	private float NAMEPLATEDISTANCEY = 50f;

	private float controllerFlashTimer = 0f;

	private State currentState = State.squadronSelect;

	private bool controlsDisabled = false;

	private IAsyncResult keyboardResult = null;

	private bool keyboardInputRequested = false;

	public EditSquadronScreen(GameWorld gw)
	{
		g = gw;
		squadronIsActive = new bool[8];
		for (int i = 0; i < squadronIsActive.Length; i++)
		{
			squadronIsActive[i] = false;
		}
		squadronNames = new string[g.squadrons.Length];
		groupNames = new string[g.squadrons.Length];
		pilotNames = new string[g.squadrons.Length, 36];
	}

	public void LoadContent(ContentManager theContentManager)
	{
		backgroundTexture = theContentManager.Load<Texture2D>("ScreenEditSquadrons");
		elementsTexture = TextureManager.GetElementsFrontEndTexture();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		Vector2 vector = new Vector2(0f, 0f);
		g.theFrontEnd.DrawScenery(theSpriteBatch);
		DrawControllerIcon(theSpriteBatch);
		if (currentState == State.squadronSelect)
		{
			DrawSquadrons(theSpriteBatch);
		}
		if (currentState == State.assigningSquadron)
		{
			DrawSquadrons(theSpriteBatch);
			if (currentState == State.assigningSquadron)
			{
				DrawAssignSquadron(theSpriteBatch);
			}
		}
		if (currentState == State.pilotSelect)
		{
			DrawPilots(theSpriteBatch);
		}
		if (currentState == State.resettingGroupNames)
		{
			DrawPilots(theSpriteBatch);
			DrawResetGroupNames(theSpriteBatch);
		}
		if (optionPosition < 8 && currentState == State.squadronSelect && !squadronIsActive[optionPosition])
		{
			ControlOverlay.DrawAssignSquadronYAB(theSpriteBatch);
		}
		else if (currentState == State.pilotSelect)
		{
			ControlOverlay.DrawResetGroupNamesYAB(theSpriteBatch);
		}
		else
		{
			ControlOverlay.DrawAB(theSpriteBatch);
		}
		if (isSaving)
		{
			ControlOverlay.DrawSaving(theSpriteBatch);
		}
		if (isLoading)
		{
			ControlOverlay.DrawLoading(theSpriteBatch);
		}
		if ((g.d.GetTrialModeCounter() != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode)
		{
			ControlOverlay.DrawSavingDisabledTrial(theSpriteBatch);
		}
		if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode) && !savingEnabled)
		{
			ControlOverlay.DrawSavingDisabled(theSpriteBatch);
		}
		if (errorSaving)
		{
			g.theOptionsOverlay.DrawErrorSaving(theSpriteBatch, new Vector2(650f, 360f), "SAVING DISABLED!");
		}
	}

	private void DrawControllerIcon(SpriteBatch theSpriteBatch)
	{
		Vector2 position = ((currentState != State.pilotSelect && currentState != State.resettingGroupNames) ? new Vector2(430f, 114f) : new Vector2(490f, 96f));
		Rectangle value = new Rectangle(4 + 51 * g.GetCurrentMenuController(), 569, 51, 51);
		Rectangle value2 = new Rectangle(208, 569, 51, 51);
		float num = (float)Math.Sin(controllerFlashTimer * 2f);
		if (num <= 0f)
		{
			controllerFlashTimer = 0f;
		}
		theSpriteBatch.Draw(elementsTexture, position, value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.Draw(elementsTexture, position, value2, Color.White * (Math.Abs(num) / 2f), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
	}

	private void DrawAssignSquadron(SpriteBatch theSpriteBatch)
	{
		Vector2 position = new Vector2(640f, 349f);
		SpriteFont font = g.theFontManager.GetFont();
		Rectangle value = new Rectangle(4, 294, 351, 136);
		theSpriteBatch.Draw(elementsTexture, position, value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		value = new Rectangle(356, 294, 78, 50);
		for (int i = 0; i < g.theSlots.Length; i++)
		{
			theSpriteBatch.Draw(elementsTexture, new Vector2(position.X - 124f + (float)(i * 83), position.Y + 30f), value, g.theSlots[i].GetCurrentSquadron().GetTheColor(), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			if (i != slotSelectPosition)
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(position.X - 124f + (float)(i * 83), position.Y + 30f), value, new Color(0f, 0f, 0f, 0.75f), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			}
		}
		value = new Rectangle(356, 350, 78, 50);
		theSpriteBatch.Draw(elementsTexture, new Vector2(position.X - 124f + (float)(slotSelectPosition * 83), position.Y + 30f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		string text = "ASSIGN A SQUADRON FOR";
		theSpriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 43f), Color.Black, 0f, font.MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
		text = g.squadrons[optionPosition].GetName().ToUpper();
		font = g.theFontManager.GetHiScoreFont();
		theSpriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 13f), Color.Black, 0f, font.MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
	}

	private void DrawResetGroupNames(SpriteBatch theSpriteBatch)
	{
		g.theOptionsOverlay.DrawYesNoOption(theSpriteBatch, new Vector2(643f, 375f), "RESET ALL NAMES\n   IN THIS GROUP?", "CONTINUE", "CANCEL");
	}

	public void DrawSquadrons(SpriteBatch theSpriteBatch)
	{
		SpriteFont hiScoreFont = g.theFontManager.GetHiScoreFont();
		Vector2 origin = new Vector2(0f, 0f);
		for (int i = 0; i < squadronIsActive.Length; i++)
		{
			squadronIsActive[i] = false;
		}
		Rectangle rectangle = new Rectangle(480, 56, 355, 50);
		string text = "EDIT PILOTS";
		General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameHeading2(), text, new Vector2(640f, 118f), new Color(225, 225, 0), new Color(255, 255, 0), new Color(195, 195, 0), 0f, g.theFontManager.GetFontGameHeading2().MeasureString(text) / 2f, 1f, 1, 1f);
		for (int i = 0; i < g.squadrons.Length; i++)
		{
			rectangle = new Rectangle(479, 55, 356, 50);
			theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
			rectangle = new Rectangle(472, 111, 370, 64);
			if (g.squadrons[i] == g.theSlots[0].GetCurrentSquadron())
			{
				rectangle = new Rectangle(479, 181, 356, 50);
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, g.squadrons[i].GetTheColor() * 0.5f, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				if (optionPosition == i)
				{
					rectangle = new Rectangle(479, 181, 356, 50);
					theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, g.squadrons[i].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				}
				rectangle = new Rectangle(472, 111, 370, 64);
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, g.squadrons[i].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				squadronIsActive[i] = true;
			}
			else if (g.squadrons[i] == g.theSlots[1].GetCurrentSquadron())
			{
				rectangle = new Rectangle(479, 181, 356, 50);
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, g.squadrons[i].GetTheColor() * 0.5f, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				if (optionPosition == i)
				{
					rectangle = new Rectangle(479, 181, 356, 50);
					theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, g.squadrons[i].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				}
				rectangle = new Rectangle(472, 111, 370, 64);
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, g.squadrons[i].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				squadronIsActive[i] = true;
			}
			else if (g.squadrons[i] == g.theSlots[2].GetCurrentSquadron())
			{
				rectangle = new Rectangle(479, 181, 356, 50);
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, g.squadrons[i].GetTheColor() * 0.5f, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				if (optionPosition == i)
				{
					rectangle = new Rectangle(479, 181, 356, 50);
					theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, g.squadrons[i].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				}
				rectangle = new Rectangle(472, 111, 370, 64);
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, g.squadrons[i].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				squadronIsActive[i] = true;
			}
			else if (g.squadrons[i] == g.theSlots[3].GetCurrentSquadron())
			{
				rectangle = new Rectangle(479, 181, 356, 50);
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, g.squadrons[i].GetTheColor() * 0.5f, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				if (optionPosition == i)
				{
					rectangle = new Rectangle(479, 181, 356, 50);
					theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, g.squadrons[i].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				}
				rectangle = new Rectangle(472, 111, 370, 64);
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(i % 4) - 1f), rectangle, g.squadrons[i].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
				squadronIsActive[i] = true;
			}
			tempString = squadronNames[i];
			General.DrawEmbossedString(theSpriteBatch, hiScoreFont, tempString, new Vector2(SQUADRONNAMEPOSITION.X + (float)(i / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + 2f + SQUADRONYGAP * (float)(i % 4)), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, hiScoreFont.MeasureString(tempString) / 2f, 1f, 1, 1f);
		}
		if (optionPosition < 8)
		{
			rectangle = new Rectangle(479, 181, 356, 50);
			theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, Color.White, 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
			if (g.squadrons[optionPosition] == g.theSlots[0].GetCurrentSquadron())
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, g.squadrons[optionPosition].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
			}
			if (g.squadrons[optionPosition] == g.theSlots[1].GetCurrentSquadron())
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, g.squadrons[optionPosition].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
			}
			if (g.squadrons[optionPosition] == g.theSlots[2].GetCurrentSquadron())
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, g.squadrons[optionPosition].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
			}
			if (g.squadrons[optionPosition] == g.theSlots[3].GetCurrentSquadron())
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + SQUADRONYGAP * (float)(optionPosition % 4) - 1f), rectangle, g.squadrons[optionPosition].GetTheColor(), 0f, new Vector2(rectangle.Width / 2, rectangle.Height / 2), 1f, SpriteEffects.None, 0f);
			}
			tempString = squadronNames[optionPosition];
			General.DrawEmbossedString(theSpriteBatch, hiScoreFont, tempString, new Vector2(SQUADRONNAMEPOSITION.X + (float)(optionPosition / 4) * SQUADRONXGAP, SQUADRONNAMEPOSITION.Y + 2f + SQUADRONYGAP * (float)(optionPosition % 4)), new Color((int)(225f * General.GetOptionThrobValue()), 0, 0), new Color((int)(255f * General.GetOptionThrobValue()), 0, 0), new Color((int)(195f * General.GetOptionThrobValue()), 0, 0), 0f, hiScoreFont.MeasureString(tempString) / 2f, 1f, 1, 1f);
		}
		theSpriteBatch.Draw(sourceRectangle: new Rectangle(480, 0, 274, 50), texture: elementsTexture, position: new Vector2(NAMEPLATEOFFSETX + 1.5f * NAMEPLATEDISTANCEX, NAMEPLATEOFFSETY + 9f * NAMEPLATEDISTANCEY), color: new Color(175, 175, 175), rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		General.DrawEmbossedString(theSpriteBatch, hiScoreFont, doneString, new Vector2(PILOTNAMEPOSITION.X + 1.5f * PILOTXGAP, PILOTNAMEPOSITION.Y + 1f + PILOTYGAP * 9f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, hiScoreFont.MeasureString(doneString) / 2f, 1f, 1, 1f);
		if (optionPosition == 8)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(5, 503, 274, 50), texture: elementsTexture, position: new Vector2(NAMEPLATEOFFSETX + 1.5f * NAMEPLATEDISTANCEX, NAMEPLATEOFFSETY + 9f * NAMEPLATEDISTANCEY), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			General.DrawEmbossedString(theSpriteBatch, hiScoreFont, doneString, new Vector2(PILOTNAMEPOSITION.X + 1.5f * PILOTXGAP, PILOTNAMEPOSITION.Y + 1f + PILOTYGAP * 9f), new Color((int)(225f * General.GetOptionThrobValue()), 0, 0), new Color((int)(255f * General.GetOptionThrobValue()), 0, 0), new Color((int)(195f * General.GetOptionThrobValue()), 0, 0), 0f, hiScoreFont.MeasureString(doneString) / 2f, 1f, 1, 1f);
		}
	}

	public void DrawPilots(SpriteBatch theSpriteBatch)
	{
		SpriteFont font = g.theFontManager.GetFont();
		Vector2 origin = new Vector2(0f, 0f);
		Rectangle rectangle = new Rectangle(480, 56, 355, 50);
		tempString = groupNames[currentSquadron];
		General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameHeading1(), tempString, new Vector2(640f, 100f), new Color(225, 225, 0), new Color(255, 255, 0), new Color(195, 195, 0), 0f, g.theFontManager.GetFontGameHeading1().MeasureString(tempString) / 2f, 1f, 1, 1f);
		rectangle = new Rectangle(480, 0, 274, 50);
		for (int i = 0; i < 36; i++)
		{
			tempString = pilotNames[currentSquadron, i];
			theSpriteBatch.Draw(elementsTexture, new Vector2(NAMEPLATEOFFSETX + (float)(i / 9) * NAMEPLATEDISTANCEX, NAMEPLATEOFFSETY + (float)(int)((float)(i % 9) * NAMEPLATEDISTANCEY)), rectangle, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(i / 9) * PILOTXGAP) - 1f, PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(i % 9)) - 1f), Color.Black, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(i / 9) * PILOTXGAP) + 1f, PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(i % 9)) - 1f), Color.Black, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(i / 9) * PILOTXGAP) - 1f, PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(i % 9)) + 1f), Color.Black, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(i / 9) * PILOTXGAP) + 1f, PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(i % 9)) + 1f), Color.Black, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(i / 9) * PILOTXGAP), PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(i % 9))), Color.White, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
		}
		if (optionPosition >= 0 && optionPosition < 36)
		{
			tempString = pilotNames[currentSquadron, optionPosition];
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(5, 503, 274, 50), texture: elementsTexture, position: new Vector2(NAMEPLATEOFFSETX + (float)(optionPosition / 9) * NAMEPLATEDISTANCEX, NAMEPLATEOFFSETY + (float)(int)((float)(optionPosition % 9) * NAMEPLATEDISTANCEY)), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(optionPosition / 9) * PILOTXGAP) - 1f, PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(optionPosition % 9)) - 1f), Color.Black, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(optionPosition / 9) * PILOTXGAP) + 1f, PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(optionPosition % 9)) - 1f), Color.Black, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(optionPosition / 9) * PILOTXGAP) - 1f, PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(optionPosition % 9)) + 1f), Color.Black, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(optionPosition / 9) * PILOTXGAP) + 1f, PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(optionPosition % 9)) + 1f), Color.Black, 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(font, tempString, new Vector2(PILOTNAMEPOSITION.X + (float)(int)((float)(optionPosition / 9) * PILOTXGAP), PILOTNAMEPOSITION.Y + (float)(int)(PILOTYGAP * (float)(optionPosition % 9))), new Color((int)(255f * General.GetOptionThrobValue()), 0, 0), 0f, font.MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
		}
		if (optionPosition == -1)
		{
			tempString = squadronNames[currentSquadron];
			theSpriteBatch.DrawString(g.theFontManager.GetHiScoreFont(), tempString, new Vector2(640f, 100f), Color.Red, 0f, g.theFontManager.GetHiScoreFont().MeasureString(tempString) / 2f, 1f, SpriteEffects.None, 0f);
		}
		rectangle = new Rectangle(480, 0, 274, 50);
		font = g.theFontManager.GetHiScoreFont();
		theSpriteBatch.Draw(elementsTexture, new Vector2(NAMEPLATEOFFSETX + (float)(int)(1.5f * NAMEPLATEDISTANCEX), NAMEPLATEOFFSETY + 9f * NAMEPLATEDISTANCEY), rectangle, new Color(175, 175, 175), 0f, origin, 1f, SpriteEffects.None, 0f);
		General.DrawEmbossedString(theSpriteBatch, font, doneString, new Vector2(PILOTNAMEPOSITION.X + 1.5f * PILOTXGAP, PILOTNAMEPOSITION.Y + 1f + PILOTYGAP * 9f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, font.MeasureString(doneString) / 2f, 1f, 1, 1f);
		if (optionPosition == 36)
		{
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(5, 503, 274, 50), texture: elementsTexture, position: new Vector2(NAMEPLATEOFFSETX + (float)(int)(1.5f * NAMEPLATEDISTANCEX), NAMEPLATEOFFSETY + 9f * NAMEPLATEDISTANCEY), color: Color.White, rotation: 0f, origin: origin, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			General.DrawEmbossedString(theSpriteBatch, font, doneString, new Vector2(PILOTNAMEPOSITION.X + 1.5f * PILOTXGAP, PILOTNAMEPOSITION.Y + 1f + PILOTYGAP * 9f), new Color((int)(225f * General.GetOptionThrobValue()), 0, 0), new Color((int)(255f * General.GetOptionThrobValue()), 0, 0), new Color((int)(195f * General.GetOptionThrobValue()), 0, 0), 0f, font.MeasureString(doneString) / 2f, 1f, 1, 1f);
		}
	}

	public void Update(GameTime theGameTime)
	{
		if (!Guide.IsVisible)
		{
			CheckControls(theGameTime);
		}
		if (keyboardInputRequested)
		{
			GetKeyboardInput();
		}
		UpdateDiskCounter(theGameTime);
		g.theFrontEnd.UpdateScenery(theGameTime);
		controllerFlashTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
	}

	public void UpdateDiskCounter(GameTime theGameTime)
	{
		if (isLoading || isSaving)
		{
			diskCounter += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if ((double)diskCounter >= 0.1)
			{
				isLoading = false;
				isSaving = false;
				diskCounter = 0f;
			}
		}
	}

	public void OptionSelected()
	{
		g.theSoundManager.MenuSelectSound();
		if (currentState == State.squadronSelect)
		{
			if (optionPosition < 8)
			{
				currentSquadron = optionPosition;
				optionPosition = 0;
				currentState = State.pilotSelect;
			}
			else if (optionPosition == 8)
			{
				if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode))
				{
					InitiateSave();
				}
				if (!errorSaving)
				{
					currentSquadron = -1;
					optionPosition = 0;
					g.SetCurrentMenuController(0);
					g.GoToFrontEnd();
				}
			}
		}
		else if (currentState == State.assigningSquadron)
		{
			g.theSlots[slotSelectPosition].SetCurrentSquadron(g.squadrons[optionPosition]);
			currentState = State.squadronSelect;
		}
		else if (currentState == State.pilotSelect)
		{
			if (optionPosition >= 0 && optionPosition < 36)
			{
				keyboardInputRequested = true;
			}
			else if (optionPosition == 36)
			{
				optionPosition = currentSquadron;
				currentSquadron = -1;
				currentState = State.squadronSelect;
			}
		}
		else if (currentState == State.resettingGroupNames)
		{
			if (g.theOptionsOverlay.GetOptionBooleanIsYes())
			{
				g.squadrons[currentSquadron].ResetMemberNames();
				currentState = State.pilotSelect;
				UpdateNames();
			}
			else
			{
				currentState = State.pilotSelect;
			}
		}
	}

	public void BackTrackSelected()
	{
		g.theSoundManager.MenuSelectSound();
		if (currentState == State.squadronSelect)
		{
			if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode))
			{
				InitiateSave();
			}
			if (!errorSaving)
			{
				currentSquadron = -1;
				optionPosition = 0;
				g.SetCurrentMenuController(0);
				g.GoToFrontEnd();
			}
		}
		else if (currentState == State.assigningSquadron)
		{
			currentState = State.squadronSelect;
		}
		else if (currentState == State.pilotSelect)
		{
			optionPosition = currentSquadron;
			currentSquadron = -1;
			currentState = State.squadronSelect;
		}
		else if (currentState == State.resettingGroupNames)
		{
			currentState = State.pilotSelect;
		}
	}

	public void CheckControls(GameTime theGameTime)
	{
		if (!errorSaving)
		{
			if (controlsDisabled)
			{
				return;
			}
			if (g.theControllerMenuManager[g.GetCurrentMenuController()].CheckLeftUpPressed())
			{
				OptionUp();
			}
			if (g.theControllerMenuManager[g.GetCurrentMenuController()].CheckLeftDownPressed())
			{
				OptionDown();
			}
			if (g.theControllerMenuManager[g.GetCurrentMenuController()].CheckLeftLeftPressed())
			{
				OptionLeft();
			}
			if (g.theControllerMenuManager[g.GetCurrentMenuController()].CheckLeftRightPressed())
			{
				OptionRight();
			}
			if (g.theControllerMenuManager[g.GetCurrentMenuController()].CheckButtonAPressed() || g.theControllerMenuManager[g.GetCurrentMenuController()].CheckButtonStartPressed())
			{
				OptionSelected();
			}
			if (g.theControllerMenuManager[g.GetCurrentMenuController()].CheckButtonBPressed() || g.theControllerMenuManager[g.GetCurrentMenuController()].CheckButtonBackPressed())
			{
				BackTrackSelected();
			}
			if (g.theControllerMenuManager[g.GetCurrentMenuController()].CheckButtonYPressed())
			{
				if (currentState == State.pilotSelect)
				{
					ResetGroupNamesSelected();
				}
				if (currentState == State.squadronSelect)
				{
					AssignSquadronSelected();
				}
			}
		}
		else if (g.theControllerMenuManager[g.GetCurrentMenuController()].CheckButtonAPressed() || g.theControllerMenuManager[g.GetCurrentMenuController()].CheckButtonStartPressed())
		{
			g.theSoundManager.MenuSelectSound();
			errorSaving = false;
			errorSavingMessageShown = true;
		}
	}

	public void AssignSquadronSelected()
	{
		if (optionPosition < squadronIsActive.Length && !squadronIsActive[optionPosition])
		{
			g.theSoundManager.MenuSelectSound();
			slotSelectPosition = 0;
			currentState = State.assigningSquadron;
		}
	}

	public void ResetGroupNamesSelected()
	{
		g.theSoundManager.MenuSelectSound();
		g.theOptionsOverlay.SetOptionBooleanIsYes(b: false);
		currentState = State.resettingGroupNames;
	}

	public void OptionUp()
	{
		if (currentState == State.squadronSelect)
		{
			if (optionPosition == 0 || optionPosition == 4)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition = 8;
			}
			else if (optionPosition != 4)
			{
				if (optionPosition == 8)
				{
					g.theSoundManager.MenuSwitchSound();
					optionPosition = 3;
				}
				else
				{
					g.theSoundManager.MenuSwitchSound();
					optionPosition--;
				}
			}
		}
		else if (currentState == State.pilotSelect)
		{
			if (optionPosition == 0 || optionPosition == 9 || optionPosition == 18 || optionPosition == 27)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition = 36;
			}
			else if (optionPosition >= 36)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition = 8;
			}
			else if (optionPosition != -1)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition--;
			}
		}
	}

	public void OptionDown()
	{
		if (currentState == State.squadronSelect)
		{
			if (optionPosition == 3 || optionPosition == 7)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition = 8;
			}
			else if (optionPosition == 8)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition = 0;
			}
			else
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition++;
			}
		}
		else if (currentState == State.pilotSelect)
		{
			if (optionPosition == 8 || optionPosition == 17 || optionPosition == 26 || optionPosition == 35)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition = 36;
			}
			else if (optionPosition == 36)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition = 0;
			}
			else
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition++;
			}
		}
	}

	public void OptionLeft()
	{
		if (currentState == State.squadronSelect && optionPosition >= 4 && optionPosition < 8)
		{
			g.theSoundManager.MenuSwitchSound();
			optionPosition -= 4;
		}
		if (currentState == State.assigningSquadron && slotSelectPosition > 0)
		{
			g.theSoundManager.MenuSwitchSound();
			slotSelectPosition--;
		}
		if (currentState == State.pilotSelect)
		{
			if (optionPosition >= 9 && optionPosition < 36)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition -= 9;
			}
			if (optionPosition == 42)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition--;
			}
		}
		if (currentState == State.resettingGroupNames && !g.theOptionsOverlay.GetOptionBooleanIsYes())
		{
			g.theSoundManager.MenuSwitchSound();
			g.theOptionsOverlay.SetOptionBooleanIsYes(b: true);
		}
	}

	public void OptionRight()
	{
		if (currentState == State.squadronSelect && optionPosition >= 0 && optionPosition < 4)
		{
			g.theSoundManager.MenuSwitchSound();
			optionPosition += 4;
		}
		if (currentState == State.assigningSquadron && slotSelectPosition < 3)
		{
			g.theSoundManager.MenuSwitchSound();
			slotSelectPosition++;
		}
		if (currentState == State.pilotSelect)
		{
			if (optionPosition >= 0 && optionPosition < 27)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition += 9;
			}
			if (optionPosition == 41)
			{
				g.theSoundManager.MenuSwitchSound();
				optionPosition++;
			}
		}
		if (currentState == State.resettingGroupNames && g.theOptionsOverlay.GetOptionBooleanIsYes())
		{
			g.theSoundManager.MenuSwitchSound();
			g.theOptionsOverlay.SetOptionBooleanIsYes(b: false);
		}
	}

	public void ResetAllNames()
	{
		for (int i = 0; i < g.squadrons.Length; i++)
		{
			g.squadrons[i].ResetMemberNames();
		}
	}

	public void UpdateNames()
	{
		for (int i = 0; i < g.squadrons.Length; i++)
		{
			squadronNames[i] = g.squadrons[i].GetName();
			groupNames[i] = g.squadrons[i].GetName().ToUpper();
			for (int j = 0; j < 36; j++)
			{
				pilotNames[i, j] = g.squadrons[i].GetMember(j).GetName();
			}
		}
	}

	public bool GetSavingEnabled()
	{
		return savingEnabled;
	}

	public void SetSavingEnabled(bool b)
	{
		savingEnabled = b;
	}

	public void GetKeyboardInput()
	{
		controlsDisabled = true;
		string title = "Edit Pilot";
		string description = "Enter name (8 characters or less)";
		string name = g.squadrons[currentSquadron].GetMember(optionPosition).GetName();
		if (keyboardResult == null && !Guide.IsVisible)
		{
			keyboardResult = Guide.BeginShowKeyboardInput(g.theControllerMenuManager[g.GetCurrentMenuController()].GetPlayerIndex(), title, description, name, null, null);
		}
		else
		{
			if (keyboardResult == null || !keyboardResult.IsCompleted)
			{
				return;
			}
			string text = Guide.EndShowKeyboardInput(keyboardResult);
			if (text != null)
			{
				string text2 = text.ToString().Trim();
				if (text2.Length > 8)
				{
					text2 = text2.Remove(8);
				}
				text2 = Regex.Replace(text2, "[^\\w]", "");
				char[] array = new char[52]
				{
					'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
					'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
					'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
					'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
					'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
					'Y', 'Z'
				};
				for (int i = 0; i < text2.Length; i++)
				{
					bool flag = false;
					do
					{
						flag = false;
						char[] array2 = array;
						foreach (char c in array2)
						{
							if (text2.ElementAt(i) == c)
							{
								flag = true;
							}
						}
						if (!flag)
						{
							text2 = text2.Replace(text2.ElementAt(i).ToString(), "");
						}
					}
					while (!flag && text2 != "" && text2.Length > i);
				}
				for (int i = 0; i < text2.Length; i++)
				{
					while (text2 != "")
					{
						text2.ElementAt(i);
						if (text2.ElementAt(i) >= ' ' && text2.ElementAt(i) <= '~')
						{
							break;
						}
						text2 = text2.Replace(text2.ElementAt(i).ToString(), "");
					}
				}
				if (text2 != "")
				{
					string text3 = text2.ElementAt(0).ToString().ToUpper();
					string text4 = text2.ToString().Substring(1).ToLower();
					string text5 = text3 + text4;
					g.squadrons[currentSquadron].GetMember(optionPosition).SetCustomName(text5);
					g.NameList.SetName(currentSquadron, optionPosition, text5);
				}
			}
			keyboardInputRequested = false;
			keyboardResult = null;
			controlsDisabled = false;
			UpdateNames();
			if (!((g.d.trialModeCounter != null) ? g.d.trialModeCounter.IsTrialMode : Guide.IsTrialMode))
			{
				InitiateSave();
			}
		}
	}

	public void InitiateSave()
	{
		try
		{
			if (device != null)
			{
				if (device.IsConnected)
				{
					DoSaveGame(device);
				}
				else if (!errorSavingMessageShown)
				{
					errorSaving = true;
					savingEnabled = false;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void InitiateLoad()
	{
		if (!Guide.IsVisible)
		{
			errorSaving = false;
			errorSavingMessageShown = false;
			device = null;
			try
			{
				StorageDevice.BeginShowSelector(GetLoadDevice, null);
			}
			catch (Exception)
			{
			}
		}
	}

	public void GetSaveDevice(IAsyncResult result)
	{
		try
		{
			device = StorageDevice.EndShowSelector(result);
			if (device != null && device.IsConnected)
			{
				try
				{
					DoSaveGame(device);
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void GetLoadDevice(IAsyncResult result)
	{
		try
		{
			device = StorageDevice.EndShowSelector(result);
			if (device != null && device.IsConnected)
			{
				try
				{
					savingEnabled = true;
					DoLoadGame(device);
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void DoSaveGame(StorageDevice device)
	{
		//IL_1e36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3d: Expected O, but got Unknown
		isSaving = true;
		SaveGameData saveGameData = default(SaveGameData);
		saveGameData.nameA00 = g.NameList.GetName(0, 0);
		saveGameData.nameA01 = g.NameList.GetName(0, 1);
		saveGameData.nameA02 = g.NameList.GetName(0, 2);
		saveGameData.nameA03 = g.NameList.GetName(0, 3);
		saveGameData.nameA04 = g.NameList.GetName(0, 4);
		saveGameData.nameA05 = g.NameList.GetName(0, 5);
		saveGameData.nameA06 = g.NameList.GetName(0, 6);
		saveGameData.nameA07 = g.NameList.GetName(0, 7);
		saveGameData.nameA08 = g.NameList.GetName(0, 8);
		saveGameData.nameA09 = g.NameList.GetName(0, 9);
		saveGameData.nameA10 = g.NameList.GetName(0, 10);
		saveGameData.nameA11 = g.NameList.GetName(0, 11);
		saveGameData.nameA12 = g.NameList.GetName(0, 12);
		saveGameData.nameA13 = g.NameList.GetName(0, 13);
		saveGameData.nameA14 = g.NameList.GetName(0, 14);
		saveGameData.nameA15 = g.NameList.GetName(0, 15);
		saveGameData.nameA16 = g.NameList.GetName(0, 16);
		saveGameData.nameA17 = g.NameList.GetName(0, 17);
		saveGameData.nameA18 = g.NameList.GetName(0, 18);
		saveGameData.nameA19 = g.NameList.GetName(0, 19);
		saveGameData.nameA20 = g.NameList.GetName(0, 20);
		saveGameData.nameA21 = g.NameList.GetName(0, 21);
		saveGameData.nameA22 = g.NameList.GetName(0, 22);
		saveGameData.nameA23 = g.NameList.GetName(0, 23);
		saveGameData.nameA24 = g.NameList.GetName(0, 24);
		saveGameData.nameA25 = g.NameList.GetName(0, 25);
		saveGameData.nameA26 = g.NameList.GetName(0, 26);
		saveGameData.nameA27 = g.NameList.GetName(0, 27);
		saveGameData.nameA28 = g.NameList.GetName(0, 28);
		saveGameData.nameA29 = g.NameList.GetName(0, 29);
		saveGameData.nameA30 = g.NameList.GetName(0, 30);
		saveGameData.nameA31 = g.NameList.GetName(0, 31);
		saveGameData.nameA32 = g.NameList.GetName(0, 32);
		saveGameData.nameA33 = g.NameList.GetName(0, 33);
		saveGameData.nameA34 = g.NameList.GetName(0, 34);
		saveGameData.nameA35 = g.NameList.GetName(0, 35);
		saveGameData.nameB00 = g.NameList.GetName(1, 0);
		saveGameData.nameB01 = g.NameList.GetName(1, 1);
		saveGameData.nameB02 = g.NameList.GetName(1, 2);
		saveGameData.nameB03 = g.NameList.GetName(1, 3);
		saveGameData.nameB04 = g.NameList.GetName(1, 4);
		saveGameData.nameB05 = g.NameList.GetName(1, 5);
		saveGameData.nameB06 = g.NameList.GetName(1, 6);
		saveGameData.nameB07 = g.NameList.GetName(1, 7);
		saveGameData.nameB08 = g.NameList.GetName(1, 8);
		saveGameData.nameB09 = g.NameList.GetName(1, 9);
		saveGameData.nameB10 = g.NameList.GetName(1, 10);
		saveGameData.nameB11 = g.NameList.GetName(1, 11);
		saveGameData.nameB12 = g.NameList.GetName(1, 12);
		saveGameData.nameB13 = g.NameList.GetName(1, 13);
		saveGameData.nameB14 = g.NameList.GetName(1, 14);
		saveGameData.nameB15 = g.NameList.GetName(1, 15);
		saveGameData.nameB16 = g.NameList.GetName(1, 16);
		saveGameData.nameB17 = g.NameList.GetName(1, 17);
		saveGameData.nameB18 = g.NameList.GetName(1, 18);
		saveGameData.nameB19 = g.NameList.GetName(1, 19);
		saveGameData.nameB20 = g.NameList.GetName(1, 20);
		saveGameData.nameB21 = g.NameList.GetName(1, 21);
		saveGameData.nameB22 = g.NameList.GetName(1, 22);
		saveGameData.nameB23 = g.NameList.GetName(1, 23);
		saveGameData.nameB24 = g.NameList.GetName(1, 24);
		saveGameData.nameB25 = g.NameList.GetName(1, 25);
		saveGameData.nameB26 = g.NameList.GetName(1, 26);
		saveGameData.nameB27 = g.NameList.GetName(1, 27);
		saveGameData.nameB28 = g.NameList.GetName(1, 28);
		saveGameData.nameB29 = g.NameList.GetName(1, 29);
		saveGameData.nameB30 = g.NameList.GetName(1, 30);
		saveGameData.nameB31 = g.NameList.GetName(1, 31);
		saveGameData.nameB32 = g.NameList.GetName(1, 32);
		saveGameData.nameB33 = g.NameList.GetName(1, 33);
		saveGameData.nameB34 = g.NameList.GetName(1, 34);
		saveGameData.nameB35 = g.NameList.GetName(1, 35);
		saveGameData.nameC00 = g.NameList.GetName(2, 0);
		saveGameData.nameC01 = g.NameList.GetName(2, 1);
		saveGameData.nameC02 = g.NameList.GetName(2, 2);
		saveGameData.nameC03 = g.NameList.GetName(2, 3);
		saveGameData.nameC04 = g.NameList.GetName(2, 4);
		saveGameData.nameC05 = g.NameList.GetName(2, 5);
		saveGameData.nameC06 = g.NameList.GetName(2, 6);
		saveGameData.nameC07 = g.NameList.GetName(2, 7);
		saveGameData.nameC08 = g.NameList.GetName(2, 8);
		saveGameData.nameC09 = g.NameList.GetName(2, 9);
		saveGameData.nameC10 = g.NameList.GetName(2, 10);
		saveGameData.nameC11 = g.NameList.GetName(2, 11);
		saveGameData.nameC12 = g.NameList.GetName(2, 12);
		saveGameData.nameC13 = g.NameList.GetName(2, 13);
		saveGameData.nameC14 = g.NameList.GetName(2, 14);
		saveGameData.nameC15 = g.NameList.GetName(2, 15);
		saveGameData.nameC16 = g.NameList.GetName(2, 16);
		saveGameData.nameC17 = g.NameList.GetName(2, 17);
		saveGameData.nameC18 = g.NameList.GetName(2, 18);
		saveGameData.nameC19 = g.NameList.GetName(2, 19);
		saveGameData.nameC20 = g.NameList.GetName(2, 20);
		saveGameData.nameC21 = g.NameList.GetName(2, 21);
		saveGameData.nameC22 = g.NameList.GetName(2, 22);
		saveGameData.nameC23 = g.NameList.GetName(2, 23);
		saveGameData.nameC24 = g.NameList.GetName(2, 24);
		saveGameData.nameC25 = g.NameList.GetName(2, 25);
		saveGameData.nameC26 = g.NameList.GetName(2, 26);
		saveGameData.nameC27 = g.NameList.GetName(2, 27);
		saveGameData.nameC28 = g.NameList.GetName(2, 28);
		saveGameData.nameC29 = g.NameList.GetName(2, 29);
		saveGameData.nameC30 = g.NameList.GetName(2, 30);
		saveGameData.nameC31 = g.NameList.GetName(2, 31);
		saveGameData.nameC32 = g.NameList.GetName(2, 32);
		saveGameData.nameC33 = g.NameList.GetName(2, 33);
		saveGameData.nameC34 = g.NameList.GetName(2, 34);
		saveGameData.nameC35 = g.NameList.GetName(2, 35);
		saveGameData.nameD00 = g.NameList.GetName(3, 0);
		saveGameData.nameD01 = g.NameList.GetName(3, 1);
		saveGameData.nameD02 = g.NameList.GetName(3, 2);
		saveGameData.nameD03 = g.NameList.GetName(3, 3);
		saveGameData.nameD04 = g.NameList.GetName(3, 4);
		saveGameData.nameD05 = g.NameList.GetName(3, 5);
		saveGameData.nameD06 = g.NameList.GetName(3, 6);
		saveGameData.nameD07 = g.NameList.GetName(3, 7);
		saveGameData.nameD08 = g.NameList.GetName(3, 8);
		saveGameData.nameD09 = g.NameList.GetName(3, 9);
		saveGameData.nameD10 = g.NameList.GetName(3, 10);
		saveGameData.nameD11 = g.NameList.GetName(3, 11);
		saveGameData.nameD12 = g.NameList.GetName(3, 12);
		saveGameData.nameD13 = g.NameList.GetName(3, 13);
		saveGameData.nameD14 = g.NameList.GetName(3, 14);
		saveGameData.nameD15 = g.NameList.GetName(3, 15);
		saveGameData.nameD16 = g.NameList.GetName(3, 16);
		saveGameData.nameD17 = g.NameList.GetName(3, 17);
		saveGameData.nameD18 = g.NameList.GetName(3, 18);
		saveGameData.nameD19 = g.NameList.GetName(3, 19);
		saveGameData.nameD20 = g.NameList.GetName(3, 20);
		saveGameData.nameD21 = g.NameList.GetName(3, 21);
		saveGameData.nameD22 = g.NameList.GetName(3, 22);
		saveGameData.nameD23 = g.NameList.GetName(3, 23);
		saveGameData.nameD24 = g.NameList.GetName(3, 24);
		saveGameData.nameD25 = g.NameList.GetName(3, 25);
		saveGameData.nameD26 = g.NameList.GetName(3, 26);
		saveGameData.nameD27 = g.NameList.GetName(3, 27);
		saveGameData.nameD28 = g.NameList.GetName(3, 28);
		saveGameData.nameD29 = g.NameList.GetName(3, 29);
		saveGameData.nameD30 = g.NameList.GetName(3, 30);
		saveGameData.nameD31 = g.NameList.GetName(3, 31);
		saveGameData.nameD32 = g.NameList.GetName(3, 32);
		saveGameData.nameD33 = g.NameList.GetName(3, 33);
		saveGameData.nameD34 = g.NameList.GetName(3, 34);
		saveGameData.nameD35 = g.NameList.GetName(3, 35);
		saveGameData.nameE00 = g.NameList.GetName(4, 0);
		saveGameData.nameE01 = g.NameList.GetName(4, 1);
		saveGameData.nameE02 = g.NameList.GetName(4, 2);
		saveGameData.nameE03 = g.NameList.GetName(4, 3);
		saveGameData.nameE04 = g.NameList.GetName(4, 4);
		saveGameData.nameE05 = g.NameList.GetName(4, 5);
		saveGameData.nameE06 = g.NameList.GetName(4, 6);
		saveGameData.nameE07 = g.NameList.GetName(4, 7);
		saveGameData.nameE08 = g.NameList.GetName(4, 8);
		saveGameData.nameE09 = g.NameList.GetName(4, 9);
		saveGameData.nameE10 = g.NameList.GetName(4, 10);
		saveGameData.nameE11 = g.NameList.GetName(4, 11);
		saveGameData.nameE12 = g.NameList.GetName(4, 12);
		saveGameData.nameE13 = g.NameList.GetName(4, 13);
		saveGameData.nameE14 = g.NameList.GetName(4, 14);
		saveGameData.nameE15 = g.NameList.GetName(4, 15);
		saveGameData.nameE16 = g.NameList.GetName(4, 16);
		saveGameData.nameE17 = g.NameList.GetName(4, 17);
		saveGameData.nameE18 = g.NameList.GetName(4, 18);
		saveGameData.nameE19 = g.NameList.GetName(4, 19);
		saveGameData.nameE20 = g.NameList.GetName(4, 20);
		saveGameData.nameE21 = g.NameList.GetName(4, 21);
		saveGameData.nameE22 = g.NameList.GetName(4, 22);
		saveGameData.nameE23 = g.NameList.GetName(4, 23);
		saveGameData.nameE24 = g.NameList.GetName(4, 24);
		saveGameData.nameE25 = g.NameList.GetName(4, 25);
		saveGameData.nameE26 = g.NameList.GetName(4, 26);
		saveGameData.nameE27 = g.NameList.GetName(4, 27);
		saveGameData.nameE28 = g.NameList.GetName(4, 28);
		saveGameData.nameE29 = g.NameList.GetName(4, 29);
		saveGameData.nameE30 = g.NameList.GetName(4, 30);
		saveGameData.nameE31 = g.NameList.GetName(4, 31);
		saveGameData.nameE32 = g.NameList.GetName(4, 32);
		saveGameData.nameE33 = g.NameList.GetName(4, 33);
		saveGameData.nameE34 = g.NameList.GetName(4, 34);
		saveGameData.nameE35 = g.NameList.GetName(4, 35);
		saveGameData.nameF00 = g.NameList.GetName(5, 0);
		saveGameData.nameF01 = g.NameList.GetName(5, 1);
		saveGameData.nameF02 = g.NameList.GetName(5, 2);
		saveGameData.nameF03 = g.NameList.GetName(5, 3);
		saveGameData.nameF04 = g.NameList.GetName(5, 4);
		saveGameData.nameF05 = g.NameList.GetName(5, 5);
		saveGameData.nameF06 = g.NameList.GetName(5, 6);
		saveGameData.nameF07 = g.NameList.GetName(5, 7);
		saveGameData.nameF08 = g.NameList.GetName(5, 8);
		saveGameData.nameF09 = g.NameList.GetName(5, 9);
		saveGameData.nameF10 = g.NameList.GetName(5, 10);
		saveGameData.nameF11 = g.NameList.GetName(5, 11);
		saveGameData.nameF12 = g.NameList.GetName(5, 12);
		saveGameData.nameF13 = g.NameList.GetName(5, 13);
		saveGameData.nameF14 = g.NameList.GetName(5, 14);
		saveGameData.nameF15 = g.NameList.GetName(5, 15);
		saveGameData.nameF16 = g.NameList.GetName(5, 16);
		saveGameData.nameF17 = g.NameList.GetName(5, 17);
		saveGameData.nameF18 = g.NameList.GetName(5, 18);
		saveGameData.nameF19 = g.NameList.GetName(5, 19);
		saveGameData.nameF20 = g.NameList.GetName(5, 20);
		saveGameData.nameF21 = g.NameList.GetName(5, 21);
		saveGameData.nameF22 = g.NameList.GetName(5, 22);
		saveGameData.nameF23 = g.NameList.GetName(5, 23);
		saveGameData.nameF24 = g.NameList.GetName(5, 24);
		saveGameData.nameF25 = g.NameList.GetName(5, 25);
		saveGameData.nameF26 = g.NameList.GetName(5, 26);
		saveGameData.nameF27 = g.NameList.GetName(5, 27);
		saveGameData.nameF28 = g.NameList.GetName(5, 28);
		saveGameData.nameF29 = g.NameList.GetName(5, 29);
		saveGameData.nameF30 = g.NameList.GetName(5, 30);
		saveGameData.nameF31 = g.NameList.GetName(5, 31);
		saveGameData.nameF32 = g.NameList.GetName(5, 32);
		saveGameData.nameF33 = g.NameList.GetName(5, 33);
		saveGameData.nameF34 = g.NameList.GetName(5, 34);
		saveGameData.nameF35 = g.NameList.GetName(5, 35);
		saveGameData.nameG00 = g.NameList.GetName(6, 0);
		saveGameData.nameG01 = g.NameList.GetName(6, 1);
		saveGameData.nameG02 = g.NameList.GetName(6, 2);
		saveGameData.nameG03 = g.NameList.GetName(6, 3);
		saveGameData.nameG04 = g.NameList.GetName(6, 4);
		saveGameData.nameG05 = g.NameList.GetName(6, 5);
		saveGameData.nameG06 = g.NameList.GetName(6, 6);
		saveGameData.nameG07 = g.NameList.GetName(6, 7);
		saveGameData.nameG08 = g.NameList.GetName(6, 8);
		saveGameData.nameG09 = g.NameList.GetName(6, 9);
		saveGameData.nameG10 = g.NameList.GetName(6, 10);
		saveGameData.nameG11 = g.NameList.GetName(6, 11);
		saveGameData.nameG12 = g.NameList.GetName(6, 12);
		saveGameData.nameG13 = g.NameList.GetName(6, 13);
		saveGameData.nameG14 = g.NameList.GetName(6, 14);
		saveGameData.nameG15 = g.NameList.GetName(6, 15);
		saveGameData.nameG16 = g.NameList.GetName(6, 16);
		saveGameData.nameG17 = g.NameList.GetName(6, 17);
		saveGameData.nameG18 = g.NameList.GetName(6, 18);
		saveGameData.nameG19 = g.NameList.GetName(6, 19);
		saveGameData.nameG20 = g.NameList.GetName(6, 20);
		saveGameData.nameG21 = g.NameList.GetName(6, 21);
		saveGameData.nameG22 = g.NameList.GetName(6, 22);
		saveGameData.nameG23 = g.NameList.GetName(6, 23);
		saveGameData.nameG24 = g.NameList.GetName(6, 24);
		saveGameData.nameG25 = g.NameList.GetName(6, 25);
		saveGameData.nameG26 = g.NameList.GetName(6, 26);
		saveGameData.nameG27 = g.NameList.GetName(6, 27);
		saveGameData.nameG28 = g.NameList.GetName(6, 28);
		saveGameData.nameG29 = g.NameList.GetName(6, 29);
		saveGameData.nameG30 = g.NameList.GetName(6, 30);
		saveGameData.nameG31 = g.NameList.GetName(6, 31);
		saveGameData.nameG32 = g.NameList.GetName(6, 32);
		saveGameData.nameG33 = g.NameList.GetName(6, 33);
		saveGameData.nameG34 = g.NameList.GetName(6, 34);
		saveGameData.nameG35 = g.NameList.GetName(6, 35);
		saveGameData.nameH00 = g.NameList.GetName(7, 0);
		saveGameData.nameH01 = g.NameList.GetName(7, 1);
		saveGameData.nameH02 = g.NameList.GetName(7, 2);
		saveGameData.nameH03 = g.NameList.GetName(7, 3);
		saveGameData.nameH04 = g.NameList.GetName(7, 4);
		saveGameData.nameH05 = g.NameList.GetName(7, 5);
		saveGameData.nameH06 = g.NameList.GetName(7, 6);
		saveGameData.nameH07 = g.NameList.GetName(7, 7);
		saveGameData.nameH08 = g.NameList.GetName(7, 8);
		saveGameData.nameH09 = g.NameList.GetName(7, 9);
		saveGameData.nameH10 = g.NameList.GetName(7, 10);
		saveGameData.nameH11 = g.NameList.GetName(7, 11);
		saveGameData.nameH12 = g.NameList.GetName(7, 12);
		saveGameData.nameH13 = g.NameList.GetName(7, 13);
		saveGameData.nameH14 = g.NameList.GetName(7, 14);
		saveGameData.nameH15 = g.NameList.GetName(7, 15);
		saveGameData.nameH16 = g.NameList.GetName(7, 16);
		saveGameData.nameH17 = g.NameList.GetName(7, 17);
		saveGameData.nameH18 = g.NameList.GetName(7, 18);
		saveGameData.nameH19 = g.NameList.GetName(7, 19);
		saveGameData.nameH20 = g.NameList.GetName(7, 20);
		saveGameData.nameH21 = g.NameList.GetName(7, 21);
		saveGameData.nameH22 = g.NameList.GetName(7, 22);
		saveGameData.nameH23 = g.NameList.GetName(7, 23);
		saveGameData.nameH24 = g.NameList.GetName(7, 24);
		saveGameData.nameH25 = g.NameList.GetName(7, 25);
		saveGameData.nameH26 = g.NameList.GetName(7, 26);
		saveGameData.nameH27 = g.NameList.GetName(7, 27);
		saveGameData.nameH28 = g.NameList.GetName(7, 28);
		saveGameData.nameH29 = g.NameList.GetName(7, 29);
		saveGameData.nameH30 = g.NameList.GetName(7, 30);
		saveGameData.nameH31 = g.NameList.GetName(7, 31);
		saveGameData.nameH32 = g.NameList.GetName(7, 32);
		saveGameData.nameH33 = g.NameList.GetName(7, 33);
		saveGameData.nameH34 = g.NameList.GetName(7, 34);
		saveGameData.nameH35 = g.NameList.GetName(7, 35);
		saveGameData.slot0Squadron = g.theSlots[0].GetCurrentSquadron().GetSquadronNumber();
		saveGameData.slot1Squadron = g.theSlots[1].GetCurrentSquadron().GetSquadronNumber();
		saveGameData.slot2Squadron = g.theSlots[2].GetCurrentSquadron().GetSquadronNumber();
		saveGameData.slot3Squadron = g.theSlots[3].GetCurrentSquadron().GetSquadronNumber();
		saveGameData.soundVolume = (int)(g.theSoundManager.GetVolumeSFX() * 10f);
		saveGameData.musicVolume = (int)(g.theSoundManager.GetVolumeMusic() * 10f);
		saveGameData.screenMode = g.theSafeArea.GetScreenMode();
		SaveGameData saveGameData2 = saveGameData;
		try
		{
			IAsyncResult asyncResult = device.BeginOpenContainer("Squadron Scramble Save Data", null, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			StorageContainer storageContainer = device.EndOpenContainer(asyncResult);
			asyncResult.AsyncWaitHandle.Close();
			string file = "Squadron Scramble Save Data.sav";
			if (storageContainer.FileExists(file))
			{
				storageContainer.DeleteFile(file);
			}
			Stream stream = storageContainer.CreateFile(file);
			XmlSerializer val = new XmlSerializer(typeof(SaveGameData));
			val.Serialize(stream, (object)saveGameData2);
			stream.Close();
			storageContainer.Dispose();
		}
		catch (Exception)
		{
			if (!errorSavingMessageShown)
			{
				errorSaving = true;
				savingEnabled = false;
			}
			isSaving = false;
		}
		SetSquadronNames();
	}

	private void DoLoadGame(StorageDevice device)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		isLoading = true;
		try
		{
			IAsyncResult asyncResult = device.BeginOpenContainer("Squadron Scramble Save Data", null, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			StorageContainer storageContainer = device.EndOpenContainer(asyncResult);
			asyncResult.AsyncWaitHandle.Close();
			string file = "Squadron Scramble Save Data.sav";
			if (!storageContainer.FileExists(file))
			{
				storageContainer.Dispose();
				ResetAllSaveVariables();
				return;
			}
			Stream stream = storageContainer.OpenFile(file, FileMode.Open);
			XmlSerializer val = new XmlSerializer(typeof(SaveGameData));
			SaveGameData saveGameData = (SaveGameData)val.Deserialize(stream);
			stream.Close();
			storageContainer.Dispose();
			g.NameList.SetName(0, 0, saveGameData.nameA00);
			g.NameList.SetName(0, 1, saveGameData.nameA01);
			g.NameList.SetName(0, 2, saveGameData.nameA02);
			g.NameList.SetName(0, 3, saveGameData.nameA03);
			g.NameList.SetName(0, 4, saveGameData.nameA04);
			g.NameList.SetName(0, 5, saveGameData.nameA05);
			g.NameList.SetName(0, 6, saveGameData.nameA06);
			g.NameList.SetName(0, 7, saveGameData.nameA07);
			g.NameList.SetName(0, 8, saveGameData.nameA08);
			g.NameList.SetName(0, 9, saveGameData.nameA09);
			g.NameList.SetName(0, 10, saveGameData.nameA10);
			g.NameList.SetName(0, 11, saveGameData.nameA11);
			g.NameList.SetName(0, 12, saveGameData.nameA12);
			g.NameList.SetName(0, 13, saveGameData.nameA13);
			g.NameList.SetName(0, 14, saveGameData.nameA14);
			g.NameList.SetName(0, 15, saveGameData.nameA15);
			g.NameList.SetName(0, 16, saveGameData.nameA16);
			g.NameList.SetName(0, 17, saveGameData.nameA17);
			g.NameList.SetName(0, 18, saveGameData.nameA18);
			g.NameList.SetName(0, 19, saveGameData.nameA19);
			g.NameList.SetName(0, 20, saveGameData.nameA20);
			g.NameList.SetName(0, 21, saveGameData.nameA21);
			g.NameList.SetName(0, 22, saveGameData.nameA22);
			g.NameList.SetName(0, 23, saveGameData.nameA23);
			g.NameList.SetName(0, 24, saveGameData.nameA24);
			g.NameList.SetName(0, 25, saveGameData.nameA25);
			g.NameList.SetName(0, 26, saveGameData.nameA26);
			g.NameList.SetName(0, 27, saveGameData.nameA27);
			g.NameList.SetName(0, 28, saveGameData.nameA28);
			g.NameList.SetName(0, 29, saveGameData.nameA29);
			g.NameList.SetName(0, 30, saveGameData.nameA30);
			g.NameList.SetName(0, 31, saveGameData.nameA31);
			g.NameList.SetName(0, 32, saveGameData.nameA32);
			g.NameList.SetName(0, 33, saveGameData.nameA33);
			g.NameList.SetName(0, 34, saveGameData.nameA34);
			g.NameList.SetName(0, 35, saveGameData.nameA35);
			g.NameList.SetName(1, 0, saveGameData.nameB00);
			g.NameList.SetName(1, 1, saveGameData.nameB01);
			g.NameList.SetName(1, 2, saveGameData.nameB02);
			g.NameList.SetName(1, 3, saveGameData.nameB03);
			g.NameList.SetName(1, 4, saveGameData.nameB04);
			g.NameList.SetName(1, 5, saveGameData.nameB05);
			g.NameList.SetName(1, 6, saveGameData.nameB06);
			g.NameList.SetName(1, 7, saveGameData.nameB07);
			g.NameList.SetName(1, 8, saveGameData.nameB08);
			g.NameList.SetName(1, 9, saveGameData.nameB09);
			g.NameList.SetName(1, 10, saveGameData.nameB10);
			g.NameList.SetName(1, 11, saveGameData.nameB11);
			g.NameList.SetName(1, 12, saveGameData.nameB12);
			g.NameList.SetName(1, 13, saveGameData.nameB13);
			g.NameList.SetName(1, 14, saveGameData.nameB14);
			g.NameList.SetName(1, 15, saveGameData.nameB15);
			g.NameList.SetName(1, 16, saveGameData.nameB16);
			g.NameList.SetName(1, 17, saveGameData.nameB17);
			g.NameList.SetName(1, 18, saveGameData.nameB18);
			g.NameList.SetName(1, 19, saveGameData.nameB19);
			g.NameList.SetName(1, 20, saveGameData.nameB20);
			g.NameList.SetName(1, 21, saveGameData.nameB21);
			g.NameList.SetName(1, 22, saveGameData.nameB22);
			g.NameList.SetName(1, 23, saveGameData.nameB23);
			g.NameList.SetName(1, 24, saveGameData.nameB24);
			g.NameList.SetName(1, 25, saveGameData.nameB25);
			g.NameList.SetName(1, 26, saveGameData.nameB26);
			g.NameList.SetName(1, 27, saveGameData.nameB27);
			g.NameList.SetName(1, 28, saveGameData.nameB28);
			g.NameList.SetName(1, 29, saveGameData.nameB29);
			g.NameList.SetName(1, 30, saveGameData.nameB30);
			g.NameList.SetName(1, 31, saveGameData.nameB31);
			g.NameList.SetName(1, 32, saveGameData.nameB32);
			g.NameList.SetName(1, 33, saveGameData.nameB33);
			g.NameList.SetName(1, 34, saveGameData.nameB34);
			g.NameList.SetName(1, 35, saveGameData.nameB35);
			g.NameList.SetName(2, 0, saveGameData.nameC00);
			g.NameList.SetName(2, 1, saveGameData.nameC01);
			g.NameList.SetName(2, 2, saveGameData.nameC02);
			g.NameList.SetName(2, 3, saveGameData.nameC03);
			g.NameList.SetName(2, 4, saveGameData.nameC04);
			g.NameList.SetName(2, 5, saveGameData.nameC05);
			g.NameList.SetName(2, 6, saveGameData.nameC06);
			g.NameList.SetName(2, 7, saveGameData.nameC07);
			g.NameList.SetName(2, 8, saveGameData.nameC08);
			g.NameList.SetName(2, 9, saveGameData.nameC09);
			g.NameList.SetName(2, 10, saveGameData.nameC10);
			g.NameList.SetName(2, 11, saveGameData.nameC11);
			g.NameList.SetName(2, 12, saveGameData.nameC12);
			g.NameList.SetName(2, 13, saveGameData.nameC13);
			g.NameList.SetName(2, 14, saveGameData.nameC14);
			g.NameList.SetName(2, 15, saveGameData.nameC15);
			g.NameList.SetName(2, 16, saveGameData.nameC16);
			g.NameList.SetName(2, 17, saveGameData.nameC17);
			g.NameList.SetName(2, 18, saveGameData.nameC18);
			g.NameList.SetName(2, 19, saveGameData.nameC19);
			g.NameList.SetName(2, 20, saveGameData.nameC20);
			g.NameList.SetName(2, 21, saveGameData.nameC21);
			g.NameList.SetName(2, 22, saveGameData.nameC22);
			g.NameList.SetName(2, 23, saveGameData.nameC23);
			g.NameList.SetName(2, 24, saveGameData.nameC24);
			g.NameList.SetName(2, 25, saveGameData.nameC25);
			g.NameList.SetName(2, 26, saveGameData.nameC26);
			g.NameList.SetName(2, 27, saveGameData.nameC27);
			g.NameList.SetName(2, 28, saveGameData.nameC28);
			g.NameList.SetName(2, 29, saveGameData.nameC29);
			g.NameList.SetName(2, 30, saveGameData.nameC30);
			g.NameList.SetName(2, 31, saveGameData.nameC31);
			g.NameList.SetName(2, 32, saveGameData.nameC32);
			g.NameList.SetName(2, 33, saveGameData.nameC33);
			g.NameList.SetName(2, 34, saveGameData.nameC34);
			g.NameList.SetName(2, 35, saveGameData.nameC35);
			g.NameList.SetName(3, 0, saveGameData.nameD00);
			g.NameList.SetName(3, 1, saveGameData.nameD01);
			g.NameList.SetName(3, 2, saveGameData.nameD02);
			g.NameList.SetName(3, 3, saveGameData.nameD03);
			g.NameList.SetName(3, 4, saveGameData.nameD04);
			g.NameList.SetName(3, 5, saveGameData.nameD05);
			g.NameList.SetName(3, 6, saveGameData.nameD06);
			g.NameList.SetName(3, 7, saveGameData.nameD07);
			g.NameList.SetName(3, 8, saveGameData.nameD08);
			g.NameList.SetName(3, 9, saveGameData.nameD09);
			g.NameList.SetName(3, 10, saveGameData.nameD10);
			g.NameList.SetName(3, 11, saveGameData.nameD11);
			g.NameList.SetName(3, 12, saveGameData.nameD12);
			g.NameList.SetName(3, 13, saveGameData.nameD13);
			g.NameList.SetName(3, 14, saveGameData.nameD14);
			g.NameList.SetName(3, 15, saveGameData.nameD15);
			g.NameList.SetName(3, 16, saveGameData.nameD16);
			g.NameList.SetName(3, 17, saveGameData.nameD17);
			g.NameList.SetName(3, 18, saveGameData.nameD18);
			g.NameList.SetName(3, 19, saveGameData.nameD19);
			g.NameList.SetName(3, 20, saveGameData.nameD20);
			g.NameList.SetName(3, 21, saveGameData.nameD21);
			g.NameList.SetName(3, 22, saveGameData.nameD22);
			g.NameList.SetName(3, 23, saveGameData.nameD23);
			g.NameList.SetName(3, 24, saveGameData.nameD24);
			g.NameList.SetName(3, 25, saveGameData.nameD25);
			g.NameList.SetName(3, 26, saveGameData.nameD26);
			g.NameList.SetName(3, 27, saveGameData.nameD27);
			g.NameList.SetName(3, 28, saveGameData.nameD28);
			g.NameList.SetName(3, 29, saveGameData.nameD29);
			g.NameList.SetName(3, 30, saveGameData.nameD30);
			g.NameList.SetName(3, 31, saveGameData.nameD31);
			g.NameList.SetName(3, 32, saveGameData.nameD32);
			g.NameList.SetName(3, 33, saveGameData.nameD33);
			g.NameList.SetName(3, 34, saveGameData.nameD34);
			g.NameList.SetName(3, 35, saveGameData.nameD35);
			g.NameList.SetName(4, 0, saveGameData.nameE00);
			g.NameList.SetName(4, 1, saveGameData.nameE01);
			g.NameList.SetName(4, 2, saveGameData.nameE02);
			g.NameList.SetName(4, 3, saveGameData.nameE03);
			g.NameList.SetName(4, 4, saveGameData.nameE04);
			g.NameList.SetName(4, 5, saveGameData.nameE05);
			g.NameList.SetName(4, 6, saveGameData.nameE06);
			g.NameList.SetName(4, 7, saveGameData.nameE07);
			g.NameList.SetName(4, 8, saveGameData.nameE08);
			g.NameList.SetName(4, 9, saveGameData.nameE09);
			g.NameList.SetName(4, 10, saveGameData.nameE10);
			g.NameList.SetName(4, 11, saveGameData.nameE11);
			g.NameList.SetName(4, 12, saveGameData.nameE12);
			g.NameList.SetName(4, 13, saveGameData.nameE13);
			g.NameList.SetName(4, 14, saveGameData.nameE14);
			g.NameList.SetName(4, 15, saveGameData.nameE15);
			g.NameList.SetName(4, 16, saveGameData.nameE16);
			g.NameList.SetName(4, 17, saveGameData.nameE17);
			g.NameList.SetName(4, 18, saveGameData.nameE18);
			g.NameList.SetName(4, 19, saveGameData.nameE19);
			g.NameList.SetName(4, 20, saveGameData.nameE20);
			g.NameList.SetName(4, 21, saveGameData.nameE21);
			g.NameList.SetName(4, 22, saveGameData.nameE22);
			g.NameList.SetName(4, 23, saveGameData.nameE23);
			g.NameList.SetName(4, 24, saveGameData.nameE24);
			g.NameList.SetName(4, 25, saveGameData.nameE25);
			g.NameList.SetName(4, 26, saveGameData.nameE26);
			g.NameList.SetName(4, 27, saveGameData.nameE27);
			g.NameList.SetName(4, 28, saveGameData.nameE28);
			g.NameList.SetName(4, 29, saveGameData.nameE29);
			g.NameList.SetName(4, 30, saveGameData.nameE30);
			g.NameList.SetName(4, 31, saveGameData.nameE31);
			g.NameList.SetName(4, 32, saveGameData.nameE32);
			g.NameList.SetName(4, 33, saveGameData.nameE33);
			g.NameList.SetName(4, 34, saveGameData.nameE34);
			g.NameList.SetName(4, 35, saveGameData.nameE35);
			g.NameList.SetName(5, 0, saveGameData.nameF00);
			g.NameList.SetName(5, 1, saveGameData.nameF01);
			g.NameList.SetName(5, 2, saveGameData.nameF02);
			g.NameList.SetName(5, 3, saveGameData.nameF03);
			g.NameList.SetName(5, 4, saveGameData.nameF04);
			g.NameList.SetName(5, 5, saveGameData.nameF05);
			g.NameList.SetName(5, 6, saveGameData.nameF06);
			g.NameList.SetName(5, 7, saveGameData.nameF07);
			g.NameList.SetName(5, 8, saveGameData.nameF08);
			g.NameList.SetName(5, 9, saveGameData.nameF09);
			g.NameList.SetName(5, 10, saveGameData.nameF10);
			g.NameList.SetName(5, 11, saveGameData.nameF11);
			g.NameList.SetName(5, 12, saveGameData.nameF12);
			g.NameList.SetName(5, 13, saveGameData.nameF13);
			g.NameList.SetName(5, 14, saveGameData.nameF14);
			g.NameList.SetName(5, 15, saveGameData.nameF15);
			g.NameList.SetName(5, 16, saveGameData.nameF16);
			g.NameList.SetName(5, 17, saveGameData.nameF17);
			g.NameList.SetName(5, 18, saveGameData.nameF18);
			g.NameList.SetName(5, 19, saveGameData.nameF19);
			g.NameList.SetName(5, 20, saveGameData.nameF20);
			g.NameList.SetName(5, 21, saveGameData.nameF21);
			g.NameList.SetName(5, 22, saveGameData.nameF22);
			g.NameList.SetName(5, 23, saveGameData.nameF23);
			g.NameList.SetName(5, 24, saveGameData.nameF24);
			g.NameList.SetName(5, 25, saveGameData.nameF25);
			g.NameList.SetName(5, 26, saveGameData.nameF26);
			g.NameList.SetName(5, 27, saveGameData.nameF27);
			g.NameList.SetName(5, 28, saveGameData.nameF28);
			g.NameList.SetName(5, 29, saveGameData.nameF29);
			g.NameList.SetName(5, 30, saveGameData.nameF30);
			g.NameList.SetName(5, 31, saveGameData.nameF31);
			g.NameList.SetName(5, 32, saveGameData.nameF32);
			g.NameList.SetName(5, 33, saveGameData.nameF33);
			g.NameList.SetName(5, 34, saveGameData.nameF34);
			g.NameList.SetName(5, 35, saveGameData.nameF35);
			g.NameList.SetName(6, 0, saveGameData.nameG00);
			g.NameList.SetName(6, 1, saveGameData.nameG01);
			g.NameList.SetName(6, 2, saveGameData.nameG02);
			g.NameList.SetName(6, 3, saveGameData.nameG03);
			g.NameList.SetName(6, 4, saveGameData.nameG04);
			g.NameList.SetName(6, 5, saveGameData.nameG05);
			g.NameList.SetName(6, 6, saveGameData.nameG06);
			g.NameList.SetName(6, 7, saveGameData.nameG07);
			g.NameList.SetName(6, 8, saveGameData.nameG08);
			g.NameList.SetName(6, 9, saveGameData.nameG09);
			g.NameList.SetName(6, 10, saveGameData.nameG10);
			g.NameList.SetName(6, 11, saveGameData.nameG11);
			g.NameList.SetName(6, 12, saveGameData.nameG12);
			g.NameList.SetName(6, 13, saveGameData.nameG13);
			g.NameList.SetName(6, 14, saveGameData.nameG14);
			g.NameList.SetName(6, 15, saveGameData.nameG15);
			g.NameList.SetName(6, 16, saveGameData.nameG16);
			g.NameList.SetName(6, 17, saveGameData.nameG17);
			g.NameList.SetName(6, 18, saveGameData.nameG18);
			g.NameList.SetName(6, 19, saveGameData.nameG19);
			g.NameList.SetName(6, 20, saveGameData.nameG20);
			g.NameList.SetName(6, 21, saveGameData.nameG21);
			g.NameList.SetName(6, 22, saveGameData.nameG22);
			g.NameList.SetName(6, 23, saveGameData.nameG23);
			g.NameList.SetName(6, 24, saveGameData.nameG24);
			g.NameList.SetName(6, 25, saveGameData.nameG25);
			g.NameList.SetName(6, 26, saveGameData.nameG26);
			g.NameList.SetName(6, 27, saveGameData.nameG27);
			g.NameList.SetName(6, 28, saveGameData.nameG28);
			g.NameList.SetName(6, 29, saveGameData.nameG29);
			g.NameList.SetName(6, 30, saveGameData.nameG30);
			g.NameList.SetName(6, 31, saveGameData.nameG31);
			g.NameList.SetName(6, 32, saveGameData.nameG32);
			g.NameList.SetName(6, 33, saveGameData.nameG33);
			g.NameList.SetName(6, 34, saveGameData.nameG34);
			g.NameList.SetName(6, 35, saveGameData.nameG35);
			g.NameList.SetName(7, 0, saveGameData.nameH00);
			g.NameList.SetName(7, 1, saveGameData.nameH01);
			g.NameList.SetName(7, 2, saveGameData.nameH02);
			g.NameList.SetName(7, 3, saveGameData.nameH03);
			g.NameList.SetName(7, 4, saveGameData.nameH04);
			g.NameList.SetName(7, 5, saveGameData.nameH05);
			g.NameList.SetName(7, 6, saveGameData.nameH06);
			g.NameList.SetName(7, 7, saveGameData.nameH07);
			g.NameList.SetName(7, 8, saveGameData.nameH08);
			g.NameList.SetName(7, 9, saveGameData.nameH09);
			g.NameList.SetName(7, 10, saveGameData.nameH10);
			g.NameList.SetName(7, 11, saveGameData.nameH11);
			g.NameList.SetName(7, 12, saveGameData.nameH12);
			g.NameList.SetName(7, 13, saveGameData.nameH13);
			g.NameList.SetName(7, 14, saveGameData.nameH14);
			g.NameList.SetName(7, 15, saveGameData.nameH15);
			g.NameList.SetName(7, 16, saveGameData.nameH16);
			g.NameList.SetName(7, 17, saveGameData.nameH17);
			g.NameList.SetName(7, 18, saveGameData.nameH18);
			g.NameList.SetName(7, 19, saveGameData.nameH19);
			g.NameList.SetName(7, 20, saveGameData.nameH20);
			g.NameList.SetName(7, 21, saveGameData.nameH21);
			g.NameList.SetName(7, 22, saveGameData.nameH22);
			g.NameList.SetName(7, 23, saveGameData.nameH23);
			g.NameList.SetName(7, 24, saveGameData.nameH24);
			g.NameList.SetName(7, 25, saveGameData.nameH25);
			g.NameList.SetName(7, 26, saveGameData.nameH26);
			g.NameList.SetName(7, 27, saveGameData.nameH27);
			g.NameList.SetName(7, 28, saveGameData.nameH28);
			g.NameList.SetName(7, 29, saveGameData.nameH29);
			g.NameList.SetName(7, 30, saveGameData.nameH30);
			g.NameList.SetName(7, 31, saveGameData.nameH31);
			g.NameList.SetName(7, 32, saveGameData.nameH32);
			g.NameList.SetName(7, 33, saveGameData.nameH33);
			g.NameList.SetName(7, 34, saveGameData.nameH34);
			g.NameList.SetName(7, 35, saveGameData.nameH35);
			g.theSlots[0].SetCurrentSquadron(g.squadrons[saveGameData.slot0Squadron]);
			g.theSlots[1].SetCurrentSquadron(g.squadrons[saveGameData.slot1Squadron]);
			g.theSlots[2].SetCurrentSquadron(g.squadrons[saveGameData.slot2Squadron]);
			g.theSlots[3].SetCurrentSquadron(g.squadrons[saveGameData.slot3Squadron]);
			g.theSoundManager.SetVolumeSFX((float)saveGameData.soundVolume / 10f);
			g.theSoundManager.SetVolumeMusic((float)saveGameData.musicVolume / 10f);
			g.theSafeArea.SetScreenMode(saveGameData.screenMode);
		}
		catch (Exception)
		{
		}
		SetSquadronNames();
		UpdateNames();
	}

	public void SetSquadronNames()
	{
		for (int i = 0; i < g.squadrons.Length; i++)
		{
			g.squadrons[i].SetMemberNames();
		}
	}

	public void ResetAllSaveVariables()
	{
		for (int i = 0; i < g.squadrons.Length; i++)
		{
			g.squadrons[i].ResetMemberNames();
		}
	}

	public bool GetIsSaving()
	{
		return isSaving;
	}

	public bool GetIsLoading()
	{
		return isLoading;
	}
}
