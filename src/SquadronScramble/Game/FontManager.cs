using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class FontManager
{
	private GameWorld g;

	private SpriteFont font;

	private SpriteFont screenScoreFont;

	private SpriteFont clockFont;

	private SpriteFont roundFont;

	private SpriteFont hiScoreTitleFont;

	private SpriteFont hiScoreFont;

	private SpriteFont suddenDeathFont;

	private SpriteFont fontGameTitle;

	private SpriteFont fontGameHeading1;

	private SpriteFont fontGameHeading2;

	private SpriteFont fontGameHeading3;

	public FontManager(GameWorld gw)
	{
		g = gw;
	}

	public void LoadContent(ContentManager theContentManager)
	{
		font = theContentManager.Load<SpriteFont>("mySpriteFont1");
		clockFont = theContentManager.Load<SpriteFont>("mySpriteFont2");
		roundFont = theContentManager.Load<SpriteFont>("MySpriteFont3");
		hiScoreTitleFont = theContentManager.Load<SpriteFont>("mySpriteFont4");
		hiScoreFont = theContentManager.Load<SpriteFont>("mySpriteFont5");
		screenScoreFont = theContentManager.Load<SpriteFont>("ScreenScoreFont");
		suddenDeathFont = theContentManager.Load<SpriteFont>("mySpriteFont6");
		fontGameTitle = theContentManager.Load<SpriteFont>("fontGameTitle");
		fontGameHeading1 = theContentManager.Load<SpriteFont>("fontGameHeading1");
		fontGameHeading2 = theContentManager.Load<SpriteFont>("fontGameHeading2");
		fontGameHeading3 = theContentManager.Load<SpriteFont>("fontGameHeading3");
	}

	public SpriteFont GetFont()
	{
		return font;
	}

	public SpriteFont GetScreenScoreFont()
	{
		return screenScoreFont;
	}

	public SpriteFont GetClockFont()
	{
		return clockFont;
	}

	public SpriteFont GetRoundFont()
	{
		return roundFont;
	}

	public SpriteFont GetHiScoreFont()
	{
		return hiScoreFont;
	}

	public SpriteFont GetHiScoreTitleFont()
	{
		return hiScoreTitleFont;
	}

	public SpriteFont GetSuddenDeathFont()
	{
		return suddenDeathFont;
	}

	public SpriteFont GetFontGameTitle()
	{
		return fontGameTitle;
	}

	public SpriteFont GetFontGameHeading1()
	{
		return fontGameHeading1;
	}

	public SpriteFont GetFontGameHeading2()
	{
		return fontGameHeading2;
	}

	public SpriteFont GetFontGameHeading3()
	{
		return fontGameHeading3;
	}
}
