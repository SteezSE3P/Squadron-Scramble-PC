using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class OptionsOverlay
{
	private GameWorld g;

	private Vector2 OPTIONPOSITION = new Vector2(650f, 450f);

	private bool isAdjustingScreen = false;

	private bool optionBooleanIsYes = false;

	private float flashTimer = 0f;

	private float FLASHSPEED = 0.25f;

	private Texture2D elementsTexture;

	public void LoadContent(ContentManager theContentManager)
	{
		elementsTexture = TextureManager.GetElementsOptionsOverlayTexture();
	}

	public OptionsOverlay(GameWorld gw)
	{
		g = gw;
	}

	public void DrawFrame(SpriteBatch theSpriteBatch, Vector2 p, string s, int o)
	{
		Vector2 position = p;
		Rectangle value = new Rectangle(0, 115, 351, 136);
		theSpriteBatch.Draw(elementsTexture, position, value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1.5f, SpriteEffects.None, 0f);
		General.DrawEmbossedString(theSpriteBatch, g.theFontManager.GetHiScoreFont(), s, new Vector2(position.X, position.Y - (float)o), new Color(225, 225, 225), new Color(255, 255, 255), new Color(195, 195, 195), 0f, g.theFontManager.GetHiScoreFont().MeasureString(s) / 2f, 1f, 1, 1f);
	}

	public void DrawVolumeControl(SpriteBatch theSpriteBatch, Vector2 p, string s)
	{
		int num = 10;
		Vector2 p2 = p;
		Rectangle value = new Rectangle(115, 0, 21, 46);
		DrawFrame(theSpriteBatch, p2, s, 60);
		for (int i = 0; i < num; i++)
		{
			theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X - ((float)(num / 2) - 0.5f) * (float)value.Width * 2f + (float)(i * value.Width) * 2f, p2.Y + 15f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 2f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X - ((float)(num / 2) - 0.5f) * (float)value.Width * 2f + (float)(i * value.Width) * 2f, p2.Y + 15f), value, new Color(0f, 0f, 0f, 0.75f), 0f, new Vector2(value.Width / 2, value.Height / 2), 2f, SpriteEffects.None, 0f);
			if (g.theSoundManager.GetProvVolume() > (float)Math.Round((double)i / 10.0, 1))
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X - ((float)(num / 2) - 0.5f) * (float)value.Width * 2f + (float)(i * value.Width) * 2f, p2.Y + 15f), value, Color.White * General.GetOptionThrobValue(), 0f, new Vector2(value.Width / 2, value.Height / 2), 2f, SpriteEffects.None, 0f);
			}
		}
	}

	public void DrawMusicVolumeControl(SpriteBatch theSpriteBatch, Vector2 p, string s)
	{
		int num = 10;
		Vector2 p2 = p;
		Rectangle value = new Rectangle(115, 0, 21, 46);
		DrawFrame(theSpriteBatch, p2, s, 60);
		for (int i = 0; i < num; i++)
		{
			theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X - ((float)(num / 2) - 0.5f) * (float)value.Width + (float)(i * value.Width), p2.Y + 20f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X - ((float)(num / 2) - 0.5f) * (float)value.Width + (float)(i * value.Width), p2.Y + 20f), value, new Color(0f, 0f, 0f, 0.75f), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			if (g.theSoundManager.GetProvVolume() > (float)Math.Round((double)i / 10.0, 1))
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X - ((float)(num / 2) - 0.5f) * (float)value.Width + (float)(i * value.Width), p2.Y + 20f), value, Color.White * General.GetOptionThrobValue(), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public void DrawScreenModeControl(SpriteBatch theSpriteBatch, Vector2 p, string s)
	{
		Vector2 p2 = p;
		int num = 3;
		float num2 = 100f;
		Vector2 vector = new Vector2(0f, 0f);
		float num3 = 0f;
		float num4 = 0f;
		DrawFrame(theSpriteBatch, p2, s, 70);
		Rectangle value = new Rectangle(0, 263, 360, 180);
		theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X, p2.Y - 8f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 0.5f, SpriteEffects.None, 0f);
		value = new Rectangle(115, 0, 21, 46);
		for (int i = 0; i < num; i++)
		{
			theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X + num2 * ((float)num - 1f) / 2f - num2 * (float)i, p2.Y + 65f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X + num2 * ((float)num - 1f) / 2f - num2 * (float)i, p2.Y + 65f), value, new Color(0f, 0f, 0f, 0.75f), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			if (g.theSafeArea.GetProvScreenMode() == i)
			{
				theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X + num2 * ((float)num - 1f) / 2f - num2 * (float)i, p2.Y + 65f), value, Color.White * General.GetOptionThrobValue(), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			}
		}
		if (g.theSafeArea.GetProvScreenMode() == 0)
		{
			if (flashTimer < FLASHSPEED)
			{
				vector = new Vector2(0f, 0f);
				num3 = 1235f;
				num4 = 675f;
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 0, 45, 45), texture: elementsTexture, position: new Vector2(vector.X, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(56, 0, 45, 45), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 56, 45, 45), texture: elementsTexture, position: new Vector2(vector.X, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(56, 56, 45, 45), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			vector = new Vector2(560f, 457f);
			num3 = 158f;
			num4 = 66f;
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(149, 0, 23, 24), texture: elementsTexture, position: new Vector2(vector.X, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(178, 0, 23, 24), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(149, 28, 23, 24), texture: elementsTexture, position: new Vector2(vector.X, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(178, 28, 23, 24), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		else if (g.theSafeArea.GetProvScreenMode() == 1)
		{
			if (flashTimer < FLASHSPEED)
			{
				vector = new Vector2(33f, 33f);
				num3 = 1168f;
				num4 = 610f;
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 0, 45, 45), texture: elementsTexture, position: new Vector2(vector.X, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(56, 0, 45, 45), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 56, 45, 45), texture: elementsTexture, position: new Vector2(vector.X, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(56, 56, 45, 45), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			vector = new Vector2(564f, 462f);
			num3 = 150f;
			num4 = 56f;
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(149, 0, 23, 24), texture: elementsTexture, position: new Vector2(vector.X, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(178, 0, 23, 24), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(149, 28, 23, 24), texture: elementsTexture, position: new Vector2(vector.X, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(178, 28, 23, 24), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
		else
		{
			if (flashTimer < FLASHSPEED)
			{
				vector = new Vector2(65f, 66f);
				num3 = 1100f;
				num4 = 540f;
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 0, 45, 45), texture: elementsTexture, position: new Vector2(vector.X, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(56, 0, 45, 45), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(0, 56, 45, 45), texture: elementsTexture, position: new Vector2(vector.X, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
				theSpriteBatch.Draw(sourceRectangle: new Rectangle(56, 56, 45, 45), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			}
			vector = new Vector2(568f, 467f);
			num3 = 142f;
			num4 = 46f;
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(149, 0, 23, 24), texture: elementsTexture, position: new Vector2(vector.X, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(178, 0, 23, 24), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(149, 28, 23, 24), texture: elementsTexture, position: new Vector2(vector.X, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			theSpriteBatch.Draw(sourceRectangle: new Rectangle(178, 28, 23, 24), texture: elementsTexture, position: new Vector2(vector.X + num3, vector.Y + num4), color: Color.White, rotation: 0f, origin: new Vector2(0f, 0f), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
		}
	}

	public void DrawContinueOption(SpriteBatch theSpriteBatch, Vector2 p, string s)
	{
		Vector2 p2 = p;
		Vector2 vector = new Vector2(0f, 0f);
		DrawFrame(theSpriteBatch, p2, s, 30);
		Rectangle value = new Rectangle(352, 115, 142, 50);
		theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X, p2.Y + 50f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X - 1f, p2.Y + 52f - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X + 1f, p2.Y + 52f - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X - 1f, p2.Y + 52f + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X + 1f, p2.Y + 52f + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X, p2.Y + 52f), Color.Red * General.GetOptionThrobValue(), 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
	}

	public void DrawYesNoOption(SpriteBatch theSpriteBatch, Vector2 p, string s, string y, string n)
	{
		Vector2 p2 = p;
		float num = 190f;
		Vector2 vector = new Vector2(0f, 0f);
		DrawFrame(theSpriteBatch, p2, s, 30);
		Rectangle value = new Rectangle(352, 115, 142, 50);
		theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X - num / 2f, p2.Y + 50f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), y, new Vector2(p2.X - num / 2f - 1f, p2.Y + 52f - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(y) / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), y, new Vector2(p2.X - num / 2f + 1f, p2.Y + 52f - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(y) / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), y, new Vector2(p2.X - num / 2f - 1f, p2.Y + 52f + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(y) / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), y, new Vector2(p2.X - num / 2f + 1f, p2.Y + 52f + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(y) / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), y, new Vector2(p2.X - num / 2f, p2.Y + 52f), Color.Red * General.GetOptionThrobValue(), 0f, g.theFontManager.GetFont().MeasureString(y) / 2f, 1f, SpriteEffects.None, 0f);
		if (!optionBooleanIsYes)
		{
			theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X - num / 2f, p2.Y + 50f), value, new Color(0f, 0f, 0f, 0.75f), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), y, new Vector2(p2.X - num / 2f, p2.Y + 52f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(y) / 2f, 1f, SpriteEffects.None, 0f);
		}
		theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X + num / 2f, p2.Y + 50f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), n, new Vector2(p2.X + num / 2f - 1f, p2.Y + 52f - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(n) / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), n, new Vector2(p2.X + num / 2f + 1f, p2.Y + 52f - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(n) / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), n, new Vector2(p2.X + num / 2f - 1f, p2.Y + 52f + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(n) / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), n, new Vector2(p2.X + num / 2f + 1f, p2.Y + 52f + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(n) / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), n, new Vector2(p2.X + num / 2f, p2.Y + 52f), Color.Red * General.GetOptionThrobValue(), 0f, g.theFontManager.GetFont().MeasureString(n) / 2f, 1f, SpriteEffects.None, 0f);
		if (optionBooleanIsYes)
		{
			theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X + num / 2f, p2.Y + 50f), value, new Color(0f, 0f, 0f, 0.75f), 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
			theSpriteBatch.DrawString(g.theFontManager.GetFont(), n, new Vector2(p2.X + num / 2f, p2.Y + 52f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString(n) / 2f, 1f, SpriteEffects.None, 0f);
		}
	}

	public void DrawErrorSaving(SpriteBatch theSpriteBatch, Vector2 p, string s)
	{
		Vector2 p2 = p;
		string text = "Connection to the storage device has";
		string text2 = "been disrupted since the last save.";
		Vector2 vector = new Vector2(0f, 0f);
		DrawFrame(theSpriteBatch, p2, s, 55);
		General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), text, new Vector2(p2.X, p2.Y - 20f), new Color(255, 255, 255), 0f, g.theFontManager.GetFont().MeasureString(text) / 2f, 1f, 1, 1f);
		General.DrawOutlineString(theSpriteBatch, g.theFontManager.GetFont(), text2, new Vector2(p2.X, p2.Y + 5f), new Color(255, 255, 255), 0f, g.theFontManager.GetFont().MeasureString(text2) / 2f, 1f, 1, 1f);
		Rectangle value = new Rectangle(352, 115, 142, 50);
		theSpriteBatch.Draw(elementsTexture, new Vector2(p2.X, p2.Y + 50f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X - 1f, p2.Y + 52f - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X + 1f, p2.Y + 52f - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X - 1f, p2.Y + 52f + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X + 1f, p2.Y + 52f + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "CONTINUE", new Vector2(p2.X, p2.Y + 52f), Color.Red * General.GetOptionThrobValue(), 0f, g.theFontManager.GetFont().MeasureString("CONTINUE") / 2f, 1f, SpriteEffects.None, 0f);
	}

	public void Update(GameTime theGameTime)
	{
		flashTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (flashTimer > FLASHSPEED * 2f)
		{
			flashTimer -= FLASHSPEED * 2f;
		}
	}

	public bool GetIsAdjustingScreen()
	{
		return isAdjustingScreen;
	}

	public void SetIsAdjustingScreen(bool b)
	{
		isAdjustingScreen = b;
	}

	public bool GetOptionBooleanIsYes()
	{
		return optionBooleanIsYes;
	}

	public void SetOptionBooleanIsYes(bool b)
	{
		g.theSoundManager.MenuSwitchSound();
		optionBooleanIsYes = b;
	}
}
