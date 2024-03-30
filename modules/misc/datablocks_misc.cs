datablock AudioDescription(AudioFSRun)
{
	volume = 0.75;
	isLooping = false;
	is3D = 1;
	ReferenceDistance = 5;
	maxDistance = 25;
	type = $SimAudioType;
};

datablock AudioDescription(AudioFSWalk)
{
	volume = 0.5;
	isLooping = false;
	is3D = 1;
	ReferenceDistance = 2.5;
	maxDistance = 25;
	type = $SimAudioType;
};

%pattern = "./*.wav";
%file = findFirstFile(%pattern);
while(%file !$= "")
{
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

	if(strstr(%file,"normal") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClose3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"quiet") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClosest3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"loud") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioDefault3d; filename = \"" @ %file @ "\"; };");

	//footsteps
	if(strstr(%file,"sounds/footsteps/") != -1)
	{
		if(strstr(%file,"walk") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioFSWalk; filename = \"" @ %file @ "\"; };");
		else if(strstr(%file,"swim") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioFSWalk; filename = \"" @ %file @ "\"; };");
		else if(strstr(%file,"run") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioFSRun; filename = \"" @ %file @ "\"; };");		
	}

	%file = findNextFile(%pattern);
}

AddDamageType("PoolCue",'<bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_poolCue> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_poolCue> %1',1,1);
AddDamageType("BarStool",'<bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_barStool> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_barStool> %1',1,1);
AddDamageType("Bottle",'<bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_bottle> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_bottle> %1',1,1);
AddDamageType("BrokenBottle",'<bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_bottle_broken> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_bottle_broken> %1',1,1);
AddDamageType("Chair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_chair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_chair> %1',1,1);
AddDamageType("FoldingChair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_foldingChair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/misc/icons/ci_foldingChair> %1',1,1);

addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_blind.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_consume.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_grab.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_handaxe.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_headache.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_meathook.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_melee.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_puppet.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_puppetcontrol.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_skinwalker_disguise.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_skinwalker_reveal.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/hicolor_vanish.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_blind.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_consume.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_grab.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_handaxe.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_headache.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_meathook.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_melee.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_puppet.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_puppetcontrol.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_skinwalker_disguise.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_skinwalker_reveal.png");
addExtraResource("Add-Ons/Gamemode_Eventide/modules/misc/icons/locolor_vanish.png");

if(isFile(%faceiflpath = "./models/face.ifl"))//Faces
{
	%write = new FileObject();
	%write.openForWrite(findFirstFile(%faceiflpath));
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
	
	%decalpath = "./models/faces/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		addExtraResource(%decalfile);
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource(findFirstFile(%faceiflpath));
}

if(isFile(%decalfilepath = "./models/decal.ifl"))//Decals
{
	%write = new FileObject();
	%write.openForWrite(findFirstFile(%decalfilepath));
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
	
	%decalpath = "./models/decals/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		eval("addExtraResource(\""@ %decalfile @ "\");");
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource(findFirstFile(%decalfilepath));
}

datablock DebrisData(singleBoneDebris)
{
   emitters = "";

	shapeFile = "./models/bone.dts";
	lifetime = 1000;
	spinSpeed			= 2000.0;
	minSpinSpeed = -100.0;
	maxSpinSpeed = 100.0;
	elasticity = 0.5;
	friction = 0.4;
	numBounces = 2;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 1;
};

datablock ExplosionData(singleBoneExplosion)
{
	soundProfile		= "";
   	explosionShape = "";
	lifeTimeMS = 10;
	debris = singleBoneDebris;
	debrisNum = 1;
	debrisNumVariance = 1;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 180;
	debrisVelocity = 8;
	debrisVelocityVariance = 6;
	particleEmitter = "";

	faceViewer     = true;
	explosionScale = "1 1 1";
};

datablock ProjectileData(singleBoneProjectile)
{
	uiname							= "";
	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;
	explosion						= singleBoneExplosion;

};

datablock DebrisData(sm_woodFragDebris)
{
	shapeFile 			= "./models/wood_fragment.dts";
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

function sm_stunImage::onMount(%this,%obj)
{
	%obj.schedule(2500,unmountImage,2);
	%obj.setactionthread("sit",1);
	%obj.stunned = 1;

	switch$(%obj.getclassName())
	{
		case "Player": %obj.client.setControlObject(%obj.client.camera);
						%obj.client.camera.setMode("Corpse",%obj);
		case "AIPlayer": %obj.stopholeloop();
	}
}

function sm_stunImage::onunMount(%this,%obj)
{
	%obj.stunned = 0;
	%obj.playThread(3,"undo");
	%obj.setactionthread("root",1);
	switch$(%obj.getclassName())
	{
		case "Player": 	%obj.client.setControlObject(%obj);
						%obj.client.camera.setMode("Observer");
		case "AIPlayer": %obj.startholeloop();
	}
}

datablock ParticleData(ConfettiParticle1)
{
	dragCoefficient		= 0;
	windCoefficient		= 2.5;
	gravityCoefficient	= 0.5;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -180.0;
	spinRandomMax		= 270.0;
	useInvAlpha		= true;
	textureName          = "base/data/particles/chunk";

   	colors[0]     = "1 0 0 1";
  	colors[1]     = "0 0 1 0.75";
  	colors[2]     = "0 1 0 0.5";
  	colors[3]     = "0 1 1 0.25";

   	sizes[0]     = "0.075";
  	sizes[1]     = "0.075";
  	sizes[2]     = "0.075";
  	sizes[3]     = "0.075";

  	times[0]      = 0.0;
   	times[1]      = 0.25;
   	times[2]      = 0.5;
   	times[3]      = 0.75;
};
datablock ParticleEmitterData(ConfettiEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 1;
   ejectionVelocity = 2;
   velocityVariance = 1;
   ejectionOffset   = 0.75;
   thetaMin         = 0.0;
   thetaMax         = 180.0;  
   particles        = "ConfettiParticle1";
   useEmitterColors = false;
   uiName = "Confetti";
};

datablock ShapeBaseImageData(ConfettiStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "ConfettiEmitter";
	stateTransitionOnTimeout[0]    = "Main";
};


datablock ParticleData(ElectricBolt1Particle)
{
	dragCoefficient		= 0;
	windCoefficient		= 1;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 50;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 90;
	spinRandomMin		= 0;
	spinRandomMax		= 360;
	useInvAlpha		= false;
	textureName          = "./texture/bolt1";
	colors[0] = "0 0.25 1 1";
	colors[1] = "0 0.25 1 0.5";
	colors[2] = "0 0.25 1 0";
	sizes[0] = 0;
	sizes[1] = 0.25;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 0.2;
};
datablock ParticleEmitterData(ElectricBolt1Emitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 10;
   ejectionVelocity = 0;
   velocityVariance = 0;
   ejectionOffset   = 0.75;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   particles        = "ElectricBolt1Particle";
   useEmitterColors = true;
   uiName = "Electric 1 Bolt";
};

datablock ParticleData(ElectricBolt2Particle)
{
	dragCoefficient		= 0;
	windCoefficient		= 1;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 50;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 90;
	spinRandomMin		= 0;
	spinRandomMax		= 360;
	useInvAlpha		= false;
	textureName          = "./texture/bolt2";
	colors[0] = "0 0.25 1 1";
	colors[1] = "0 0.25 1 0.5";
	colors[2] = "0 0.25 1 0";
	sizes[0] = 0;
	sizes[1] = 0.25;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 0.2;
};
datablock ParticleEmitterData(ElectricBolt2Emitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 10;
   ejectionVelocity = 0;
   velocityVariance = 0;
   ejectionOffset   = 1.25;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   particles        = "ElectricBolt2Particle";
   useEmitterColors = true;
   uiName = "Electric 2 Bolt";
};

datablock ShapeBaseImageData(ElectricStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1.5";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Bolt1";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "ElectricBolt1Emitter";
	stateTransitionOnTimeout[0]    = "Bolt2";

	stateName[1]                   = "Bolt2";
	stateTimeoutValue[1]           = 0.1;
	stateEmitterTime[1]            = 0.1;
	stateEmitter[1]                = "ElectricBolt2Emitter";
	stateTransitionOnTimeout[1]    = "Bolt1";
};


datablock ParticleData(FireStatusParticle)
{
	textureName          = "base/data/particles/cloud";
	dragCoefficient      = 1;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.0;
	windCoefficient      = 2.5;
	constantAcceleration = 1;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 500;
	spinSpeed     = 0;
	spinRandomMin = -30.0;
	spinRandomMax =  30.0;
	useInvAlpha   = false;

	colors[0]	= "1 1 0.5 0.5";
	colors[1]	= "1 0.5 0.25 0.25";
	colors[2]	= "1 0.8 0.25 0.0";

	sizes[0]	= 0.25;
	sizes[1]	= 0.75;
	sizes[2]	= 1.25;

	times[0]	= 0.0;
	times[1]	= 0.5;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(FireStatusEmitter)
{
  	ejectionPeriodMS = 5;
  	periodVarianceMS = 4;
  	ejectionVelocity = 0.5;
  	ejectionOffset   = 0.5;
  	velocityVariance = 0.1;
  	thetaMin         = 0;
  	thetaMax         = 180;
  	phiReferenceVel  = 0;
  	phiVariance      = 360;
  	overrideAdvance = false;
  	particles = "FireStatusParticle";      
   	useEmitterColors = true;
	uiName = "Fire Status Particle";
};

datablock ShapeBaseImageData(FireStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1.25";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;	
	
	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "FireStatusEmitter";	
	stateTransitionOnTimeout[0]    = "Main";
};


datablock ParticleData(HeartStatusParticle)
{
	dragCoefficient		= 0;
	windCoefficient		= 2.5;
	gravityCoefficient	= -0.125;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1500;
	lifetimeVarianceMS	= 1000;
	spinSpeed		= 25.0;
	spinRandomMin		= -180.0;
	spinRandomMax		= 270.0;
	useInvAlpha		= true;
	textureName          = "base/data/particles/heart";

   	colors[0]     = "1 0 0 0";
  	colors[1]     = "1 0 0 1";
  	colors[2]     = "1 0 0 0.75";
  	colors[3]     = "1 0 0 0";

   	sizes[0]     = "0.2";
  	sizes[1]     = "0.15";
  	sizes[2]     = "0.15";
  	sizes[3]     = "0.01";

  	times[0]      = 0.05;
   	times[1]      = 0.15;
   	times[2]      = 0.4;
   	times[3]      = 0.6;
};
datablock ParticleEmitterData(HeartStatusEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 25;
   ejectionVelocity = 1;
   velocityVariance = 0;
   ejectionOffset   = 1.25;
   thetaMin         = 0.0;
   thetaMax         = 180.0;  
   particles        = "HeartStatusParticle";
   useEmitterColors = false;
   uiName = "Heart Status";
};

datablock ShapeBaseImageData(HeartStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "HeartStatusEmitter";
	stateTransitionOnTimeout[0]    = "Main";
};


datablock ParticleData (NeonFlameStatusParticle)
{
	textureName = "base/data/particles/cloud";
	dragCoefficient      = 1;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.0;
	windCoefficient      = 2.5;
	constantAcceleration = 1;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 750;
	spinSpeed     = 0;
	spinRandomMin = -30.0;
	spinRandomMax =  30.0;
	useInvAlpha   = false;
	colors[0] = "0.25 0.8 0.25 0.25";
	colors[1] = "0.25 0.8 0.3 1.0";
	colors[2] = "0.6 0.0 0.0 0.0";
	sizes[0] = 0.75;
	sizes[1] = 0.375;
	sizes[2] = 0.1;
	times[0] = 0;
	times[1] = 0.75;
	times[2] = 1;
};
datablock ParticleEmitterData (NeonFlameStatusEmitter)
{
  	ejectionPeriodMS = 3;
  	periodVarianceMS = 2;
  	ejectionVelocity = 0.25;
  	ejectionOffset   = 0.375;
  	velocityVariance = 0.15;
  	thetaMin         = 0;
  	thetaMax         = 180;
  	phiReferenceVel  = 0;
  	phiVariance      = 360;
  	overrideAdvance = false;
	particles = NeonFlameStatusParticle;
	doFalloff = 0;
	doDetail = 0;
	uiName = "Neon Flame Status";
};

datablock ShapeBaseImageData(NeonFlameStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1.25";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "NeonFlameStatusEmitter";
	stateTransitionOnTimeout[0]    = "Main";
};


datablock ParticleData(SparkleStatusParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 250;
	lifetimeVarianceMS = 1;
	textureName = "base/lighting/flare";
	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
	colors[0] = "0 1 0.5 1";
	colors[1] = "0 1 0.5 1";
	colors[2] = "0 1 0.5 1";
	colors[3] = "0 1 0.5 1";
	sizes[0] = 0;
	sizes[1] = 2;
	sizes[2] = 0.5;
	sizes[3] = 0;
	times[0] = 0;
	times[1] = 0.1;
	times[2] = 0.3;
	times[3] = 1;
	useInvAlpha = false;
};

datablock ParticleEmitterData(SparkleStatusEmitter)
{
	uiName = "Sparkle Status";
	ejectionPeriodMS = 50;
	periodVarianceMS = 25;
	ejectionVelocity = 1;
	ejectionOffset = 1;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	useEmitterColors = true;
	particles = "SparkleStatusParticle";
};

datablock ParticleData(SparkleStatusParticle2: SparkleStatusParticle)
{
	textureName = "./texture/flare2";
};

datablock ParticleEmitterData(SparkleStatusEmitter2 : SparkleStatusEmitter)
{
	uiName = "Sparkle Status 2";
	particles = "SparkleStatusParticle2";
};

datablock ShapeBaseImageData(SparkleStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -1.375";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;

	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "SparkleStatusEmitter";
	stateTransitionOnTimeout[0]    = "Secondary";

	stateName[1]                   = "Secondary";
	stateTimeoutValue[1]           = 0.1;
	stateEmitterTime[1]            = 0.1;
	stateEmitter[1]                = "SparkleStatusEmitter2";
	stateTransitionOnTimeout[1]    = "Main";
};


datablock ParticleData(StinkyFlyParticle)
{
	dragCoefficient		= 0;
	windCoefficient		= 2.5;
	gravityCoefficient	= -0.25;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -180.0;
	spinRandomMax		= 270.0;
	useInvAlpha		= true;
	textureName          = "base/data/particles/chunk";

   	colors[0]     = "0.1 0.1 0.1 0";
  	colors[1]     = "0.1 0.1 0.1 1";
  	colors[2]     = "0.1 0.1 0.1 1";
  	colors[3]     = "0.1 0.1 0.1 0";

   	sizes[0]     = "0.025";
  	sizes[1]     = "0.025";
  	sizes[2]     = "0.025";
  	sizes[3]     = "0.025";

  	times[0]      = 0.0;
   	times[1]      = 0.25;
   	times[2]      = 0.5;
   	times[3]      = 0.75;
};
datablock ParticleEmitterData(StinkyFlyEmitter)
{
   ejectionPeriodMS = 25;
   periodVarianceMS = 20;
   ejectionVelocity = 1;
   velocityVariance = 0.75;
   ejectionOffset   = 1;
   thetaMin         = 0.0;
   thetaMax         = 180.0;  
   particles        = "StinkyFlyParticle";
   useEmitterColors = false;
   uiName = "Stinky Flies";
};

datablock ParticleData(StinkyParticle)
{
	textureName          = "base/data/particles/cloud";
	dragCoefficient      = 0.125;
	gravityCoefficient   = -0.25;
	inheritedVelFactor   = 0.0;
	windCoefficient      = 2.5;
	constantAcceleration = 1;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 500;
	spinSpeed     = 0;
	spinRandomMin = -30.0;
	spinRandomMax =  30.0;
	useInvAlpha   = true;

	colors[0]	= "0.1 1 0.1 0.0";
	colors[1]	= "0.1 1 0.1 0.15";
	colors[2]	= "0.1 1 0.1 0.0";

	sizes[0]	= 0.75;
	sizes[1]	= 0.75;
	sizes[2]	= 1.25;

	times[0]	= 0.0;
	times[1]	= 0.2;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(StinkyEmitter)
{
  	ejectionPeriodMS = 10;
  	periodVarianceMS = 5;
  	ejectionVelocity = 0.25;
  	ejectionOffset   = 0.75;
  	velocityVariance = 0.1;
  	thetaMin         = 0;
  	thetaMax         = 180;
  	phiReferenceVel  = 0;
  	phiVariance      = 360;
  	overrideAdvance = false;
  	particles = "StinkyParticle";      
   	useEmitterColors = true;
	uiName = "Stinky Particle";
};

datablock ShapeBaseImageData(StinkyStatusImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 5;
	offset = "0 0 -0.75";
	eyeOffset = 0;
	rotation = "0 0 0";
	correctMuzzleVector = true;
	className = "WeaponImage";
	projectile = "";
	melee = false;
	armReady = false;
	doColorShift = false;	
	
	stateName[0]                   = "Main";
	stateTimeoutValue[0]           = 0.1;
	stateEmitterTime[0]            = 0.1;
	stateEmitter[0]                = "StinkyEmitter";	
	stateTransitionOnTimeout[0]    = "Flies";

	stateName[1]                   = "Flies";
	stateTimeoutValue[1]           = 0.05;
	stateEmitterTime[1]            = 0.05;
	stateEmitter[1]                = "StinkyFlyEmitter";	
	stateTransitionOnTimeout[1]    = "Main";
};

datablock ParticleData(PrepperParticle)
{
   dragCoefficient      = 5.0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.0;
   windCoefficient      = 0;
   constantAcceleration = 0.0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 0;
   useInvAlpha          = false;
   textureName          = "Add-Ons/Brick_Halloween/Prepper";
   colors[0]     = "0.1 0.1 0.1 0.7";
   colors[1]     = "1 0 0 0.8";
   colors[2]     = "1 1 1 0.5";
   sizes[0]      = 1;
   sizes[1]      = 1.5;
   sizes[2]      = 1.3;
   times[0]      = 0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(PrepperEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 0;
   ejectionVelocity = 0.0;
   ejectionOffset   = 1.8;
   velocityVariance = 0.0;
   thetaMin         = 0;
   thetaMax         = 0;
   phiReferenceVel  = 0;
   phiVariance      = 0;
   overrideAdvance = false;
   lifeTimeMS = 100;
   particles = "PrepperParticle";

   doFalloff = true;

   emitterNode = GenericEmitterNode;
   pointEmitterNode = TenthEmitterNode;
};

datablock ExplosionData(PrepperExplosion)
{
   lifeTimeMS = 2000;
   emitter[0] = PrepperEmitter;
};

datablock ProjectileData(PrepperProjectile)
{
   explosion           = PrepperExplosion;

   armingDelay         = 0;
   lifetime            = 10;
   explodeOnDeath		= true;
};

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

datablock ParticleData(RenderAmbientParticle)
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

	colors[0] = "0 0 0 .75";
	colors[1] = "0 0 0 0.25";
	colors[3] = "0 0 0 0";
	sizes[0] = 0.2;
	sizes[1] = 0.4;
	sizes[2] = 0.6;
};
datablock ParticleEmitterData(RenderAmbientEmitter)
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
	particles = RenderAmbientParticle;

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
	colors[0]				= "1 1 1 0.5";
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
	ejectionPeriodMS	= 1;
	periodVarianceMS	= 0;
	ejectionVelocity	= 0.0;
	velocityVariance	= 0.0;
	ejectionOffset		= 0.35;
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
	colors[0]				= "1.0 1.0 1.0 0.5";
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
	ejectionPeriodMS	= 1;
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
	colors[0]				= "0 0 0 0.4";
	colors[1]				= "0 0 0 0.1";
	colors[2]				= "0 0 0 0";
	sizes[0]				= 2.6;
	sizes[1]				= 2.6;
	sizes[2]				= 2.6;
	times[0]				= 0;
	times[1]				= 0.5;
	times[2]				= 1.0;
	useInvAlpha				= true;
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

datablock ParticleData(BleedParticle)
{
   dragCoefficient = 3;
   gravityCoefficient = 0.5;
   inheritedVelFactor = 0.3;
   constantAcceleration = 0;
   lifetimeMS         = 100;
   lifetimeVarianceMS = 250;
   textureName = "base/data/particles/cloud";
   spinSpeed     = 0;
   spinRandomMin = -20;
   spinRandomMax = 20;
   colors[0] = "0.6 0 0 1";
   colors[1] = "0.5 0 0 0.3 ";
   colors[2] = "0.4 0 0 0";
   sizes[0] = 0.12;
   sizes[1] = 0.4;
   sizes[2] = 0.08;
   times[1] = 0.5;
   times[2] = 1;
   useInvAlpha = true;
};

datablock ParticleEmitterData(BleedEmitter)
{
   ejectionPeriodMS = 50;
   periodVarianceMS = 0;
   ejectionVelocity = 1;
   velocityVariance = 0;
   ejectionOffset   = 0;
   thetaMin = 0;
   thetaMax = 180;
   phiReferenceVel = 0;
   phiVariance     = 360;
   overrideAdvance = false;
   particles = "BleedParticle";

   uiName = "";
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
    stateSound[0]               = "renowned_Possessed2_sound";
};

datablock ShapeBaseImageData(RenderTurnImage)
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
	stateEmitter[0]            = RenderAmbientEmitter;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
};

datablock ShapeBaseImageData(GlowFaceImage) 
{
	shapeFile			= "base/data/shapes/empty.dts";
	mountPoint			= 5;
	offset 				= "-0.09 -0.03 0.033";
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

datablock ShapeBaseImageData(BleedImage) 
{
	shapeFile			= "base/data/shapes/empty.dts";
	mountPoint			= 2;
	offset = "-0.5 0.05 -0.45";
	correctMuzzleVector	= false;
	stateName[0]				= "Bleed";
	stateEmitter[0]				= BleedEmitter;
	stateEmitterTime[0]			= 1000;
	stateWaitForTimeout[0]		= true;
	stateTimeoutValue[0]		= 1000;
	stateTransitionOnTimeout[0]	= "Bleed";
	stateScript[0]				= "onBleed";
};

datablock ShapeBaseImageData(FogImage) 
{
	shapeFile			= "base/data/shapes/empty.dts";
	mountPoint			= 2;
	offset = "0 -0.5 -0.25";
	correctMuzzleVector	= false;
	stateName[0]				= "Fog";
	stateEmitter[0]				= FogEmitter;
	stateEmitterTime[0]			= 1000;
	stateWaitForTimeout[0]		= true;
	stateTimeoutValue[0]		= 1000;
	stateTransitionOnTimeout[0]	= "Fog";
	stateScript[0]				= "onFog";
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
