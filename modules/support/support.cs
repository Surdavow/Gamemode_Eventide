exec("./support_extraresources.cs");
exec("./support_chatsystem.cs");
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
		%obj.gazeLoop();
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
		if(isObject(%obj.emptycandlebot)) %obj.emptycandlebot.delete();
		if(isObject(%obj.lightbot.light)) %obj.lightbot.light.delete();
		if(isObject(%obj.lightbot)) %obj.lightbot.delete();
	}

    function fxDTSBrick::onActivate (%obj, %player, %client, %pos, %vec)
	{
		if(!isObject(%obj)) return;
		
		if(!isObject(%obj.interactiveshape) && isObject(%player.getMountedImage(0)))
		{
			

			if(%player.getMountedImage(0).getName() $= %obj.getDataBlock().staticShapeItemMatch)
			{
				%obj.ShowEventideProp(%player);

				if(isObject($EventideEventCaller))
				{
					$InputTarget_["Self"] = $EventideEventCaller;
					$InputTarget_["Player"] = %player;
					$InputTarget_["Client"] = %player.client;			
					$InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
					$EventideEventCaller.processInputEvent("onRitualPlaced",%client);
				}

				%player.Tool[%player.currTool] = 0;
				messageClient(%player.client,'MsgItemPickup','',%player.currTool,0);						
				serverCmdUnUseTool(%player.client);
			}

			if(strstr(strlwr(%player.getMountedImage(0).getName()),"gem") != -1)
			{
				%obj.ShowEventidePropGem(%player,%player.getMountedImage(0));
				
				if(isObject($EventideEventCaller))
				{
					$InputTarget_["Self"] = $EventideEventCaller;
					$InputTarget_["Player"] = %player;
					$InputTarget_["Client"] = %player.client;			
					$InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
					$EventideEventCaller.processInputEvent("onRitualPlaced",%client);
				}

				%player.Tool[%player.currTool] = 0;
				messageClient(%player.client,'MsgItemPickup','',%player.currTool,0);						
				serverCmdUnUseTool(%player.client);
			}			
		}
		
		if(isObject(%obj.interactiveshape))
		{
			if(%obj.getdataBlock().getName() $= "brickCandleData")
			{
	            if(!%obj.isLightOn) %obj.getdatablock().ToggleLight(%obj,true);
            	else %obj.getdatablock().ToggleLight(%obj,false);
			}
		}

		Parent::onActivate (%obj, %player, %client, %pos, %vec);
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

	function fxDtsBrick::setGazeName(%brick, %name)
	{
		%brick.gazeName = %name;
	}

	function GameConnection::startGazing(%client)
	{
		%client.cantGaze = 0;
	}

	function GameConnection::stopGazing(%client)
	{
		cancel(%client.startGazing);
		%client.cantGaze = 1;
	}

	function SimObject::getGazeName(%this, %gazer)
	{
		switch$(%this.getClassName())
		{
			case "Player": 	if(%this.client == %gazer || %gazer.player == %this || !($Pref::Server::GazeMode & 1)) return "";
							return %this.client.name;
			case "fxDtsBrick": if(!($Pref::Server::GazeMode & 2)) return "";

								//These events are really fucking abusable - admin wand immunity.
								if(isObject(%gazer.player) && !%gazer.player.hasWandImmunity(%this))
								{
									$InputTarget_Self = %this;
									$InputTarget_Player = %gazer.player;
									$InputTarget_Client = %gazer;
									if($Server::LAN || getMinigameFromObject(%this) == getMinigameFromObject(%gazer)) $InputTarget_Minigame = getMinigameFromObject(%gazer);
									else $InputTarget_Minigame = 0;
									%this.processInputEvent("onGaze", %gazer);
								}
								return %this.gazeName;

			case "AIPlayer": if(!($Pref::Server::GazeMode & 1)) return "";
							 //These events are really fucking abusable - admin wand immunity.
							if(isObject(%spawn = %this.spawnBrick) && isObject(%gazer.player) && !%gazer.player.hasWandImmunity(%spawn))
							{
								$InputTarget_Self = %spawn;
								$InputTarget_Player = %gazer.player;
								$InputTarget_Client = %gazer;
								$InputTarget_Bot = %this;
								if($Server::LAN || getMinigameFromObject(%spawn) == getMinigameFromObject(%gazer)) $InputTarget_Minigame = getMinigameFromObject(%gazer);
								else $InputTarget_Minigame = 0;
								%spawn.processInputEvent("onGaze_Bot", %gazer);
							}

			default: return "";
		}
	}

	function Player::hasWandImmunity(%pl, %brick)
	{
		if(!isObject(%tool = %pl.getMountedImage(0))) return 0;
		if(%tool.getName() $= "AdminWandImage") return 1;
		if(%tool.getName() $= "WandImage" && isObject(%brick) && isObject(%pl.client) && %brick.getGroup().getID() == %pl.client.brickgroup.getID()) return 1;
		return 0;
	}

	function GameConnection::bottomPrint(%client, %msg, %time)
	{
		if(!%client.gazing)
		{
			%client.stopGazing();
			if(%time <= 0) %client.startGazing = %client.schedule(60000, "startGazing");   //Cap time of one minute. That's long enough for any useful info.
			else %client.startGazing = %client.schedule(%time * 1000, "startGazing");
		}
		Parent::bottomPrint(%client, %msg, %time);
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