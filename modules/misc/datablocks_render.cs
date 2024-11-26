datablock ParticleData(RenderAmbientParticle)
{
	dragCoefficient = 1;
	windCoefficient = 10;
	gravityCoefficient = 0.5;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 500;
	textureName = "base/data/particles/dot";
	spinSpeed = 0;
	spinRandomMin = -300;
	spinRandomMax = 300;
	useInvAlpha = true;

	colors[0] = "0 0 0 .75";
	colors[1] = "0 0 0 0.25";
	colors[3] = "0 0 0 0";
	sizes[0] = 0.2;
	sizes[1] = 0.4;
	sizes[2] = 0.6;
};
datablock ParticleEmitterData(RenderAmbientEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	velocityVariance = 1;
	ejectionOffset = 0.25;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = true;
	particles = RenderAmbientParticle;

	uiName = "";
};

datablock ShapeBaseImageData(RenderTurnImage)
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
	stateEmitter[0]            = RenderAmbientEmitter;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
};

datablock ParticleData(PrepperParticle)
{
   dragCoefficient      = 5.0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.0;
   windCoefficient      = 0;
   constantAcceleration = 0.0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 0;
   useInvAlpha          = false;
   textureName          = "Add-Ons/Brick_Halloween/Prepper";
   colors[0]     = "0.1 0.1 0.1 0.7";
   colors[1]     = "1 0 0 0.8";
   colors[2]     = "1 1 1 0.5";
   sizes[0]      = 1;
   sizes[1]      = 1.5;
   sizes[2]      = 1.3;
   times[0]      = 0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(PrepperEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 0;
   ejectionVelocity = 0.0;
   ejectionOffset   = 1.8;
   velocityVariance = 0.0;
   thetaMin         = 0;
   thetaMax         = 0;
   phiReferenceVel  = 0;
   phiVariance      = 0;
   overrideAdvance = false;
   lifeTimeMS = 100;
   particles = "PrepperParticle";

   doFalloff = true;

   emitterNode = GenericEmitterNode;
   pointEmitterNode = TenthEmitterNode;
};

datablock ShapeBaseImageData(PrepperImage) 
{
	shapeFile			= "base/data/shapes/empty.dts";
	mountPoint			= 5;
	offset 				= "0 0 0";
	correctMuzzleVector	= false;
	stateName[0]				= "Glow";
	stateEmitter[0]				= PrepperEmitter;
	stateEmitterTime[0]			= 1000;
	stateWaitForTimeout[0]		= true;
	stateTimeoutValue[0]		= 1000;
	stateTransitionOnTimeout[0]	= "Glow";
	stateScript[0]				= "onGlow";
};

datablock ExplosionData(PrepperExplosion)
{
   lifeTimeMS = 2000;
   emitter[0] = PrepperEmitter;
};

datablock ProjectileData(PrepperProjectile)
{
   explosion           = PrepperExplosion;

   armingDelay         = 0;
   lifetime            = 10;
   explodeOnDeath		= true;
};