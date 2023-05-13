exec("./datablocks_misc.cs");

%path = "./*.cs";
for(%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
	if(strstr(strlwr(%file),"module_weapons") != -1 || strstr(strlwr(%file),"datablocks_misc") != -1) continue;
	exec(%file);
}
