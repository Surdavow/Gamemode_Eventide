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