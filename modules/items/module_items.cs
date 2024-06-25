// sounds

%pattern = "./*.wav";
%file = findFirstFile(%pattern);
while(%file !$= "")
{
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

	if(strstr(%file,"normal") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClose3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"quiet") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClosest3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"loud") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioDefault3d; filename = \"" @ %file @ "\"; };");

	%file = findNextFile(%pattern);
}

AddDamageType("PoolCue",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_poolCue> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_poolCue> %1',1,1);
AddDamageType("BarStool",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_barStool> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_barStool> %1',1,1);
AddDamageType("Bottle",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_bottle> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_bottle> %1',1,1);
AddDamageType("BrokenBottle",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_bottle_broken> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_bottle_broken> %1',1,1);
AddDamageType("Chair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_chair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_chair> %1',1,1);
AddDamageType("FoldingChair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_foldingChair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_foldingChair> %1',1,1);

%path = "./*.cs";
for (%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path) )
{
    if (strstr(strlwr(%file),"module_items") != -1) continue;
	exec(%file);
}