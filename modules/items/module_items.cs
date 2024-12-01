AddDamageType("PoolCue",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_poolCue> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_poolCue> %1',1,1); 
AddDamageType("BarStool",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_barStool> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_barStool> %1',1,1); 
AddDamageType("Bottle",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_bottle> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_bottle> %1',1,1); 
AddDamageType("BrokenBottle",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_bottle_broken> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_bottle_broken> %1',1,1); 
AddDamageType("Chair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_chair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_chair> %1',1,1); 
AddDamageType("FoldingChair",'<bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_foldingChair> %1','%2 <bitmap:Add-Ons/Gamemode_Eventide/modules/items/icons/ci_foldingChair> %1',1,1);

%soundfolder = "./*.wav";
%file = findFirstFile(%soundfolder);
while(%file !$= "")
{
    %soundName = strreplace(filename(strlwr(%file)), ".wav", "");

	if(strstr(%file,"normal") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClose3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"quiet") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioClosest3d; filename = \"" @ %file @ "\"; };");	
	if(strstr(%file,"loud") != -1) eval("datablock AudioProfile(" @ %soundName @ "_sound) { preload = true; description = AudioDefault3d; filename = \"" @ %file @ "\"; };");

	%file = findNextFile(%soundfolder);
}

exec("./item_airhorn.cs");
exec("./item_book.cs");
exec("./item_camera.cs");
exec("./item_candle.cs");
exec("./item_flaregun.cs");
exec("./item_gem.cs");
exec("./item_jumpgun.cs");
exec("./item_peeing.cs");
exec("./item_radio.cs");
exec("./item_radio.cs");
exec("./item_rope.cs");
exec("./item_soda.cs");
exec("./item_wrench.cs");
exec("./weapon_barstool.cs");
exec("./weapon_bottle.cs");
exec("./weapon_chainsaw.cs");
exec("./weapon_chair.cs");
exec("./weapon_dagger.cs");
exec("./weapon_foldingchair.cs");
exec("./weapon_killers.cs");
exec("./weapon_poolcue.cs");
exec("./weapon_shotgun.cs");
exec("./weapon_stungun.cs");