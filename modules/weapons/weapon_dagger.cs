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

datablock ItemData(daggerItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/dagger.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Ceremonial Dagger";
	iconName = "./icons/icon_dagger";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = daggerImage;
	canDrop = true;
};

datablock ShapeBaseImageData(daggerImage)
{
    shapeFile = "./models/dagger.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";
    correctMuzzleVector = false;
    eyeOffset = "0 0 0";
    className = "WeaponImage";

    item = daggerItem;
    ammo = " ";
    projectile = "";
    projectileType = Projectile;

    staticShape = "brickdaggerStaticShape";
    isRitual = true;	

    melee = true;
    doRetraction = false;
    armReady = false;
    doColorShift = daggerItem.doColorShift;
    colorShiftColor = daggerItem.colorShiftColor;

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = WeaponSwitchsound;

	stateName[1]                     = "Ready";
	stateScript[1]                  = "onReady";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]					= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.15;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateFire[3]                    = false;
	stateScript[3]                  = "onFire";
	stateSound[3]					= "sworddaggerswing_sound";
	stateTimeoutValue[3]            = 0.1;
	stateEmitter[3]					= "";
	stateEmitterNode[3]             = "muzzlePoint";
	stateEmitterTime[3]             = "0.225";

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "StopFire";

	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Break";
	stateTimeoutValue[5]            = 0.1925;
	stateScript[5]                  = "onStopFire";
	stateEmitter[5]					= "";
	stateEmitterNode[5]             = "muzzlePoint";
	stateEmitterTime[5]             = "0.1";

	stateName[6]                    = "Break";
	stateTransitionOnTimeout[6]     = "Ready";
	stateTimeoutValue[6]            = 0.1;
};

datablock StaticShapeData(brickdaggerStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/daggerstatic.dts";
	placementSound = "sworddagger_place_sound";
};

function brickdaggerStaticShape::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	%obj.schedule(33,playaudio,3,%this.placementSound);
}

function daggerImage::onReady(%this, %obj, %slot)
{
	%obj.playthread(1, "root");
}

function daggerImage::onFire(%this, %obj, %slot)
{
	if(%obj.getstate() $= "Dead") return;
	DaggerCheck(%obj,%this,%slot);
	%obj.playthread(1, "shiftTo");
	
}

function daggerImage::onPreFire(%this, %obj, %slot)
{	
	%obj.playthread(1, "shiftAway");
}

function DaggerCheck(%obj,%this,%slot)
{   
	%pos = %obj.getMuzzlePoint(%slot);
	%searchMasks = $TypeMasks::StaticObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType;

	for(%i = 0; %i < clientgroup.getCount(); %i++)
	if(isObject(%nearbyplayer = clientgroup.getObject(%i).player))
   	{		
		if(%nearbyplayer == %obj || %nearbyplayer.getDataBlock().classname $= "PlayerData") continue;

		%hitplayerpos = %nearbyplayer.getposition();
		%line = vectorNormalize(vectorSub(%hitplayerpos,%pos));
		%dot = vectorDot(%obj.getEyeVector(), %line);
		%obscure = containerRayCast(%pos,%hitplayerpos,$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);		

		if(isObject(%obscure) || %dot > 0.65 || VectorDist(%hitplayerpos,%pos) > 5) continue;
		
		if(minigameCanDamage(%obj,%nearbyplayer))
		{
			%p = new projectile()
			{
				datablock = "daggerProjectile";
				initialPosition = %nearbyplayer.getHackPosition();
			};
			%p.explode();   

			%damage = %nearbyplayer.getdatablock().maxDamage/8;
			if(vectorDot(%nearbyplayer.getforwardvector(),%obj.getforwardvector()) > 0.65) %damageclamp = mClamp(%damagepower*1.5, 40, %nearbyplayer.getdatablock().maxDamage);
			else %damageclamp = mClamp(%damagepower, 40, %nearbyplayer.getdatablock().maxDamage);
			serverPlay3D("sworddaggerhitPL" @ getRandom(1,2) @ "_sound",%hitplayerpos);					
			
			%nearbyplayer.damage(%obj, %hitplayerpos, %damageclamp, $DamageType::Default);
			%nearbyplayer.applyimpulse(%hitplayerpos,vectoradd(vectorscale(%vec,250),"0 0 250"));
		}
   } 
   return;
}