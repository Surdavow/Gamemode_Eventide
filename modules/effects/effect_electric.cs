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