using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class PauseManager
{
	private enum State
	{
		options
	}

	private enum SubOptionState
	{
		off,
		viewControls,
		vibrationControl,
		soundVolumeControl,
		musicVolumeControl,
		screenModeControl,
		checkQuit,
		controllerDisconnected
	}

	private GameWorld g;

	private bool gamePaused = false;

	private bool pauseMenu = false;

	private bool quitting = false;

	private bool controlsDisabled = false;

	private Player pausePlayer;

	private float timer = 0f;

	private Vector2 OPTIONPOSITION = new Vector2(625f, 300f);

	private int optionSwitch = 0;

	private string[] optionString;

	private float[] optionSize;

	private int MAXOPTIONLIMIT = 6;

	private int currentOptionLimit = 0;

	private float controllerFlashTimer = 0f;

	private Texture2D elementsTexture;

	private State currentState = State.options;

	private SubOptionState currentSubOptionState = SubOptionState.off;

	public PauseManager(GameWorld gw)
	{
		g = gw;
		optionString = new string[MAXOPTIONLIMIT];
		optionSize = new float[MAXOPTIONLIMIT];
		for (int i = 0; i < optionString.Length; i++)
		{
			optionString[i] = "";
		}
	}

	public void LoadContent(ContentManager theContentManager)
	{
		elementsTexture = TextureManager.GetElementsFrontEndTexture();
	}

	public void Update(GameTime theGameTime)
	{
		if (!gamePaused)
		{
			return;
		}
		if (pauseMenu)
		{
			controllerFlashTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			CheckControls(theGameTime);
			if (currentState == State.options)
			{
				Options(theGameTime);
			}
			if (quitting)
			{
				FadeControl(theGameTime);
			}
		}
		else if (!Guide.IsVisible)
		{
			ResumeGame();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		if (!gamePaused || quitting)
		{
			return;
		}
		if (pauseMenu)
		{
			Vector2 o = g.theFontManager.GetFontGameHeading1().MeasureString("PAUSED");
			Color ct = new Color(255, 255, 0);
			General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetFontGameHeading1(), "PAUSED", new Vector2(705f, 250f), new Color(ct.R - 25, ct.G - 25, ct.B - 25), ct, new Color(ct.R - 50, ct.G - 50, ct.B - 50), 0f, o, 1f, 1, 1f);
			if (currentSubOptionState == SubOptionState.off)
			{
				Rectangle value = new Rectangle(4, 470, 304, 30);
				theSpriteBatch.Draw(elementsTexture, new Vector2(OPTIONPOSITION.X, OPTIONPOSITION.Y + (float)(50 * optionSwitch) - 3f), value, new Color(200, 200, 255) * 0.75f, 0f, new Vector2(value.Width / 2, value.Height / 2), 1.5f, SpriteEffects.None, 0f);
				for (int i = 0; i < MAXOPTIONLIMIT; i++)
				{
					General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), optionString[i], new Vector2(OPTIONPOSITION.X, OPTIONPOSITION.Y + (float)(50 * i)), new Color(225, 225, 225), Color.White, new Color(195, 195, 195), 0f, g.theFontManager.GetHiScoreFont().MeasureString(optionString[i]) / 2f, optionSize[i], 1, 1f);
				}
				General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), optionString[optionSwitch], new Vector2(OPTIONPOSITION.X, OPTIONPOSITION.Y + (float)(50 * optionSwitch)), new Color((int)(225f * General.GetOptionThrobValue()), 0, 0), new Color((int)(255f * General.GetOptionThrobValue()), 0, 0), new Color((int)(195f * General.GetOptionThrobValue()), 0, 0), 0f, g.theFontManager.GetHiScoreFont().MeasureString(optionString[optionSwitch]) / 2f, optionSize[optionSwitch], 1, 1f);
			}
			if (currentSubOptionState == SubOptionState.viewControls)
			{
				g.theScreenControlDetails.SetActive(b: true);
			}
			if (currentSubOptionState == SubOptionState.vibrationControl)
			{
				g.theOptionsOverlay.DrawYesNoOption(theSpriteBatch, new Vector2(625f, 390f), "CONTROLLER VIBRATION", "ON", "OFF");
			}
			if (currentSubOptionState == SubOptionState.soundVolumeControl)
			{
				g.theOptionsOverlay.DrawVolumeControl(theSpriteBatch, new Vector2(625f, 390f), "ADJUST SOUND VOLUME");
			}
			if (currentSubOptionState == SubOptionState.musicVolumeControl)
			{
				g.theOptionsOverlay.DrawVolumeControl(theSpriteBatch, new Vector2(625f, 390f), "ADJUST MUSIC VOLUME");
			}
			if (currentSubOptionState == SubOptionState.screenModeControl)
			{
				g.theOptionsOverlay.DrawScreenModeControl(theSpriteBatch, new Vector2(625f, 390f), "SELECT SCREEN AREA");
			}
			if (currentSubOptionState == SubOptionState.checkQuit)
			{
				g.theOptionsOverlay.DrawYesNoOption(theSpriteBatch, new Vector2(625f, 390f), "RETURN TO MAIN MENU?", "YES", "NO");
			}
			if (currentSubOptionState == SubOptionState.controllerDisconnected)
			{
				g.theOptionsOverlay.DrawContinueOption(theSpriteBatch, new Vector2(625f, 390f), "PLEASE RECONNECT\n       CONTROLLER");
			}
			g.theScreenControlDetails.Draw(theSpriteBatch);
			if (currentSubOptionState == SubOptionState.viewControls)
			{
				ControlOverlay.DrawB(theSpriteBatch);
			}
			else
			{
				ControlOverlay.DrawAB(theSpriteBatch);
			}
		}
		DrawControllerIcon(theSpriteBatch);
	}

	private void DrawControllerIcon(SpriteBatch theSpriteBatch)
	{
		Vector2 position = ((currentSubOptionState != SubOptionState.viewControls) ? new Vector2(500f, 210f) : new Vector2(162f, 201f));
		if (pausePlayer != null && pauseMenu)
		{
			Rectangle value = new Rectangle(4 + 51 * pausePlayer.GetCurrentPlayerIndexNumber(), 569, 51, 51);
			Rectangle value2 = new Rectangle(208, 569, 51, 51);
			float num = (float)Math.Sin(controllerFlashTimer * 2f);
			if (num <= 0f)
			{
				controllerFlashTimer = 0f;
			}
			theSpriteBatch.Draw(elementsTexture, position, value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(elementsTexture, position, value2, Color.White * (Math.Abs(num) / 2f), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		}
	}

	public void PausePressed(GameTime theGameTime, Player p)
	{
		pauseMenu = true;
		if (gamePaused)
		{
			return;
		}
		pausePlayer = p;
		for (int i = 0; i < g.players.Length; i++)
		{
			if (g.players[i] != pausePlayer)
			{
				g.players[i].SetStartDepressed(b: true);
			}
		}
		g.theSoundManager.MenuSelectSound();
		gamePaused = true;
		pausePlayer.SetStartDepressed(b: true);
		g.theScreenFadeOverlay.SetAlphaValue(0.9f);
		g.theSoundManager.PauseSounds();
	}

	public void GuideVisible(GameTime theGameTime)
	{
		gamePaused = true;
		g.theSoundManager.PauseSounds();
	}

	public void CheckControls(GameTime theGameTime)
	{
		if (quitting)
		{
			return;
		}
		if (currentSubOptionState == SubOptionState.off && !controlsDisabled && pausePlayer != null)
		{
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftUpPressed())
			{
				OptionUp();
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftDownPressed())
			{
				OptionDown();
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBackPressed())
			{
				g.theSoundManager.MenuSelectSound();
				ResumeGame();
			}
		}
		if (currentSubOptionState == SubOptionState.viewControls && g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBPressed())
		{
			g.theSoundManager.MenuSelectSound();
			currentSubOptionState = SubOptionState.off;
			g.theScreenControlDetails.SetActive(b: false);
		}
		if (currentSubOptionState == SubOptionState.vibrationControl && !controlsDisabled && pausePlayer != null)
		{
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftRightPressed())
			{
				g.theOptionsOverlay.SetOptionBooleanIsYes(b: false);
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftLeftPressed())
			{
				g.theOptionsOverlay.SetOptionBooleanIsYes(b: true);
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonAPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonStartPressed())
			{
				if (g.theOptionsOverlay.GetOptionBooleanIsYes())
				{
					g.theSoundManager.MenuSelectSound();
					g.theControllerVibrationManager[pausePlayer.GetCurrentPlayerIndexNumber()].SetVibrationOn(b: true);
					currentState = State.options;
					currentSubOptionState = SubOptionState.off;
				}
				else
				{
					g.theSoundManager.MenuSelectSound();
					g.theControllerVibrationManager[pausePlayer.GetCurrentPlayerIndexNumber()].SetVibrationOn(b: false);
					currentState = State.options;
					currentSubOptionState = SubOptionState.off;
				}
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBPressed())
			{
				g.theSoundManager.MenuSelectSound();
				currentState = State.options;
				currentSubOptionState = SubOptionState.off;
			}
		}
		if (currentSubOptionState == SubOptionState.soundVolumeControl && !controlsDisabled && pausePlayer != null)
		{
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftRightPressed())
			{
				g.theSoundManager.IncrementProvSFXVolume();
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftLeftPressed())
			{
				g.theSoundManager.DecrementProvSFXVolume();
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonAPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonStartPressed())
			{
				g.theSoundManager.MenuSelectSound();
				g.theSoundManager.SetVolumeSFX(g.theSoundManager.GetProvVolume());
				g.theSoundManager.AdjustAllSoundVolumes();
				currentSubOptionState = SubOptionState.off;
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBackPressed())
			{
				g.theSoundManager.MenuSelectSound();
				currentSubOptionState = SubOptionState.off;
			}
		}
		if (currentSubOptionState == SubOptionState.musicVolumeControl && !controlsDisabled && pausePlayer != null)
		{
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftRightPressed())
			{
				g.theSoundManager.IncrementProvMusicVolume();
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftLeftPressed())
			{
				g.theSoundManager.DecrementProvMusicVolume();
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonAPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonStartPressed())
			{
				g.theSoundManager.MenuSelectSound();
				g.theSoundManager.SetVolumeMusic(g.theSoundManager.GetProvVolume());
				currentSubOptionState = SubOptionState.off;
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBackPressed())
			{
				g.theSoundManager.MenuSelectSound();
				currentSubOptionState = SubOptionState.off;
			}
		}
		if (currentSubOptionState == SubOptionState.checkQuit && !controlsDisabled && pausePlayer != null)
		{
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftRightPressed())
			{
				g.theOptionsOverlay.SetOptionBooleanIsYes(b: false);
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckLeftLeftPressed())
			{
				g.theOptionsOverlay.SetOptionBooleanIsYes(b: true);
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonAPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonStartPressed())
			{
				if (g.theOptionsOverlay.GetOptionBooleanIsYes())
				{
					g.theSoundManager.MenuSelectSound();
					controlsDisabled = true;
					quitting = true;
				}
				else
				{
					g.theSoundManager.MenuSelectSound();
					currentState = State.options;
					currentSubOptionState = SubOptionState.off;
				}
			}
			if (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonBPressed())
			{
				g.theSoundManager.MenuSelectSound();
				currentState = State.options;
				currentSubOptionState = SubOptionState.off;
			}
		}
		if (currentSubOptionState == SubOptionState.controllerDisconnected)
		{
			g.theScreenControlDetails.SetActive(b: false);
			if (!controlsDisabled && pausePlayer != null && (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonAPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonStartPressed()))
			{
				g.theSoundManager.MenuSelectSound();
				currentState = State.options;
				currentSubOptionState = SubOptionState.off;
			}
		}
	}

	public void ResumeGame()
	{
		gamePaused = false;
		pauseMenu = false;
		g.theSoundManager.UnPauseSounds();
		g.theScreenFadeOverlay.SetAlphaValue(0f);
	}

	public void ResetPause()
	{
		gamePaused = false;
		pauseMenu = false;
	}

	public void Options(GameTime theGameTime)
	{
		currentOptionLimit = 5;
		optionString[0] = "RESUME GAME";
		optionString[1] = "VIEW CONTROLS";
		optionString[2] = "VIBRATION ON/OFF";
		optionString[3] = "SOUND VOLUME";
		optionString[4] = "MUSIC VOLUME";
		optionString[5] = "RETURN TO MAIN MENU";
		optionSize[0] = 0.75f;
		optionSize[1] = 0.75f;
		optionSize[2] = 0.75f;
		optionSize[3] = 0.75f;
		optionSize[4] = 0.75f;
		optionSize[5] = 0.75f;
		float num = 1f;
		if (currentSubOptionState != 0)
		{
			return;
		}
		switch (optionSwitch)
		{
		case 0:
			optionSize[0] = num;
			if (IsValidOption())
			{
				ResumeGame();
			}
			break;
		case 1:
			optionSize[1] = num;
			if (IsValidOption())
			{
				currentSubOptionState = SubOptionState.viewControls;
				g.theScreenControlDetails.SetActive(b: true);
			}
			break;
		case 2:
			optionSize[2] = num;
			if (IsValidOption())
			{
				currentSubOptionState = SubOptionState.vibrationControl;
				g.theOptionsOverlay.SetOptionBooleanIsYes(g.theControllerVibrationManager[pausePlayer.GetCurrentPlayerIndexNumber()].GetVibrationOn());
			}
			break;
		case 3:
			optionSize[3] = num;
			if (IsValidOption())
			{
				g.theSoundManager.SetProvVolume(g.theSoundManager.GetVolumeSFX());
				currentSubOptionState = SubOptionState.soundVolumeControl;
			}
			break;
		case 4:
			optionSize[4] = num;
			if (IsValidOption())
			{
				g.theSoundManager.SetProvVolume(g.theSoundManager.GetVolumeMusic());
				currentSubOptionState = SubOptionState.musicVolumeControl;
			}
			break;
		case 5:
			optionSize[5] = num;
			if (IsValidOption())
			{
				currentSubOptionState = SubOptionState.checkQuit;
				g.theOptionsOverlay.SetOptionBooleanIsYes(b: false);
			}
			break;
		}
	}

	public void FadeControl(GameTime theGameTime)
	{
		if ((int)timer == 0)
		{
			g.theScreenFadeOverlay.FadeScreen(1f);
		}
		timer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if ((int)timer == 3)
		{
			timer = 0f;
			optionSwitch = 0;
			quitting = false;
			controlsDisabled = false;
			currentSubOptionState = SubOptionState.off;
			g.theOptionsOverlay.SetOptionBooleanIsYes(b: false);
			g.theRoundManager.FullReset();
			g.theRoundManager.SetRoundIsPaused(b: false);
			g.theSoundManager.StopAllSounds();
			pausePlayer = null;
			g.ResetAll();
			g.theScreenFadeOverlay.UnFadeScreen(1f);
			gamePaused = false;
			g.theRoundManager.SetRoundIsOver(b: true);
			g.theInGameScreenText.ResetInGameScreenText();
			g.theRoundManager.SetRoundIsPlayable(b: true);
			g.GoToFrontEnd();
		}
	}

	public bool IsValidOption()
	{
		if (pausePlayer != null && (g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonAPressed() || g.theControllerMenuManager[pausePlayer.GetCurrentPlayerIndexNumber()].CheckButtonStartPressed()))
		{
			g.theSoundManager.MenuSelectSound();
			return true;
		}
		return false;
	}

	public void OptionUp()
	{
		if (optionSwitch > 0)
		{
			g.theSoundManager.MenuSwitchSound();
			optionSwitch--;
		}
		else
		{
			g.theSoundManager.MenuSwitchSound();
			optionSwitch = currentOptionLimit;
		}
	}

	public void OptionDown()
	{
		if (optionSwitch < currentOptionLimit)
		{
			g.theSoundManager.MenuSwitchSound();
			optionSwitch++;
		}
		else
		{
			g.theSoundManager.MenuSwitchSound();
			optionSwitch = 0;
		}
	}

	public bool GetGamePaused()
	{
		return gamePaused;
	}

	public void ControllerDisconnectionDetected()
	{
		currentSubOptionState = SubOptionState.controllerDisconnected;
	}
}
