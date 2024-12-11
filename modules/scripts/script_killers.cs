package Eventide_Killers
{
	function getBrickGroupFromObject(%obj)
	{	
		// Workaround for AIPlayers
		if(isObject(%obj) && %obj.getClassName() $= "AIPlayer")
		{
			switch$(%obj.getDataBlock().getName())			
			{
				case "ShireZombieBot": return %obj.ghostclient.brickgroup;
				case "PuppetMasterPuppet": return %obj.client.brickgroup;
			}
		}

		Parent::getBrickGroupFromObject(%obj);
	}

	function serverCmdUseTool(%client, %tool)
	{	
		// If the player is not a victim, then continue
		if(!%client.player.victim)
		{
			return parent::serverCmdUseTool(%client, %tool);
		}
	}	

	function Player::addItem(%player, %image, %client)
	{
		// Check if the player is not a skinwalker, only then can they pick up items
		if(!%obj.isSkinwalker)
		{
			Parent::addItem(%player, %image, %client);		
		}
		
    }
	function serverCmdLight(%client)
	{
		if(!isObject(%client.player)) return;

		// Ensure the player is valid and is the Puppet Master
	    if (%client.player.getDataBlock().getName() $= "PlayerPuppetMaster" && isObject(Eventide_MinigameGroup))
	    {
			if(isObject(%client.player.getMountedImage(2)) && %client.player.getMountedImage(2).getName() $= "sm_stunImage")
			return;

	        // Populate the temporary puppet list
	        for (%i = 0; %i < Eventide_MinigameGroup.getCount(); %i++)	        
			if (isObject(%puppet = Eventide_MinigameGroup.getObject(%i)) && %puppet.getDataBlock().getName() $= "PuppetMasterPuppet")
			%puppetList[%puppetCount++] = %puppet;	
	        	        
			if (%client.player.puppetIndex <= %puppetCount)
	        {
	            %currentPuppet = %puppetList[%client.player.puppetIndex];
				%client.getControlObject().schedule(1500, setActionThread, sit, 1);
				%client.setControlObject(%currentPuppet);
				%client.player.puppetIndex++;
	        }
			else
			{				
				%client.player.puppetIndex = 1;				
				%client.getControlObject().schedule(1500, setActionThread, sit, 1);
				%client.setControlObject(%client.player);
			}

			return;
	    }
		else if(%client.player.getdataBlock().isKiller) return;

		Parent::serverCmdLight(%client);		
	}

	function MiniGameSO::Reset(%obj, %client)
	{
		parent::Reset(%obj, %client);
		$Eventide_currentKiller = "";
	}
	
	function Observer::onTrigger (%this, %obj, %trigger, %state)
	{		
		if (%obj.getControllingClient().player.stunned) return; 		
		
		Parent::onTrigger (%this, %obj, %trigger, %state);
	}

	function Armor::onNewDatablock(%this,%obj)
	{		
		Parent::onNewDatablock(%this,%obj);
		
		if(%this.isEventideModel) 
		%this.schedule(33,killerCheck,%obj);
	}

	function Armor::onDisabled(%this, %obj, %state)
	{
        Parent::onDisabled(%this, %obj, %state);
		
		if (!isObject(%killer = %obj.killer)) return;
		
		%killer.ChokeAmount = 0;
		%killer.victim = 0;
		%killer.playthread(3,"activate2");
		%obj.dismount();
		%obj.setVelocity(vectorscale(vectorAdd(%killer.getForwardVector(),"0 0 0.25"),15));		
    }
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Killers)) deactivatePackage(Eventide_Killers);
activatePackage(Eventide_Killers);

function getCurrentKiller()
{
	return $Eventide_currentKiller;
}

