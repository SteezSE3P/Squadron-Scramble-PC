using System;
using Microsoft.Xna.Framework;

namespace SquadronScramble;

public class AIPlayer : Participant
{
	private enum PilotMotive
	{
		daydreaming,
		wantingToFly,
		wantingToBank
	}

	private enum PlaneMotive
	{
		daydreaming,
		cruising,
		attackingPlane,
		attackingBonus
	}

	private GameWorld g;

	private Pilot pilotTarget;

	private Squadron[] theSquadrons = new Squadron[4];

	private Pilot[] squadronPilots = new Pilot[4];

	private PilotMotive currentPilotMotive = PilotMotive.wantingToFly;

	private float thoughtCounter = 0f;

	private float targetAltitude = 0f;

	private float bailReactionTime = 0f;

	private float triggerFingerTime = 0f;

	private bool shotTaken = false;

	private bool flyRight = true;

	private bool gotTarget = false;

	private float TRIGGERLIMIT = 0.15f;

	private float DIVEABORTALTITUDE = 0.5f;

	private int maxTargetAltitude = 0;

	private int minTargetAltitude = 0;

	private Vector2 targetPosition = new Vector2(0f, 0f);

	private float aimAdjust = 0f;

	private PlaneMotive currentPlaneMotive = PlaneMotive.cruising;

	private float startTimer = 0f;

	private float startReactionTime = 0f;

	private float shuffleTimer = 0f;

	private float shuffleRandom = 0f;

	private float SHUFFLEMINDISTANCE = 100f;

	public AIPlayer(GameWorld gw)
	{
		g = gw;
	}

	public void Update(GameTime theGameTime, Pilot p)
	{
		if (GetParticipating())
		{
			SetTargetPosition(theGameTime, p);
			PilotControl(theGameTime, p);
			PlaneControl(theGameTime, p);
			UpdateTriggerFinger(theGameTime, p);
		}
	}

	public void UpdateTriggerFinger(GameTime theGameTime, Pilot p)
	{
		if (triggerFingerTime > 0f)
		{
			triggerFingerTime -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
			shotTaken = true;
		}
		if (triggerFingerTime <= 0f)
		{
			triggerFingerTime = 0f;
			shotTaken = false;
		}
	}

	public void PilotControl(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if (p.IsStarting() && g.theRoundManager.GetRoundIsPlayable())
		{
			if (startReactionTime == 0f)
			{
				startReactionTime = (float)General.GetNextRandom(1, 6) / 10f;
			}
			startTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (startTimer > startReactionTime)
			{
				p.FirePressed(theGameTime);
				startReactionTime = 0f;
				startTimer = 0f;
			}
		}
		if (!thePlane.GetInControl() && p.GetInPlane())
		{
			PilotBailOut(theGameTime, p);
		}
		if (currentPilotMotive == PilotMotive.wantingToFly)
		{
			if (p.GetPosition().X < g.GetHangar().GetPositionX() && shuffleTimer == 0f)
			{
				p.MoveRight(theGameTime);
			}
			else if (p.GetPosition().X > g.GetHangar().GetPositionX() && shuffleTimer == 0f)
			{
				p.MoveLeft(theGameTime);
			}
			if (General.CheckDistanceX(p.GetPosition().X, g.GetHangar().GetPositionX()) < 10f && !g.GetHangar().GetBusy() && shuffleTimer == 0f)
			{
				p.FirePressed(theGameTime);
			}
			if (shuffleTimer > 0f)
			{
				PilotShuffle(theGameTime, p);
			}
			if (General.CheckDistanceX(p.GetPosition().X, g.GetHangar().GetPositionX()) < SHUFFLEMINDISTANCE && g.GetHangar().GetBusy() && shuffleTimer == 0f)
			{
				ActivateShuffle();
			}
		}
		if (currentPilotMotive == PilotMotive.wantingToBank)
		{
			if (p.GetPosition().X < g.GetTower().GetPositionX())
			{
				p.MoveRight(theGameTime);
			}
			else if (p.GetPosition().X > g.GetTower().GetPositionX())
			{
				p.MoveLeft(theGameTime);
			}
			if (General.CheckDistanceX(p.GetPosition().X, g.GetTower().GetPosition().X) < 10f && !g.GetTower().GetBusy() && shuffleTimer == 0f)
			{
				p.FirePressed(theGameTime);
				PilotMakeADecision(p);
			}
			if (shuffleTimer > 0f)
			{
				PilotShuffle(theGameTime, p);
			}
			if (General.CheckDistanceX(p.GetPosition().X, g.GetTower().GetPosition().X) < SHUFFLEMINDISTANCE && g.GetTower().GetBusy() && shuffleTimer == 0f)
			{
				ActivateShuffle();
			}
		}
	}

