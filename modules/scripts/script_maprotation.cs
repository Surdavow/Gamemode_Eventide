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