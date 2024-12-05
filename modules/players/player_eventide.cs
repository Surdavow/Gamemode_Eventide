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
	tunnelFOVIncrease = 20;

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

function EventidePlayer::assignClass(%this,%obj,%class)
{
	if(!isObject(%obj) || !isObject(%obj.client) || %class $= "") return;

	commandToClient(%obj.client,'PlayGui_CreateToolHud',(%class $= "hoarder") ? 5 : %this.maxTools);

	%formatString = "<font:impact:40><color:FFFF00>";
	%firstString = "You acquired a";

	switch$(%class)
	{
		case "mender":  %healitem = (getRandom(1)) ? GauzeItem.getID() : ZombieMedpackItem.getID();
						%obj.tool[0] = %healitem;
         				messageClient(%obj.client,'MsgItemPickup','',0,%healitem);
						%obj.client.centerprint(%formatString @ "Class: Mender <br>" @ %firstString SPC "medical item and can revive survivors faster!",4);

		case "runner": 	%obj.setTempSpeed();
						%obj.tool[0] = SodaItem.getID();
         				messageClient(%obj.client,'MsgItemPickup','',0,SodaItem.getID());
						%obj.client.centerprint(%formatString @ "Class: Runner <br>" @ %firstString SPC "soda and can run slightly faster!",4);

		case "hoarder": %obj.hoarderToolCount = 5;
						%obj.tool[0] = DCamera.getID();
         				messageClient(%obj.client,'MsgItemPickup','',0,DCamera.getID());
						%obj.client.centerprint(%formatString @ "Class: Hoarder <br>" @ %firstString SPC "camera and have 5 slots!",4);

		case "fighter":	%obj.pseudoHealth = 75;
						%obj.tool[0] = sm_poolCueItem.getID();
         				messageClient(%obj.client,'MsgItemPickup','',0,sm_poolCueItem.getID());
						%obj.client.centerprint(%formatString @ "Class: Fighter <br>" @ %firstString SPC "pool cue, can shove further and can take 1 hit before getting damaged!",4);

		case "tinkerer": %obj.tool[0] = MonkeyWrench.getID();
         				 messageClient(%obj.client,'MsgItemPickup','',0,MonkeyWrench.getID());
						 %obj.tool[1] = StunGun.getID();
         				 messageClient(%obj.client,'MsgItemPickup','',1,StunGun.getID());
						 %obj.client.centerprint(%formatString @ "Class: Tinkerer <br>" @ %firstString SPC "monkey wrench, stungun, use the wrench to repair generators faster!",4);
	}
}

function EventidePlayer::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);

	%obj.schedule(33,setEnergyLevel,0);
	%obj.setScale("1 1 1");
	%this.scheduleNoQuota(33,createBillboard,%obj);	
}