function Armor::killerMelee(%this,%obj,%radius)
{
	if(%obj.getState() $= "Dead" || %obj.isInvisible || %obj.getEnergyLevel() < %this.maxEnergy/8 || %obj.lastMeleeTime+1250 > getSimTime()) 
	{
		return;
	}
		
	%obj.lastMeleeTime = getSimTime();
	%meleeAnim = (%this.shapeFile $= EventideplayerDts.baseShape) ? getRandom(1,4) : getRandom(1,2);
	%hackPos = %obj.getHackPosition();
	%obj.setEnergyLevel(%obj.getEnergyLevel()-%this.maxEnergy/6);	
	%obj.playthread(2,"melee" @ %meleeAnim);							

	if(%this.meleetrailskin !$= "") 
	{
		%meleetrailangle = %this.meleetrailangle[%meleeAnim];
		%obj.spawnKillerTrail(%this.meleetrailskin,%this.meleetrailoffset,%meleetrailangle,%this.meleetrailscale);		
	}	

	if(%this.killerMeleesound !$= "") 
	{
		serverPlay3D(%this.killerMeleesound @ getRandom(1,%this.killerMeleesoundamount) @ "_sound",%hackPos);
	}
	
	if(%this.killerWeaponSound !$= "") 
	{
		serverPlay3D(%this.killerWeaponSound @ getRandom(1,%this.killerWeaponSoundamount) @ "_sound",%hackPos);
	}	

	initContainerRadiusSearch(%obj.getMuzzlePoint(0), %radius, $TypeMasks::PlayerObjectType);		
	while(%hit = containerSearchNext())
	{
		if(%hit == %obj || %hit == %obj.effectbot || VectorDist(%obj.getPosition(),%hit.getPosition()) > %radius) 
		{
			continue;
		}

		%typemasks = $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType;
		%obscure = containerRayCast(%obj.getEyePoint(),%hit.getPosition(),%typemasks, %obj);
		%dot = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%hit.getHackPosition(),%obj.getPosition())));				

		if(isObject(%obscure) && %this.hitobscureprojectile !$= "")
		{								
			%c = new Projectile()
			{
				dataBlock = %this.hitobscureprojectile;
				initialPosition = posfromraycast(%obscure);
				sourceObject = %obj;
				client = %obj.client;
			};
			
			MissionCleanup.add(%c);
			%c.explode();
			return;
		}

		if(%dot < 0.4)
		{
			continue;
		}

		if((%hit.getType() && $TypeMasks::PlayerObjectType) && minigameCanDamage(%obj,%hit))								
		{
			switch$(%this.getName())
			{
				case "PlayerSkinWalker":	if(!isObject(%obj.victim) && %hit.getdataBlock().isDowned)
											{
												if(%hit.getDamagePercent() > 0.05)
												{
													if(isObject(%hit.client)) 
													{
														%obj.stunned = true;
														%hit.client.setControlObject(%hit.client.camera);
														%hit.client.camera.setMode("Corpse",%hit);
													}
													%obj.victim = %hit;
													%obj.victimreplicatedclient = %hit.client;																
													%obj.playthread(1,"eat");
													%obj.playthread(2,"talk");
													%obj.playaudio(1,"skinwalker_grab_sound");
													%obj.mountobject(%hit,6);
													%hit.schedule(2250,kill);
													%hit.setarmthread("activate2");
													%hit.schedule(2250,spawnExplosion,"goryExplosionProjectile",%hit.getScale()); 
													%hit.schedule(2295,kill);        
													%hit.schedule(2300,delete);        
													%obj.schedule(2250,playthread,1,"root");
													%obj.schedule(2250,playthread,2,"root");
													%obj.schedule(2250,setField,victim,0);
													%this.schedule(2250,EventideAppearance,%obj,%obj.client);
													return;
												}
												else continue;													
											}

				case "PlayerSkullwolf":	if(%hit.getDamagePercent() > 0.25 && %hit.getdataBlock().isDowned)
										{
											%this.eatVictim(%obj,%hit);
											return;
										}
			}
				
			if(isObject(%obj.hookrope))
			{
				%obj.hookrope.delete();
			}

			if(%hit.getdataBlock().isDowned) 
			{
				continue;
			}
			
			if(%this.killerMeleehitsound !$= "")
			{
				%obj.stopaudio(3);
				%obj.playaudio(3,%this.killerMeleehitsound @ getRandom(1,%this.killerMeleehitsoundamount) @ "_sound");		
			}

			if(%this.hitprojectile !$= "")
			{
				%effect = new Projectile()
				{
					dataBlock = %this.hitprojectile;
					initialPosition = %hit.getHackPosition();
					initialVelocity = vectorNormalize(vectorSub(%hit.getHackPosition(), %obj.getEyePoint()));
					scale = %obj.getScale();
					sourceObject = %obj;
				};
				
				MissionCleanup.add(%effect);
				%effect.explode();
			}

			
			%hit.setvelocity(vectorscale(VectorNormalize(vectorAdd(%obj.getForwardVector(),"0" SPC "0" SPC "0.15")),15));								
			%hit.damage(%obj, %hit.getHackPosition(), 50*getWord(%obj.getScale(),2), $DamageType::Default);					
			
			%obj.setTempSpeed(0.3);	
			%obj.schedule(2500,setTempSpeed,1);
		}			
	}	
}

