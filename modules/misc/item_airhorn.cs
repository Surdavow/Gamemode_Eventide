//gun.cs

//audio
datablock AudioProfile(airhornShotSound)
{
   filename    = "./models/airhorn/airhornShot.wav";
   description = AudioClose3d;
   preload = true;
};


//muzzle flash effects
datablock ParticleData(airhornFlashParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0;
	constantAcceleration = 0;
	lifetimeMS           = 45;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/thinRing";
	spinSpeed		= 0;
	spinRandomMin		= 0;
	spinRandomMax		= 0;
	colors[0]     = "0.6 0.6 0.6 0.3";
	colors[1]     = "0.3 0.3 0.3 0.0";
	sizes[0]      = 0.1;
	sizes[1]      = 0.7;

	useInvAlpha = false;
};

datablock ParticleEmitterData(airhornFlashEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 30;
   velocityVariance = 0;
   ejectionOffset   = 0;
   thetaMin         = 0;
   thetaMax         = 0;
   phiReferenceVel  = 0;
   phiVariance      = 0;
   overrideAdvance = false;
   particles = "airhornFlashParticle";

   uiName = "Air Horn Blast";
};

datablock ExplosionData(airhornExplosion)
{
   //explosionShape = "";
//	soundProfile = bulletHitSound;

   lifeTimeMS = 1000;

//   particleEmitter = gunExplosionEmitter;
//   particleDensity = 5;
//   particleRadius = 0.2;

//   emitter[0] = gunExplosionRingEmitter;

//   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Dynamic light
//   lightStartRadius = 2;
//   lightEndRadius = 2;
//   lightStartColor = "0.5 0.8 0.9";
//   lightEndColor = "0 0 0";
};


AddDamageType("Airhorn",   '<bitmap:add-ons/Weapon_airhorn/CI_airhorn> %1',    '%2 <bitmap:add-ons/Weapon_airhorn/CI_airhorn> %1',0.2,1);
datablock ProjectileData(airhornProjectile)
{
//   projectileShapeName = "./bullet.dts";
   directDamage        = 0;
   directDamageType    = $DamageType::Airhorn;
   radiusDamageType    = $DamageType::Airhorn;

   brickExplosionRadius = 1;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 1;
   brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 1;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 1000;
   verticalImpulse	  = 600;
   explosion           = airhornExplosion;
   particleEmitter     = ""; //bulletTrailEmitter;

   muzzleVelocity      = 90;
   velInheritFactor    = 1;

   armingDelay         = 00;
   lifetime            = 80;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "Airhorn Blast";
};

//////////
// item //
//////////
datablock ItemData(AirhornItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./models/airhorn/airhorn.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Air horn";
	iconName = "./models/airhorn/icon_airhorn";
	doColorShift = false;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	 // Dynamic properties defined by the scripts
	image = airhornImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(airhornImage)
{
   // Basic Item properties
   shapeFile = "./models/airhorn/airhorn.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = BowItem;
   ammo = " ";
   projectile = airhornProjectile;
   projectileType = Projectile;

//	casing = airhornShellDebris;
//	shellExitDir        = "1.0 -1.3 1.0";
//	shellExitOffset     = "0 0 0";
//	shellExitVariance   = 15.0;	
//	shellVelocity       = 7.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = true;
   colorShiftColor = airhornItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.15;
	stateTransitionOnTimeout[0]       = "Ready";
	//stateSound[0]					= weaponSwitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;
	stateSequence[1]	= "Ready";

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Smoke";
	stateTimeoutValue[2]            = 0.55;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;
	stateEmitter[2]					= airhornFlashEmitter;
	stateEmitterTime[2]				= 0.55;
	stateEmitterNode[2]				= "muzzleNode";
	stateSound[2]					= airhornShotSound;
	stateEjectShell[2]       = true;

	stateName[3] = "Smoke";
//	stateEmitter[3]					= gunSmokeEmitter;
	stateEmitterTime[3]				= 0.05;
	stateEmitterNode[3]				= "muzzleNode";
	stateTimeoutValue[3]            = 0.01;
	stateTransitionOnTimeout[3]     = "Reload";

	stateName[4]			= "Reload";
	stateSequence[4]                = "Reload";
	stateTransitionOnTriggerUp[4]     = "Ready";
	stateSequence[4]	= "Ready";

};

function airhornImage::onFire(%this,%obj,%slot)
{
	if(%obj.getDamagePercent() < 1.0)
		%obj.playThread(2, plantBrick);
	Parent::onFire(%this,%obj,%slot);	
}
