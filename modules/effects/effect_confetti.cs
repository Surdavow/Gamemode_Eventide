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