/// This function is called every tick on the killer player
/// It will check if the player is valid, dead or not a killer
/// If the player is valid, it will update the appearance and send a message to all players
/// It will also handle the killer's light
function Armor::killerCheck(%this,%obj)
{
	// Return early if the object is invalid, dead, or not a killer
	if(!isObject(%obj) || %obj.getState() $= "Dead" || !%this.isKiller) 
	{
		return;
	}

	// Update the appearance, only if the client exists
	if(isObject(%obj.client)) 
	{
		%this.EventideAppearance(%obj,%obj.client);
	}

	// Send an announcement message when the killer spawns
	// The message is sent with a chat message and a sound effect
	if(!%obj.firstMessageSpawn && isObject(%minigame = getMinigamefromObject(%obj)))
	{
		switch(getRandom(1,4))
		{
			case 1: %message = "The hunter has arrived.";
			case 2: %message = "Ready yourselves, the hunter has arrived.";
			case 3: %message = "Prepare yourselves, it is coming.";
			case 4: %message = %this.killerSpawnMessage;
		}

		%minigame.chatMsgAll("<font:Impact:30>\c0" @ %message);
		%minigame.playSound("round_start_sound");

		// Set the flag to prevent the message from being sent again
		%obj.firstMessageSpawn = true;
	}

	// Handle the killer's light
	// The killer's light is only visible to the killer itself
	// If the killer is invisible, the light is not created
	if(!%obj.isInvisible)
	{
		// If the light does not exist, create it
		if(!isObject(%obj.light))
		{
			%obj.light = new fxLight()
			{
				dataBlock = %this.killerlight;
				source = %obj;
			};
		}

		// Attach the light to the player and set a net flag
		%obj.light.attachToObject(%obj);
		%obj.light.setNetFlag(6,true);
		%obj.light.ScopeToClient(%client);

		// Handle scope to client, only the killer should see the light
		for(%i = 0; %i < clientgroup.getCount(); %i++)
		{
			if(isObject(%client = clientgroup.getObject(%i))) 
			{
				if(%obj != %client.player) 
				{					
					// Clear the scope to client for clients that are not the killer
					%obj.light.clearScopeToClient(%client);
				}
			}
		}		
		
		// If the Eventide Minigame Group does not exist, create it
		if(!isObject(Eventide_MinigameGroup)) 
		{
			missionCleanUp.add(new SimGroup(Eventide_MinigameGroup));
		}

		// Add the light to the Eventide Minigame Group
		Eventide_MinigameGroup.add(%obj.light);
	}
	else if(isObject(%obj.light)) 
	{
		// Delete the light if the killer is invisible
		%obj.light.delete();
	}

	%this.onKillerLoop(%obj);
}

function Armor::onKillerChase(%this,%obj,%chasing)
{
	//Hello world
}

function Armor::onKillerHit(%this,%obj,%hit)
{
	//Hello world
}

