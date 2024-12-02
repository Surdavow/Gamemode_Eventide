if(ForceRequiredAddOn("Support_CustomCDN") == $Error::AddOn_NotFound) return error("Support_CustomCDN is required for Gamemode_Eventide to work");
$CustomCDN::CDN_to_clients = "http://borrowedtime.online/blobs";

if(ForceRequiredAddOn("Gamemode_Slayer") == $Error::AddOn_NotFound) return error("Gamemode_Slayer is required for Gamemode_Eventide to work");
if(ForceRequiredAddOn("Event_BrickText") == $Error::AddOn_NotFound) return error("Event_BrickText is required for Gamemode_Eventide to work");
if(ForceRequiredAddOn("Item_Medical") == $Error::AddOn_NotFound) return error("Item_Medical is required for Gamemode_Eventide to work");
if(ForceRequiredAddOn("Brick_Halloween") == $Error::AddOn_NotFound) return error("Brick_Halloween is required for Gamemode_Eventide to work");

// Execute essential scripts and preferences first
exec("./prefs.cs");
exec("./modules/scripts/module_scripts.cs");

exec("./modules/misc/module_misc.cs");
exec("./modules/bricks/module_bricks.cs");
exec("./modules/items/module_items.cs");
exec("./modules/players/module_players.cs");