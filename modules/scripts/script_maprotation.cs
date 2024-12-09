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

function Eventide_loadNextMap()
{
	// Move to the next map, or wrap around to the start if we reach the end
	$Pref::Server::MapRotation::current = ($Pref::Server::MapRotation::current + 1) % $Pref::Server::MapRotation::numMap;
	%filename = $Pref::Server::MapRotation::map[$Pref::Server::MapRotation::current];

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

	// Clear all of the public bricks
	BrickGroup_888888.chaindeleteall();

   // Taken from Gamemode_Speedkart, with some modifications
   // load environment if it exists
	%envFile = filePath(%fileName) @ "/environment.txt"; 
	if(isFile(%envFile))
	{  
		//usage: GameModeGuiServer::ParseGameModeFile(%filename, %append);
		//if %append == 0, all minigame variables will be cleared 
		%res = GameModeGuiServer::ParseGameModeFile(%envFile, 1);
	
		EnvGuiServer::getIdxFromFilenames();
		EnvGuiServer::SetSimpleMode();
	
		if(!$EnvGuiServer::SimpleMode)     
		{
			EnvGuiServer::fillAdvancedVarsFromSimple();
			EnvGuiServer::SetAdvancedMode();
		}
	}

	scheduleNoQuota(33, 0, serverDirectSaveFileLoad, %fileName, 3, "", 2, 0);
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