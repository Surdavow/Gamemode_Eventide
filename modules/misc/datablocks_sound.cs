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
while (%file !$= "") 
{
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

    // Automatic sound instancing based on name of the sound
    if (strstr(%file, "normal") != -1) 
	{
        %description = "AudioClose3d";
    } 
	else if (strstr(%file, "quiet") != -1) 
	{
        %description = "AudioClosest3d";
    } 
	else if (strstr(%file, "loud") != -1) 
	{
        %description = "AudioDefault3d";
    }
    else if (strstr(%file, "normal_loop") != -1) 
	{
        %description = "AudioCloseLooping3d";
    }
    else if (strstr(%file, "quiet_loop") != -1) 
	{
        %description = "AudioClosestLooping3d";
    } 
    else if (strstr(%file, "loud_loop") != -1) 
	{
        %description = "AudioDefaultLooping3d";
    }
    else if (strstr(%file, "music") != -1) 
	{
        %soundName = fileBase(%file);
        %description = "AudioMusicLooping3d";
    }
	else 
	{
        %description = ""; // No match, do not create datablock
    }

    // Special handling for footsteps
    if (strstr(%file, "sounds/footsteps/") != -1) 
	{
        if (strstr(%file, "walk") != -1 || strstr(%file, "swim") != -1) 
		{
            %description = "AudioFSWalk";
        } 
		else if (strstr(%file, "run") != -1) 
		{
            %description = "AudioFSRun";
        } 
		else 
		{
            %description = ""; // No match for footsteps, skip
        }
    }

    // Skip if no valid description
    if (%description $= "") 
	{
        %file = findNextFile(%pattern);
        continue;
    }

    // Create AudioProfile datablock
    if(%description $= "AudioMusicLooping3d")
    {
        eval("datablock AudioProfile(musicData_" @ %soundName @ ") { preload = true; description = " @ %description @ "; filename = \"" @ %file @ "\"; uiName=\"" @ %soundName @ "\"; };");
    }
    else
    {
        eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = " @ %description @ "; filename = \"" @ %file @ "\"; };");
    }
    %file = findNextFile(%pattern);
}