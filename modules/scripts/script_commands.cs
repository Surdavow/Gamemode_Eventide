function serverCmdClearEventideShapes(%client)
{
	if(!%client.isAdmin || !isObject(EventideShapeGroup) || !EventideShapeGroup.getCount()) return;
	
	MessageAll('MsgClearBricks', "\c3" @ %client.getPlayerName () @ "\c1\c0 cleared all Eventide shapes.");
	EventideShapeGroup.delete();
}

function serverCmdResetMinigame(%client)
{
	if(!%client.isAdmin && !isObject(%client.minigame)) return;
	%client.minigame.reset();	
}

function serverCmdResetMG(%client)
{
	serverCmdResetMinigame(%client);
}

function serverCmdnextMap(%client) 
{
	if(!$Pref::Server::MapRotation::enabled || !%client.isAdmin) 
	return;

	%msg = "\c3 A new map has been called, loading now...";
	nextMap(%msg);
}

function serverCmdEventideDebug(%client) 
{
    if (!%client.isAdmin) return;

	if(isObject(EventideShapeGroup) && EventideRitualSet.getCount())
    %client.chatmessage("\c2" @ EventideRitualSet.getCount() SPC "\c2ritual placements exist");

	%client.chatmessage("\c2" @ $EventideRitualAmount SPC "\c2max ritual items can be placed");
    
    if (!$Pref::Server::MapRotation::enabled) return;
    
    messageClient(%client, '', "\c3BI0Hazzard's Map Rotation - Map List");
    
    for (%a = 0; %a < $Pref::Server::MapRotation::numMap; %a++) {
        %mapname = $Pref::Server::MapRotation::map[%a];
        %mapname = strReplace(%mapname, "saves/EventideMapRotation/", "");
        %mapname = strReplace(%mapname, ".bls", "");
        messageClient(%client, '', " \c3" @ (%a+1) SPC "\c6-" SPC %mapname);
    }
}

function serverCmdmapHelp(%client) 
{
	// Return immediately if the map rotation is not enabled
	if(!$Pref::Server::MapRotation::enabled) return;
	
	// Display the help messages
	messageClient(%client, '', "\c3BI0Hazzard's \c6Map \c3Rotation. (Eventide Supported)");
	messageClient(%client, '', " \c6- \c3/mapList \c6- Shows a list of maps that the rotator uses.");
	
	if(%client.isAdmin) messageClient(%client, '', " \c6- \c3/reloadMaps \c6- Reloads the servers collection of maps.");	
}

function servercmdReloadMaps(%client)
{
	// Return if the client is not an admin
	if(!%client.isAdmin) return;
	
	messageAll('', "\c3" @ %client.name SPC "\c6reloaded the server maps.");
	setModPaths(getModPaths());
	BuildMapLists();
}