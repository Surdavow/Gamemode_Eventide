%path = "./*.cs";
for(%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
	if(strstr(strlwr(%file),"module_weapons") != -1) continue;
	exec(%file);
}
