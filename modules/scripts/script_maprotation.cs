package Eventide_MapRotation
{
	function MiniGameSO::Reset(%minigame,%client)
	{
		if (ClientGroup.getCount() && $Pref::Server::MapRotation::Enabled)
  		{
			if ($Pref::Server::MapRotation::ResetCount >= $Pref::Server::MapRotation::minReset) 
			{
				$Pref::Server::MapRotation::ResetCount = 0;
				%minigame.playSound("item_get_sound");
				%roundstring = ($Pref::Server::MapRotation::minReset == 1) ? "The round has" : $Pref::Server::MapRotation::minReset SPC "rounds have";
				%minigame.chatMsgAll("<font:Impact:30>\c3" @ %roundstring SPC "passed, loading the next map!");
				Eventide_loadNextMap();
				echo("Map Changer: Loading next map...");
				return Parent::Reset(%minigame, %client);
			}

			%minigame.chatMsgAll("<font:Impact:30>\c3Round" SPC $Pref::Server::MapRotation::ResetCount++ SPC "of" SPC $Pref::Server::MapRotation::minReset);
		}

		Parent::Reset(%minigame, %client);
	}

	function gameConnection::spawnPlayer(%client)
	{		
		if ($Eventide_MapChanging) 
		{
			return;
		}

		Parent::spawnPlayer(%client);
	}

	function ServerLoadSaveFile_End()
	{
		Parent::ServerLoadSaveFile_End();
		scheduleNoQuota(3000,0,Eventide_endMapChange);
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
		%file.writeLine("Optionally, you can also drop the .ez files here to load the environment zones.");
		%file.writeLine("The .ez and .bls file must have the same name so both can be loaded properly.");
	}
	
	%file.close();
	%file.delete();
}

function Eventide_endMapChange()
{
	echo("Map Changer: Respawning all players...");
	$Eventide_MapChanging = false;
	
	// Respawn all players
	for (%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		if (isObject(%client = ClientGroup.getObject(%a)) && isObject(getMiniGameFromObject(%client)))
		{
			%minigame = getMiniGameFromObject(%client);
			%client.spawnPlayer();
		}
	}

	if(isObject(%minigame))
	{
		%minigame.chatMsgAll("<font:Impact:30>\c3Round" SPC $Pref::Server::MapRotation::ResetCount++ SPC "of" SPC $Pref::Server::MapRotation::minReset);
	}
}

function Eventide_loadNextMap()
{	
	deleteAllEnvZones();
	
	// Move to the next map, or wrap around to the start if we reach the end
	if($Eventide_CurrentMap >= $Eventide_MapsAmount) 
	{
		$Eventide_CurrentMap = 1;
	}
	else
	{
		$Eventide_CurrentMap++;
	}
	
	%fileName = strlwr($Eventide_Maps[$Eventide_CurrentMap]);

	// Prevent players from respawning
	$Eventide_MapChanging = true;

	// Force all players into spectator mode
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		if(isObject(%client = ClientGroup.getObject(%a)))
		{
			// Set the camera to observer
			%client.centerPrint();
			%client.camera.setFlyMode();
			%client.camera.mode = "Observer";
			%client.setControlObject(%client.camera);			
			
			// Delete the player
			if(isObject(%client.player)) 
			{
				%inventoryToolCount = (%client.player.hoarderToolCount) ? %client.player.hoarderToolCount : %client.player.getDataBlock().maxTools;
				for (%i = 0; %i < %inventoryToolCount; %i++) if (isObject(%client.player.tool[%i]))
				{
					%client.player.tool[%i] = 0;
					messageClient(%client, 'MsgItemPickup', '', %i, 0);
				}

				%client.player.delete();
			}
		}
	}

	// Clear all of the public bricks
	BrickGroup_888888.chaindeleteall();

	// Load the save file
	scheduleNoQuota(33, 0, serverDirectSaveFileLoad, %fileName, 3, "", 2, 0);

	// Load the environment zones, if there is one
	%zonefilename = strreplace(fileName(%fileName), ".bls", "");
	%zonefilepath = "saves/EventideMapRotation/" @ fileName(%zonefilename) @ ".ez";
	if(isFile(%zonefilepath))
	{
		loadEnvironmentZones(%zonefilepath);
	}
	
}

function Eventide_loadMapList()
{
	%mapdir = "saves/EventideMapRotation/*.bls";	
	$Eventide_MapsAmount = 0;
	
	%file = findFirstFile(%mapdir);
	while(%file !$= "")
	{
		$Eventide_Maps[$Eventide_MapsAmount++] = %file;
		%file = findNextFile(%mapdir);		
	}

	echo("Map Changer:" SPC $Eventide_MapsAmount SPC "maps loaded.");
}

Eventide_loadMapList();