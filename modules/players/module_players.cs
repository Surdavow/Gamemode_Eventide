%pattern = "./*.wav";
%file = findFirstFile(%pattern);
while(%file !$= "")
{
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

	if(strstr(%file,"normal") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClose3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"quiet") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClosest3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"loud") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioDefault3d; filename = \"" @ %file @ "\"; };");

	//footsteps
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
{
	addExtraResource(%iconfile);	
}

// Faces
if (isFile(%faceiflpath = "./models/face.ifl"))
{
	%write = new FileObject();
	%write.openForWrite(findFirstFile(%faceiflpath));
	%write.writeLine("base/data/shapes/player/faces/smiley.png");
	%write.writeLine("Add-Ons/Face_Default/smileyRedBeard2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyRedBeard.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate3.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyOld.png");
	%write.writeLine("Add-Ons/Face_Default/smileyFemale1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyEvil2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyEvil1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyCreepy.png");
	%write.writeLine("Add-Ons/Face_Default/smileyBlonde.png");
	%write.writeLine("Add-Ons/Face_Default/memeYaranika.png");
	%write.writeLine("Add-Ons/Face_Default/memePBear.png");
	%write.writeLine("Add-Ons/Face_Default/memeHappy.png");
	%write.writeLine("Add-Ons/Face_Default/memeGrinMan.png");
	%write.writeLine("Add-Ons/Face_Default/memeDesu.png");
	%write.writeLine("Add-Ons/Face_Default/memeCats.png");
	%write.writeLine("Add-Ons/Face_Default/memeBlockMongler.png");
	%write.writeLine("Add-Ons/Face_Default/asciiTerror.png");
	
	%decalpath = "./models/faces/*.png";
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
	%write.writeLine("Add-Ons/Decal_WORM/worm_engineer.png");
	%write.writeLine("Add-Ons/Decal_WORM/worm-sweater.png");
	%write.writeLine("Add-Ons/Decal_PlayerFitNE/zhwindnike.png");
	%write.writeLine("Add-Ons/Decal_Jirue/LinkTunic.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Knight.png");
	%write.writeLine("Add-Ons/Decal_Jirue/HCZombie.png");
	%write.writeLine("Add-Ons/Decal_Jirue/DrKleiner.png");
	%write.writeLine("Add-Ons/Decal_Jirue/DKnight.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Chef.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Archer.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Alyx.png");
	%write.writeLine("Add-Ons/Decal_Hoodie/Hoodie.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-Old.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-New.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-Nasa.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Suit.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Prisoner.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Police.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Pilot.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-DareDevil.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Army.png");
	%write.writeLine("Add-Ons/Decal_Default/Meme-Mongler.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-YARLY.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Tunic.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Rider.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-ORLY.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Lion.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Eagle.png");
	
	%decalpath = "./models/decals/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		eval("addExtraResource(\""@ %decalfile @ "\");");
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource(findFirstFile(%decalfilepath));
}

exec("./player_eventide.cs");
exec("./player_renowned.cs");

// Scripts
%path = "./*.cs";
for (%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
    if (strstr(strlwr(%file),"module_players") != -1 || strstr(strlwr(%file),"player_eventide") != -1 || strstr(strlwr(%file),"player_renowned") != -1 || strstr(strlwr(%file),"module_misc") != -1) continue;
	exec(%file);
}