exec("./support_extraresources.cs");
exec("./script_chatsystem.cs");
exec("./script_gazeloop.cs");
exec("./script_killers.cs");
exec("./script_items.cs");
exec("./support_dataInstance.cs");
exec("./support_itemammo.cs");
exec("./support_statuseffect.cs");
exec("./support_stringutilities.cs");
exec("./script_peggyfootsteps.cs");


registerOutputEvent("GameConnection", "Escape", "", false);

function Player::SetSpeedModifier(%obj,%a)
{
	if(%obj.Speed_Modifier $= "") %obj.Speed_Modifier = 1;	

	if(%a <= 0) return;	

	%prev = %obj.Speed_Modifier;
	%curr = %obj.Speed_Modifier = %a;
	%mod = (1 / %prev) * %curr;
	%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() * %mod);
	%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() * %mod);
	%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() * %mod);
	%obj.setMaxCrouchForwardSpeed(%obj.getMaxCrouchForwardSpeed() * %mod);
	%obj.setMaxCrouchBackwardSpeed(%obj.getMaxCrouchBackwardSpeed() * %mod);
	%obj.setMaxCrouchSideSpeed(%obj.getMaxCrouchSideSpeed() * %mod);
	%obj.setMaxUnderwaterForwardSpeed(%obj.getMaxUnderwaterForwardSpeed() * %mod);
	%obj.setMaxUnderwaterBackwardSpeed(%obj.getMaxUnderwaterBackwardSpeed() * %mod);
	%obj.setMaxUnderwaterSideSpeed(%obj.getMaxUnderwaterSideSpeed() * %mod);
}

function Player::AddMoveSpeedModifier(%obj,%a)
{
	if(%obj.Speed_Modifier $= "") %obj.Speed_Modifier = 1;	

	%obj.SetSpeedModifier(%obj.Speed_Modifier + %a);
}

function Player::SetTempSpeed(%obj,%slowdowndivider)
{						
	if(!isObject(%obj) || %obj.getstate() $= "Dead") return;

	%datablock = %obj.getDataBlock();
	%obj.setMaxForwardSpeed(%datablock.MaxForwardSpeed*%slowdowndivider);
	%obj.setMaxSideSpeed(%datablock.MaxSideSpeed*%slowdowndivider);
	%obj.setMaxBackwardSpeed(%datablock.maxBackwardSpeed*%slowdowndivider);

	%obj.setMaxCrouchForwardSpeed(%datablock.maxForwardCrouchSpeed*%slowdowndivider);
  	%obj.setMaxCrouchBackwardSpeed(%datablock.maxSideCrouchSpeed*%slowdowndivider);
  	%obj.setMaxCrouchSideSpeed(%datablock.maxSideCrouchSpeed*%slowdowndivider);

 	%obj.setMaxUnderwaterBackwardSpeed(%datablock.MaxUnderwaterBackwardSpeed*%slowdowndivider);
  	%obj.setMaxUnderwaterForwardSpeed(%datablock.MaxUnderwaterForwardSpeed*%slowdowndivider);
  	%obj.setMaxUnderwaterSideSpeed(%datablock.MaxUnderwaterForwardSpeed*%slowdowndivider);	
}

