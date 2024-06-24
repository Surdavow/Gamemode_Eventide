if(ForceRequiredAddOn("Brick_Halloween") == $Error::AddOn_NotFound) return error("Brick_Halloween is required for Gamemode_Eventide to work");

//check if slayer is enabled
if ($Slayer::Server::GameModeArg $= "") $isSlayerEnabled = false;
else $isSlayerEnabled = true;

//if(ForceRequiredAddOn("Server_VehicleGore") == $Error::AddOn_NotFound) return error("Server_VehicleGore is required for Gamemode_Eventide to work");
//if(ForceRequiredAddOn("System_Instruments") == $Error::AddOn_NotFound) return error("System_Instruments is required for Gamemode_Eventide to work");

exec("./prefs.cs");
exec("./modules/scripts/module_scripts.cs");
exec("./modules/misc/module_misc.cs");
