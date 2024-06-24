%path = "./*.cs";
for(%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
	// Skip these files
	if(strstr(strlwr(%file),"module_scripts") != -1) continue;
	if(strstr(strlwr(%file),"script_saveloadstats") != -1) continue;

	exec(%file);
}

// saveloadstats should be last to execute
exec("./script_saveloadstats.cs");