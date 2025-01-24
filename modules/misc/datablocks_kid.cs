datablock ParticleData (KidBinaryParticle0)
{
	dragCoefficient = 0.1;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	useInvAlpha = 0;
	textureName = "./particles/binary0";
	colors[0] = "0.6 0.0 0.0 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";
	sizes[0] = 0.4;
	sizes[1] = 0.2;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData (KidBinaryEmitter0)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	ejectionOffset = 0.75;
	velocityVariance = 0.49;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = "KidBinaryParticle0 KidBinaryParticle1";
	uiName = "Kid Binary 0";
};
datablock ShapeBaseImageData (KidBinaryImage0)
{
	shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   mountPoint = 5;
   offset = "0 0 0";
   eyeOffset = 0;
   rotation = "0 0 0";

   correctMuzzleVector = true;

   className = "WeaponImage";

   item = "";
   ammo = " ";
   projectile = "";
   projectileType = Projectile;

   melee = false;
   armReady = false;

   doColorShift = false;
   colorShiftColor = "1 1 1 1";

	stateName[0]               = "Wait";
	stateTimeoutValue[0]       = 1;
	stateEmitter[0]            = KidBinaryEmitter0;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
    stateSound[0]               = "renowned_Possessed2_sound";
};
datablock ParticleData (KidBinaryParticle1)
{
	dragCoefficient = 0.1;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;
	useInvAlpha = 0;
	textureName = "./particles/binary1";
	colors[0] = "0.6 0.0 0.0 0.0";
	colors[1] = "1   1   0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";
	sizes[0] = 0.4;
	sizes[1] = 0.2;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};
datablock ParticleEmitterData (KidBinaryEmitter1)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	ejectionOffset = 0.75;
	velocityVariance = 0.49;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = KidBinaryParticle1;
	uiName = "Kid Binary 1";
};
datablock ShapeBaseImageData (KidBinaryImage1)
{
	shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   mountPoint = 5;
   offset = "0 0 0";
   eyeOffset = 0;
   rotation = "0 0 0";

   correctMuzzleVector = true;

   className = "WeaponImage";

   item = "";
   ammo = " ";
   projectile = "";
   projectileType = Projectile;

   melee = false;
   armReady = false;

   doColorShift = false;
   colorShiftColor = "1 1 1 1";

	stateName[0]               = "Wait";
	stateTimeoutValue[0]       = 1;
	stateEmitter[0]            = KidBinaryEmitter1;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
    stateSound[0]               = "kid_trapReady_sound";
};

datablock ExplosionData(KidBinaryExplosion)
{
	soundProfile = radioWaveExplosionSound;

	lifeTimeMS = 150;

	particleEmitter = KidBinaryEmitter0;
	particleDensity = 10;
	particleRadius = 0.2;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	// Dynamic light
	lightStartRadius = 1;
	lightEndRadius = 0;
	lightStartColor = "0 1 0.5";
	lightEndColor = "0 0 0";
};