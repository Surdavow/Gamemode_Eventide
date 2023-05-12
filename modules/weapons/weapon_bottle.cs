sm_addDamageType("Bottle");
sm_addDamageType("Bottle_Broken");
datablock AudioProfile(sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_hit1.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(sm_bottleHit2Sound : sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_hit2.wav";
};
datablock AudioProfile(sm_bottleHitPlayer1Sound : sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_hitPlayer1.wav";
};
datablock AudioProfile(sm_bottleHitPlayer2Sound : sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_hitPlayer2.wav";
};
datablock AudioProfile(sm_bottleBrokenHit1Sound : sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_broken_hit1.wav";
};
datablock AudioProfile(sm_bottleBrokenHit2Sound : sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_broken_hit2.wav";
};
datablock AudioProfile(sm_bottleBrokenHitPlayer1Sound : sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_broken_hitPlayer1.wav";
};
datablock AudioProfile(sm_bottleBrokenHitPlayer2Sound : sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_broken_hitPlayer2.wav";
};
datablock AudioProfile(sm_bottleBreakSound : sm_bottleHit1Sound)
{
	filename    = "./sound/bottle_smash.wav";
};
datablock DebrisData(sm_bottleShard1Debris)
{
	shapeFile 			= "./model/d_glassShard1.dts";
	lifetime 			= 2.8;
	spinSpeed			= 1200.0;
	minSpinSpeed 		= -3600.0;
	maxSpinSpeed 		= 3600.0;
	elasticity 			= 0.5;
	friction 			= 0.2;
	numBounces 			= 3;
	staticOnMaxBounce 	= true;
	snapOnMaxBounce 	= false;
	fade 				= true;
	gravModifier 		= 4;
};
datablock DebrisData(sm_bottleShard2Debris : sm_bottleShard1Debris)
{
	shapeFile 			= "./model/d_glassShard2.dts";
};
datablock DebrisData(sm_bottleShard3Debris : sm_bottleShard1Debris)
{
	shapeFile 			= "./model/d_glassShard3.dts";
};
datablock ExplosionData(sm_bottleShard1Explosion)
{
	debris 					= sm_bottleShard1Debris;
	debrisNum 				= 3;
	debrisNumVariance 		= 1;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 5;
	debrisVelocityVariance 	= 2;
};
datablock ExplosionData(sm_bottleShard2Explosion : sm_bottleShard1Explosion)
{
	debris 					= sm_bottleShard2Debris;
	debrisNum 				= 8;
	debrisNumVariance 		= 3;
	debrisVelocity 			= 12;
	debrisVelocityVariance 	= 6;
};
datablock ExplosionData(sm_bottleSmashExplosion)
{
	debris 					= sm_bottleShard3Debris;
	debrisNum 				= 18;
	debrisNumVariance 		= 8;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 12;
	debrisVelocityVariance 	= 6;
	explosionShape 			= "";
	lifeTimeMS 				= 150;
	subExplosion[0] 		= sm_bottleShard1Explosion;
	subExplosion[1] 		= sm_bottleShard2Explosion;
	faceViewer     			= true;
	explosionScale 			= "1 1 1";
	shakeCamera 			= true;
	camShakeFreq 			= "4.0 5.0 4.0";
	camShakeAmp 			= "3.0 4.0 3.0";
	camShakeDuration 		= 0.1;
	camShakeRadius 			= 10.0;
};
datablock ProjectileData(sm_bottleSmashProjectile)
{
	explosion = sm_bottleSmashExplosion;
};
datablock ProjectileData(sm_bottleMiniSmashProjectile)
{
	explosion = sm_bottleShard2Explosion;
};
datablock ParticleData(sm_bottleExplosionParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = 0.4;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 300;
	lifetimeVarianceMS   = 100;
	textureName          = "base/data/particles/cloud";
	spinSpeed			= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]			= "0.8 0.8 0.6 0.3";
	colors[1]			= "0.8 0.8 0.6 0.0";
	sizes[0]			= 0.75;
	sizes[1]			= 1.25;

	useInvAlpha = true;
};

