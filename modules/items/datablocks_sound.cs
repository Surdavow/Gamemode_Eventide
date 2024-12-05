%soundfolder = "./*.wav";
%file = findFirstFile(%soundfolder);
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

	%file = findNextFile(%soundfolder);
}