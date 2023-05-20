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