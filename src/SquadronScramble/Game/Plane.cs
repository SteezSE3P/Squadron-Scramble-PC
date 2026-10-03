using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Plane
{
	private GameWorld g;

	private Pilot thePilot;

	private Vector2 position = new Vector2(-10000f, -10000f);

	private Vector2 tailColPosition = new Vector2(-10000f, -10000f);

	private Vector2 centreColPosition = new Vector2(-10000f, -10000f);

	private Vector2 rearColPosition = new Vector2(-10000f, -10000f);

	private Vector2 frontColPosition = new Vector2(-10000f, -10000f);

	private Vector2 origin = new Vector2(29f, 24f);

	private float direction = 0f;

	private Vector2 momentum = new Vector2(0f, 0f);

	private bool inControl = false;

	private bool active = false;

	private bool collidable = false;

	private bool hasBeenShot = false;

	private bool stalled = false;

	private bool goingDown = false;

	private bool faceRight = true;

	private bool vulnerable = false;

	private bool takingOff = false;

	private bool wingLost = false;

	private bool gonnaBlow = false;

	private bool secondStrike = false;

	private bool stallLock = false;

	private bool hangarCleared = false;

	private bool engineOn = false;

	private bool cartwheeling = false;

	private bool controlsLocked = false;

	private int cartwheelingCounter = 0;

	private bool pilotStriken = false;

	private bool turnLockOn = false;

	private bool successiveFlipLock = false;

	private float vulnerableTimer = 0f;

	private float flipTimer = 0f;

	private float turnRate = 0f;

	private float turnTarget = 0f;

	private float fallingSpeed = 0f;

	private float collidableTimer = -1f;

	private float fuseTimer = -1f;

	private float smokeDamageTimer = 0f;

	private float successiveFlipLockTimer = 0f;

	public bool spinning = false;

	public bool topView = false;

	private int frameWidth = 48;

	private int frameHeight = 48;

	private float animFrame = 0f;

	public Rectangle sourceRect;

	public Rectangle cockpitSourceRect;

	public Rectangle landingGearSourceRect;

	public bool gearsUp = false;

	public float GEARSPEED = 10f;

	public float gearFrame = 0f;

	private Rectangle shadowRect = new Rectangle(403, 22, 40, 4);

	private Vector2 shadowOrigin = new Vector2(20f, 2f);

	private Vector2 shadowPosition;

	private float shadowScale = 0f;

	private float shadowTakeOffOffSet = 0f;

	public static float TOPSPEED = 400f;

	public static float TOPTURNRATE = 275f;

	public float TURNINCREMENT = 1750f;

	public float TURNFALL = 4f;

	public float TURNFALLTARGET = 30f;

	public float ENGINELOSS = 125f;

	public float stallSpeed = 100f;

	public float TURNFLIPLIMIT = 0.4f;

	public float FLIPLIMIT = 0.75f;

	public float FLIPSPEED = 0.025f;

	private float SPINSPEED = 0.02f;

	public float STALLRECOVERYSPEED = 300f;

	public static float TAKEOFFRATE = 300f;

	public float ASCENDRATE = 75f;

	public float DESCENDRATE = 400f;

	public float FALLRATE = 0.025f;

	private float BULLETPLANECOLLISIONSIZE = 12f;

	private float BULLETPLANETAILCOLLISIONSIZE = 10f;

	private float BULLETPILOTCOLLISIONSIZE = 12f;

	private float BULLETPARACHUTECOLLISIONSIZE = 13f;

	private float BULLETPILOTDODGESIZE = 100f;

	private float MISSILEPLANECOLLISIONSIZE = 12f;

	private float BOMBPILOTCOLLISIONSIZE = 65f;

	private float PLANEPLANECOLLISIONSIZE = 30f;

	private float WINGPILOTCOLLISIONSIZE = 12f;

	private float WINGPLANECOLLISIONSIZE = 12f;

	private float MINSHOOTALTITUDE = 70f;

	private float SUCCESSIVEFLIPLOCKTIME = 0.25f;

	private bool autoStall = false;

	private bool outlineOn = false;

	public float engineSpeed;

	public float topSpeed = TOPSPEED;

	private float enginePitch = 0f;

	private float divePitch = 0f;

	private float divePitchTimer = 0f;

	private float divePitchRate = 0.07f;

	private int diveTone = 0;

	public float topTurnRate = TOPTURNRATE;

	public Bullet[] bullets;

	public ExhaustManager theExhaustManager;

	public SmokeManager theSmokeManager;

	public ImpactManager theImpactManager;

	public DustCloud theDustCloud;

	public Explosion explosion;

	public GroundCrash theGroundCrash;

	public Flames theFlames;

	public Wing theWing;

	public Ignition theIgnition;

	public Splash splash;

	private Texture2D mSpriteTexture;

	private Texture2D mOutlineTexture;

	public Plane(GameWorld gw)
	{
		g = gw;
		bullets = new Bullet[CustomOptions.GetBulletLimit()];
		for (int i = 0; i < CustomOptions.GetBulletLimit(); i++)
		{
			bullets[i] = new Bullet(g);
		}
		explosion = new Explosion(g);
		splash = new Splash(g, this);
		theDustCloud = new DustCloud(g, this);
		theGroundCrash = new GroundCrash(g, this);
		theFlames = new Flames(this);
		theExhaustManager = new ExhaustManager(this);
		theSmokeManager = new SmokeManager(this);
		theImpactManager = new ImpactManager();
		theWing = new Wing(g);
		theIgnition = new Ignition(this);
		engineSpeed = topSpeed;
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetPlaneTexture();
		mOutlineTexture = TextureManager.GetPlaneOutlineTexture();
		for (int i = 0; i < bullets.Length; i++)
		{
			GetBullet(i).LoadContent();
		}
		explosion.LoadContent();
		splash.LoadContent();
		theDustCloud.LoadContent();
		theGroundCrash.LoadContent();
		theFlames.LoadContent();
		theExhaustManager.LoadContent();
		theSmokeManager.LoadContent();
		theImpactManager.LoadContent();
		theWing.LoadContent();
		theIgnition.LoadContent();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		if (thePilot.IsParticipating())
		{
			theExhaustManager.Draw(theSpriteBatch);
			theIgnition.Draw(theSpriteBatch);
			theSpriteBatch.Draw(mSpriteTexture, position, sourceRect, thePilot.GetCurrentColor(), direction, origin, 1.5f, SpriteEffects.None, 0f);
			DrawLandingGear(theSpriteBatch);
			if (thePilot.GetInPlane() && thePilot.GetThePlane() == this)
			{
				theSpriteBatch.Draw(mSpriteTexture, position, cockpitSourceRect, thePilot.GetCurrentColor(), direction, origin, 1.5f, SpriteEffects.None, 0f);
			}
			theFlames.Draw(theSpriteBatch);
			theSmokeManager.Draw(theSpriteBatch);
			theWing.Draw(theSpriteBatch, this);
			theImpactManager.Draw(theSpriteBatch);
		}
	}

	public void DrawBullets(SpriteBatch theSpriteBatch)
	{
		for (int i = 0; i < bullets.Length; i++)
		{
			bullets[i].Draw(theSpriteBatch);
		}
	}

	public void DrawShadow(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, shadowPosition, shadowRect, Color.Black * 0.2f, 0f, shadowOrigin, shadowScale, SpriteEffects.None, 0f);
	}

	public void DrawLandingGear(SpriteBatch theSpriteBatch)
	{
		theSpriteBatch.Draw(mSpriteTexture, position, landingGearSourceRect, Color.White, direction, origin, 1.5f, SpriteEffects.None, 0f);
	}

	public void DrawExplosions(SpriteBatch theSpriteBatch)
	{
		theDustCloud.Draw(theSpriteBatch);
		explosion.Draw(theSpriteBatch);
		theGroundCrash.Draw(theSpriteBatch);
		splash.Draw(theSpriteBatch);
	}

	public void DrawOutline(SpriteBatch theSpriteBatch)
	{
		Vector2 vector = position;
		int num = 0;
		int num2 = 70;
		int num3 = -20;
		int num4 = 30;
		if (stalled)
		{
			outlineOn = true;
		}
		if (vector.X < g.theSafeArea.GetPlaneXMinLimit() + (float)num2)
		{
			vector.X = g.theSafeArea.GetPlaneXMinLimit() + (float)num2;
		}
		if (vector.X > g.theSafeArea.GetPlaneXMaxLimit() - (float)num2)
		{
			vector.X = g.theSafeArea.GetPlaneXMaxLimit() - (float)num2;
		}
		if (vector.Y < g.theSafeArea.GetStallCeiling() + (float)num4)
		{
			vector.Y = g.theSafeArea.GetStallCeiling() + (float)num4;
		}
		if (vector == position)
		{
			outlineOn = false;
		}
		if (outlineOn && thePilot.GetParticipantAI() == null)
		{
			theSpriteBatch.Draw(mOutlineTexture, vector, sourceRect, thePilot.GetCurrentColor(), direction, origin, 1.5f, SpriteEffects.None, 0f);
			theSpriteBatch.Draw(mOutlineTexture, vector, new Rectangle(19, 57, 14, 27), thePilot.GetCurrentColor(), (float)General.GetDirectionToTarget(vector, position), new Vector2(-7f, 14f), 1.5f, SpriteEffects.None, 0f);
		}
	}

	public void Update(GameTime theGameTime, int theLevel)
	{
		if (thePilot.IsParticipating())
		{
			RunEngine(theGameTime, theLevel);
			CheckPosition(theGameTime, theLevel);
			UpdateShadow(theGameTime);
			UpdateCollisionBoxes(theGameTime);
			Animate(theGameTime);
			PlaySounds(theGameTime);
			for (int i = 0; i < CustomOptions.GetBulletLimit(); i++)
			{
				GetBullet(i).Update(theGameTime);
			}
			theWing.Update(theGameTime);
			theFlames.Update(theGameTime);
			theExhaustManager.Update(theGameTime);
			theSmokeManager.Update(theGameTime);
			theImpactManager.Update(theGameTime);
			theIgnition.Update(theGameTime);
			explosion.Update(theGameTime);
			splash.Update(theGameTime);
			theDustCloud.Update(theGameTime);
			theGroundCrash.Update(theGameTime);
		}
	}

	public void UpdateCollisionBoxes(GameTime theGameTime)
	{
		Vector2 vector = new Vector2((float)Math.Cos(direction) * 20f, (float)Math.Sin(direction) * 20f);
		Vector2 vector2 = new Vector2(0f, 0f);
		Vector2 vector3 = new Vector2((float)Math.Cos(direction) * -20f, (float)Math.Sin(direction) * -20f);
		Vector2 vector4 = new Vector2((float)Math.Cos(direction) * -20f, (float)Math.Sin(direction) * -20f);
		frontColPosition = position + vector;
		centreColPosition = position + vector2;
		rearColPosition = position + vector3;
		tailColPosition = position + vector4;
	}

	public bool CheckCollisionBoxes(Vector2 p, float f)
	{
		if (General.CheckDistance(p, frontColPosition) < f)
		{
			return true;
		}
		if (General.CheckDistance(p, centreColPosition) < f)
		{
			return true;
		}
		if (General.CheckDistance(p, rearColPosition) < f)
		{
			return true;
		}
		if (General.CheckDistance(p, tailColPosition) < f)
		{
			return true;
		}
		return false;
	}

	public void UpdateShadow(GameTime theGameTime)
	{
		if (!vulnerable)
		{
			if (vulnerableTimer > 0f)
			{
				shadowTakeOffOffSet = 0f - (Level.GetGroundY() - Level.GetTakeOffY()) * ((vulnerableTimer - 1f) / 0.5f);
			}
		}
		else
		{
			shadowTakeOffOffSet = 0f;
		}
		if (takingOff)
		{
			shadowTakeOffOffSet = 0f - (Level.GetGroundY() - Level.GetTakeOffY());
		}
		shadowPosition = new Vector2(position.X, Level.GetGroundY() + shadowTakeOffOffSet + 7f + (Level.GetGroundY() - position.Y) / 10f);
		shadowScale = position.Y / shadowPosition.Y * 1.25f;
	}

	public void Animate(GameTime theGameTime)
	{
		CheckLandingGears(theGameTime);
		int num = frameWidth;
		int num2 = frameHeight;
		int num3 = 0;
		int num4 = 16;
		int num5 = 24 + 8 * (int)gearFrame;
		if (spinning)
		{
			Spin(theGameTime);
		}
		if (topView)
		{
			TopView(theGameTime);
		}
		if (wingLost)
		{
			num3 = 8;
		}
		sourceRect = new Rectangle(((int)animFrame + num3) % 8 * num, ((int)animFrame + num3) / 8 * num2, frameWidth, frameHeight);
		cockpitSourceRect = new Rectangle(((int)animFrame + num4) % 8 * num, ((int)animFrame + num4) / 8 * num2, frameWidth, frameHeight);
		landingGearSourceRect = new Rectangle(((int)animFrame + num5) % 8 * num, ((int)animFrame + num5) / 8 * num2, frameWidth, frameHeight);
	}

	public void PlaySounds(GameTime theGameTime)
	{
		enginePitch = engineSpeed / TOPSPEED;
		g.theSoundManager.EnginePitch(this, enginePitch);
		if (goingDown)
		{
			divePitchTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds * divePitchRate;
			divePitch = (float)Math.Sin(divePitchTimer);
			g.theSoundManager.DivePitch(this, divePitch);
		}
	}

	public void AnalogueLeft(float f, GameTime theGameTime)
	{
		if (!takingOff && !stallLock && !controlsLocked && (!turnLockOn || faceRight))
		{
			turnLockOn = false;
			flipTimer = 0f;
			if (f < 0f - TURNFLIPLIMIT && !successiveFlipLock)
			{
				faceRight = true;
				successiveFlipLockTimer = SUCCESSIVEFLIPLOCKTIME;
			}
			float num = f * TOPTURNRATE;
			if (turnRate > num)
			{
				turnRate -= TURNINCREMENT * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (turnRate <= num && !stalled)
			{
				engineSpeed -= ENGINELOSS * (0f - f) * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
	}

	public void AnalogueRight(float f, GameTime theGameTime)
	{
		if (!takingOff && !stallLock && !controlsLocked && (!turnLockOn || !faceRight))
		{
			turnLockOn = false;
			flipTimer = 0f;
			if (f > TURNFLIPLIMIT && !successiveFlipLock)
			{
				faceRight = false;
				successiveFlipLockTimer = SUCCESSIVEFLIPLOCKTIME;
			}
			float num = f * TOPTURNRATE;
			if (turnRate < num)
			{
				turnRate += TURNINCREMENT * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (turnRate >= num && !stalled)
			{
				engineSpeed -= ENGINELOSS * f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
	}

	public void TurnLeft(GameTime theGameTime)
	{
		if (!takingOff && !stallLock && !controlsLocked && (!turnLockOn || faceRight))
		{
			turnLockOn = false;
			flipTimer = 0f;
			if (!successiveFlipLock)
			{
				faceRight = true;
				successiveFlipLockTimer = SUCCESSIVEFLIPLOCKTIME;
			}
			if (turnRate > 0f - topTurnRate)
			{
				turnRate -= TURNINCREMENT * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (turnRate <= 0f - topTurnRate && !stalled)
			{
				engineSpeed -= ENGINELOSS * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
	}

	public void TurnRight(GameTime theGameTime)
	{
		if (!takingOff && !stallLock && !controlsLocked && (!turnLockOn || !faceRight))
		{
			turnLockOn = false;
			flipTimer = 0f;
			if (!successiveFlipLock)
			{
				faceRight = false;
				successiveFlipLockTimer = SUCCESSIVEFLIPLOCKTIME;
			}
			if (turnRate < topTurnRate)
			{
				turnRate += TURNINCREMENT * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (turnRate >= topTurnRate && !stalled)
			{
				engineSpeed -= ENGINELOSS * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
	}

	public void NoTurn(GameTime theGameTime)
	{
		if (!takingOff && !controlsLocked && !turnLockOn)
		{
			if (turnRate < 0f)
			{
				turnRate += TURNINCREMENT * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (turnRate > 0f)
			{
				turnRate -= TURNINCREMENT * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (turnRate > 0f - TURNINCREMENT && turnRate < TURNINCREMENT)
			{
				turnRate = 0f;
			}
		}
	}

	public void Shoot(GameTime theGameTime)
	{
		if (vulnerable && gearFrame == 0f && !outlineOn && !controlsLocked)
		{
			int i;
			for (i = 0; i < CustomOptions.GetBulletLimit() - 1 && bullets[i].getBulletFired(); i++)
			{
			}
			if (!bullets[i].getBulletFired())
			{
				bullets[i].FireBullet(theGameTime, direction, position);
				if (thePilot.GetThePlane() == this)
				{
					thePilot.PlaneShootsVibration();
				}
			}
			else if (thePilot.GetParticipantAI() == null)
			{
				g.theSoundManager.MisfireSound();
			}
		}
		else if (thePilot.GetParticipantAI() == null)
		{
			g.theSoundManager.MisfireSound();
		}
	}

	public void BailRequest(GameTime theGameTime)
	{
		if (CustomOptions.GetManualBailEnabled() && thePilot.GetThePlane() == this && !takingOff)
		{
			thePilot.BailOut(position);
			if (General.GetNextRandom(0, 2) == 0)
			{
				turnRate -= TURNINCREMENT * 3f;
			}
			else
			{
				turnRate += TURNINCREMENT * 3f;
			}
			if (!stalled)
			{
				Stalled(theGameTime);
			}
			inControl = false;
			if (!hasBeenShot)
			{
				thePilot.PunishPilot(1);
			}
		}
	}

	public void Spin(GameTime theGameTime)
	{
		if (animFrame > 0f)
		{
			animFrame -= SPINSPEED * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		if (animFrame <= 0f)
		{
			animFrame = 7.9f;
		}
	}

	public void TopView(GameTime theGameTime)
	{
		if (animFrame < 3f)
		{
			animFrame += SPINSPEED * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
		if (animFrame > 3f)
		{
			animFrame -= SPINSPEED * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		}
	}

	public void CheckLandingGears(GameTime theGameTime)
	{
		if (gearsUp)
		{
			if (gearFrame > 0f)
			{
				gearFrame -= GEARSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (gearFrame <= 0f)
			{
				gearFrame = 0f;
			}
		}
		if (!gearsUp)
		{
			if (gearFrame < 2f)
			{
				gearFrame += GEARSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (gearFrame >= 2f)
			{
				gearFrame = 2f;
			}
		}
	}

	public void StartEngine()
	{
		if (!engineOn)
		{
			engineOn = true;
			theIgnition.SetActive(b: true);
			theExhaustManager.SetActive(b: true);
			theExhaustManager.SetShade(0);
			g.theSoundManager.IgnitionSound();
			g.theSoundManager.StartEngine(this);
			g.theSoundManager.StopStallSound(this);
		}
	}

	public void StopEngine()
	{
		engineOn = false;
		theIgnition.SetActive(b: false);
		theExhaustManager.SetActive(b: false);
		g.theSoundManager.StopEngine(this);
	}

	public void TakingOff(GameTime theGameTime, int l)
	{
		if (thePilot.GetThePlane() == this)
		{
			thePilot.PlaneTakingOffVibration();
		}
		if (l == 1 || l == 5 || l == 10)
		{
			StartEngine();
			engineSpeed += TAKEOFFRATE * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (engineSpeed >= topSpeed)
			{
				engineSpeed = topSpeed;
				takingOff = false;
				hangarCleared = true;
				vulnerableTimer = 1.5f;
				turnRate = 0f - TURNINCREMENT * 3f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
		if (l == 2)
		{
			if (Level.GetCarrierLiftGoingUp() && !hangarCleared)
			{
				position.Y = Level.GetCarrierLiftPosition().Y - 9f;
				position.X = Level.GetCarrierLiftPosition().X;
			}
			if (!Level.GetCarrierLiftGoingUp() || hangarCleared)
			{
				StartEngine();
				position.Y = Level.GetCarrierPosition().Y - 7f;
				engineSpeed += TAKEOFFRATE * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
				if ((double)engineSpeed >= (double)topSpeed / 2.5 && !hangarCleared)
				{
					g.GetHangar().SetBusy(b: false);
					hangarCleared = true;
				}
				if (engineSpeed >= topSpeed)
				{
					engineSpeed = topSpeed;
					takingOff = false;
					vulnerableTimer = 1.5f;
					turnRate = TURNINCREMENT * 3f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
				}
			}
		}
		if (l == 4 || l == 3)
		{
			StartEngine();
			engineSpeed += TAKEOFFRATE * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (engineSpeed >= topSpeed)
			{
				engineSpeed = topSpeed;
				takingOff = false;
				hangarCleared = true;
				vulnerableTimer = 1.5f;
				turnRate = TURNINCREMENT * 3f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
	}

	public void TakeOff(int theLevel)
	{
		StopEngine();
		active = true;
		collidable = false;
		collidableTimer = -1f;
		fuseTimer = -1f;
		vulnerable = false;
		vulnerableTimer = -1f;
		takingOff = true;
		turnLockOn = true;
		hasBeenShot = false;
		inControl = true;
		stalled = false;
		outlineOn = false;
		stallLock = false;
		topSpeed = TOPSPEED;
		topTurnRate = TOPTURNRATE;
		engineSpeed = 0f;
		turnRate = 0f;
		turnTarget = 0f;
		spinning = false;
		goingDown = false;
		wingLost = false;
		topView = false;
		gonnaBlow = false;
		secondStrike = false;
		hangarCleared = false;
		cartwheeling = false;
		controlsLocked = false;
		pilotStriken = false;
		cartwheelingCounter = 0;
		fallingSpeed = 0f;
		theSmokeManager.SetActive(b: false);
		theFlames.SetActive(b: false);
		if (theLevel == 1 || theLevel == 3 || theLevel == 5 || theLevel == 10)
		{
			position.X = Level.GetHangarPosition().X + 170f;
			position.Y = Level.GetTakeOffY();
			direction = 0f;
			momentum = new Vector2(1f, 0f);
			animFrame = 0f;
			faceRight = true;
			g.GetHangar().SetBusy(b: false);
		}
		if (theLevel == 2)
		{
			position.X = Level.GetCarrierLiftPosition().X;
			position.Y = Level.GetCarrierLiftPosition().Y;
			direction = (float)Math.PI;
			momentum = new Vector2(-1f, 0f);
			animFrame = 7f;
			faceRight = false;
		}
		if (theLevel == 3)
		{
			position.X = Level.GetHangarPosition().X + 70f;
			position.Y = Level.GetTakeOffY();
			direction = (float)Math.PI;
			momentum = new Vector2(-1f, 0f);
			animFrame = 7f;
			faceRight = false;
			g.GetHangar().SetBusy(b: false);
		}
		if (theLevel == 4)
		{
			position.X = Level.GetHangarPosition().X + 190f;
			position.Y = Level.GetTakeOffY();
			direction = (float)Math.PI;
			momentum = new Vector2(-1f, 0f);
			animFrame = 7f;
			faceRight = false;
			g.GetHangar().SetBusy(b: false);
		}
	}

	public void PilotStrike(GameTime theGameTime)
	{
		if (!pilotStriken)
		{
			pilotStriken = true;
			if (thePilot.GetThePlane() == this)
			{
				thePilot.PlaneHitVibration();
			}
			theSmokeManager.SetActive(b: true);
			StopEngine();
			topSpeed = TOPSPEED;
			collidableTimer = 2f;
			inControl = false;
			controlsLocked = true;
			topView = true;
			Stalled(theGameTime);
			spinning = false;
			theWing.WingBroken(this);
			wingLost = true;
			gonnaBlow = true;
			turnRate += TURNINCREMENT * 4f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			g.theSoundManager.HitSound();
		}
	}

	public void SmokeDamage(GameTime theGameTime)
	{
		hasBeenShot = true;
		topSpeed -= 20f;
		topTurnRate -= 20f;
		theSmokeManager.SetActive(b: true);
		g.theSoundManager.HitSound();
		smokeDamageTimer = 5f;
	}

	public void Shot(GameTime theGameTime)
	{
		if (!vulnerable)
		{
			SmokeDamage(theGameTime);
		}
		if (!vulnerable)
		{
			return;
		}
		hasBeenShot = true;
		int num = General.GetNextRandom(0, 10);
		if (gonnaBlow)
		{
			num = 0;
			secondStrike = true;
		}
		if (stalled)
		{
			num = 0;
		}
		if (g.theInGameScreenText.CheckIfSuddenDeath())
		{
			num = 0;
		}
		switch (num)
		{
		case 0:
			Explode(theGameTime);
			break;
		case 1:
			if (thePilot.GetThePlane() == this)
			{
				thePilot.PlaneHitVibration();
			}
			theFlames.SetActive(b: true);
			theFlames.SetMode(General.GetNextRandom(0, 2));
			theSmokeManager.SetActive(b: true);
			topSpeed = TOPSPEED + 200f;
			collidableTimer = 2f;
			inControl = false;
			goingDown = true;
			spinning = true;
			gonnaBlow = true;
			topView = false;
			turnTarget = TURNFALLTARGET / (float)General.GetNextRandom(1, 4);
			KnockedOffCourse(theGameTime, 2);
			g.theSoundManager.HitSound();
			g.theSoundManager.StartDive(this);
			diveTone = General.GetNextRandom(0, 2);
			if (diveTone == 0)
			{
				divePitchTimer = 0f;
			}
			else
			{
				divePitchTimer = (float)Math.PI;
			}
			break;
		case 3:
			if (thePilot.GetThePlane() == this)
			{
				thePilot.PlaneHitVibration();
			}
			topSpeed = TOPSPEED + 200f;
			collidableTimer = 2f;
			inControl = false;
			topView = true;
			goingDown = true;
			spinning = false;
			gonnaBlow = true;
			theFlames.SetActive(b: true);
			theFlames.SetMode(General.GetNextRandom(0, 2));
			theSmokeManager.SetActive(b: true);
			turnTarget = TURNFALLTARGET / (float)General.GetNextRandom(1, 4);
			KnockedOffCourse(theGameTime, 2);
			g.theSoundManager.HitSound();
			g.theSoundManager.StartDive(this);
			diveTone = General.GetNextRandom(0, 2);
			if (diveTone == 0)
			{
				divePitchTimer = 0f;
			}
			else
			{
				divePitchTimer = (float)Math.PI;
			}
			break;
		case 4:
			if (thePilot.GetThePlane() == this)
			{
				thePilot.PlaneHitVibration();
			}
			StopEngine();
			topSpeed = TOPSPEED;
			collidableTimer = 2f;
			inControl = false;
			topView = true;
			Stalled(theGameTime);
			spinning = false;
			theWing.WingBroken(this);
			wingLost = true;
			gonnaBlow = true;
			turnRate += TURNINCREMENT * 4f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			g.theSoundManager.HitSound();
			break;
		default:
			Explode(theGameTime);
			break;
		}
	}

	public void KnockedOffCourse(GameTime theGameTime, int i)
	{
		if (General.GetNextRandom(0, 2) == 0)
		{
			turnRate += TURNINCREMENT * (float)i * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		else
		{
			turnRate -= TURNINCREMENT * (float)i * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		if (turnRate > topTurnRate)
		{
			turnRate = topTurnRate;
		}
		if (turnRate < 0f - topTurnRate)
		{
			turnRate = 0f - topTurnRate;
		}
	}

	public void Explode(GameTime theGameTime)
	{
		if (vulnerable)
		{
			explosion.TriggerExplosion(theGameTime, position, 1.5f);
			RemovePlane();
			fuseTimer = -1f;
			if (thePilot.GetThePlane() == this && thePilot.GetInPlane())
			{
				thePilot.PlaneExplodedVibration();
				thePilot.BailOut(position);
			}
		}
	}

	public void ExplodeGround(GameTime theGameTime)
	{
		if (g.theRoundManager.GetRoundIsOver() || cartwheeling)
		{
			return;
		}
		if (((double)direction < 0.39269909262657166 || (double)direction > 5.733406752347946) && !stalled && inControl)
		{
			cartwheeling = true;
			cartwheelingCounter = General.GetNextRandom(0, 4);
			return;
		}
		if ((double)direction > 2.7488936483860016 && (double)direction < 3.534291833639145 && !stalled && inControl)
		{
			cartwheeling = true;
			cartwheelingCounter = General.GetNextRandom(0, 4);
			return;
		}
		if (direction > (float)Math.PI / 2f && (double)direction < 4.71238911151886)
		{
			theGroundCrash.TriggerExplosion(theGameTime, position, b: false);
		}
		else
		{
			theGroundCrash.TriggerExplosion(theGameTime, position, b: true);
		}
		RemovePlane();
		fuseTimer = -1f;
		if (thePilot.GetThePlane() == this && thePilot.GetInPlane())
		{
			thePilot.PilotDying(theGameTime);
			thePilot.PilotKilled(theGameTime);
			thePilot.GetTheScreenScore().SetTheNameColor(Color.Red);
			thePilot.GetTheScreenScore().DisplayScreenName();
			thePilot.GetTheScreenScore().SetNamePosition(position);
			thePilot.GetTheScreenScore().SetNameScorePosition(position);
		}
	}

	public void SplashSea(GameTime theGameTime)
	{
		if (!g.theRoundManager.GetRoundIsOver())
		{
			bool b = momentum.X >= 0f;
			splash.TriggerSplash(theGameTime, position, b, 0.3f);
			RemovePlane();
			if (thePilot.GetThePlane() == this && thePilot.GetInPlane())
			{
				thePilot.PilotDying(theGameTime);
				thePilot.PilotKilled(theGameTime);
			}
		}
	}

	public void Cartwheel(GameTime theGameTime)
	{
		stalled = true;
		controlsLocked = true;
		if (g.GetCurrentLevel() == 2 && position.Y >= Level.GetAbsoluteGroundY())
		{
			SplashSea(theGameTime);
		}
		if (General.isBetween(position.X, Level.GetLandingMinX(), Level.GetLandingMaxX()) && position.Y >= Level.GetGroundY() && momentum.Y > 0f)
		{
			cartwheelingCounter--;
			momentum.Y = -1f + 0.2f * (float)cartwheelingCounter;
			fallingSpeed = 100f;
			if (momentum.X > 0f)
			{
				turnRate = 400f;
			}
			else
			{
				turnRate = -400f;
			}
			wingLost = true;
			spinning = true;
			theSmokeManager.SetActive(b: true);
			if (cartwheelingCounter > 0)
			{
				g.theSoundManager.HitSound();
				if (thePilot.GetInPlane())
				{
					thePilot.PlaneHitGroundVibration();
				}
			}
			if (direction > (float)Math.PI / 2f && (double)direction < 4.71238911151886)
			{
				theDustCloud.TriggerDustCloud(theGameTime, position, b: false);
			}
			else
			{
				theDustCloud.TriggerDustCloud(theGameTime, position, b: true);
			}
		}
		if (cartwheelingCounter <= 0 || Math.Abs(momentum.X) < 0.2f)
		{
			if (momentum.X <= 0f)
			{
				theGroundCrash.TriggerExplosion(theGameTime, position, b: false);
			}
			else
			{
				theGroundCrash.TriggerExplosion(theGameTime, position, b: true);
			}
			RemovePlane();
			fuseTimer = -1f;
			if (thePilot.GetThePlane() == this && thePilot.GetInPlane())
			{
				thePilot.PilotDying(theGameTime);
				thePilot.PilotKilled(theGameTime);
				thePilot.GetTheScreenScore().SetTheNameColor(Color.Red);
				thePilot.GetTheScreenScore().DisplayScreenName();
				thePilot.GetTheScreenScore().SetNamePosition(position);
				thePilot.GetTheScreenScore().SetNameScorePosition(position);
			}
		}
	}

	public void RemovePlane()
	{
		active = false;
		inControl = false;
		goingDown = false;
		spinning = false;
		outlineOn = false;
		stalled = false;
		pilotStriken = false;
		cartwheeling = false;
		cartwheelingCounter = 0;
		controlsLocked = false;
		turnLockOn = false;
		StopEngine();
		theFlames.SetActive(b: false);
		theSmokeManager.SetActive(b: false);
		g.theSoundManager.StopDive(this);
		g.theSoundManager.StopEngine(this);
	}

	public void Stalled(GameTime theGameTime)
	{
		stalled = true;
		StopEngine();
		if (theFlames.GetActive())
		{
			Explode(theGameTime);
			return;
		}
		g.theSoundManager.StallSound(this);
		fallingSpeed = engineSpeed;
		if (thePilot.GetThePlane() == this)
		{
			thePilot.PlaneStalledVibration();
		}
	}

	public void StallRecovery(GameTime theGameTime)
	{
		if (stalled && autoStall && inControl && !controlsLocked)
		{
			stallLock = true;
			if (direction < (float)Math.PI / 3f && turnRate < topTurnRate)
			{
				turnRate += TURNINCREMENT * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (direction > (float)Math.PI * 2f / 3f && turnRate > 0f - topTurnRate)
			{
				turnRate -= TURNINCREMENT * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
		if (!stalled && autoStall)
		{
			stallLock = false;
		}
	}

	public void RunEngine(GameTime theGameTime, int l)
	{
		if (!active)
		{
			return;
		}
		if (takingOff)
		{
			TakingOff(theGameTime, l);
		}
		StallRecovery(theGameTime);
		if (!stalled && engineSpeed < stallSpeed && !takingOff)
		{
			Stalled(theGameTime);
		}
		if (stalled && inControl && (double)direction > 1.2566370964050293 && (double)direction < 1.884955644607544 && fallingSpeed >= STALLRECOVERYSPEED)
		{
			stalled = false;
			StartEngine();
			engineSpeed = fallingSpeed;
		}
		if (!stalled)
		{
			if (momentum.Y < 0f)
			{
				engineSpeed += ASCENDRATE * momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			else
			{
				engineSpeed += DESCENDRATE * momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
		if (smokeDamageTimer > 0f)
		{
			smokeDamageTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (smokeDamageTimer <= 0f)
			{
				smokeDamageTimer = 0f;
				hasBeenShot = false;
				theSmokeManager.SetActive(b: false);
			}
		}
		else if (thePilot.GetThePlane() == this && inControl)
		{
			if (topSpeed < TOPSPEED)
			{
				topSpeed += (float)theGameTime.ElapsedGameTime.TotalSeconds * 10f * CustomOptions.GetGameSpeed();
			}
			else
			{
				topSpeed = TOPSPEED;
			}
			if (topTurnRate < TOPTURNRATE)
			{
				topTurnRate += (float)theGameTime.ElapsedGameTime.TotalSeconds * 10f * CustomOptions.GetGameSpeed();
			}
			else
			{
				topTurnRate = TOPTURNRATE;
			}
		}
	}

	public void CheckOrientation(GameTime theGameTime)
	{
		if (direction > (float)Math.PI * 3f / 4f && direction < 3.926991f)
		{
			flipTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (flipTimer >= FLIPLIMIT)
			{
				flipTimer = FLIPLIMIT;
				faceRight = false;
			}
		}
		else if (direction < (float)Math.PI / 4f || direction > 5.4977875f)
		{
			flipTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (flipTimer >= FLIPLIMIT)
			{
				flipTimer = FLIPLIMIT;
				faceRight = true;
			}
		}
		else
		{
			flipTimer = 0f;
		}
		if (!stalled)
		{
			if (faceRight)
			{
				if (animFrame > 0f)
				{
					animFrame -= FLIPSPEED * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
				}
				if (animFrame <= 0f)
				{
					animFrame = 0f;
				}
			}
			else
			{
				if (animFrame < 4f)
				{
					animFrame += FLIPSPEED * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
				}
				if (animFrame >= 4f)
				{
					animFrame = 4f;
				}
			}
		}
		else if (!controlsLocked)
		{
			if ((double)animFrame < 2.5)
			{
				animFrame += FLIPSPEED * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if ((double)animFrame > 2.5)
			{
				animFrame -= FLIPSPEED * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
		}
		if ((g.GetCurrentLevel() == 1 || g.GetCurrentLevel() == 5 || g.GetCurrentLevel() == 10) && !vulnerable && position.Y < Level.GetTakeOffY())
		{
			animFrame = 1f;
		}
		if ((g.GetCurrentLevel() == 3 || g.GetCurrentLevel() == 4) && !vulnerable && position.Y < Level.GetTakeOffY())
		{
			animFrame = 3f;
		}
	}

	public void CheckPosition(GameTime theGameTime, int i)
	{
		if (active)
		{
			if (!stalled)
			{
				momentum.X = (float)Math.Cos(direction);
				momentum.Y = (float)Math.Sin(direction);
			}
			else
			{
				if (momentum.Y < 1f)
				{
					momentum.Y += FALLRATE;
				}
				else
				{
					momentum.Y = 1f;
				}
				if (momentum.X < 0f)
				{
					momentum.X += 0.005f;
				}
				else if (momentum.X > 0f)
				{
					momentum.X -= 0.005f;
				}
				else
				{
					momentum.X = 0f;
				}
				if (momentum.Y < 0f)
				{
					fallingSpeed += ASCENDRATE / 1.5f * momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
				}
				else
				{
					fallingSpeed += DESCENDRATE / 1.5f * momentum.Y * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
				}
			}
			if (position.Y > Level.GetGroundY() && !takingOff && General.isBetween(position.X, Level.GetLandingMinX(), Level.GetLandingMaxX()))
			{
				ExplodeGround(theGameTime);
			}
			if (i == 2 && position.Y > Level.GetAbsoluteGroundY() && vulnerable)
			{
				SplashSea(theGameTime);
			}
			if (position.Y < g.theSafeArea.GetStallCeiling() && !stalled)
			{
				engineSpeed -= 5f;
			}
			if (inControl)
			{
				CheckOrientation(theGameTime);
			}
			else if (goingDown)
			{
				if (thePilot.GetThePlane() == this)
				{
					thePilot.PlaneBurningVibration();
				}
				if (direction > (float)Math.PI / 2f && (double)direction < 4.71238911151886)
				{
					if (turnRate < 0f - turnTarget)
					{
						turnRate += TURNFALL;
					}
					if (turnRate > 0f - turnTarget)
					{
						turnRate -= TURNFALL;
					}
				}
				else
				{
					if (turnRate < turnTarget)
					{
						turnRate += TURNFALL;
					}
					if (turnRate > turnTarget)
					{
						turnRate -= TURNFALL;
					}
				}
			}
			if (fuseTimer >= 0f)
			{
				fuseTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if ((int)fuseTimer == 0)
			{
				Explode(theGameTime);
				fuseTimer = -1f;
			}
			if (successiveFlipLockTimer > 0f)
			{
				successiveFlipLock = true;
				successiveFlipLockTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
				if (successiveFlipLockTimer <= 0f)
				{
					successiveFlipLockTimer = 0f;
					successiveFlipLock = false;
				}
			}
			if (inControl)
			{
				if (position.Y > Level.GetPilotGroundY() - MINSHOOTALTITUDE)
				{
					gearsUp = false;
				}
				else
				{
					gearsUp = true;
				}
			}
			if (cartwheeling)
			{
				Cartwheel(theGameTime);
			}
			direction = (direction + turnRate * ((float)Math.PI / 180f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed())) % ((float)Math.PI * 2f);
			if (direction < 0f)
			{
				direction += (float)Math.PI * 2f;
			}
			if (direction > (float)Math.PI * 2f)
			{
				direction -= (float)Math.PI * 2f;
			}
			if (!stalled)
			{
				position += momentum * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			else
			{
				position.Y += momentum.Y * fallingSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
				position.X += momentum.X * engineSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (engineSpeed > topSpeed)
			{
				engineSpeed = topSpeed;
			}
			if (fallingSpeed > TOPSPEED)
			{
				fallingSpeed = TOPSPEED;
			}
			if (position.X < g.theSafeArea.GetPlaneXMinLimit())
			{
				position.X = g.theSafeArea.GetPlaneXMaxLimit();
			}
			if (position.X > g.theSafeArea.GetPlaneXMaxLimit())
			{
				position.X = g.theSafeArea.GetPlaneXMinLimit();
			}
		}
		else
		{
			position.X = -10000f;
			position.Y = -10000f;
		}
		if (collidableTimer > 0f)
		{
			collidableTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		if ((int)collidableTimer == 0)
		{
			collidable = true;
		}
		if (vulnerableTimer > 0f)
		{
			vulnerableTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		if ((int)vulnerableTimer == 0)
		{
			vulnerable = true;
		}
		if (position.Y < Level.GetTakeOffY() - 10f)
		{
			turnLockOn = false;
		}
		if (i != 2)
		{
		}
	}

	public void InCloud()
	{
		theExhaustManager.SetDensity(1f);
		if (g.GetCurrentLevel() == 5 && position.Y > Level.GetAbsoluteGroundY() - 200f)
		{
			theExhaustManager.SetShade(140);
			theExhaustManager.SetDensity(0.4f);
		}
	}

	public float GetAnimFrame()
	{
		return animFrame;
	}

	public bool GetCollidable()
	{
		return collidable;
	}

	public Bullet GetBullet(int i)
	{
		return bullets[i];
	}

	public float GetDirection()
	{
		return direction;
	}

	public float GetEngineSpeed()
	{
		return engineSpeed;
	}

	public Explosion GetExplosion()
	{
		return explosion;
	}

	public bool GetInControl()
	{
		return inControl;
	}

	public bool GetControlsLocked()
	{
		return controlsLocked;
	}

	public Vector2 GetMomentum()
	{
		return momentum;
	}

	public bool GetActive()
	{
		return active;
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public Vector2 GetExhaustPosition()
	{
		return new Vector2(position.X + (float)Math.Cos(direction) * -35f, position.Y + (float)Math.Sin(direction) * -35f);
	}

	public Vector2 GetTailColPosition()
	{
		return tailColPosition;
	}

	public Flames GetTheFlames()
	{
		return theFlames;
	}

	public Pilot GetThePilot()
	{
		return thePilot;
	}

	public Wing GetTheWing()
	{
		return theWing;
	}

	public float GetBulletPlaneTailCollisionSize()
	{
		return BULLETPLANETAILCOLLISIONSIZE;
	}

	public float GetBulletPlaneCollisionSize()
	{
		return BULLETPLANECOLLISIONSIZE;
	}

	public float GetBulletPilotCollisionSize()
	{
		return BULLETPILOTCOLLISIONSIZE;
	}

	public float GetBulletParachuteCollisionSize()
	{
		return BULLETPARACHUTECOLLISIONSIZE;
	}

	public float GetBulletPilotDodgeSize()
	{
		return BULLETPILOTDODGESIZE;
	}

	public float GetMissilePlaneCollisionSize()
	{
		return MISSILEPLANECOLLISIONSIZE;
	}

	public float GetBombPilotCollisionSize()
	{
		return BOMBPILOTCOLLISIONSIZE;
	}

	public float GetPlanePlaneCollisionSize()
	{
		return PLANEPLANECOLLISIONSIZE;
	}

	public float GetWingPilotCollisionSize()
	{
		return WINGPILOTCOLLISIONSIZE;
	}

	public float GetWingPlaneCollisionSize()
	{
		return WINGPLANECOLLISIONSIZE;
	}

	public bool GetStalled()
	{
		return stalled;
	}

	public bool GetVulnerable()
	{
		return vulnerable;
	}

	public bool GetGonnaBlow()
	{
		return gonnaBlow;
	}

	public bool GetTakingOff()
	{
		return takingOff;
	}

	public bool GetSecondStrike()
	{
		return secondStrike;
	}

	public void SetActive(bool b)
	{
		active = b;
	}

	public void SetThePilot(Pilot p)
	{
		thePilot = p;
	}

	public void SetPositionX(float f)
	{
		position.X = f;
	}

	public void SetPositionY(float f)
	{
		position.Y = f;
	}

	public void SetAutoStall(bool b)
	{
		autoStall = b;
	}
}
