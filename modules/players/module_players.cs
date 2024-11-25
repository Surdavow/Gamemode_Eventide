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
	
	%decalpath = "./faces/*.png";
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
	
	%decalpath = "./decals/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		addExtraResource(%decalfile);
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource(findFirstFile(%decalfilepath));
}

// Execute these first, the other playertypes inherit from them.
exec("./player_eventide.cs");
exec("./player_renowned.cs");

exec("./player_angler.cs");
exec("./player_cannibal.cs");
exec("./player_disfigured.cs");
exec("./player_doll.cs");
exec("./player_empty.cs");
exec("./player_genocide.cs");
exec("./player_grabber.cs");
exec("./player_huntress.cs");
exec("./player_knight.cs");
exec("./player_lurker.cs");
exec("./player_lurkerinvis.cs");
exec("./player_nightmare.cs");
exec("./player_nightmareTeleport.cs");
exec("./player_puppetmaster.cs");
exec("./player_puppetmasterpuppet.cs");
exec("./player_render.cs");
exec("./bot_shirezombie.cs");
exec("./player_shire.cs");
exec("./player_skinwalker.cs");
exec("./player_skullwolf.cs");