//Reference particle
datablock ParticleData(SkullwolfHitParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "./particles/baseClaw";
	
	useInvAlpha = false;

	colors[0]	= "1 0.9 0.8 1";
	colors[1]	= "1 0.7 0.6 1";
	colors[2]	= "1 0.3 0.2 0";

	sizes[0]	= 2.5;
	sizes[1]	= 2.5;
	sizes[2]	= 2.5;
	
	times[0]	= 0;
	times[1]	= 0.5;
	times[2]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 5;
	gravityCoefficient = 0;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 0;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(SkullwolfHitEmitter)
{	
	particles = "SkullwolfHitParticle";	
	overrideAdvance = false;
	useEmitterColors = false;	
	orientParticles = true;
	orientOnVelocity = true;
	
	ejectionPeriodMS = 76;
	periodVarianceMS = 0;	
	ejectionVelocity = -5;
	velocityVariance = 0;	
	ejectionOffset = 3;
	
	thetaMin = 160;
	thetaMax = 180;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "";
};
datablock ExplosionData(SkullwolfHitExplosion)
{	
	particleEmitter = SkullwolfHitEmitter;
	particleDensity = 1;
	particleRadius = 0;

	lifeTimeMS = 150;	
	shakeCamera = false;
};
datablock ProjectileData(SkullwolfHitProjectile)
{	
	explosion = SkullwolfHitExplosion;
	explodeOnDeath = true;	
	uiName = "";
};

//Generic particle
datablock ParticleData(KillerHitParticle : SkullwolfHitParticle)
{
	textureName = "./particles/baseClaw";
};
datablock ParticleEmitterData(KillerHitEmitter : SkullwolfHitEmitter)
{	
	particles = KillerHitParticle;	
};
datablock ExplosionData(KillerHitExplosion : SkullwolfHitExplosion)
{	
	particleEmitter = KillerHitEmitter;
};
datablock ProjectileData(KillerHitProjectile : SkullwolfHitProjectile)
{	
	explosion = KillerHitExplosion;
};
