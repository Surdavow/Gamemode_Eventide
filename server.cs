if(ForceRequiredAddOn("Brick_Halloween") == $Error::AddOn_NotFound) return error("Brick_Halloween is required for Gamemode_Eventide to work");

//check if slayer is enabled
if ($Slayer::Server::GameModeArg $= "") $isSlayerEnabled = false;
else $isSlayerEnabled = true;

exec("./prefs.cs");
exec("./modules/scripts/module_scripts.cs");
exec("./modules/misc/module_misc.cs");