	public void PilotShuffle(GameTime theGameTime, Pilot p)
	{
		shuffleTimer -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (shuffleTimer < 0f)
		{
			shuffleTimer = 0f;
		}
		if (shuffleTimer <= 0f)
		{
			if (currentPilotMotive == PilotMotive.wantingToBank && g.GetTower().GetBusy())
			{
				ActivateShuffle();
			}
			else if (currentPilotMotive == PilotMotive.wantingToFly && g.GetHangar().GetBusy())
			{
				ActivateShuffle();
			}
		}
		if (shuffleRandom == 0f || shuffleRandom == 1f)
		{
			p.MoveLeft(theGameTime);
		}
		else if (shuffleRandom == 2f || shuffleRandom == 3f)
		{
			p.MoveRight(theGameTime);
		}
		else
		{
			p.NoLean(theGameTime);
		}
	}

	public void PilotBailOut(GameTime theGameTime, Pilot p)
	{
		if (bailReactionTime == 0f)
		{
			bailReactionTime = (float)General.GetNextRandom(2, 7) / 10f;
		}
		bailReactionTime -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (bailReactionTime < 0f)
		{
			p.FirePressed(theGameTime);
			bailReactionTime = 0f;
		}
	}

	public void PilotMakeADecision(Pilot p)
	{
		if (!p.HasFlown())
		{
			currentPilotMotive = PilotMotive.wantingToFly;
		}
		if (!p.HasFlown())
		{
			return;
		}
		if (p.GetCurrentSquadron().LowerScorePilotIsAvailable(p))
		{
			if (General.GetNextRandom(0, 3) == 0)
			{
				currentPilotMotive = PilotMotive.wantingToFly;
			}
			else
			{
				currentPilotMotive = PilotMotive.wantingToBank;
			}
		}
		else
		{
			currentPilotMotive = PilotMotive.wantingToFly;
		}
		if (g.GetCurrentLevel() == 10)
		{
			currentPilotMotive = PilotMotive.wantingToFly;
		}
	}

