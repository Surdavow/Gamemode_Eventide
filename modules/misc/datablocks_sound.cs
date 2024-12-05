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

	// Automatic sound instancing based on name of the sound
	%description = strstr(%file, "normal") != -1 ? "AudioClose3d" :
    	           strstr(%file, "quiet") != -1  ? "AudioClosest3d" :
        	       strstr(%file, "loud") != -1   ? "AudioDefault3d" : "";

	// Continue if no description
	if (%description $= "") continue;

    eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = " @ %description @ "; filename = \"" @ %file @ "\"; };");


	%file = findNextFile(%pattern);
}