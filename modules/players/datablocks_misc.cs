if(isFile("add-ons/Gamemode_Eventide/modules/players/models/face.ifl"))//Faces
{
	%write = new FileObject();
	%write.openForWrite("add-ons/Gamemode_Eventide/modules/players/models/face.ifl");
	%write.writeLine("base/data/shapes/player/faces/smiley.png");
	%write.writeLine("Add-Ons/Face_Default/smileyRedBeard2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyRedBeard.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate3.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyOld.png");
	%write.writeLine("Add-Ons/Face_Default/smileyFemale1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyEvil2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyEvil1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyCreepy.png");
	%write.writeLine("Add-Ons/Face_Default/smileyBlonde.png");
	%write.writeLine("Add-Ons/Face_Default/memeYaranika.png");
	%write.writeLine("Add-Ons/Face_Default/memePBear.png");
	%write.writeLine("Add-Ons/Face_Default/memeHappy.png");
	%write.writeLine("Add-Ons/Face_Default/memeGrinMan.png");
	%write.writeLine("Add-Ons/Face_Default/memeDesu.png");
	%write.writeLine("Add-Ons/Face_Default/memeCats.png");
	%write.writeLine("Add-Ons/Face_Default/memeBlockMongler.png");
	%write.writeLine("Add-Ons/Face_Default/asciiTerror.png");
	
	%decalpath = "add-ons/Gamemode_Eventide/modules/players/models/faces/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		eval("addExtraResource(\""@ %decalfile @ "\");");
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource("add-ons/Gamemode_Eventide/modules/players/models/face.ifl");
}

if(isFile("add-ons/Gamemode_Eventide/modules/players/models/decal.ifl"))//Decals
{
	%write = new FileObject();
	%write.openForWrite("add-ons/Gamemode_Eventide/modules/players/models/decal.ifl");
	%write.writeLine("base/data/shapes/players/decals/AAA-none.png");
	%write.writeLine("Add-Ons/Decal_WORM/worm_engineer.png");
	%write.writeLine("Add-Ons/Decal_WORM/worm-sweater.png");
	%write.writeLine("Add-Ons/Decal_PlayerFitNE/zhwindnike.png");
	%write.writeLine("Add-Ons/Decal_Jirue/LinkTunic.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Knight.png");
	%write.writeLine("Add-Ons/Decal_Jirue/HCZombie.png");
	%write.writeLine("Add-Ons/Decal_Jirue/DrKleiner.png");
	%write.writeLine("Add-Ons/Decal_Jirue/DKnight.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Chef.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Archer.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Alyx.png");
	%write.writeLine("Add-Ons/Decal_Hoodie/Hoodie.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-Old.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-New.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-Nasa.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Suit.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Prisoner.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Police.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Pilot.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-DareDevil.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Army.png");
	%write.writeLine("Add-Ons/Decal_Default/Meme-Mongler.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-YARLY.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Tunic.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Rider.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-ORLY.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Lion.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Eagle.png");
	
	%decalpath = "add-ons/Gamemode_Eventide/modules/players/models/decals/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		eval("addExtraResource(\""@ %decalfile @ "\");");
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource("add-ons/Gamemode_Eventide/modules/players/models/decal.ifl");
}

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
	textureName				= "./models/glowFace";
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