	public void PlaneControl(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if (!thePlane.GetInControl())
		{
			return;
		}
		thoughtCounter -= (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (thePlane.GetPosition().Y > Level.GetGroundY() - DIVEABORTALTITUDE * thePlane.GetEngineSpeed() && currentPlaneMotive != PlaneMotive.cruising)
		{
			if (thePlane.GetDirection() > 0f && thePlane.GetDirection() < (float)Math.PI)
			{
				thoughtCounter = 0f;
			}
			else if (currentPlaneMotive == PlaneMotive.cruising)
			{
				currentPlaneMotive = PlaneMotive.attackingPlane;
			}
		}
		if (thoughtCounter <= 0f)
		{
			PlaneMakeADecision(theGameTime, p);
		}
		if (currentPlaneMotive == PlaneMotive.daydreaming)
		{
			thePlane.NoTurn(theGameTime);
		}
		if (currentPlaneMotive == PlaneMotive.attackingPlane)
		{
			if (!gotTarget)
			{
				PlaneFindPilotTarget(theGameTime, p);
			}
			if (gotTarget)
			{
				PlaneAimAtTarget(theGameTime, p);
				PlaneShootAtTarget(theGameTime, p);
			}
		}
		if (currentPlaneMotive == PlaneMotive.cruising)
		{
			PlaneGetToAltitude(theGameTime, p);
			if (gotTarget)
			{
				PlaneShootAtTarget(theGameTime, p);
			}
		}
		if (currentPlaneMotive == PlaneMotive.attackingBonus)
		{
			PlaneAimAtBonus(theGameTime, p);
			PlaneShootAtBonus(theGameTime, p);
		}
	}

	public void PlaneMakeADecision(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		int nextRandom = General.GetNextRandom(0, 50);
		if (thePlane.GetPosition().Y > Level.GetGroundY() - DIVEABORTALTITUDE * thePlane.GetEngineSpeed() && currentPlaneMotive != PlaneMotive.cruising)
		{
			nextRandom = General.GetNextRandom(0, 2);
		}
		switch (nextRandom)
		{
		case 0:
			minTargetAltitude = (int)Level.GetGroundY() - 320;
			maxTargetAltitude = (int)Level.GetGroundY() - 300;
			currentPlaneMotive = PlaneMotive.cruising;
			targetAltitude = General.GetNextRandom(minTargetAltitude, maxTargetAltitude);
			flyRight = false;
			break;
		case 1:
			minTargetAltitude = (int)Level.GetGroundY() - 320;
			maxTargetAltitude = (int)Level.GetGroundY() - 300;
			currentPlaneMotive = PlaneMotive.cruising;
			targetAltitude = General.GetNextRandom(minTargetAltitude, maxTargetAltitude);
			flyRight = true;
			break;
		default:
			currentPlaneMotive = PlaneMotive.attackingPlane;
			gotTarget = false;
			break;
		}
		thoughtCounter = General.GetNextRandom(1, 4);
		if (g.theSpecialBonus.GetActive() && g.theSpecialBonus.PresenceIsKnown() && nextRandom > 1)
		{
			currentPlaneMotive = PlaneMotive.attackingBonus;
		}
	}

	public void PlaneFindPilotTarget(GameTime theGameTime, Pilot p)
	{
		int nextRandom = General.GetNextRandom(0, 50);
		if (nextRandom < 25)
		{
			IdentifyPilotInTopScoringSquadron(theGameTime, p);
		}
		else if (nextRandom < 40)
		{
			IdentifyTopScoringPilot(theGameTime, p);
		}
		else
		{
			IdentifyPilotInRandomSquadron(theGameTime, p);
		}
		AdjustAim();
		if (pilotTarget != null && pilotTarget.GetThePlane() != null)
		{
			if (!p.GetThePlane().GetVulnerable() && (pilotTarget.GetPosition().Y >= p.GetThePlane().GetPosition().Y || pilotTarget.GetThePlane().GetPosition().Y >= p.GetThePlane().GetPosition().Y))
			{
				currentPlaneMotive = PlaneMotive.cruising;
			}
		}
		else
		{
			currentPlaneMotive = PlaneMotive.cruising;
		}
	}

	public void IdentifyPilotInRandomSquadron(GameTime theGameTime, Pilot p)
	{
		int num = -1;
		pilotTarget = null;
		for (int i = 0; i < g.squadrons.Length; i++)
		{
			if (g.squadrons[i].SquadronIsInService() && g.squadrons[i] != p.GetCurrentSquadron())
			{
				num++;
				theSquadrons[num] = g.squadrons[i];
			}
		}
		if (num != -1)
		{
			IdentifyRandomPilotInSquadron(theGameTime, theSquadrons[General.GetNextRandom(0, num)]);
		}
		if (pilotTarget == null)
		{
			thoughtCounter = 0f;
		}
	}

	public void IdentifyPilotInTopScoringSquadron(GameTime theGameTime, Pilot p)
	{
		int num = -1;
		Squadron squadron = null;
		pilotTarget = null;
		for (int i = 0; i < g.squadrons.Length; i++)
		{
			if (g.squadrons[i].SquadronIsInService() && g.squadrons[i] != p.GetCurrentSquadron() && g.squadrons[i].GetScore() > num)
			{
				squadron = g.squadrons[i];
				num = squadron.GetScore();
				IdentifyRandomPilotInSquadron(theGameTime, squadron);
			}
		}
		if (pilotTarget == null)
		{
			thoughtCounter = 0f;
		}
	}

	public void IdentifyTopPilotInSquadron(GameTime theGameTime, Squadron s)
	{
		int num = -1;
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (g.pilots[i].IsParticipating() && g.pilots[i].GetCurrentSquadron() == s && g.pilots[i].GetCurrentSquadronMember().GetScore() > num && g.pilots[i].GetCurrentSquadronMember().GetAlive())
			{
				pilotTarget = g.pilots[i];
				num = pilotTarget.GetCurrentSquadronMember().GetScore();
				gotTarget = true;
			}
		}
	}

