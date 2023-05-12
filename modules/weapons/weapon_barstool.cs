sm_addDamageType("BarStool");
datablock DebrisData(sm_barStoolLegDebris)
{
	shapeFile 			= "./model/d_barStoolLeg.dts";
	lifetime 			= 2.8;
	spinSpeed			= 300.0;
	minSpinSpeed 		= -1200.0;
	maxSpinSpeed 		= 1200.0;
	elasticity 			= 0.5;
	friction 			= 0.2;
	numBounces 			= 3;
	staticOnMaxBounce 	= true;
	snapOnMaxBounce 	= false;
	fade 				= true;
	gravModifier 		= 4;
};
datablock DebrisData(sm_barStoolSeat1Debris : sm_barStoolLegDebris)
{
	shapeFile 			= "./model/d_barStoolSeat1.dts";
};
datablock ExplosionData(sm_barStoolLegExplosion)
{
	debris 					= sm_barStoolLegDebris;
	debrisNum 				= 8;
	debrisNumVariance 		= 7;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 12;
	debrisVelocityVariance 	= 6;
};
datablock ExplosionData(sm_barStoolSeat1Explosion : sm_chairSeat1Explosion)
{
	debris 					= sm_barStoolSeat1Debris;
	debrisNum 				= 3;
	debrisNumVariance 		= 2;
};
datablock ExplosionData(sm_barStoolSmashExplosion)
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
	subExplosion[0] 		= sm_barStoolSeat1Explosion;
	subExplosion[1] 		= sm_chairSeat1Explosion;
	subExplosion[2] 		= sm_chairRestExplosion;
	subExplosion[3] 		= sm_barStoolLegExplosion;
	faceViewer     			= true;
	explosionScale 			= "1 1 1";
	shakeCamera 			= true;
	camShakeFreq 			= "10.0 11.0 10.0";
	camShakeAmp 			= "6.0 8.0 6.0";
	camShakeDuration 		= 0.5;
	camShakeRadius 			= 20.0;
};
datablock ProjectileData(sm_barStoolSmashProjectile)
{
	explosion = sm_barStoolSmashExplosion;
};
datablock ItemData(sm_barStoolItem)
{
	category 			= "Weapon";
	className 			= "Weapon";

	shapeFile 			= "./model/barStool.dts";
	rotate 				= false;
	mass 				= 1;
	density 			= 2;
	elasticity 			= 0.02;
	friction 			= 0.9;
	emap 				= true;

	uiName 				= ($Pref::Swol_SMMelee_Prefix ? "SM " : "") @ "Bar Stool";
	iconName 			= "./icon/icon_barStool";
	doColorShift 		= true;
	colorShiftColor 	= "0.56 0.4 0.2 1";

	image 				= sm_barStoolImage;
	canDrop 			= true;
	
	meleeRange			= 3.5;
	meleeHealth			= 4;
	meleeDamageHit		= 50;
	meleeDamageBreak	= 25;
	meleeDamageType 	= $DamageType::SM_BarStool;
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
	meleeExplosion_Smash= "sm_barStoolSmashProjectile";
};
datablock ShapeBaseImageData(sm_barStoolImage)
{
	shapeFile 			= sm_barStoolItem.shapeFile;
	emap 				= true;

	mountPoint 			= 0;
	offset 				= "-0.53 0.47 0.65";
	eyeOffset 			= "0 0 0";
	rotation 			= "0 0 0 10";
	correctMuzzleVector = false;

	doColorShift 		= sm_barStoolItem.doColorShift;
	colorShiftColor 	= sm_barStoolItem.colorShiftColor;
	className 			= "WeaponImage";
	item 				= sm_barStoolItem;
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
function sm_barStoolImage::onFire(%db,%pl,%slot)
{
	swolMelee_onFire(%db,%pl,%slot);
	%pl.playThread(3,shiftDown);
}
function sm_barStoolImage::callback_smash(%db,%pl,%slot,%currTool)
{
	%pl.playThread(1,activate2);
	%pl.playThread(2,root);
}
function sm_barStoolImage::callback_hitPlayer(%db,%pl,%slot,%pos,%victim,%smash)
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
function sm_barStoolImage::onMount(%db,%pl,%slot)
{
	parent::onMount(%db,%pl,%slot);
	%pl.playThread(2,armReadyLeft);
	%pl.setArmThread(land);
}
function sm_barStoolImage::onUnMount(%db,%pl,%slot)
{
	%pl.setArmThread(look);
	%pl.playThread(2,root);
	parent::onUnMount(%db,%pl,%slot);
}