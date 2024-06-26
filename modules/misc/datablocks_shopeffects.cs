//No longer using these.
return;

datablock ParticleData(ConfettiParticle1)
{
	dragCoefficient		= 0;
	windCoefficient		= 2.5;
	gravityCoefficient	= 0.5;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -180.0;
	spinRandomMax		= 270.0;
	useInvAlpha		= true;
	textureName          = "base/data/particles/chunk";

   	colors[0]     = "1 0 0 1";
  	colors[1]     = "0 0 1 0.75";
  	colors[2]     = "0 1 0 0.5";
  	colors[3]     = "0 1 1 0.25";

   	sizes[0]     = "0.075";
  	sizes[1]     = "0.075";
  	sizes[2]     = "0.075";
  	sizes[3]     = "0.075";

  	times[0]      = 0.0;
   	times[1]      = 0.25;
   	times[2]      = 0.5;
   	times[3]      = 0.75;
};
datablock ParticleEmitterData(ConfettiEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 1;
   ejectionVelocity = 2;
   velocityVariance = 1;
   ejectionOffset   = 0.75;
   thetaMin         = 0.0;
   thetaMax         = 180.0;  
   particles        = "ConfettiParticle1";
   useEmitterColors = false;
   uiName = "Confetti";
};

datablock ShapeBaseImageData(ConfettiStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "ConfettiEmitter";
	stateTransitionOnTimeout[0]    = "Main";
};


datablock ParticleData(ElectricBolt1Particle)
{
	dragCoefficient		= 0;
	windCoefficient		= 1;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 50;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 90;
	spinRandomMin		= 0;
	spinRandomMax		= 360;
	useInvAlpha		= false;
	textureName          = "./texture/bolt1";
	colors[0] = "0 0.25 1 1";
	colors[1] = "0 0.25 1 0.5";
	colors[2] = "0 0.25 1 0";
	sizes[0] = 0;
	sizes[1] = 0.25;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 0.2;
};
datablock ParticleEmitterData(ElectricBolt1Emitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 10;
   ejectionVelocity = 0;
   velocityVariance = 0;
   ejectionOffset   = 0.75;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   particles        = "ElectricBolt1Particle";
   useEmitterColors = true;
   uiName = "Electric 1 Bolt";
};

datablock ParticleData(ElectricBolt2Particle)
{
	dragCoefficient		= 0;
	windCoefficient		= 1;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 50;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 90;
	spinRandomMin		= 0;
	spinRandomMax		= 360;
	useInvAlpha		= false;
	textureName          = "./texture/bolt2";
	colors[0] = "0 0.25 1 1";
	colors[1] = "0 0.25 1 0.5";
	colors[2] = "0 0.25 1 0";
	sizes[0] = 0;
	sizes[1] = 0.25;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 0.2;
};
datablock ParticleEmitterData(ElectricBolt2Emitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 10;
   ejectionVelocity = 0;
   velocityVariance = 0;
   ejectionOffset   = 1.25;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   particles        = "ElectricBolt2Particle";
   useEmitterColors = true;
   uiName = "Electric 2 Bolt";
};

datablock ShapeBaseImageData(ElectricStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1.5";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Bolt1";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "ElectricBolt1Emitter";
	stateTransitionOnTimeout[0]    = "Bolt2";

	stateName[1]                   = "Bolt2";
	stateTimeoutValue[1]           = 0.1;
	stateEmitterTime[1]            = 0.1;
	stateEmitter[1]                = "ElectricBolt2Emitter";
	stateTransitionOnTimeout[1]    = "Bolt1";
};


