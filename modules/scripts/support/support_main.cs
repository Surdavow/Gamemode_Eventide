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

	function Player::Pickup(%obj,%item)
	{		
		Parent::Pickup(%obj,%item);

		if(%obj.getDatablock().getName() $= "EventidePlayer" && isObject(getMinigameFromObject(%obj))) 
		%item.delete();
	}

	function ServerCmdDropTool(%client,%slot)
	{
		if(!isObject(getMinigameFromObject(%client))) return Parent::ServerCmdDropTool(%client, %slot);		

		if(isObject(EventideShapeGroup) && EventideShapeGroup.getCount() >= $EventideRitualAmount) return Parent::ServerCmdDropTool(%client, %slot);
	
		if(isObject(%player = %client.Player) && isObject(%item = %player.tool[%slot]))
		{
			if(!%item.image.isRitual) return Parent::ServerCmdDropTool(%client, %slot);
			if(!%item.canDrop || !%player.getDatablock().isEventideModel) return Parent::ServerCmdDropTool(%client, %slot);

			%zScale = getWord (%player.getScale (), 2);
			%muzzlepoint = VectorAdd (%player.getPosition (), "0 0" SPC 1.5 * %zScale);
			%muzzlevector = %player.getEyeVector ();
			%muzzlepoint = VectorAdd (%muzzlepoint, %muzzlevector);
			%playerRot = rotFromTransform (%player.getTransform ());
			%thrownItem = new Item ("")
			{
				dataBlock = %item;
			};
			%thrownItem.setScale (%player.getScale ());
			MissionCleanup.add (%thrownItem);
			%thrownItem.setTransform (%muzzlepoint @ " " @ %playerRot);
			%thrownItem.setVelocity (VectorScale (%muzzlevector, 20 * %zScale));
			%thrownItem.miniGame = %client.miniGame;
			%thrownItem.bl_id = %client.getBLID ();
			%thrownItem.setCollisionTimeout(%player);
			if(!isObject(DroppedItemGroup))
			{
				new SimGroup(DroppedItemGroup);
				missionCleanUp.add(DroppedItemGroup);
			}
			DroppedItemGroup.add(%thrownItem);				
			if(%item.className $= "Weapon") %player.weaponCount -= 1;
			
			%player.tool[%slot] = 0;
			messageClient (%client, 'MsgItemPickup', '', %slot, 0);			
			if(isObject(%player.getMountedImage(%item.image.mountPoint))) %player.unmountImage (%item.image.mountPoint);			
		}
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

		%obj.schedule(10,onKillerLoop);
	}

	function Armor::onDisabled(%this, %obj, %state)
	{
        Parent::onDisabled(%this, %obj, %state);

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
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
		Parent::OnRemove(%data,%brick);
	}

	function fxDTSBrick::onDeath(%data, %brick)
	{
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
	   	Parent::onDeath(%data, %brick);
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