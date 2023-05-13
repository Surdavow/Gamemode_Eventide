datablock DebrisData(sm_chairSeat1Debris)
{
	shapeFile 			= "./models/d_chairSeat1.dts";
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
datablock DebrisData(sm_chairSeat2Debris : sm_chairSeat1Debris)
{
	shapeFile 			= "./models/d_chairSeat2.dts";
};
datablock DebrisData(sm_chairSeat3Debris : sm_chairSeat1Debris)
{
	shapeFile 			= "./models/d_chairSeat3.dts";
};
datablock DebrisData(sm_chairRestDebris : sm_chairSeat1Debris)
{
	shapeFile 			= "./models/d_chairRest.dts";
	spinSpeed			= 300.0;
	minSpinSpeed 		= -1200.0;
	maxSpinSpeed 		= 1200.0;
};
datablock DebrisData(sm_chairLegDebris : sm_chairRestDebris)
{
	shapeFile 			= "./models/d_chairLeg.dts";
};
datablock ExplosionData(sm_chairSeat1Explosion)
{
	debris 					= sm_chairSeat1Debris;
	debrisNum 				= 3;
	debrisNumVariance 		= 1;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 12;
	debrisVelocityVariance 	= 6;
};
datablock ExplosionData(sm_chairSeat2Explosion : sm_chairSeat1Explosion)
{
	debris 					= sm_chairSeat2Debris;
};
datablock ExplosionData(sm_chairSeat3Explosion : sm_chairSeat1Explosion)
{
	debris 					= sm_chairSeat2Debris;
	debrisNum 				= 4;
	debrisNumVariance 		= 2;
};
datablock ExplosionData(sm_chairRestExplosion : sm_chairSeat1Explosion)
{
	debris 					= sm_chairRestDebris;
	debrisNum 				= 6;
	debrisNumVariance 		= 4;
};
datablock ExplosionData(sm_chairLegExplosion : sm_chairSeat1Explosion)
{
	debris 					= sm_chairLegDebris;
	debrisNum 				= 6;
	debrisNumVariance 		= 4;
};
datablock ExplosionData(sm_chairSmashExplosion)
{
	debris 					= sm_woodFragDebris;
	debrisNum 				= 12;
	debrisNumVariance 		= 8;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 12;
	debrisVelocityVariance 	= 6;
	explosionShape 			= "";
	lifeTimeMS 				= 150;
	subExplosion[0] 		= sm_chairSeat1Explosion;
	subExplosion[1] 		= sm_chairSeat2Explosion;
	subExplosion[2] 		= sm_chairSeat3Explosion;
	subExplosion[3] 		= sm_chairRestExplosion;
	subExplosion[4] 		= sm_chairLegExplosion;
	faceViewer     			= true;
	explosionScale 			= "1 1 1";
	shakeCamera 			= true;
	camShakeFreq 			= "10.0 11.0 10.0";
	camShakeAmp 			= "6.0 8.0 6.0";
	camShakeDuration 		= 0.5;
	camShakeRadius 			= 20.0;
};
datablock ProjectileData(sm_chairSmashProjectile)
{
	explosion = sm_chairSmashExplosion;
};
datablock ParticleData(sm_chairExplosionParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = 0.4;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 500;
	lifetimeVarianceMS   = 100;
	textureName          = "base/data/particles/cloud";
	spinSpeed			= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]			= "0.8 0.8 0.6 0.3";
	colors[1]			= "0.8 0.8 0.6 0.0";
	sizes[0]			= 1.25;
	sizes[1]			= 2.25;
	useInvAlpha 		= true;
};

datablock ParticleEmitterData(sm_chairExplosionEmitter)
{
	ejectionPeriodMS	= 1;
	periodVarianceMS	= 0;
	ejectionVelocity	= 4;
	velocityVariance	= 1.0;
	ejectionOffset  	= 0.0;
	thetaMin			= 89;
	thetaMax			= 90;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= sm_chairExplosionParticle;
};