datablock ParticleData(FireStatusParticle)
{
	textureName          = "base/data/particles/cloud";
	dragCoefficient      = 1;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.0;
	windCoefficient      = 2.5;
	constantAcceleration = 1;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 500;
	spinSpeed     = 0;
	spinRandomMin = -30.0;
	spinRandomMax =  30.0;
	useInvAlpha   = false;

	colors[0]	= "1 1 0.5 0.5";
	colors[1]	= "1 0.5 0.25 0.25";
	colors[2]	= "1 0.8 0.25 0.0";

	sizes[0]	= 0.25;
	sizes[1]	= 0.75;
	sizes[2]	= 1.25;

	times[0]	= 0.0;
	times[1]	= 0.5;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(FireStatusEmitter)
{
  	ejectionPeriodMS = 5;
  	periodVarianceMS = 4;
  	ejectionVelocity = 0.5;
  	ejectionOffset   = 0.5;
  	velocityVariance = 0.1;
  	thetaMin         = 0;
  	thetaMax         = 180;
  	phiReferenceVel  = 0;
  	phiVariance      = 360;
  	overrideAdvance = false;
  	particles = "FireStatusParticle";      
   	useEmitterColors = true;
	uiName = "Fire Status Particle";
};

datablock ShapeBaseImageData(FireStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1.25";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;	
	
	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "FireStatusEmitter";	
	stateTransitionOnTimeout[0]    = "Main";
};


datablock ParticleData(HeartStatusParticle)
{
	dragCoefficient		= 0;
	windCoefficient		= 2.5;
	gravityCoefficient	= -0.125;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1500;
	lifetimeVarianceMS	= 1000;
	spinSpeed		= 25.0;
	spinRandomMin		= -180.0;
	spinRandomMax		= 270.0;
	useInvAlpha		= true;
	textureName          = "base/data/particles/heart";

   	colors[0]     = "1 0 0 0";
  	colors[1]     = "1 0 0 1";
  	colors[2]     = "1 0 0 0.75";
  	colors[3]     = "1 0 0 0";

   	sizes[0]     = "0.2";
  	sizes[1]     = "0.15";
  	sizes[2]     = "0.15";
  	sizes[3]     = "0.01";

  	times[0]      = 0.05;
   	times[1]      = 0.15;
   	times[2]      = 0.4;
   	times[3]      = 0.6;
};
datablock ParticleEmitterData(HeartStatusEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 25;
   ejectionVelocity = 1;
   velocityVariance = 0;
   ejectionOffset   = 1.25;
   thetaMin         = 0.0;
   thetaMax         = 180.0;  
   particles        = "HeartStatusParticle";
   useEmitterColors = false;
   uiName = "Heart Status";
};

datablock ShapeBaseImageData(HeartStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "HeartStatusEmitter";
	stateTransitionOnTimeout[0]    = "Main";
};


datablock ParticleData (NeonFlameStatusParticle)
{
	textureName = "base/data/particles/cloud";
	dragCoefficient      = 1;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.0;
	windCoefficient      = 2.5;
	constantAcceleration = 1;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 750;
	spinSpeed     = 0;
	spinRandomMin = -30.0;
	spinRandomMax =  30.0;
	useInvAlpha   = false;
	colors[0] = "0.25 0.8 0.25 0.25";
	colors[1] = "0.25 0.8 0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";
	sizes[0] = 0.75;
	sizes[1] = 0.375;
	sizes[2] = 0.1;
	times[0] = 0;
	times[1] = 0.75;
	times[2] = 1;
};
datablock ParticleEmitterData (NeonFlameStatusEmitter)
{
  	ejectionPeriodMS = 3;
  	periodVarianceMS = 2;
  	ejectionVelocity = 0.25;
  	ejectionOffset   = 0.375;
  	velocityVariance = 0.15;
  	thetaMin         = 0;
  	thetaMax         = 180;
  	phiReferenceVel  = 0;
  	phiVariance      = 360;
  	overrideAdvance = false;
	particles = NeonFlameStatusParticle;
	doFalloff = 0;
	doDetail = 0;
	uiName = "Neon Flame Status";
};

datablock ShapeBaseImageData(NeonFlameStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1.25";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "NeonFlameStatusEmitter";
	stateTransitionOnTimeout[0]    = "Main";
};


datablock ParticleData(SparkleStatusParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 250;
	lifetimeVarianceMS = 1;
	textureName = "base/lighting/flare";
	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
	colors[0] = "0 1 0.5 1";
	colors[1] = "0 1 0.5 1";
	colors[2] = "0 1 0.5 1";
	colors[3] = "0 1 0.5 1";
	sizes[0] = 0;
	sizes[1] = 2;
	sizes[2] = 0.5;
	sizes[3] = 0;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 0.3;
	times[3] = 1;
	useInvAlpha = false;
};

datablock ParticleEmitterData(SparkleStatusEmitter)
{
	uiName = "Sparkle Status";
	ejectionPeriodMS = 50;
	periodVarianceMS = 25;
	ejectionVelocity = 1;
	ejectionOffset = 1;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	useEmitterColors = true;
	particles = "SparkleStatusParticle";
};

datablock ParticleData(SparkleStatusParticle2: SparkleStatusParticle)
{
	textureName = "./texture/flare2";
};

datablock ParticleEmitterData(SparkleStatusEmitter2 : SparkleStatusEmitter)
{
	uiName = "Sparkle Status 2";
	particles = "SparkleStatusParticle2";
};

datablock ShapeBaseImageData(SparkleStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1.375";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "SparkleStatusEmitter";
	stateTransitionOnTimeout[0]    = "Secondary";

	stateName[1]                   = "Secondary";
	stateTimeoutValue[1]           = 0.1;
	stateEmitterTime[1]            = 0.1;
	stateEmitter[1]                = "SparkleStatusEmitter2";
	stateTransitionOnTimeout[1]    = "Main";
};


datablock ParticleData(StinkyFlyParticle)
{
	dragCoefficient		= 0;
	windCoefficient		= 2.5;
	gravityCoefficient	= -0.25;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -180.0;
	spinRandomMax		= 270.0;
	useInvAlpha		= true;
	textureName          = "base/data/particles/chunk";

   	colors[0]     = "0.1 0.1 0.1 0";
  	colors[1]     = "0.1 0.1 0.1 1";
  	colors[2]     = "0.1 0.1 0.1 1";
  	colors[3]     = "0.1 0.1 0.1 0";

   	sizes[0]     = "0.025";
  	sizes[1]     = "0.025";
  	sizes[2]     = "0.025";
  	sizes[3]     = "0.025";

  	times[0]      = 0.0;
   	times[1]      = 0.25;
   	times[2]      = 0.5;
   	times[3]      = 0.75;
};
datablock ParticleEmitterData(StinkyFlyEmitter)
{
   ejectionPeriodMS = 25;
   periodVarianceMS = 20;
   ejectionVelocity = 1;
   velocityVariance = 0.75;
   ejectionOffset   = 1;
   thetaMin         = 0.0;
   thetaMax         = 180.0;  
   particles        = "StinkyFlyParticle";
   useEmitterColors = false;
   uiName = "Stinky Flies";
};

datablock ParticleData(StinkyParticle)
{
	textureName          = "base/data/particles/cloud";
	dragCoefficient      = 0.125;
	gravityCoefficient   = -0.25;
	inheritedVelFactor   = 0.0;
	windCoefficient      = 2.5;
	constantAcceleration = 1;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 500;
	spinSpeed     = 0;
	spinRandomMin = -30.0;
	spinRandomMax =  30.0;
	useInvAlpha   = true;

	colors[0]	= "0.1 1 0.1 0.0";
	colors[1]	= "0.1 1 0.1 0.15";
	colors[2]	= "0.1 1 0.1 0.0";

	sizes[0]	= 0.75;
	sizes[1]	= 0.75;
	sizes[2]	= 1.25;

	times[0]	= 0.0;
	times[1]	= 0.2;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(StinkyEmitter)
{
  	ejectionPeriodMS = 10;
  	periodVarianceMS = 5;
  	ejectionVelocity = 0.25;
  	ejectionOffset   = 0.75;
  	velocityVariance = 0.1;
  	thetaMin         = 0;
  	thetaMax         = 180;
  	phiReferenceVel  = 0;
  	phiVariance      = 360;
  	overrideAdvance = false;
  	particles = "StinkyParticle";      
   	useEmitterColors = true;
	uiName = "Stinky Particle";
};

datablock ShapeBaseImageData(StinkyStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -0.75";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;	
	
	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "StinkyEmitter";	
	stateTransitionOnTimeout[0]    = "Flies";

	stateName[1]                   = "Flies";
	stateTimeoutValue[1]           = 0.05;
	stateEmitterTime[1]            = 0.05;
	stateEmitter[1]                = "StinkyFlyEmitter";	
	stateTransitionOnTimeout[1]    = "Main";
};