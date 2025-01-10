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

	function MiniGameSO::Reset(%obj, %client)
	{
		parent::Reset(%obj, %client);
		$Eventide_currentKiller = "";
	}
	
	function Observer::onTrigger (%this, %obj, %trigger, %state)
	{		
		if (%obj.getControllingClient().player.stunned)
		{
			return;
		}
		
		Parent::onTrigger (%this, %obj, %trigger, %state);
	}

	function Armor::onNewDatablock(%this,%obj)
	{		
		Parent::onNewDatablock(%this,%obj);
		
		if(%this.isEventideModel)
		{
			%this.schedule(33,killerCheck,%obj);
		}		
	}

	function Armor::onDisabled(%this, %obj, %state)
	{
        Parent::onDisabled(%this, %obj, %state);
		
		if (!isObject(%killer = %obj.killer))
		{
			return;
		}
		
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
		if(%hit == %obj || %hit == %obj.effectbot || VectorDist(%obj.getPosition(),%hit.getPosition()) > %radius || %hit.stunned) 
		{
			continue;
		}

		%typemasks = $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType;
		%obscure = containerRayCast(%obj.getEyePoint(),%hit.getHackPosition(),%typemasks, %obj);
		%dot = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%hit.getHackPosition(),%obj.getHackPosition())));				

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
			if(%this.onKillerHit(%obj,%hit) || %hit.getdataBlock().isDowned) 
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

function Armor::onKillerChaseStart(%this, %obj)
{
	//Hello, World!
}

function Armor::onKillerChase(%this, %obj, %chasing)
{
	//Hello world
}

function Armor::onKillerChaseEnd(%this, %obj)
{
	//Hello, World!
}


function Armor::onKillerHit(%this,%obj,%hit)
{
	//Hello world
}

function Armor::onIncapacitateVictim(%this, %obj, %victim, %killed)
{
	//Hello, World!
}

function Armor::onEnterStun(%this, %obj)
{
	//Hello, World!
}

function Armor::onExitStun(%this, %obj)
{
	//Hello, World!
}

function Armor::onAllRitualsPlaced(%this, %obj)
{
	//Hello, World!
}

