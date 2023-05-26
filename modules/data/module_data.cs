exec("./datablocks_misc.cs");
exec("./player_eventide.cs");
exec("./player_renowned.cs");

%path = "./*.cs";
for(%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
    if(strstr(strlwr(%file),"datablocks_misc") != -1 || strstr(strlwr(%file),"player_eventide") != -1 || strstr(strlwr(%file),"player_renowned") != -1 || strstr(strlwr(%file),"module_data") != -1) continue;
	exec(%file);
}