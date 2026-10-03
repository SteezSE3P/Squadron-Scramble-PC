using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public static class ControlOverlay
{
	private static GameWorld g;

	private static Vector2 refPosition = new Vector2(0f, 640f);

	private static Texture2D elementsTexture;

	public static void LoadContent(ContentManager theContentManager)
	{
		elementsTexture = TextureManager.GetElementsControlOverlayTexture();
	}

	public static void DrawAB(SpriteBatch theSpriteBatch)
	{
		DrawASelect(theSpriteBatch);
		DrawBBack(theSpriteBatch);
	}

	public static void DrawB(SpriteBatch theSpriteBatch)
	{
		DrawBBack(theSpriteBatch);
	}

	public static void DrawAssignSquadronYAB(SpriteBatch theSpriteBatch)
	{
		DrawYAssignSquadron(theSpriteBatch);
		DrawASelect(theSpriteBatch);
		DrawBBack(theSpriteBatch);
	}

	public static void DrawResetGroupNamesYAB(SpriteBatch theSpriteBatch)
	{
		DrawYResetGroupNames(theSpriteBatch);
		DrawASelect(theSpriteBatch);
		DrawBBack(theSpriteBatch);
	}

	public static void SetGameWorld(GameWorld gw)
	{
		g = gw;
	}

	public static void DrawLoading(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Loading...", new Vector2(refPosition.X + 100f - 1f, refPosition.Y - 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Loading...").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Loading...", new Vector2(refPosition.X + 100f + 1f, refPosition.Y - 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Loading...").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Loading...", new Vector2(refPosition.X + 100f - 1f, refPosition.Y + 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Loading...").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Loading...", new Vector2(refPosition.X + 100f + 1f, refPosition.Y + 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Loading...").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Loading...", new Vector2(refPosition.X + 100f, refPosition.Y), Color.White, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Loading...").Y / 2f), 1f, SpriteEffects.None, 0f);
	}

	public static void DrawSaving(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving...", new Vector2(refPosition.X + 100f - 1f, refPosition.Y - 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving...").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving...", new Vector2(refPosition.X + 100f + 1f, refPosition.Y - 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving...").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving...", new Vector2(refPosition.X + 100f - 1f, refPosition.Y + 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving...").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving...", new Vector2(refPosition.X + 100f + 1f, refPosition.Y + 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving...").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving...", new Vector2(refPosition.X + 100f, refPosition.Y), Color.White, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving...").Y / 2f), 1f, SpriteEffects.None, 0f);
	}

	public static void DrawSavingDisabledTrial(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled in trial version", new Vector2(refPosition.X + 100f - 1f, refPosition.Y - 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled in trial version").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled in trial version", new Vector2(refPosition.X + 100f + 1f, refPosition.Y - 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled in trial version").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled in trial version", new Vector2(refPosition.X + 100f - 1f, refPosition.Y + 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled in trial version").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled in trial version", new Vector2(refPosition.X + 100f + 1f, refPosition.Y + 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled in trial version").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled in trial version", new Vector2(refPosition.X + 100f, refPosition.Y), Color.White, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled in trial version").Y / 2f), 1f, SpriteEffects.None, 0f);
	}

	public static void DrawSavingDisabled(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled", new Vector2(refPosition.X + 100f - 1f, refPosition.Y - 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled", new Vector2(refPosition.X + 100f + 1f, refPosition.Y - 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled", new Vector2(refPosition.X + 100f - 1f, refPosition.Y + 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled", new Vector2(refPosition.X + 100f + 1f, refPosition.Y + 1f), Color.Black, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled").Y / 2f), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "Saving disabled", new Vector2(refPosition.X + 100f, refPosition.Y), Color.White, 0f, new Vector2(0f, g.theFontManager.GetFont().MeasureString("Saving disabled").Y / 2f), 1f, SpriteEffects.None, 0f);
	}

	public static void DrawASelect(SpriteBatch theSpriteBatch)
	{
		Rectangle value = new Rectangle(0, 0, 32, 32);
		theSpriteBatch.Draw(elementsTexture, new Vector2(refPosition.X + 925f, refPosition.Y - 3f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SELECT", new Vector2(refPosition.X + 1000f - 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("SELECT") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SELECT", new Vector2(refPosition.X + 1000f + 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("SELECT") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SELECT", new Vector2(refPosition.X + 1000f - 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("SELECT") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SELECT", new Vector2(refPosition.X + 1000f + 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("SELECT") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SELECT", new Vector2(refPosition.X + 1000f, refPosition.Y), Color.White, 0f, g.theFontManager.GetFont().MeasureString("SELECT") / 2f, 1f, SpriteEffects.None, 0f);
	}

	public static void DrawAShowTip(SpriteBatch theSpriteBatch, float x, float y)
	{
		Rectangle value = new Rectangle(0, 0, 32, 32);
		theSpriteBatch.Draw(elementsTexture, new Vector2(refPosition.X + x + 925f, refPosition.Y + y - 3f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SHOW TIP", new Vector2(refPosition.X + 1000f + x - 1f, refPosition.Y + y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("SHOW TIP") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SHOW TIP", new Vector2(refPosition.X + 1000f + x + 1f, refPosition.Y + y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("SHOW TIP") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SHOW TIP", new Vector2(refPosition.X + 1000f + x - 1f, refPosition.Y + y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("SHOW TIP") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SHOW TIP", new Vector2(refPosition.X + 1000f + x + 1f, refPosition.Y + y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("SHOW TIP") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "SHOW TIP", new Vector2(refPosition.X + 1000f + x, refPosition.Y + y), Color.White, 0f, g.theFontManager.GetFont().MeasureString("SHOW TIP") / 2f, 1f, SpriteEffects.None, 0f);
	}

	public static void DrawBBack(SpriteBatch theSpriteBatch)
	{
		Rectangle value = new Rectangle(32, 0, 32, 32);
		theSpriteBatch.Draw(elementsTexture, new Vector2(refPosition.X + 1090f, refPosition.Y - 3f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "BACK", new Vector2(refPosition.X + 1150f - 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("BACK") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "BACK", new Vector2(refPosition.X + 1150f + 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("BACK") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "BACK", new Vector2(refPosition.X + 1150f - 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("BACK") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "BACK", new Vector2(refPosition.X + 1150f + 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("BACK") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "BACK", new Vector2(refPosition.X + 1150f, refPosition.Y), Color.White, 0f, g.theFontManager.GetFont().MeasureString("BACK") / 2f, 1f, SpriteEffects.None, 0f);
	}

	public static void DrawYAssignSquadron(SpriteBatch theSpriteBatch)
	{
		Rectangle value = new Rectangle(64, 0, 32, 32);
		theSpriteBatch.Draw(elementsTexture, new Vector2(refPosition.X + 630f, refPosition.Y - 3f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "ASSIGN SQUADRON", new Vector2(refPosition.X + 770f - 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("ASSIGN SQUADRON") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "ASSIGN SQUADRON", new Vector2(refPosition.X + 770f + 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("ASSIGN SQUADRON") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "ASSIGN SQUADRON", new Vector2(refPosition.X + 770f - 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("ASSIGN SQUADRON") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "ASSIGN SQUADRON", new Vector2(refPosition.X + 770f + 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("ASSIGN SQUADRON") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "ASSIGN SQUADRON", new Vector2(refPosition.X + 770f, refPosition.Y), Color.White, 0f, g.theFontManager.GetFont().MeasureString("ASSIGN SQUADRON") / 2f, 1f, SpriteEffects.None, 0f);
	}

	public static void DrawYResetGroupNames(SpriteBatch theSpriteBatch)
	{
		Rectangle value = new Rectangle(64, 0, 32, 32);
		theSpriteBatch.Draw(elementsTexture, new Vector2(refPosition.X + 630f, refPosition.Y - 3f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "RESET GROUP NAMES", new Vector2(refPosition.X + 770f - 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("RESET GROUP NAMES") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "RESET GROUP NAMES", new Vector2(refPosition.X + 770f + 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("RESET GROUP NAMES") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "RESET GROUP NAMES", new Vector2(refPosition.X + 770f - 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("RESET GROUP NAMES") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "RESET GROUP NAMES", new Vector2(refPosition.X + 770f + 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("RESET GROUP NAMES") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "RESET GROUP NAMES", new Vector2(refPosition.X + 770f, refPosition.Y), Color.White, 0f, g.theFontManager.GetFont().MeasureString("RESET GROUP NAMES") / 2f, 1f, SpriteEffects.None, 0f);
	}

	public static void DrawYCredits(SpriteBatch theSpriteBatch)
	{
		Rectangle value = new Rectangle(64, 0, 32, 32);
		theSpriteBatch.Draw(elementsTexture, new Vector2(refPosition.X + 690f, refPosition.Y - 3f), value, Color.White, 0f, new Vector2(value.Width / 2, value.Height / 2), 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "VIEW CREDITS", new Vector2(refPosition.X + 800f - 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("VIEW CREDITS") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "VIEW CREDITS", new Vector2(refPosition.X + 800f + 1f, refPosition.Y - 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("VIEW CREDITS") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "VIEW CREDITS", new Vector2(refPosition.X + 800f - 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("VIEW CREDITS") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "VIEW CREDITS", new Vector2(refPosition.X + 800f + 1f, refPosition.Y + 1f), Color.Black, 0f, g.theFontManager.GetFont().MeasureString("VIEW CREDITS") / 2f, 1f, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(g.theFontManager.GetFont(), "VIEW CREDITS", new Vector2(refPosition.X + 800f, refPosition.Y), Color.White, 0f, g.theFontManager.GetFont().MeasureString("VIEW CREDITS") / 2f, 1f, SpriteEffects.None, 0f);
	}
}
