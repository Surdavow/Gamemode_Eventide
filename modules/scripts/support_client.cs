package Eventide_GameConnection
{
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
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_GameConnection)) deactivatePackage(Eventide_GameConnection);
activatePackage(Eventide_GameConnection);