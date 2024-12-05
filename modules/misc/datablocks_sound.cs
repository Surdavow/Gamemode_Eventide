datablock AudioDescription(AudioFSRun)
{
	volume = 0.85;
	isLooping = false;
	is3D = 1;
	ReferenceDistance = 10;
	maxDistance = 40;
	type = $SimAudioType;
};

datablock AudioDescription(AudioFSWalk)
{
	volume = 0.65;
	isLooping = false;
	is3D = 1;
	ReferenceDistance = 5;
	maxDistance = 15;
	type = $SimAudioType;
};

%pattern = "./*.wav";
%file = findFirstFile(%pattern);
while(%file !$= "")
{
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

	if(strstr(%file,"normal") != -1) 
    eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClose3d; filename = \"" @ %file @ "\"; length =" SPC alxGetWaveLen(%file) @ "; };");
	if(strstr(%file,"quiet") != -1) 
    eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClosest3d; filename = \"" @ %file @ "\"; length =" SPC alxGetWaveLen(%file) @ "; };");	
	if(strstr(%file,"loud") != -1) 
    eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioDefault3d; filename = \"" @ %file @ "\"; length =" SPC alxGetWaveLen(%file) @ "; };");

	%file = findNextFile(%pattern);
}