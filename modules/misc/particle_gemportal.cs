datablock ParticleData(GemPortalParticle)
{
	dragCoefficient      = 10;
	gravityCoefficient   = 0.0;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 0.0;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 200;
	textureName          = "base/data/particles/dot";
	spinSpeed		= 20.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "1 1 1 0.2";
	colors[1]     = "1 1 1 0.1";
	colors[2]     = "1 1 1 0.5";
	colors[3]     = "0 0 0 0.0";
	sizes[0]      = 1.84;
	sizes[1]      = 1.07;
	sizes[2]      = 0.8;
	sizes[3]      = 1.03;
	windCoefficient = 0.0;

	times[0]      = 0.0;
	times[1]      = 0.2;
	times[2]      = 0.8;
	times[3]      = 1.0;

	useInvAlpha = true;
};

datablock ParticleEmitterData(GemPortalEmitter)
{
   ejectionPeriodMS = 100;
   periodVarianceMS = 2;
   ejectionVelocity = 0.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "GemPortalParticle";

    uiName = "Gem Portal";
};