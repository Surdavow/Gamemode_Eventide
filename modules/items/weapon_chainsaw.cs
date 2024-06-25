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

datablock ShapeBaseImageData(ChainsawImage)
{
   // Basic Item properties
   	shapeFile = "./models/Chainsaw/Chainsaw.dts";
   	emap = true;
   	mountPoint = 0;
   	offset = "0 0 0";
   	eyeOffset = 0;
   	rotation = eulerToMatrix( "0 0 0" );
	correctMuzzleVector = true;
   	className = "WeaponImage";

   	// Projectile && Ammo.
   	item = "";
   	ammo = " ";
   	projectile = HammerProjectile;
   	projectileType = Projectile;

	casing = GunShellDebris;
	shellExitDir        = "1.0 -1.3 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 15.0;	
	shellVelocity       = 7.0;

   	melee = false;
   	armReady = true;
   	LarmReady = true;

   	doColorShift = true;
   	colorShiftColor = (180/255) SPC (180/255) SPC (180/255) SPC (255/255);

   // Initial start up state
	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.15;
	stateTransitionOnTimeout[0]     = "Ready";
	stateSequence[0]				= "activate";
	stateSound[0]					= "chainsaw_rev_sound";
	
	stateName[1]                    = "Ready";
	stateTransitionOnTimeout[1]     = "Ready";
	stateTimeoutValue[1]            = 0.14;
	stateEmitter[1]                = "ChainsawRev2Emitter";
	stateEmitterTime[1]            = 0.14;
	stateEmitterNode[1]            = "smokeNode";
	stateSequence[1]				= "activate";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]        = true;
	stateSound[1]					= "chainsaw_rev_sound";

	stateName[2]                    = "Spinup";
	stateAllowImageChange[2]        = false;
	stateTransitionOnTimeout[2]     = "Fire";
	stateTimeoutValue[2]            = 0.10;
	stateWaitForTimeout[2]			= true;	
	stateTransitionOnTriggerUp[2]   = "Ready";
	stateSound[2]					= "chainsaw_saw";
	stateSequenceOnTimeout[2]	= "Spin";
	
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
	stateSound[3]					= "chainsaw_saw";

	stateName[4] 					= "Smoke";
	stateTimeoutValue[4]            = 0.01;
	stateTransitionOnTimeout[4]     = "Check";

	stateName[5]					= "Check";
	stateTransitionOnTriggerDown[5] = "Fire";
	
	stateName[6]					= "Slow";
	stateTransitionOnTriggerDown[6] = "Fire";
	stateSound[6]					= "chainsaw_end_sound";
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
