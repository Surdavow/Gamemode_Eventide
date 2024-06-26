package Eventide_GameConnection
{

	function gameConnection::spawnPlayer(%client)
	{
		Parent::spawnPlayer(%client);

		if ($Pref::Server::MapRotation::MapChange) return;	
		%client.hasVoted = false;
	}

	function gameConnection::autoAdminCheck(%client) 
	{
		schedule(100,0,Eventide_loadEventideStats,%client);
		Parent::autoAdminCheck(%client);
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
		
		// Call the EventideAppearance function if the player is an Eventide player
		if(isObject(%player = %client.player) && %player.getDataBlock().isEventideModel) 
		%player.getDataBlock().EventideAppearance(%player,%client);
	}
	function gameConnection::applyBodyParts(%client) 
	{
		parent::applyBodyParts(%client);

		// Call the EventideAppearance function if the player is an Eventide player
		if(isObject(%player = %client.player) && fileName(%player.getDataBlock().shapeFile) $= "Eventideplayer.dts") 
		%player.getDataBlock().EventideAppearance(%player,%client);
	}

	function serverCmdUseTool(%client, %tool)
	{		
		if(!%client.player.victim) return parent::serverCmdUseTool(%client, %tool);
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
		echo(%client.name @ ": " @ getSubStr(%message, 0, strlen(%message)));
	}


	function serverCmdLight(%client)
	{
		if(isObject(%client.player) && %client.player.getdataBlock().isKiller) return;
		Parent::serverCmdLight(%client);		
	}		
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_GameConnection)) deactivatePackage(Eventide_GameConnection);
activatePackage(Eventide_GameConnection);