function EventidePlayer::createBillboard(%this,%obj)
{
	if(!isObject(%obj.billboardbot))
	{
		if(!isObject(Eventide_MinigameGroup)) missionCleanUp.add(new SimGroup(Eventide_MinigameGroup));

		%obj.billboardbot = new Player() 
		{ 
			dataBlock = "EmptyPlayer";
			source = %obj;		
		};

		%obj.mountObject(%obj.billboardbot,5);

		%obj.billboardbot.light = new fxLight()
		{
			dataBlock = "blankBillboard";
		};

		%obj.billboardbot.light.setTransform(%obj.billboardbot.getTransform());
		%obj.billboardbot.light.attachToObject(%obj.billboardbot);

		// Force the light to be visible only to the survivors, and not the killers
		for(%i = 0; %i < clientgroup.getCount(); %i++) if(isObject(%client.player))		
		{
			if(isObject(%client = clientgroup.getObject(%i))) 
			%obj.billboardbot.light.ScopeToClient(%client);

			else if (%client.player.getdataBlock().isKiller || %client.player $= %obj) 
			%obj.billboardbot.light.ClearScopeToClient(%client);
		}

		Eventide_MinigameGroup.add(%obj.billboardbot.light);
		Eventide_MinigameGroup.add(%obj.billboardbot);
	}
	
	else
	{
		cancel(%obj.billboardbot.lightschedule0);
		cancel(%obj.billboardbot.lightschedule1);
		cancel(%obj.billboardbot.lightschedule2);
		%obj.billboardbot.light.setdatablock("blankBillboard");
	}
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
	%triggerTime = getSimTime();
	%obj.setEnergyLevel(%obj.getEnergyLevel()-4);

	// Reset the delay if the player waits long enough, 3.5 seconds
	if(%triggerTime - %obj.staminaTime > 3500) 
	{
		%obj.staminaCount = 0;
		%obj.staminaTime = 0;
	}
	else
	{
		%obj.staminaCount += 0.25;
		%obj.staminaTime = (%triggerTime+200)+(10*%obj.staminaCount);

		// Show the vignette when the player is is exhausted
		if(%obj.staminaCount >= 5) 
		{
			// Only enable the tunnel vision once
			if(%obj.staminaCount == 5) %this.tunnelVision(%obj,true);

			// Reset the stamina after 4 seconds, cancel first to avoid double scheduling
			cancel(%obj.resetStamina);
			%obj.resetStamina = %this.schedule(4000,resetStamina,%obj);
		}		
	}

	// When the player is possessed by the renowned, perform some actions
	if(%obj.isPossessed) 
	{		
		%obj.playthread(3,"activate2");
		%obj.AntiPossession = mClampF(%obj.AntiPossession+1, 0, 15);
		EventidePlayer_BreakFreePrint(%obj.client,%obj.AntiPossession/2);		

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

function EventidePlayer::resetStamina(%this,%obj)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead") return;

	%obj.staminaCount = 0;
	%obj.staminaTime = 0;
	%this.tunnelVision(%obj,false);
}

function EventidePlayer::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%press)
	{
		switch(%trig)
		{
			case 0:	if (isObject(%obj.getMountedImage(0))) return;
			
					%eyePoint = %obj.getEyePoint();
					%endPoint = vectoradd(%obj.getEyePoint(),vectorscale(%obj.getEyeVector(),5*getWord(%obj.getScale(),2)));
					%masks = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
			
					%ray = containerRayCast(%eyePoint, %endPoint,%masks,%obj);
					if(isObject(%ray) && (%ray.getType() & $TypeMasks::PlayerObjectType) && %ray.getdataBlock().isDowned && !%ray.isBeingSaved)
					{
						%obj.isSaving = %ray;
						%obj.playthread(2,"armReadyRight");
						%ray.isBeingSaved = true;
						%this.SaveVictim(%obj,%ray,%press);
					}

			case 4: if(%obj.isSkinwalker)
					{
						if(%obj.getEnergyLevel() >= %this.maxEnergy && !isObject(%obj.victim) && !isEventPending(%obj.monsterTransformschedule))
						PlayerSkinwalker.monsterTransform(%obj,true);
					}		
					else
					{
						%triggerTime = getSimTime();

						if(%triggerTime - %obj.staminaTime > 3500)//Reset the delay if the player waits long enough, 3.5 seconds
                    	{
                        	%obj.staminaCount = 0;
	                        %obj.staminaTime = 0;
						}

						if(%obj.staminaTime < %triggerTime && %obj.getEnergyLevel() >= %this.maxEnergy/4)//Shoving
						{
							%obj.setEnergyLevel(%obj.getEnergyLevel()-20);
							%obj.staminaCount++;
							%obj.staminaTime = (%triggerTime+400)+(40*%obj.staminaCount);
							%soundpitch = getRandom(50,125);

							if(%obj.staminaCount >= 5)
							{
								cancel(%obj.resetStamina);
								%soundpitch = getRandom(50,80);
								if(getRandom(1,4) == 1) %obj.playaudio(3,"PainCrySound");
								%obj.resetStamina = %this.schedule(4000,resetStamina,%obj);

								if(%obj.staminaCount == 5)
								{							
									%this.tunnelVision(%obj,true);
									%obj.resetStamina = %this.schedule(4000,resetStamina,%obj);
								}								
							}
							
							%obj.playthread(3,"activate2");
							$oldTimescale = getTimescale();
							setTimescale((%soundpitch*0.01) * $oldTimescale);
							serverPlay3D("melee_swing" @ getRandom(1,2) @ "_sound",%obj.getHackPosition());
							setTimescale($oldTimescale);
							
							%pos = %obj.getEyePoint();
							%radius = 0.25;
							%eyeVec = %obj.getEyeVector();
							%mask = $TypeMasks::PlayerObjectType;

							initContainerRadiusSearch(%pos,%radius,%mask);
							while(%hit = containerSearchNext())
							{
								%obscure = containerRayCast(%obj.getEyePoint(),%hit.getHackPosition(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
								%dot = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%hit.getHackPosition(),%obj.getHackPosition())));
		
								if(%hit == %obj || isObject(%obscure) || %dot < 0.5) continue;
								if(%hit.getState() $= "Dead") continue;

								serverPlay3D("melee_shove_sound",%hit.getHackPosition());
								%hit.playThread(3,"jump");
								
								if(!%obj.shoveForce) %obj.shoveForce = 1;
								%exhausted = (%obj.staminaCount >= 5) ? 2 : 1;
								%forwardimpulse = (((%obj.survivorclass $= "fighter") ? 950 : 800) / %exhausted) * %obj.shoveForce;
								%zimpulse = (((%obj.survivorclass $= "fighter") ? 325 : 200) / %exhausted) * %obj.shoveForce;
								%hit.applyimpulse(%hit.getPosition(),VectorAdd(VectorScale(%obj.getEyeVector(),%forwardimpulse),"0 0 " @ %zimpulse));
							}												
						}
					
					}
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
			%time = (%obj.survivorClass $= "mender") ? 250 : 1000;
			%obj.SaveVictimSched = %this.schedule(%time,SaveVictim,%obj,%victim,%bool);
		}
		else
		{
			%obj.savevictimcounter = 0;
			if(isObject(%obj.client)) %obj.client.centerprint("<color:FFFFFF><font:impact:40>You revived" SPC %victim.client.name,1);
			if(isObject(%victim.client)) %victim.client.centerprint("<color:FFFFFF><font:impact:40>You were revived by" SPC %obj.client.name,1);
			%victim.setHealth(75);
			if(%victim.survivorclass $= "fighter") %victim.pseudoHealth = 75;
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
			%newhat = (%funcclient.accent ? "helmet" : "hoodie1");
			%obj.unHideNode(%newhat);
			%obj.setNodeColor(%newhat,%funcclient.hatColor);
		}
		else
		{
			%obj.unHideNode(%hatName);
			%obj.setNodeColor(%hatName,%funcclient.hatColor);
		}			
	}
	
	if(%funcclient.hip) %obj.unHideNode("skirt");
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

	//Face system functionality: prevent face from being overwritten by an avatar update.
	if(isObject(%obj.faceConfig))
	{
		%neededFacePack = (%obj.client.chest ? $Eventide_FacePacks["female"] : $Eventide_FacePacks["male"]);
		if(%obj.faceConfig.getFacePack() !$= %neededFacePack)
		{
			//If the player updated their avatar, give them a new face pack to reflect it.
			%obj.createFaceConfig(%neededFacePack);
		}
		if(%obj.faceConfig.currentFace !$= "")
		{
			%obj.faceConfigShowFace(%obj.faceConfig.currentFace);
		}
	}
	else
	{
		%obj.setFaceName(%funcclient.faceName);
	}
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

