function onObjectCollisionTest(%obj, %col)//This function is part of the ObjectCollision.dll
{
	if(isObject(%obj) && isObject(%col))
	{
		if((%obj.getType() & $TypeMasks::PlayerObjectType) && (%col.getType() & $TypeMasks::PlayerObjectType))
		if(%obj.getdataBlock().getName() $= "PlayerGrabberNoJump" || (%obj.getdataBlock().getName() $= "PlayerSkinwalker" && %col.getDamagePercent() < 0.5)) return true;
		else return false;
	}		
	
	return true;	
}

package Eventide_MainPackage
{
	function ItemData::onAdd(%this, %obj)
	{
		if (!%obj.static) itemEmitterLoop(%obj);		
		return parent::onAdd(%this, %obj);
	}

	function Item::schedulePop (%obj)
	{		
		if(MiniGameGroup.getCount() || (isObject(Slayer_MiniGameHandlerSG) && Slayer_MiniGameHandlerSG.getCount()))
		{
			if(!isObject(DroppedItemGroup))
			{
				new SimGroup(DroppedItemGroup);
				missionCleanUp.add(DroppedItemGroup);
			}
			DroppedItemGroup.add(%obj);
			return;
		}
	}

	function onMissionEnded(%this, %a, %b, %c, %d)
	{
		$PFGlassInit = false;
		$PFRTBInit = false;
		return Parent::onMissionEnded(%this, %a, %b, %c, %d);
	}

	function ServerCmdTeamMessageSent(%client, %message)
	{
		if(!$Pref::Server::ChatMod::lchatEnabled)
		{
			Parent::ServerCmdTeamMessageSent(%client, %message);
			return;
		}

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);

		if(%message !$= "0")
		{
			if(isObject(%client.player))
			{
				%client.player.playThread(3,talk);
				%client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);

				if(%client.player.radioEquipped) ChatMod_RadioMessage(%client, %message, true);
				if(isObject(%client.minigame)) ChatMod_TeamLocalChat(%client, %message);
				else if(!%client.player.radioEquipped) messageClient(%client,'',"\c5You must be in a minigame to team chat.");
			}
			else messageClient(%client,'',"\c5You are dead. You must respawn to use team chat.");
			%client.lastMessageSent = %client;
		}			
	}	

	function ServerCmdStartTalking(%client)
	{
		if($Pref::Server::ChatMod::lchatEnabled) return;
		else parent::ServerCmdStartTalking(%client);
	}	

	function serverCmdMessageSent(%client,%message)
	{		
		if(%client.customtitlecolor $= "") %color = "FFFFFF";
        else %color = %client.customtitlecolor;

        if(%client.customtitlefont $= "") %font = "Palatino Linotype";
        else %font = %client.customtitlefont;

        if(%client.customtitlebitmap $= "") %bitmap = "";
        else %bitmap = %client.customtitlebitmap;

        if(%client.customtitle !$= "") %client.clanPrefix = %bitmap @ "<color:" @ %color @ ">" @ "<font:" @ %font @ ":25>" @ %client.customtitle SPC "";
        else if(%client.customtitlebitmap !$= "") %client.clanPrefix = %bitmap @ "";
		else %client.clanPrefix = "";

		if(!$Pref::Server::ChatMod::lchatEnabled)
		{
			Parent::ServerCmdMessageSent(%client, %message);
			return;
		}		

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);
		if(%message !$= "0")
		{
			if(ChatMod_getGlobalChatPerm(%client) && getSubStr(%message, 0, 1) $= "&") 
			{
				messageAll('', "\c6[\c4GLOBAL\c6] \c3" @ %client.name @ "\c6: " @ getSubStr(%message, 1, strlen(%message)));
				if(isObject(%client.player))
				{				
					%client.player.playThread(3,talk);
					%client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);
				}
			}
			else if(isObject(%client.player))
			{
				ChatMod_LocalChat(%client, %message);
				%client.player.playThread(3,talk);
				%client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);
			}
			else for(%i=0; %i<clientGroup.getCount(); %i++) if(isObject(%targetClient = clientGroup.getObject(%i)) && !isObject(%targetClient.player)) 
			chatMessageClientRP(%targetClient, "", "\c7[DEAD] "@ %client.name, "", %message);
		}		
		%client.lastMessageSent = %message;		
	}

	function Observer::onTrigger (%this, %obj, %trigger, %state)
	{		
		if(isObject(%client = %obj.getControllingClient ()) && isObject(%player = %client.Player)) 
		if(%player.stunned) return;
		
		Parent::onTrigger (%this, %obj, %trigger, %state);
	}

	function Player::Pickup(%obj,%item)
	{		
		//Skinwalker players should not be able to pickup items
		if(%obj.isSkinwalker || %obj.getdataBlock().isKiller) return false;
		%parent = Parent::Pickup(%obj,%item);

		if(%parent == 1)
		if(%obj.getDatablock().getName() $= "EventidePlayer" && isObject(getMinigameFromObject(%obj))) 
		{
			if(isObject(%brick = %item.spawnBrick)) %brick.setEmitter();
			%item.delete();
		}		
	}

	function serverCmdLight(%client)
	{
		if(isObject(%client.player) && %client.player.getdataBlock().isKiller) return;
		Parent::serverCmdLight(%client);		
	}

	function Armor::onImpact(%this, %obj, %col, %vec, %force)
	{
		Parent::onImpact(%this, %obj, %col, %vec, %force);

		if(%obj.isInvisible) return;

		if(%force < 40) serverPlay3D("impact_medium" @ getRandom(1,3) @ "_sound",%obj.getPosition());
		else serverPlay3D("impact_hard" @ getRandom(1,3) @ "_sound",%obj.getPosition());

		%oScale = getWord(%obj.getScale(),2);
		%forcescale = %force/25 * %oscale;
		%obj.spawnExplosion(pushBroomProjectile,%forcescale SPC %forcescale SPC %forcescale);

		if(%obj.getState() !$= "Dead" && getWord(%vec,2) > %obj.getdataBlock().minImpactSpeed)
        serverPlay3D("impact_fall_sound",%obj.getPosition());		
	}

	function ServerCmdPlantBrick (%client)
	{		
		if(isObject(%client.player) && %client.player.getdataBlock().getName() $= "PlayerPuppetMaster" && isObject(PuppetGroup))
		{	
			if(%client.puppetnumber $= "") %client.puppetnumber = 0;

			if(isObject(PuppetGroup.getObject(%client.puppetnumber))) 
			{
				%client.getcontrolObject().schedule(1500,setActionThread,sit,1);
				%client.setcontrolobject(PuppetGroup.getObject(%client.puppetnumber));
				%client.puppetnumber++;
			}
			else
			{
				%client.getcontrolObject().schedule(1500,setActionThread,sit,1);
				%client.setcontrolobject(%client.player);
				%client.puppetnumber = 0;
			}			
		}

		Parent::ServerCmdPlantBrick(%client);
	}

	function player::setDamageFlash(%obj,%value)
	{
		if(!isObject(%obj.getControllingClient())) return;
		
		if(!%obj.ShireBlind && %value > 0.2) %value = 0.2;
		Parent::setDamageFlash(%obj,%value);
	}

	function Armor::onNewDatablock(%this,%obj)
	{		
		Parent::onNewDatablock(%this,%obj);

		if(%this.isKiller) 
		{
			%obj.onKillerLoop();		
			if(isObject(%obj.getMountedImage(2))) %obj.unmountImage(2);
			if(isObject(%client = %obj.client)) %this.schedule(500,EventideAppearance,%obj,%client);			
		}

		if(isObject(%client = %obj.client) && !isObject(%obj.effectbot))
		{
			%obj.effectbot = new Player() 
    		{
    	    	dataBlock = "EmptyBot";
    		};
			%obj.mountobject(%obj.effectbot,5);			
			%obj.effectbot.mountImage(%client.effect,0);

			%obj.effectbot.setNetFlag(6,true);
			for(%i = 0; %i < clientgroup.getCount(); %i++) if(isObject(%client = clientgroup.getObject(%i)) && %client.player != %obj)
			%obj.effectbot.clearScopeToClient(%client);			
		}

		if(%this != %obj.getDatablock() && %this.maxTools != %obj.client.lastMaxTools)
		{
			%obj.client.lastMaxTools = %this.maxTools;
			commandToClient(%obj.client,'PlayGui_CreateToolHud',%this.maxTools);
		}
		
		if(isObject(%obj.client) && %this.maxTools != %obj.client.lastMaxTools)
		{
			%obj.client.lastMaxTools = %this.maxTools;
			commandToClient(%obj.client,'PlayGui_CreateToolHud',%this.maxTools);
			for(%i=0;%i<%this.maxTools;%i++)
			{
				if(isObject(%obj.tool[%i])) messageClient(%obj.client,'MsgItemPickup',"",%i,%obj.tool[%i].getID(),1);
				else messageClient(%obj.client,'MsgItemPickup',"",%i,0,1);
			}
		}

		if(%this.rideable || isEventPending(%obj.peggstep)) return;
		%obj.touchcolor = "";
		%obj.surface = parseSoundFromNumber($Pref::Server::PF::defaultStep, %obj);
		%obj.isSlow = 0;
		%obj.peggstep = schedule(50,0,PeggFootsteps,%obj);
	}

	function Armor::onDisabled(%this, %obj, %state)
	{
        Parent::onDisabled(%this, %obj, %state);

		for(%i = 0; %i < %obj.getMountedObjectCount(); %i++) 
		if(isObject(%obj.getMountedObject(%i)) && (%obj.getMountedObject(%i).getDataBlock().className $= "PlayerData")) %obj.getMountedObject(%i).delete();		

        if(isObject(%client = %obj.client) && isObject(%client.EventidemusicEmitter))
		{
			%client.EventidemusicEmitter.delete();        
        	%client.musicChaseLevel = 0;		
		}
		
		if(isObject(%killer = %obj.killer))
		{
			%killer.ChokeAmount = 0;
			%killer.victim = 0;
			%killer.playthread(3,"activate2");
			%obj.dismount();
			%obj.setVelocity(vectorscale(vectorAdd(%killer.getForwardVector(),"0 0 0.25"),15));		
		}
    }

	function Armor::onRemove(%this,%obj)
	{
		Parent::onRemove(%this,%obj);

		for(%i = 0; %i < %obj.getMountedObjectCount(); %i++) 
		if(isObject(%obj.getMountedObject(%i)) && (%obj.getMountedObject(%i).getDataBlock().className $= "PlayerData")) %obj.getMountedObject(%i).delete();
	}

	function fxDTSBrick::onActivate(%obj, %player, %client, %pos, %vec)
	{
		Parent::onActivate(%obj,%player,%client,%pos,%vec);

		if(isFunction(%obj.getDataBlock().getName(),onActivate)) %obj.getDataBlock().onActivate(%obj,%player,%client,%pos,%vec);
	}

	function fxDTSBrick::onRemove(%data, %brick)
	{		
		Parent::OnRemove(%data,%brick);
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
	}

	function fxDTSBrick::onDeath(%data, %brick)
	{		
	   	Parent::onDeath(%data, %brick);
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
	}	
	
	function serverCmdUseTool(%client, %tool)
	{		
		if(!%client.player.victim) return parent::serverCmdUseTool(%client, %tool);
	}

	function Slayer_MiniGameSO::endRound(%this, %winner, %resetTime)
	{
		Parent::endRound(%this, %winner, %resetTime);
		
		if(strlwr(%this.title) $= "eventide")
		{
			for(%i=0;%i<%this.numMembers;%i++)
			if(isObject(%client = %this.member[%i]) && %client.getClassName() $= "GameConnection") %client.play2d("round_end_sound");		 						
		}
	}

    function MiniGameSO::Reset(%minigame,%client)
	{
        Parent::Reset(%minigame,%client);

		if(strlwr(%minigame.title) $= "eventide")
		{
			for(%i=0;%i<%minigame.numMembers;%i++)
			if(isObject(%client = %minigame.member[%i]) && %client.getClassName() $= "GameConnection") 
			{
				%client.play2d("round_start_sound");		 			
				%client.escaped = false;
			}
			%minigame.centerprintall("<font:impact:40>\c3Eventide: The Hunt begins",2);
		}

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter))
        {
            %client.EventidemusicEmitter.delete();
            %client.musicChaseLevel = 0;
        }

		if(isObject(Shire_BotGroup)) Shire_BotGroup.delete();
		if(isObject(EventideShapeGroup)) EventideShapeGroup.delete();
		if(isObject(DroppedItemGroup)) DroppedItemGroup.delete();

		%minigame.randomizeEventideItems(true);
    }

    function MinigameSO::endGame(%minigame,%client)
    {
        Parent::endGame(%minigame,%client);

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter)) 
		{
			%client.EventidemusicEmitter.delete();
			%client.escaped = false;
		}
		if(isObject(Shire_BotGroup)) Shire_BotGroup.delete();

		%minigame.randomizeEventideItems(false);
    }

	function GameConnection::onConnect(%client)
	{
		parent::onConnect(%client);
		schedule(100,0,Eventide_loadEventideStats,%client);
	}

	function GameConnection::onClientEnterGame(%client)
	{
		parent::onClientEnterGame(%client);
		Eventide_loadEventideStats(%client);		
	}	

	function GameConnection::onClientLeaveGame(%client)
	{
		parent::onClientLeaveGame(%client);
		Eventide_storeEventideStats(%client);	
	}	

	function GameConnection::setControlObject(%this,%obj)
	{
		Parent::setControlObject(%this,%obj);
		if(%obj == %this.player && %obj.getDatablock().maxTools != %this.lastMaxTools)
		{
			%this.lastMaxTools = %obj.getDatablock().maxTools;
			commandToClient(%this,'PlayGui_CreateToolHud',%obj.getDatablock().maxTools);
		}
	}
	
	function gameConnection::applyBodyColors(%client) 
	{
		parent::applyBodyColors(%client);
		if(isObject(%player = %client.player) && %player.getDataBlock().isEventideModel) %player.getDataBlock().EventideAppearance(%player,%client);
	}
	function gameConnection::applyBodyParts(%client) 
	{
		parent::applyBodyParts(%client);
		if(isObject(%player = %client.player) && fileName(%player.getDataBlock().shapeFile) $= "Eventideplayer.dts") %player.getDataBlock().EventideAppearance(%player,%client);
	}

	function getBrickGroupFromObject(%obj)
	{
		if(isObject(%obj) && %obj.getClassName() $= "AIPlayer")		
		switch$(%obj.getDataBlock().getName())			
		{
			case "ShireZombieBot": return %obj.ghostclient.brickgroup;
			case "PuppetMasterPuppet": return %obj.client.brickgroup;
		}		

		Parent::getBrickGroupFromObject(%obj);
	}

	function Player::ActivateStuff (%player)//Not parenting, I made an overhaul of this function so it might cause compatibility issues...
	{
		Parent::ActivateStuff(%player);
		
		if(isObject(%player) && %player.getState() !$= "Dead" && isFunction(%player.getDataBlock().getName(),onActivate)) 
		%player.getDataBlock().onActivate(%player);
	}
};

if(isPackage(Eventide_MainPackage)) deactivatePackage(Eventide_MainPackage);
activatePackage(Eventide_MainPackage);