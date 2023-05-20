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
	ejectionVelocity = 0;
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