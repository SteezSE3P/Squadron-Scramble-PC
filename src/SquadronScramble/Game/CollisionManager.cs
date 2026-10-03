using Microsoft.Xna.Framework;

namespace SquadronScramble;

public class CollisionManager
{
	private GameWorld g;

	private Pilot[] pilots;

	private Plane[] planes;

	private Cloud[] clouds;

	private Cloud[] heavyCloudsFront;

	private Cloud[] heavyCloudsBack;

	private bool groundExplosionsKill = true;

	public CollisionManager(GameWorld gw)
	{
		g = gw;
		pilots = g.pilots;
		planes = g.planes;
		clouds = Level.GetClouds();
		heavyCloudsFront = Level.GetHeavyCloudsFront();
		heavyCloudsBack = Level.GetHeavyCloudsBack();
	}

	public void Update(GameTime theGameTime)
	{
		CheckBulletPlaneCollisions(theGameTime);
		CheckBulletPilotCollision(theGameTime);
		CheckBulletSpecialsCollision(theGameTime);
		CheckPlanePlaneCollision(theGameTime);
		CheckSpecialBulletPlaneCollisions(theGameTime);
		CheckSpecialMissilePlaneCollisions(theGameTime);
		CheckSpecialBulletPilotCollision(theGameTime);
		CheckSpecialBombPilotCollision(theGameTime);
		CheckPlaneCrashPilotCollision(theGameTime);
		CheckPlaneCloudCollision(theGameTime);
		CheckWingPilotCollision(theGameTime);
		CheckWingPlaneCollision(theGameTime);
	}

