$CustomCDN::CDN_to_clients = "http://borrowedtime.online/blobs";

// Preference support for Blockland Glass and Return to Blockland
if($RTB::Hooks::ServerControl)
{
	RTB_registerPref("Enable local chat (minigame only)",	"Eventide","$Pref::Server::ChatMod::lchatEnabled",	"bool","Gamemode_Eventide","1","0","0","");
	RTB_registerPref("Enable chase music",	"Eventide","$Pref::Server::Eventide::chaseMusicEnabled",	"bool","Gamemode_Eventide","1","0","0","");
	RTB_registerPref("Enable killer sounds",	"Eventide","$Pref::Server::Eventide::killerSoundsEnabled",	"bool","Gamemode_Eventide","1","0","0","");
	RTB_registerPref("Enabled","Eventide - Map Rotation","$Pref::Server::MapRotation::enabled","bool","Gamemode_Eventide",false,false,false);
	RTB_registerPref("Rounds per map","Eventide - Map Rotation","$Pref::Server::MapRotation::minreset","int 1 10","Gamemode_Eventide",5,false,true);	
	RTB_registerPref("Allow Gaze", "Eventide - Gaze", "$Pref::Server::GazeEnabled", "bool", "Gamemode_Eventide", 1, 0, 0);
	RTB_registerPref("Sight Range", "Eventide - Gaze", "$Pref::Server::GazeRange", "int 0 100", "Gamemode_Eventide", 20, 0, 0);
	RTB_RegisterPref("Enable Footstep SoundFX", "Eventide - Footsteps", "$Pref::Server::PF::footstepsEnabled", "bool", "Gamemode_Eventide", 1, 0, 0);
	RTB_RegisterPref("BrickFX custom SoundFX", "Eventide - Footsteps", "$Pref::Server::PF::brickFXSounds::enabled", "bool", "Gamemode_Eventide", 1, 0, 0);
	RTB_RegisterPref("Enable Landing SoundFX", "Eventide - Footsteps", "$Pref::Server::PF::landingFX", "bool", "Gamemode_Eventide", 1, 0, 0);
	RTB_RegisterPref("Enable Swimming SoundFX", "Eventide - Footsteps", "$Pref::Server::PF::waterSFX", "bool", "Gamemode_Eventide", 1, 0, 0);
	RTB_RegisterPref("Landing Threshold", "Eventide - Footsteps", "$Pref::Server::PF::minLandSpeed", "int 0.0 20.0", "Gamemode_Eventide", 10.0, 0, 0);
	RTB_RegisterPref("Running Threshold", "Eventide - Footsteps", "$Pref::Server::PF::runningMinSpeed", "int 0.0 20.0", "Gamemode_Eventide", 2.8, 0, 0);
	RTB_RegisterPref("Default Step SoundFX", "Eventide - Footsteps", "$Pref::Server::PF::defaultStep", "List	Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Gamemode_Eventide", 1, 0, 0);
	RTB_RegisterPref("Steps on Terrain SoundFX", "Eventide - Footsteps", "$Pref::Server::PF::terrainStep", "List	Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Gamemode_Eventide", 0, 0, 0);
	RTB_RegisterPref("Steps on Vehicles SoundFX", "Eventide - Footsteps", "$Pref::Server::PF::vehicleStep", "List	Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Gamemode_Eventide", 0, 0, 0);		
}
else
{	
	if ($Pref::Server::ChatMod::lchatEnabled $= "") $Pref::Server::ChatMod::lchatEnabled = 1;
	if ($Pref::Server::Eventide::chaseMusicEnabled $= "") $Pref::Server::Eventide::chaseMusicEnabled = 1;
	if ($Pref::Server::Eventide::killerSoundsEnabled $= "") $Pref::Server::Eventide::killerSoundsEnabled = 1;
	if ($Pref::Server::GazeRange $= "") $Pref::Server::GazeRange = 20;
	if ($Pref::Server::GazeEnabled $= "") $Pref::Server::GazeEnabled = 1;
	if ($Pref::Server::PF::footstepsEnabled $= "") $Pref::Server::PF::footstepsEnabled = 1;
	if ($Pref::Server::PF::brickFXSounds::enabled $= "") $Pref::Server::PF::brickFXSounds::enabled = 1;
	if ($Pref::Server::PF::brickFXSounds::enabled $= "") $Pref::Server::PF::landingFX = 1;
	if ($Pref::Server::PF::minLandSpeed $= "") $Pref::Server::PF::minLandSpeed = 8.0;
	if ($Pref::Server::PF::runningMinSpeed $= "") $Pref::Server::PF::runningMinSpeed = 2.8;
	if ($Pref::Server::PF::waterSFX $= "") $Pref::Server::PF::waterSFX = 1;
	if ($Pref::Server::PF::defaultStep $= "") $Pref::Server::PF::defaultStep = 1;
	if ($Pref::Server::PF::terrainStep $= "") $Pref::Server::PF::terrainStep = 0;
	if ($Pref::Server::PF::vehicleStep $= "") $Pref::Server::PF::vehicleStep = 0;	
	if ($Pref::Server::MapRotation::enabled $= "") $Pref::Server::MapRotation::enabled = true;
	if ($Pref::Server::MapRotation::minreset $= "") $Pref::Server::MapRotation::minreset = 5;
}

if ($Pref::Server::PF::brickFXsounds::pearlStep $= "") $Pref::Server::PF::pearlStep = 4;
if ($Pref::Server::PF::brickFXsounds::chromeStep $= "") $Pref::Server::PF::chromeStep = 4;
if ($Pref::Server::PF::brickFXsounds::glowStep $= "") $Pref::Server::PF::brickFXsounds::glowStep = 0;
if ($Pref::Server::PF::brickFXsounds::blinkStep $= "") $Pref::Server::PF::brickFXsounds::blinkStep = 0;
if ($Pref::Server::PF::brickFXsounds::swirlStep $= "") $Pref::Server::PF::brickFXsounds::swirlStep = 0;
if ($Pref::Server::PF::brickFXsounds::rainbowStep $= "") $Pref::Server::PF::brickFXsounds::rainbowStep = 0;
if ($Pref::Server::PF::brickFXsounds::unduloStep $= "") $Pref::Server::PF::unduloStep = 8;

// Always set this to 0, don't want there to be problems when the server is initialized
$Pref::Server::MapRotation::ResetCount = 0;
$Pref::Server::MapRotation::cooldown = 10;
$iconspath = "Add-ons/Gamemode_Eventide/modules/players/icons/";
