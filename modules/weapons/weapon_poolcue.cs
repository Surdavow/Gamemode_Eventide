sm_addDamageType("PoolCue");
datablock AudioProfile(sm_poolCueSmash1Sound)
{
	filename    = "./sound/poolCue_Smash1.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(sm_poolCueSmash2Sound : sm_poolCueSmash1Sound)
{
	filename    = "./sound/poolCue_Smash2.wav";
};
datablock DebrisData(sm_poolCueGripDebris)
{
	shapeFile 			= "./model/d_poolCueGrip.dts";
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
datablock DebrisData(sm_poolCueShaftDebris : sm_poolCueGripDebris)
{
	shapeFile 			= "./model/d_poolCueShaft.dts";
};
datablock DebrisData(sm_poolCueEndDebris : sm_poolCueGripDebris)
{
	shapeFile 			= "./model/d_poolCueEnd.dts";
};
datablock ExplosionData(sm_poolCueGripExplosion)
{
	debris 					= sm_poolCueGripDebris;
	debrisNum 				= 1;
	debrisNumVariance 		= 0;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 12;
	debrisVelocityVariance 	= 6;
};
datablock ExplosionData(sm_poolCueShaftExplosion : sm_poolCueGripExplosion)
{
	debris 					= sm_poolCueShaftDebris;
};
datablock ExplosionData(sm_poolCueEndExplosion : sm_poolCueGripExplosion)
{
	debris 					= sm_poolCueEndDebris;
};
datablock ExplosionData(sm_poolCueSmashExplosion)
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
	subExplosion[0] 		= sm_poolCueGripExplosion;
	subExplosion[1] 		= sm_poolCueShaftExplosion;
	subExplosion[2] 		= sm_poolCueEndExplosion;
	faceViewer     			= true;
	explosionScale 			= "1 1 1";
	shakeCamera 			= true;
	camShakeFreq 			= "10.0 11.0 10.0";
	camShakeAmp 			= "6.0 8.0 6.0";
	camShakeDuration 		= 0.5;
	camShakeRadius 			= 20.0;
};
datablock ProjectileData(sm_poolCueSmashProjectile)
{
	explosion = sm_poolCueSmashExplosion;
};
datablock ItemData(sm_poolCueItem)
{
	category 			= "Weapon";
	className 			= "Weapon";

	shapeFile 			= "./model/poolCue.dts";
	rotate 				= false;
	mass 				= 1;
	density 			= 2;
	elasticity 			= 0.02;
	friction 			= 0.9;
	emap 				= true;

	uiName 				= ($Pref::Swol_SMMelee_Prefix ? "SM " : "") @ "Pool Cue";
	iconName 			= "./icon/icon_poolCue";
	doColorShift 		= true;
	colorShiftColor 	= "0.56 0.4 0.2 1";

	image 				= sm_poolCueImage;
	canDrop 			= true;
	
	meleeRange			= 4.5;
	meleeHealth			= 6;
	meleeDamageHit		= 35;
	meleeDamageBreak	= 35;
	meleeDamageType 	= $DamageType::SM_PoolCue;
	meleeVelocity		= 7;
	
	meleeSound_Swing[0] = "sm_genericHeavySwing1Sound";
	meleeSound_Swing[1] = "sm_genericHeavySwing2Sound";
	meleeSound_SwingCnt = 2;
	
	meleeSound_Hit[0] 	= "sm_chairHit1Sound";
	meleeSound_Hit[1] 	= "sm_chairHit2Sound";
	meleeSound_HitCnt 	= 2;
	
	meleeSound_Smash[0] = "sm_poolCueSmash1Sound";
	meleeSound_Smash[1] = "sm_poolCueSmash2Sound";
	meleeSound_SmashCnt = 2;
	
	meleeExplosion_Hit 	= "sm_chairHitProjectile";
	meleeExplosion_Smash= "sm_poolCueSmashProjectile";
};
datablock ShapeBaseImageData(sm_poolCueImage)
{
	shapeFile 			= sm_poolCueItem.shapeFile;
	emap 				= true;

	mountPoint 			= 0;
	offset 				= "0 0 1.1";
	eyeOffset 			= "0 0 0";
	rotation 			= "0 0 0 10";
	correctMuzzleVector = false;

	doColorShift 		= sm_poolCueItem.doColorShift;
	colorShiftColor 	= sm_poolCueItem.colorShiftColor;
	className 			= "WeaponImage";
	item 				= sm_poolCueItem;
	armReady 			= true;
	melee				= true;
	
	stateName[0] 					= "Activate";
	stateTimeoutValue[0] 			= 0.65;
	stateTransitionOnTimeout[0] 	= "Ready";
	
	stateName[1] 					= "Ready";
	stateTransitionOnTriggerDown[1] = "Swing";
	
	stateName[2] 					= "Swing";
	stateScript[2] 					= "onFire";
	stateFire[2] 					= true;
	stateTransitionOnTimeout[2] 	= "Ready";
	stateTimeoutValue[2] 			= 0.63;
};
function sm_poolCueImage::onFire(%db,%pl,%slot)
{
	swolMelee_onFire(%db,%pl,%slot);
	%pl.playThread(2,shiftDown);
}
function sm_poolCueImage::callback_smash(%db,%pl,%slot,%currTool)
{
	%pl.playThread(1,activate);
}