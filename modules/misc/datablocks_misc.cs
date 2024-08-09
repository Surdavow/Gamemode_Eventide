datablock AudioDescription(AudioFSRun)
{
	volume = 0.85;
	isLooping = false;
	is3D = 1;
	ReferenceDistance = 10;
	maxDistance = 40;
	type = $SimAudioType;
};

datablock AudioDescription(AudioFSWalk)
{
	volume = 0.65;
	isLooping = false;
	is3D = 1;
	ReferenceDistance = 5;
	maxDistance = 15;
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

datablock DebrisData(singleBoneDebris)
{
   emitters = "";

	shapeFile = "./models/bone.dts";
	lifetime = 10;
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

datablock ParticleData(BleedParticle)
{
   dragCoefficient = 3;
   gravityCoefficient = 0.5;
   inheritedVelFactor = 0.3;
   constantAcceleration = 0;
   lifetimeMS         = 100;
   lifetimeVarianceMS = 50;
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