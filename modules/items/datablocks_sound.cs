%soundfolder = "./*.wav";
%file = findFirstFile(%soundfolder);

while (%file !$= "") {
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

    // Automatic sound instancing based on name of the sound
    if (strstr(%file, "normal") != -1) {
        %description = "AudioClose3d";
    } else if (strstr(%file, "quiet") != -1) {
        %description = "AudioClosest3d";
    } else if (strstr(%file, "loud") != -1) {
        %description = "AudioDefault3d";
    } else {
        %description = ""; // Default to empty if no match
    }

    // Continue if no description is set
    if (%description $= "") {
        %file = findNextFile(%soundfolder);
        continue;
    }

    // Dynamically create the AudioProfile datablock
    eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = " @ %description @ "; filename = \"" @ %file @ "\"; };");

    %file = findNextFile(%soundfolder);
}