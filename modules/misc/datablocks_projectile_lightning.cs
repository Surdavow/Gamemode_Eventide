//explosion
datablock ParticleData(LightningArcParticle)
{
	dragCoefficient      = 5;
	gravityCoefficient   = 0.0;
	inheritedVelFactor   = 1.0;
	constantAcceleration = 0.0;
	lifetimeMS           = 250;
	lifetimeVarianceMS   = 0;
	textureName          = "./particles/lightningarc";
	spinSpeed		= 0.0;
	spinRandomMin		= -0.0;
	spinRandomMax		= 0.0;
	colors[0]     = "1 1 1 1";
   colors[1]     = "1 1 1 1";
	colors[2]     = "1 1 1 0";
	sizes[0]      = 1;
   sizes[1]      = 1;
	sizes[2]      = 0.8;
   times[0] = 0;
   times[1] = 0.1;
   times[2] = 0.2;
};

datablock ParticleEmitterData(LightningArcEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   ejectionVelocity = 0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.5;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "LightningArcParticle";

   useEmitterColors = true;

   uiName = "Lightning Arc Explosion";
};

datablock ExplosionData(LightningArcExplosion)
{
   //explosionShape = "";

   lifeTimeMS = 150;

   particleEmitter = LightningArcEmitter;
   particleDensity = 1;
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
   lightStartColor = "1 1 1";
   lightEndColor = "0 0 0";
};


//trail
datablock ParticleData(LightningArcTrailParticle)
 {
   dragCoefficient      = 3;
   gravityCoefficient   = -0.0;
   inheritedVelFactor   = 1.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 125;
   lifetimeVarianceMS   = 0;
   textureName          = "./particles/lightningarc";
   spinSpeed		   = 0.0;
   spinRandomMin		= 0.0;
   spinRandomMax		= 0.0;
   colors[0]     = "1 1 1 0.9";
   colors[1]     = "1 1 1 0.5";
   colors[2]     = "1 1 1 0";

   sizes[0]      = 0.5;
   sizes[1]      = 0.5;
   sizes[2]      = 0.5;

   times[0] = 0.0;
   times[1] = 0.1;
   times[2] = 0.2;

   useInvAlpha = false;
};
datablock ParticleEmitterData(LightningArcTrailEmitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 1;
   ejectionVelocity = 0.25;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "LightningArcTrailParticle";

   useEmitterColors = true;

   uiName = "Lightning Arc Trail";
};

datablock ProjectileData(LightningArcProjectile)
{
   projectileShapeName = "base/data/shapes/empty.dts";
   explosion           = LightningArcExplosion;
   particleEmitter     = LightningArcTrailEmitter;
   explodeOnDeath = true;

   brickExplosionRadius = 0;
   brickExplosionImpact = 0;
   brickExplosionForce  = 0;
   brickExplosionMaxVolume = 0;
   brickExplosionMaxVolumeFloating = 0;

   collideWithPlayers = false;

   muzzleVelocity      = 65;
   velInheritFactor    = 1.0;

   armingDelay         = 0;
   lifetime            = 30000;
   fadeDelay           = 29500;
   bounceElasticity    = 0.99;
   bounceFriction      = 0.00;
   isBallistic         = true;
   gravityMod          = 0.0;

   hasLight    = true;
   lightRadius = 1.0;
   lightColor  = "1.0 1.0 1.0";

   uiName = "Lightning Arc"; 
};