datablock ParticleData(GlowFaceZombieParticle) 
{
	textureName				= "./models/glowFaceZombie";
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

datablock ParticleEmitterData(GlowFaceZombieEmitter) {
	uiName				= "";
	particles			= "GlowFaceZombieParticle";
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

datablock ParticleData(ZombieBodyParticle) 
{
	textureName				= "./models/ZombieBody";
	lifetimeMS				= 500;
	lifetimeVarianceMS		= 0;
	dragCoefficient			= 0.0;
	windCoefficient			= 0.0;
	gravityCoefficient		= 0.0;
	inheritedVelFactor		= 0.0;
	constantAcceleration	= 0.0;
	spinRandomMin			= 0.0;
	spinRandomMax			= 0.0;
	colors[0]				= "0.41 0.1 0.61 0.1";
	colors[1]				= "0.41 0.1 0.61 0.0";
	colors[2]				= "0.41 0.1 0.61 0.0";
	sizes[0]				= 2.6;
	sizes[1]				= 2.6;
	sizes[2]				= 2.6;
	times[0]				= 0;
	times[1]				= 0.5;
	times[2]				= 1.0;
	useInvAlpha				= false;
};

datablock ParticleEmitterData(ZombieBodyEmitter) {
	uiName				= "";
	particles			= "ZombieBodyParticle";
	ejectionPeriodMS	= 10;
	periodVarianceMS	= 0;
	ejectionVelocity	= 0.0;
	velocityVariance	= 0.0;
	ejectionOffset		= 0.0;
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
	lifetime = 500;
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

datablock ParticleEmitterData(DarkAmbientZombieEmitter)
{
	ejectionPeriodMS = 5;
	periodVarianceMS = 0;
	ejectionVelocity = 2.5;
	velocityVariance = 1.5;
	ejectionOffset = 1.25;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 180;
	phiVariance = 360;
	overrideAdvance = false;
	particles = DarkAmbientParticle;

	uiName = "Darkness - Ambient";
};

datablock ShapeBaseImageData(DarkCastZombieImage : DarkCastImage)
{
	mountPoint = 2;

	stateName[0]               = "Wait";
	stateTimeoutValue[0]       = 1;
	stateEmitter[0]            = DarkAmbientZombieEmitter;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
    stateSound[0]               = "shire_charged_sound";	
};

datablock ShapeBaseImageData(DarkCastZombieHandRImage : DarkCastImage)
{
	mountPoint = 0;
};
datablock ShapeBaseImageData(DarkCastZombieHandLImage : DarkCastImage)
{
	mountPoint = 0;
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

	stateName[0]               = "Blind";
	stateTimeoutValue[0]       = 1;
	stateEmitter[0]            = "DarkBlindEmitter";
	stateEmitterTime[0]        = 1;
	stateTransitionOnTimeout[0]= "Wait";
	stateScript[0]             = "onBlind";

	stateName[1]               = "Wait";
	stateTransitionOnTimeout[1]= "Blind";
	stateTimeoutValue[1]       = 0.1;
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

datablock ShapeBaseImageData(GlowFaceZombieImage) 
{
	shapeFile			= "base/data/shapes/empty.dts";
	mountPoint			= 6;
	correctMuzzleVector	= false;
	stateName[0]				= "Glow";
	stateEmitter[0]				= GlowFaceZombieEmitter;
	stateEmitterTime[0]			= 1000;
	stateWaitForTimeout[0]		= true;
	stateTimeoutValue[0]		= 1000;
	stateTransitionOnTimeout[0]	= "Glow";
	stateScript[0]				= "onGlow";
};

datablock ShapeBaseImageData(ZombieBodyImage) 
{
	shapeFile			= "base/data/shapes/empty.dts";
	mountPoint			= 2;
	offset = "0 0 -0.55";
	correctMuzzleVector	= false;
	stateName[0]				= "Glow";
	stateEmitter[0]				= ZombieBodyEmitter;
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

datablock fxLightData(NegativePlayerLight)
{
	uiName = "Player\'s Negative Light";
	LightOn = 1;
	radius = 15;
	Brightness = -5;
	color = "1 1 1 1";
	FlareOn = 0;
	FlareTP = 1;
	FlareBitmap = "base/lighting/corona";
	FlareColor = "1 1 1";
	ConstantSizeOn = 1;
	ConstantSize = 1;
	NearSize = 3;
	FarSize = 0.5;
	NearDistance = 10;
	FarDistance = 30;
	FadeTime = 0.1;
	BlendMode = 0;
};

datablock fxLightData(NoFlareGLight)
{
	uiName = "No Flare Green";
	LightOn = true;
	radius = 15;
	brightness = 5;
	color = "0.1 1 0.1";
	FlareOn			= false;
	FlareTP			= false;
	Flarebitmap		= "";
	FlareColor		= "1 1 1";
	ConstantSizeOn	= false;
	ConstantSize	= 1;
	NearSize		= 1;
	FarSize			= 0.5;
	NearDistance	= 10.0;
	FarDistance		= 30.0;
	FadeTime		= 0.1;
};
datablock fxLightData(NoFlareRLight : NoFlareGLight)
{
	uiName = "No Flare Red";
	color = "1 0.1 0.1";
};
datablock fxLightData(NoFlarePLight : NoFlareGLight)
{
	uiName = "No Flare Purple";
	color = "1 0.05 0.5";
};
datablock fxLightData(NoFlareYLight : NoFlareGLight)
{
	uiName = "No Flare Yellow";
	color = "1 1 0.1";
};
datablock fxLightData(NoFlareBLight : NoFlareGLight)
{
	uiName = "No Flare Blue";
	color = "0.1 0.1 1";
};