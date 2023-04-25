ForceRequiredAddOn("Server_VehicleGore");

$ItemInstance::Save = "";

registerInputEvent("fxDtsBrick", "onGaze", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
registerInputEvent("fxDtsBrick", "onGazeStart", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
registerInputEvent("fxDtsBrick", "onGazeStop", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
registerInputEvent("fxDtsBrick", "onGaze_Bot", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame\tBot Player");
registerInputEvent("fxDtsBrick", "onGazeStart_Bot", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame\tBot Player");
registerInputEvent("fxDtsBrick", "onGazeStop_Bot", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame\tBot Player");
registerOutputEvent("fxDtsBrick", "setGazeName", "string 200 255", 0);
registerInputEvent("fxDTSBrick","onRitualPlaced","Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick","onAllRitualsPlaced","Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerOutputEvent("GameConnection", "Escape", "", false);

exec("./modules/support/support.cs");
exec("./modules/sounds/module_sounds.cs");
exec("./modules/items/module_items.cs");
exec("./modules/players/module_players.cs");

//check if slayer is enabled
if ($Slayer::Server::GameModeArg $= "") $isSlayerEnabled = false;
else $isSlayerEnabled = true;

if(isfile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
        if(!$RTB::RTBR_ServerControl_Hook) exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
        if($Pref::Server::ChatMod::lchatDistance < 0) $Pref::Server::ChatMod::lchatDistance = 30;        
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
		RTB_registerPref("Sight Range", "Eventide - Gaze", "$Pref::Server::GazeRange", "int 0 100", "Gamemode_Eventide", 20, 0, 0);//, "update function");
		RTB_registerPref("Allow onGaze", "Eventide - Gaze", "$Pref::Server::onGazeEnabled", "bool", "Gamemode_Eventide", 1, 0, 0);//, "update function");
		RTB_registerPref("Gaze Mode", "Eventide - Gaze", "$Pref::Server::GazeMode", "list Bricks/Players/Bots 3 Bricks 2 Players/Bots 1 Sightless 0", "Gamemode_Eventide", 3, 0, 0);//, "update function");		
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
	$Pref::Server::onGazeEnabled = 1;
	$Pref::Server::GazeMode = 3;	
}