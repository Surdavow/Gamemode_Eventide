AddDamageType("FoldingChair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_foldingChair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_foldingChair> %1',1,1);
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

	uiName 				= "Folding Chair";
	iconName 			= "./icons/icon_foldingChair";
	doColorShift 		= true;
	colorShiftColor 	= "0.7 0.7 0.7 1";

	image 				= sm_foldingChairAttackImage;
	canDrop 			= true;
};
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
	stateTimeoutValue[3] 			= 0.6;
};
function sm_foldingChairAttackImage::onSwing(%this,%obj,%slot)
{
	%obj.playThread(3,shiftDown);
	serverPlay3D("generic_heavyswing" @ getRandom(1,2) @ "_sound",%obj.getMuzzlePoint(0));
}

function sm_foldingChairAttackImage::onFire(%this,%obj,%slot)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead") return;
	%startpos = %obj.getMuzzlePoint(0);
	%endpos = %obj.getMuzzleVector(0);
	
	%hit = containerRayCast(%startpos,vectorAdd(%startpos,VectorScale(%endpos,7)),$TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType,%obj);
	if(isObject(%hit))
	{
		%hitpos = posFromRaycast(%hit);
		serverPlay3D("folding_chair_hit" @ getRandom(1,2) @ "_sound",%hitpos);
		%p = new Projectile()
		{
			dataBlock = "sm_foldingChairHitProjectile";
			initialPosition = %hitpos;
			sourceObject = %obj;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
		%p.explode();			
		
		if((%hit.getType() & $TypeMasks::PlayerObjectType) && minigameCanDamage(%obj,%hit) == 1)
		{			
			%hit.Damage(%obj, %hit.getPosition(), 25, $DamageType::barStool);
			%hit.applyImpulse(%hit.getposition(),vectorAdd(vectorScale(%obj.getMuzzleVector(0),1000),"0 0 1000"));
		}
	}
}

function sm_foldingChairAttackImage::onMount(%db,%pl,%slot)
{
	parent::onMount(%db,%pl,%slot);
	%pl.playThread(2,armReadyLeft);
}
function sm_foldingChairAttackImage::onUnMount(%db,%pl,%slot)
{
	%pl.playThread(2,root);
	parent::onUnMount(%db,%pl,%slot);
}