	public void IdentifyRandomPilotInSquadron(GameTime theGameTime, Squadron s)
	{
		int num = -1;
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (g.pilots[i].IsParticipating() && g.pilots[i].GetCurrentSquadron() == s && g.pilots[i].GetCurrentSquadronMember().GetAlive())
			{
				num++;
				squadronPilots[num] = g.pilots[i];
				gotTarget = true;
			}
		}
		if (num != -1)
		{
			pilotTarget = squadronPilots[General.GetNextRandom(0, num + 1)];
		}
	}

	public void IdentifyTopScoringPilot(GameTime theGameTime, Pilot p)
	{
		int num = -1;
		pilotTarget = null;
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (g.pilots[i].IsParticipating() && g.pilots[i] != p && g.pilots[i].GetCurrentSquadron() != p.GetCurrentSquadron() && g.pilots[i].GetCurrentSquadronMember().GetScore() > num && g.pilots[i].GetCurrentSquadronMember().GetAlive())
			{
				pilotTarget = g.pilots[i];
				num = pilotTarget.GetCurrentSquadronMember().GetScore();
				gotTarget = true;
			}
		}
		if (pilotTarget == null)
		{
			thoughtCounter = 0f;
		}
	}

	public void PlaneGetToAltitude(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if (targetAltitude + 100f < thePlane.GetPosition().Y)
		{
			PlaneGainAltitude(theGameTime, p);
		}
		else
		{
			PlaneFlyLevel(theGameTime, p);
		}
	}

	public void PlaneFlyLevel(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if (flyRight)
		{
			if (thePlane.GetDirection() < (float)Math.PI)
			{
				thePlane.AnalogueLeft((float)(0.0 - General.RelativeAgnosticNormalisedAngle(thePlane.GetPosition(), new Vector2(thePlane.GetPosition().X + 10f, targetAltitude), thePlane.GetDirection(), 6.2831854820251465)), theGameTime);
			}
			if (thePlane.GetDirection() >= (float)Math.PI)
			{
				thePlane.AnalogueRight((float)General.RelativeAgnosticNormalisedAngle(thePlane.GetPosition(), new Vector2(thePlane.GetPosition().X + 10f, targetAltitude), thePlane.GetDirection(), 6.2831854820251465), theGameTime);
			}
		}
		if (!flyRight)
		{
			if (thePlane.GetDirection() > (float)Math.PI)
			{
				thePlane.AnalogueLeft((float)(0.0 - General.RelativeAgnosticNormalisedAngle(thePlane.GetPosition(), new Vector2(thePlane.GetPosition().X - 10f, thePlane.GetPosition().Y), thePlane.GetDirection(), 0.7853981852531433)), theGameTime);
			}
			if (thePlane.GetDirection() <= (float)Math.PI)
			{
				thePlane.AnalogueRight((float)General.RelativeAgnosticNormalisedAngle(thePlane.GetPosition(), new Vector2(thePlane.GetPosition().X - 10f, thePlane.GetPosition().Y), thePlane.GetDirection(), 0.7853981852531433), theGameTime);
			}
		}
	}

	public void PlaneGainAltitude(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if ((double)thePlane.GetDirection() > 1.5707963705062866 && (double)thePlane.GetDirection() <= 3.298672378063202)
		{
			thePlane.TurnRight(theGameTime);
		}
		else if ((double)thePlane.GetDirection() <= 1.5707963705062866 || (double)thePlane.GetDirection() > 6.126105844974518)
		{
			thePlane.TurnLeft(theGameTime);
		}
		else
		{
			thePlane.NoTurn(theGameTime);
		}
	}

	public void PlaneAimAtTarget(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if (!General.IsDirectionSideClockwise(thePlane.GetPosition(), targetPosition, thePlane.GetDirection(), 3.1415927410125732))
		{
			thePlane.AnalogueLeft((float)(0.0 - General.RelativeAgnosticNormalisedAngle(thePlane.GetPosition(), targetPosition, thePlane.GetDirection(), 1.5707963705062866)), theGameTime);
		}
		if (General.IsDirectionSideClockwise(thePlane.GetPosition(), targetPosition, thePlane.GetDirection(), 3.1415927410125732))
		{
			thePlane.AnalogueRight((float)General.RelativeAgnosticNormalisedAngle(thePlane.GetPosition(), targetPosition, thePlane.GetDirection(), 1.5707963705062866), theGameTime);
		}
	}

	public void PlaneShootAtTarget(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		float num = 500f;
		if (General.IsInDirectionRange(thePlane.GetPosition(), targetPosition, thePlane.GetDirection(), 0.06) && General.CheckDistance(thePlane.GetPosition(), targetPosition) <= num && !shotTaken)
		{
			triggerFingerTime = TRIGGERLIMIT;
			thePlane.Shoot(theGameTime);
		}
	}

	public void AdjustAim()
	{
		aimAdjust = 1f - (float)General.GetNextRandom(0, 3) / 10f;
	}

	public float GetAimAdjust()
	{
		return aimAdjust;
	}

	public void SetTargetPosition(GameTime theGameTime, Pilot p)
	{
		if (pilotTarget != null)
		{
			if (pilotTarget.GetThePlane().GetInControl())
			{
				targetPosition = pilotTarget.GetThePlane().GetPosition() + aimAdjust * (pilotTarget.GetThePlane().GetMomentum() * (pilotTarget.GetThePlane().GetEngineSpeed() / 2000f * (General.CheckDistance(p.GetThePlane().GetPosition(), pilotTarget.GetThePlane().GetPosition()) * 3.5f)));
			}
			else
			{
				targetPosition = pilotTarget.GetPosition();
			}
		}
	}

	public Vector2 GetTargetPosition()
	{
		return targetPosition;
	}

	public void PlaneAimAtBonus(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if (!General.IsDirectionSideClockwise(thePlane.GetPosition(), GetBonusPosition(theGameTime, p), thePlane.GetDirection(), 3.1415927410125732))
		{
			thePlane.AnalogueLeft((float)(0.0 - General.RelativeAgnosticNormalisedAngle(thePlane.GetPosition(), GetBonusPosition(theGameTime, p), thePlane.GetDirection(), 1.5707963705062866)), theGameTime);
		}
		if (General.IsDirectionSideClockwise(thePlane.GetPosition(), GetBonusPosition(theGameTime, p), thePlane.GetDirection(), 3.1415927410125732))
		{
			thePlane.AnalogueRight((float)General.RelativeAgnosticNormalisedAngle(thePlane.GetPosition(), GetBonusPosition(theGameTime, p), thePlane.GetDirection(), 1.5707963705062866), theGameTime);
		}
		if (!g.theSpecialBonus.GetActive())
		{
			thoughtCounter = 0f;
		}
	}

	public void PlaneShootAtBonus(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if (General.IsInDirectionRange(thePlane.GetPosition(), GetBonusPosition(theGameTime, p), thePlane.GetDirection(), 0.05) && !shotTaken)
		{
			triggerFingerTime = TRIGGERLIMIT;
			thePlane.Shoot(theGameTime);
		}
	}

	public Vector2 GetBonusPosition(GameTime theGameTime, Pilot p)
	{
		Vector2 vector = new Vector2(0f, 0f);
		return g.theSpecialBonus.GetPosition();
	}

	public void ActivateShuffle()
	{
		if (shuffleTimer == 0f)
		{
			shuffleRandom = General.GetNextRandom(0, 6);
			shuffleTimer = (float)General.GetNextRandom(1, 6) / 10f;
		}
	}

	public void ActivateShuffleEvade()
	{
		if (shuffleTimer == 0f)
		{
			shuffleRandom = General.GetNextRandom(0, 5);
			shuffleTimer = (float)General.GetNextRandom(3, 9) / 10f;
		}
	}

	public void Reset()
	{
		shuffleTimer = 0f;
	}
}
