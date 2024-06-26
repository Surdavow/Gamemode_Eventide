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

		if($Pref::Server::MapRotation::current != 0) 
		messageAll('MsgUploadEnd', %msg);

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

	//clear all bricks 
	// note: this function is deferred, so we'll have to set a callback to be triggered when it's done
	BrickGroup_888888.chaindeletecallback = "LoadLevel(\"" @ %filename @ "\");";
	BrickGroup_888888.chaindeleteall();	
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

function serverCmdmapList(%client) 
{
	if(!$Pref::Server::MapRotation::enabled) return;
	
	messageClient(%client, '', "\c3BI0Hazzard's \c6Map \c3Rotation - Map List");
	
	for(%a = 0; %a < $Pref::Server::MapRotation::numMap; %a++)
	{
		%mapname = $Pref::Server::MapRotation::map[%a];
		%mapname = strReplace(%mapname, "saves/EventideMapRotation/", "");
		%mapname = strReplace(%mapname, ".bls", "");
		messageClient(%client, '', " \c3" @ (%a+1) SPC "\c6-" SPC %mapname);
	}
}

function serverCmdmapHelp(%client) 
{
	// Return immediately if the map rotation is not enabled
	if(!$Pref::Server::MapRotation::enabled) return;
	
	// Display the help messages
	messageClient(%client, '', "\c3BI0Hazzard's \c6Map \c3Rotation. (Eventide Supported)");
	messageClient(%client, '', " \c6- \c3/mapList \c6- Shows a list of maps that the rotator uses.");
	
	if(%client.isAdmin) messageClient(%client, '', " \c6- \c3/reloadMaps \c6- Reloads the servers collection of maps.");	
}

function servercmdReloadMaps(%client)
{
	// Return if the client is not an admin
	if(!%client.isAdmin) return;
	
	messageAll('', "\c3" @ %client.name SPC "\c6reloaded the server maps.");
	setModPaths(getModPaths());
	BuildMapLists();
}

function LoadLevel(%filename)
{	
	schedule(10, 0, serverDirectSaveFileLoad, %fileName, 3, "", 2, 1);
}

BuildMapLists();