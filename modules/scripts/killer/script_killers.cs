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