datablock ExplosionData(sm_chairHitExplosion)
{
	debris 					= sm_woodFragDebris;
	debrisNum 				= 12;
	debrisNumVariance 		= 8;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 12;
	debrisVelocityVariance 	= 6;
	explosionShape 			= "";
	particleEmitter 		= sm_chairExplosionEmitter;
	particleDensity 		= 10;
	particleRadius 			= 0.2;
	lifeTimeMS 				= 150;
	faceViewer     			= true;
	explosionScale 			= "1 1 1";
	shakeCamera 			= true;
	camShakeFreq 			= "10.0 11.0 10.0";
	camShakeAmp 			= "3.0 4.0 3.0";
	camShakeDuration 		= 0.3;
	camShakeRadius 			= 20.0;
};
datablock ProjectileData(sm_chairHitProjectile)
{
	explosion = sm_chairHitExplosion;
};
datablock ItemData(sm_chairItem)
{
	category 			= "Weapon";
	className 			= "Weapon";

	shapeFile 			= "./models/chair.dts";
	rotate 				= false;
	mass 				= 1;
	density 			= 0.2;
	elasticity 			= 0.2;
	friction 			= 0.6;
	emap 				= true;

	uiName 				= "Chair";
	iconName 			= "./icons/icon_chair";
	doColorShift 		= true;
	colorShiftColor 	= "0.56 0.4 0.2 1";

	image 				= sm_chairImage;
	canDrop 			= true;
	
	meleeRange			= 3.5;
	meleeHealth			= 4;
	meleeDamageHit		= 50;
	meleeDamageBreak	= 25;
	meleeDamageType 	= $DamageType::Chair;
	meleeVelocity		= 22;
	
	meleeSound_Swing[0] = "sm_genericHeavySwing1Sound";
	meleeSound_Swing[1] = "sm_genericHeavySwing2Sound";
	meleeSound_SwingCnt = 2;
	
	meleeSound_Hit[0] 	= "sm_chairHit1Sound";
	meleeSound_Hit[1] 	= "sm_chairHit2Sound";
	meleeSound_HitCnt 	= 2;
	
	meleeSound_Smash[0] = "sm_chairSmash1Sound";
	meleeSound_Smash[1] = "sm_chairSmash2Sound";
	meleeSound_SmashCnt = 2;
	
	meleeExplosion_Hit 	= "sm_chairHitProjectile";
	meleeExplosion_Smash= "sm_chairSmashProjectile";
};
datablock ShapeBaseImageData(sm_chairImage)
{
	shapeFile 			= sm_chairItem.shapeFile;
	emap 				= true;

	mountPoint 			= 0;
	offset 				= "-0.53 0.3 0.72";
	eyeOffset 			= "0 0 0";
	rotation 			= "0 1 0 180";
	correctMuzzleVector = false;

	doColorShift 		= sm_chairItem.doColorShift;
	colorShiftColor 	= sm_chairItem.colorShiftColor;
	className 			= "WeaponImage";
	item 				= sm_chairItem;
	armReady 			= true;
	melee				= true;
	
	stateName[0] 					= "Activate";
	stateTimeoutValue[0] 			= 0.5;
	stateTransitionOnTimeout[0] 	= "Ready";
	
	stateName[1] 					= "Ready";
	stateTransitionOnTriggerDown[1] = "Swing";
	
	stateName[2] 					= "Swing";
	stateScript[2] 					= "onFire";
	stateFire[2] 					= true;
	stateTransitionOnTimeout[2] 	= "Ready";
	stateTimeoutValue[2] 			= 0.48;
};
function sm_chairImage::onFire(%db,%pl,%slot)
{
	swolMelee_onFire(%db,%pl,%slot);
	%pl.playThread(3,shiftDown);
}
function sm_chairImage::callback_smash(%db,%pl,%slot,%currTool)
{
	%pl.playThread(1,activate2);
	%pl.playThread(2,root);
}
function sm_chairImage::callback_hitPlayer(%db,%pl,%slot,%pos,%victim,%smash)
{
	if(minigameCanDamage(%pl,%victim) == 1)
	{
		%victim.sm_stun(500,1);
		if(%smash)
		{
			if(getRandom(0,20) == 0)
				serverPlay3d("sm_screamSound",%pos);
		}
	}
}
function sm_chairImage::onMount(%db,%pl,%slot)
{
	parent::onMount(%db,%pl,%slot);
	%pl.playThread(2,armReadyLeft);
	%pl.setArmThread(land);
}
function sm_chairImage::onUnMount(%db,%pl,%slot)
{
	%pl.setArmThread(look);
	%pl.playThread(2,root);
	parent::onUnMount(%db,%pl,%slot);
}