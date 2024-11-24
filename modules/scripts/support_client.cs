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
		if(isObject(%player = %client.player) && %player.getDataBlock().isEventideModel) 
		%player.getDataBlock().EventideAppearance(%player,%client);
	}

	function serverCmdUseTool(%client, %tool)
	{		
		if(!%client.player.victim) return parent::serverCmdUseTool(%client, %tool);
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
		%color = (%client.customtitlecolor $= "") ? "FFFFFF" : %client.customtitlecolor;
        %bitmap = (%client.customtitlebitmap $= "") ? "" : %client.customtitlebitmap;

		if (%client.customtitle !$= "") 
		%client.clanPrefix = %bitmap @ "<color:" @ %color @ ">" @ %client.customtitle SPC "";
		
		else %client.clanPrefix = (%client.customtitlebitmap !$= "") ? (%bitmap @ "") : "";

		if(!$Pref::Server::ChatMod::lchatEnabled)
		{
			Parent::ServerCmdMessageSent(%client, %message);
			return;
		}

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);
		if(%message !$= "")
		{
			if(isObject(%client.player))
			{
				%client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);
				%client.player.playThread(3,talk);
			}

			ChatMod_LocalChat(%client, %message);
		}

		%client.lastMessageSent = %message;
		echo(%client.name @ ": " @ getSubStr(%message, 0, strlen(%message)));		
	}

	function serverCmdLight(%client)
	{
		if(isObject(%client.player) && %client.player.getdataBlock().isKiller) return;
		Parent::serverCmdLight(%client);		
	}

	function ServerCmdTeamMessageSent(%client, %message)
	{
		if(!$Pref::Server::ChatMod::lchatEnabled)
		{
			Parent::ServerCmdTeamMessageSent(%client, %message);
			return;
		}

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);
		if(%message $= "0") return;
		
		if(isObject(%client.player))
		{
			%client.player.playThread(3,talk);
			%client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);

			if(%client.player.radioEquipped) 
			{
				ChatMod_RadioMessage(%client, %message);
				ChatMod_LocalChat(%client, %message);
			}

			else messageClient(%client,'',"\c5You need to use a radio to use team chat.");		
		}
		else messageClient(%client,'',"\c5You are dead. You must respawn to use team chat.");
		%client.lastMessageSent = %client;			
	}		
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_GameConnection)) deactivatePackage(Eventide_GameConnection);
activatePackage(Eventide_GameConnection);