function Armor::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");
	%obj.unHideNode((%client.chest ? "femChest" : "chest"));	
	%obj.unHideNode((%client.rhand ? "rhook" : "rhand"));
	%obj.unHideNode((%client.lhand ? "lhook" : "lhand"));
	%obj.unHideNode((%client.rarm ? "rarmSlim" : "rarm"));
	%obj.unHideNode((%client.larm ? "larmSlim" : "larm"));
	%obj.unHideNode("headskin");

	if($pack[%client.pack] !$= "none")
	{
		%obj.unHideNode($pack[%client.pack]);
		%obj.setNodeColor($pack[%client.pack],%client.packColor);
	}
	if($secondPack[%client.secondPack] !$= "none")
	{
		%obj.unHideNode($secondPack[%client.secondPack]);
		%obj.setNodeColor($secondPack[%client.secondPack],%client.secondPackColor);
	}
	if($hat[%client.hat] !$= "none")
	{
		%obj.unHideNode($hat[%client.hat]);
		%obj.setNodeColor($hat[%client.hat],%client.hatColor);
	}
	if(%client.hip)
	{
		%obj.unHideNode("skirthip");
		%obj.unHideNode("skirttrimleft");
		%obj.unHideNode("skirttrimright");
	}
	else
	{
		%obj.unHideNode("pants");
		%obj.unHideNode((%client.rleg ? "rpeg" : "rshoe"));
		%obj.unHideNode((%client.lleg ? "lpeg" : "lshoe"));
	}

	%obj.setHeadUp(0);
	if(%client.pack+%client.secondPack > 0) %obj.setHeadUp(1);
	if($hat[%client.hat] $= "Helmet")
	{
		if(%client.accent == 1 && $accent[4] !$= "none")
		{
			%obj.unHideNode($accent[4]);
			%obj.setNodeColor($accent[4],%client.accentColor);
		}
	}
	else if($accent[%client.accent] !$= "none" && strpos($accentsAllowed[$hat[%client.hat]],strlwr($accent[%client.accent])) != -1)
	{
		%obj.unHideNode($accent[%client.accent]);
		%obj.setNodeColor($accent[%client.accent],%client.accentColor);
	}

	if (%obj.bloody["lshoe"]) %obj.unHideNode("lshoe_blood");
	if (%obj.bloody["rshoe"]) %obj.unHideNode("rshoe_blood");
	if (%obj.bloody["lhand"]) %obj.unHideNode("lhand_blood");
	if (%obj.bloody["rhand"]) %obj.unHideNode("rhand_blood");
	if (%obj.bloody["chest_front"]) %obj.unHideNode((%client.chest ? "fem" : "") @ "chest_blood_front");
	if (%obj.bloody["chest_back"]) %obj.unHideNode((%client.chest ? "fem" : "") @ "chest_blood_back");

	%obj.setFaceName(%client.faceName);
	%obj.setDecalName(%client.decalName);

	%obj.setNodeColor("headskin",%client.headColor);	
	%obj.setNodeColor("chest",%client.chestColor);
	%obj.setNodeColor("femChest",%client.chestColor);
	%obj.setNodeColor("pants",%client.hipColor);
	%obj.setNodeColor("skirthip",%client.hipColor);	
	%obj.setNodeColor("rarm",%client.rarmColor);
	%obj.setNodeColor("larm",%client.larmColor);
	%obj.setNodeColor("rarmSlim",%client.rarmColor);
	%obj.setNodeColor("larmSlim",%client.larmColor);
	%obj.setNodeColor("rhand",%client.rhandColor);
	%obj.setNodeColor("lhand",%client.lhandColor);
	%obj.setNodeColor("rhook",%client.rhandColor);
	%obj.setNodeColor("lhook",%client.lhandColor);	
	%obj.setNodeColor("rshoe",%client.rlegColor);
	%obj.setNodeColor("lshoe",%client.llegColor);
	%obj.setNodeColor("rpeg",%client.rlegColor);
	%obj.setNodeColor("lpeg",%client.llegColor);
	%obj.setNodeColor("skirttrimright",%client.rlegColor);
	%obj.setNodeColor("skirttrimleft",%client.llegColor);

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

function Eventide_MinigameConditionalCheck(%objA,%objB,%exemptDeath)//exemptdeath is to skip checking if the victim is dead
{
	if((%objA.getClassName() $= "Player" || %objA.getClassName() $= "AIPlayer") && (%objB.getClassName() $= "Player" || %objB.getClassName() $= "AIPlayer"))
	{		
		if(%exemptDeath) 
		{				
			if(%objA.getstate() !$= "Dead" && %objA.getdataBlock().isKiller && !%objB.getdataBlock().isKiller)
			if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB) 
			return true;
		}
		else 
		if(%objA.getstate() !$= "Dead" && %objB.getstate() !$= "Dead" && %objA.getdataBlock().isKiller && !%objB.getdataBlock().isKiller)
		{
			if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB)
			return true;
		}
	}
	return false;
}

