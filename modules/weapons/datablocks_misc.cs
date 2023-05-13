AddDamageType("PoolCue",'<bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_poolCue> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_poolCue> %1',1,1);
AddDamageType("BarStool",'<bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_barStool> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_barStool> %1',1,1);
AddDamageType("Bottle",'<bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_bottle> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_bottle> %1',1,1);
AddDamageType("BrokenBottle",'<bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_bottle_broken> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_bottle_broken> %1',1,1);
AddDamageType("Chair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_chair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_chair> %1',1,1);
AddDamageType("FoldingChair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_foldingChair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/weapons/icons/ci_foldingChair> %1',1,1);

datablock DebrisData(sm_woodFragDebris)
{
	shapeFile 			= "./models/d_woodFrag.dts";
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
datablock ParticleData(sm_stunParticle)
{
	dragCoefficient      = 13;
	gravityCoefficient   = 0.2;
	inheritedVelFactor   = 1.0;
	constantAcceleration = 0.0;
	lifetimeMS           = 400;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/star1";
	spinSpeed		   = 0.0;
	spinRandomMin		= 0.0;
	spinRandomMax		= 0.0;
	colors[0]     = "1 1 0.2 0.9";
	colors[1]     = "1 1 0.4 0.5";
	colors[2]     = "1 1 0.5 0";

	sizes[0]      = 0.5;
	sizes[1]      = 0.2;
	sizes[2]      = 0.1;

	times[0] = 0.0;
	times[1] = 0.5;
	times[2] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(sm_stunEmitter)
{
	ejectionPeriodMS = 12;
	periodVarianceMS = 1;
	ejectionVelocity = 5.25;
	velocityVariance = 0.0;
	ejectionOffset   = 0.25;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = sm_stunParticle;
};
datablock ShapeBaseImageData(sm_stunImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;
	offset = "0 0 0.4";
	eyeOffset = "0 0 999";

	stateName[0]				= "Ready";
	stateTimeoutValue[0]		= 0.01;
	stateTransitionOnTimeout[0]	= "FireA";

	stateName[1]				= "FireA";
	stateEmitter[1]				= sm_stunEmitter;
	stateEmitterTime[1]			= 1.2;
	stateTimeoutValue[1]		= 1.2;
	stateTransitionOnTimeout[1]	= "Done";
	stateWaitForTimeout[1]		= true;

	stateName[2]				= "Done";
	stateTimeoutValue[2]		= 0.01;
	stateTransitionOnTimeout[2]	= "FireA";
};
datablock PlayerData(sm_playerFrozen : PlayerStandardArmor)
{
	runForce = 1800;
	runEnergyDrain = 0;
	minRunEnergy = 0;
	maxForwardSpeed = 0;
	maxBackwardSpeed = 0;
	maxSideSpeed = 0;
	maxForwardCrouchSpeed = 0;
	maxBackwardCrouchSpeed = 0;
	maxSideCrouchSpeed = 0;
	jumpForce = 0;
	jumpDelay = 0;
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;
	runSurfaceAngle  = 150;
	jumpSurfaceAngle = 150;
	uiName = "";
};
datablock PlayerData(PlayerMeleeFP : PlayerStandardArmor)
{
	cameraHorizontalOffset = 0;
	cameraVerticalOffset = 2;
	cameraMaxDist = 1;
	thirdPersonOnly = 1;
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;
	firstPersonOnly = 1;
	uiName = ($Pref::Swol_SMMelee_Prefix ? "SM " : "") @ "Melee Player";
	maxTools = 1;
	maxWeapons = 1;
};