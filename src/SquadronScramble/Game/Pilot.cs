using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public class Pilot
{
	private enum State
	{
		off,
		deathPause,
		starting,
		freeFalling,
		parachuting,
		parachutingShot,
		hittingTheGround,
		walking,
		walkingShot,
		enteringHangar,
		enteringTower,
		switchInTower,
		requestExitingTower,
		exitingTower
	}

	private GameWorld g;

	private int thePilotNumber;

	private ScreenScore theScreenScore;

	private bool participating = false;

	private bool pilotAvailable = true;

	private Player participantPlayer;

	private AIPlayer participantAI;

	private Squadron currentSquadron;

	private SquadronMember currentSquadronMember = null;

	private Plane thePlane;

	private Plane[] availablePlanes = new Plane[2];

	private bool vulnerable = true;

	private bool flown = false;

	private bool inPlane = false;

	private bool isJumping = false;

	private bool scrambled = false;

	private bool onTheGround = false;

	private bool beamed = false;

	private float vulnerableTimer = 0f;

	private Color currentColor;

	public ImpactManager theImpactManager;

	public SplashPilot splash;

	private bool walkingLeft = false;

	private State currentState = State.starting;

	private Vector2 position;

	private Vector2 momentum = new Vector2(0f, 0f);

	private float animFrame;

	public Rectangle sourceRect;

	private int pilotSpeedX;

	private int pilotSpeedY;

	private float rotation = 0f;

	private float rotationRock = 0f;

	private float rotationRockTimer = 0f;

	private float ROTATIONROCKSPEED = 3f;

	private float ROTATIONDAMPENING = 200f;

	private Vector2 colPosition1 = new Vector2(0f, 0f);

	private Vector2 colPosition2 = new Vector2(0f, 0f);

	private float pilotGroundY = 0f;

	private int frameWidth = 72;

	private int frameHeight = 72;

	private float animCounter = 0f;

	private static float ANIMSPEED = 10f;

	private static float FLASHSPEED = 20f;

	private int flashCount = 0;

	private float bounceCounter = 0f;

	private float jumpCounter = 0f;

	private float lookAroundTimer = 0f;

	private float lookAroundDirection = 0f;

	private Rectangle shadowRect = new Rectangle(589, 44, 14, 4);

	private Vector2 shadowOrigin = new Vector2(7f, 2f);

	private Vector2 shadowPosition;

	private float shadowScale = 0f;

	private float gracePeriod = 0f;

	private float deathPauseTimer = 0f;

	private float startTimer = 0f;

	private float FORCESTARTTIME = 3f;

	private static float GRACEPERIODTIME = 1f;

	private float DEATHPAUSETIME = 2f;

	private static int WALKINGSPEED = 290;

	private static int INJUREDSPEED = 215;

	private static int JUMPINGSPEED = 12;

	private static int PARACHUTINGSPEED = 150;

	private static float FREEFALLTIMELIMIT = 0.3f;

	private static int FREEFALLSPEED = 400;

	private static float BOUNCEANIMSPEED = 0.05f;

	private static float FALLINGANIMSPEED = 1f;

	private static int DEADFLASHLIMIT = 5;

	private static float CONTROLLERFLASHTIME = 10f;

	private static float PARACHUTETOPLEANSPEED = 0.3f;

	private static float PARACHUTELEANMOMENTUM = 0.9f;

	private static float INVULNERABLETIME = 0.25f;

	private Texture2D mSpriteTexture;

	public Pilot(GameWorld gw, int n, Plane p, Plane pb)
	{
		g = gw;
		thePilotNumber = n;
		availablePlanes[0] = p;
		availablePlanes[1] = pb;
		thePlane = availablePlanes[0];
		position = new Vector2(-300f, -300f);
		availablePlanes[0].SetThePilot(this);
		availablePlanes[1].SetThePilot(this);
		pilotSpeedX = WALKINGSPEED;
		pilotSpeedY = PARACHUTINGSPEED;
		theScreenScore = new ScreenScore(g, this);
		theImpactManager = new ImpactManager();
		splash = new SplashPilot(g);
	}

	public void LoadContent()
	{
		mSpriteTexture = TextureManager.GetPilotTexture();
		theImpactManager.LoadContent();
		splash.LoadContent();
	}

	public void Draw(SpriteBatch theSpriteBatch)
	{
		if (IsParticipating())
		{
			theImpactManager.Draw(theSpriteBatch);
			theSpriteBatch.Draw(origin: new Vector2(frameWidth / 2, frameHeight / 2), texture: mSpriteTexture, position: position, sourceRectangle: sourceRect, color: currentColor, rotation: rotation, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
			splash.Draw(theSpriteBatch);
		}
	}

	public void DrawShadow(SpriteBatch theSpriteBatch)
	{
		if (animFrame != 7f)
		{
			theSpriteBatch.Draw(mSpriteTexture, shadowPosition, shadowRect, Color.Black * 0.2f, 0f, shadowOrigin, shadowScale, SpriteEffects.None, 0f);
		}
	}

	public void Update(GameTime theGameTime, int i)
	{
		if (participating)
		{
			if (g.theRoundManager.GetRoundIsPlayable())
			{
				UpdateParticipant(theGameTime);
				startTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
				theImpactManager.Update(theGameTime);
			}
			CheckPosition(theGameTime, i);
			UpdateShadow(theGameTime);
			Animate(theGameTime);
			theScreenScore.Update(theGameTime);
			splash.Update(theGameTime);
			if (currentState == State.starting && startTimer > FORCESTARTTIME)
			{
				FirePressed(theGameTime);
			}
		}
	}

	public void UpdateShadow(GameTime theGameTime)
	{
		shadowPosition = new Vector2(position.X, pilotGroundY + 23f + (pilotGroundY - position.Y) / 10f);
		shadowScale = position.Y / shadowPosition.Y;
	}

	public void UpdateParticipant(GameTime theGameTime)
	{
		if (participantPlayer != null)
		{
			participantPlayer.Update(theGameTime, this);
		}
		if (participantAI != null)
		{
			participantAI.Update(theGameTime, this);
		}
	}

	public void Animate(GameTime theGameTime)
	{
		int num = frameWidth;
		int num2 = frameHeight;
		sourceRect = new Rectangle((int)animFrame % 8 * num, (int)animFrame / 8 * num2, frameWidth, frameHeight);
		if (currentState == State.parachuting)
		{
			rotationRockTimer += ROTATIONROCKSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			rotationRock = (float)Math.Cos(rotationRockTimer);
			rotation += rotationRock / ROTATIONDAMPENING;
		}
		else
		{
			rotation = 0f;
			rotationRock = 0f;
			rotationRockTimer = 0f;
		}
	}

	public bool CheckCollisionBoxes(Vector2 p, float f)
	{
		if (General.CheckDistance(p, colPosition1) < f)
		{
			return true;
		}
		if (General.CheckDistance(p, colPosition2) < f)
		{
			return true;
		}
		return false;
	}

	public void NoLean(GameTime theGameTime)
	{
		if (currentState == State.walking)
		{
			momentum.X = 0f;
			bounceCounter = 0f;
			if (isJumping)
			{
				animFrame = 48f;
				lookAroundTimer = (float)General.GetNextRandom(4, 20) / 10f;
				lookAroundDirection = 0f;
				return;
			}
			lookAroundTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (lookAroundTimer <= 0f)
			{
				lookAroundTimer = (float)General.GetNextRandom(4, 20) / 10f;
				if (lookAroundDirection == 1f || lookAroundDirection == 2f)
				{
					lookAroundDirection = 0f;
				}
				else
				{
					lookAroundDirection = General.GetNextRandom(0, 5);
				}
			}
			if (lookAroundDirection == 1f)
			{
				animFrame = 5f;
			}
			else if (lookAroundDirection == 2f)
			{
				animFrame = 6f;
			}
			else
			{
				animFrame = 4f;
			}
		}
		else if (currentState == State.parachuting)
		{
			if (momentum.X > 0f)
			{
				momentum.X += (0f - PARACHUTELEANMOMENTUM) * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (momentum.X < 0f)
			{
				momentum.X += PARACHUTELEANMOMENTUM * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			}
			if (momentum.X > -0.001f && momentum.X < 0.001f)
			{
				momentum.X = 0f;
			}
		}
	}

	public void MoveLeft(GameTime theGameTime)
	{
		if (beamed)
		{
			return;
		}
		if (currentState == State.walking)
		{
			momentum.X = -1f;
			walkingLeft = true;
			if (!isJumping)
			{
				bounceCounter += (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (position.X <= g.theSafeArea.GetPilotXMinLimit())
			{
				bounceCounter = 0f;
				momentum.X = 0f;
			}
			if (bounceCounter < BOUNCEANIMSPEED)
			{
				animFrame = 8f;
			}
			else
			{
				animFrame = 9f;
			}
			if (bounceCounter >= BOUNCEANIMSPEED * 2f)
			{
				bounceCounter = 0f;
			}
		}
		else if (currentState == State.parachuting)
		{
			momentum.X += (0f - PARACHUTELEANMOMENTUM) * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (momentum.X < 0f - PARACHUTETOPLEANSPEED)
			{
				momentum.X = 0f - PARACHUTETOPLEANSPEED;
			}
			if (position.X < g.theSafeArea.GetPilotXMinLimit())
			{
				momentum.X = 0f;
			}
		}
	}

	public void MoveRight(GameTime theGameTime)
	{
		if (beamed)
		{
			return;
		}
		if (currentState == State.walking)
		{
			momentum.X = 1f;
			animFrame = 12f;
			walkingLeft = false;
			if (!isJumping)
			{
				bounceCounter += (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			}
			if (position.X >= g.theSafeArea.GetPilotXMaxLimit())
			{
				bounceCounter = 0f;
				momentum.X = 0f;
			}
			if (bounceCounter < BOUNCEANIMSPEED)
			{
				animFrame = 12f;
			}
			else
			{
				animFrame = 13f;
			}
			if (bounceCounter >= BOUNCEANIMSPEED * 2f)
			{
				bounceCounter = 0f;
			}
		}
		else if (currentState == State.parachuting)
		{
			momentum.X += PARACHUTELEANMOMENTUM * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (momentum.X > PARACHUTETOPLEANSPEED)
			{
				momentum.X = PARACHUTETOPLEANSPEED;
			}
			if (position.X > g.theSafeArea.GetPilotXMaxLimit())
			{
				momentum.X = 0f;
			}
		}
	}

	public void FirePressed(GameTime theGameTime)
	{
		if (beamed || isJumping)
		{
			return;
		}
		if (currentState == State.starting)
		{
			g.GetTower().AddToStartQueue(thePilotNumber);
		}
		if (inPlane && !thePlane.GetInControl() && thePlane.GetActive() && !thePlane.GetControlsLocked())
		{
			BailOut(thePlane.GetPosition());
		}
		if (currentState != State.walking)
		{
			return;
		}
		if (g.GetCurrentLevel() != 2)
		{
			if (position.X > g.GetHangar().GetPositionX() - (float)g.GetHangar().GetDoorWidth() && position.X < g.GetHangar().GetPositionX() + (float)g.GetHangar().GetDoorWidth() && !isJumping && !g.GetHangar().GetBusy())
			{
				currentState = State.enteringHangar;
			}
		}
		else if (position.X > g.GetHangar().GetPositionX() - (float)g.GetHangar().GetDoorWidth() && position.X < g.GetHangar().GetPositionX() + (float)g.GetHangar().GetDoorWidth() - 10f && !isJumping && !g.GetHangar().GetBusy())
		{
			currentState = State.enteringHangar;
		}
		if (position.X > g.GetTower().GetPositionX() - (float)g.GetTower().GetDoorWidth() && position.X < g.GetTower().GetPositionX() + (float)g.GetTower().GetDoorWidth() && g.GetCurrentLevel() != 10 && !isJumping && !g.GetTower().GetBusy())
		{
			if (flown)
			{
				if (currentSquadron.PilotIsAvailable())
				{
					currentState = State.enteringTower;
				}
				else
				{
					g.theNamePlate.SetControllerFlashTime(thePilotNumber, CONTROLLERFLASHTIME);
					theScreenScore.DisplayNoPilotsLeftWarning();
				}
			}
			else
			{
				g.theNamePlate.SetControllerFlashTime(thePilotNumber, CONTROLLERFLASHTIME);
				theScreenScore.DisplayNeedToFlyWarning();
			}
		}
		if (currentState == State.walking && (g.GetCurrentLevel() != 2 || !General.isBetween(position.X, g.GetHangar().GetPositionX() - (float)g.GetHangar().GetDoorWidth() - 1f, g.GetHangar().GetPositionX() + (float)g.GetHangar().GetDoorWidth() - 10f + 1f)))
		{
			g.theSoundManager.JumpSound();
			isJumping = true;
		}
	}

	public void BailOut(Vector2 v)
	{
		position = v;
		currentState = State.freeFalling;
		inPlane = false;
		vulnerable = true;
		animFrame = 0f;
		if (participantAI != null)
		{
			participantAI.PilotMakeADecision(this);
		}
	}

	public void PlaneStrike(GameTime theGameTime)
	{
		currentState = State.parachutingShot;
	}

	public void FreeFalling(GameTime theGameTime, int i)
	{
		if (vulnerable)
		{
			vulnerable = false;
			momentum.X = 0f;
			momentum.Y = 1f;
			pilotSpeedY = FREEFALLSPEED;
			animFrame = 16f;
			animCounter = 0f;
		}
		animCounter += (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		if (position.Y > Level.GetAbsoluteGroundY() && i == 2)
		{
			Drowned(theGameTime);
		}
		if (General.isBetween(position.Y, pilotGroundY, pilotGroundY + 30f) && General.isBetween(position.X, Level.GetLandingMinX(), Level.GetLandingMaxX()))
		{
			currentState = State.hittingTheGround;
			theScreenScore.DisplayScreenName();
			theScreenScore.DisplayScreenScore();
		}
		if (animCounter >= FREEFALLTIMELIMIT)
		{
			vulnerable = true;
			currentState = State.parachuting;
			g.theSoundManager.ParachuteOpenSound();
			ParachuteOpensVibration();
		}
	}

	public void Parachuting(GameTime theGameTime, int i)
	{
		onTheGround = false;
		momentum.Y = 1f;
		pilotSpeedY = PARACHUTINGSPEED;
		animFrame = 0f;
		if (General.isBetween(position.Y, pilotGroundY, pilotGroundY + 30f) && General.isBetween(position.X, Level.GetLandingMinX(), Level.GetLandingMaxX()))
		{
			currentState = State.walking;
			onTheGround = true;
			position.Y = Level.GetPilotGroundY();
			momentum.Y = 0f;
			theScreenScore.DisplayScreenName();
			theScreenScore.DisplayScreenScore();
		}
		if (position.Y > Level.GetAbsoluteGroundY() && i == 2)
		{
			Drowned(theGameTime);
		}
		if (position.X < g.theSafeArea.GetPilotXMinLimit())
		{
			momentum.X = 0.2f;
		}
		if (position.X > g.theSafeArea.GetPilotXMaxLimit())
		{
			momentum.X = -0.2f;
		}
	}

	public void Shot(GameTime theGameTime)
	{
		if (currentState == State.walking)
		{
			currentState = State.walkingShot;
		}
		if (currentState == State.parachuting)
		{
			currentState = State.parachutingShot;
		}
	}

	public void Drowned(GameTime theGameTime)
	{
		vulnerable = false;
		PilotDying(theGameTime);
		PilotKilled(theGameTime);
		splash.TriggerSplash(theGameTime, position, 1f, 0f);
		g.theSoundManager.StopPilotFallingSound(this);
	}

	public void Walking(GameTime theGameTime)
	{
		if (position.X < g.theSafeArea.GetPilotXMinLimit())
		{
			position.X = g.theSafeArea.GetPilotXMinLimit();
		}
		if (position.X > g.theSafeArea.GetPilotXMaxLimit())
		{
			position.X = g.theSafeArea.GetPilotXMaxLimit();
		}
	}

	public void Jumping(GameTime theGameTime, int l)
	{
		jumpCounter += (float)JUMPINGSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		momentum.Y = (float)(0.0 - Math.Sin(jumpCounter));
		if ((double)jumpCounter >= 4.71238911151886)
		{
			jumpCounter = 4.712389f;
		}
		if (l == 2)
		{
			if (General.isBetween(position.X, g.GetHangar().GetPositionX() - (float)g.GetHangar().GetDoorWidth(), g.GetHangar().GetPositionX() + (float)g.GetHangar().GetDoorWidth()))
			{
				if (position.Y > Level.GetCarrierLiftPosition().Y - 28f)
				{
					position.Y = Level.GetCarrierLiftPosition().Y - 32f;
					isJumping = false;
					jumpCounter = 0f;
					momentum.Y = 0f;
				}
			}
			else if (position.Y > pilotGroundY + 4f)
			{
				position.Y = pilotGroundY;
				isJumping = false;
				jumpCounter = 0f;
				momentum.Y = 0f;
			}
		}
		else if (position.Y > pilotGroundY)
		{
			position.Y = pilotGroundY;
			isJumping = false;
			jumpCounter = 0f;
			momentum.Y = 0f;
		}
	}

	public void WalkingShot(GameTime theGameTime)
	{
		if (vulnerable)
		{
			vulnerable = false;
			PilotDying(theGameTime);
			momentum.Y = 0f;
			animFrame = 24f;
			animCounter = 0f;
			flashCount = 0;
			g.theSoundManager.PilotShotSound();
			theScreenScore.ScreenScoreOff();
			theScreenScore.DisplayScreenName();
			GetTheScreenScore().SetTheNameColor(Color.Red);
			if (!isJumping)
			{
				isJumping = true;
			}
			if (momentum.X > 0f)
			{
				walkingLeft = true;
			}
			else
			{
				walkingLeft = false;
			}
		}
		if (animFrame >= 28f)
		{
			animFrame -= 4f;
		}
		if (animFrame < 27f && flashCount == 0)
		{
			animFrame += ANIMSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		if (animFrame >= 27f || flashCount > 0)
		{
			animCounter += FLASHSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		}
		if (animCounter >= 2f)
		{
			if (animFrame == 27f)
			{
				animFrame = 7f;
				flashCount++;
			}
			else
			{
				animFrame = 27f;
			}
			animCounter = 0f;
		}
		if (flashCount == DEADFLASHLIMIT)
		{
			PilotKilled(theGameTime);
		}
		if (!walkingLeft && animFrame != 7f)
		{
			animFrame += 4f;
		}
		if (!(position.Y >= pilotGroundY))
		{
			return;
		}
		if (momentum.X > 0f)
		{
			momentum.X -= 1f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (momentum.X <= 0f)
			{
				momentum.X = 0f;
			}
		}
		if (momentum.X < 0f)
		{
			momentum.X += 1f * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (momentum.X >= 0f)
			{
				momentum.X = 0f;
			}
		}
	}

	public void ParachutingShot(GameTime theGameTime, int i)
	{
		if (vulnerable)
		{
			vulnerable = false;
			momentum.X = 0f;
			momentum.Y = 1f;
			pilotSpeedY = FREEFALLSPEED;
			animFrame = 16f;
			animCounter = 0f;
			g.theSoundManager.StartPilotFallingSound(this);
		}
		animCounter += ANIMSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		if (position.Y < pilotGroundY && !onTheGround)
		{
			if (animCounter < FALLINGANIMSPEED)
			{
				animFrame = 16f;
			}
			else
			{
				animFrame = 17f;
			}
			if (animCounter >= FALLINGANIMSPEED * 2f)
			{
				animCounter = 0f;
			}
			FreeFallingVibration();
		}
		if (position.Y > Level.GetAbsoluteGroundY() && i == 2)
		{
			Drowned(theGameTime);
		}
		if (General.isBetween(position.Y, pilotGroundY, pilotGroundY + 30f) && General.isBetween(position.X, Level.GetLandingMinX(), Level.GetLandingMaxX()))
		{
			currentState = State.hittingTheGround;
			theScreenScore.DisplayScreenName();
			theScreenScore.DisplayScreenScore();
		}
	}

	public void HitTheGround(GameTime theGameTime)
	{
		animCounter += ANIMSPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		if (animFrame == 16f || animFrame == 17f)
		{
			position.Y = pilotGroundY;
			momentum.Y = 0f;
			onTheGround = true;
			pilotSpeedY = PARACHUTINGSPEED;
			animFrame = 18f;
			animCounter = 0f;
			theImpactManager.SetActive(b: true, position + new Vector2(0f, 25f), "GROUND");
			g.theSoundManager.HitGroundSound();
			g.theSoundManager.StopPilotFallingSound(this);
			if (participantPlayer != null)
			{
				HitGroundVibration();
			}
		}
		if ((int)animCounter == 1)
		{
			animFrame = 19f;
		}
		if ((int)animCounter == 2)
		{
			animFrame = 20f;
		}
		if ((int)animCounter == 10)
		{
			animFrame = 21f;
		}
		if ((int)animCounter == 11)
		{
			animFrame = 22f;
		}
		if ((int)animCounter == 12)
		{
			animFrame = 23f;
		}
		if ((int)animCounter >= 13)
		{
			currentState = State.walking;
			pilotSpeedX = INJUREDSPEED;
			vulnerable = true;
		}
	}

	public void DeathPause(GameTime theGameTime)
	{
		deathPauseTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		if (deathPauseTimer <= 0f)
		{
			currentSquadronMember.SetDying(b: false);
			deathPauseTimer = 0f;
			NewPilot();
		}
	}

	public void NewPilot()
	{
		if (currentSquadron.PilotIsAvailable())
		{
			g.GetTower().AddToStartQueue(thePilotNumber);
			currentState = State.requestExitingTower;
			if (participantAI != null)
			{
				participantAI.PilotMakeADecision(this);
			}
		}
		theScreenScore.ScreenScoreOff();
		theScreenScore.ScreenNameOff();
		GetTheScreenScore().SetTheScoreColor(Color.White);
	}

	public void PilotDying(GameTime theGameTime)
	{
		if (!g.theRoundManager.GetRoundIsOver() && g.theRoundManager.SquadronsAreCompeting())
		{
			vulnerable = false;
			currentSquadronMember.SetAlive(b: false);
			currentSquadronMember.SetDying(b: true);
			g.theInGameScreenText.Change();
			FlashControllerIcon(theGameTime);
			if (participantPlayer != null)
			{
				g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(1f, 1f);
			}
		}
	}

	public void PlaneHitVibration()
	{
		if (inPlane && participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.2f, 0.3f);
		}
	}

	public void PlaneStalledVibration()
	{
		if (inPlane && participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.11f, 1f);
		}
	}

	public void FreeFallingVibration()
	{
		if (participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.11f, 0.1f);
		}
	}

	public void PlaneExplodedVibration()
	{
		if (inPlane && participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.5f, 0.3f);
		}
	}

	public void PlaneShootsVibration()
	{
		if (inPlane && participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.3f, 0.1f);
		}
	}

	public void ParachuteOpensVibration()
	{
		if (participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.5f, 0.1f);
		}
	}

	public void HitGroundVibration()
	{
		if (participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(1f, 0.2f);
		}
	}

	public void PlaneHitGroundVibration()
	{
		if (participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(1f, 0.2f);
		}
	}

	public void DoorVibration()
	{
		if (participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.4f, 0.1f);
		}
	}

	public void PlaneBurningVibration()
	{
		if (inPlane && participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.2f, 0.1f);
		}
	}

	public void PlaneTakingOffVibration()
	{
		if (inPlane && participantPlayer != null)
		{
			g.theControllerVibrationManager[participantPlayer.GetCurrentPlayerIndexNumber()].SetVibration(0.105f, 0.1f);
		}
	}

	public void PilotKilled(GameTime theGameTime)
	{
		if (!g.theRoundManager.GetRoundIsOver() && g.theRoundManager.SquadronsAreCompeting())
		{
			currentState = State.deathPause;
			deathPauseTimer = DEATHPAUSETIME;
			g.theRoundManager.CheckSquadronsCompeting(theGameTime);
			currentSquadronMember.SetAlive(b: false);
			g.theInGameScreenText.Change();
		}
		if (g.theInGameScreenText.CheckIfSuddenDeath() && g.theRoundManager.CheckIfRoundWinner())
		{
			g.RoundOver();
		}
	}

	public void PilotTaken(GameTime theGameTime)
	{
		animFrame = 7f;
		if (!g.theRoundManager.GetRoundIsOver())
		{
			currentState = State.deathPause;
			deathPauseTimer = DEATHPAUSETIME;
			g.theRoundManager.CheckSquadronsCompeting(theGameTime);
			currentSquadronMember.SetAlive(b: false);
			currentSquadronMember.SetTaken(b: true);
			g.theSpecialBonus.SetTargetPilot(null);
			g.theInGameScreenText.Change();
		}
		if (g.theInGameScreenText.CheckIfSuddenDeath() && g.theRoundManager.CheckIfRoundWinner())
		{
			g.RoundOver();
		}
	}

	public void EnterHangar(GameTime theGameTime, int l)
	{
		if (l == 1 || l == 3 || l == 4 || l == 5 || l == 10)
		{
			if (vulnerable)
			{
				vulnerable = false;
				momentum.X = 0f;
				momentum.Y = 0f;
				pilotSpeedX = WALKINGSPEED;
				position = g.GetHangar().GetPosition();
				animFrame = 32f;
				g.GetHangar().SetAnimFrame(animFrame - 32f);
				g.GetHangar().SetBusy(b: true);
				theScreenScore.ScreenScoreOff();
				theScreenScore.ScreenNameOff();
			}
			if (animFrame < 40f)
			{
				animFrame += (float)g.GetHangar().GetDoorOpenSpeed() * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				g.GetHangar().SetAnimFrame(animFrame - 32f);
			}
			if (animFrame >= 40f)
			{
				animFrame = 7f;
				g.GetHangar().SetAnimFrame(0f);
				currentState = State.off;
				inPlane = true;
				flown = true;
				onTheGround = false;
				if (!availablePlanes[0].GetActive())
				{
					thePlane = availablePlanes[0];
				}
				else
				{
					thePlane = availablePlanes[1];
				}
				LaunchPlane(l);
				g.theSoundManager.DoorCloseSound();
				DoorVibration();
			}
		}
		if (l != 2)
		{
			return;
		}
		if (vulnerable)
		{
			Level.SetCarrierLiftActive(b: true);
			momentum.X = 0f;
			momentum.Y = 0f;
			animFrame = 4f;
			pilotSpeedX = WALKINGSPEED;
			vulnerable = false;
			g.GetHangar().SetBusy(b: true);
			theScreenScore.ScreenScoreOff();
			theScreenScore.ScreenNameOff();
		}
		if (Level.GetCarrierLiftGoingUp())
		{
			currentState = State.off;
			inPlane = true;
			flown = true;
			onTheGround = false;
			if (!availablePlanes[0].GetActive())
			{
				thePlane = availablePlanes[0];
			}
			else
			{
				thePlane = availablePlanes[1];
			}
			LaunchPlane(l);
		}
	}

	public void EnterTower(GameTime theGameTime)
	{
		if (vulnerable && !g.GetTower().GetBusy())
		{
			vulnerable = false;
			momentum.X = 0f;
			momentum.Y = 0f;
			position = g.GetTower().GetPosition();
			animFrame = 32f;
			g.GetTower().SetAnimFrame(animFrame - 32f);
			g.GetTower().SetBusy(b: true);
			g.GetTower().SetCurrentPilot(thePilotNumber);
			theScreenScore.ScreenScoreOff();
			theScreenScore.ScreenNameOff();
		}
		if (g.GetTower().GetCurrentPilot() == thePilotNumber)
		{
			if (animFrame < 40f)
			{
				animFrame += (float)g.GetTower().GetDoorOpenSpeed() * (float)theGameTime.ElapsedGameTime.TotalSeconds;
				g.GetTower().SetAnimFrame(animFrame - 32f);
			}
			if (animFrame >= 40f)
			{
				g.GetTower().SetAnimFrame(0f);
				animFrame = 7f;
				gracePeriod = GRACEPERIODTIME;
				currentState = State.switchInTower;
				if (g.GetCurrentLevel() != 5)
				{
					g.theSoundManager.DoorCloseSound();
					DoorVibration();
				}
				else
				{
					g.theSoundManager.ParachuteOpenSound();
					DoorVibration();
				}
			}
		}
		else
		{
			currentState = State.walking;
		}
	}

	public void SwitchInTower(GameTime theGameTime)
	{
		gracePeriod -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (gracePeriod <= 0f)
		{
			gracePeriod = 0f;
			currentState = State.requestExitingTower;
		}
	}

	public void ExitTower(GameTime theGameTime)
	{
		if (g.GetTower().GetCurrentPilot() != thePilotNumber)
		{
			return;
		}
		if (currentState == State.requestExitingTower)
		{
			if (scrambled)
			{
				SelectCurrentSquadronMember();
			}
			scrambled = true;
			vulnerable = false;
			flown = false;
			inPlane = false;
			isJumping = false;
			beamed = false;
			jumpCounter = 0f;
			onTheGround = true;
			animFrame = 40f;
			g.GetTower().SetAnimFrame(animFrame - 40f);
			momentum.X = 0f;
			momentum.Y = 0f;
			pilotSpeedX = WALKINGSPEED;
			pilotSpeedY = PARACHUTINGSPEED;
			position = g.GetTower().GetPosition();
			currentState = State.exitingTower;
			if (participantAI != null)
			{
				participantAI.PilotMakeADecision(this);
			}
			if (currentSquadronMember == null)
			{
				pilotAvailable = false;
				animFrame = 7f;
				position.X = -200f;
				position.Y = -200f;
			}
		}
		if (!pilotAvailable)
		{
			return;
		}
		if (animFrame < 48f)
		{
			animFrame += (float)g.GetTower().GetDoorOpenSpeed() * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			g.GetTower().SetAnimFrame(animFrame - 40f);
		}
		if (animFrame >= 48f)
		{
			g.GetTower().SetAnimFrame(0f);
			animFrame = 4f;
			currentState = State.walking;
			vulnerableTimer = INVULNERABLETIME;
			position.Y = Level.GetPilotGroundY();
			g.GetTower().SetBusy(b: false);
			g.GetTower().SetCurrentPilot(-1);
			if (g.GetCurrentLevel() != 5)
			{
				g.theSoundManager.DoorCloseSound();
				DoorVibration();
			}
			else
			{
				g.theSoundManager.ParachuteOpenSound();
				DoorVibration();
			}
			theScreenScore.DisplayScreenName();
			theScreenScore.DisplayScreenScore();
		}
	}

	public void ExitDropPlane(GameTime theGameTime)
	{
		if (g.GetTower().GetCurrentPilot() == thePilotNumber && currentState == State.requestExitingTower)
		{
			if (scrambled)
			{
				SelectCurrentSquadronMember();
			}
			scrambled = true;
			vulnerable = false;
			flown = false;
			inPlane = false;
			onTheGround = true;
			animFrame = 16f;
			momentum.X = 0f;
			momentum.Y = 1f;
			pilotSpeedX = WALKINGSPEED;
			position = g.GetTower().GetPosition();
			currentState = State.freeFalling;
			vulnerable = true;
			g.GetTower().SetBusy(b: false);
			g.GetTower().SetCurrentPilot(-1);
			if (currentSquadronMember.GetScore() > 5)
			{
				theScreenScore.DisplayScreenScore();
			}
			if (participantAI != null)
			{
				participantAI.PilotMakeADecision(this);
			}
			if (currentSquadronMember == null)
			{
				pilotAvailable = false;
				animFrame = 7f;
				position.X = -200f;
				position.Y = -200f;
			}
		}
	}

	public void LaunchPlane(int l)
	{
		thePlane.TakeOff(l);
	}

	public void CheckPosition(GameTime theGameTime, int l)
	{
		if (currentState == State.freeFalling)
		{
			FreeFalling(theGameTime, l);
		}
		if (currentState == State.parachuting)
		{
			Parachuting(theGameTime, l);
		}
		else if (currentState == State.enteringHangar)
		{
			EnterHangar(theGameTime, l);
		}
		else if (currentState == State.enteringTower)
		{
			EnterTower(theGameTime);
		}
		else if (currentState == State.switchInTower)
		{
			SwitchInTower(theGameTime);
		}
		else if (currentState == State.requestExitingTower)
		{
			if (l != 10)
			{
				ExitTower(theGameTime);
			}
			else
			{
				ExitDropPlane(theGameTime);
			}
		}
		else if (currentState == State.exitingTower)
		{
			if (l != 10)
			{
				ExitTower(theGameTime);
			}
			else
			{
				ExitDropPlane(theGameTime);
			}
		}
		else if (currentState == State.walkingShot)
		{
			WalkingShot(theGameTime);
		}
		else if (currentState == State.parachutingShot)
		{
			ParachutingShot(theGameTime, l);
		}
		else if (currentState == State.hittingTheGround)
		{
			HitTheGround(theGameTime);
		}
		else if (currentState == State.deathPause)
		{
			DeathPause(theGameTime);
		}
		else if (currentState == State.walking)
		{
			Walking(theGameTime);
		}
		else if (currentState == State.off)
		{
			position.X = -500f;
			position.Y = -500f;
		}
		if (currentState != State.parachuting)
		{
			colPosition1 = new Vector2(-12000f, -12000f);
			colPosition2 = position;
		}
		else
		{
			colPosition1 = position + new Vector2(0f, -11f);
			colPosition2 = position + new Vector2(0f, 11f);
		}
		if (beamed)
		{
			animFrame = 4f;
			vulnerable = false;
			momentum.X = 0f;
			momentum.Y = -0.2f;
			if (position.Y < g.theSpecialBonus.GetPosition().Y)
			{
				animFrame = 7f;
				position = new Vector2(30000f, 30000f);
				PilotDying(theGameTime);
				PilotTaken(theGameTime);
			}
		}
		position.X += momentum.X * (float)pilotSpeedX * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		position.Y += momentum.Y * (float)pilotSpeedY * (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
		if (isJumping)
		{
			Jumping(theGameTime, l);
		}
		if (vulnerableTimer > 0f && currentState == State.walking)
		{
			vulnerableTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds * CustomOptions.GetGameSpeed();
			if (vulnerableTimer <= 0f)
			{
				vulnerableTimer = 0f;
				vulnerable = true;
			}
		}
		if (l == 1 || l == 3 || l == 4 || l == 5 || l == 10)
		{
			pilotGroundY = Level.GetPilotGroundY();
		}
		if (l != 2)
		{
			return;
		}
		pilotGroundY = Level.GetPilotGroundY();
		if (!onTheGround)
		{
			return;
		}
		position.X += Level.GetCurrentCarrierPositionOffset().X;
		if (!General.isBetween(position.X, g.GetHangar().GetPositionX() - (float)g.GetHangar().GetDoorWidth(), g.GetHangar().GetPositionX() + (float)g.GetHangar().GetDoorWidth() - 10f))
		{
			if (position.Y > pilotGroundY + 15f)
			{
				if (position.X < g.GetHangar().GetPositionX() - (float)g.GetHangar().GetDoorWidth())
				{
					position.X = g.GetHangar().GetPositionX() - (float)g.GetHangar().GetDoorWidth();
				}
				if (position.X > g.GetHangar().GetPositionX() + (float)g.GetHangar().GetDoorWidth() - 10f)
				{
					position.X = g.GetHangar().GetPositionX() + (float)g.GetHangar().GetDoorWidth() - 10f;
				}
			}
			else if (!isJumping)
			{
				position.Y = pilotGroundY;
			}
		}
		if (General.isBetween(position.X, g.GetHangar().GetPositionX() - (float)g.GetHangar().GetDoorWidth(), g.GetHangar().GetPositionX() + (float)g.GetHangar().GetDoorWidth() - 10f) && !isJumping)
		{
			if (position.Y < Level.GetCarrierLiftPosition().Y - 32f)
			{
				momentum.X = 0f;
				momentum.Y = 1f;
			}
			if (position.Y >= Level.GetCarrierLiftPosition().Y - 32f)
			{
				position.Y = Level.GetCarrierLiftPosition().Y - 32f;
			}
		}
		if (position.X < Level.GetLandingMinX())
		{
			position.X = Level.GetLandingMinX();
		}
		if (position.X > Level.GetLandingMaxX())
		{
			position.X = Level.GetLandingMaxX();
		}
	}

	public void FlashControllerIcon(GameTime theGameTime)
	{
		g.theNamePlate.SetControllerFlashTime(thePilotNumber, CONTROLLERFLASHTIME);
		if (currentState == State.walking)
		{
			theScreenScore.DisplayScreenName();
		}
	}

	public bool IsOff()
	{
		if (currentState == State.off || currentState == State.deathPause)
		{
			return true;
		}
		return false;
	}

	public void PunishPilot(int i)
	{
		GetTheScreenScore().SetTheScoreColor(Color.Red);
		GetCurrentSquadronMember().DecrementScore(i);
		GetTheScreenScore().DisplayScreenScore();
		g.theInGameScreenText.Change();
	}

	public void RewardPilot(int i)
	{
		GetTheScreenScore().SetTheScoreColor(Color.White);
		GetCurrentSquadronMember().IncrementScore(i);
		GetTheScreenScore().DisplayScreenScore();
		g.theInGameScreenText.Change();
	}

	public void SetParticipant(Participant p)
	{
		if (p is Player)
		{
			participantPlayer = (Player)p;
			availablePlanes[0].SetAutoStall(b: false);
			availablePlanes[1].SetAutoStall(b: false);
		}
		if (p is AIPlayer)
		{
			participantAI = (AIPlayer)p;
			availablePlanes[0].SetAutoStall(b: true);
			availablePlanes[1].SetAutoStall(b: true);
		}
	}

	public void SetParticipating(bool b)
	{
		participating = b;
	}

	public void SetStateRequestExitingTower()
	{
		currentState = State.requestExitingTower;
	}

	public Color GetCurrentColor()
	{
		return currentColor;
	}

	public bool GetIsWalking()
	{
		if (currentState == State.walking)
		{
			return true;
		}
		return false;
	}

	public void SetCurrentSquadron(int i, Color c)
	{
		currentSquadron = g.squadrons[i];
		currentColor = c;
	}

	public bool HasFlown()
	{
		return flown;
	}

	public SquadronMember GetCurrentSquadronMember()
	{
		return currentSquadronMember;
	}

	public Participant GetParticipant()
	{
		if (participantPlayer != null)
		{
			return participantPlayer;
		}
		return participantAI;
	}

	public AIPlayer GetParticipantAI()
	{
		return participantAI;
	}

	public bool GetInPlane()
	{
		return inPlane;
	}

	public Plane GetThePlane()
	{
		return thePlane;
	}

	public Plane GetAvailablePlane(int i)
	{
		return availablePlanes[i];
	}

	public void SetMomentumX(float f)
	{
		momentum.X = f;
	}

	public int GetThePilotNumber()
	{
		return thePilotNumber;
	}

	public Squadron GetCurrentSquadron()
	{
		return currentSquadron;
	}

	public Vector2 GetPosition()
	{
		return position;
	}

	public Vector2 GetBodyPosition()
	{
		return colPosition2;
	}

	public Vector2 GetParachutePosition()
	{
		return colPosition1;
	}

	public Vector2 GetMomentum()
	{
		return momentum;
	}

	public void SetBeamed(bool b)
	{
		beamed = b;
		if (!beamed && currentState != State.deathPause)
		{
			currentState = State.parachutingShot;
			vulnerable = true;
		}
	}

	public bool GetVulnerable()
	{
		return vulnerable;
	}

	public ScreenScore GetTheScreenScore()
	{
		return theScreenScore;
	}

	public bool IsOnTheGround()
	{
		return onTheGround;
	}

	public bool IsParticipating()
	{
		return participating;
	}

	public bool IsStarting()
	{
		if (currentState == State.starting)
		{
			return true;
		}
		return false;
	}

	public void SetCurrentSquadronMember(SquadronMember s)
	{
		currentSquadronMember = s;
	}

	public void SelectCurrentSquadronMember()
	{
		currentSquadronMember = currentSquadron.SelectOnDuty(currentSquadronMember);
	}
}