// Function that manages the behavior of the killer, handling its state, playing sounds, and scheduling future actions.
function Armor::onKillerLoop(%this, %obj)
{
    // Skip if invalid state
    if (!isObject(%obj) || %obj.getState() $= "Dead" || !isObject(%minigame = getMinigamefromObject(%obj)))
	{
		return;
	}

    if(%this.isKiller)
    {	
        %chasingVictims = 0;
        initContainerRadiusSearch(%obj.getMuzzlePoint(0), 40, $TypeMasks::PlayerObjectType);

        // Process nearby players
        while(%victim = containerSearchNext())
        {
			// Skip invalid conditions
			if(!isObject(getMinigamefromObject(%victim)) || %victim.getDataBlock().isKiller || containerSearchCurrDist() > 40) {
				continue;
			}
			
			%typemasks = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType;
            %dot = vectorDot(%obj.getEyeVector(), vectorNormalize(vectorSub(%victim.getMuzzlePoint(2), %obj.getEyePoint())));
            %canSeeVictim = !isObject(containerRayCast(%obj.getEyePoint(), %victim.getMuzzlePoint(2), %typemasks, %obj));

            // If we can see the victim, play some music and perform some actions
            if (%dot > 0.45 && %canSeeVictim && !%obj.isInvisible)
            {
                %chasingVictims++;
                %obj.isChasing = true;
                %this.onKillerChase(%obj, true);

                if($Pref::Server::Eventide::chaseMusicEnabled)
                {
                    // Update victim's chase state
                    if (isObject(%victim.client))
                    {
                        if(%victim.chaseLevel != 2)
                        {
                            %victim.client.SetChaseMusic(%obj.getDataBlock().killerChaseLvl2Music, true);
                            %victim.chaseLevel = 2;
                        }
                        %victim.TimeSinceChased = getSimTime();
                        cancel(%victim.client.StopChaseMusic);
                        %victim.client.StopChaseMusic = %victim.client.schedule(6000, StopChaseMusic);
                    }

                    // Update killer's chase state
                    if (isObject(%obj.client) && %chasingVictims)
                    {
                        if(%obj.chaseLevel != 2)
                        {
                            %obj.client.SetChaseMusic(%obj.getDataBlock().killerChaseLvl2Music, false);
                            %obj.chaseLevel = 2;
                        }
                        cancel(%obj.client.StopChaseMusic);
                        %obj.client.StopChaseMusic = %obj.client.schedule(6000, StopChaseMusic);
                    }
                }

                // Update victim's face
                if(isObject(%victim.faceConfig))
                {
					if(%victim.faceConfig.subCategory $= "" && $Eventide_FacePacks[%victim.faceConfig.category, "Scared"] !$= "")                    
					%victim.createFaceConfig($Eventide_FacePacks[%victim.faceConfig.category, "Scared"]);

                    if(%victim.faceConfig.isFace("Scared")) 
					%victim.faceConfig.dupeFaceSlot("Neutral", "Scared");                    
                }
            }		
            else // If we cannot see the victim, stop music and perform some actions
            {
                // Update victim's chase state after 6 seconds
                if (isObject(%victim.client) && %victim.TimeSinceChased + 6000 < getSimTime())
                {
                	// Reset victim's face
					if(isObject(%victim.faceConfig) && %victim.faceConfig.face["Neutral"].faceName $= "Scared") 
					{
						%victim.faceConfig.resetFaceSlot("Neutral");
					}					
					
					if($Pref::Server::Eventide::chaseMusicEnabled)
					{
						if(%victim.chaseLevel != 1)
                    	{
                    	    %victim.client.SetChaseMusic(%obj.getDataBlock().killerChaseLvl1Music, false);
                    	    %victim.chaseLevel = 1;
                    	}

                    	cancel(%victim.client.StopChaseMusic);
                    	%victim.client.StopChaseMusic = %victim.client.schedule(6000, StopChaseMusic);
					}
                }

				// Update killer's chase state
                if (!%obj.isChasing && $Pref::Server::Eventide::chaseMusicEnabled)
                {
					%this.onKillerChase(%obj, false);

					// Update killer's chase state
					if(isObject(%obj.client) && !%chasingVictims)
					{
						if(%obj.chaseLevel != 1)
						{
							%obj.client.SetChaseMusic(%obj.getDataBlock().killerChaseLvl1Music, false);
							%obj.chaseLevel = 1;
						}
						cancel(%obj.client.StopChaseMusic);
						%obj.client.StopChaseMusic = %obj.client.schedule(6000, StopChaseMusic);
					}
                }
				
                %obj.isChasing = false;
            }
        }

		// Handle raising the arms
		if (%this.killerRaiseArms && %obj.isChasing != %obj.raiseArms) 
		{    				
			%obj.playThread(1, %obj.isChasing ? "armReadyBoth" : "root");
			%obj.raiseArms = %obj.isChasing;
		}

        // Handle killer sounds
        if (%obj.lastKillerSoundTime + getRandom(7000, 10000) < getSimTime())
        {                       
			if (!%obj.isInvisible && $Pref::Server::Eventide::killerSoundsEnabled) 
			{
			    // Determine if chasing or idle sounds should be played
				%soundType = %obj.isChasing ? %this.killerChaseSound : %this.killerIdleSound;
                %soundAmount = %obj.isChasing ? %this.killerChaseSoundAmount : %this.killerIdleSoundAmount;

                if (%soundType !$= "") 
				{
                	%obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
                }

				%obj.playThread(3, "plant");
			}

			%obj.lastKillerSoundTime = getSimTime();
        }
    }

    // Update UI and schedule next loop
    if (isObject(%obj.client)) 
	{
		%this.killerGUI(%obj, %obj.client);
	}

	// Schedule next loop, preventing duplication
    cancel(%obj.onKillerLoop);
    %obj.onKillerLoop = %this.schedule(500, onKillerLoop, %obj);
}