function GameConnection::Escape(%client)
{
	if(!isObject(%minigame = getMinigameFromObject(%client))) return %client.centerprint("This only works in minigames!",1);
	if(strlwr(%client.slyrTeam.name) !$= "survivors") return %client.centerprint("Only survivors can escape the map!",1);

	%client.escaped = true;
	%client.camera.setMode("Spectator",%client.player);
	%client.setcontrolobject(%client.camera);
	
	for(%i = 0; %i < %client.slyrTeam.numMembers; %i++)
	{
		if(isObject(%members = %client.slyrTeam.member[%i]) && isObject(%members.player) && %members.escaped) %escaped++;
		if(isObject(%members = %client.slyrTeam.member[%i]) && isObject(%members.player)) %living++;
	}
	

	%client.player.delete();
	%minigame.chatmsgall("<font:Impact:30>\c3" @ %client.name SPC "\c3has escaped!");
	%client.lives = 0;

	if(%escaped >= %living)
	{
		%minigame.endRound(%client.slyrTeam);
		return;
	}	
}

function Eventide_MinigameConditionalCheckNoKillers(%objA,%objB,%exemptDeath)//exemptdeath is to skip checking if the victim is dead
{
	if((%objA.getClassName() $= "Player" || %objA.getClassName() $= "AIPlayer") && (%objB.getClassName() $= "Player" || %objB.getClassName() $= "AIPlayer"))
	{		
		if(%exemptDeath) 
		{				
			if(%objA.getstate() !$= "Dead")
			if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB) 
			return true;
		}
		else 
		if(%objA.getstate() !$= "Dead" && %objB.getstate() !$= "Dead")
		{
			if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB)
			return true;
		}
	}
	return false;
}

package Eventide_MainPackage
{
	function Armor::onImpact(%this, %obj, %col, %vec, %force)
	{
		Parent::onImpact(%this, %obj, %col, %vec, %force);

		if(%force < 40) serverPlay3D("impact_medium" @ getRandom(1,3) @ "_sound",%obj.getPosition());
		else serverPlay3D("impact_hard" @ getRandom(1,3) @ "_sound",%obj.getPosition());

		%oScale = getWord(%obj.getScale(),2);
		%forcescale = %force/25 * %oscale;
		%obj.spawnExplosion(pushBroomProjectile,%forcescale SPC %forcescale SPC %forcescale);

		if(%obj.getState() !$= "Dead" && getWord(%vec,2) > %obj.getdataBlock().minImpactSpeed)
        serverPlay3D("impact_fall_sound",%obj.getPosition());		
	}

	function onObjectCollisionTest(%obj, %col)//This function is part of the ObjectCollision.dll
	{
		if(isObject(%obj) && isObject(%col))
		{
			if((%obj.getType() & $TypeMasks::PlayerObjectType) && (%col.getType() & $TypeMasks::PlayerObjectType))
			if(%obj.getdataBlock().getName() $= "PlayerGrabberNoJump") return true;
			else return false;
		}		
		
		return true;		
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
	    if(%obj.getdataBlock().staticShape $= "") return Parent::onActivate(%obj, %player, %client, %pos, %vec);
	
	    if(!isObject(%obj.interactiveshape))
	    {
	        //Check if player has proper item equipped for interacting with object
	        if(isObject(%item = %player.getMountedImage(0)) && (%item.getName() $= %obj.getDataBlock().staticShapeItemMatch || (%item.isGemRitual && %obj.getdataBlock().staticShapeItemMatch $= "gem")))
	        {            
	            if(%item.isGemRitual && %obj.getdataBlock().staticShapeItemMatch $= "gem") %obj.ShowEventideProp(%player,true,%item);
				if(!%item.isGemRitual && %obj.getdataBlock().staticShapeItemMatch !$= "gem") %obj.ShowEventideProp(%player);
	
	            %player.Tool[%player.currTool] = 0;
	            messageClient(%player.client, 'MsgItemPickup', '', %player.currTool, 0);
	            serverCmdUnUseTool(%player.client);
	
	            //Trigger an event if the eventide console exists
	            if(isObject($EventideEventCaller))
	            {
	                $InputTarget_["Self"] = $EventideEventCaller;
	                $InputTarget_["Player"] = %player;
	                $InputTarget_["Client"] = %player.client;
	                $InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
	                $EventideEventCaller.processInputEvent("onRitualPlaced", %client);
	            }
	        }
	    }        
		//If the interactive shape already exists then just check if its the candle to toggle the light
	    else if(%obj.getdataBlock().getName() $= "brickCandleData")
	    {        
	        switch(%obj.isLightOn)
	        {
	            case true: %obj.getdatablock().ToggleLight(%obj,false);
	            case false: %obj.getdatablock().ToggleLight(%obj,true);
	        }            
	    }
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