function Armor::onRoundEnd(%this, %obj, %won)
{
	//Hello, World!
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

// Support function to handle victim's state changes during chase
function Armor::handleVictimChaseState(%this, %victim, %obj, %canSeeKiller, %victimDistance, %searchDistance, %isActiveChase)
{
    if(%isActiveChase)
    {
        if(%victimDistance < %searchDistance/2.5)
        {
            %victim.playthread(2, "talk");
            
            if(%victimDistance < %searchDistance/4)
            {
				%viewNormal = vectorNormalize(vectorSub(%obj.getEyePoint(), %victim.getMuzzlePoint(2)));
                %dot = vectorDot(%victim.getEyeVector(), %viewNormal);
                // Handle panic sounds when victim sees killer
                if((%dot > 0.45) && %victim.lastChaseCall < getSimTime())
                {
                    %genderSound = (!%victim.client.chest) ? "male" : "female";
                    %genderSoundAmount = (!%victim.client.chest) ? 3 : 5;
                    %sound = %genderSound @ "_shock" @ getRandom(1, %genderSoundAmount) @ "_sound";
                    %victim.playaudio(0, %sound);        
                    %victim.lastChaseCall = getSimTime() + getRandom(1000, 5000);
                }
            }
            
            // Update victim's face to scared
            if(isObject(%victim.faceConfig))
            {
                if(%victim.faceConfig.subCategory $= "" && $Eventide_FacePacks[%victim.faceConfig.category, "Scared"] !$= "")
                {
                    %victim.createFaceConfig($Eventide_FacePacks[%victim.faceConfig.category, "Scared"]);
                }
                
                if(%victim.faceConfig.isFace("Scared"))
                {
                    %victim.faceConfig.dupeFaceSlot("Neutral", "Scared");                    
                }                    
            }
            
            // Handle victim's chase music
            if($Pref::Server::Eventide::chaseMusicEnabled && isObject(%victim.client))
            {
                if(%victim.chaseLevel != 2)
                {
                    %victim.client.SetChaseMusic(%obj.getDataBlock().killerChaseLvl2Music, true);
                }
                %victim.TimeSinceChased = getSimTime();
                cancel(%victim.client.StopChaseMusic);
                %victim.client.StopChaseMusic = %victim.client.schedule(6000, StopChaseMusic);
                %victim.chaseLevel = 2;
            }
        }
    }
    else
    {
        %chaseEndGracePeriod = 4000;
        %victimChaseExpired = (%victim.TimeSinceChased + %chaseEndGracePeriod) < getSimTime();
        
        if(%victimChaseExpired)
        {
            %victim.playthread(2, "root");
            // Reset victim's face back to neutral when chase ends
            if(isObject(%victim.faceConfig) && %victim.faceConfig.face["Neutral"].faceName $= "Scared") 
            {                        
                %victim.faceConfig.resetFaceSlot("Neutral");                    
            }
            
            // Handle stepping down victim's music
            if($Pref::Server::Eventide::chaseMusicEnabled && isObject(%victim.client))
            {
                if(%victim.chaseLevel != 1)
                {
                    %victim.client.SetChaseMusic(%obj.getDataBlock().killerChaseLvl1Music, false);
                }
                cancel(%victim.client.StopChaseMusic);
                %victim.client.StopChaseMusic = %victim.client.schedule(6000, StopChaseMusic);
                %victim.chaseLevel = 1;
            }
        }
    }
}

// Support function to manage killer's chase state
function Armor::handleKillerChaseState(%this, %obj, %chasingVictims, %isActiveChase)
{
    if(%isActiveChase)
    {
        if(!%obj.isChasing)
        {
            %this.onKillerChaseStart(%obj);
            %obj.isChasing = true;
        }
        %obj.TimeSinceChased = getSimTime();
        %this.onKillerChase(%obj, true);
        
        // Handle killer's chase music
        if($Pref::Server::Eventide::chaseMusicEnabled && isObject(%obj.client) && %chasingVictims)
        {    
            if(%obj.chaseLevel != 2)
            {
                %obj.client.SetChaseMusic(%obj.getDataBlock().killerChaseLvl2Music, false);
            }
            cancel(%obj.client.StopChaseMusic);
            %obj.client.StopChaseMusic = %obj.client.schedule(6000, StopChaseMusic);
            %obj.chaseLevel = 2;
        }
    }
    else
    {
        %chaseEndGracePeriod = 4000;
        %killerChaseExpired = (%obj.TimeSinceChased + %chaseEndGracePeriod) < getSimTime();
        
        if(%killerChaseExpired)
        {
            if(%obj.isChasing)
            {
                %this.onKillerChaseEnd(%obj);
                %obj.isChasing = false;
            }
            %this.onKillerChase(%obj, false);
            
            // Handle stepping down killer's music
            if($Pref::Server::Eventide::chaseMusicEnabled && isObject(%obj.client) && !%chasingVictims)
            {
                if(%obj.chaseLevel != 1)
                {
                    %obj.client.SetChaseMusic(%obj.getDataBlock().killerChaseLvl1Music, false);
                }
                cancel(%obj.client.StopChaseMusic);
                %obj.client.StopChaseMusic = %obj.client.schedule(6000, StopChaseMusic);
                %obj.chaseLevel = 1;
            }
        }
    }
}

// Main function refactored to use support functions
function Armor::killerContainerRadiusSearch(%this, %obj)
{
    %chasingVictims = 0;
    %searchDistance = 50;
    initContainerRadiusSearch(%obj.getMuzzlePoint(0), %searchDistance, $TypeMasks::PlayerObjectType);

    while(%victim = containerSearchNext())
    {
        // Skip invalid conditions
        %victimDatablock = %victim.getDataBlock();
        if(!isObject(getMinigamefromObject(%victim)) || %victimDatablock.isKiller || %victimDatablock.isDowned || %victim.getState() $= "Dead") 
        {
            continue;
        }
        
        %typemasks = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType;
        %dot = vectorDot(%obj.getEyeVector(), vectorNormalize(vectorSub(%victim.getMuzzlePoint(2), %obj.getEyePoint())));
        %canSeeVictim = !isObject(containerRayCast(%obj.getEyePoint(), %victim.getMuzzlePoint(2), %typemasks, %obj));
        %victimDistance = containerSearchCurrDist();

        %isActiveChase = %dot > 0.45 && %canSeeVictim && !%obj.isInvisible;
        if(%isActiveChase)
        {
            %chasingVictims++;
        }

        // Update states for both killer and victim
        %this.handleKillerChaseState(%obj, %chasingVictims, %isActiveChase);
        %this.handleVictimChaseState(%victim, %obj, %canSeeVictim, %victimDistance, %searchDistance, %isActiveChase);
    }
}

function Armor::playKillerLoopActions(%this,%obj)
{	
	// Handle raising the arms
	if (%this.killerRaiseArms && %obj.isChasing != %obj.raiseArms) 
	{    				
		%obj.playThread(1, %obj.isChasing ? "armReadyBoth" : "root");
		%obj.raiseArms = %obj.isChasing;
	}
	
	// Handle killer sounds
	if ($Pref::Server::Eventide::killerSoundsEnabled && !%obj.isCrouched() && %obj.lastKillerSoundTime + getRandom(7000, 10000) < getSimTime())
	{                       
		if (!%obj.isInvisible) 
		{
			// Determine if chasing or idle sounds should be played
			%soundType = %obj.isChasing ? %this.killerChaseSound : %this.killerIdleSound;
			%soundAmount = %obj.isChasing ? %this.killerChaseSoundAmount : %this.killerIdleSoundAmount;

			if(%soundType !$= "") 
			{
				%obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
				%obj.playThread(3, "plant");
				%obj.lastKillerSoundTime = getSimTime();
			}
		}
	}
}

// Function that manages the behavior of the killer, handling its state, playing sounds, and scheduling future actions.
function Armor::onKillerLoop(%this, %obj)
{
    // Skip if invalid state
    if (!isObject(%obj) || %obj.getState() $= "Dead" || !isObject(%minigame = getMinigamefromObject(%obj)))
	{
		return;
	}

	// Update UI and schedule next loop
    if (isObject(%obj.client)) 
	{
		%this.killerGUI(%obj, %obj.client);
	}

    if(%obj.getDataBlock().isKiller)
    {	
		// Handle killer container search
		%this.killerContainerRadiusSearch(%obj);
		%this.playKillerLoopActions(%obj);
    }

	// Schedule next loop, preventing duplication
    cancel(%obj.onKillerLoop);
    %obj.onKillerLoop = %this.schedule(500, onKillerLoop, %obj);
}

function GameConnection::SetChaseMusic(%client, %songname, %ischasing)
{
    if(!isObject(%client) || !isObject(%songname)) 
	{
		return;    
	}
    
	//Delete the old emitter, if it's playing other music.
	%currentMusicEmitter = %client.EventidemusicEmitter;
	if(isObject(%currentMusicEmitter)) 
	{
		if(%currentMusicEmitter.profile $= %songname)
		{
			return;
		}
		else
		{
			%client.EventidemusicEmitter.delete();
		}
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
	adjustObjectScopeToAll(%client.EventidemusicEmitter, false, %client);
		
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
			if(%client.player.getDamagePercent() > 0.33 && $Eventide_FacePacks[%client.player.faceConfig.category, "Hurt"] !$= "")
			{
				%client.player.createFaceConfig($Eventide_FacePacks[%client.player.faceConfig.category, "Hurt"]);
			}
			else
			{
				%client.player.createFaceConfig($Eventide_FacePacks[%client.player.faceConfig.category]);
			}		
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