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
	offset = "0 0 0";
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