function EventidePlayer::tunnelVision(%this,%obj,%bool)
{
	if(!isObject(%obj) || !isObject(%obj.client) || %obj.getState() $= "Dead") return;

	if(!%obj.TunnelFOV) %obj.TunnelFOV = %tunnelVisionFOV;

	if(%bool) 
	{		
		%obj.tunnelVision = mClampF(%obj.tunnelVision + 0.1, 0, 1);
		commandToClient(%obj.client, 'SetVignette', true, "0 0 0" SPC %obj.tunnelVision);

		if (%obj.tunnelVision >= 1) return;
	}
	else if (!%obj.chaseLevel)
	{
		if(%obj.tunnelVision > 0)
		{
			%obj.tunnelVision = mClampF(%obj.tunnelVision - 0.1, 0, 1);
		    commandToClient(%obj.client, 'SetVignette', true, "0 0 0" SPC %obj.tunnelVision);
		}
		else
		{
			commandToClient(%obj.client, 'SetVignette', $EnvGuiServer::VignetteMultiply, $EnvGuiServer::VignetteColor);
			return;
		}
	}

	%obj.tunnelVisionsched = %this.schedule(50, tunnelVision, %obj, %bool);	
}

function EventidePlayer::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType)
{
	// If the damage received too much damage and the player is not already incapacitated, check some conditions to see if they should be
	if(%obj.getState() !$= "Dead" && %damage+%obj.getdamageLevel() >= %this.maxDamage && %damage < mFloor(%this.maxDamage/1.33) && %obj.downedamount < 1)
    {        
        %obj.setDatablock("EventidePlayerDowned");
		%obj.setHealth(100);
		%obj.downedamount++;	
			
		if(isObject(%minigame = getMinigamefromObject(%obj))) 
		{
			%minigame.playSound("outofbounds_sound");

			// This will only work team based Slayer minigames.
			if(isObject(%teams = %minigame.teams))
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

		// Return here, or else the player will die after this condition is met
        return;
    }

    Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType);

	//Pseudo health for the fighter class, gives the player a temporary health boost until they are hurt again
	if(%obj.pseudoHealth)
	{
		%obj.pseudoHealth -= %damage;
		%obj.addhealth(mAbs(%damage)*2);
		%obj.mountimage("HealImage",3);
		%obj.setwhiteout(0.1);
		
		if(isObject(%obj.client)) %obj.client.play2D("printfiresound");				
	}
	
	if(%damage && %obj.isSkinwalker) 
	{
		%obj.setHealth(%this.maxDamage);

		//Uh oh, the disguise is about to be broken, now that the player has been hurt.
		if(getRandom(1,4) == 1) 
		{
			%obj.playaudio(3,"skinwalker_pain_sound");
			if(!isObject(%obj.victim) && !isEventPending(%obj.monsterTransformschedule)) PlayerSkinwalker.monsterTransform(%obj,true);
		}
	}

	//Face system functionality: play a pained facial expression when the player is hurt, and switch to hurt facial expression afterward 
	//if enough damage has been received.
	if(isObject(%obj.faceConfig))
	{
		if(%obj.getDamagePercent() > 0.33 && $Eventide_FacePacks[%obj.faceConfig.category, "Hurt"] !$= "")
		%obj.createFaceConfig($Eventide_FacePacks[%obj.faceConfig.category, "Hurt"]);		

		if(%obj.faceConfig.isFace("Pain"))		
		%obj.schedule(33, "faceConfigShowFace", "Pain"); //This needs to be delayed for whatever reason. Blinking doesn't start otherwise.
		
	}
}

