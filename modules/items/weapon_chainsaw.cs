eval("chainsaw_sawing_sound.description = \"AudioDefaultLooping3D\";");

datablock ExplosionData(chainsawExplosion : hammerExplosion)
{
	camShakeRadius = 0.1;
};
datablock ProjectileData(chainsawProjectile : hammerProjectile)
{
	Explosion = chainsawExplosion;
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

datablock ShapeBaseImageData(ChainsawImage)
{
   	shapeFile = "./models/Chainsaw/Chainsaw.dts";
   	emap = true;
   	mountPoint = 0;
   	offset = "0 0 0";
   	eyeOffset = 0;
   	rotation = eulerToMatrix( "0 0 0" );
	correctMuzzleVector = true;
   	className = "WeaponImage";
   	
   	item = "";
   	ammo = " ";
   	projectile = chainsawProjectile;
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

	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.15;
	stateTransitionOnTimeout[0]     = "Ready";
	stateSequence[0]				= "activate";
	stateSound[0]					= "chainsaw_rev_sound";
	
	stateName[1]                    = "Ready";
	stateTransitionOnTimeout[1]     = "Loop";
	stateTimeoutValue[1]            = 0.14;
	stateEmitter[1]                = "ChainsawRev2Emitter";
	stateEmitterTime[1]            = 0.14;
	stateEmitterNode[1]            = "smokeNode";
	stateSound[1]					= "chainsaw_idle_sound";

	stateName[2]                    = "Loop";
	stateTimeoutValue[2]            = 0.1;
	stateTransitionOnTimeout[2]     = "Ready";
};

datablock ShapeBaseImageData(ChainsawSawingImage : ChainsawImage)
{
	stateName[0]                    = "Fire";
	stateTransitionOnTimeout[0]     = "Fire";
	stateTimeoutValue[0]            = 0.05;
	stateEmitter[0]                 = "ChainsawRevEmitter";
	stateEmitterTime[0]             = 0.05;
	stateEmitterNode[0]             = "smokeNode";
	stateFire[0]                    = true;
	stateSequence[0]                = "Fire";
	stateScript[0]                  = "onFire";

	stateName[1]                    = "Smoke";
	stateTimeoutValue[1]            = 0.01;
	stateTransitionOnTimeout[1]     = "Check";

	stateName[2]                    = "Check";
	stateTimeoutValue[2]            = 0.05;
	stateTransitionOnTimeout[2]     = "Fire";
};

function ChainsawSawingImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(3, plant);
	
	if(!%obj.isSawing)
	{
		%obj.playAudio(1,"chainsaw_sawing_sound");
		%obj.isSawing = true;
	}	
	Parent::onFire(%this, %obj, %slot);
}

function ChainsawSawingImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, plant);
	
	if(!%obj.isSawing)
	{
		%obj.playAudio(1,"chainsaw_sawing_sound");
		%obj.isSawing = true;
	}	
	Parent::onFire(%this, %obj, %slot);
}

function ChainsawSawingImage::onUnmount(%this, %obj, %slot)
{	
	if(%obj.isSawing) 
	{
		%obj.isSawing = false;
		%obj.playthread(1,"root");
		%obj.stopAudio(1);
	}
		
	Parent::onUnmount(%this, %obj, %slot);
}

function ChainsawSawingImage::onMount(%this, %obj, %slot)
{	
	%obj.playthread(1,"armReadyRight");
		
	Parent::onMount(%this, %obj, %slot);
}
