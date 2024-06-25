%path = "./*.cs";
for(%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
	// Skip these files
	if(strstr(strlwr(%file),"module_scripts") != -1) continue;

	exec(%file);
}