datablock AudioDescription(AudioFSRun)
{
	volume = 0.75;
	isLooping = 0;
	is3D = 1;
	ReferenceDistance = 3;
	maxDistance = 60;
	type = $SimAudioType;
};
datablock AudioDescription(AudioFSWalk)
{
	volume = 0.5
	isLooping = 0;
	is3D = 1;
	ReferenceDistance = 1;
	maxDistance = 30;
	type = $SimAudioType;
};

%pattern = "Add-Ons/Gamemode_Eventide/modules/sounds/*.wav";
%file = findFirstFile(%pattern);
while(%file !$= "")
{
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

	if(strstr(%file,"normal") != -1)
	{
		if(strstr(%file,"walk") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioFSWalk; filename = \"" @ %file @ "\"; };");
		else if(strstr(%file,"run") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioFSRun; filename = \"" @ %file @ "\"; };");
		else if(strstr(%file,"walk") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClose3d; filename = \"" @ %file @ "\"; };");
	} 
	if(strstr(%file,"quiet") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClosest3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"loud") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioDefault3d; filename = \"" @ %file @ "\"; };");

	%file = findNextFile(%pattern);
}