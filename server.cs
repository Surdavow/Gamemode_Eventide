if(ForceRequiredAddOn("Server_VehicleGore") == $Error::AddOn_NotFound) return error("Server_VehicleGore must be enabled for Gamemode_Eventide to work");

exec("./modules/scripts/module_scripts.cs");
exec("./modules/sounds/module_sounds.cs");
exec("./modules/items/module_items.cs");
exec("./modules/players/module_players.cs");

//check if slayer is enabled
if ($Slayer::Server::GameModeArg $= "") $isSlayerEnabled = false;
else $isSlayerEnabled = true;

if(isfile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
        if(!$RTB::RTBR_ServerControl_Hook) exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");

        $Pref::Server::ChatMod::lchatEnabled = 0;
        RTB_registerPref("Enable local chat",	"Eventide","$Pref::Server::ChatMod::lchatEnabled",	"bool","Gamemode_Eventide","1","0","0","");
		RTB_registerPref("Chat Distance",		"Eventide","$Pref::Server::ChatMod::lchatDistance",	"int 1 200","Gamemode_Eventide","30","0","0","");
		RTB_registerPref("Shout Distance Multiplier",	"Eventide","$Pref::Server::ChatMod::lchatShoutMultiplier",	"float 0 10","Gamemode_Eventide","2","0","0","");
		RTB_registerPref("Whisper Distance Multiplier",	"Eventide","$Pref::Server::ChatMod::lchatWhisperMultiplier",	"float 0 1","Gamemode_Eventide",".5","0","0","");
		RTB_registerPref("& Global Chat restriction",	"Eventide","$Pref::Server::ChatMod::lchatGlobalChatLevel",	"list Everyone 0 Admin 1 Super_Admin 2","Gamemode_Eventide","0","0","0","");
		RTB_registerPref("Enable local chat scaling",	"Eventide - Chat Scaling","$Pref::Server::ChatMod::lchatSizeModEnabled",	"bool","Gamemode_Eventide","0","0","0","");
		RTB_registerPref("Local chat maximum size",	"Eventide - Chat Scaling","$Pref::Server::ChatMod::lchatSizeMax",	"int 1 48","Gamemode_Eventide","24","0","0","ChatMod_lchatSize");
		RTB_registerPref("Local chat minimum size",	"Eventide - Chat Scaling","$Pref::server::ChatMod::lchatSizeMin",	"int 1 48","Gamemode_Eventide","12","0","0","ChatMod_lchatSize");
        RTB_registerPref("Number of channels",	"Eventide - Radio","$Pref::Server::ChatMod::radioNumChannels","int 1 9","Gamemode_Eventide","1","0","0","ChatMod_resetRadioChannels");

		RTB_registerPref("Allow Gaze", "Eventide - Gaze", "$Pref::Server::GazeEnabled", "bool", "Gamemode_Eventide", 1, 0, 0);
		RTB_registerPref("Sight Range", "Eventide - Gaze", "$Pref::Server::GazeRange", "int 0 100", "Gamemode_Eventide", 20, 0, 0);

		RTB_RegisterPref("Enable Footstep SoundFX", "Eventide", "$Pref::Server::PF::footstepsEnabled", "bool", "Script_PeggFootsteps", 1, 0, 0);
		RTB_RegisterPref("BrickFX custom SoundFX", "Eventide", "$Pref::Server::PF::brickFXSounds::enabled", "bool", "Script_PeggFootsteps", 1, 0, 0);
		RTB_RegisterPref("Enable Landing SoundFX", "Eventide", "$Pref::Server::PF::landingFX", "bool", "Script_PeggFootsteps", 1, 0, 0);
	  	RTB_RegisterPref("Enable Swimming SoundFX", "Eventide", "$Pref::Server::PF::waterSFX", "bool", "Script_PeggFootsteps", 1, 0, 0);
		RTB_RegisterPref("Landing Threshold", "Eventide", "$Pref::Server::PF::minLandSpeed", "int 0.0 20.0", "Script_PeggFootsteps", 10.0, 0, 0);
		RTB_RegisterPref("Running Threshold", "Eventide", "$Pref::Server::PF::runningMinSpeed", "int 0.0 20.0", "Script_PeggFootsteps", 2.8, 0, 0);
		RTB_RegisterPref("Default Step SoundFX", "Eventide", "$Pref::Server::PF::defaultStep", "List	Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Script_PeggFootsteps", 1, 0, 0);
		RTB_RegisterPref("Steps on Terrain SoundFX", "Eventide", "$Pref::Server::PF::terrainStep", "List	Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Script_PeggFootsteps", 0, 0, 0);
		RTB_RegisterPref("Steps on Vehicles SoundFX", "Eventide", "$Pref::Server::PF::vehicleStep", "List	Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Script_PeggFootsteps", 0, 0, 0);

		if ( $Pref::Server::PF::brickFXsounds::pearlStep $= "" ) $Pref::Server::PF::pearlStep = 4;
		if ( $Pref::Server::PF::brickFXsounds::chromeStep $= "" ) $Pref::Server::PF::chromeStep = 4;
		if ( $Pref::Server::PF::brickFXsounds::glowStep $= "" ) $Pref::Server::PF::brickFXsounds::glowStep = 0;
		if ( $Pref::Server::PF::brickFXsounds::blinkStep $= "" ) $Pref::Server::PF::brickFXsounds::blinkStep = 0;
		if ( $Pref::Server::PF::brickFXsounds::swirlStep $= "" ) $Pref::Server::PF::brickFXsounds::swirlStep = 0;
		if ( $Pref::Server::PF::brickFXsounds::rainbowStep $= "" ) $Pref::Server::PF::brickFXsounds::rainbowStep = 0;
		if ( $Pref::Server::PF::brickFXsounds::unduloStep $= "" ) $Pref::Server::PF::unduloStep = 8;				
}
else
{
	$Pref::Server::ChatMod::lchatEnabled = 1;
	$Pref::Server::ChatMod::lchatDistance = 30;
	$Pref::Server::ChatMod::lchatShoutMultiplier = 2;
	$Pref::Server::ChatMod::lchatWhisperMultiplier = 0.5;
	$Pref::Server::ChatMod::lchatGlobalChatLevel = 1;
	$Pref::Server::ChatMod::lchatSizeModEnabled = 0;
	$Pref::Server::ChatMod::lchatSizeMax = 24;
	$Pref::server::ChatMod::lchatSizeMin = 12;
	$Pref::Server::ChatMod::radioNumChannels = 1;
	$Pref::Server::GazeRange = 20;
	$Pref::Server::GazeEnabled = 1;

	if ( $Pref::Server::PF::footstepsEnabled $= "" ) $Pref::Server::PF::footstepsEnabled = 1;
	if ( $Pref::Server::PF::brickFXSounds::enabled $= "" ) $Pref::Server::PF::brickFXSounds::enabled = 1;
	if ( $Pref::Server::PF::brickFXSounds::enabled $= "" ) $Pref::Server::PF::landingFX = 1;
	if ( $Pref::Server::PF::minLandSpeed $= "" ) $Pref::Server::PF::minLandSpeed = 8.0;
	if ( $Pref::Server::PF::runningMinSpeed $= "" ) $Pref::Server::PF::runningMinSpeed = 2.8;
	if ( $Pref::Server::PF::waterSFX $= "" ) $Pref::Server::PF::waterSFX = 1;
	if ( $Pref::Server::PF::defaultStep $= "" ) $Pref::Server::PF::defaultStep = 1;
	if ( $Pref::Server::PF::terrainStep $= "" ) $Pref::Server::PF::terrainStep = 0;
	if ( $Pref::Server::PF::vehicleStep $= "" ) $Pref::Server::PF::vehicleStep = 0;

	if ( $Pref::Server::PF::brickFXsounds::pearlStep $= "" ) $Pref::Server::PF::pearlStep = 4;
	if ( $Pref::Server::PF::brickFXsounds::chromeStep $= "" ) $Pref::Server::PF::chromeStep = 4;
	if ( $Pref::Server::PF::brickFXsounds::glowStep $= "" ) $Pref::Server::PF::brickFXsounds::glowStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::blinkStep $= "" ) $Pref::Server::PF::brickFXsounds::blinkStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::swirlStep $= "" ) $Pref::Server::PF::brickFXsounds::swirlStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::rainbowStep $= "" ) $Pref::Server::PF::brickFXsounds::rainbowStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::unduloStep $= "" ) $Pref::Server::PF::unduloStep = 8;	
}