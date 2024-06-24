datablock AudioProfile(ChainsawFireSound)
{
   filename    = "./models/Chainsaw/chainsaw_saw.wav";
   description = AudioCloseLooping3d;
   preload = true;
};
datablock AudioProfile(ChainsawEndSound)
{
   filename    = "./models/Chainsaw/chainsaw_end.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(ChainsawPullSound)
{
   filename    = "./models/Chainsaw/chainsaw_rev.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(ChainsawIdleSound)
{
   filename    = "./models/Chainsaw/chainsaw_idle.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock ParticleData(ChainsawRevParticle)
{
	dragCoefficient = 5;
	gravityCoefficient = -2;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	textureName = "base/data/particles/cloud";
	spinSpeed = 9000;
	spinRandomMin = -9000;
	spinRandomMax = 9000;
	useInvAlpha = false;
	colors[0] = "0.3 0.3 0.3 0.1";
	colors[1] = "0.3 0.3 0.3 0";
	sizes[0] = 1.45;
	sizes[1] = 0.05;
};
datablock ParticleEmitterData(ChainsawRevEmitter : GunFlashEmitter)
{
	ejectionPeriodMS = 4;
	particles = ChainsawRevParticle;

	uiName = "";
};

datablock ParticleData(ChainsawRev2Particle)
{
	dragCoefficient = 5;
	gravityCoefficient = -2;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	textureName = "base/data/particles/cloud";
	spinSpeed = 9000;
	spinRandomMin = -9000;
	spinRandomMax = 9000;
	useInvAlpha = false;
	colors[0] = "0.3 0.3 0.3 0.1";
	colors[1] = "0.3 0.3 0.3 0";
	sizes[0] = 0.85;
	sizes[1] = 0.05;
};
datablock ParticleEmitterData(ChainsawRev2Emitter : GunFlashEmitter)
{
	ejectionPeriodMS = 4;
	particles = ChainsawRev2Particle;

	uiName = "";
};

AddDamageType("Chainsaw",   '<bitmap:base/client/ui/CI/generic> %1',    '%2 <bitmap:base/client/ui/CI/generic> %1',0.5,1);
//AddDamageType("L4BChainsaw",   '<bitmap:add-ons/Weapon_melee_extended_II/ci_Chainsaw> %1',    '%2 <bitmap:add-ons/Weapon_melee_extended_II/ci_Chainsaw> %1',0.5,1);

//////////
// item //
//////////
datablock ItemData(ChainsawItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./models/Chainsaw/Chainsaw.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Chainsaw";
	iconName = "./Chainsaw";
	doColorShift = false;
	colorShiftColor = (180/255) SPC (180/255) SPC (180/255) SPC (255/255);

	 // Dynamic properties defined by the scripts
	image = ChainsawImage;
	canDrop = false;
};


////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(ChainsawImage)
{
   // Basic Item properties
   shapeFile = "./models/Chainsaw/Chainsaw.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0;
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
   item = ChainsawItem;
   ammo = " ";
   projectile = HammerProjectile;
   projectileType = Projectile;

	casing = GunShellDebris;
	shellExitDir        = "1.0 -1.3 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 15.0;	
	shellVelocity       = 7.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;
   LarmReady = true;

   doColorShift = true;
   colorShiftColor = ChainsawItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.15;
	stateTransitionOnTimeout[0]     = "Ready";
	stateSequence[0]				= "activate";
	stateSound[0]					= ChainsawPullSound;
	
	stateName[1]                    = "Ready";
	stateTransitionOnTimeout[1]     = "Ready";
	stateTimeoutValue[1]            = 0.14;
	stateEmitter[1]                = ChainsawRev2Emitter;
	stateEmitterTime[1]            = 0.14;
	stateEmitterNode[1]            = "smokeNode";
	stateSequence[1]				= "activate";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]        = true;
	stateSound[1]					= ChainsawIdleSound;

	stateName[2]                    = "Spinup";
	stateAllowImageChange[2]        = false;
	stateTransitionOnTimeout[2]     = "Fire";
	stateTimeoutValue[2]            = 0.10;
	stateWaitForTimeout[2]			= true;
	//stateSound[2]					= L4BChainsawLoopSound;
	stateTransitionOnTriggerUp[2]   = "Ready";
	//stateSequenceOnTimeout[2]	= "Spin";
	
	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "Fire";
	stateTransitionOnTriggerUp[3]   = "Slow";
	stateTimeoutValue[3]            = 0.05;
	stateEmitter[3]                = ChainsawRevEmitter;
	stateEmitterTime[3]            = 0.05;
	stateEmitterNode[3]            = "smokeNode";
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]			= false;
	stateSound[3]					= ChainsawFireSound;

	stateName[4] 					= "Smoke";
	stateTimeoutValue[4]            = 0.01;
	stateTransitionOnTimeout[4]     = "Check";

	stateName[5]					= "Check";
	stateTransitionOnTriggerDown[5] = "Fire";
	
	stateName[6]					= "Slow";
	stateTransitionOnTriggerDown[6] = "Fire";
	//stateSequence[6]                = "ready";
	stateSound[6]					= ChainsawEndSound;
	stateAllowImageChange[6]        = false;
	stateTransitionOnTimeout[6]     = "Ready";
	stateTimeoutValue[6]            = 0.20;
	stateWaitForTimeout[6]			= true;

};

function ChainsawImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, plant);
	Parent::onFire(%this, %obj, %slot);
}

function ChainsawImage::onMount(%this,%obj,%slot)
{
	%obj.playThread(0, plant);
	Parent::onMount(%this,%obj,%slot);
}