	public void CheckBulletPlaneCollisions(GameTime theGameTime)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < planes.Length; j++)
			{
				if (planes[j] == null || planes[j] == planes[i])
				{
					continue;
				}
				for (int k = 0; k < CustomOptions.GetBulletLimit(); k++)
				{
					if (!planes[i].CheckCollisionBoxes(planes[j].GetBullet(k).GetPosition(), planes[i].GetBulletPlaneCollisionSize()) || (!CustomOptions.GetFriendlyFireEnabled() && planes[j].GetThePilot().GetCurrentSquadron() == planes[i].GetThePilot().GetCurrentSquadron()) || planes[i].GetTakingOff())
					{
						continue;
					}
					planes[i].Shot(theGameTime);
					planes[i].theImpactManager.SetActive(b: true, planes[j].GetBullet(k).GetPosition(), "SHOT");
					planes[j].GetBullet(k).BulletOff();
					if (planes[i].GetVulnerable() && !planes[i].GetInControl() && !planes[i].GetSecondStrike())
					{
						if (planes[j].GetThePilot().GetCurrentSquadron() != planes[i].GetThePilot().GetCurrentSquadron())
						{
							planes[j].GetThePilot().RewardPilot(1);
							g.theSoundManager.PointScoredSound();
						}
						else
						{
							planes[j].GetThePilot().PunishPilot(1);
						}
					}
				}
			}
		}
	}

	public void CheckBulletPilotCollision(GameTime theGameTime)
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			if (pilots[i] == null)
			{
				continue;
			}
			for (int j = 0; j < planes.Length; j++)
			{
				if (planes[j] == null || planes[j] == pilots[i].GetThePlane())
				{
					continue;
				}
				for (int k = 0; k < CustomOptions.GetBulletLimit(); k++)
				{
					if (pilots[i].CheckCollisionBoxes(planes[j].GetBullet(k).GetPosition(), planes[i].GetBulletPilotDodgeSize()) && (CustomOptions.GetFriendlyFireEnabled() || planes[j].GetThePilot().GetCurrentSquadron() != planes[i].GetThePilot().GetCurrentSquadron()) && pilots[i].GetParticipantAI() != null)
					{
						pilots[i].GetParticipantAI().ActivateShuffleEvade();
						if (planes[j].GetBullet(k).GetMomentum().Y < 0.3f && planes[j].GetBullet(k).GetMomentum().Y > -0.3f && planes[j].GetBullet(k).GetPosition().Y > Level.GetPilotGroundY() - 20f && planes[j].GetBullet(k).GetPosition().Y < Level.GetPilotGroundY() + 20f)
						{
							pilots[i].FirePressed(theGameTime);
						}
					}
					if ((General.CheckDistance(pilots[i].GetBodyPosition(), planes[j].GetBullet(k).GetPosition()) < planes[i].GetBulletPilotCollisionSize() || General.CheckDistance(pilots[i].GetParachutePosition(), planes[j].GetBullet(k).GetPosition()) < planes[i].GetBulletParachuteCollisionSize()) && (CustomOptions.GetFriendlyFireEnabled() || planes[j].GetThePilot().GetCurrentSquadron() != planes[i].GetThePilot().GetCurrentSquadron()) && pilots[i].GetVulnerable())
					{
						pilots[i].Shot(theGameTime);
						pilots[i].SetMomentumX(planes[j].GetBullet(k).GetMomentum().X * 0.5f);
						pilots[i].theImpactManager.SetActive(b: true, planes[j].GetBullet(k).GetPosition(), "SHOT");
						planes[j].GetBullet(k).BulletOff();
					}
				}
			}
		}
	}

	public void CheckBulletSpecialsCollision(GameTime theGameTime)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < CustomOptions.GetBulletLimit(); j++)
			{
				if ((General.CheckDistance(g.theSpecialBonus.GetPosition() + g.theSpecialBonus.GetCollisionOffset(0), planes[i].GetBullet(j).GetPosition()) < g.theSpecialBonus.GetBulletCollisionSize() || General.CheckDistance(g.theSpecialBonus.GetPosition() + g.theSpecialBonus.GetCollisionOffset(1), planes[i].GetBullet(j).GetPosition()) < g.theSpecialBonus.GetBulletCollisionSize()) && g.theSpecialBonus.GetActive() && g.theSpecialBonus.GetVulnerable())
				{
					g.theSpecialBonus.Shot(theGameTime, planes[i].GetThePilot().GetCurrentSquadron().GetTheColor());
					planes[i].GetThePilot().RewardPilot(3);
					g.theSoundManager.BonusPointSound();
					planes[i].GetBullet(j).BulletOff();
				}
			}
		}
	}

	public void CheckPlanePlaneCollision(GameTime theGameTime)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < planes.Length; j++)
			{
				if (planes[j] != null && planes[i] != planes[j] && General.CheckDistance(planes[i].GetPosition(), planes[j].GetPosition()) < planes[i].GetPlanePlaneCollisionSize() && (planes[i].GetCollidable() || planes[j].GetCollidable()) && planes[i].GetActive() && planes[j].GetActive() && planes[i].GetVulnerable() && planes[j].GetVulnerable())
				{
					planes[i].Explode(theGameTime);
					planes[j].Explode(theGameTime);
				}
			}
		}
	}

	public void CheckPlanePilotCollision(GameTime theGameTime)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < pilots.Length; j++)
			{
				if (pilots[j] != null && pilots[j] != planes[i].GetThePilot() && General.CheckDistance(planes[i].GetPosition(), pilots[j].GetPosition()) < planes[i].GetPlanePlaneCollisionSize() && !planes[i].GetTakingOff() && planes[i].GetVulnerable() && planes[i].GetInControl() && pilots[j].GetIsWalking())
				{
					planes[i].PilotStrike(theGameTime);
					planes[i].theImpactManager.SetActive(b: true, pilots[j].GetPosition(), "SHOT");
					pilots[j].PlaneStrike(theGameTime);
				}
			}
		}
	}

	public void CheckSpecialBulletPlaneCollisions(GameTime theGameTime)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < g.theSpecialBonus.GetBulletLimit(); j++)
			{
				if (General.CheckDistance(planes[i].GetPosition(), g.theSpecialBonus.GetBullet(j).GetPosition()) < planes[i].GetBulletPlaneCollisionSize() && planes[i].GetVulnerable())
				{
					planes[i].Shot(theGameTime);
					planes[i].theImpactManager.SetActive(b: true, g.theSpecialBonus.GetBullet(j).GetPosition(), "SHOT");
					g.theSpecialBonus.GetBullet(j).BulletOff();
				}
			}
		}
	}

	public void CheckSpecialMissilePlaneCollisions(GameTime theGameTime)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < g.theSpecialBonus.GetMissileLimit(); j++)
			{
				if (planes[i].CheckCollisionBoxes(g.theSpecialBonus.GetMissile(j).GetPosition(), planes[i].GetMissilePlaneCollisionSize()) && planes[i].GetVulnerable())
				{
					planes[i].Shot(theGameTime);
					g.theSpecialBonus.GetMissile(j).GetTheExplosion().TriggerExplosion(theGameTime, g.theSpecialBonus.GetPosition(), 0.35f);
					g.theSpecialBonus.GetMissile(j).MissileOff();
				}
			}
		}
	}

	public void CheckSpecialBulletPilotCollision(GameTime theGameTime)
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			if (pilots[i] == null)
			{
				continue;
			}
			for (int j = 0; j < g.theSpecialBonus.GetBulletLimit(); j++)
			{
				if (General.CheckDistance(pilots[i].GetPosition(), g.theSpecialBonus.GetBullet(j).GetPosition()) < planes[i].GetBulletPilotCollisionSize() && g.theSpecialBonus.GetPilotBulletDeadly(g.theSpecialBonus.GetBullet(j)) && pilots[i].GetVulnerable())
				{
					pilots[i].Shot(theGameTime);
					pilots[i].SetMomentumX(g.theSpecialBonus.GetBullet(j).GetMomentum().X * 0.5f);
					g.theSpecialBonus.GetBullet(j).BulletOff();
				}
			}
		}
	}

	public void CheckSpecialBombPilotCollision(GameTime theGameTime)
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			if (pilots[i] == null)
			{
				continue;
			}
			for (int j = 0; j < g.theSpecialBonus.GetBombLimit(); j++)
			{
				if (General.CheckDistance(pilots[i].GetPosition(), g.theSpecialBonus.GetBomb(j).GetTheGroundExplosion().GetPosition()) < planes[i].GetBombPilotCollisionSize() && pilots[i].GetVulnerable())
				{
					pilots[i].Shot(theGameTime);
					pilots[i].SetMomentumX(g.theSpecialBonus.GetBomb(j).GetMomentum().X);
				}
			}
		}
	}

	public void CheckPlaneCrashPilotCollision(GameTime theGameTime)
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			if (pilots[i] == null)
			{
				continue;
			}
			for (int j = 0; j < planes.Length; j++)
			{
				if (planes[j] != null && General.CheckDistanceX(pilots[i].GetPosition().X, planes[j].theGroundCrash.GetPosition().X) < planes[j].theGroundCrash.GetExplosionRadius() && planes[j].theGroundCrash.GetIsNew() && groundExplosionsKill && pilots[i].GetVulnerable() && pilots[i].GetIsWalking() && g.theRoundManager.SquadronsAreCompeting())
				{
					pilots[i].Shot(theGameTime);
					pilots[i].SetMomentumX((pilots[i].GetPosition().X - planes[j].theGroundCrash.GetPosition().X) * 0.05f);
				}
			}
		}
	}

	public void CheckPlaneCloudCollision(GameTime theGameTime)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < clouds.Length; j++)
			{
				if (clouds[j] != null && clouds[j].CheckCollisionBoxes(planes[i].GetExhaustPosition()))
				{
					planes[i].InCloud();
				}
			}
			for (int j = 0; j < heavyCloudsFront.Length; j++)
			{
				if (heavyCloudsFront[j] != null && heavyCloudsFront[j].CheckCollisionBoxes(planes[i].GetExhaustPosition()))
				{
					planes[i].InCloud();
				}
			}
			for (int j = 0; j < heavyCloudsBack.Length; j++)
			{
				if (heavyCloudsBack[j] != null && heavyCloudsBack[j].CheckCollisionBoxes(planes[i].GetExhaustPosition()))
				{
					planes[i].InCloud();
				}
			}
		}
	}

	public void CheckWingPilotCollision(GameTime theGameTime)
	{
		for (int i = 0; i < pilots.Length; i++)
		{
			if (pilots[i] == null)
			{
				continue;
			}
			for (int j = 0; j < planes.Length; j++)
			{
				if (planes[j] != null && planes[j] != pilots[i].GetThePlane() && pilots[i].CheckCollisionBoxes(planes[j].GetTheWing().GetPosition(), planes[i].GetBulletPilotCollisionSize()) && pilots[i].GetVulnerable())
				{
					pilots[i].Shot(theGameTime);
					pilots[i].SetMomentumX(planes[j].GetTheWing().GetMomentum().X * 0.5f);
					pilots[i].theImpactManager.SetActive(b: true, planes[j].GetTheWing().GetPosition(), "SHOT");
					planes[j].GetTheWing().WingOff();
				}
			}
		}
	}

	public void CheckWingPlaneCollision(GameTime theGameTime)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < planes.Length; j++)
			{
				if (planes[j] != null && planes[j] != planes[i] && planes[i].CheckCollisionBoxes(planes[j].GetTheWing().GetPosition(), planes[i].GetWingPlaneCollisionSize()) && planes[j].GetThePilot().GetCurrentSquadron() != planes[i].GetThePilot().GetCurrentSquadron() && !planes[i].GetTakingOff())
				{
					planes[i].SmokeDamage(theGameTime);
					planes[i].theImpactManager.SetActive(b: true, planes[j].GetTheWing().GetPosition(), "SHOT");
					planes[j].GetTheWing().WingOff();
				}
			}
		}
	}
}
