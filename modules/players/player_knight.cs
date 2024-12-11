datablock PlayerData(PlayerKnight : PlayerRenowned)
{
	uiName = "Knight Player";
	
	killerSpawnMessage = "An armored figure quests for blood.";
	
	killerChaseLvl1Music = "musicData_Eventide_SorrowfulNear";
	killerChaseLvl2Music = "musicData_Eventide_SorrowfulChase";

	killeridlesound = "knight_idle";
	killeridlesoundamount = 4;

	killerchasesound = "knight_idle";
	killerchasesoundamount = 4;
	
	killermeleesound = "knight_melee";
	killermeleesoundamount = 3;	
	
	killerweaponsound = "knight_weapon";
	killerweaponsoundamount = 4;

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
	showEnergyBar = true;
	rechargeRate = 0.45;
	maxForwardSpeed = 5.8;
	maxBackwardSpeed = 3.2;
	maxSideSpeed = 4.9;
	
	knightDash = true;
	knightDashZ = 0;
	knightDashCost = 100;
	knightDashDelay = 500;
	knightDashVel = 40;
	knightDashTime = 250;
	
	killerlight = "NoFlareRLight";
	
	rightclickicon = "color_grab";
	leftclickicon = "color_melee";	
};

function PlayerKnight::onImpact(%this,%obj,%hit,%vec,%force)
{
	if(%obj.isknightDash && (%this.knightDashZ || mAbs(getWord(%vec,2)) < %this.minImpactSpeed)) 
	{
		return;
	}
	
	Parent::onImpact(%this,%obj,%hit,%vec,%force);
}

function PlayerKnight::onTrigger(%this, %obj, %trig, %press)
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	if(%press)
	{
		switch(%trig)
		{
			case 0: if(%obj.getEnergyLevel() >= 25)
					{
						%this.killerMelee(%obj,4);
						%obj.faceConfigShowFace("Attack");
						return;
					}
			case 4: if(!isObject(%obj.getObjectMount()))
					{
						%obj.knightDashStart();
					}					
		}
	}
}

function PlayerKnight::onNewDatablock(%this,%obj)
{
	%obj.createEmptyFaceConfig($Eventide_FacePacks["knight"]);
	%facePack = %obj.faceConfig.getFacePack();
	%obj.faceConfig.face["Neutral"] = %facePack.getFaceData(compileFaceDataName(%facePack, "Neutral"));
	%obj.faceConfig.setFaceAttribute("Neutral", "length", -1);
	
	Parent::onNewDatablock(%this,%obj);
	%obj.setScale("1.2 1.2 1.2");
	%obj.mountImage("ZweihanderImage",0);
}

