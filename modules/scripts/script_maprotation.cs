package Eventide_MapRotation
{
	function MiniGameSO::Reset(%minigame,%client)
	{
		if (ClientGroup.getCount() && $Pref::Server::MapRotation::enabled)
  		{
			if ($Pref::Server::MapRotation::ResetCount >= $Pref::Server::MapRotation::minReset) 
			{
				$Pref::Server::MapRotation::ResetCount = 0;
				%minigame.playSound("item_get_sound");
				%roundstring = ($Pref::Server::MapRotation::minReset == 1) ? "The round has" : $Pref::Server::MapRotation::minReset SPC "rounds have";
				%minigame.chatMsgAll("<font:Impact:30>\c3" @ %roundstring SPC "passed, loading the next map!");
				Eventide_loadNextMap();		
			}

			$Pref::Server::MapRotation::ResetCount++;
			%minigame.chatMsgAll("<font:Impact:30>\c3Round" SPC $Pref::Server::MapRotation::ResetCount SPC "of" SPC $Pref::Server::MapRotation::minReset);
		}

		Parent::Reset(%minigame, %client);
	}

	function gameConnection::spawnPlayer(%client)
	{
		Parent::spawnPlayer(%client);

		if ($Eventide_MapChanging) 
		{
			return;
		}
	}

	function ServerLoadSaveFile_End()
	{
		Parent::ServerLoadSaveFile_End();
		
		$Eventide_MapChanging = false;
		echo("Respawning all players...");
		
		// Respawn all players
		for (%a = 0; %a < ClientGroup.getCount(); %a++)
		{
			if (isObject(%client = ClientGroup.getObject(%a)) && isObject(getMiniGameFromObject(%client)))
			{
				%client.scheduleNoQuota(2000,spawnPlayer);
			}
		}		
	}	
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_MapRotation)) deactivatePackage(Eventide_MapRotation);
activatePackage(Eventide_MapRotation);

if (!isFile("saves/EventideMapRotation/README.txt"))
{	
	%file = new FileObject();

	if (%file.openForWrite("saves/EventideMapRotation/README.txt"))
	{
		%file.writeLine("You need to place save files in this folder for the Map Rotation to be able to work!");
	}	
	else 
	{
		error("File is not open for writing");
	}
	
	%file.close();
	%file.delete();
}

function Eventide_loadNextMap()
{
	$Pref::Server::MapRotation::ResetCount = 0;
	%nextnum = $Pref::Server::MapRotation::current+1;
	%current = $Pref::Server::MapRotation::current;	

	if($Pref::Server::MapRotation::map[%nextnum] !$= "") 
	{
		%filename = $Pref::Server::MapRotation::map[%nextnum];
		$Pref::Server::MapRotation::current++;
		
	} 
	else 
	{
		%filename = $Pref::Server::MapRotation::map0;
		$Pref::Server::MapRotation::current = 0;
	}

	// Prevent players from respawning
	$Eventide_MapChanging = true;

	// Force all players into spectator mode
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		if(isObject(%client = ClientGroup.getObject(%a)))
		{
			// Set the camera to observer
			%client.camera.setFlyMode();
			%client.camera.mode = "Observer";
			%client.setControlObject(%client.camera);			
			
			// Delete the player
			if(isObject(%client.player)) 
			{
				%client.player.delete();
			}
		}
	}

	//clear all of the public bricks
	BrickGroup_888888.chaindeleteall();	
	scheduleNoQuota(33, 0, serverDirectSaveFileLoad, %fileName, 3, "", 2, 0);
}

function BuildMapLists()
{
	%mapdir = "saves/EventideMapRotation/*.bls";	
	$Pref::Server::MapRotation::numMap = 0;
	
	%file = findFirstFile(%mapdir);
	while(%file !$= "")
	{
		$Pref::Server::MapRotation::map[$Pref::Server::MapRotation::numMap++] = %file;
		%file = findNextFile(%mapdir);		
	}

	echo($Pref::Server::MapRotation::numMap SPC "maps loaded.");
}

BuildMapLists();