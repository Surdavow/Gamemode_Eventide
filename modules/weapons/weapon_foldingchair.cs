datablock ParticleData(sm_foldingChairExplosionParticle)
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
	colors[0]			= "0.6 0.6 0.6 0.3";
	colors[1]			= "0.6 0.6 0.6 0.0";
	sizes[0]			= 1.25;
	sizes[1]			= 2.25;
	useInvAlpha 		= true;
};

datablock ParticleEmitterData(sm_foldingChairExplosionEmitter)
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
	particles			= sm_foldingChairExplosionParticle;
};

datablock ExplosionData(sm_foldingChairHitExplosion)
{
	explosionShape 			= "";
	particleEmitter 		= sm_foldingChairExplosionEmitter;
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
datablock ProjectileData(sm_foldingChairHitProjectile)
{
	explosion = sm_foldingChairHitExplosion;
};
datablock ItemData(sm_foldingChairItem)
{
	category 			= "Weapon";
	className 			= "Weapon";

	shapeFile 			= "./models/folding_chair.dts";
	rotate 				= false;
	mass 				= 1;
	density 			= 2;
	elasticity 			= 0.02;
	friction 			= 0.9;
	emap 				= true;

	uiName 				= ($Pref::Swol_SMMelee_Prefix ? "SM " : "") @ "Folding Chair";
	iconName 			= "./icons/icon_foldingChair";
	doColorShift 		= true;
	colorShiftColor 	= "0.7 0.7 0.7 1";

	image 				= sm_foldingChairImage;
	canDrop 			= true;
	
	meleeRange			= 3.5;
	meleeHealth			= 0;
	meleeDamageHit		= 100;
	meleeDamageBreak	= 25;
	meleeDamageType 	= $DamageType::FoldingChair;
	meleeVelocity		= 32;
	
	meleeSound_Swing[0] = "sm_genericHeavySwing1Sound";
	meleeSound_Swing[1] = "sm_genericHeavySwing2Sound";
	meleeSound_SwingCnt = 2;
	
	meleeSound_Hit[0] 	= "sm_foldingChairHit1Sound";
	meleeSound_Hit[1] 	= "sm_foldingChairHit2Sound";
	meleeSound_HitCnt 	= 2;
	
	meleeSound_Smash[0] = "sm_chairSmash1Sound";
	meleeSound_Smash[1] = "sm_chairSmash2Sound";
	meleeSound_SmashCnt = 2;
	
	meleeExplosion_Hit 	= "sm_foldingChairHitProjectile";
	meleeExplosion_Smash= "sm_foldingChairHitProjectile";
};
datablock ShapeBaseImageData(sm_foldingChairImage)
{
	shapeFile 			= sm_foldingChairItem.shapeFile;
	emap 				= true;

	mountPoint 			= 0;
	offset 				= "-0.55 0.1 -0.34";
	eyeOffset 			= "0 0 0";
	rotation 			= "0 1 0 90";
	correctMuzzleVector = false;

	doColorShift 		= sm_foldingChairItem.doColorShift;
	colorShiftColor 	= sm_foldingChairItem.colorShiftColor;
	className 			= "WeaponImage";
	item 				= sm_foldingChairItem;
	armReady 			= true;
	melee				= true;
	
	stateName[0] 					= "Activate";
	stateTimeoutValue[0] 			= 0.5;
	stateTransitionOnTimeout[0] 	= "Ready";
	
	stateName[1] 					= "Ready";
	stateTransitionOnTriggerDown[1] = "Prep";
	
	stateName[2] 					= "Prep";
	stateScript[2] 					= "onPrep";
};
function sm_foldingChairImage::onPrep(%db,%pl,%slot)
{
	%pl.mountImage(sm_foldingChairAttackImage,0);
	cancel(%pl.foldingChairReset);
	%pl.playThread(3,rotCW);
	%pl.foldingChairReset = sm_foldingChairAttackImage.schedule(2500,resetView,%pl,%slot);
}
datablock ShapeBaseImageData(sm_foldingChairAttackImage)
{
	shapeFile 			= sm_foldingChairItem.shapeFile;
	emap 				= true;

	mountPoint 			= 0;
	offset 				= "-0.525 0 0.73";
	eyeOffset 			= "0 0 0";
	rotation 			= "0 0 0 10";
	correctMuzzleVector = false;

	doColorShift 		= sm_foldingChairItem.doColorShift;
	colorShiftColor 	= sm_foldingChairItem.colorShiftColor;
	className 			= "WeaponImage";
	item 				= sm_foldingChairItem;
	armReady 			= true;
	melee				= true;
		
	stateName[0] 					= "Activate";
	stateTimeoutValue[0] 			= 0.80;
	stateTransitionOnTimeout[0] 	= "Ready";
	
	stateName[1] 					= "Ready";
	stateTransitionOnTriggerDown[1] = "Swing";
	
	stateName[2] 					= "Swing";
	stateScript[2] 					= "onFire";
	stateFire[2] 					= true;
	stateTransitionOnTimeout[2] 	= "Ready";
	stateTimeoutValue[2] 			= 0.78;
};
function sm_foldingChairAttackImage::onFire(%db,%pl,%slot)
{
	cancel(%pl.foldingChairReset);
	%pl.foldingChairReset = sm_foldingChairAttackImage.schedule(2500,resetView,%pl,%slot);
	swolMelee_onFire(%db,%pl,%slot);
	%pl.playThread(3,shiftDown);
}
function sm_foldingChairAttackImage::resetView(%db,%pl,%slot)
{
	cancel(%pl.foldingChairReset);
	if(%pl.getDamagePercent() < 1.0)
	{
		%pl.playThread(3,rotCCW);
		%pl.mountImage(sm_foldingChairImage,0);
	}
}
function sm_foldingChairAttackImage::callback_smash(%db,%pl,%slot,%currTool)
{
	%pl.playThread(1,activate2);
	%pl.playThread(2,root);
}
function sm_foldingChairImage::onMount(%db,%pl,%slot)
{
	parent::onMount(%db,%pl,%slot);
	%pl.playThread(2,armReadyLeft);
	%pl.setArmThread(land);
}
function sm_foldingChairImage::onUnMount(%db,%pl,%slot)
{
	%pl.setArmThread(look);
	%pl.playThread(2,root);
	parent::onUnMount(%db,%pl,%slot);
	cancel(%pl.foldingChairReset);
}
function sm_foldingChairAttackImage::onMount(%db,%pl,%slot)
{
	parent::onMount(%db,%pl,%slot);
	%pl.playThread(2,armReadyLeft);
	%pl.setArmThread(land);
	%pl.prevFoldingDb = %pl.getDatablock();
	if(%pl.getDamagePercent() < 1.0)
	{
		%pl.setDatablock(PlayerFoldingChairMode);
		%pl.setActionThread("root");
	}
}
function sm_foldingChairAttackImage::onUnMount(%db,%pl,%slot)
{
	%pl.setArmThread(look);
	%pl.playThread(2,root);
	parent::onUnMount(%db,%pl,%slot);
	cancel(%pl.foldingChairReset);
	if(%pl.getDatablock().getName() $= PlayerFoldingChairMode)
	{
		if(%pl.getDamagePercent() < 1.0)
		{
			%pl.setDatablock(%pl.prevFoldingDb);
			%pl.setActionThread("root");
		}
	}
}
datablock PlayerData(PlayerFoldingChairMode : PlayerStandardArmor)
{
	cameraHorizontalOffset = 0;
	cameraVerticalOffset = 2;
	cameraMaxDist = 1;
	thirdPersonOnly = 1;
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;
	maxLookAngle = 1.5;
	maxFreeLookAngle = 0;
	cameraTilt = 0.261;
	maxTools = 1;
	maxWeapons = 1;
	uiName = "";
	
	maxForwardSpeed = 5;
	maxForwardCrouchSpeed = 2;
	maxBackwardSpeed = 2;
	maxBackwardCrouchSpeed = 1;
	maxSideSpeed = 4;
	maxSideCrouchSpeed = 1;
	
	maxUnderwaterForwardSpeed = 6.4;
	maxUnderwaterBackwardSpeed = 5.8;
	maxUnderwaterSideSpeed = 5.8;
};