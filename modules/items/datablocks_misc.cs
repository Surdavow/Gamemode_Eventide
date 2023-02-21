datablock fxLightData(CandleLight)
{
	uiName = "Candle Light";

	LightOn = true;
	radius = 7.5;
	brightness = 1;
	color = "1 0.75 0 1";

	FlareOn			= true;
    FlareBitmap = "./models/candleLightFlare";
    FlareColor = "0.25 0.2 0";
	AnimColor		= false;
	AnimBrightness	= true;
	AnimOffsets		= true;
	AnimRotation	= false;
	LinkFlare		= false;
	LinkFlareSize	= true;
	MinColor		= "1 1 0";
	MaxColor		= "0 0 1";
	MinBrightness	= 0.0;
	MaxBrightness	= 5.0;
	MinRadius		= 0.1;
	MaxRadius		= 10;
	StartOffset		= "0 0 0";
	EndOffset		= "0 0 0";
	MinRotation		= 0;
	MaxRotation		= 359;

	SingleColorKeys	= false;
	RedKeys			= "AWTCFAH";
	GreenKeys		= "AWTCFAH";
	BlueKeys		= "AWTCFAH";
	
	BrightnessKeys	= "DEDEDFGF";
	RadiusKeys		= "AZAAAAA";
	OffsetKeys		= "AZAAAAA";
	RotationKeys	= "AZAAAAA";

	ColorTime		= 1.0;
	BrightnessTime	= 1.0;
	RadiusTime		= 1.0;
	OffsetTime		= 1.0;
	RotationTime	= 1.0;

	LerpColor		= true;
	LerpBrightness	= true;
	LerpRadius		= true;
	LerpOffset		= false;
	LerpRotation	= false;
};

datablock PlayerData(EmptyCandleBot : PlayerStandardArmor)
{
	shapeFile = "base/data/shapes/empty.dts";
	uiName = "";
	className = PlayerData;
};

datablock PlayerData(EmptyLightBot : PlayerStandardArmor)
{
	shapeFile = "base/data/shapes/empty.dts";
	uiName = "";
	className = PlayerData;
};

datablock PlayerData(CandleItemProp : PlayerStandardArmor)
{
	shapeFile = "./models/candle.dts";
	uiName = "";
    renderFirstPerson = false;
	className = PlayerData;
};

datablock ParticleData(daggerFlashParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 80;
	lifetimeVarianceMS   = 15;
	textureName          = "base/data/particles/star1";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.6 0.6 0.1 0.9";
	colors[1]     = "0.6 0.6 0.6 0.0";
	sizes[0]      = 1.0;
	sizes[1]      = 2.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(daggerFlashEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 1.0;
	velocityVariance = 1.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 90;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = daggerFlashParticle;
};

datablock ExplosionData(daggerExplosion)
{
	soundProfile = "";
	lifeTimeMS = 150;

	particleDensity = 5;
	particleRadius = 0.2;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeDuration = 1;
	camShakeRadius = 10.0;

	camShakeFreq = "3 3 3";
	camShakeAmp = "0.6 0.6 0.6";
	particleEmitter = daggerFlashEmitter;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";
};
datablock ProjectileData(daggerProjectile)
{
	explosion = daggerExplosion;
};