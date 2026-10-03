using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SquadronScramble;

public class SoundManager
{
	private enum State
	{
		normal,
		settingMusic
	}

	private GameWorld g;

	private float volumeSFX = 0.8f;

	private float volumeMusic = 0.8f;

	private float provVolume = 0f;

	private float musicFade = 1f;

	private float musicFadeSpeed = 0f;

	private float MENUVOLUME = 0.175f;

	private SoundEffect beamSound;

	private SoundEffect biplaneSound;

	private SoundEffect bonusPointSound;

	private SoundEffect bulletSound;

	private SoundEffect buzzerSound;

	private SoundEffect carrierLiftSound;

	private SoundEffect clockTickSound;

	private SoundEffect cupAwardedSound;

	private SoundEffect dive1Sound;

	private SoundEffect dive2Sound;

	private SoundEffect dive3Sound;

	private SoundEffect dive4Sound;

	private SoundEffect dive5Sound;

	private SoundEffect doorCloseSound;

	private SoundEffect electricSound;

	private SoundEffect engineSound;

	private SoundEffect explosionSound;

	private SoundEffect flyBySound;

	private SoundEffect footStepSound;

	private SoundEffect helicopterSound;

	private SoundEffect hitGroundSound;

	private SoundEffect hitSound;

	private SoundEffect ignitionSound;

	private SoundEffect jumpSound;

	private SoundEffect keyboardSound;

	private SoundEffect klaxonSound;

	private SoundEffect menuSelectSound;

	private SoundEffect menuSwitchSound;

	private SoundEffect maximiseSound;

	private SoundEffect minimiseSound;

	private SoundEffect misfireSound;

	private SoundEffect missileLaunchSound;

	private SoundEffect missileSound;

	private SoundEffect oceanSound;

	private SoundEffect parachuteOpenSound;

	private SoundEffect pilotFallingSound;

	private SoundEffect pilotShot1Sound;

	private SoundEffect pilotShot2Sound;

	private SoundEffect podiumResultSound;

	private SoundEffect pointScoredSound;

	private SoundEffect roundEndSound;

	private SoundEffect shotPilotImpactSound;

	private SoundEffect sirenCloseSound;

	private SoundEffect sirenDistantSound;

	private SoundEffect sonarSound;

	private SoundEffect splashSound;

	private SoundEffect stallSound;

	private SoundEffect truckEngineSound;

	private SoundEffect windSound;

	private SoundEffect rollCallMusic;

	private SoundEffect sortieReportMusic;

	private SoundEffect titleMusic;

	private SoundEffectInstance beamSoundInstance;

	private bool beamSoundOn = false;

	private SoundEffectInstance biplaneSoundInstance;

	private bool biplaneSoundOn = false;

	private SoundEffectInstance electricSoundInstance;

	private bool electricSoundOn = false;

	private SoundEffectInstance flyBySoundInstance;

	private bool flyBySoundOn = false;

	private SoundEffectInstance helicopterSoundInstance;

	private bool helicopterSoundOn;

	private SoundEffectInstance truckEngineSoundInstance;

	private bool truckEngineSoundOn = false;

	private SoundEffectInstance klaxonSoundInstance;

	private bool klaxonSoundOn = false;

	private SoundEffectInstance oceanSoundInstance;

	private bool oceanSoundOn = false;

	private SoundEffectInstance sirenCloseSoundInstance;

	private bool sirenCloseSoundOn = false;

	private SoundEffectInstance sirenDistantSoundInstance;

	private bool sirenDistantSoundOn = false;

	private SoundEffectInstance windSoundInstance;

	private bool windSoundOn = false;

	private bool[] engineSoundOn;

	private SoundEffectInstance[] engineSoundInstance;

	private bool[] dive1SoundOn;

	private bool[] dive2SoundOn;

	private bool[] dive3SoundOn;

	private bool[] dive4SoundOn;

	private SoundEffectInstance[] dive1SoundInstance;

	private SoundEffectInstance[] dive2SoundInstance;

	private SoundEffectInstance[] dive3SoundInstance;

	private SoundEffectInstance[] dive4SoundInstance;

	private SoundEffectInstance[] stallSoundInstance;

	private bool[] pilotFallingSoundOn;

	private SoundEffectInstance[] pilotFallingSoundInstance;

	private bool[] missileSoundOn;

	private SoundEffectInstance[] missileSoundInstance;

	private SoundEffectInstance rollCallMusicInstance;

	private bool rollCallMusicOn = false;

	private float volumeRollCallMusic = 0.5f;

	private SoundEffectInstance sortieReportMusicInstance;

	private bool sortieReportMusicOn = false;

	private float volumeSortieReportMusic = 0.75f;

	private SoundEffectInstance titleMusicInstance;

	private bool titleMusicOn = false;

	private float volumeTitleMusic = 0.5f;

	private State currentState = State.normal;

