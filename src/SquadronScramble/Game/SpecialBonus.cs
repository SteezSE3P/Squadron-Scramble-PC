using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class SpecialBonus
{
	private enum Mode
	{
		off,
		truck,
		tank,
		balloon,
		bomber,
		biplane,
		helicopter,
		ptBoat,
		launcher,
		airShip,
		jeep,
		rare
	}

	private GameWorld g;

	private Vector2 position = new Vector2(-700f, -700f);

	private Vector2 momentum = new Vector2(0f, 0f);

	private bool active = false;

	private float direction = 0f;

	private float topTurnRate = 300f;

	private float turnRate = 0f;

	private float TURNINCREMENT = 40f;

	private int depth = 0;

	private float speed = 0f;

	private float presenceKnown = 0f;

	private float countdown = 0f;

	private float explosionScale = 0f;

	private int OPTIONS = 80;

	private int currentOptions = 0;

	private int PIVOTPOINT = 4;

	private float rotation = 0f;

	private bool vulnerable = false;

	private Rectangle shadowRect = new Rectangle(1091, 196, 40, 4);

	private Vector2 shadowOrigin = new Vector2(20f, 2f);

	private Vector2 shadowPosition;

	private float shadowScale = 0f;

	private float wobbleTimer = 0f;

	private float wobble = 0f;

	private Pilot targetPilot;

	private bool rareBeamOn = false;

	private float rareBeamFrame = 0f;

	private Rectangle beamRect;

	private float beamTimer = 0f;

	private bool pilotTaken = false;

	private float currentAnalogueInput = 0f;

	private bool moveLeft = false;

	private bool moveRight = false;

	private float targetHeight = 0f;

	private float animTimer = 0f;

	private float animSpeed = 0f;

	private float activeTimer = 0f;

	private float missileFlameTimer = 0f;

	private Mode currentMode = Mode.off;

	private int choice = 0;

	private int frameWidth = 128;

	private int frameHeight = 128;

	private float animFrame = 0f;

	private Rectangle sourceRect;

	private Vector2 origin = new Vector2(0f, 0f);

	private Vector2[] collisionOffsets;

	private int COLLISIONOFFSETSLIMIT = 2;

	private float bulletCollisionSize = 0f;

	private bool appendageOn = false;

	private float appendageDirection = 0f;

	private float appendageAnimFrame = 0f;

	private float appendageAnimTimer = 0f;

	private float appendageAnimSpeed = 0f;

	private Rectangle appendageSourceRect;

	private Vector2 appendagePosition = new Vector2(0f, 0f);

	private Explosion explosion;

	private ScreenScoreBonus theScreenScoreBonus;

	private Bullet[] bullets;

	private int BULLETLIMIT = 10;

	private int actualBulletLimit = 0;

	private float weaponDelayTimer = 0f;

	private Bomb[] bombs;

	private int BOMBLIMIT = 10;

	private int bombSupply = 0;

	private float bombDropX = 0f;

	private float BOMBERMAXRANGE = 5000f;

	private Missile[] missiles;

	private int MISSILELIMIT = 5;

	private int actualMissileLimit = 0;

	private Rectangle missileFlameRect;

	private Texture2D mSpriteTexture;

	public SpecialBonus(GameWorld gw)
	{
		g = gw;
		explosion = new Explosion(g);
		explosionScale = 1.5f;
		theScreenScoreBonus = new ScreenScoreBonus(g, this);
		currentOptions = OPTIONS;
		SetCountdown();
		collisionOffsets = new Vector2[COLLISIONOFFSETSLIMIT];
		for (int i = 0; i < collisionOffsets.Length; i++)
		{
			ref Vector2 reference = ref collisionOffsets[i];
			reference = new Vector2(0f, 0f);
		}
		bullets = new Bullet[BULLETLIMIT];
		for (int i = 0; i < BULLETLIMIT; i++)
		{
			bullets[i] = new Bullet(g);
		}
		missiles = new Missile[MISSILELIMIT];
		for (int i = 0; i < MISSILELIMIT; i++)
		{
			missiles[i] = new Missile(g);
		}
		bombs = new Bomb[BOMBLIMIT];
		for (int i = 0; i < BOMBLIMIT; i++)
		{
			bombs[i] = new Bomb(g);
		}
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetSpecialsTexture();
		explosion.LoadContent();
		for (int i = 0; i < bullets.Length; i++)
		{
			bullets[i].LoadContent();
		}
		for (int i = 0; i < bombs.Length; i++)
		{
			bombs[i].LoadContent();
		}
		for (int i = 0; i < missiles.Length; i++)
		{
			missiles[i].LoadContent();
		}
	}

	public void DrawDepth0(SpriteBatch theSpriteBatch)
	{
		if (depth == 0)
		{
			DrawSpecial(theSpriteBatch);
		}
	}

	public void DrawDepth1(SpriteBatch theSpriteBatch)
	{
		if (depth == 1)
		{
			DrawSpecial(theSpriteBatch);
		}
	}

	public void DrawDepth2(SpriteBatch theSpriteBatch)
	{
		if (depth == 2)
		{
			DrawSpecial(theSpriteBatch);
		}
	}

	public void DrawDepth3(SpriteBatch theSpriteBatch)
	{
		if (depth == 3)
		{
			DrawSpecial(theSpriteBatch);
		}
	}

	public void DrawDepth4(SpriteBatch theSpriteBatch)
	{
		if (depth == 4)
		{
			DrawSpecial(theSpriteBatch);
		}
	}

	public void DrawShadow(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, shadowPosition, shadowRect, Color.Black * 0.2f, 0f, shadowOrigin, shadowScale, SpriteEffects.None, 0f);
	}

	public void DrawSpecial(SpriteBatch theSpriteBatch)
	{
		DrawBombs(theSpriteBatch);
		DrawBullets(theSpriteBatch);
		DrawMissiles(theSpriteBatch);
		origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
		if (currentMode == Mode.biplane || currentMode == Mode.helicopter)
		{
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, direction, origin, 1.5f, SpriteEffects.None, 0f);
		}
		else if (currentMode == Mode.balloon)
		{
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, rotation / ((float)Math.PI * 12f), origin, 1.5f, SpriteEffects.None, 0f);
		}
		else if (currentMode == Mode.jeep)
		{
			if (direction == 0f)
			{
				theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, 0f, origin, 1.5f, SpriteEffects.FlipHorizontally, 0f);
			}
			else
			{
				theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
			}
		}
		else if (currentMode == Mode.rare)
		{
			if (rareBeamOn)
			{
				theSpriteBatch.Draw(mSpriteTexture, position, beamRect, Color.White * 0.75f, 0f, new Vector2(24f, -7f), 1.6f, SpriteEffects.None, 0f);
			}
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, wobble, origin, 1.5f, SpriteEffects.None, 0f);
		}
		else
		{
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, Color.White, 0f, origin, 1.5f, SpriteEffects.None, 0f);
		}
		if (appendageOn)
		{
			theSpriteBatch.Draw(mSpriteTexture, appendagePosition, appendageSourceRect, Color.White, appendageDirection, origin, 1.5f, SpriteEffects.None, 0f);
		}
		if (currentMode == Mode.launcher)
		{
			theSpriteBatch.Draw(mSpriteTexture, position + new Vector2(0f, -5f), missileFlameRect, Color.White, 0f, new Vector2(24f, 24f), 1.5f, SpriteEffects.None, 0f);
		}
		explosion.Draw(theSpriteBatch);
	}

	public void Animate(GameTime theGameTime)
	{
		if (currentMode == Mode.truck)
		{
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer < animSpeed)
			{
				if (direction == 0f)
				{
					animFrame = 0f;
				}
				else
				{
					animFrame = 36f;
				}
			}
			else if (direction == 0f)
			{
				animFrame = 1f;
			}
			else
			{
				animFrame = 37f;
			}
			if (animTimer >= animSpeed * 2f)
			{
				animTimer = 0f;
			}
		}
		if (currentMode == Mode.tank)
		{
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer < animSpeed)
			{
				animFrame = 2f;
			}
			else
			{
				animFrame = 3f;
			}
			if (animTimer >= animSpeed * 2f)
			{
				animTimer = 0f;
			}
		}
		if (currentMode == Mode.balloon)
		{
			animFrame = 4f;
			animTimer += animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			momentum.X = (float)Math.Sin(animTimer);
			if (animTimer >= (float)Math.PI * 2f)
			{
				animTimer = 0f;
			}
			float num = 5f;
			if (momentum.X > 0f && rotation < num)
			{
				rotation += (float)theGameTime.ElapsedGameTime.TotalSeconds * momentum.X;
			}
			if (momentum.X < 0f && rotation > 0f - num)
			{
				rotation += (float)theGameTime.ElapsedGameTime.TotalSeconds * momentum.X;
			}
		}
		if (currentMode == Mode.bomber)
		{
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer < animSpeed)
			{
				if (animFrame == 12f)
				{
					animFrame = 13f;
				}
				if (animFrame == 14f)
				{
					animFrame = 15f;
				}
			}
			else
			{
				if (animFrame == 13f)
				{
					animFrame = 12f;
				}
				if (animFrame == 15f)
				{
					animFrame = 14f;
				}
			}
			if (animTimer >= animSpeed * 2f)
			{
				animTimer = 0f;
			}
		}
		if (currentMode == Mode.biplane)
		{
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer < animSpeed)
			{
				if (animFrame == 8f)
				{
					animFrame = 9f;
				}
				if (animFrame == 10f)
				{
					animFrame = 11f;
				}
			}
			else
			{
				if (animFrame == 9f)
				{
					animFrame = 8f;
				}
				if (animFrame == 11f)
				{
					animFrame = 10f;
				}
			}
			if (animTimer >= animSpeed * 2f)
			{
				animTimer = 0f;
			}
		}
		if (currentMode == Mode.helicopter)
		{
			appendageAnimTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			appendageAnimFrame = 25f + appendageAnimTimer * appendageAnimSpeed;
			if (appendageAnimFrame >= 31f)
			{
				appendageAnimFrame = 25f;
				appendageAnimTimer = 0f;
			}
			if (direction < -(float)Math.PI / 12f && (int)animFrame > 16)
			{
				animFrame -= animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (direction > (float)Math.PI / 12f && (int)animFrame < 24)
			{
				animFrame += animSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
		}
		if (currentMode == Mode.ptBoat)
		{
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer > 0f)
			{
				animFrame = 31f;
			}
			if (animTimer > animSpeed)
			{
				animFrame = 32f;
			}
			if (animTimer > animSpeed * 2f)
			{
				animFrame = 33f;
			}
			if (animTimer >= animSpeed * 3f)
			{
				animTimer = 0f;
			}
		}
		if (currentMode == Mode.launcher)
		{
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer < animSpeed)
			{
				animFrame = 6f;
			}
			else
			{
				animFrame = 7f;
			}
			if (animTimer >= animSpeed * 2f)
			{
				animTimer = 0f;
			}
			if (missileFlameTimer > 0f)
			{
				missileFlameTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (missileFlameTimer == 0f)
			{
				missileFlameRect = new Rectangle(1232, 0, 48, 48);
			}
			if (missileFlameTimer > 0f && missileFlameTimer <= 0.1f)
			{
				missileFlameRect = new Rectangle(1040, 0, 48, 48);
			}
			if (missileFlameTimer > 0.1f && missileFlameTimer <= 0.2f)
			{
				missileFlameRect = new Rectangle(1088, 0, 48, 48);
			}
			if (missileFlameTimer > 0.2f && missileFlameTimer <= 0.3f)
			{
				missileFlameRect = new Rectangle(1136, 0, 48, 48);
			}
			if (missileFlameTimer > 0.3f && missileFlameTimer <= 0.4f)
			{
				missileFlameRect = new Rectangle(1184, 0, 48, 48);
			}
			if (missileFlameTimer > 0.4f)
			{
				missileFlameTimer = 0f;
			}
		}
		if (currentMode == Mode.jeep)
		{
			animTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (animTimer < animSpeed)
			{
				animFrame = 38f;
			}
			else
			{
				animFrame = 39f;
			}
			if (animTimer >= animSpeed * 2f)
			{
				animTimer = 0f;
			}
		}
		if (currentMode == Mode.rare)
		{
			if (rareBeamOn)
			{
				beamTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			else
			{
				beamTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (beamTimer < 0f)
			{
				beamTimer = 0f;
			}
			if (beamTimer == 0f)
			{
				beamRect = new Rectangle(1184, 48, 48, 48);
			}
			if (beamTimer > 0f && beamTimer <= 0.1f)
			{
				beamRect = new Rectangle(1040, 48, 48, 48);
			}
			if (beamTimer > 0.1f && beamTimer <= 0.2f)
			{
				beamRect = new Rectangle(1088, 48, 48, 48);
			}
			if (beamTimer > 0.2f)
			{
				beamRect = new Rectangle(1136, 48, 48, 48);
				beamTimer = 0.2f;
			}
		}
		int num2 = frameWidth;
		int num3 = frameHeight;
		sourceRect = new Rectangle((int)animFrame % 8 * num2, (int)animFrame / 8 * num3, frameWidth, frameHeight);
		appendageSourceRect = new Rectangle((int)appendageAnimFrame % 8 * num2, (int)appendageAnimFrame / 8 * num3, frameWidth, frameHeight);
	}

	public void Update(GameTime theGameTime)
	{
		Animate(theGameTime);
		UpdateBullets(theGameTime);
		UpdateMissiles(theGameTime);
		UpdateBombs(theGameTime);
		UpdateShadow(theGameTime);
		presenceKnown -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (presenceKnown < 0f)
		{
			presenceKnown = 0f;
		}
		if (currentMode == Mode.off)
		{
			countdown -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			int currentLevel = g.GetCurrentLevel();
			if ((int)countdown == 1)
			{
				if (General.GetNextRandom(0, 2000) == 0 && g.GetCurrentLevel() != 2)
				{
					currentMode = Mode.rare;
				}
				else
				{
					choice = General.GetNextRandom(0, currentOptions);
					currentMode = Mode.off;
					if (currentLevel == 1)
					{
						if (choice == PIVOTPOINT)
						{
							currentMode = Mode.tank;
						}
						else if (choice < PIVOTPOINT)
						{
							currentMode = Mode.balloon;
						}
					}
					if (currentLevel == 2)
					{
						if (choice == PIVOTPOINT)
						{
							currentMode = Mode.ptBoat;
						}
						else if (choice < PIVOTPOINT)
						{
							currentMode = Mode.biplane;
						}
					}
					if (currentLevel == 3)
					{
						if (choice == PIVOTPOINT)
						{
							currentMode = Mode.launcher;
						}
						else if (choice < PIVOTPOINT)
						{
							currentMode = Mode.airShip;
						}
					}
					if (currentLevel == 4)
					{
						if (choice == PIVOTPOINT)
						{
							currentMode = Mode.bomber;
						}
						else if (choice < PIVOTPOINT)
						{
							currentMode = Mode.truck;
						}
					}
					if (currentLevel == 5)
					{
						if (choice == PIVOTPOINT)
						{
							currentMode = Mode.helicopter;
						}
						else if (choice < PIVOTPOINT)
						{
							currentMode = Mode.jeep;
						}
					}
					countdown = -1f;
					if (currentMode != 0)
					{
						currentOptions = OPTIONS;
					}
					else
					{
						SpecialOff();
					}
				}
			}
		}
		if (currentMode == Mode.off)
		{
			position = new Vector2(-10000f, -10000f);
			momentum = new Vector2(0f, 0f);
			active = false;
		}
		else
		{
			UpdateSpecial(theGameTime);
		}
		explosion.Update(theGameTime);
		theScreenScoreBonus.Update(theGameTime);
	}

	public void UpdateShadow(GameTime theGameTime)
	{
		if (currentMode == Mode.truck)
		{
			shadowPosition = new Vector2(position.X + 0f, position.Y + 19f);
			shadowScale = 1.5f;
		}
		else if (currentMode == Mode.tank)
		{
			shadowPosition = new Vector2(position.X + 0f, position.Y + 21f);
			shadowScale = 2f;
		}
		else if (currentMode == Mode.jeep)
		{
			shadowPosition = new Vector2(position.X + 0f, position.Y + 19f);
			shadowScale = 1.5f;
		}
		else if (currentMode == Mode.launcher)
		{
			shadowPosition = new Vector2(position.X + 0f, position.Y + 19f);
			shadowScale = 1.5f;
		}
		else if (currentMode == Mode.rare)
		{
			shadowPosition = new Vector2(position.X, Level.GetGroundY() + 7f + (Level.GetGroundY() - position.Y) / 10f);
			shadowScale = position.Y / shadowPosition.Y * 1.25f;
		}
		else
		{
			shadowPosition = new Vector2(-10000f, -10000f);
			shadowScale = 1f;
		}
	}

	public void Shot(GameTime theGameTime, Color c)
	{
		if (currentMode != Mode.balloon && currentMode != Mode.jeep)
		{
			depth = 4;
		}
		explosion.TriggerExplosion(theGameTime, position, explosionScale);
		theScreenScoreBonus.DisplayScreenScoreBonus(c);
		SpecialOff();
	}

	public void UpdatePosition(GameTime theGameTime)
	{
		UpdateDirection(theGameTime);
		UpdateTurn(theGameTime);
		if (currentMode != Mode.balloon)
		{
			momentum.X = (float)Math.Cos(direction);
		}
		momentum.Y = (float)Math.Sin(direction);
		position.Y += momentum.Y * speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		position.X += momentum.X * speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
	}

	public void UpdateDirection(GameTime theGameTime)
	{
		direction = (direction + turnRate * ((float)Math.PI / 180f * (float)theGameTime.ElapsedGameTime.TotalSeconds)) % ((float)Math.PI * 2f);
		if (direction < 0f)
		{
			direction += (float)Math.PI * 2f;
		}
		if (direction > (float)Math.PI * 2f)
		{
			direction -= (float)Math.PI * 2f;
		}
	}

	public void UpdateTurn(GameTime theGameTime)
	{
		float num = currentAnalogueInput * topTurnRate;
		if (turnRate > num)
		{
			turnRate -= TURNINCREMENT;
			if (turnRate <= num)
			{
				turnRate = num;
			}
		}
		if (turnRate < num)
		{
			turnRate += TURNINCREMENT;
			if (turnRate >= num)
			{
				turnRate = num;
			}
		}
		if (num == 0f)
		{
			if (turnRate < 0f)
			{
				turnRate += TURNINCREMENT;
			}
			if (turnRate > 0f)
			{
				turnRate -= TURNINCREMENT;
			}
			if (turnRate > 0f - TURNINCREMENT && turnRate < TURNINCREMENT)
			{
				turnRate = 0f;
			}
		}
	}

	public void UpdateSpecial(GameTime theGameTime)
	{
		if (!active)
		{
			if (currentMode == Mode.truck)
			{
				if (General.GetNextRandom(0, 2) == 0)
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMinLimit() - 50f, Level.GetAbsoluteGroundY() + 5f);
					direction = 0f;
					animFrame = 0f;
				}
				else
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 50f, Level.GetAbsoluteGroundY() + 5f);
					direction = (float)Math.PI;
					animFrame = 36f;
				}
				depth = 4;
				currentAnalogueInput = 0f;
				speed = 120f;
				animSpeed = 0.125f;
				turnRate = 0f;
				animTimer = 0f;
				bulletCollisionSize = 30f;
				g.theSoundManager.SetTruckEnginePitch(-0.4f);
				g.theSoundManager.StartTruckEngine();
				presenceKnown = 3f;
				ref Vector2 reference = ref collisionOffsets[0];
				reference = new Vector2(0f, 0f);
				ref Vector2 reference2 = ref collisionOffsets[1];
				reference2 = new Vector2(0f, 0f);
			}
			if (currentMode == Mode.tank)
			{
				if (g.theSafeArea.GetScreenMode() == 0)
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 50f, Level.GetAbsoluteGroundY() - 7f);
				}
				else
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 50f, Level.GetAbsoluteGroundY() + 2f);
				}
				direction = (float)Math.PI;
				depth = 4;
				currentAnalogueInput = 0f;
				speed = 60f;
				turnRate = 0f;
				animSpeed = 0.25f;
				animTimer = 0f;
				animFrame = 2f;
				bulletCollisionSize = 40f;
				actualBulletLimit = 2;
				explosionScale = 1.5f;
				g.theSoundManager.SetTruckEnginePitch(-0.8f);
				g.theSoundManager.StartTruckEngine();
				presenceKnown = 3f;
				ref Vector2 reference3 = ref collisionOffsets[0];
				reference3 = new Vector2(0f, 0f);
				ref Vector2 reference4 = ref collisionOffsets[1];
				reference4 = new Vector2(0f, 0f);
			}
			if (currentMode == Mode.balloon)
			{
				position = new Vector2(Level.GetHangarPosition().X + 120f, Level.GetHangarPosition().Y + 50f);
				direction = 4.712389f;
				depth = 0;
				currentAnalogueInput = 0f;
				speed = 30f;
				turnRate = 0f;
				animSpeed = 0.6f;
				animTimer = 0f;
				animFrame = 4f;
				rotation = -1.75f;
				bulletCollisionSize = 20f;
				explosionScale = 2f;
				presenceKnown = 3f;
				ref Vector2 reference5 = ref collisionOffsets[0];
				reference5 = new Vector2(0f, 0f - bulletCollisionSize);
				ref Vector2 reference6 = ref collisionOffsets[1];
				reference6 = new Vector2(0f, bulletCollisionSize);
			}
			if (currentMode == Mode.bomber)
			{
				if (General.GetNextRandom(0, 2) == 0)
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + (float)General.GetNextRandom((int)BOMBERMAXRANGE / 2, (int)BOMBERMAXRANGE), Level.GetAbsoluteGroundY() - 200f - (float)General.GetNextRandom(0, 300));
					direction = (float)Math.PI;
					animFrame = 12f;
				}
				else
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMinLimit() - (float)General.GetNextRandom((int)BOMBERMAXRANGE / 2, (int)BOMBERMAXRANGE), Level.GetAbsoluteGroundY() - 200f - (float)General.GetNextRandom(0, 300));
					direction = 0f;
					animFrame = 14f;
				}
				depth = 3;
				currentAnalogueInput = 0f;
				speed = 350f;
				turnRate = 0f;
				animSpeed = 0.125f;
				animTimer = 0f;
				bulletCollisionSize = 40f;
				actualBulletLimit = 0;
				bombSupply = 10;
				bombDropX = General.GetNextRandom(0, 300);
				Level.SetFrontSearchLightsOn(b: true);
				presenceKnown = 30f;
				explosionScale = 2f;
				ref Vector2 reference7 = ref collisionOffsets[0];
				reference7 = new Vector2(0f, 0f);
				ref Vector2 reference8 = ref collisionOffsets[1];
				reference8 = new Vector2(0f, 0f);
			}
			if (currentMode == Mode.biplane)
			{
				if (General.GetNextRandom(0, 2) == 0)
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMinLimit() - 50f, Level.GetAbsoluteGroundY() - 400f - (float)General.GetNextRandom(0, 75));
					direction = (float)Math.PI / 6f;
					animFrame = 10f;
				}
				else
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 50f, Level.GetAbsoluteGroundY() - 400f - (float)General.GetNextRandom(0, 75));
					direction = 2.6179938f;
					animFrame = 8f;
				}
				depth = 3;
				currentAnalogueInput = 0f;
				speed = 400f;
				turnRate = 0f;
				animSpeed = 0.125f;
				animTimer = 0f;
				bulletCollisionSize = 30f;
				g.theSoundManager.StartBiplaneSound();
				presenceKnown = 3f;
				ref Vector2 reference9 = ref collisionOffsets[0];
				reference9 = new Vector2(0f, 0f);
				ref Vector2 reference10 = ref collisionOffsets[1];
				reference10 = new Vector2(0f, 0f);
			}
			if (currentMode == Mode.helicopter)
			{
				position = new Vector2(Level.GetHangarPosition().X + 120f, Level.GetHangarPosition().Y + 50f);
				targetHeight = Level.GetHangarPosition().Y - 100f;
				momentum = new Vector2(0f, 0f);
				direction = 0f;
				depth = 0;
				currentAnalogueInput = 0f;
				speed = 400f;
				turnRate = 1f;
				animSpeed = 20f;
				animTimer = 0f;
				animFrame = 20f;
				bulletCollisionSize = 40f;
				actualBulletLimit = 3;
				explosionScale = 1.5f;
				g.theSoundManager.StartHelicopterSound();
				presenceKnown = 3f;
				ref Vector2 reference11 = ref collisionOffsets[0];
				reference11 = new Vector2(0f, 0f);
				ref Vector2 reference12 = ref collisionOffsets[1];
				reference12 = new Vector2(0f, 0f);
				appendageOn = true;
				appendageAnimSpeed = 65f;
				appendageAnimTimer = 0f;
			}
			if (currentMode == Mode.ptBoat)
			{
				position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 80f, Level.GetAbsoluteGroundY() + 5f);
				momentum = new Vector2(0f, 0f);
				direction = (float)Math.PI;
				depth = 4;
				currentAnalogueInput = 0f;
				speed = 0.25f;
				turnRate = 0f;
				animSpeed = 0.15f;
				animTimer = 0f;
				animFrame = 31f;
				bulletCollisionSize = 40f;
				actualBulletLimit = 2;
				explosionScale = 2f;
				presenceKnown = 3f;
				ref Vector2 reference13 = ref collisionOffsets[0];
				reference13 = new Vector2(0f, 0f);
				ref Vector2 reference14 = ref collisionOffsets[1];
				reference14 = new Vector2(0f, 0f);
			}
			if (currentMode == Mode.launcher)
			{
				position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 50f, Level.GetAbsoluteGroundY() - 0f);
				direction = (float)Math.PI;
				depth = 3;
				currentAnalogueInput = 0f;
				speed = 120f;
				animSpeed = 0.125f;
				turnRate = 0f;
				animTimer = 0f;
				animFrame = 6f;
				bulletCollisionSize = 40f;
				actualMissileLimit = 2;
				g.theSoundManager.SetTruckEnginePitch(-0.4f);
				g.theSoundManager.StartTruckEngine();
				presenceKnown = 3f;
				explosionScale = 1.5f;
				ref Vector2 reference15 = ref collisionOffsets[0];
				reference15 = new Vector2(0f, 0f);
				ref Vector2 reference16 = ref collisionOffsets[1];
				reference16 = new Vector2(0f, 0f);
			}
			if (currentMode == Mode.airShip)
			{
				if (General.GetNextRandom(0, 2) == 0)
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMinLimit() - 50f, g.theSafeArea.GetStallCeiling());
					direction = 0f;
					animFrame = 34f;
				}
				else
				{
					position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 50f, g.theSafeArea.GetStallCeiling());
					direction = (float)Math.PI;
					animFrame = 35f;
				}
				depth = 2;
				currentAnalogueInput = 0f;
				speed = 120f;
				animSpeed = 0.125f;
				turnRate = 0f;
				animTimer = 0f;
				bulletCollisionSize = 30f;
				explosionScale = 2f;
				presenceKnown = 4f;
				ref Vector2 reference17 = ref collisionOffsets[0];
				reference17 = new Vector2(0f - bulletCollisionSize, 0f);
				ref Vector2 reference18 = ref collisionOffsets[1];
				reference18 = new Vector2(bulletCollisionSize, 0f);
			}
			if (currentMode == Mode.jeep)
			{
				if (General.GetNextRandom(0, 2) == 0)
				{
					if (g.theSafeArea.GetScreenMode() == 0)
					{
						position = new Vector2(g.theSafeArea.GetPlaneXMinLimit() - 50f, Level.GetAbsoluteGroundY() - 50f);
					}
					else
					{
						position = new Vector2(g.theSafeArea.GetPlaneXMinLimit() - 50f, Level.GetAbsoluteGroundY() - 44f);
					}
					direction = 0f;
					animFrame = 38f;
				}
				else
				{
					if (g.theSafeArea.GetScreenMode() == 0)
					{
						position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 50f, Level.GetAbsoluteGroundY() - 50f);
					}
					else
					{
						position = new Vector2(g.theSafeArea.GetPlaneXMaxLimit() + 50f, Level.GetAbsoluteGroundY() - 44f);
					}
					direction = (float)Math.PI;
					animFrame = 38f;
				}
				depth = 0;
				currentAnalogueInput = 0f;
				speed = 120f;
				animSpeed = 0.125f;
				turnRate = 0f;
				animTimer = 0f;
				bulletCollisionSize = 30f;
				g.theSoundManager.SetTruckEnginePitch(-0.2f);
				g.theSoundManager.StartTruckEngine();
				presenceKnown = 3f;
				ref Vector2 reference19 = ref collisionOffsets[0];
				reference19 = new Vector2(0f, 0f);
				ref Vector2 reference20 = ref collisionOffsets[1];
				reference20 = new Vector2(0f, 0f);
			}
			if (currentMode == Mode.rare)
			{
				position = new Vector2(20 + General.GetNextRandom(0, 680), g.theSafeArea.GetStallCeiling() - 1500f);
				direction = 0f;
				animFrame = 5f;
				depth = 3;
				currentAnalogueInput = 0f;
				speed = 0f;
				animSpeed = 0.125f;
				turnRate = 0f;
				animTimer = 0f;
				bulletCollisionSize = 30f;
				pilotTaken = false;
				presenceKnown = 100000f;
				ref Vector2 reference21 = ref collisionOffsets[0];
				reference21 = new Vector2(0f, 0f);
				ref Vector2 reference22 = ref collisionOffsets[1];
				reference22 = new Vector2(0f, 0f);
			}
			active = true;
			activeTimer = 0f;
		}
		if (active)
		{
			activeTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (currentMode != Mode.helicopter && currentMode != Mode.ptBoat)
			{
				UpdatePosition(theGameTime);
			}
			if (currentMode == Mode.tank && activeTimer > 2f)
			{
				weaponDelayTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
				if (weaponDelayTimer > 0.25f)
				{
					weaponDelayTimer = 0f;
					if (General.GetNextRandom(0, 3) == 1)
					{
						ShootBullet(theGameTime, 3.7699115f);
					}
				}
			}
			if (currentMode == Mode.bomber)
			{
				if ((momentum.X > 0f && position.X >= g.theSafeArea.GetPlaneXMinLimit() - 300f) || (momentum.X < 0f && position.X <= g.theSafeArea.GetPlaneXMaxLimit() + 300f))
				{
					g.theSoundManager.StartFlyBySound(-0.3f);
				}
				if ((momentum.X > 0f && position.X >= g.theSafeArea.GetPlaneXMinLimit() + bombDropX - 300f) || (momentum.X < 0f && position.X <= g.theSafeArea.GetPlaneXMaxLimit() - bombDropX + 300f))
				{
					weaponDelayTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
					if (weaponDelayTimer > 0.25f)
					{
						weaponDelayTimer = 0f;
						DropBomb(theGameTime);
					}
				}
			}
			if (currentMode == Mode.biplane)
			{
				if (activeTimer >= 1.25f && activeTimer <= 4.75f)
				{
					if (animFrame >= 10f)
					{
						currentAnalogueInput = -0.4f;
					}
					else
					{
						currentAnalogueInput = 0.4f;
					}
				}
				if (activeTimer > 4.75f)
				{
					currentAnalogueInput = 0f;
				}
			}
			if (currentMode == Mode.helicopter)
			{
				if (activeTimer > 1f)
				{
					moveLeft = false;
					moveRight = true;
				}
				if (activeTimer > 4f)
				{
					moveLeft = true;
					moveRight = false;
				}
				if (activeTimer > 8f)
				{
					moveLeft = false;
					moveRight = true;
					targetHeight = Level.GetHangarPosition().Y - 1000f;
				}
				if (!moveLeft && !moveRight)
				{
					if (direction > 0f)
					{
						direction -= turnRate * (float)theGameTime.ElapsedGameTime.TotalSeconds;
					}
					if (direction < 0f)
					{
						direction += turnRate * (float)theGameTime.ElapsedGameTime.TotalSeconds;
					}
				}
				if (moveLeft && direction > -(float)Math.PI / 5f)
				{
					direction -= turnRate * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				}
				if (moveRight && direction < (float)Math.PI / 5f)
				{
					direction += turnRate * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				}
				momentum.X = (float)Math.Sin(direction);
				float num = 0.3f;
				if (position.Y > targetHeight)
				{
					if (momentum.Y > 0f - num)
					{
						momentum.Y -= 0.3f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
						if (momentum.Y < 0f - num)
						{
							momentum.Y = 0f - num;
						}
					}
				}
				else
				{
					depth = 4;
					if (momentum.Y < 0f)
					{
						momentum.Y += 0.3f * (float)theGameTime.ElapsedGameTime.TotalSeconds;
						if (momentum.Y > 0f)
						{
							momentum.Y = 0f;
						}
					}
				}
				momentum += new Vector2((-0.5f + (float)General.GetNextRandom(0, 11) / 10f) * (float)theGameTime.ElapsedGameTime.TotalSeconds, (-0.5f + (float)General.GetNextRandom(0, 11) / 10f) * (float)theGameTime.ElapsedGameTime.TotalSeconds);
				position.Y += momentum.Y * speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				position.X += momentum.X * speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				appendagePosition = position;
				appendageDirection = direction;
				if (position.X < g.theSafeArea.GetPilotXMaxLimit() && ((int)animFrame == 16 || (int)animFrame == 24))
				{
					weaponDelayTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
					if (weaponDelayTimer > 0.15f)
					{
						weaponDelayTimer = 0f;
						if (General.GetNextRandom(0, 3) == 1)
						{
							position.Y += 20f;
							if ((int)animFrame == 16)
							{
								ShootBullet(theGameTime, (float)Math.PI * 4f / 5f);
							}
							if ((int)animFrame == 24)
							{
								ShootBullet(theGameTime, (float)Math.PI / 5f);
							}
							position.Y -= 20f;
						}
					}
				}
			}
			if (currentMode == Mode.ptBoat)
			{
				momentum.Y += (float)Math.Cos(activeTimer) * 0.1f;
				momentum.X += (float)Math.Cos(activeTimer * 0.3f) * -1f;
				position.Y += momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				position.X += momentum.X * speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				if (position.X < g.theSafeArea.GetPilotXMaxLimit())
				{
					weaponDelayTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
					if (weaponDelayTimer > 0.15f)
					{
						weaponDelayTimer = 0f;
						if (General.GetNextRandom(0, 3) == 1)
						{
							position.Y -= 20f;
							ShootBullet(theGameTime, 3.7699113f);
							position.Y += 20f;
						}
					}
				}
			}
			if (currentMode == Mode.launcher && activeTimer > 2f)
			{
				weaponDelayTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
				if (weaponDelayTimer > 0.25f)
				{
					weaponDelayTimer = 0f;
					if (General.GetNextRandom(0, 3) == 1)
					{
						ShootMissile(theGameTime, 3.7699115f);
					}
				}
			}
			if (currentMode == Mode.rare)
			{
				wobbleTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds * 10f;
				if (wobbleTimer > (float)Math.PI * 2f)
				{
					wobbleTimer -= (float)Math.PI * 2f;
				}
				wobble = (float)Math.Sin(wobbleTimer) / 10f;
				if ((targetPilot == null || !targetPilot.GetIsWalking()) && !pilotTaken)
				{
					int nextRandom = General.GetNextRandom(0, 8);
					if ((g.pilots[nextRandom].GetParticipant() != null || g.pilots[nextRandom].GetParticipantAI() != null) && g.pilots[nextRandom].GetIsWalking() && g.pilots[nextRandom].GetCurrentSquadronMember().GetScore() >= 1)
					{
						targetPilot = g.pilots[nextRandom];
					}
				}
				Vector2 vector = new Vector2(0f, 0f);
				if (targetPilot != null && targetPilot.GetIsWalking())
				{
					vector = new Vector2(targetPilot.GetPosition().X + targetPilot.GetMomentum().X * 45f, Level.GetPilotGroundY() - 60f);
				}
				else
				{
					rareBeamOn = false;
					g.theSoundManager.StopBeamSound();
					if (beamTimer == 0f)
					{
						vector = new Vector2(-200f, g.theSafeArea.GetStallCeiling() - 1500f);
						targetPilot = null;
					}
				}
				direction = (float)General.GetDirectionToTarget(position, vector);
				speed = 500f * (General.CheckDistance(position, vector) / 100f);
				if (targetPilot != null && General.CheckDistance(position, targetPilot.GetPosition()) < 70f)
				{
					if (!rareBeamOn && targetPilot.GetVulnerable())
					{
						rareBeamOn = true;
						pilotTaken = true;
						targetPilot.SetBeamed(b: true);
						g.theSoundManager.StartBeamSound();
					}
				}
				else
				{
					rareBeamOn = false;
				}
			}
		}
		if (position.X > g.theSafeArea.GetPlaneXMinLimit() - 10f && position.X < g.theSafeArea.GetPlaneXMaxLimit() + 10f && position.Y > g.theSafeArea.GetStallCeiling() - 10f)
		{
			vulnerable = true;
		}
		else
		{
			vulnerable = false;
		}
		if (currentMode == Mode.truck)
		{
			if (position.X > g.theSafeArea.GetPlaneXMaxLimit() + 50f)
			{
				SpecialOff();
			}
			if (position.X < g.theSafeArea.GetPlaneXMinLimit() - 50f)
			{
				SpecialOff();
			}
		}
		if (currentMode == Mode.tank && position.X < g.theSafeArea.GetPlaneXMinLimit() - 50f)
		{
			SpecialOff();
		}
		if (currentMode == Mode.balloon && position.Y < g.theSafeArea.GetStallCeiling() - 100f)
		{
			SpecialOff();
		}
		if (currentMode == Mode.bomber)
		{
			if (position.X < g.theSafeArea.GetPlaneXMinLimit() - BOMBERMAXRANGE - 50f)
			{
				SpecialOff();
			}
			if (position.X > g.theSafeArea.GetPlaneXMaxLimit() + BOMBERMAXRANGE + 50f)
			{
				SpecialOff();
			}
		}
		if (currentMode == Mode.biplane)
		{
			if (position.X > g.theSafeArea.GetPlaneXMaxLimit() + 50f)
			{
				SpecialOff();
			}
			if (position.X < g.theSafeArea.GetPlaneXMinLimit() - 50f)
			{
				SpecialOff();
			}
		}
		if (currentMode == Mode.helicopter && position.Y < g.theSafeArea.GetStallCeiling() - 100f)
		{
			SpecialOff();
		}
		if (currentMode == Mode.launcher && position.X < g.theSafeArea.GetPlaneXMinLimit() - 50f)
		{
			SpecialOff();
		}
		if (currentMode == Mode.airShip)
		{
			if (position.X > g.theSafeArea.GetPlaneXMaxLimit() + 50f)
			{
				SpecialOff();
			}
			if (position.X < g.theSafeArea.GetPlaneXMinLimit() - 50f)
			{
				SpecialOff();
			}
		}
		if (currentMode == Mode.jeep)
		{
			if (position.X > g.theSafeArea.GetPlaneXMaxLimit() + 50f)
			{
				SpecialOff();
			}
			if (position.X < g.theSafeArea.GetPlaneXMinLimit() - 50f)
			{
				SpecialOff();
			}
		}
	}

	public void SetCountdown()
	{
		if (currentOptions > 10)
		{
			currentOptions -= 10;
		}
		else
		{
			currentOptions = 10;
		}
		countdown = General.GetNextRandom(0, 10) + 15;
		if (g.theRoundManager.GetRoundNumber() == 1)
		{
			countdown += 35f;
		}
		if (Level.GetFrontSearchLightsOn())
		{
			countdown += 30f;
		}
	}

	public void SpecialOff()
	{
		active = false;
		vulnerable = false;
		currentMode = Mode.off;
		appendageOn = false;
		moveLeft = false;
		moveRight = false;
		position = new Vector2(-5000f, -5000f);
		explosionScale = 1.5f;
		missileFlameTimer = 0f;
		g.theSoundManager.StopTruckEngineSound();
		g.theSoundManager.StopFlyBySound();
		g.theSoundManager.StopBiplaneSound();
		g.theSoundManager.StopHelicopterSound();
		if (targetPilot != null)
		{
			targetPilot.SetBeamed(b: false);
			targetPilot = null;
		}
		rareBeamOn = false;
		g.theSoundManager.StopBeamSound();
		pilotTaken = false;
		SetCountdown();
	}

	public bool GetVulnerable()
	{
		return vulnerable;
	}

	public Vector2 GetCollisionOffset(int i)
	{
		return collisionOffsets[i];
	}

	public bool GetActive()
	{
		return active;
	}

	public float GetBulletCollisionSize()
	{
		return bulletCollisionSize;
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public Vector2 GetMomentum()
	{
		return momentum;
	}

	public int GetBulletLimit()
	{
		return bullets.Length;
	}

	public int GetMissileLimit()
	{
		return missiles.Length;
	}

	public int GetBombLimit()
	{
		return bombs.Length;
	}

	public Bullet GetBullet(int i)
	{
		return bullets[i];
	}

	public Missile GetMissile(int i)
	{
		return missiles[i];
	}

	public Bomb GetBomb(int i)
	{
		return bombs[i];
	}

	public void SetTargetPilot(Pilot p)
	{
		targetPilot = p;
	}

	public bool PresenceIsKnown()
	{
		if (presenceKnown == 0f)
		{
			return true;
		}
		return false;
	}

	public void DrawBullets(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < bullets.Length; i++)
		{
			bullets[i].Draw(theSpriteBatch);
		}
	}

	public void UpdateBullets(GameTime theGameTime)
	{
		for (int i = 0; i < bullets.Length; i++)
		{
			bullets[i].Update(theGameTime);
		}
	}

	public void DrawMissiles(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < missiles.Length; i++)
		{
			missiles[i].Draw(theSpriteBatch);
		}
	}

	public void UpdateMissiles(GameTime theGameTime)
	{
		for (int i = 0; i < missiles.Length; i++)
		{
			missiles[i].Update(theGameTime);
		}
	}

	public void DrawBombs(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < bombs.Length; i++)
		{
			bombs[i].Draw(theSpriteBatch);
		}
	}

	public void UpdateBombs(GameTime theGameTime)
	{
		for (int i = 0; i < bombs.Length; i++)
		{
			bombs[i].Update(theGameTime);
		}
	}

	public ScreenScoreBonus GetTheScreenScoreBonus()
	{
		return theScreenScoreBonus;
	}

	public void ShootBullet(GameTime theGameTime, float f)
	{
		int i;
		for (i = 0; i < actualBulletLimit - 1 && bullets[i].getBulletFired(); i++)
		{
		}
		if (!bullets[i].getBulletFired())
		{
			bullets[i].FireBullet(theGameTime, f, position);
		}
	}

	public void ShootMissile(GameTime theGameTime, float f)
	{
		int i;
		for (i = 0; i < actualMissileLimit - 1 && missiles[i].GetMissileFired(); i++)
		{
		}
		if (!missiles[i].GetMissileFired())
		{
			missiles[i].FireMissile(theGameTime, f, position);
			missileFlameTimer = 0.01f;
		}
	}

	public void DropBomb(GameTime theGameTime)
	{
		if (bombSupply > 0)
		{
			bombs[bombSupply - 1].BombDropped(this);
			bombSupply--;
		}
		if (bombSupply < 0)
		{
			bombSupply = 0;
		}
	}

	public bool GetPilotBulletDeadly(Bullet b)
	{
		if (currentMode == Mode.tank && b.GetPosition().Y > Level.GetAbsoluteGroundY() - 50f)
		{
			return false;
		}
		return true;
	}
}
