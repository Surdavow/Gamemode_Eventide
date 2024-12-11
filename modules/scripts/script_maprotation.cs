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

function serverCmdLoadEnvZones(%client, %f0, %f1, %f2, %f3, %f4, %f5, %f6, %f7)
{
	if(!%client.isAdmin){messageClient(%client, '', $EZ::AdminFailMsg); return;}

	if(!$ShowEnvironmentZones)
		showEnvironmentZones(1);

	%fileName = trim(%f0 SPC %f1 SPC %f2 SPC %f3 SPC %f4 SPC %f5 SPC %f6 SPC %f7);
	if(!strLen(%fileName))
	{
		messageClient(%client, '', "\c0You have to specify a name for the save file to load!");
		return;
	}

	%allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ._-()";
	%filePath = "config/server/EnvironmentZoneSaves/" @ %fileName @ ".ez";
	%filePath = strReplace(%filePath, ".ez.ez", ".ez");

	for(%i = 0; %i < strLen(%fileName); %i++)
	{
		if(strStr(%allowed, getSubStr(%fileName, %i, 1)) == -1)
		{
			%forbidden = true;
			break;
		}
	}

	if(%forbidden || !strLen(%fileName) || strLen(%fileName) > 50)
	{
		messageClient(%client, '', "\c0The file name contains invalid characters, try again!");
		return;
	}

	if(!isFile(%filePath))
	{
		messageClient(%client, '', "\c0There is no save file with that name!");
		return;
	}

	loadEnvironmentZones(%filePath);
	messageClient(%client, '', "\c6All zones have been loaded.");
}

function Eventide_loadNextMap()
{	
	deleteAllEnvZones();
	
	// Move to the next map, or wrap around to the start if we reach the end
	$Pref::Server::MapRotation::current = ($Pref::Server::MapRotation::current + 1) % $Pref::Server::MapRotation::numMap;
	%fileName = strlwr($Pref::Server::MapRotation::map[$Pref::Server::MapRotation::current]);	

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

	// Load the new map and the environment zones
	%zonefilename = strreplace(fileName(%fileName), ".bls", "");
	scheduleNoQuota(33, 0, serverDirectSaveFileLoad, %fileName, 3, "", 2, 0);
	loadEnvironmentZones("config/server/EnvironmentZoneSaves/" @ fileName(%zonefilename) @ ".ez");
}

function Eventide_loadMapList()
{
	%mapdir = "saves/EventideMapRotation/*.bls";	
	$Pref::Server::MapRotation::numMap = 0;
	
	%file = findFirstFile(%mapdir);
	while(%file !$= "")
	{
		$Pref::Server::MapRotation::map[$Pref::Server::MapRotation::numMap++] = %file;
		%file = findNextFile(%mapdir);		
	}

	echo("Map Changer:" SPC $Pref::Server::MapRotation::numMap SPC "maps loaded.");
}

Eventide_loadMapList();