forceRequiredAddOn("Server_VehicleGore");
registerInputEvent("fxDTSBrick","onRitualPlaced","Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

exec("./modules/support/support_extraresources.cs");
exec("./modules/support/support_chasemusic.cs");
exec("./modules/support/support_multipleslots.cs");
exec("./modules/support/support_chatsystem.cs");
exec("./modules/sounds/module_sounds.cs");
exec("./modules/players/module_players.cs");
exec("./modules/items/module_items.cs");

//check if slayer is enabled
if ($Slayer::Server::GameModeArg $= "") $isSlayerEnabled = false;
else $isSlayerEnabled = true;

if(isfile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
        if(!$RTB::RTBR_ServerControl_Hook) exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
        if($Pref::Server::ChatMod::lchatDistance < 0) $Pref::Server::ChatMod::lchatDistance = 30;        
        $Pref::Server::ChatMod::lchatEnabled = 0; 		
        RTB_registerPref("Enable local chat",	"Eventide","$Pref::Server::ChatMod::lchatEnabled",	"bool","Gamemode_Eventide","1","0","0","");
		RTB_registerPref("Chat Distance",		"Eventide","$Pref::Server::ChatMod::lchatDistance",	"int 1 200","Gamemode_Eventide","30","0","0","");
		RTB_registerPref("Shout Distance Multiplier",	"Eventide","$Pref::Server::ChatMod::lchatShoutMultiplier",	"float 0 10","Gamemode_Eventide","2","0","0","");
		RTB_registerPref("Whisper Distance Multiplier",	"Eventide","$Pref::Server::ChatMod::lchatWhisperMultiplier",	"float 0 1","Gamemode_Eventide",".5","0","0","");
		RTB_registerPref("& Global Chat restriction",	"Eventide","$Pref::Server::ChatMod::lchatGlobalChatLevel",	"list Everyone 0 Admin 1 Super_Admin 2","Gamemode_Eventide","0","0","0","");
		RTB_registerPref("Enable local chat scaling",	"Eventide - Chat Scaling","$Pref::Server::ChatMod::lchatSizeModEnabled",	"bool","Gamemode_Eventide","0","0","0","");
		RTB_registerPref("Local chat maximum size",	"Eventide - Chat Scaling","$Pref::Server::ChatMod::lchatSizeMax",	"int 1 48","Gamemode_Eventide","24","0","0","ChatMod_lchatSize");
		RTB_registerPref("Local chat minimum size",	"Eventide - Chat Scaling","$Pref::server::ChatMod::lchatSizeMin",	"int 1 48","Gamemode_Eventide","12","0","0","ChatMod_lchatSize");
        RTB_registerPref("Number of channels",	"Eventide - Radio","$Pref::Server::ChatMod::radioNumChannels","int 1 9","Gamemode_Eventide","1","0","0","ChatMod_resetRadioChannels");
}
else
{
	$Pref::Server::ChatMod::lchatEnabled = 1;
	$Pref::Server::ChatMod::lchatDistance = 30;
	$Pref::Server::ChatMod::lchatShoutMultiplier = 2;
	$Pref::Server::ChatMod::lchatWhisperMultiplier = 0.5;
	$Pref::Server::ChatMod::lchatGlobalChatLevel = 1;
	$Pref::Server::ChatMod::lchatSizeModEnabled = 0;
	$Pref::Server::ChatMod::lchatSizeMax = 24;
	$Pref::server::ChatMod::lchatSizeMin = 12;
	$Pref::Server::ChatMod::radioNumChannels = 1;
}

function Armor::EventideAppearance(%db,%pl,%cl)
{
	%pl.hideNode("ALL");
	%pl.unHideNode((%cl.chest ? "femChest" : "chest"));	
	%pl.unHideNode((%cl.rhand ? "rhook" : "rhand"));
	%pl.unHideNode((%cl.lhand ? "lhook" : "lhand"));
	%pl.unHideNode((%cl.rarm ? "rarmSlim" : "rarm"));
	%pl.unHideNode((%cl.larm ? "larmSlim" : "larm"));
	%pl.unHideNode("headskin");

	if($pack[%cl.pack] !$= "none")
	{
		%pl.unHideNode($pack[%cl.pack]);
		%pl.setNodeColor($pack[%cl.pack],%cl.packColor);
	}
	if($secondPack[%cl.secondPack] !$= "none")
	{
		%pl.unHideNode($secondPack[%cl.secondPack]);
		%pl.setNodeColor($secondPack[%cl.secondPack],%cl.secondPackColor);
	}
	if($hat[%cl.hat] !$= "none")
	{
		%pl.unHideNode($hat[%cl.hat]);
		%pl.setNodeColor($hat[%cl.hat],%cl.hatColor);
	}
	if(%cl.hip)
	{
		%pl.unHideNode("skirthip");
		%pl.unHideNode("skirttrimleft");
		%pl.unHideNode("skirttrimright");
	}
	else
	{
		%pl.unHideNode("pants");
		%pl.unHideNode((%cl.rleg ? "rpeg" : "rshoe"));
		%pl.unHideNode((%cl.lleg ? "lpeg" : "lshoe"));
	}

	%pl.setHeadUp(0);
	if(%cl.pack+%cl.secondPack > 0) %pl.setHeadUp(1);
	if($hat[%cl.hat] $= "Helmet")
	{
		if(%cl.accent == 1 && $accent[4] !$= "none")
		{
			%pl.unHideNode($accent[4]);
			%pl.setNodeColor($accent[4],%cl.accentColor);
		}
	}
	else if($accent[%cl.accent] !$= "none" && strpos($accentsAllowed[$hat[%cl.hat]],strlwr($accent[%cl.accent])) != -1)
	{
		%pl.unHideNode($accent[%cl.accent]);
		%pl.setNodeColor($accent[%cl.accent],%cl.accentColor);
	}

	if (%pl.bloody["lshoe"]) %pl.unHideNode("lshoe_blood");
	if (%pl.bloody["rshoe"]) %pl.unHideNode("rshoe_blood");
	if (%pl.bloody["lhand"]) %pl.unHideNode("lhand_blood");
	if (%pl.bloody["rhand"]) %pl.unHideNode("rhand_blood");
	if (%pl.bloody["chest_front"]) %pl.unHideNode((%cl.chest ? "fem" : "") @ "chest_blood_front");
	if (%pl.bloody["chest_back"]) %pl.unHideNode((%cl.chest ? "fem" : "") @ "chest_blood_back");

	%pl.setFaceName(%cl.faceName);
	%pl.setDecalName(%cl.decalName);

	%pl.setNodeColor("headskin",%cl.headColor);	
	%pl.setNodeColor("chest",%cl.chestColor);
	%pl.setNodeColor("femChest",%cl.chestColor);
	%pl.setNodeColor("pants",%cl.hipColor);
	%pl.setNodeColor("skirthip",%cl.hipColor);	
	%pl.setNodeColor("rarm",%cl.rarmColor);
	%pl.setNodeColor("larm",%cl.larmColor);
	%pl.setNodeColor("rarmSlim",%cl.rarmColor);
	%pl.setNodeColor("larmSlim",%cl.larmColor);
	%pl.setNodeColor("rhand",%cl.rhandColor);
	%pl.setNodeColor("lhand",%cl.lhandColor);
	%pl.setNodeColor("rhook",%cl.rhandColor);
	%pl.setNodeColor("lhook",%cl.lhandColor);	
	%pl.setNodeColor("rshoe",%cl.rlegColor);
	%pl.setNodeColor("lshoe",%cl.llegColor);
	%pl.setNodeColor("rpeg",%cl.rlegColor);
	%pl.setNodeColor("lpeg",%cl.llegColor);
	%pl.setNodeColor("skirttrimright",%cl.rlegColor);
	%pl.setNodeColor("skirttrimleft",%cl.llegColor);

	//Set blood colors.
	%pl.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%pl.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%pl.setNodeColor("lhand_blood", "0.7 0 0 1");
	%pl.setNodeColor("rhand_blood", "0.7 0 0 1");
	%pl.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%pl.setNodeColor("chest_blood_back", "0.7 0 0 1");
	%pl.setNodeColor("femchest_blood_front", "0.7 0 0 1");
	%pl.setNodeColor("femchest_blood_back", "0.7 0 0 1");
}

package Eventide_MainPackage
{
	function Armor::onRemove(%this,%obj)
	{
		if(isObject(%obj.emptycandlebot)) %obj.emptycandlebot.delete();
		Parent::onRemove(%this,%obj);
	}

    function fxDTSBrick::onActivate (%obj, %player, %client, %pos, %vec)
	{
		if(!isObject(%obj.interactiveshape) && isObject(%player.getMountedImage(0)) && %player.getMountedImage(0).getName() $= %obj.getDataBlock().staticShapeItemMatch)
		{
			%obj.ShowEventideProp();

			$InputTarget_["Self"] = %obj;
			$InputTarget_["Player"] = %player;
			$InputTarget_["Client"] = %player.client;			
			$InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
			%obj.processInputEvent("onRitualPlaced",%client);			
			%player.Tool[%player.currTool] = 0;
			messageClient(%player.client,'MsgItemPickup','',%player.currTool,0);						
			serverCmdUnUseTool(%player.client);
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

    function MiniGameSO::Reset(%minigame,%client)
	{
        Parent::Reset(%minigame,%client);

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter))
        {
            %client.EventidemusicEmitter.delete();
            %client.musicChaseLevel = 0;
        }

		if(isObject(EventideShapeGroup)) EventideShapeGroup.delete();		
    }

    function MinigameSO::endGame(%minigame,%client)
    {
        Parent::endGame(%minigame,%client);

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter)) %client.EventidemusicEmitter.delete();            
    }

	function Armor::onNewDatablock(%this,%obj)
	{		
		Parent::onNewDatablock(%this,%obj);
		%obj.schedule(10,KillerScanCheck);
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
};
activatePackage(Eventide_MainPackage);