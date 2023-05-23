datablock TSShapeConstructor(EventideplayerDts) 
{
	baseShape = "./models/eventideplayer.dts";
	sequence0 = "./models/default.dsq";
};

datablock PlayerData(EventidePlayer : PlayerStandardArmor)
{
	shapeFile = EventideplayerDts.baseShape;
	uiName = "Eventide Player";
	uniformCompatible = true;//For slayer uniform compatibility
	isEventideModel = true;
	showEnergyBar = false;
	canJet = false;
	rechargeRate = 0.375;
	maxTools = 3;
	maxWeapons = 3;
	jumpDelay = 0;
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
	
	Parent::onImpact(%this, %obj, %col, %vec, %force);	
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
			%victim.setHealth(50);
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
    if(%obj.isSkinwalker && isObject(%obj.victimreplicatedclient)) Parent::EventideAppearance(%this,%obj,%obj.victimreplicatedclient);
    else Parent::EventideAppearance(%this,%obj,%client);
}

function EventidePlayer::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType)
{			
	if(%obj.getState() !$= "Dead" && %damage+%obj.getdamageLevel() >= %this.maxDamage && %damage < mFloor(%this.maxDamage/1.33))
    {        
        %obj.setDatablock("EventidePlayerDowned");
        %obj.setHealth(100);
        return;
    }

	if(%obj.isSkinwalker) %obj.addhealth(%damage*5);	

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
			%obj.addhealth(-1.25);
			%obj.setdamageflash(0.25);

			if(%obj.lastcry+10000 < getsimtime())
			{
				%obj.lastcry = getsimtime();
				%obj.playaudio(0,"grabber_scream_sound");
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

	if(isObject(%client = %obj.client)) %obj.ghostclient = %client;
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