	public SoundManager(GameWorld gw)
	{
		g = gw;
		dive1SoundInstance = new SoundEffectInstance[g.planes.Length];
		dive2SoundInstance = new SoundEffectInstance[g.planes.Length];
		dive3SoundInstance = new SoundEffectInstance[g.planes.Length];
		dive4SoundInstance = new SoundEffectInstance[g.planes.Length];
		engineSoundInstance = new SoundEffectInstance[g.planes.Length];
		stallSoundInstance = new SoundEffectInstance[g.planes.Length];
		pilotFallingSoundInstance = new SoundEffectInstance[g.pilots.Length];
		missileSoundInstance = new SoundEffectInstance[6];
		missileSoundOn = new bool[6];
		engineSoundOn = new bool[g.planes.Length];
		dive1SoundOn = new bool[g.planes.Length];
		dive2SoundOn = new bool[g.planes.Length];
		dive3SoundOn = new bool[g.planes.Length];
		dive4SoundOn = new bool[g.planes.Length];
		pilotFallingSoundOn = new bool[g.pilots.Length];
		for (int i = 0; i < g.planes.Length; i++)
		{
			engineSoundOn[i] = false;
			dive1SoundOn[i] = false;
			dive2SoundOn[i] = false;
			dive3SoundOn[i] = false;
			dive4SoundOn[i] = false;
		}
		for (int i = 0; i < g.pilots.Length; i++)
		{
			pilotFallingSoundOn[i] = false;
		}
	}

	public void LoadContent(ContentManager theContentManager)
	{
		beamSound = theContentManager.Load<SoundEffect>("Sound_Beam");
		biplaneSound = theContentManager.Load<SoundEffect>("Sound_Biplane");
		bonusPointSound = theContentManager.Load<SoundEffect>("Sound_BonusPoint");
		bulletSound = theContentManager.Load<SoundEffect>("Sound_Bullet");
		buzzerSound = theContentManager.Load<SoundEffect>("Sound_Buzzer");
		carrierLiftSound = theContentManager.Load<SoundEffect>("Sound_CarrierLift");
		clockTickSound = theContentManager.Load<SoundEffect>("Sound_ClockTick");
		cupAwardedSound = theContentManager.Load<SoundEffect>("Sound_CupAwarded");
		dive1Sound = theContentManager.Load<SoundEffect>("Sound_DiveWhine1");
		dive2Sound = theContentManager.Load<SoundEffect>("Sound_DiveWhine2");
		dive3Sound = theContentManager.Load<SoundEffect>("Sound_DiveWhine3");
		dive4Sound = theContentManager.Load<SoundEffect>("Sound_DiveWhine4");
		doorCloseSound = theContentManager.Load<SoundEffect>("Sound_DoorClose");
		electricSound = theContentManager.Load<SoundEffect>("Sound_Electric");
		engineSound = theContentManager.Load<SoundEffect>("Sound_JetEngineLoud");
		explosionSound = theContentManager.Load<SoundEffect>("Sound_Explosion3");
		flyBySound = theContentManager.Load<SoundEffect>("Sound_FlyBy");
		footStepSound = theContentManager.Load<SoundEffect>("Sound_FootStep");
		helicopterSound = theContentManager.Load<SoundEffect>("Sound_Helicopter");
		hitGroundSound = theContentManager.Load<SoundEffect>("Sound_HitGround");
		hitSound = theContentManager.Load<SoundEffect>("Sound_Hit");
		ignitionSound = theContentManager.Load<SoundEffect>("Sound_Ignition");
		jumpSound = theContentManager.Load<SoundEffect>("Sound_Jump");
		keyboardSound = theContentManager.Load<SoundEffect>("Sound_Keyboard2");
		klaxonSound = theContentManager.Load<SoundEffect>("Sound_Klaxon");
		menuSelectSound = theContentManager.Load<SoundEffect>("Sound_MenuSelect");
		menuSwitchSound = theContentManager.Load<SoundEffect>("Sound_MenuSwitch");
		maximiseSound = theContentManager.Load<SoundEffect>("Sound_Maximise");
		minimiseSound = theContentManager.Load<SoundEffect>("Sound_Minimise");
		misfireSound = theContentManager.Load<SoundEffect>("Sound_Misfire");
		missileLaunchSound = theContentManager.Load<SoundEffect>("Sound_MissileLaunch");
		missileSound = theContentManager.Load<SoundEffect>("Sound_Missile");
		oceanSound = theContentManager.Load<SoundEffect>("Sound_Ocean");
		parachuteOpenSound = theContentManager.Load<SoundEffect>("Sound_ParachuteOpen");
		pilotFallingSound = theContentManager.Load<SoundEffect>("Sound_PilotFalling");
		pilotShot1Sound = theContentManager.Load<SoundEffect>("Sound_PilotShot1");
		pilotShot2Sound = theContentManager.Load<SoundEffect>("Sound_PilotShot2");
		podiumResultSound = theContentManager.Load<SoundEffect>("Sound_PodiumResult");
		pointScoredSound = theContentManager.Load<SoundEffect>("Sound_PointScored");
		roundEndSound = theContentManager.Load<SoundEffect>("Sound_RoundEnd");
		shotPilotImpactSound = theContentManager.Load<SoundEffect>("Sound_ShotPilotImpact");
		sirenCloseSound = theContentManager.Load<SoundEffect>("Sound_SirenClose");
		sirenDistantSound = theContentManager.Load<SoundEffect>("Sound_SirenDistant");
		sonarSound = theContentManager.Load<SoundEffect>("Sound_Sonar");
		stallSound = theContentManager.Load<SoundEffect>("Sound_Stall");
		splashSound = theContentManager.Load<SoundEffect>("Sound_Splash");
		truckEngineSound = theContentManager.Load<SoundEffect>("Sound_TruckEngine");
		windSound = theContentManager.Load<SoundEffect>("Sound_Wind");
		rollCallMusic = theContentManager.Load<SoundEffect>("Music_RollCall");
		sortieReportMusic = theContentManager.Load<SoundEffect>("Music_SortieReport");
		titleMusic = theContentManager.Load<SoundEffect>("Music_Title");
		for (int i = 0; i < g.planes.Length; i++)
		{
			dive1SoundInstance[i] = dive1Sound.CreateInstance();
			dive2SoundInstance[i] = dive2Sound.CreateInstance();
			dive3SoundInstance[i] = dive3Sound.CreateInstance();
			dive4SoundInstance[i] = dive4Sound.CreateInstance();
			dive1SoundInstance[i].IsLooped = true;
			dive2SoundInstance[i].IsLooped = true;
			dive3SoundInstance[i].IsLooped = true;
			dive4SoundInstance[i].IsLooped = true;
			engineSoundInstance[i] = engineSound.CreateInstance();
			engineSoundInstance[i].IsLooped = true;
			stallSoundInstance[i] = stallSound.CreateInstance();
		}
		for (int i = 0; i < g.pilots.Length; i++)
		{
			pilotFallingSoundInstance[i] = pilotFallingSound.CreateInstance();
		}
		for (int i = 0; i < g.theSpecialBonus.GetMissileLimit(); i++)
		{
			missileSoundInstance[i] = missileSound.CreateInstance();
			missileSoundInstance[i].IsLooped = true;
		}
		beamSoundInstance = beamSound.CreateInstance();
		biplaneSoundInstance = biplaneSound.CreateInstance();
		electricSoundInstance = electricSound.CreateInstance();
		flyBySoundInstance = flyBySound.CreateInstance();
		helicopterSoundInstance = helicopterSound.CreateInstance();
		helicopterSoundInstance.IsLooped = true;
		truckEngineSoundInstance = truckEngineSound.CreateInstance();
		truckEngineSoundInstance.IsLooped = true;
		klaxonSoundInstance = klaxonSound.CreateInstance();
		oceanSoundInstance = oceanSound.CreateInstance();
		oceanSoundInstance.IsLooped = true;
		sirenCloseSoundInstance = sirenCloseSound.CreateInstance();
		sirenDistantSoundInstance = sirenDistantSound.CreateInstance();
		sirenDistantSoundInstance.IsLooped = true;
		rollCallMusicInstance = rollCallMusic.CreateInstance();
		rollCallMusicInstance.IsLooped = true;
		sortieReportMusicInstance = sortieReportMusic.CreateInstance();
		sortieReportMusicInstance.IsLooped = true;
		titleMusicInstance = titleMusic.CreateInstance();
		titleMusicInstance.IsLooped = true;
		windSoundInstance = windSound.CreateInstance();
		windSoundInstance.IsLooped = true;
	}

