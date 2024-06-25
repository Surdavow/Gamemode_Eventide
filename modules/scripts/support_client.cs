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
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_GameConnection)) deactivatePackage(Eventide_GameConnection);
activatePackage(Eventide_GameConnection);