function PlayerKnight::onPeggFootstep(%this,%obj)
{
	serverplay3d("knight_footstep" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
	%obj.spawnExplosion("Eventide_footstepShakeProjectile", 0.5 + (getRandom() / 2));
}

datablock ParticleData(knight_trailParticle)
{
	textureName				= "./models/blockhead";
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
datablock ParticleEmitterData(knight_trailEmitter)
{
	uiName				= "";
	particles			= "knight_trailParticle";
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
datablock ShapeBaseImageData(knight_trailImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;
	
	mountPoint = $BackSlot;

	offset = "0 -0.6 -0.5";
	eyeOffset = "0 0 -9999";
	
	stateName[0] 					= "Idle";
	stateTransitionOnTimeout[0] 	= "Idle2";
	stateTimeoutValue[0]	 		= 1000;
	stateEmitter[0]					= knight_trailEmitter;
	stateEmitterTime[0]				= 1000;
	stateEmitterNode[0]				= "muzzleNode";	
	
	stateName[1] 					= "Idle2";
	stateTransitionOnTimeout[1] 	= "Idle";
	stateTimeoutValue[1]	 		= 1000;
	stateEmitter[1]					= knight_trailEmitter;
	stateEmitterTime[1]				= 1000;
	stateEmitterNode[1]				= "muzzleNode";	
};
function rgbGradient(%step, %c1, %c2)
{
	%r1 = getWord(%c1, 0);
	%g1 = getWord(%c1, 1);
	%b1 = getWord(%c1, 2);

	%r2 = getWord(%c2, 0);
	%g2 = getWord(%c2, 1);
	%b2 = getWord(%c2, 2);

	%r3 = %r1 + %step * (%r2 - %r1);
	%g3 = %g1 + %step * (%g2 - %g1);
	%b3 = %b1 + %step * (%b2 - %b1);

	return %r3 SPC %g3 SPC %b3;
}
function player::knightDashStart(%pl)
{
	%db = %pl.getDatablock();
	if(!%pl.isknightDash)
	{
		if((%time = getSimTime()-%pl.lastknightDash) > %db.knightDashDelay)
		{
			if((%energy = %pl.getEnergyLevel()) >= (%cost = %db.knightDashCost))
			{
				if(isObject(%cl = %pl.client))
				{
					%pl.setDamageFlash(0.15);
					%pl.knightFov = %cl.getControlCameraFov();
				}
				cancel(%pl.knightFizzleSched);
				%pl.lastknightDash = getSimTime();
				%pl.knightDashSound = %pl.knightCancelDash = %pl.knightHasWastedEnergy = false;
				%vec = vectorScale((%db.knightDashZ ? %pl.getEyeVector() : %pl.getForwardVector()),%db.knightDashVel);
				%pl.isknightDash = 1;
				%pl.knightDash(%vec,0);
				%pl.setEnergyLevel(%pl.knightEnergy = (%energy-%cost-((%db.rechargeRate*32)*(0.00065*%db.knightDashDelay))));
				%pl.playaudio(3,"knight_teleport_sound");
				%pl.mountImage(knight_trailImage,3);
				%pl.playThread(0,plant);
			}
			else if(getSimTime()-%pl.lastknightDash > 260) %error = 1;
		}
		else %error = 2;
	}
	else %error = 0;
	if(%error)
	{
		if(getSimTime()-%pl.lastknightInvalid > 400)
		{
			%pl.lastknightInvalid = getSimTime();
			%time = (getSimTime()-%pl.lastknightDash);
			if(%error == 2)
			{
				if(!%pl.knightHasWastedEnergy && %time > %db.knightDashDelay-(%db.knightDashDelay*0.8))
				{
					%pl.setEnergyLevel(%pl.knightEnergy = (%pl.getEnergyLevel()-(%db.knightDashCost*0.5)));
					%pl.knightHasWastedEnergy = 1;
				}
			}
			else if(%error == 1)
			{
				%pl.knightFizzleSched = %pl.schedule(100,unMountImage,3);
				%pl.playThread(0,plant);
				%pl.playThread(3,undo);
				if(isObject(%cl = %pl.client)) %pl.playaudio(3,"knight_invalid_sound");
			}
		}
	}
}
function player::knightDash(%pl,%vec,%tick)
{
	%db = %pl.getDatablock();
	%forward = vectorScale((%db.knightDashZ ? %pl.getEyeVector() : %pl.getForwardVector()),0.8);
	%pos = vectorAdd(%pl.getPosition(),"0 0 0.2");
	%hit = containerRayCast(%pos,vectorAdd(%pos,%forward),$TypeMasks::FxBrickObjectType | $TypeMasks::StaticObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType,%pl);
	if(%hit || %pl.getState() $= "Dead")
		%pl.knightCancelDash = 1;
	%vel = %pl.getVelocity();
	%x = getWord(%vel,0);
	%y = getWord(%vel,1);
	%z = getWord(%vel,2);
	if(%pl.knightCancelDash && %tick > 0) %pl.unMountImage(3);
	
	%time = getSimTime()-%pl.lastknightDash;
	if(!%pl.knightDashSound && (%time > (%max = %db.knightDashTime)-100 || %pl.knightCancelDash && %tick > 4))
	{
		//%pl.schedule(1,playAudio,2,knight_airRush @ getRandom(1,2) @ Sound);
		//%pl.knightDashSound = 1;
	}
	if(%time > %max || (%pl.knightCancelDash && %tick > 4))
	{		
		if(isObject(%cl = %pl.client))
		{
			%cl.setControlCameraFov(%pl.knightFov);
			commandToClient(%cl,'setVignette',$EnvGuiServer::VignetteMultiply,$EnvGuiServer::VignetteColor);
		}
		%pl.playThread(0,jump);
		%pl.unMountImage(3);
		%pl.setTempSpeed(0.375);
		%pl.setEnergyLevel(16);
		%pl.schedule(1750,setTempSpeed,1);
		%pl.isknightDash = 0;
		%pl.setVelocity(%x/4 SPC %y/4 SPC (%db.knightDashZ ? %z/4 : %z));
		return;
	}
	else
	{
		if(isObject(%cl = %pl.client))
		{
			%cl.setControlCameraFov(%pl.knightFov-((%prcnt = ((1+mCos($PI*(%time/%max)))/2))*(%pl.knightFov/5)));
			%alpha = mClampF(%prcnt*0.6,0,1);
			%vigAlpha = mClampF(getWord($EnvGuiServer::VignetteColor,3),0,1);
			%fAlpha = mClampF(%alpha,(%cont ? mClampF(%vigAlpha,0,0.2) : %vigAlpha),1);
			commandToClient(%cl,'setVignette',$EnvGuiServer::VignetteMultiply,rgbGradient(1-%alpha,"0.4 0 0",$EnvGuiServer::VignetteColor) SPC %fAlpha);
		}
	}
	%pl.setEnergyLevel(%pl.knightEnergy);
	if(!%pl.knightCancelDash) %pl.setVelocity(vectorAdd(%x/5 SPC %y/5 SPC (%db.knightDashZ ? %z/5 : %z),%vec));
	%pl.knightDashSched = %pl.schedule(32,knightDash,%vec,%tick++);
}

function PlayerKnight::onDisabled(%this,%obj)
{
	Parent::onDisabled(%this,%obj);

	if(%obj.isknightDash && isObject(%client = %obj.client))
	{
		%client.setControlCameraFov(%obj.knightFov);
		commandToClient(%client,'setVignette',$EnvGuiServer::VignetteMultiply,$EnvGuiServer::VignetteColor);
	}
		
}

function PlayerKnight::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");	
	%obj.unhideNode("pants");
	%obj.unhideNode("headskin");
	%obj.unhideNode("larm");
	%obj.unhideNode("rarm");
	%obj.unhideNode("rshoe");
	%obj.unhideNode("lshoe");
	%obj.unhideNode("lhand");
	%obj.unhideNode("rhand");
	%obj.unhideNode("chest");
	%obj.unhideNode("armor");
	%obj.unhideNode("shoulderPads");

	%armorColor = "0.4 0.4 0.4 1";
	%otherColor = "0 0 0 1";
	%skinColor = "0 0 0 1";

	if(isObject(%obj.faceConfig))
	%obj.faceConfigShowFaceTimed("Neutral", -1);
	
	%obj.setDecalName("none");
	%obj.setNodeColor("rarm",%armorColor);
	%obj.setNodeColor("larm",%armorColor);
	%obj.setNodeColor("chest",%skinColor);
	%obj.setNodeColor("shoulderPads",%armorColor);
	%obj.setNodeColor("pants",%armorColor);
	%obj.setNodeColor("armor",%armorColor);
	%obj.setNodeColor("rshoe",%armorColor);
	%obj.setNodeColor("lshoe",%armorColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	
	//Set blood colors.
	%obj.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("lhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("rhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_back", "0.7 0 0 1");
}