	public void Update(GameTime theGameTime)
	{
		FadeMusic(theGameTime);
		CheckMusicVolumes(theGameTime);
	}

	public void PauseSounds()
	{
		for (int i = 0; i < g.planes.Length; i++)
		{
			if (engineSoundOn[i])
			{
				engineSoundInstance[i].Pause();
			}
			if (dive1SoundOn[i])
			{
				dive1SoundInstance[i].Pause();
			}
			if (dive2SoundOn[i])
			{
				dive2SoundInstance[i].Pause();
			}
			if (dive3SoundOn[i])
			{
				dive3SoundInstance[i].Pause();
			}
			if (dive4SoundOn[i])
			{
				dive4SoundInstance[i].Pause();
			}
		}
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (pilotFallingSoundOn[i])
			{
				pilotFallingSoundInstance[i].Pause();
			}
		}
		for (int i = 0; i < g.theSpecialBonus.GetMissileLimit(); i++)
		{
			if (missileSoundOn[i])
			{
				missileSoundInstance[i].Pause();
			}
		}
		if (beamSoundOn)
		{
			beamSoundInstance.Pause();
		}
		if (biplaneSoundOn)
		{
			biplaneSoundInstance.Pause();
		}
		if (electricSoundOn)
		{
			electricSoundInstance.Pause();
		}
		if (flyBySoundOn)
		{
			flyBySoundInstance.Pause();
		}
		if (helicopterSoundOn)
		{
			helicopterSoundInstance.Pause();
		}
		if (truckEngineSoundOn)
		{
			truckEngineSoundInstance.Pause();
		}
		if (klaxonSoundOn)
		{
			klaxonSoundInstance.Pause();
		}
		if (oceanSoundOn)
		{
			oceanSoundInstance.Pause();
		}
		if (sirenCloseSoundOn)
		{
			sirenCloseSoundInstance.Pause();
		}
		if (sirenDistantSoundOn)
		{
			sirenDistantSoundInstance.Pause();
		}
		if (windSoundOn)
		{
			windSoundInstance.Pause();
		}
	}

	public void UnPauseSounds()
	{
		for (int i = 0; i < g.planes.Length; i++)
		{
			if (engineSoundOn[i])
			{
				engineSoundInstance[i].Resume();
			}
			if (dive1SoundOn[i])
			{
				dive1SoundInstance[i].Resume();
			}
			if (dive2SoundOn[i])
			{
				dive2SoundInstance[i].Resume();
			}
			if (dive3SoundOn[i])
			{
				dive3SoundInstance[i].Resume();
			}
			if (dive4SoundOn[i])
			{
				dive4SoundInstance[i].Resume();
			}
		}
		for (int i = 0; i < g.pilots.Length; i++)
		{
			if (pilotFallingSoundOn[i])
			{
				pilotFallingSoundInstance[i].Resume();
			}
		}
		for (int i = 0; i < g.theSpecialBonus.GetMissileLimit(); i++)
		{
			if (missileSoundOn[i])
			{
				missileSoundInstance[i].Resume();
			}
		}
		if (beamSoundOn)
		{
			beamSoundInstance.Resume();
		}
		if (biplaneSoundOn)
		{
			biplaneSoundInstance.Resume();
		}
		if (electricSoundOn)
		{
			electricSoundInstance.Resume();
		}
		if (flyBySoundOn)
		{
			flyBySoundInstance.Resume();
		}
		if (helicopterSoundOn)
		{
			helicopterSoundInstance.Resume();
		}
		if (truckEngineSoundOn)
		{
			truckEngineSoundInstance.Resume();
		}
		if (klaxonSoundOn)
		{
			klaxonSoundInstance.Resume();
		}
		if (oceanSoundOn)
		{
			oceanSoundInstance.Resume();
		}
		if (sirenCloseSoundOn)
		{
			sirenCloseSoundInstance.Resume();
		}
		if (sirenDistantSoundOn)
		{
			sirenDistantSoundInstance.Resume();
		}
		if (windSoundOn)
		{
			windSoundInstance.Resume();
		}
	}

	public void StopAllSounds()
	{
		for (int i = 0; i < g.planes.Length; i++)
		{
			StopEngine(g.planes[i]);
			StopDive(g.planes[i]);
			StopStallSound(g.planes[i]);
		}
		for (int i = 0; i < g.pilots.Length; i++)
		{
			StopPilotFallingSound(g.pilots[i]);
		}
		for (int i = 0; i < g.theSpecialBonus.GetMissileLimit(); i++)
		{
			StopMissile(g.theSpecialBonus.GetMissile(i));
		}
		StopBeamSound();
		StopBiplaneSound();
		StopElectricSound();
		StopFlyBySound();
		StopHelicopterSound();
		StopTruckEngineSound();
		StopKlaxonSound();
		StopOceanSound();
		StopSirenCloseSound();
		StopSirenDistantSound();
		StopWindSound();
	}

	public void AdjustAllSoundVolumes()
	{
		for (int i = 0; i < g.planes.Length; i++)
		{
			VolumeEngine(i);
			VolumeDive(i);
			VolumeStallSound(i);
		}
		for (int i = 0; i < g.pilots.Length; i++)
		{
			VolumePilotFallingSound(i);
		}
		for (int i = 0; i < g.theSpecialBonus.GetMissileLimit(); i++)
		{
			VolumeMissile(i);
		}
		VolumeBeamSound();
		VolumeBiplaneSound();
		VolumeElectricSound();
		VolumeFlyBySound();
		VolumeHelicopterSound();
		VolumeTruckEngineSound();
		VolumeKlaxonSound();
		VolumeOceanSound();
		VolumeSirenCloseSound();
		VolumeSirenDistantSound();
		VolumeWindSound();
	}

	public void BonusPointSound()
	{
		float num = 1f;
		float pitch = 1f;
		bonusPointSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void BulletHitGroundSound()
	{
		float num = 0.25f;
		float pitch = 1f;
		parachuteOpenSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void BulletSound()
	{
		float num = 0.5f;
		bulletSound.Play(num * volumeSFX, 1f, 0f);
	}

	public void BulletSoundTest()
	{
		float num = 0.5f;
		bulletSound.Play(num * provVolume, 1f, 0f);
		float mENUVOLUME = MENUVOLUME;
		float pitch = 0f;
		menuSwitchSound.Play(mENUVOLUME * provVolume, pitch, 0f);
	}

	public void BuzzerSound()
	{
		float num = 1f;
		buzzerSound.Play(num * volumeSFX, 0f, 0f);
	}

	public void CarrierLiftSound(float f)
	{
		float num = 1f;
		carrierLiftSound.Play(num * volumeSFX, f, 0f);
	}

	public void ClockTickSound(float f)
	{
		float num = 1f;
		float num2 = f;
		if (num2 < -1f)
		{
			num2 = -1f;
		}
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		clockTickSound.Play(num * volumeSFX, num2, 0f);
	}

	public void CupAwardedSound()
	{
		float num = 1f;
		float pitch = 0f;
		cupAwardedSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void DoorCloseSound()
	{
		float num = 1f;
		float pitch = -0.75f;
		doorCloseSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void StartDive(Plane p)
	{
		float num = 0.55f;
		float num2 = 0.55f;
		float num3 = 0.55f;
		float num4 = 0.55f;
		float num5 = 0.55f;
		int num6 = IdentifyPlane(p);
		int nextRandom = General.GetNextRandom(0, 5);
		if (p.GetActive())
		{
			switch (nextRandom)
			{
			case 0:
				dive1SoundInstance[num6].Volume = num * volumeSFX;
				dive1SoundInstance[num6].Play();
				dive1SoundOn[num6] = true;
				break;
			case 1:
				dive2SoundInstance[num6].Volume = num2 * volumeSFX;
				dive2SoundInstance[num6].Play();
				dive2SoundOn[num6] = true;
				break;
			case 2:
				dive3SoundInstance[num6].Volume = num3 * volumeSFX;
				dive3SoundInstance[num6].Play();
				dive3SoundOn[num6] = true;
				break;
			default:
				dive4SoundInstance[num6].Volume = num4 * volumeSFX;
				dive4SoundInstance[num6].Play();
				dive4SoundOn[num6] = true;
				break;
			}
		}
	}

	public void DivePitch(Plane p, float f)
	{
		int num = IdentifyPlane(p);
		if (f <= -1f)
		{
			f = -1f;
		}
		if (f >= 1f)
		{
			f = 1f;
		}
		dive1SoundInstance[num].Pitch = f;
		dive2SoundInstance[num].Pitch = f;
		dive3SoundInstance[num].Pitch = f;
		dive4SoundInstance[num].Pitch = f;
	}

	public void StopDive(Plane p)
	{
		int num = IdentifyPlane(p);
		dive1SoundInstance[num].Stop();
		dive2SoundInstance[num].Stop();
		dive3SoundInstance[num].Stop();
		dive4SoundInstance[num].Stop();
		dive1SoundOn[num] = false;
		dive2SoundOn[num] = false;
		dive3SoundOn[num] = false;
		dive4SoundOn[num] = false;
	}

	public void VolumeDive(int i)
	{
		float num = 0.55f;
		float num2 = 0.55f;
		float num3 = 0.55f;
		float num4 = 0.55f;
		float num5 = 0.55f;
		dive1SoundInstance[i].Volume = num * volumeSFX;
		dive2SoundInstance[i].Volume = num2 * volumeSFX;
		dive3SoundInstance[i].Volume = num3 * volumeSFX;
		dive4SoundInstance[i].Volume = num4 * volumeSFX;
	}

	public void ExplosionSound()
	{
		float num = 1f;
		float pitch = 0.5f;
		explosionSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void StartBeamSound()
	{
		float num = 1f;
		float pitch = 0.5f;
		if (!beamSoundOn)
		{
			beamSoundInstance.Volume = num * volumeSFX;
			beamSoundInstance.Pitch = pitch;
			beamSoundInstance.Play();
			beamSoundOn = true;
		}
	}

	public void StopBeamSound()
	{
		beamSoundInstance.Stop();
		beamSoundOn = false;
	}

	public void VolumeBeamSound()
	{
		float num = 0.5f;
		beamSoundInstance.Volume = num * volumeSFX;
	}

	public void StartBiplaneSound()
	{
		float num = 0.5f;
		float pitch = 0.75f;
		if (!biplaneSoundOn)
		{
			biplaneSoundInstance.Volume = num * volumeSFX;
			biplaneSoundInstance.Pitch = pitch;
			biplaneSoundInstance.Play();
			biplaneSoundOn = true;
		}
	}

	public void StopBiplaneSound()
	{
		biplaneSoundInstance.Stop();
		biplaneSoundOn = false;
	}

	public void VolumeBiplaneSound()
	{
		float num = 0.5f;
		biplaneSoundInstance.Volume = num * volumeSFX;
	}

	public void StartElectricSound()
	{
		float num = 0.0025f;
		float pitch = 1f;
		if (!electricSoundOn)
		{
			electricSoundInstance.Volume = num * volumeSFX;
			electricSoundInstance.Pitch = pitch;
			electricSoundInstance.Play();
			electricSoundOn = true;
		}
	}

	public void StopElectricSound()
	{
		electricSoundInstance.Stop();
		electricSoundOn = false;
	}

	public void VolumeElectricSound()
	{
		float num = 0.0025f;
		electricSoundInstance.Volume = num * volumeSFX;
	}

	public void StartFlyBySound(float p)
	{
		float num = 1f;
		if (!flyBySoundOn)
		{
			flyBySoundInstance.Volume = num * volumeSFX;
			flyBySoundInstance.Pitch = p;
			flyBySoundInstance.Play();
			flyBySoundOn = true;
		}
	}

	public void StopFlyBySound()
	{
		flyBySoundInstance.Stop();
		flyBySoundOn = false;
	}

	public void VolumeFlyBySound()
	{
		float num = 0.5f;
		flyBySoundInstance.Volume = num * volumeSFX;
	}

	public void FootStepSound()
	{
		float num = 0.01f;
		float pitch = 1f;
		footStepSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void StartHelicopterSound()
	{
		float num = 0.1f;
		float pitch = 0f;
		if (!helicopterSoundOn)
		{
			helicopterSoundInstance.Volume = num * volumeSFX;
			helicopterSoundInstance.Pitch = pitch;
			helicopterSoundInstance.Play();
			helicopterSoundOn = true;
		}
	}

	public void StopHelicopterSound()
	{
		helicopterSoundInstance.Stop();
		helicopterSoundOn = false;
	}

	public void VolumeHelicopterSound()
	{
		float num = 0.1f;
		helicopterSoundInstance.Volume = num * volumeSFX;
	}

	public void HitGroundSound()
	{
		float num = 1f;
		float pitch = -1f;
		hitGroundSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void HitSound()
	{
		float num = 1f;
		float pitch = -0.5f;
		hitSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void IgnitionSound()
	{
		float num = 0.175f;
		float pitch = 0f;
		ignitionSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void JumpSound()
	{
		float num = 0.35f;
		float pitch = 0f;
		jumpSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void KeyboardSound()
	{
		float num = 1f;
		float pitch = 0f;
		keyboardSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void StartKlaxonSound()
	{
		float num = 0.5f;
		float pitch = 0.25f;
		klaxonSoundInstance.Volume = num * volumeSFX;
		klaxonSoundInstance.Pitch = pitch;
		klaxonSoundInstance.Play();
		klaxonSoundOn = true;
	}

	public void StopKlaxonSound()
	{
		klaxonSoundInstance.Stop();
		klaxonSoundOn = false;
	}

	public void VolumeKlaxonSound()
	{
		float num = 0.5f;
		klaxonSoundInstance.Volume = num * volumeSFX;
	}

	public void StartOceanSound()
	{
		float num = 0.75f;
		float pitch = 0f;
		oceanSoundInstance.Volume = num * volumeSFX;
		oceanSoundInstance.Pitch = pitch;
		oceanSoundInstance.Play();
		oceanSoundOn = true;
	}

	public void StopOceanSound()
	{
		oceanSoundInstance.Stop();
		oceanSoundOn = false;
	}

	public void VolumeOceanSound()
	{
		float num = 0.75f;
		oceanSoundInstance.Volume = num * volumeSFX;
	}

	public void MaximiseSound()
	{
		float num = 1f;
		float pitch = 0f;
		maximiseSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void MenuSelectSound()
	{
		float mENUVOLUME = MENUVOLUME;
		float pitch = 0f;
		menuSelectSound.Play(mENUVOLUME * volumeSFX, pitch, 0f);
	}

	public void MenuSwitchSound()
	{
		float mENUVOLUME = MENUVOLUME;
		float pitch = 0f;
		menuSwitchSound.Play(mENUVOLUME * volumeSFX, pitch, 0f);
	}

	public void MinimiseSound()
	{
		float num = 1f;
		float pitch = 0f;
		minimiseSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void MisfireSound()
	{
		float num = 0.02f;
		float pitch = -1f;
		misfireSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void MissileLaunchSound()
	{
		float num = 1f;
		float pitch = 0f;
		missileLaunchSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void MusicSoundTest()
	{
		float num = provVolume;
		if (!MediaPlayer.GameHasControl)
		{
			provVolume = 0f;
		}
		titleMusicInstance.Volume = volumeTitleMusic * provVolume;
		provVolume = num;
	}

	public void StartEngine(Plane p)
	{
		float num = 0.25f;
		int num2 = IdentifyPlane(p);
		engineSoundInstance[num2].Volume = num * volumeSFX;
		engineSoundInstance[num2].Play();
		engineSoundOn[num2] = true;
	}

	public void StopEngine(Plane p)
	{
		int num = IdentifyPlane(p);
		engineSoundInstance[num].Stop();
		engineSoundOn[num] = false;
	}

	public void VolumeEngine(int i)
	{
		float num = 0.25f;
		engineSoundInstance[i].Volume = num * volumeSFX;
	}

	public void EnginePitch(Plane p, float f)
	{
		int num = IdentifyPlane(p);
		if (f < 0f)
		{
			f = 0f;
		}
		if (f > 1f)
		{
			f = 1f;
		}
		f -= 0.25f;
		engineSoundInstance[num].Pitch = f;
	}

	public void StartMissile(Missile m)
	{
		float num = 0.25f;
		int num2 = IdentifyMissile(m);
		missileSoundInstance[num2].Volume = num * volumeSFX;
		missileSoundInstance[num2].Play();
		missileSoundOn[num2] = true;
	}

	public void StopMissile(Missile m)
	{
		int num = IdentifyMissile(m);
		missileSoundInstance[num].Stop();
		missileSoundOn[num] = false;
	}

	public void VolumeMissile(int i)
	{
		float num = 0.15f;
		missileSoundInstance[i].Volume = num * volumeSFX;
	}

	public void missilePitch(Missile m, float f)
	{
		int num = IdentifyMissile(m);
		if (f < 0f)
		{
			f = 0f;
		}
		if (f > 1f)
		{
			f = 1f;
		}
		f -= 0.25f;
		missileSoundInstance[num].Pitch = f;
	}

	public void StartPilotFallingSound(Pilot p)
	{
		int num = IdentifyPilot(p);
		float num2 = 1f;
		float pitch = 0.75f;
		pilotFallingSoundInstance[num].Volume = num2 * volumeSFX;
		pilotFallingSoundInstance[num].Pitch = pitch;
		pilotFallingSoundInstance[num].Play();
		pilotFallingSoundOn[num] = true;
	}

	public void StopPilotFallingSound(Pilot p)
	{
		int num = IdentifyPilot(p);
		pilotFallingSoundInstance[num].Stop();
		pilotFallingSoundOn[num] = false;
	}

	public void VolumePilotFallingSound(int i)
	{
		float num = 1f;
		pilotFallingSoundInstance[i].Volume = num * volumeSFX;
	}

	public void ParachuteOpenSound()
	{
		float num = 1f;
		parachuteOpenSound.Play(num * volumeSFX, 0f, 0f);
	}

	public void PilotShotSound()
	{
		float num = 1f;
		float num2 = 0.75f;
		switch (General.GetNextRandom(0, 2))
		{
		case 0:
			pilotShot1Sound.Play(num * volumeSFX, 0.5f, 0f);
			break;
		case 1:
			pilotShot2Sound.Play(num2 * volumeSFX, 0.5f, 0f);
			break;
		}
	}

	public void PodiumResultSound()
	{
		float num = 1f;
		podiumResultSound.Play(num * volumeSFX, -0.2f, 0f);
	}

	public void PointScoredSound()
	{
		float num = 0.5f;
		float pitch = 0f;
		float pan = 0f;
		pointScoredSound.Play(num * volumeSFX, pitch, pan);
	}

	public void RoundEndSound()
	{
		float num = 1f;
		float pitch = 0f;
		roundEndSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void ShotPilotImpactSound()
	{
		float num = 0.25f;
		float pitch = -1f;
		shotPilotImpactSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void SonarSound()
	{
		float num = 1f;
		float pitch = 0f;
		sonarSound.Play(num * volumeSFX, pitch, 0f);
	}

	public void StartSirenCloseSound()
	{
		float num = 1f;
		float pitch = 0f;
		sirenCloseSoundInstance.Volume = num * volumeSFX;
		sirenCloseSoundInstance.Pitch = pitch;
		sirenCloseSoundInstance.Play();
		sirenCloseSoundOn = true;
	}

	public void StopSirenCloseSound()
	{
		sirenCloseSoundInstance.Stop();
		sirenCloseSoundOn = false;
	}

	public void VolumeSirenCloseSound()
	{
		float num = 1f;
		sirenCloseSoundInstance.Volume = num * volumeSFX;
	}

	public void StartSirenDistantSound()
	{
		float num = 0.15f;
		float pitch = 0f;
		sirenDistantSoundInstance.Volume = num * volumeSFX;
		sirenDistantSoundInstance.Pitch = pitch;
		sirenDistantSoundInstance.Play();
		sirenDistantSoundOn = true;
	}

	public void StopSirenDistantSound()
	{
		sirenDistantSoundInstance.Stop();
		sirenDistantSoundOn = false;
	}

	public void VolumeSirenDistantSound()
	{
		float num = 0.15f;
		sirenDistantSoundInstance.Volume = num * volumeSFX;
	}

	public void SplashSound(float f)
	{
		float num = 1f;
		splashSound.Play(num * volumeSFX, f, 0f);
	}

	public void StallSound(Plane p)
	{
		float num = 0.6f;
		float pitch = -0.5f;
		int num2 = IdentifyPlane(p);
		stallSoundInstance[num2].Volume = num * volumeSFX;
		stallSoundInstance[num2].Pitch = pitch;
		stallSoundInstance[num2].Play();
	}

	public void StopStallSound(Plane p)
	{
		int num = IdentifyPlane(p);
		stallSoundInstance[num].Stop();
	}

	public void VolumeStallSound(int i)
	{
		float num = 0.6f;
		stallSoundInstance[i].Volume = num * volumeSFX;
	}

	public void SetTruckEnginePitch(float f)
	{
		truckEngineSoundInstance.Pitch = f;
	}

	public void StartTruckEngine()
	{
		float num = 1f;
		truckEngineSoundInstance.Volume = num * volumeSFX;
		truckEngineSoundInstance.Play();
		truckEngineSoundOn = true;
	}

	public void StopTruckEngineSound()
	{
		truckEngineSoundInstance.Stop();
		truckEngineSoundOn = false;
	}

	public void VolumeTruckEngineSound()
	{
		float num = 1f;
		truckEngineSoundInstance.Volume = num * volumeSFX;
	}

	public void StartRollCallMusic()
	{
		musicFade = 1f;
		musicFadeSpeed = 0f;
		float num = volumeMusic;
		if (!MediaPlayer.GameHasControl)
		{
			volumeMusic = 0f;
		}
		rollCallMusicInstance.Volume = volumeRollCallMusic * musicFade * volumeMusic;
		rollCallMusicInstance.Play();
		rollCallMusicOn = true;
		volumeMusic = num;
	}

	public void StopRollCallMusic()
	{
		rollCallMusicInstance.Stop();
		rollCallMusicOn = false;
	}

	public void VolumeRollCallMusic()
	{
		rollCallMusicInstance.Volume = volumeRollCallMusic * musicFade * volumeMusic;
	}

	public void StartSortieReportMusic()
	{
		musicFade = 1f;
		musicFadeSpeed = 0f;
		float num = volumeMusic;
		if (!MediaPlayer.GameHasControl)
		{
			volumeMusic = 0f;
		}
		sortieReportMusicInstance.Volume = volumeSortieReportMusic * musicFade * volumeMusic;
		sortieReportMusicInstance.Play();
		sortieReportMusicOn = true;
		volumeMusic = num;
	}

	public void StopSortieReportMusic()
	{
		sortieReportMusicInstance.Stop();
		sortieReportMusicOn = false;
	}

	public void VolumeSortieReportMusic()
	{
		sortieReportMusicInstance.Volume = volumeSortieReportMusic * musicFade * volumeMusic;
	}

	public void StartTitleMusic()
	{
		musicFade = 1f;
		musicFadeSpeed = 0f;
		float num = volumeMusic;
		if (!MediaPlayer.GameHasControl)
		{
			volumeMusic = 0f;
		}
		titleMusicInstance.Volume = volumeTitleMusic * musicFade * volumeMusic;
		titleMusicInstance.Play();
		titleMusicOn = true;
		volumeMusic = num;
		g.theFrontEnd.FrontEndMusicOn(b: true);
	}

	public void StopTitleMusic()
	{
		titleMusicInstance.Stop();
		titleMusicOn = false;
		g.theFrontEnd.FrontEndMusicOn(b: false);
	}

	public void VolumeTitleMusic()
	{
		titleMusicInstance.Volume = volumeTitleMusic * musicFade * volumeMusic;
	}

	public void CheckMusicVolumes(GameTime theGameTime)
	{
		if (currentState != State.settingMusic)
		{
			float num = volumeMusic;
			if (!MediaPlayer.GameHasControl)
			{
				volumeMusic = 0f;
			}
			VolumeRollCallMusic();
			VolumeSortieReportMusic();
			VolumeTitleMusic();
			volumeMusic = num;
		}
	}

	public void SetMusicFadeSpeed(float f)
	{
		musicFadeSpeed = f;
	}

	public void FadeMusic(GameTime theGameTime)
	{
		musicFade += musicFadeSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
		if (musicFade >= 1f)
		{
			musicFade = 1f;
		}
		if (musicFade <= 0f)
		{
			musicFade = 0f;
		}
	}

	public void StartWindSound()
	{
		float num = 0.5f;
		float pitch = 0f;
		windSoundInstance.Volume = num * volumeSFX;
		windSoundInstance.Pitch = pitch;
		windSoundInstance.Play();
		windSoundOn = true;
	}

	public void StopWindSound()
	{
		windSoundInstance.Stop();
		windSoundOn = false;
	}

	public void VolumeWindSound()
	{
		float num = 0.15f;
		windSoundInstance.Volume = num * volumeSFX;
	}

	public int IdentifyMissile(Missile m)
	{
		int i;
		for (i = 0; g.theSpecialBonus.GetMissile(i) != m; i++)
		{
		}
		return i;
	}

	public int IdentifyPlane(Plane p)
	{
		int i;
		for (i = 0; g.planes[i] != p; i++)
		{
		}
		return i;
	}

	public int IdentifyPilot(Pilot p)
	{
		int i;
		for (i = 0; g.pilots[i] != p; i++)
		{
		}
		return i;
	}

	public float GetVolumeSFX()
	{
		return volumeSFX;
	}

	public void SetVolumeSFX(float f)
	{
		volumeSFX = f;
	}

	public float GetVolumeMusic()
	{
		return volumeMusic;
	}

	public void SetVolumeMusic(float f)
	{
		volumeMusic = f;
	}

	public float GetProvVolume()
	{
		return provVolume;
	}

	public void SetProvVolume(float f)
	{
		provVolume = f;
	}

	public void SetCurrentStateSettingMusic()
	{
		currentState = State.settingMusic;
		float num = provVolume;
		if (!MediaPlayer.GameHasControl)
		{
			provVolume = 0f;
		}
		titleMusicInstance.Volume = volumeTitleMusic * provVolume;
		provVolume = num;
	}

	public void SetCurrentStateNormal()
	{
		currentState = State.normal;
	}

	public void IncrementProvSFXVolume()
	{
		if (provVolume < 1f)
		{
			provVolume += 0.1f;
			BulletSoundTest();
		}
		if (provVolume > 1f)
		{
			provVolume = 1f;
		}
		provVolume = (float)Math.Round(provVolume, 1);
	}

	public void DecrementProvSFXVolume()
	{
		if (provVolume > 0f)
		{
			provVolume -= 0.1f;
			BulletSoundTest();
		}
		if (provVolume < 0f)
		{
			provVolume = 0f;
		}
		provVolume = (float)Math.Round(provVolume, 1);
	}

	public void IncrementProvMusicVolume()
	{
		if (provVolume < 1f)
		{
			provVolume += 0.1f;
			MenuSelectSound();
			MusicSoundTest();
		}
		if (provVolume > 1f)
		{
			provVolume = 1f;
		}
		provVolume = (float)Math.Round(provVolume, 1);
	}

	public void DecrementProvMusicVolume()
	{
		if (provVolume > 0f)
		{
			provVolume -= 0.1f;
			MenuSelectSound();
			MusicSoundTest();
		}
		if (provVolume < 0f)
		{
			provVolume = 0f;
		}
		provVolume = (float)Math.Round(provVolume, 1);
	}

	public void StartRound()
	{
		if (g.GetCurrentLevel() == 1)
		{
		}
		if (g.GetCurrentLevel() == 2)
		{
			StartOceanSound();
		}
		if (g.GetCurrentLevel() == 4)
		{
			StartSirenDistantSound();
		}
		if (g.GetCurrentLevel() == 5)
		{
			StartWindSound();
		}
	}
}
