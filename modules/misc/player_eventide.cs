datablock TSShapeConstructor(EventideplayerDts) 
{
	baseShape = "./models/eventideplayer.dts";
	sequence0 = "./models/default.dsq";
};

datablock PlayerData(EventidePlayer : PlayerStandardArmor)
{
	shapeFile = EventideplayerDts.baseShape;
	uiName = "Eventide Player";

	uniformCompatible = true;
	isEventideModel = true;
	showEnergyBar = false;
	firstpersononly = false;
	canJet = false;
	renderFirstPerson = false;
	defaultTunnelFOV = 100;

	rechargeRate = 0.375;
	maxTools = 3;
	maxWeapons = 3;
	jumpForce = 10 * 85;
	
	cameramaxdist = 2;
    cameraVerticalOffset = 1;
    cameraHorizontalOffset = 0.75;
    cameratilt = 0.1;
	maxfreelookangle = 2.25;

	minimpactspeed = 15;
	groundImpactMinSpeed = 5;
	groundImpactShakeFreq = "4.0 4.0 4.0";
	groundImpactShakeAmp = "1.0 1.0 1.0";
	groundImpactShakeDuration = 0.8;
	groundImpactShakeFalloff = 15;
};

datablock PlayerData(EventidePlayerDowned : EventidePlayer)
{	
	maxForwardSpeed = 0;
   	maxBackwardSpeed = 0;
   	maxSideSpeed = 0;
   	maxForwardCrouchSpeed = 1.5;
   	maxBackwardCrouchSpeed = 1.5;
   	maxSideCrouchSpeed = 1.5;
   	jumpForce = 0;
	isDowned = true;
	uiName = "";
};

registerInputEvent("fxDtsBrick", "onGaze", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
function EventidePlayer::GazeLoop(%this,%obj)
{		
	if(!isObject(%obj) || !isObject(%client = %obj.client) || %obj.getState() $= "Dead" || %obj.getdataBlock() != %this || !isObject(%minigame = getMinigamefromObject(%obj)))
	return;

	cancel(%obj.GazeLoop);
	%obj.GazeLoop = %this.schedule(33,GazeLoop,%obj);

	if($Pref::Server::GazeEnabled)
	{
		%hit = firstWord(containerRaycast(%obj.getEyePoint(),vectorAdd(%obj.getEyePoint(),vectorScale(%obj.getEyeVector(), $Pref::Server::GazeRange)),$TypeMasks::FxBrickObjectType,%obj));
		if(isObject(%hit))
		{
			$InputTarget_Self = %hit;
			$InputTarget_Player = %obj;
			$InputTarget_Client = %client;
			$InputTarget_Minigame = %minigame;
			%hit.processInputEvent("onGaze", %gazer);
		}
	}
}

function EventidePlayer::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(1,setEnergyLevel,0);
	%obj.setScale("1 1 1");	
	%this.GazeLoop(%obj);
}

function EventidePlayer::onImpact(%this, %obj, %col, %vec, %force)
{
	if(%obj.getState() !$= "Dead") 
	{				
		%zvector = getWord(%vec,2);
		if(%zvector > %this.minImpactSpeed) %obj.playthread(3,"plant");

		if(%zvector > %this.minImpactSpeed && %zvector < %this.minImpactSpeed+5) %force = %force*0.5;
		else if(%zvector > %this.minImpactSpeed+5 && %zvector < %this.minImpactSpeed+20) %force = %force*1.5;
		else %force = %force*2.5;
	}
	
	Parent::onImpact(%this, %obj, %col, %vec, mCeil(%force));	
}

function EventidePlayer_BreakFreePrint(%client,%amount)
{
    if(!isobject(%client)) return;
	
	%addsymbol = "";
    %symbol = "|";
	for(%i = 0; %i < %amount; %i++) %addsymbol = %addsymbol @ %symbol;

    %client.centerprint("<color:FFFFFF><font:impact:40> Control yourself! <br><color:00e100>" @ %addsymbol,1);
}

