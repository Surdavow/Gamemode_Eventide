datablock ParticleData(DarkAmbientParticle)
{
	dragCoefficient = 1.75;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 500;
	textureName = "base/data/particles/dot";
	spinSpeed = 0;
	spinRandomMin = -300;
	spinRandomMax = 300;
	useInvAlpha = true;

	colors[0] = "0.75 0.53 0.88 .75";
	colors[1] = "0.5 0.33 0.68 0.25";
	sizes[0] = 0.2;
	sizes[1] = 0.4;
};
datablock ParticleEmitterData(DarkAmbientEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	velocityVariance = 1;
	ejectionOffset = 0.0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = DarkAmbientParticle;

	uiName = "Darkness - Ambient";
};

datablock ParticleData(DarkBlindParticle)
{
	dragCoefficient = 1;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 1;
	constantAcceleration = 0;
	lifetimeMS = 115;
	lifetimeVarianceMS = 15;
	textureName = "base/data/particles/dot";
	spinSpeed = 0;
	spinRandomMin = -100;
	spinRandomMax = 100;
	useInvAlpha = true;

	colors[0] = "0.5 0.33 0.68 0.95";
	colors[1] = "0.5 0.33 0.68 0.25";
	sizes[0] = 1.5;
	sizes[1] = 1;
};
datablock ParticleEmitterData(DarkBlindEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 3;
	velocityVariance = 2.5;
	ejectionOffset = 0.3;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = DarkBlindParticle;
};

datablock ParticleData(RenownedAmbientParticle)
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

	colors[0] = "1 1 0.7 .75";
	colors[1] = "1 1 0.7 0.25";
	colors[3] = "1 1 0.7 0";
	sizes[0] = 0.2;
	sizes[1] = 0.4;
	sizes[2] = 0.6;
};
datablock ParticleEmitterData(RenownedAmbientEmitter)
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
	particles = RenownedAmbientParticle;

	uiName = "";
};

datablock ParticleData(GlowFaceParticle) {
	textureName				= "./glowFace";
	lifetimeMS				= 500;
	lifetimeVarianceMS		= 0;
	dragCoefficient			= 0.0;
	windCoefficient			= 0.0;
	gravityCoefficient		= 0.0;
	inheritedVelFactor		= 0.0;
	constantAcceleration	= 0.0;
	spinRandomMin			= 0.0;
	spinRandomMax			= 0.0;
	colors[0]				= "1.0 1.0 1.0 1.0";
	colors[1]				= "0.1 0.1 0.1 0.1";
	colors[2]				= "0.0 0.0 0.0 0.0";
	sizes[0]				= 0.7;
	sizes[1]				= 0.7;
	sizes[2]				= 0.7;
	times[0]				= 0;
	times[1]				= 0.5;
	times[2]				= 1.0;
	useInvAlpha				= false;
};

datablock ParticleEmitterData(GlowFaceEmitter) {
	uiName				= "Glow Face Emitter";
	particles			= "GlowFaceParticle";
	ejectionPeriodMS	= 10;
	periodVarianceMS	= 0;
	ejectionVelocity	= 0.0;
	velocityVariance	= 0.0;
	ejectionOffset		= 0.4;
	thetaMin			= 0.0;
	thetaMax			= 0.0;
	phiReferenceVel		= 0.0;
	phiVariance			= 0.0;
};

datablock ExplosionData(DarkMExplosion)
{
	lifeTimeMS = 250;

	particleEmitter = DarkAmbientEmitter;
	particleDensity = 75;
	particleRadius = 1;

	emitter[0] = DarkAmbientEmitter;

	faceViewer = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "30 30 30";
	camShakeAmp = "7 2 7";
	camShakeDuration = 0.6;
	camShakeRadius = 2.5;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "1 1 1";
	lightEndColor = "1 1 1";

	uiName = "";
};

AddDamageType("DarkM", ' %1', '%2 %1', 1, 1);

datablock ProjectileData(DarknessProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";
	directDamage = 0;
	directDamageType = $DamageType::DarkM;

	impactImpulse = 10;
	verticalImpulse = 25;
	explosion = DarkMExplosion;
	particleEmitter = DarkAmbientEmitter;
	sound = "";

	muzzleVelocity = 70;
	velInheritFactor = 0;

	armingDelay = 0;
	lifetime = 4000;
	fadeDelay = 4000;
	bounceElasticity = 0.5;
	bounceFriction = 0.5;
	isBallistic = false;

	hasLight = false;
	lightRadius = 1;
	lightColor = "0 0 0";

	uiName = "";
};

datablock ProjectileData(AnglerHookProjectile)
{
	projectileShapeName = "./models/anglerhookproj.dts";
	directDamage = 0;
	directDamageType = $DamageType::DarkM;

	impactImpulse = 10;
	verticalImpulse = 25;
	explosion = "";
	particleEmitter = "";
	sound = "angler_hookCast_sound";

	muzzleVelocity = 70;
	velInheritFactor = 0;

	armingDelay = 0;
	lifetime = 1000;
	fadeDelay = 1000;
	bounceElasticity = 0.5;
	bounceFriction = 0.5;
	isBallistic = false;

	hasLight = false;
	lightRadius = 1;
	lightColor = "0 0 0";
	gravityMod 	= true;

	uiName = "";
};

datablock ShapeBaseImageData(AnglerHookImage)
{
   shapeFile = "./models/anglerhook.dts";
   emap = true;

   mountPoint = 1;
   offset = "0 0 0";
   eyeOffset = 0;
   rotation = "0 0 0";

   correctMuzzleVector = true;

   className = "WeaponImage";

   item = "";
   ammo = " ";
   projectile = "AnglerHookProjectile";
   projectileType = Projectile;

   melee = false;
   armReady = false;

   doColorShift = false;
   colorShiftColor = "0.5 0.5 0.5 1";

	stateName[0]               = "Wait";
	stateTimeoutValue[0]       = 1;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
    stateSound[0]               = "angler_chainReturn_sound";
};

datablock ShapeBaseImageData(DarkCastImage)
{
   shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   mountPoint = 1;
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
	stateEmitter[0]            = DarkAmbientEmitter;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
    stateSound[0]               = "shire_charged_sound";
};

datablock ShapeBaseImageData(DarkBlindPlayerImage)
{
   shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   mountPoint = $Headslot;
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
	stateTimeoutValue[0]       = 5;
	stateEmitter[0]            = DarkBlindEmitter;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Dismount";

	stateName[1]               = "Dismount";
	stateScript[1]             = "Dismount";
};

datablock ShapeBaseImageData(RenownedCastImage)
{
   shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   mountPoint = 1;
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
	stateEmitter[0]            = RenownedAmbientEmitter;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
    stateSound[0]               = "renowned_Charged_sound";
};

datablock ShapeBaseImageData(RenownedPossessedImage)
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
	stateEmitter[0]            = RenownedAmbientEmitter;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
    stateSound[0]               = "renowned_Possessed_sound";
};

datablock ShapeBaseImageData(GlowFaceImage) 
{
	shapeFile			= "base/data/shapes/empty.dts";
	mountPoint			= 6;
	correctMuzzleVector	= false;
	stateName[0]				= "Glow";
	stateEmitter[0]				= GlowFaceEmitter;
	stateEmitterTime[0]			= 1000;
	stateWaitForTimeout[0]		= true;
	stateTimeoutValue[0]		= 1000;
	stateTransitionOnTimeout[0]	= "Glow";
	stateScript[0]				= "onGlow";
};

datablock StaticShapeData(AnglerHookRope)
{
	shapeFile = "./models/hookrope.dts";
	isHookRope = true;
};