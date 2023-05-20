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