function EventidePlayer::onActivate(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);

	if(%obj.isPossessed) 
	{		
		%obj.AntiPossession = mClampF(%obj.AntiPossession+1, 0, 15);
		EventidePlayer_BreakFreePrint(%obj.client,%obj.AntiPossession/2);
		%obj.playthread(3,"activate2");

		if(%obj.AntiPossession >= 15)
		{
			if(isObject(%obj.Possesser))
			{
				%obj.Possesser.client.Camera.setMode("Corpse", %obj.Possesser);
				%obj.Possesser.client.setControlObject(%obj.Possesser.client.camera);
				%obj.Possesser.client.centerprint("<color:FFFFFF><font:Impact:40>Your victim broke free!",2);
				cancel(%obj.Possesser.returnObserveSchedule);
				%obj.Possesser.returnObserveSchedule = %obj.Possesser.schedule(4000,ClearRenownedEffect);
				
				%obj.Possesser.playthread(2,"undo");
				%obj.Possesser.playthread(3,"activate2");
				%obj.Possesser.mountImage("RenownedPossessedImage",3);
				%obj.Possesser.playaudio(3,"renowned_melee" @ getRandom(0,2) @ "_sound");
			}

			%obj.client.centerprint("<color:FFFFFF><font:Impact:40>You broke free!",1);
			%obj.ClearRenownedEffect();			
		}
	}
}

function EventidePlayer::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%press)
	{
		switch(%trig)
		{
			case 0:	%ray = containerRayCast(%obj.getEyePoint(), vectoradd(%obj.getEyePoint(),vectorscale(%obj.getEyeVector(),5*getWord(%obj.getScale(),2))),$TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType,%obj);
					if(isObject(%ray) && (%ray.getClassName() $= "Player" || %ray.getClassName() $= "AIPlayer") && %ray.getdataBlock().isDowned && !isObject(%obj.getMountedImage(0)))
					{
						if(!%ray.isBeingSaved)
						{
							%obj.isSaving = %ray;
							%obj.playthread(2,"armReadyRight");
							%ray.isBeingSaved = true;
							%this.SaveVictim(%obj,%ray,%press);
						}
					}
			case 2: 
			case 4: if(%obj.isSkinwalker && %obj.getEnergyLevel() >= %this.maxEnergy && !isObject(%obj.victim) && !isEventPending(%obj.monstertransformschedule))
					PlayerSkinwalker.monstertransform(%obj,true);
		}
	}
	else if(isObject(%obj.isSaving)) %this.SaveVictim(%obj,%obj.isSaving,0);
}

function EventidePlayer_SaveCounterPrint(%client,%amount)
{
    if(!isobject(%client)) return;
	
	%addsymbol = "";
    %symbol = "|";
	for(%i = 0; %i < %amount; %i++) %addsymbol = %addsymbol @ %symbol;

    %client.centerprint("<color:FFFFFF><font:impact:40> Get up! <br><color:00e100>" @ %addsymbol,1);
}

function EventidePlayer::SaveVictim(%this,%obj,%victim,%bool)
{
	if(%bool && vectorDist(%obj.getPosition(),%victim.getPosition()) < 5)
	{		
		if(%obj.savevictimcounter <= 4)
		{
			%obj.savevictimcounter++;
			EventidePlayer_SaveCounterPrint(%obj.client,%obj.savevictimcounter);
			EventidePlayer_SaveCounterPrint(%victim.client,%obj.savevictimcounter);

			cancel(%obj.SaveVictimSched);
			%obj.SaveVictimSched = %this.schedule(1000,SaveVictim,%obj,%victim,%bool);
		}
		else
		{
			%obj.savevictimcounter = 0;
			if(isObject(%obj.client)) %obj.client.centerprint("<color:FFFFFF><font:impact:40>You revived" SPC %victim.client.name,1);
			if(isObject(%victim.client)) %victim.client.centerprint("<color:FFFFFF><font:impact:40>You were revived by" SPC %obj.client.name,1);
			%victim.setHealth(10);
			%victim.setDatablock("EventidePlayer");			
			%victim.playthread(0,"root");
			return;
		}					
	}
	else
	{
		cancel(%obj.SaveVictimSched);
		%obj.isSaving = 0;
		%obj.savevictimcounter = 0;
		%victim.isBeingSaved = false;
		%obj.playthread(2,"root");
		return;
	}	
}