datablock ParticleEmitterData(sm_bottleExplosionEmitter)
{
	ejectionPeriodMS	= 1;
	periodVarianceMS	= 0;
	ejectionVelocity	= 2;
	velocityVariance	= 1.0;
	ejectionOffset  	= 0.0;
	thetaMin			= 89;
	thetaMax			= 90;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= sm_bottleExplosionParticle;
};
datablock ExplosionData(sm_bottleHitExplosion)
{
	debris 					= sm_bottleShard3Debris;
	debrisNum 				= 5;
	debrisNumVariance 		= 3;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 12;
	debrisVelocityVariance 	= 6;
	explosionShape 			= "";
	particleEmitter 		= sm_bottleExplosionEmitter;
	particleDensity 		= 10;
	particleRadius 			= 0.2;
	lifeTimeMS 				= 150;
	faceViewer     			= true;
	explosionScale 			= "1 1 1";
	shakeCamera 			= true;
	camShakeFreq 			= "4.0 5.0 4.0";
	camShakeAmp 			= "3.0 4.0 3.0";
	camShakeDuration 		= 0.1;
	camShakeRadius 			= 20.0;
};
datablock ProjectileData(sm_bottleHitProjectile)
{
	explosion = sm_bottleHitExplosion;
};
datablock ItemData(sm_bottleItem)
{
	category 			= "Weapon";
	className 			= "Weapon";

	shapeFile 			= "./model/bottle.dts";
	rotate 				= false;
	mass 				= 1;
	density 			= 0.2;
	elasticity 			= 0.2;
	friction 			= 0.6;
	emap 				= true;

	uiName 				= ($Pref::Swol_SMMelee_Prefix ? "SM " : "") @ "Bottle";
	iconName 			= "./icon/icon_bottle";
	doColorShift 		= true;
	colorShiftColor 	= "0.4 0.2 0 1";

	image 				= sm_bottleImage;
	canDrop 			= true;
	
	meleeRange			= 3;
	meleeHealth			= 3;
	meleeDamageHit		= 25;
	meleeDamageBreak	= 25;
	meleeDamageType 	= $DamageType::SM_Bottle;
	
	meleeSound_Swing[0] = "sm_genericLightSwing1Sound";
	meleeSound_Swing[1] = "sm_genericLightSwing2Sound";
	meleeSound_SwingCnt = 2;
	
	meleeSound_Hit[0] = "sm_bottleHitPlayer1Sound";
	meleeSound_Hit[1] = "sm_bottleHitPlayer2Sound";
	meleeSound_HitCnt = 2;
	
	meleeSound_HitSolid[0] = "sm_bottleHit1Sound";
	meleeSound_HitSolid[1] = "sm_bottleHit2Sound";
	meleeSound_HitSolidCnt = 2;
	
	meleeSound_Smash[0] = "sm_bottleBreakSound";
	meleeSound_SmashCnt = 1;
	
	meleeExplosion_Hit = "sm_bottleHitProjectile";
	meleeExplosion_Smash = "sm_bottleMiniSmashProjectile";
};
datablock ShapeBaseImageData(sm_bottleImage)
{
	shapeFile 			= sm_bottleItem.shapeFile;
	emap 				= true;

	mountPoint 			= 0;
	offset 				= "0 0 0.3";
	eyeOffset 			= "0 0 0";
	rotation 			= "1 0 0 180";
	correctMuzzleVector = false;

	doColorShift 		= sm_bottleItem.doColorShift;
	colorShiftColor 	= sm_bottleItem.colorShiftColor;
	className 			= "WeaponImage";
	item 				= sm_bottleItem;
	armReady 			= true;
	melee				= true;
	
	stateName[0] 					= "Activate";
	stateTimeoutValue[0] 			= 0.5;
	stateTransitionOnTimeout[0] 	= "Ready";
	
	stateName[1] 					= "Ready";
	stateTransitionOnTriggerDown[1] = "PreSwing";
	
	stateName[2] 					= "PreSwing";
	stateScript[2] 					= "onSwing";
	stateFire[2] 					= true;
	stateTransitionOnTimeout[2] 	= "Swing";
	stateTimeoutValue[2] 			= 0.07;
	
	stateName[3] 					= "Swing";
	stateScript[3] 					= "onFire";
	stateFire[3] 					= true;
	stateTransitionOnTimeout[3] 	= "Ready";
	stateTimeoutValue[3] 			= 0.38;
};
function sm_bottleImage::onSwing(%db,%pl,%slot)
{
	%pl.playThread(2,shiftTo);
}
function sm_bottleImage::onFire(%db,%pl,%slot)
{
	swolMelee_onFire(%db,%pl,%slot);
}
function sm_bottleImage::callback_hitPlayer(%db,%pl,%slot,%pos,%victim,%smash)
{
	if(minigameCanDamage(%pl,%victim) == 1)
	{
		if(%smash)
			%victim.sm_stun(800,0);
	}
}
function sm_bottleImage::callback_smash(%db,%pl,%slot,%currTool)
{
	%pl.tool[%currTool] = sm_bottleBrokenItem.getId();
	if(isObject(%cl = %pl.client))
		messageClient(%cl,'MsgItemPickup','',%currTool,sm_bottleBrokenItem.getId(),1);
	%pl.mountImage(sm_bottleBrokenImage,0);
	%pl.meleeHealth[sm_bottleBrokenItem] = sm_bottleBrokenItem.meleeHealth;
	%pl.playThread(2,shiftTo);
}
datablock ItemData(sm_bottleBrokenItem : sm_bottleItem)
{
	shapeFile 			= "./model/bottle_broken.dts";
	uiName 				= ($Pref::Swol_SMMelee_Prefix ? "SM " : "") @ "Bottle Broken";
	iconName 			= "./icon/icon_bottle_broken";

	image 				= sm_bottleBrokenImage;
	
	meleeRange			= 2.5;
	meleeHealth			= 1;
	meleeDamageHit		= 100;
	meleeDamageBreak	= 100;
	meleeDamageType 	= $DamageType::SM_Bottle_Broken;
	
	meleeSound_HitCnt = 0;
	
	meleeSound_HitSolid[0] = "sm_bottleBrokenHit1Sound";
	meleeSound_HitSolid[1] = "sm_bottleBrokenHit2Sound";
	meleeSound_HitSolidCnt = 0;
	
	meleeSound_Smash[0] = "sm_bottleBrokenHitPlayer1Sound";
	meleeSound_Smash[1] = "sm_bottleBrokenHitPlayer1Sound";
	meleeSound_SmashCnt = 0;
	
	meleeExplosion_Smash = "sm_bottleSmashProjectile";
};
datablock ShapeBaseImageData(sm_bottleBrokenImage : sm_bottleImage)
{
	shapeFile 			= sm_bottleBrokenItem.shapeFile;
	item 				= sm_bottleBrokenItem;
	rotation 			= "1 0 0 210";
	offset 				= "0 0.08 0.15";
};
function sm_bottleBrokenImage::onSwing(%db,%pl,%slot)
{
	%pl.playThread(2,shiftAway);
}
function sm_bottleBrokenImage::onFire(%db,%pl,%slot)
{
	swolMelee_onFire(%db,%pl,%slot);
}
function sm_bottleBrokenImage::callback_smash(%db,%pl,%slot,%currTool)
{
	%pl.playThread(1,shiftAway);
}
function sm_bottleBrokenImage::callback_hitPlayer(%db,%pl,%slot,%pos,%victim,%smash)
{
	if(%smash)
		serverPlay3D(%db.item.meleeSound_Smash[getRandom(0,1)],%pos);
}
function sm_bottleBrokenImage::callback_hitSolid(%db,%pl,%slot,%pos,%victim,%smash)
{
	if(%smash)
		serverPlay3D(%db.item.meleeSound_HitSolid[getRandom(0,1)],%pos);
}