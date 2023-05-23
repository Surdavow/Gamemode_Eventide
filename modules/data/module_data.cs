datablock AudioDescription(AudioFSRun)
{
	volume = 0.75;
	isLooping = false;
	is3D = 1;
	ReferenceDistance = 5;
	maxDistance = 25;
	type = $SimAudioType;
};

datablock AudioDescription(AudioFSWalk)
{
	volume = 0.5;
	isLooping = false;
	is3D = 1;
	ReferenceDistance = 2.5;
	maxDistance = 25;
	type = $SimAudioType;
};

%pattern = "Add-Ons/Gamemode_Eventide/modules/data/sounds/*.wav";
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

exec("./datablocks_misc.cs");
exec("./player_eventide.cs");
exec("./player_renowned.cs");

%path = "./*.cs";
for(%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
    if(strstr(strlwr(%file),"datablocks_misc") != -1 || strstr(strlwr(%file),"player_eventide") != -1 || strstr(strlwr(%file),"player_renowned") != -1 || strstr(strlwr(%file),"module_data") != -1) continue;
	exec(%file);
}