function EventidePlayer::EventideAppearance(%this,%obj,%client)
{
	if(%obj.isSkinwalker && isObject(%obj.victimreplicatedclient)) %funcclient = %obj.victimreplicatedclient;
    else %funcclient = %client;	
	
	%obj.hideNode("ALL");
	%obj.unHideNode((%funcclient.chest ? "femChest" : "chest"));	
	%obj.unHideNode((%funcclient.rhand ? "rhook" : "rhand"));
	%obj.unHideNode((%funcclient.lhand ? "lhook" : "lhand"));
	%obj.unHideNode((%funcclient.rarm ? "rarmSlim" : "rarm"));
	%obj.unHideNode((%funcclient.larm ? "larmSlim" : "larm"));
	%obj.unHideNode("headskin");

	if($pack[%funcclient.pack] !$= "none")
	{
		%obj.unHideNode($pack[%funcclient.pack]);
		%obj.setNodeColor($pack[%funcclient.pack],%funcclient.packColor);
	}
	if($secondPack[%funcclient.secondPack] !$= "none")
	{
		%obj.unHideNode($secondPack[%funcclient.secondPack]);
		%obj.setNodeColor($secondPack[%funcclient.secondPack],%funcclient.secondPackColor);
	}
	
	switch$(%client.hat)
	{
		case 1: if(%client.accent)
				{
					%obj.mountImage("helmetimage",2,1,addTaggedString(getColorName(%funcclient.hatColor)));	
					%obj.currentHat = "helmet";
				}		
				else
				{
					%obj.mountImage("hoodieimage",2,1,addTaggedString(getColorName(%funcclient.hatColor)));	
					%obj.currentHat = "hoodie";
				}
		default: %obj.mountImage($hat[%funcclient.hat] @ "image",2,1,addTaggedString(getColorName(%funcclient.hatColor)));
	}	
	
	if(%funcclient.hip)
	{
		%obj.unHideNode("skirthip");
		%obj.unHideNode("skirttrimleft");
		%obj.unHideNode("skirttrimright");
	}
	else
	{
		%obj.unHideNode("pants");
		%obj.unHideNode((%funcclient.rleg ? "rpeg" : "rshoe"));
		%obj.unHideNode((%funcclient.lleg ? "lpeg" : "lshoe"));
	}

	%obj.setHeadUp(0);
	if(%funcclient.pack+%funcclient.secondPack > 0) %obj.setHeadUp(1);

	if (%obj.bloody["lshoe"]) %obj.unHideNode("lshoe_blood");
	if (%obj.bloody["rshoe"]) %obj.unHideNode("rshoe_blood");
	if (%obj.bloody["lhand"]) %obj.unHideNode("lhand_blood");
	if (%obj.bloody["rhand"]) %obj.unHideNode("rhand_blood");
	if (%obj.bloody["chest_front"]) %obj.unHideNode((%funcclient.chest ? "fem" : "") @ "chest_blood_front");
	if (%obj.bloody["chest_back"]) %obj.unHideNode((%funcclient.chest ? "fem" : "") @ "chest_blood_back");

	%obj.setFaceName(%funcclient.faceName);
	%obj.setDecalName(%funcclient.decalName);

	%obj.setNodeColor("headskin",%funcclient.headColor);	
	%obj.setNodeColor("chest",%funcclient.chestColor);
	%obj.setNodeColor("femChest",%funcclient.chestColor);
	%obj.setNodeColor("pants",%funcclient.hipColor);
	%obj.setNodeColor("skirthip",%funcclient.hipColor);	
	%obj.setNodeColor("rarm",%funcclient.rarmColor);
	%obj.setNodeColor("larm",%funcclient.larmColor);
	%obj.setNodeColor("rarmSlim",%funcclient.rarmColor);
	%obj.setNodeColor("larmSlim",%funcclient.larmColor);
	%obj.setNodeColor("rhand",%funcclient.rhandColor);
	%obj.setNodeColor("lhand",%funcclient.lhandColor);
	%obj.setNodeColor("rhook",%funcclient.rhandColor);
	%obj.setNodeColor("lhook",%funcclient.lhandColor);	
	%obj.setNodeColor("rshoe",%funcclient.rlegColor);
	%obj.setNodeColor("lshoe",%funcclient.llegColor);
	%obj.setNodeColor("rpeg",%funcclient.rlegColor);
	%obj.setNodeColor("lpeg",%funcclient.llegColor);
	%obj.setNodeColor("skirttrimright",%funcclient.rlegColor);
	%obj.setNodeColor("skirttrimleft",%funcclient.llegColor);

	//Set blood colors.
	%obj.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("lhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("rhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_back", "0.7 0 0 1");
	%obj.setNodeColor("femchest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("femchest_blood_back", "0.7 0 0 1");
}

function EventidePlayer::TunnelVision(%this,%obj,%bool)
{
	if(!isObject(%obj) || !isObject(%obj.client) || %obj.getState() $= "Dead") return;

	if(!%obj.TunnelFOV) %obj.TunnelFOV = %this.defaultTunnelFOV;

	if(%bool) 
	{		
		if(%obj.tunnelvision <= 1)
		{
			%obj.tunnelvision = mClampF(%obj.tunnelvision+0.1,0,1);
			commandToClient( %obj.client, 'SetVignette', true, "0 0 0" SPC %obj.tunnelvision);
			%obj.client.setcontrolcamerafov(mClampF(%obj.TunnelFOV--,50,%this.defaultTunnelFOV));
		}

		if(%obj.tunnelvision >= 1) return;
	}
	else
	{
		if(%obj.tunnelvision > 0)
		{
			%obj.tunnelvision = mClampF(%obj.tunnelvision-0.1,0,1);
			%obj.client.setcontrolcamerafov(mClampF(%obj.TunnelFOV++,50,%this.defaultTunnelFOV));
			commandToClient(%obj.client, 'SetVignette', true, "0 0 0" SPC %obj.tunnelvision);
		}

		if(%obj.tunnelvision <= 0)
		{
			commandToClient(%obj.client, 'SetVignette', $EnvGuiServer::VignetteMultiply, $EnvGuiServer::VignetteColor);		
			return;
		}
	}

	%obj.tunnelvisionsched = %this.schedule(50, TunnelVision, %obj, %bool);	
}

function EventidePlayer::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType)
{			
	if(%obj.getState() !$= "Dead" && %damage+%obj.getdamageLevel() >= %this.maxDamage && %damage < mFloor(%this.maxDamage/1.33) && %obj.downedamount < 2)
    {        
        %obj.setDatablock("EventidePlayerDowned");
        %obj.setHealth(100);
		%obj.downedamount++;
        return;
    }

	if(%obj.isSkinwalker) %obj.addhealth(%damage*5);	
	if(%obj.downedamount && %obj.getdamageLevel() < 25) %obj.downedamount = 0;

    Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType);

	if(%damage >= %this.maxDamage*2 && %obj.getState() $= "Dead" && %damageType != $DamageType::Suicide) 
	{
		%obj.spawnExplosion("goryExplosionProjectile",%obj.getScale());
		%obj.schedule(1,delete);
	}
}