function EventidePlayerDowned::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	
	%this.DownLoop(%obj);
    %obj.playthread(0,sit);
}

function EventidePlayerDowned::DownLoop(%this,%obj)
{ 
	if(isobject(%obj) && %obj.getstate() !$= "Dead" && %obj.getdataBlock().isDowned)
	{
		if(!%obj.isBeingSaved)
		{
			if(isObject(%obj.billboardbot.light))
			{
				%obj.billboardbot.lightschedule0 = %obj.billboardbot.light.schedule(440,setdatablock,"redLight");
				%obj.billboardbot.lightschedule1 = %obj.billboardbot.light.schedule(450,setdatablock,"downedBillboard");				
				%obj.billboardbot.lightschedule2 = %obj.billboardbot.light.schedule(400,setdatablock,"blankBillboard");
			} 			

			%obj.addHealth(-1);
			%obj.setDamageFlash(0.25);

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
		
	for (%j = 0; %j < 4; %j++) %obj.unmountimage(%j); // Remove all mounted images
	%obj.playThread(1, "Death1"); //TODO: Quick-fix for corpses standing up on death. Need to create a systematic way of using animation threads.
	if(isObject(%obj.billboardbot)) %obj.billboardbot.delete();

	// Let the killer know that a survivor has been killed
	if(isObject(%killer = getCurrentKiller().client)) 
	{
		%killer.client.PlaySkullFrames();
		%killer.client.play2D("elimination_sound");
	}	

	// Only do this if the client exists
	if(isObject(%obj.client))
	{
		%funcclient = (isObject(%obj.ghostclient)) ? %obj.ghostclient : %obj.client;
		commandToClient(%funcclient, 'SetVignette', $EnvGuiServer::VignetteMultiply, $EnvGuiServer::VignetteColor);	
		
		if(isObject(%minigame = getMinigamefromObject(%obj)))
		{
			// Drop all of the player's tools
			%inventoryToolCount = (%obj.hoarderToolCount) ? %obj.hoarderToolCount : %obj.getDataBlock().maxTools;
			for(%i = 0; %i < %inventoryToolCount; %i++) if(isObject(%item = %obj.tool[%i]))
			{
				//Play a sound for the radio being dropped
				if(%obj.tool[%i].getName() $= "RadioItem") 
				serverPlay3d("radio_unmount_sound",%obj.getPosition());	

				%item = new Item()
				{
					dataBlock = %item;
					position = %obj.getPosition();
					velocity = %obj.getVelocity();
					BL_ID = %funcclient.BL_ID;
					minigame = %minigame;
					spawnBrick = 0;
				};			

				if(!isObject(Eventide_MinigameGroup)) missionCleanUp.add(new SimGroup(Eventide_MinigameGroup));
				Eventide_MinigameGroup.add(%item);
			}

			// Varying conditions on how the player was killed, do not return on either condition if the player is already marked for death
			if(%obj.markedforRenderDeath || %obj.markedForShireZombify)
			{
				if(%obj.markedforRenderDeath) %minigame.playSound("render_kill_sound");
	
				if(%obj.markedForShireZombify)
				{
					%bot = new AIPlayer()
					{
						dataBlock = "ShireZombieBot";
						minigame = %obj.ghostClient.minigame;
						ghostclient = %obj.ghostclient;
					};				
	
					if(!isObject(Eventide_MinigameGroup)) missionCleanup.add(new SimGroup(Eventide_MinigameGroup));
					Eventide_MinigameGroup.add(%bot);
					%bot.setTransform(%obj.getTransform());
				}
	
				%obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");
				%obj.schedule(33,delete);
			}
		}
	}	
}