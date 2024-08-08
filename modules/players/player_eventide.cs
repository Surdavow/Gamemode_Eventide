datablock TSShapeConstructor(EventideplayerDts) 
{
	baseShape = "./models/eventideplayer.dts";
	sequence0 = "./models/default.dsq";
	sequence1 = "./models/default_melee.dsq";
};

datablock PlayerData(EventidePlayer : PlayerStandardArmor)
{
	shapeFile = EventideplayerDts.baseShape;
	uiName = "Eventide Player";

	// To be used for a skinwalker mimick
	rightclickicon = "color_skinwalker_reveal";
	leftclickicon = "color_melee";
	rightclickspecialicon = "";
	leftclickspecialicon = "color_consume";	

	uniformCompatible = true;
	isEventideModel = true;
	showEnergyBar = false;
	firstpersononly = false;
	canJet = false;
	renderFirstPerson = false;
	defaultTunnelFOV = 110;

	rechargeRate = 0.375;
	maxTools = 3;
	maxWeapons = 3;
	jumpForce = 0;
	
	cameramaxdist = 2.25;
    cameratilt = 0.1;
	maxfreelookangle = 2.5;

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

function EventidePlayer::PulsingScreen(%this,%obj)
{
	if((!isObject(%obj) || %obj.getclassname() !$= "Player" || %obj.getState() $= "Dead") || %obj.getdamageLevel() < 25)
	return;	

	if(isObject(%obj.client)) %obj.client.play2D("survivor_heartbeat_sound");
	%obj.setdamageflash(0.125);
	%obj.PulsingScreen = %this.schedule(850,PulsingScreen,%obj);
}

function EventidePlayer::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(1,setEnergyLevel,0);
	%obj.setScale("1 1 1");	

	if(isObject(%obj.billboardbot)) %ob.billboardbot.delete();
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

function EventidePlayer_BreakFreePrint(%funcclient,%amount)
{
    if(!isobject(%funcclient)) return;
	
	%addsymbol = "";
    %symbol = "|";
	for(%i = 0; %i < %amount; %i++) %addsymbol = %addsymbol @ %symbol;

    %funcclient.centerprint("<color:FFFFFF><font:impact:40> Control yourself! <br><color:00e100>" @ %addsymbol,1);
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

function EventidePlayer_SaveCounterPrint(%funcclient,%amount)
{
    if(!isobject(%funcclient)) return;
	
	%addsymbol = "";
    %symbol = "|";
	for(%i = 0; %i < %amount; %i++) %addsymbol = %addsymbol @ %symbol;

    %funcclient.centerprint("<color:FFFFFF><font:impact:40> Get up! <br><color:00e100>" @ %addsymbol,1);
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
			%victim.setHealth(75);			
			%victim.setDatablock("EventidePlayer");			
			%victim.playthread(0,"root");
			if(%victim.downedamount >= 1) %victim.getdataBlock().PulsingScreen(%victim);
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

function EventidePlayer::EventideAppearance(%this,%obj,%funcclient)
{
	if(%obj.isSkinwalker && isObject(%obj.victimreplicatedclient)) %funcclient = %obj.victimreplicatedclient;
    else %funcclient = %funcclient;	
	
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

	if(%funcclient.hat)
	{
		%hatName = $hat[%funcclient.hat];
		%funcclient.hatString = %hatName;

		if(%funcclient.hat == 1)
		{
			if(%funcclient.accent) %newhat = "helmet";
			else %newhat = "hoodie1";
			%obj.unHideNode(%newhat);
			%obj.setNodeColor(%newhat,%funcclient.hatColor);
		}
		else
		{
			%obj.unHideNode(%hatName);
			%obj.setNodeColor(%hatName,%funcclient.hatColor);
		}			
	}
	
	if(%funcclient.hip)
	{
		%obj.unHideNode("skirt");
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
	%obj.setNodeColor("skirt",%funcclient.hipColor);	
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

function EventidePlayerDowned::EventideAppearance(%this,%obj,%funcclient)
{
	EventidePlayer::EventideAppearance(%this,%obj,%funcclient);
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
	if(%obj.getState() !$= "Dead" && %damage+%obj.getdamageLevel() >= %this.maxDamage && %damage < mFloor(%this.maxDamage/1.33) && %obj.downedamount < 1)
    {        
        %obj.setDatablock("EventidePlayerDowned");
        %obj.setHealth(100);
		%obj.downedamount++;		
        return;
    }

    Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType);

	if(%obj.isSkinwalker) 
	{
		%obj.setHealth(%this.maxDamage);
		if(getRandom(1,4) == 1) %obj.playaudio(3,"skinwalker_pain_sound");
	}	

	if(%obj.downedamount && %obj.getdamageLevel() < 25) %obj.downedamount = 0;	

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

	if(!isObject(%obj.billboardbot))
	{
		%obj.billboardbot = new Player() 
		{ 
			dataBlock = "EmptyPlayer";
			source = %obj;
			slotToMountBot = 5;
			lightToMount = "downedBillboard";
		};
	}

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
			%obj.addhealth(-1);
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

	if(isObject(%obj.billboardbot)) %obj.billboardbot.delete();

	if(isObject(%funcclient = %obj.client))
	{
		%obj.ghostclient = %funcclient;
		commandToClient(%funcclient, 'SetVignette', $EnvGuiServer::VignetteMultiply, $EnvGuiServer::VignetteColor);
		%obj.client.setcontrolcamerafov(%this.defaultTunnelFOV);

		if(%obj.markedForShireZombify && isObject(%funcclient.minigame))
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

			%obj.schedule(1,delete);
		}		
	}
	if(%obj.radioEquipped) serverPlay3d("radio_unmount_sound",%obj.getPosition());	
	if(%obj.markedforRenderDeath)
	{
		%obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");
		%obj.schedule(1,delete);
	}

	if(isObject(%minigame = getMinigamefromObject(%obj)))
	{
		%obj.unmountimage(0);
		%obj.unmountimage(1);
		if(isObject(%funcclient))
		for(%i=0;%i<%obj.getDatablock().maxTools;%i++) if(isObject(%item = %obj.tool[%i]))
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
			//%itemVec = vectorAdd(%itemVec,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
			%item.BL_ID = %funcclient.BL_ID;
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
		if(isObject(%member = %minigame.member[%i]) && %obj.markedforRenderDeath) %member.play2D("render_kill_sound");
	}	
}

function EventidePlayer::onRemove(%this,%obj)
{
	EventidePlayerDowned::onRemove(%this,%obj);
}

function EventidePlayerDowned::onRemove(%this,%obj)
{	
	Parent::onRemove(%this,%obj);
}