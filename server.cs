if(ForceRequiredAddOn("Brick_Halloween") == $Error::AddOn_NotFound) return error("Brick_Halloween is required for Gamemode_Eventide to work");
if(ForceRequiredAddOn("Gamemode_Slayer") == $Error::AddOn_NotFound) return error("Gamemode_Slayer is required for Gamemode_Eventide to work");
$isSlayerEnabled = $Slayer::Server::GameModeArg !$= "" ? true : false;

// Execute essential scripts and preferences first
exec("./prefs.cs");
exec("./modules/scripts/module_scripts.cs");

exec("./modules/misc/module_misc.cs");
exec("./modules/misc/module_bricks.cs");
exec("./modules/items/module_items.cs");
exec("./modules/players/module_players.cs");