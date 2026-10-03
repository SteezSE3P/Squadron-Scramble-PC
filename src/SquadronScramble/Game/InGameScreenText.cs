using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class InGameScreenText
{
	private enum RoundState
	{
		neutral,
		showTutorialText,
		showFullGameText,
		initialiseRoundText,
		initialiseRoundOverText,
		initialiseSquadronScrambleText,
		startPause,
		start,
		growing,
		full,
		shrinking,
		roundComplete
	}

	private GameWorld g;

	private Clock theClock;

	private Color clockColor = new Color(50, 0, 0);

	private Color clockTextColor = Color.White;

	private bool clockSoundPlayed = false;

	private float tutorialScale = 0f;

	private bool displayTutorialHeadingText = false;

	private string tutorialHeadingText = "HOW TO PLAY";

	private bool displayTutorial1Text = false;

	private string simplifiedText = "GET YOUR PILOTS TO THEIR PLANES!";

	private string tutorial1aText = "EACH SQUADRON HAS ";

	private string tutorial1bText = " PILOTS";

	private string tutorial1cText = " PILOT";

	private bool displayTutorial2Text = false;

	private string tutorial2Text = "SHOOT PLANES TO EARN POINTS";

	private bool displayTutorial3Text = false;

	private string tutorial3Text = "IF YOU LOSE A PILOT YOU LOSE THEIR POINTS!";

	private float tutorialTextTimer = 0f;

	private float fullGameScale = 0f;

	private bool displayFullGameHeadingText = false;

	private bool displayFullGame1Text = false;

	private bool displayFullGame2Text = false;

	private bool displayFullGame3Text = false;

	private float fullGameTextTimer = 0f;

	private int fullGameTextCounter = 0;

	private string fullGameHeadingText = "";

	private string fullGame1Text = "";

	private string fullGame2Text = "";

	private string fullGame3Text = "";

	private bool roundTextOn = false;

	private Vector2 roundPos = new Vector2(-1000f, -1000f);

	private Vector2 roundShadowPos = new Vector2(-1000f, -1000f);

	private Vector2 roundOrigin = new Vector2(0f, 0f);

	private Color roundColor = Color.White;

	private float roundScale = 1f;

	private string roundText = "";

	private float roundTextTimer = 0f;

	private bool isSquadronScrambleText = false;

	private string[] squadronName = new string[4];

	private Color[] squadronColor = new Color[4];

	private string[] squadronScore = new string[4];

	private Vector2[] squadronScoreOrigin = new Vector2[4];

	private string[,] combatantName = new string[4, 4];

	private Color[,] combatantColor = new Color[4, 4];

	private string[,] combatantScore = new string[4, 4];

	private Vector2[,] combatantScoreOrigin = new Vector2[4, 4];

	private RoundState roundState = RoundState.neutral;

	private Vector2 subPosition = new Vector2(0f, 0f);

	private Vector2 subOrigin = new Vector2(0f, 0f);

	private Texture2D mSpriteTexture1;

	private Texture2D mSpriteTexture2;

	private Texture2D mSpriteTexture3;

	private bool change = false;

	public InGameScreenText(GameWorld gw)
	{
		g = gw;
		theClock = new Clock(g, 0.3f);
	}

	public void LoadContent()
	{
		mSpriteTexture1 = TextureManager.GetSubCarrierTexture();
		mSpriteTexture2 = TextureManager.GetSubSnowTexture();
		mSpriteTexture3 = TextureManager.GetSubDesertTexture();
	}

	public void Update(GameTime theGameTime)
	{
		theClock.Update(theGameTime);
		UpdateTutorialText(theGameTime);
		UpdateFullGameText(theGameTime);
		UpdateNamePlateText(theGameTime);
		if (change)
		{
			change = false;
		}
	}

	public void UpdateTutorialText(GameTime theGameTime)
	{
		float num = 4f;
		if (roundState != RoundState.showTutorialText)
		{
			return;
		}
		tutorialTextTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (tutorialTextTimer > 1f && tutorialTextTimer < 12f)
		{
			if (tutorialScale == 0f)
			{
				g.theSoundManager.MaximiseSound();
			}
			if (tutorialScale < 1f)
			{
				tutorialScale += num * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (tutorialScale > 1f)
			{
				tutorialScale = 1f;
			}
		}
		else
		{
			if (tutorialScale == 1f)
			{
				g.theSoundManager.MinimiseSound();
			}
			if (tutorialScale > 0f)
			{
				tutorialScale -= num * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (tutorialScale < 0f)
			{
				tutorialScale = 0f;
			}
		}
		if (tutorialTextTimer > 1f)
		{
			displayTutorialHeadingText = true;
		}
		if (tutorialTextTimer > 3f && tutorialTextTimer < 12f)
		{
			if (!displayTutorial1Text)
			{
				g.theSoundManager.ClockTickSound(0f);
			}
			displayTutorial1Text = true;
		}
		if (tutorialTextTimer > 6f && tutorialTextTimer < 12f)
		{
			if (!displayTutorial2Text)
			{
				g.theSoundManager.ClockTickSound(0.2f);
			}
			displayTutorial2Text = true;
		}
		if (tutorialTextTimer > 9f && tutorialTextTimer < 12f)
		{
			if (!displayTutorial3Text)
			{
				g.theSoundManager.ClockTickSound(0.6f);
			}
			displayTutorial3Text = true;
		}
		if (tutorialTextTimer > 12f)
		{
			displayTutorial1Text = false;
			displayTutorial2Text = false;
			displayTutorial3Text = false;
		}
		if (tutorialTextTimer > 13f)
		{
			roundState = RoundState.initialiseRoundText;
			displayTutorialHeadingText = false;
			tutorialTextTimer = 0f;
		}
	}

	public void UpdateFullGameText(GameTime theGameTime)
	{
		float num = 4f;
		if (roundState != RoundState.showFullGameText)
		{
			return;
		}
		if (fullGameTextCounter == 0)
		{
			fullGameHeadingText = "FULL GAME FEATURES";
			fullGame1Text = "1 TO 8 PLAYER MAYHEM ON ONE CONSOLE!";
			fullGame2Text = "1 OR 2 PLAYERS ON EACH JOYPAD";
			fullGame3Text = "UP TO 7 A.I. OPPONENTS";
		}
		if (fullGameTextCounter == 1)
		{
			fullGameHeadingText = "FULL GAME FEATURES";
			fullGame1Text = "5 ENVIRONMENTS EACH FEATURING SPECIAL BONUS TARGETS";
			fullGame2Text = "EDIT AND SAVE PILOT NAMES";
			fullGame3Text = "EDIT GAME OPTIONS IN 'CUSTOM GAME' MODE";
		}
		if (fullGameTextCounter == 2)
		{
			fullGameHeadingText = "CUSTOM MODE OPTIONS";
			fullGame1Text = "TURN ON/OFF FRIENDLY FIRE";
			fullGame2Text = "ALTER THE AMMO LIMIT";
			fullGame3Text = "LIMIT THE NUMBER OF PILOTS";
		}
		if (fullGameTextCounter == 3)
		{
			fullGameHeadingText = "CUSTOM MODE OPTIONS";
			fullGame1Text = "SET THE SORTIE TIME LIMIT";
			fullGame2Text = "CHOOSE HOW MANY CUPS TO WIN";
			fullGame3Text = "TOGGLE ENVIRONMENTS ON/OFF";
		}
		fullGameTextTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (fullGameTextTimer > 1f && fullGameTextTimer < 12f)
		{
			if (fullGameScale == 0f)
			{
				g.theSoundManager.MaximiseSound();
			}
			if (fullGameScale < 1f)
			{
				fullGameScale += num * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (fullGameScale > 1f)
			{
				fullGameScale = 1f;
			}
		}
		else
		{
			if (fullGameScale == 1f)
			{
				g.theSoundManager.MinimiseSound();
			}
			if (fullGameScale > 0f)
			{
				fullGameScale -= num * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (fullGameScale < 0f)
			{
				fullGameScale = 0f;
			}
		}
		if (fullGameTextTimer > 1f)
		{
			displayFullGameHeadingText = true;
		}
		if (fullGameTextTimer > 3f && fullGameTextTimer < 12f)
		{
			if (!displayFullGame1Text)
			{
				g.theSoundManager.ClockTickSound(0f);
			}
			displayFullGame1Text = true;
		}
		if (fullGameTextTimer > 6f && fullGameTextTimer < 12f)
		{
			if (!displayFullGame2Text)
			{
				g.theSoundManager.ClockTickSound(0.2f);
			}
			displayFullGame2Text = true;
		}
		if (fullGameTextTimer > 9f && fullGameTextTimer < 12f)
		{
			if (!displayFullGame3Text)
			{
				g.theSoundManager.ClockTickSound(0.6f);
			}
			displayFullGame3Text = true;
		}
		if (fullGameTextTimer > 12f)
		{
			displayFullGame1Text = false;
			displayFullGame2Text = false;
			displayFullGame3Text = false;
		}
		if (fullGameTextTimer > 13f)
		{
			roundState = RoundState.initialiseRoundText;
			displayFullGameHeadingText = false;
			fullGameTextCounter++;
			if (fullGameTextCounter > 3)
			{
				fullGameTextCounter = 0;
			}
			fullGameTextTimer = 0f;
		}
	}

	public void DrawTutorialText(SpriteBatch theSpriteBatch)
	{
		Vector2 vector = new Vector2(0f, 0f);
		Vector2 vector2 = new Vector2(0f, 0f);
		Vector2 vector3 = new Vector2(0f, 0f);
		Vector2 vector4 = new Vector2(0f, 0f);
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			vector = new Vector2(485f, 190f);
			vector2 = new Vector2(485f, 290f);
			vector3 = new Vector2(485f, 390f);
			vector4 = new Vector2(485f, 490f);
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			vector = new Vector2(490f, 180f);
			vector2 = new Vector2(490f, 280f);
			vector3 = new Vector2(490f, 380f);
			vector4 = new Vector2(490f, 480f);
		}
		else
		{
			vector = new Vector2(475f, 150f);
			vector2 = new Vector2(475f, 250f);
			vector3 = new Vector2(475f, 350f);
			vector4 = new Vector2(475f, 450f);
		}
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			subPosition = new Vector2(484f, 181f);
			subOrigin = new Vector2(418f, 126f);
		}
		if (g.theSafeArea.GetScreenMode() == 1)
		{
			subPosition = new Vector2(481f, 179f);
			subOrigin = new Vector2(418f, 118f);
		}
		else
		{
			subPosition = new Vector2(478f, 150f);
			subOrigin = new Vector2(418f, 88f);
		}
		if (fullGameTextTimer > 0f && fullGameTextTimer < 13f)
		{
			if (fullGameTextCounter == 0)
			{
				theSpriteBatch.Draw(mSpriteTexture1, subPosition, new Rectangle(0, 0, 836, 595), Color.White, 0f, subOrigin, fullGameScale, SpriteEffects.None, 0f);
			}
			if (fullGameTextCounter == 1)
			{
				theSpriteBatch.Draw(mSpriteTexture2, subPosition, new Rectangle(0, 0, 836, 595), Color.White, 0f, subOrigin, fullGameScale, SpriteEffects.None, 0f);
			}
			if (fullGameTextCounter >= 2)
			{
				theSpriteBatch.Draw(mSpriteTexture3, subPosition, new Rectangle(0, 0, 836, 595), Color.White, 0f, subOrigin, fullGameScale, SpriteEffects.None, 0f);
			}
		}
		if (displayTutorialHeadingText)
		{
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameHeading2(), tutorialHeadingText, vector, new Color(225, 0, 0), new Color(255, 0, 0), new Color(195, 0, 0), 0f, g.theFontManager.GetFontGameHeading2().MeasureString(tutorialHeadingText) / 2f, tutorialScale, 1, 1f);
		}
		if (displayTutorial1Text)
		{
			if (CustomOptions.GetSquadronSectionSize(0) > 1)
			{
				string text = simplifiedText;
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2 + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2 + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2 + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2 + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2, Color.White, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				string text = simplifiedText;
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2 + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2 + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2 + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2 + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), text, vector2, Color.White, 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
			}
		}
		if (displayTutorial2Text)
		{
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial2Text, vector3 + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(tutorial2Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial2Text, vector3 + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(tutorial2Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial2Text, vector3 + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(tutorial2Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial2Text, vector3 + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(tutorial2Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial2Text, vector3, Color.White, 0f, g.theFontManager.GetFont().MeasureString(tutorial2Text) / 2f, 1f, SpriteEffects.None, 0f);
		}
		if (displayTutorial3Text)
		{
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial3Text, vector4 + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(tutorial3Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial3Text, vector4 + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(tutorial3Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial3Text, vector4 + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(tutorial3Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial3Text, vector4 + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(tutorial3Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), tutorial3Text, vector4, Color.White, 0f, g.theFontManager.GetFont().MeasureString(tutorial3Text) / 2f, 1f, SpriteEffects.None, 0f);
		}
	}

	public void DrawFullGameText(SpriteBatch theSpriteBatch)
	{
		Vector2 vector = new Vector2(0f, 0f);
		Vector2 vector2 = new Vector2(0f, 0f);
		Vector2 vector3 = new Vector2(0f, 0f);
		Vector2 vector4 = new Vector2(0f, 0f);
		if (g.theSafeArea.GetScreenMode() == 0)
		{
			vector = new Vector2(485f, 190f);
			vector2 = new Vector2(485f, 290f);
			vector3 = new Vector2(485f, 390f);
			vector4 = new Vector2(485f, 490f);
		}
		else if (g.theSafeArea.GetScreenMode() == 1)
		{
			vector = new Vector2(490f, 180f);
			vector2 = new Vector2(490f, 280f);
			vector3 = new Vector2(490f, 380f);
			vector4 = new Vector2(490f, 480f);
		}
		else
		{
			vector = new Vector2(475f, 150f);
			vector2 = new Vector2(475f, 250f);
			vector3 = new Vector2(475f, 350f);
			vector4 = new Vector2(475f, 450f);
		}
		if (displayFullGameHeadingText)
		{
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameHeading2(), fullGameHeadingText, vector, new Color(225, 0, 0), new Color(255, 0, 0), new Color(195, 0, 0), 0f, g.theFontManager.GetFontGameHeading2().MeasureString(fullGameHeadingText) / 2f, fullGameScale, 1, 1f);
		}
		if (displayFullGame1Text)
		{
			if (CustomOptions.GetSquadronSectionSize(0) > 1)
			{
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2 + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2 + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2 + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2 + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2, Color.White, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2 + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2 + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2 + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2 + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame1Text, vector2, Color.White, 0f, g.theFontManager.GetFont().MeasureString(fullGame1Text) / 2f, 1f, SpriteEffects.None, 0f);
			}
		}
		if (displayFullGame2Text)
		{
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame2Text, vector3 + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame2Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame2Text, vector3 + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame2Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame2Text, vector3 + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame2Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame2Text, vector3 + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame2Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame2Text, vector3, Color.White, 0f, g.theFontManager.GetFont().MeasureString(fullGame2Text) / 2f, 1f, SpriteEffects.None, 0f);
		}
		if (displayFullGame3Text)
		{
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame3Text, vector4 + new Vector2(-1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame3Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame3Text, vector4 + new Vector2(1f, -1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame3Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame3Text, vector4 + new Vector2(-1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame3Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame3Text, vector4 + new Vector2(1f, 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(fullGame3Text) / 2f, 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), fullGame3Text, vector4, Color.White, 0f, g.theFontManager.GetFont().MeasureString(fullGame3Text) / 2f, 1f, SpriteEffects.None, 0f);
		}
	}

	public void ShowTutorial()
	{
		roundState = RoundState.showTutorialText;
		roundScale = 0f;
	}

	public void ShowFullGameText()
	{
		roundState = RoundState.showFullGameText;
		roundScale = 0f;
	}

	public void UpdateRoundText(GameTime theGameTime)
	{
		float num = 4f;
		float num2 = 0f;
		float num3 = 1f;
		float num4 = 1f;
		float num5 = 1.5f;
		float num6 = 3.5f;
		float num7 = 2f;
		roundPos = new Vector2(470f, 350f);
		roundShadowPos = new Vector2(474f, 354f);
		if (!roundTextOn)
		{
			return;
		}
		if (roundState == RoundState.initialiseRoundText)
		{
			roundTextTimer = 0f;
			roundScale = num2;
			roundText = "SORTIE " + g.theRoundManager.GetRoundNumber();
			roundColor = Color.White;
			roundState = RoundState.startPause;
			roundOrigin = g.theFontManager.GetFontGameTitle().MeasureString(roundText) / 2f;
		}
		if (roundState == RoundState.initialiseSquadronScrambleText)
		{
			roundTextTimer = 0f;
			roundScale = num3;
			roundText = "SQUADRON\n SCRAMBLE!";
			roundColor = Color.Red;
			roundState = RoundState.full;
			roundOrigin = g.theFontManager.GetFontGameTitle().MeasureString(roundText) / 2f;
			g.theRoundManager.SetRoundIsPlayable(b: true);
		}
		if (roundState == RoundState.initialiseRoundOverText)
		{
			g.theSoundManager.StopAllSounds();
			g.theSoundManager.RoundEndSound();
			roundTextTimer = 0f;
			roundScale = num2;
			roundText = "SORTIE OVER";
			roundColor = Color.White;
			roundState = RoundState.growing;
			roundOrigin = g.theFontManager.GetFontGameTitle().MeasureString(roundText) / 2f;
			g.theRoundManager.SetRoundIsPlayable(b: true);
		}
		if (roundState == RoundState.startPause)
		{
			roundTextTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (roundTextTimer >= num4)
			{
				roundTextTimer = 0f;
				roundState = RoundState.growing;
				g.theSoundManager.MaximiseSound();
			}
		}
		if (roundState == RoundState.growing)
		{
			roundScale += num * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (roundScale >= num3)
			{
				roundScale = num3;
				roundState = RoundState.full;
			}
		}
		if (roundState == RoundState.full)
		{
			roundTextTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (roundTextTimer >= num5)
			{
				roundState = RoundState.shrinking;
				if (!isSquadronScrambleText)
				{
					isSquadronScrambleText = true;
					roundState = RoundState.initialiseSquadronScrambleText;
					g.theSoundManager.StartKlaxonSound();
				}
				else if (!g.theRoundManager.GetRoundIsOver())
				{
					g.theSoundManager.StopKlaxonSound();
					g.theSoundManager.MinimiseSound();
				}
				if (g.theRoundManager.GetRoundIsOver())
				{
					roundState = RoundState.full;
					if (roundTextTimer >= num6)
					{
						g.theScreenFadeOverlay.FadeScreen(1f);
						ResetInGameScreenText();
					}
				}
			}
		}
		if (roundState == RoundState.shrinking)
		{
			roundScale -= num * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (roundScale <= num2)
			{
				roundScale = num2;
				if (isSquadronScrambleText)
				{
					roundState = RoundState.initialiseRoundText;
					roundTextOn = false;
				}
			}
		}
		if (roundState == RoundState.roundComplete)
		{
			roundTextTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (roundTextTimer >= num7)
			{
				g.GoToRoundResultScreen();
				roundTextTimer = 0f;
				roundScale = num2;
			}
		}
	}

	public void UpdateNamePlateText(GameTime theGameTime)
	{
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null && g.activeSlots[i].GetParticipating())
			{
				Squadron currentSquadron = g.activeSlots[i].GetCurrentSquadron();
				Color color = new Color(30, 30, 30);
				if (change)
				{
					squadronName[i] = g.activeSlots[i].GetSlotName();
					squadronScore[i] = currentSquadron.GetScore().ToString();
					if (squadronScore[i] == "-1")
					{
						squadronScore[i] = "OUT";
					}
				}
				ref Vector2 reference = ref squadronScoreOrigin[i];
				reference = new Vector2(g.theFontManager.GetFont().MeasureString(squadronScore[i]).X, 0f);
				ref Color reference2 = ref squadronColor[i];
				reference2 = currentSquadron.GetTheColor();
				for (int j = 0; j < g.activeSlots[i].GetCurrentSquadron().GetSquadronSectionSize(); j++)
				{
					if (change)
					{
						combatantName[i, j] = currentSquadron.GetCombatant(j).GetName();
						combatantScore[i, j] = currentSquadron.GetCombatant(j).GetScore().ToString();
						ref Vector2 reference3 = ref combatantScoreOrigin[i, j];
						reference3 = new Vector2(g.theFontManager.GetFont().MeasureString(combatantScore[i, j]).X, 0f);
					}
					if (currentSquadron.GetCombatant(j).CheckColor(0))
					{
						ref Color reference4 = ref combatantColor[i, j];
						reference4 = Color.White;
					}
					if (currentSquadron.GetCombatant(j).CheckColor(1))
					{
						ref Color reference5 = ref combatantColor[i, j];
						reference5 = Color.White;
					}
					if (currentSquadron.GetCombatant(j).CheckColor(2))
					{
						combatantColor[i, j] = color;
					}
					if (currentSquadron.GetCombatant(j).CheckColor(3))
					{
						ref Color reference6 = ref combatantColor[i, j];
						reference6 = Color.Red;
					}
				}
			}
			else if (change)
			{
				squadronName[i] = "";
				squadronScore[i] = "";
				ref Color reference7 = ref squadronColor[i];
				reference7 = Color.White;
				for (int j = 0; j < 4; j++)
				{
					combatantName[i, j] = "";
					combatantScore[i, j] = "";
					ref Color reference8 = ref combatantColor[i, j];
					reference8 = Color.White;
				}
			}
		}
	}

	public void DrawText(SpriteBatch theSpriteBatch)
	{
		Vector2 position = g.theNamePlate.GetPosition();
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), squadronName[i], new Vector2(position.X + 158f, position.Y + 113f + (float)(149 * i)), squadronColor[i], 0f, g.theFontManager.GetFont().MeasureString(squadronName[i]) / 2f, 1f, 1, 1f);
			General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), squadronScore[i], new Vector2(position.X + 300f, position.Y + 99f + (float)(149 * i)), squadronColor[i], 0f, squadronScoreOrigin[i], 1f, 1, 1f);
			for (int j = 0; j < 4; j++)
			{
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantName[i, j], new Vector2(position.X + 50f - 1f, position.Y + 126f - 1f + (float)(149 * i) + (float)(27 * j)), Color.Black);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantName[i, j], new Vector2(position.X + 50f + 1f, position.Y + 126f - 1f + (float)(149 * i) + (float)(27 * j)), Color.Black);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantName[i, j], new Vector2(position.X + 50f - 1f, position.Y + 126f + 1f + (float)(149 * i) + (float)(27 * j)), Color.Black);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantName[i, j], new Vector2(position.X + 50f + 1f, position.Y + 126f + 1f + (float)(149 * i) + (float)(27 * j)), Color.Black);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantName[i, j], new Vector2(position.X + 50f, position.Y + 126f + (float)(149 * i) + (float)(27 * j)), combatantColor[i, j]);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantScore[i, j], new Vector2(position.X + 300f - 1f, position.Y + 126f - 1f + (float)(149 * i) + (float)(27 * j)), Color.Black, 0f, combatantScoreOrigin[i, j], 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantScore[i, j], new Vector2(position.X + 300f + 1f, position.Y + 126f - 1f + (float)(149 * i) + (float)(27 * j)), Color.Black, 0f, combatantScoreOrigin[i, j], 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantScore[i, j], new Vector2(position.X + 300f - 1f, position.Y + 126f + 1f + (float)(149 * i) + (float)(27 * j)), Color.Black, 0f, combatantScoreOrigin[i, j], 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantScore[i, j], new Vector2(position.X + 300f + 1f, position.Y + 126f + 1f + (float)(149 * i) + (float)(27 * j)), Color.Black, 0f, combatantScoreOrigin[i, j], 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), combatantScore[i, j], new Vector2(position.X + 300f, position.Y + 126f + (float)(149 * i) + (float)(27 * j)), combatantColor[i, j], 0f, combatantScoreOrigin[i, j], 1f, SpriteEffects.None, 0f);
			}
			if (theClock.GetIsTimeShown() && theClock.GetTime() > -60f)
			{
				SpriteFont roundFont = g.theFontManager.GetRoundFont();
				SpriteFont hiScoreFont = g.theFontManager.GetHiScoreFont();
				if (g.theSafeArea.GetScreenMode() == 0)
				{
					if (theClock.GetSeconds() >= 0)
					{
						General.DrawEmbossedString(theSpriteBatch, roundFont, theClock.GetMinuteString(), new Vector2(g.theNamePlate.GetPosition().X + 74f, 8f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, roundFont, ":", new Vector2(g.theNamePlate.GetPosition().X + 124f, 1f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						if (theClock.GetSeconds() < 10)
						{
							General.DrawEmbossedString(theSpriteBatch, roundFont, "0", new Vector2(g.theNamePlate.GetPosition().X + 149f, 8f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
							General.DrawEmbossedString(theSpriteBatch, roundFont, theClock.GetSecondString(), new Vector2(g.theNamePlate.GetPosition().X + 197f, 8f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						}
						else
						{
							General.DrawEmbossedString(theSpriteBatch, roundFont, theClock.GetSecondString(), new Vector2(g.theNamePlate.GetPosition().X + 149f, 8f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						}
					}
					else if (theClock.GetIsTimeShown())
					{
						General.DrawEmbossedString(theSpriteBatch, roundFont, "0", new Vector2(g.theNamePlate.GetPosition().X + 74f, 8f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, roundFont, ":", new Vector2(g.theNamePlate.GetPosition().X + 124f, 1f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, roundFont, "0", new Vector2(g.theNamePlate.GetPosition().X + 149f, 8f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, roundFont, "0", new Vector2(g.theNamePlate.GetPosition().X + 197f, 8f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					}
				}
				else if (g.theSafeArea.GetScreenMode() == 1)
				{
					if (theClock.GetSeconds() >= 0)
					{
						General.DrawEmbossedString(theSpriteBatch, roundFont, theClock.GetMinuteString(), new Vector2(g.theNamePlate.GetPosition().X + 74f, 21f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, roundFont, ":", new Vector2(g.theNamePlate.GetPosition().X + 124f, 14f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						if (theClock.GetSeconds() < 10)
						{
							General.DrawEmbossedString(theSpriteBatch, roundFont, "0", new Vector2(g.theNamePlate.GetPosition().X + 149f, 21f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
							General.DrawEmbossedString(theSpriteBatch, roundFont, theClock.GetSecondString(), new Vector2(g.theNamePlate.GetPosition().X + 197f, 21f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						}
						else
						{
							General.DrawEmbossedString(theSpriteBatch, roundFont, theClock.GetSecondString(), new Vector2(g.theNamePlate.GetPosition().X + 149f, 21f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						}
					}
					else if (theClock.GetIsTimeShown())
					{
						General.DrawEmbossedString(theSpriteBatch, roundFont, "0", new Vector2(g.theNamePlate.GetPosition().X + 74f, 21f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, roundFont, ":", new Vector2(g.theNamePlate.GetPosition().X + 124f, 14f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, roundFont, "0", new Vector2(g.theNamePlate.GetPosition().X + 149f, 21f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, roundFont, "0", new Vector2(g.theNamePlate.GetPosition().X + 197f, 21f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					}
				}
				else if (theClock.GetSeconds() >= 0)
				{
					General.DrawEmbossedString(theSpriteBatch, hiScoreFont, theClock.GetMinuteString(), new Vector2(68f, 65f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					General.DrawEmbossedString(theSpriteBatch, hiScoreFont, ":", new Vector2(95f, 60f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					if (theClock.GetSeconds() < 10)
					{
						General.DrawEmbossedString(theSpriteBatch, hiScoreFont, "0", new Vector2(109f, 65f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
						General.DrawEmbossedString(theSpriteBatch, hiScoreFont, theClock.GetSecondString(), new Vector2(132f, 65f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					}
					else
					{
						General.DrawEmbossedString(theSpriteBatch, hiScoreFont, theClock.GetSecondString(), new Vector2(109f, 65f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					}
				}
				else if (theClock.GetIsTimeShown())
				{
					General.DrawEmbossedString(theSpriteBatch, hiScoreFont, "0", new Vector2(68f, 65f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					General.DrawEmbossedString(theSpriteBatch, hiScoreFont, ":", new Vector2(95f, 60f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					General.DrawEmbossedString(theSpriteBatch, hiScoreFont, "0", new Vector2(109f, 65f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
					General.DrawEmbossedString(theSpriteBatch, hiScoreFont, "0", new Vector2(132f, 65f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
				}
				if ((int)theClock.GetTime() <= 10)
				{
					if (theClock.GetTime() - (float)(int)theClock.GetTime() > 0.5f)
					{
						if (!clockSoundPlayed)
						{
							g.theSoundManager.ClockTickSound(0f);
						}
						clockColor = new Color(255, 0, 0);
						clockTextColor = new Color(255, 255, 255);
						clockSoundPlayed = true;
						if (g.theSafeArea.GetScreenMode() == 2)
						{
							clockTextColor = new Color(255, 0, 0);
						}
					}
					else
					{
						clockColor = new Color(50, 0, 0);
						clockTextColor = new Color(255, 255, 255);
						clockSoundPlayed = false;
						if (g.theSafeArea.GetScreenMode() == 2)
						{
							clockTextColor = new Color(255, 255, 255);
						}
					}
				}
				else
				{
					clockColor = new Color(50, 0, 0);
					clockTextColor = new Color(255, 255, 255);
				}
			}
			else if (theClock.GetTime() >= -1f)
			{
				SpriteFont roundFont = g.theFontManager.GetSuddenDeathFont();
				SpriteFont hiScoreFont = g.theFontManager.GetFont();
				if (g.theSafeArea.GetScreenMode() == 0)
				{
					General.DrawEmbossedString(theSpriteBatch, roundFont, "SUDDEN DEATH", new Vector2(g.theNamePlate.GetPosition().X + 5f, 26f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
				}
				else if (g.theSafeArea.GetScreenMode() == 1)
				{
					General.DrawEmbossedString(theSpriteBatch, roundFont, "SUDDEN DEATH", new Vector2(g.theNamePlate.GetPosition().X + 5f, 41f), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, new Vector2(0f, 0f), 1f, 1, 1f);
				}
				else
				{
					theSpriteBatch.DrawString(hiScoreFont, "SUDDEN DEATH", new Vector2(68f, 67f), new Color(0f, 0f, 0f, 0.1f));
					theSpriteBatch.DrawString(hiScoreFont, "SUDDEN DEATH", new Vector2(70f, 67f), new Color(0f, 0f, 0f, 0.1f));
					theSpriteBatch.DrawString(hiScoreFont, "SUDDEN DEATH", new Vector2(68f, 69f), new Color(0f, 0f, 0f, 0.1f));
					theSpriteBatch.DrawString(hiScoreFont, "SUDDEN DEATH", new Vector2(70f, 69f), new Color(0f, 0f, 0f, 0.1f));
					theSpriteBatch.DrawString(hiScoreFont, "SUDDEN DEATH", new Vector2(69f, 68f), clockTextColor);
				}
				clockColor = new Color(255, 0, 0);
				clockTextColor = new Color(255, 255, 255);
				if (g.theSafeArea.GetScreenMode() == 2)
				{
					clockTextColor = new Color(255, 0, 0);
				}
			}
			else
			{
				clockColor = new Color(50, 0, 0);
			}
			if (CustomOptions.GetDebugOn())
			{
				theSpriteBatch.DrawString(g.theFontManager.GetFont(), CustomOptions.GetGameSpeed().ToString(), new Vector2(400f, 100f), Color.White);
			}
		}
	}

	public void DrawScreenScores(SpriteBatch theSpriteBatch)
	{
		if (!g.theRoundManager.GetRoundIsOver())
		{
			theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.theSpecialBonus.GetTheScreenScoreBonus().GetScoreString(), new Vector2(g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition().X - 1f, g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition().Y - 1f), Color.Black, 0f, g.theSpecialBonus.GetTheScreenScoreBonus().GetOrigin(), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.theSpecialBonus.GetTheScreenScoreBonus().GetScoreString(), new Vector2(g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition().X + 1f, g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition().Y - 1f), Color.Black, 0f, g.theSpecialBonus.GetTheScreenScoreBonus().GetOrigin(), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.theSpecialBonus.GetTheScreenScoreBonus().GetScoreString(), new Vector2(g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition().X - 1f, g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition().Y + 1f), Color.Black, 0f, g.theSpecialBonus.GetTheScreenScoreBonus().GetOrigin(), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.theSpecialBonus.GetTheScreenScoreBonus().GetScoreString(), new Vector2(g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition().X + 1f, g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition().Y + 1f), Color.Black, 0f, g.theSpecialBonus.GetTheScreenScoreBonus().GetOrigin(), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.theSpecialBonus.GetTheScreenScoreBonus().GetScoreString(), g.theSpecialBonus.GetTheScreenScoreBonus().GetPosition(), g.theSpecialBonus.GetTheScreenScoreBonus().GetTheColor(), 0f, g.theSpecialBonus.GetTheScreenScoreBonus().GetOrigin(), 1f, SpriteEffects.None, 0f);
			for (int i = 0; i < g.pilots.Length; i++)
			{
				theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.pilots[i].GetTheScreenScore().GetScoreString(), new Vector2(g.pilots[i].GetTheScreenScore().GetScorePosition().X - 1f, g.pilots[i].GetTheScreenScore().GetScorePosition().Y - 1f), Color.Black, 0f, g.pilots[i].GetTheScreenScore().GetScoreOrigin(), 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.pilots[i].GetTheScreenScore().GetScoreString(), new Vector2(g.pilots[i].GetTheScreenScore().GetScorePosition().X + 1f, g.pilots[i].GetTheScreenScore().GetScorePosition().Y - 1f), Color.Black, 0f, g.pilots[i].GetTheScreenScore().GetScoreOrigin(), 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.pilots[i].GetTheScreenScore().GetScoreString(), new Vector2(g.pilots[i].GetTheScreenScore().GetScorePosition().X - 1f, g.pilots[i].GetTheScreenScore().GetScorePosition().Y + 1f), Color.Black, 0f, g.pilots[i].GetTheScreenScore().GetScoreOrigin(), 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.pilots[i].GetTheScreenScore().GetScoreString(), new Vector2(g.pilots[i].GetTheScreenScore().GetScorePosition().X + 1f, g.pilots[i].GetTheScreenScore().GetScorePosition().Y + 1f), Color.Black, 0f, g.pilots[i].GetTheScreenScore().GetScoreOrigin(), 1f, SpriteEffects.None, 0f);
				theSpriteBatch.DrawString(g.theFontManager.GetScreenScoreFont(), g.pilots[i].GetTheScreenScore().GetScoreString(), g.pilots[i].GetTheScreenScore().GetScorePosition(), g.pilots[i].GetTheScreenScore().GetTheScoreColor(), 0f, g.pilots[i].GetTheScreenScore().GetScoreOrigin(), 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public void DrawScreenNames(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (!g.theRoundManager.GetRoundIsOver())
			{
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), g.pilots[i].GetTheScreenScore().GetNameString(), g.pilots[i].GetTheScreenScore().GetNamePosition(), g.pilots[i].GetTheScreenScore().GetTheNameColor(), 0f, g.pilots[i].GetTheScreenScore().GetNameOrigin(), 1f, 1, 1f);
				General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), g.pilots[i].GetTheScreenScore().GetNameScoreString(), g.pilots[i].GetTheScreenScore().GetNameScorePosition(), g.pilots[i].GetTheScreenScore().GetTheNameColor(), 0f, g.pilots[i].GetTheScreenScore().GetNameScoreOrigin(), 1f, 1, 1f);
			}
		}
	}

	public void DrawRound(SpriteBatch theSpriteBatch)
	{
		if (roundTextOn)
		{
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameTitle(), roundText, roundPos, new Color(roundColor.R - 25, roundColor.G - 25, roundColor.B - 25), roundColor, new Color(roundColor.R - 50, roundColor.G - 50, roundColor.B - 50), 0f, roundOrigin, roundScale, 1, 1f);
		}
	}

	public void ClearNamesAndScores()
	{
		for (int i = 0; i < g.activeSlots.Length; i++)
		{
			if (g.activeSlots[i] != null)
			{
				for (int j = 0; j < 4; j++)
				{
					combatantName[i, j] = "";
					combatantScore[i, j] = "";
					ref Color reference = ref combatantColor[i, j];
					reference = Color.White;
				}
			}
		}
	}

	public void ResetInGameScreenText()
	{
		roundTextTimer = 0f;
		isSquadronScrambleText = false;
		roundState = RoundState.roundComplete;
	}

	public void SetRoundTextOn(bool b)
	{
		roundTextOn = b;
	}

	public void InitialiseRoundText()
	{
		roundState = RoundState.initialiseRoundText;
	}

	public void InitialiseRoundOverText()
	{
		roundState = RoundState.initialiseRoundOverText;
	}

	public bool CheckIfSuddenDeath()
	{
		if (theClock.GetTime() == -1f)
		{
			return true;
		}
		return false;
	}

	public void ResetClock()
	{
		if (!g.theRoundManager.GetIsFinal())
		{
			theClock = new Clock(g, CustomOptions.GetTimeLimit());
		}
		else
		{
			theClock = new Clock(g, -1f);
		}
		clockSoundPlayed = false;
	}

	public Color GetClockColor()
	{
		return clockColor;
	}

	public void Change()
	{
		change = true;
	}
}
