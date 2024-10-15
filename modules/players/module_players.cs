// Load sounds
%pattern = "./*.wav";
%file = findFirstFile(%pattern);
while(%file !$= "")
{
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

	// Automatic sound instancing based on name of the sound, self explanatory names.
	if(strstr(%file,"normal") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClose3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"quiet") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClosest3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"loud") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioDefault3d; filename = \"" @ %file @ "\"; };");

	//Unique condition for footsteps
	if(strstr(%file,"sounds/footsteps/") != -1)
	{
		if(strstr(%file,"walk") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioFSWalk; filename = \"" @ %file @ "\"; };");
		else if(strstr(%file,"swim") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioFSWalk; filename = \"" @ %file @ "\"; };");
		else if(strstr(%file,"run") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioFSRun; filename = \"" @ %file @ "\"; };");		
	}

	%file = findNextFile(%pattern);
}

// Icons
%iconpath = "./icons/*.png"; 
for (%iconfile = findFirstFile(%iconpath); %iconfile !$= ""; %iconfile = findNextFile(%iconpath))
addExtraResource(%iconfile);

// Faces
if (isFile(%faceiflpath = "./models/face.ifl"))
{
	%write = new FileObject();
	%write.openForWrite(findFirstFile(%faceiflpath));
	%write.writeLine("base/data/shapes/player/faces/smiley.png");

	for(%faceFile = findFirstFile("Add-Ons/Face_Default/*.png"); %faceFile !$= ""; %faceFile = findNextFile("Add-Ons/Face_Default/*.png"))
	{
		%write.writeLine(%faceFile);
	}
	
	%decalpath = "./models/faces/*.png";
	for (%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		addExtraResource(%decalfile);
		%write.writeLine(%decalfile);
	}

	//Face system functionality.
	%decalpath = "./faces/*.png";
	for (%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		addExtraResource(%decalfile);
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource(findFirstFile(%faceiflpath));
}

// Decals
if (isFile(%decalfilepath = "./models/decal.ifl"))
{
	%write = new FileObject();
	%write.openForWrite(findFirstFile(%decalfilepath));
	%write.writeLine("base/data/shapes/players/decals/AAA-none.png");
	%write.writeLine("Add-Ons/Decal_PlayerFitNE/zhwindnike.png");
	%write.writeLine("Add-Ons/Decal_Hoodie/Hoodie.png");

	for(%decalFile = findFirstFile("Add-Ons/Decal_WORM/*.png"); %decalFile !$= ""; %decalFile = findNextFile("Add-Ons/Decal_WORM/*.png"))
	{
		%write.writeLine(%decalFile);
	}

	for(%decalFile = findFirstFile("Add-Ons/Decal_Jirue/*.png"); %decalFile !$= ""; %decalFile = findNextFile("Add-Ons/Decal_Jirue/*.png"))
	{
		%write.writeLine(%decalFile);
	}
	
	for(%decalFile = findFirstFile("Add-Ons/Decal_Default/*.png"); %decalFile !$= ""; %decalFile = findNextFile("Add-Ons/Decal_Default/*.png"))
	{
		%write.writeLine(%decalFile);
	}
	
	%decalpath = "./models/decals/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		addExtraResource(%decalfile);
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource(findFirstFile(%decalfilepath));
}

exec("./player_eventide.cs");
exec("./player_renowned.cs");

exec("./player_facesystem.cs");
parseFacePacks("Add-Ons/Gamemode_Eventide/modules/players/faces");

// Execute scripts
%path = "./*.cs";
for (%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
    if (strstr(strlwr(%file),"module_players") != -1 || strstr(strlwr(%file),"player_eventide") != -1 || strstr(strlwr(%file),"player_renowned") != -1 || strstr(strlwr(%file),"module_misc") != -1) continue;
	exec(%file);
}
