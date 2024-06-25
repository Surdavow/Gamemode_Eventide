function serverCmdClearEventideShapes(%client)
{
	if(!%client.isAdmin || !isObject(EventideShapeGroup) || !EventideShapeGroup.getCount()) return;
	
	MessageAll('MsgClearBricks', "\c3" @ %client.getPlayerName () @ "\c1\c0 cleared all Eventide shapes.");
	EventideShapeGroup.delete();
}

function serverCmdGetRitualCount(%client)
{
	if(!%client.isAdmin || !isObject(EventideShapeGroup) || !EventideRitualSet.getCount()) return;
	%client.chatmessage("\c2" @ EventideRitualSet.getCount() SPC "\c2ritual placements exist");
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