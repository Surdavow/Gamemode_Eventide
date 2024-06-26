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
    stateSound[0]               = "renowned_Possessed2_sound";
};