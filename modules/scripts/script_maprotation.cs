if (!isFile("saves/EventideMapRotation/README.txt")) 
{	
	%file = new FileObject();

	if (%file.openForWrite("saves/EventideMapRotation/README.txt"))
	%file.writeLine("You need to place save files in this folder for the Map Rotation to be able to work!");
	else error("File is not open for writing");
	
	%file.close();
	%file.delete();
}

function nextMap(%msg) 
{
	$Pref::Server::MapRotation::ResetCount = 0;
	%nextnum = $Pref::Server::MapRotation::current+1;
	%current = $Pref::Server::MapRotation::current;

	if($Pref::Server::MapRotation::map[%nextnum] !$= "") 
	{
		%filename = $Pref::Server::MapRotation::map[%nextnum];
		$Pref::Server::MapRotation::current++;
		messageAll('MsgUploadEnd', %msg);
	} 
	else 
	{
		if($Pref::Server::MapRotation::current) messageAll('MsgUploadEnd', %msg);

		%filename = $Pref::Server::MapRotation::map0;
		$Pref::Server::MapRotation::current = 0;
	}

	//suspend minigame resets
	$Pref::Server::MapRotation::MapChange = true;

	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		%client = ClientGroup.getObject(%a);
		%player = %client.player;

		if(isObject(%player)) %player.delete();

		%camera = %client.camera;
		%camera.setFlyMode();
		%camera.mode = "Observer";
		%client.setControlObject(%camera);
	}

	//clear all of the public bricks
	BrickGroup_230349.chaindeletecallback = "LoadLevel(\"" @ %filename @ "\");";
	BrickGroup_230349.chaindeleteall();	
}

function BuildMapLists()
{
	%mapdir = "saves/EventideMapRotation/*.bls";	
	$Pref::Server::MapRotation::numMap = 0;
	
	%file = findFirstFile(%mapdir);

	while(%file !$= "")
	{
		$Pref::Server::MapRotation::map[$Pref::Server::MapRotation::numMap] = %file;
		$Pref::Server::MapRotation::numMap++;
		%file = findNextFile(%mapdir);
	}

	messageAll('', "\c3" @ ($Pref::Server::MapRotation::numMap+1) SPC "\c6maps loaded.");
}

function LoadLevel(%filename)
{	
	schedule(10, 0, serverDirectSaveFileLoad, %fileName, 3, "", 2, 1);
}

BuildMapLists();

package Eventide_MapRotation
{
	function MiniGameSO::Reset(%minigame,%client)
	{
		if (ClientGroup.getCount() && $Pref::Server::MapRotation::enabled)
  		{
			if ($Pref::Server::MapRotation::ResetCount >= $Pref::Server::MapRotation::minreset) 
			{
				%minigame.playSound("item_get_sound");			
				nextMap("\c3" SPC $Pref::Server::MapRotation::minreset SPC "rounds have passed, loading the next map!");
   				$Pref::Server::MapRotation::ResetCount = 0;
			}

			$Pref::Server::MapRotation::ResetCount++;
			%minigame.chatMsgAll("\c3Round" SPC $Pref::Server::MapRotation::ResetCount SPC "of" SPC $Pref::Server::MapRotation::minreset);
		}

		Parent::Reset(%minigame, %client);
	}

	function gameConnection::spawnPlayer(%client)
	{
		Parent::spawnPlayer(%client);

		if ($Pref::Server::MapRotation::MapChange) return;	
		%client.hasVoted = false;
	}
	
	function onMissionEnded(%this, %a, %b, %c, %d)
	{
		$PFGlassInit = false;
		$PFRTBInit = false;
		return Parent::onMissionEnded(%this, %a, %b, %c, %d);
	}

	function getBrickGroupFromObject(%obj)
	{
		if(isObject(%obj) && %obj.getClassName() $= "AIPlayer")		
		switch$(%obj.getDataBlock().getName())			
		{
			case "ShireZombieBot": return %obj.ghostclient.brickgroup;
			case "PuppetMasterPuppet": return %obj.client.brickgroup;
		}		

		Parent::getBrickGroupFromObject(%obj);
	}

	function ServerLoadSaveFile_End()
	{
		$Pref::Server::MapRotation::MapChange = false;

		for(%a = 0; %a < ClientGroup.getCount(); %a++)
		{
			%client = ClientGroup.getObject(%a);
			%client.spawnPlayer();
		}

		Parent::ServerLoadSaveFile_End();
	}	
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_MapRotation)) deactivatePackage(Eventide_MapRotation);
activatePackage(Eventide_MapRotation);