function Armor::killerGUI(%this,%obj,%client)
{	
	if (!isObject(%obj) || !isObject(%client))
	{
		return;
	}	

	// Some dynamic varirables
	%energylevel = %obj.getEnergyLevel();
	%leftclickstatus = (%energylevel >= 25) ? "hi" : "lo";
	%rightclickstatus = (%energylevel == %this.maxEnergy) ? "hi" : "lo";
	%leftclicktext = (%this.leftclickicon !$= "") ? "<just:left>\c6Left click" : "";
	%rightclicktext = (%this.rightclickicon !$= "") ? "<just:right>\c6Right click" : "";	

	// Regular icons
	%leftclickicon = (%this.leftclickicon !$= "") ? "<just:left><bitmap:" @ $iconspath @ %leftclickstatus @ %this.leftclickicon @ ">" : "";
	%rightclickicon = (%this.rightclickicon !$= "") ? "<just:right><bitmap:" @ $iconspath @ %rightclickstatus @ %This.rightclickicon @ ">" : "";

	%client.bottomprint(%leftclicktext @ %rightclicktext @ "<br>" @ %leftclickicon @ %rightclickicon, 1);
}

function GameConnection::SetChaseMusic(%client,%songname,%ischasing)
{
    if(!isObject(%client) || !isObject(%songname)) 
	{
		return;    
	}
    
	// Delete old emitter
	if(isObject(%client.EventidemusicEmitter)) 
	{
		%client.EventidemusicEmitter.delete();
	}

    %client.EventidemusicEmitter = new AudioEmitter()
    {
        position = "9e9 9e9 9e9";
        profile = %songname;
        volume = 1;
        type = 10;
        useProfileDescription = false;
        is3D = false;
    };
    MissionCleanup.add(%client.EventidemusicEmitter);
    %client.EventidemusicEmitter.scopeToClient(%client);
		
	if(isObject(%client.player) && %client.player.getdataBlock().getName() $= "EventidePlayer" && !%client.player.tunnelvision)
	{
		%client.player.getdataBlock().TunnelVision(%client.player,%ischasing);	
	}	
}

function GameConnection::PlaySkullFrames(%client,%frame)
{
    if(!isObject(%client) || %frame > 12)
	{
		return;
	}

	if(!%frame)
	{
		%frame = 1;
	}

	%client.centerprint("<br><br><bitmap:Add-ons/Server_SkullFrames/SkullFrame" @ %frame @ ">",0.2);

	// Schedule next frame, preventing duplication
	cancel(%client.SkullFrameSched);
	%client.SkullFrameSched = %client.schedule(60, PlaySkullFrames, %frame++);
}

function GameConnection::StopChaseMusic(%client)
{
    if(!isObject(%client))
	{
		return;
	}

    if(isObject(%client.EventidemusicEmitter))
	{
		%client.EventidemusicEmitter.delete();
	}

	// Handle survivor conditions
	if(isObject(%client.player) && %client.player.getdataBlock().getName() $= "EventidePlayer")
	{
		// Reset tunnelvision
		if(%client.player.tunnelvision)
		{
			%client.player.getdataBlock().TunnelVision(%client.player,false);
		}		

		//Face system functionality. Make the victim return to calm facial expressions when they are no longer being chased.
		if(isObject(%client.player.faceConfig) && %client.player.faceConfig.subCategory $= "Scared")
		{
			%client.player.createFaceConfig($Eventide_FacePacks[%client.player.faceConfig.category]);		
		}
	}

    %client.player.chaseLevel = 0;
    %client.musicChaseLevel = 0;
}

function Player::spawnKillerTrail(%this, %skin, %offset, %angle, %scale)
{
	%shape = new StaticShape()
	{
		dataBlock = KillerTrailShape;
		scale = %scale;
	};
	
	%shape.setSkinName(%skin);
	
	%rotation = relativeVectorToRotation(%this.getLookVector(), %this.getUpVector());
	%clamped = mClampF(firstWord(%rotation), -89.9, 89.9) SPC restWords(%rotation);		
	%local = %this.getHackPosition() SPC %clamped;
	%combined = %offset SPC eulerToQuat(%angle);
	%actual = matrixMultiply(%local, %combined);
	
	%shape.setTransform(%actual);
	%shape.playThread(0, "rotate");
	%shape.schedule(1000, delete);
	MissionCleanup.add(%shape);		
}