exec("./support_extraresources.cs");
exec("./script_chatsystem.cs");
exec("./script_gazeloop.cs");
exec("./script_killers.cs");
exec("./support_dataInstance.cs");
exec("./support_itemammo.cs");
exec("./support_statuseffect.cs");
exec("./support_stringutilities.cs");

package Eventide_MainPackage
{
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
			%minigame.chatmsgall("<font:impact:30>\c3Eventide: The Hunt begins");
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

				if(%client.player.RadioOn) ChatMod_RadioMessage(%client, %message, true);
				if(isObject(%client.minigame)) ChatMod_TeamLocalChat(%client, %message);
				else if(!%client.player.RadioOn) messageClient(%client,'',"\c5You must be in a minigame to team chat.");
			}
			else messageClient(%client,'',"\c5You are dead. You must respawn to use team chat.");
			%client.lastMessageSent = %client;
		}			
	}

	function ServerCmdMessageSent(%client, %message)
	{		
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

	function ServerCmdStartTalking(%client)
	{
		if($Pref::Server::ChatMod::lchatEnabled) return;
		else parent::ServerCmdStartTalking(%client);
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
		if(%obj.getClassName() $= "AIPlayer" && %obj.getDataBlock().getName() $= "ShireZombieBot") return %obj.ghostclient.brickgroup;
		Parent::getBrickGroupFromObject(%obj);
	}

	function Player::ActivateStuff (%player)//Not parenting, I made an overhaul of this function so it might cause compatibility issues...
	{
		Parent::ActivateStuff(%player);
		if(isObject(%player) && %player.getState() !$= "Dead") %player.getDataBlock().onActivate(%player);
	}

	function Armor::onActivate(%this,%obj)
	{
		//Do something!
	}
	
};

if(isPackage(Eventide_MainPackage)) deactivatePackage(Eventide_MainPackage);
activatePackage(Eventide_MainPackage);