function EventidePlayerDowned::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	%this.DownLoop(%obj);
    %obj.playthread(0,sit);

	if(isObject(%obj.client) && isObject(%minigame = getMinigamefromObject(%obj)) && isObject(%teams = %minigame.teams))
	{				
		for(%i = 0; %i < %teams.getCount(); %i++) if(isObject(%team = %teams.getObject(%i)))
		{
			if(strstr(strlwr(%team.name), "hunter") != -1) %hunterteam = %team;
			
			if(strstr(strlwr(%team.name), "survivor") != -1)
			for(%j = 0; %j < %team.numMembers; %j++) if(isObject(%member = %team.member[%j].player) && !%member.getdataBlock().isDowned) %livingcount++;
		}
		
		if(!%livingcount) %minigame.endRound(%hunterteam);
	}		
}

function EventidePlayerDowned::DownLoop(%this,%obj)
{ 
	if(isobject(%obj) && %obj.getstate() !$= "Dead" && %obj.getdataBlock().isDowned)
	{
		if(!%obj.isBeingSaved)
		{
			%obj.addhealth(-2.5);
			%obj.setdamageflash(0.25);

			if(%obj.lastcry+10000 < getsimtime())
			{
				%obj.lastcry = getsimtime();
				%obj.playaudio(0,"norm_scream" @ getRandom(0,4) @ "_sound");
				%obj.playthread(3,"plant");
			}
		}
	
		cancel(%obj.downloop);
		%obj.downloop = %this.schedule(1000,DownLoop,%obj);
	}
	else return;
}

