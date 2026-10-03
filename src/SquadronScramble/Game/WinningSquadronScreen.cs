using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class WinningSquadronScreen
{
	private GameWorld g;

	private float timer = 0f;

	private Squadron winningSquadron;

	private Slot winningSlot;

	private SpriteFont squadronFont;

	private Vector2 squadronNameOrigin;

	private float squadronNameScale = 1f;

	private Vector2 squadronNamePosition = new Vector2(0f, 0f);

	private SpriteFont winsFont;

	private Vector2 winsOrigin;

	private float winsScale = 1f;

	private Vector2 winsPosition = new Vector2(0f, 0f);

	private float textSpeed = 0f;

	private float TEXTSTARTSPEED = 1500f;

	private float TEXTSLOWSPEED = 5f;

	private float TEXTSLOWPOSITION = 400f;

	private float TEXTDECELLERATE = 5000f;

	private int formation = 0;

	private bool initialisePlanes = false;

	private float planeTimer = 0f;

	private float textActivateTime = 0f;

	private Texture2D backgroundTexture;

	private Vector2 screenPosition = new Vector2(0f, 0f);

	private VictoryPlane[] theVictoryPlanes;

	private int numberOfVictoryPlanes = 4;

	public WinningSquadronScreen(GameWorld gw)
	{
		g = gw;
		theVictoryPlanes = new VictoryPlane[numberOfVictoryPlanes];
		for (int i = 0; i < numberOfVictoryPlanes; i++)
		{
			theVictoryPlanes[i] = new VictoryPlane(g);
		}
	}

	public void SetWinningSquadron(Squadron theSquadron)
	{
		winningSquadron = theSquadron;
	}

	public void SetWinningSlot(Slot theSlot)
	{
		winningSlot = theSlot;
	}

	public void LoadContent()
	{
		backgroundTexture = TextureManager.GetScreenWinningSquadronTexture();
		for (int i = 0; i < numberOfVictoryPlanes; i++)
		{
			theVictoryPlanes[i].LoadContent();
		}
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(origin: new Vector2(0f, 0f), texture: backgroundTexture, position: screenPosition, sourceRectangle: null, color: Color.White, rotation: 0f, scale: 1.5f, effects: SpriteEffects.None, layerDepth: 0f);
		for (int i = 0; i < numberOfVictoryPlanes; i++)
		{
			theVictoryPlanes[i].Draw(theSpriteBatch);
		}
		DrawText(theSpriteBatch);
	}

	public void DrawText(SpriteBatch theSpriteBatch)
	{
		squadronFont = g.theFontManager.GetFontGameTitle();
		squadronNameOrigin = new Vector2(squadronFont.MeasureString(winningSlot.GetSlotFullName()).X / 2f, squadronFont.MeasureString(winningSlot.GetSlotFullName()).Y / 2f);
		winsFont = g.theFontManager.GetFontGameTitle();
		winsOrigin = new Vector2(squadronFont.MeasureString("WINS").X / 2f, squadronFont.MeasureString("WINS").Y / 2f);
		General.DrawEmbossedString(theSpriteBatch, squadronFont, winningSlot.GetSlotFullName(), squadronNamePosition, new Color(winningSquadron.GetTheColor().R - 25, winningSquadron.GetTheColor().G - 25, winningSquadron.GetTheColor().B - 25), winningSquadron.GetTheColor(), new Color(winningSquadron.GetTheColor().R - 50, winningSquadron.GetTheColor().G - 50, winningSquadron.GetTheColor().B - 50), 0f, squadronNameOrigin, squadronNameScale, 1, 1f);
		General.DrawEmbossedString(theSpriteBatch, squadronFont, "WINS!", winsPosition, new Color(winningSquadron.GetTheColor().R - 25, winningSquadron.GetTheColor().G - 25, winningSquadron.GetTheColor().B - 25), winningSquadron.GetTheColor(), new Color(winningSquadron.GetTheColor().R - 50, winningSquadron.GetTheColor().G - 50, winningSquadron.GetTheColor().B - 50), 0f, winsOrigin, winsScale, 1, 1f);
	}

	public void Update(GameTime theGameTime)
	{
		FadeControl(theGameTime);
		UpdateText(theGameTime);
		UpdateVictoryPlanes(theGameTime);
	}

	public void FadeControl(GameTime theGameTime)
	{
		if (timer == 0f)
		{
			initialisePlanes = true;
			planeTimer = 0f;
			textActivateTime = 10000f;
			formation = General.GetNextRandom(0, 11);
			g.theSoundManager.StartFlyBySound(-0.3f);
		}
		timer = (timer += (float)theGameTime.ElapsedGameTime.TotalSeconds);
		if ((int)timer == 0)
		{
			g.theScreenFadeOverlay.UnFadeScreen(1f);
		}
		if ((float)(int)timer < textActivateTime)
		{
			SetUpText();
		}
		if ((float)(int)timer == textActivateTime + 5f)
		{
			g.theScreenFadeOverlay.FadeScreen(1f);
		}
		if ((float)(int)timer == textActivateTime + 7f)
		{
			g.theSoundManager.StopFlyBySound();
			timer = 0f;
			g.theScreenFadeOverlay.UnFadeScreen(1f);
			g.GoToTopPilotsScreen();
		}
	}

	private void UpdateText(GameTime theGameTime)
	{
		if (squadronNamePosition.X > TEXTSLOWPOSITION)
		{
			if (textSpeed > TEXTSLOWSPEED)
			{
				textSpeed -= TEXTDECELLERATE * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (textSpeed < TEXTSLOWSPEED)
			{
				textSpeed = TEXTSLOWSPEED;
			}
		}
		squadronNamePosition.X += textSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		winsPosition.X -= textSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
	}

	public void UpdateVictoryPlanes(GameTime theGameTime)
	{
		if (!initialisePlanes)
		{
			planeTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		else
		{
			planeTimer = 0f;
		}
		for (int i = 0; i < numberOfVictoryPlanes; i++)
		{
			theVictoryPlanes[i].Update(theGameTime);
		}
		if (formation == 0)
		{
			if (initialisePlanes)
			{
				textActivateTime = 3f;
				for (int i = 0; i < numberOfVictoryPlanes; i++)
				{
					theVictoryPlanes[i].SetStartValues(new Vector2(-400f, 200 + i * 100), 0f, winningSquadron.GetTheColor());
					theVictoryPlanes[i].SetSmokeColor(winningSquadron.GetTheColor());
				}
			}
			else
			{
				for (int i = 0; i < numberOfVictoryPlanes; i++)
				{
					theVictoryPlanes[i].SmokeActivated(b: true);
				}
			}
		}
		if (formation == 1)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[2].SetStartValues(new Vector2(615f, 875f), 4.712389f, winningSquadron.GetTheColor());
				theVictoryPlanes[0].SetStartValues(new Vector2(665f, 875f), 4.712389f, winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetAnimFrame(2);
				theVictoryPlanes[0].SetAnimFrame(2);
				theVictoryPlanes[2].SetSmokeColor(Color.Red);
				theVictoryPlanes[0].SetSmokeColor(Color.Red);
				theVictoryPlanes[1].SetStartValues(new Vector2(-400f, 10000f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(-400f, 10000f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetSmokeColor(winningSquadron.GetTheColor());
			}
			else
			{
				if (planeTimer > 1.5f)
				{
					theVictoryPlanes[2].SetTargetAnimFrame(0);
					theVictoryPlanes[0].SetTargetAnimFrame(4);
				}
				if ((double)planeTimer > 1.5 && planeTimer < 3.5f)
				{
					theVictoryPlanes[2].SetCurrentAnalogueInput(-0.4f);
					theVictoryPlanes[0].SetCurrentAnalogueInput(0.4f);
					theVictoryPlanes[2].SmokeActivated(b: true);
					theVictoryPlanes[0].SmokeActivated(b: true);
				}
				else
				{
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[0].SetCurrentAnalogueInput(0f);
				}
				if (planeTimer > 4f)
				{
					theVictoryPlanes[2].SmokeActivated(b: false);
					theVictoryPlanes[0].SmokeActivated(b: false);
				}
				if ((double)planeTimer < 2.5)
				{
					theVictoryPlanes[1].SetStartValues(new Vector2(-100f, 50f), (float)Math.PI / 8f, winningSquadron.GetTheColor());
				}
				else if (planeTimer > 3f && planeTimer < 5.25f)
				{
					theVictoryPlanes[1].SmokeActivated(b: true);
				}
				else
				{
					theVictoryPlanes[1].SmokeActivated(b: false);
				}
			}
		}
		if (formation == 2)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[1].SetStartValues(new Vector2(-400f, 300f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetStartValues(new Vector2(-400f, 400f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(-600f, 250f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetSmokeColor(Color.Red);
				theVictoryPlanes[0].SetStartValues(new Vector2(-600f, 250f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[0].SetSmokeColor(Color.Red);
			}
			else
			{
				float num = 5f;
				float num2 = 2.5f;
				float num3 = 150f;
				float num4 = (float)Math.Cos(planeTimer * num3 * (float)theGameTime.ElapsedGameTime.TotalSeconds);
				theVictoryPlanes[3].SetAnimFrame((int)(planeTimer * num2 % 8f));
				theVictoryPlanes[3].SetCurrentAnalogueInput(num4 / 3.5f);
				theVictoryPlanes[0].SetAnimFrame((int)(planeTimer * num2 % 8f));
				theVictoryPlanes[0].SetCurrentAnalogueInput(num4 / 3.5f);
				if (num4 > 0f && theVictoryPlanes[0].GetPosition().Y < 350f)
				{
					theVictoryPlanes[3].SetPositionY(theVictoryPlanes[0].GetPosition().Y);
					theVictoryPlanes[0].SetPositionY(10000f);
				}
				if (num4 < 0f && theVictoryPlanes[3].GetPosition().Y > 350f)
				{
					theVictoryPlanes[0].SetPositionY(theVictoryPlanes[3].GetPosition().Y);
					theVictoryPlanes[3].SetPositionY(-10000f);
				}
				theVictoryPlanes[0].SmokeActivated(b: true);
				theVictoryPlanes[1].SmokeActivated(b: true);
				theVictoryPlanes[2].SmokeActivated(b: true);
				theVictoryPlanes[3].SmokeActivated(b: true);
			}
		}
		if (formation == 3)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[0].SetStartValues(new Vector2(-300f, 350f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetStartValues(new Vector2(1580f, 350f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetStartValues(new Vector2(10000f, 10000f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(10000f, 10000f), (float)Math.PI, winningSquadron.GetTheColor());
			}
			else
			{
				float num = 5f;
				float num2 = 2.5f;
				float num3 = 150f;
				float num5 = 3.5f;
				float num4 = (float)Math.Cos(planeTimer * num3 * (float)theGameTime.ElapsedGameTime.TotalSeconds);
				if (planeTimer < 2f)
				{
					theVictoryPlanes[0].SetAnimFrame(4 - (int)(planeTimer * num2) % 8);
					theVictoryPlanes[0].SetCurrentAnalogueInput(num4 / num5);
					theVictoryPlanes[1].SetAnimFrame(4 - (int)(planeTimer * num2) % 8);
					theVictoryPlanes[1].SetCurrentAnalogueInput(num4 / num5);
				}
				else
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(0f);
				}
				theVictoryPlanes[0].SmokeActivated(b: true);
				theVictoryPlanes[1].SmokeActivated(b: true);
			}
		}
		if (formation == 4)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[0].SetStartValues(new Vector2(-480f, 250f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetStartValues(new Vector2(1760f, 450f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetStartValues(new Vector2(10000f, 10000f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(10000f, 10000f), (float)Math.PI, winningSquadron.GetTheColor());
			}
			else
			{
				float num = 5f;
				float num2 = 2.5f;
				float num3 = 150f;
				float num5 = 3.5f;
				float num4 = (float)Math.Cos(planeTimer * num3 * (float)theGameTime.ElapsedGameTime.TotalSeconds);
				theVictoryPlanes[0].SetAnimFrame((int)(planeTimer * num2) % 8);
				theVictoryPlanes[0].SetCurrentAnalogueInput(num4 / num5);
				theVictoryPlanes[1].SetAnimFrame((int)(planeTimer * num2) % 8);
				theVictoryPlanes[1].SetCurrentAnalogueInput(num4 / num5);
				theVictoryPlanes[0].SmokeActivated(b: true);
				theVictoryPlanes[1].SmokeActivated(b: true);
			}
		}
		if (formation == 5)
		{
			if (initialisePlanes)
			{
				textActivateTime = 3f;
				for (int i = 0; i < numberOfVictoryPlanes; i++)
				{
					theVictoryPlanes[i].SetStartValues(new Vector2(-400f, 200 + i * 100), 0f, winningSquadron.GetTheColor());
					theVictoryPlanes[i].SetSmokeColor(winningSquadron.GetTheColor());
				}
			}
			else
			{
				if ((double)planeTimer > 0.75)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(-0.1f);
				}
				if ((double)planeTimer > 1.25)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(-0.1f);
				}
				if ((double)planeTimer > 1.75)
				{
					theVictoryPlanes[2].SetCurrentAnalogueInput(-0.1f);
				}
				if ((double)planeTimer > 2.25)
				{
					theVictoryPlanes[3].SetCurrentAnalogueInput(-0.1f);
				}
				for (int i = 0; i < numberOfVictoryPlanes; i++)
				{
					theVictoryPlanes[i].SmokeActivated(b: true);
				}
			}
		}
		if (formation == 6)
		{
			if (initialisePlanes)
			{
				textActivateTime = 3f;
				for (int i = 0; i < numberOfVictoryPlanes; i++)
				{
					theVictoryPlanes[i].SetStartValues(new Vector2(565 + i * 50, 1070f), 4.712389f, winningSquadron.GetTheColor());
					theVictoryPlanes[i].SetSmokeColor(winningSquadron.GetTheColor());
					theVictoryPlanes[i].SetAnimFrame(2);
				}
			}
			else
			{
				for (int i = 0; i < numberOfVictoryPlanes; i++)
				{
					theVictoryPlanes[i].SmokeActivated(b: true);
				}
				if ((double)planeTimer > 1.5)
				{
					theVictoryPlanes[0].SetTargetAnimFrame(0);
					theVictoryPlanes[1].SetTargetAnimFrame(0);
					theVictoryPlanes[2].SetTargetAnimFrame(4);
					theVictoryPlanes[3].SetTargetAnimFrame(4);
				}
				if ((double)planeTimer > 1.5)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(-0.2f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(-0.1f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0.1f);
					theVictoryPlanes[3].SetCurrentAnalogueInput(0.2f);
				}
				if ((double)planeTimer > 3.75)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(-0f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(-0f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[3].SetCurrentAnalogueInput(0f);
				}
			}
		}
		if (formation == 7)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[0].SetStartValues(new Vector2(-300f, 400f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetStartValues(new Vector2(1580f, 400f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetStartValues(new Vector2(-350f, 350f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(1630f, 350f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[0].SetAnimFrame(0);
				theVictoryPlanes[1].SetAnimFrame(4);
				theVictoryPlanes[2].SetAnimFrame(0);
				theVictoryPlanes[3].SetAnimFrame(4);
				theVictoryPlanes[0].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetSmokeColor(Color.Red);
				theVictoryPlanes[2].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetSmokeColor(Color.Red);
			}
			else
			{
				theVictoryPlanes[0].SmokeActivated(b: true);
				theVictoryPlanes[1].SmokeActivated(b: true);
				theVictoryPlanes[2].SmokeActivated(b: true);
				theVictoryPlanes[3].SmokeActivated(b: true);
				if ((double)planeTimer > 0.5)
				{
					theVictoryPlanes[0].SetTargetAnimFrame(2);
					theVictoryPlanes[1].SetTargetAnimFrame(2);
					theVictoryPlanes[2].SetTargetAnimFrame(2);
					theVictoryPlanes[3].SetTargetAnimFrame(2);
				}
				if ((double)planeTimer > 0.5)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(-0.1f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(-0.1f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(-0.1f);
					theVictoryPlanes[3].SetCurrentAnalogueInput(-0.1f);
				}
				if ((double)planeTimer > 0.75)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[3].SetCurrentAnalogueInput(0f);
				}
			}
		}
		if (formation == 8)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[0].SetStartValues(new Vector2(-300f, 220f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetStartValues(new Vector2(1580f, 260f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetStartValues(new Vector2(-350f, 470f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(1630f, 510f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[0].SetAnimFrame(0);
				theVictoryPlanes[1].SetAnimFrame(4);
				theVictoryPlanes[2].SetAnimFrame(0);
				theVictoryPlanes[3].SetAnimFrame(4);
				theVictoryPlanes[0].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetSmokeColor(Color.Red);
				theVictoryPlanes[2].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetSmokeColor(Color.Red);
			}
			else
			{
				theVictoryPlanes[0].SmokeActivated(b: true);
				theVictoryPlanes[1].SmokeActivated(b: true);
				theVictoryPlanes[2].SmokeActivated(b: true);
				theVictoryPlanes[3].SmokeActivated(b: true);
				if ((double)planeTimer > 0.5)
				{
					theVictoryPlanes[0].SetTargetAnimFrame(2);
					theVictoryPlanes[1].SetTargetAnimFrame(2);
					theVictoryPlanes[2].SetTargetAnimFrame(2);
					theVictoryPlanes[3].SetTargetAnimFrame(2);
				}
				if ((double)planeTimer > 0.5)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(0.1f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(0.1f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0.1f);
					theVictoryPlanes[3].SetCurrentAnalogueInput(0.1f);
				}
				if ((double)planeTimer > 0.75)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[3].SetCurrentAnalogueInput(0f);
				}
			}
		}
		if (formation == 9)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[0].SetStartValues(new Vector2(1580f, 350f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetStartValues(new Vector2(-300f, 350f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[0].SetAnimFrame(4);
				theVictoryPlanes[1].SetAnimFrame(0);
				theVictoryPlanes[0].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetStartValues(new Vector2(10000f, 10000f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(10000f, 10000f), (float)Math.PI, winningSquadron.GetTheColor());
			}
			else
			{
				theVictoryPlanes[0].SmokeActivated(b: true);
				theVictoryPlanes[1].SmokeActivated(b: true);
				if ((double)planeTimer > 1.5)
				{
					theVictoryPlanes[0].SetTargetAnimFrame(6);
					theVictoryPlanes[1].SetTargetAnimFrame(2);
				}
				if ((double)planeTimer > 1.5)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(0.1f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(-0.1f);
				}
				if ((double)planeTimer > 2.25)
				{
					theVictoryPlanes[0].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[1].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[3].SetCurrentAnalogueInput(0f);
				}
			}
		}
		if (formation == 10)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[0].SetStartValues(new Vector2(10000f, 10000f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(10000f, 10000f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[0].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetStartValues(new Vector2(-310f, 500f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetAnimFrame(0);
				theVictoryPlanes[1].SetSmokeColor(Color.Red);
				theVictoryPlanes[2].SetStartValues(new Vector2(-310f, 525f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetAnimFrame(0);
				theVictoryPlanes[2].SetSmokeColor(winningSquadron.GetTheColor());
			}
			else
			{
				theVictoryPlanes[0].SmokeActivated(b: false);
				theVictoryPlanes[1].SmokeActivated(b: true);
				theVictoryPlanes[2].SmokeActivated(b: true);
				theVictoryPlanes[3].SmokeActivated(b: false);
				if ((double)planeTimer > 0.5)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(0.1f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0.1f);
				}
				if ((double)planeTimer > 0.75)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
				}
				if ((double)planeTimer > 1.75)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(-0.335f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(-0.335f);
				}
				if ((double)planeTimer > 5.35)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
				}
			}
		}
		if (formation == 11)
		{
			if (initialisePlanes)
			{
				textActivateTime = 5f;
				theVictoryPlanes[0].SetStartValues(new Vector2(10000f, 10000f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetStartValues(new Vector2(10000f, 10000f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[0].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[3].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetStartValues(new Vector2(-310f, 650f), 0f, winningSquadron.GetTheColor());
				theVictoryPlanes[1].SetAnimFrame(0);
				theVictoryPlanes[1].SetSmokeColor(winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetStartValues(new Vector2(1590f, 650f), (float)Math.PI, winningSquadron.GetTheColor());
				theVictoryPlanes[2].SetAnimFrame(4);
				theVictoryPlanes[2].SetSmokeColor(winningSquadron.GetTheColor());
			}
			else
			{
				theVictoryPlanes[0].SmokeActivated(b: false);
				theVictoryPlanes[1].SmokeActivated(b: true);
				theVictoryPlanes[2].SmokeActivated(b: true);
				theVictoryPlanes[3].SmokeActivated(b: false);
				if ((double)planeTimer > 0.5)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(-0.1f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0.1f);
				}
				if ((double)planeTimer > 0.75)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
				}
				if ((double)planeTimer > 1.75)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(-0.335f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0.335f);
				}
				if ((double)planeTimer > 5.35)
				{
					theVictoryPlanes[1].SetCurrentAnalogueInput(0f);
					theVictoryPlanes[2].SetCurrentAnalogueInput(0f);
				}
			}
		}
		if (timer >= 0.5f)
		{
			initialisePlanes = false;
		}
	}

	public void SetUpText()
	{
		float num = 550f;
		squadronNamePosition = new Vector2(0f - num, 295f);
		winsPosition = new Vector2(1280f + num, 425f);
		textSpeed = TEXTSTARTSPEED;
	}
}