function EventidePlayer::onDisabled(%this,%obj)
{
	EventidePlayerDowned::onDisabled(%this,%obj);
}

function EventidePlayerDowned::onDisabled(%this,%obj)
{	
	Parent::onDisabled(%this,%obj);

	if(isObject(%client = %obj.client)) 
	{
		%obj.ghostclient = %client;
		commandToClient(%client, 'SetVignette', $EnvGuiServer::VignetteMultiply, $EnvGuiServer::VignetteColor);
		%obj.client.setcontrolcamerafov(%this.defaultTunnelFOV);
	}
	if(%obj.radioEquipped) serverPlay3d("radio_unmount_sound",%obj.getPosition());	
	if(%obj.markedforRenderDeath)
	{
		%obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");
		%obj.schedule(1,delete);
	}

	if(isObject(%minigame = getMinigamefromObject(%obj)))
	{
		if(isObject(%client))
		for(%i=0;%i<%obj.getDatablock().maxTools;%i++) if(isObject(%item = %obj.tool[%i]) && %item.image.isRitual)
		{						
			%pos = %obj.getPosition();
			%posX = getWord(%pos,0);
			%posY = getWord(%pos,1);
			%posZ = getWord(%pos,2);
			%vec = %obj.getVelocity();
			%vecX = getWord(%vec,0);
			%vecY = getWord(%vec,1);
			%vecZ = getWord(%vec,2);
			%item = new Item()
			{
				dataBlock = %item;
				position = %pos;
			};
			%itemVec = %vec;
			%itemVec = vectorAdd(%itemVec,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
			%item.BL_ID = %client.BL_ID;
			%item.minigame = %minigame;
			%item.spawnBrick = -1;
			%item.setVelocity(%itemVec);						

			if(!isObject(DroppedItemGroup))
			{
				new SimGroup(DroppedItemGroup);
				missionCleanUp.add(DroppedItemGroup);
			}
			DroppedItemGroup.add(%item);
		}		
		
		for(%i = 0; %i < %minigame.numMembers; %i++)
		if(isObject(%member = %minigame.member[%i]) && !%obj.markedforRenderDeath) %member.play2D("fallen_survivor_sound");
		else %member.play2D("render_kill_sound");
	}	
}

function EventidePlayer::onRemove(%this,%obj)
{
	EventidePlayerDowned::onRemove(%this,%obj);
}

function EventidePlayerDowned::onRemove(%this,%obj)
{	
	Parent::onRemove(%this,%obj);

	if(%obj.markedForShireZombify && isObject(%obj.ghostclient) && isObject(%obj.ghostClient.minigame))
	{
		%bot = new AIPlayer()
		{
			dataBlock = "ShireZombieBot";
			minigame = %obj.ghostClient.minigame;
			ghostclient = %obj.ghostclient;
		};

		%bot.setTransform(%obj.getTransform());
		%obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");

		if(!isObject(Shire_BotGroup))
		{
    		new SimGroup(Shire_BotGroup);
    		missionCleanup.add(Shire_BotGroup);
			Shire_BotGroup.add(%bot);
		}
		else if(!Shire_BotGroup.isMember(%bot)) Shire_BotGroup.add(%bot);
	}
}