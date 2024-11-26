function serverCmdResetMinigame(%client)
{
	if(!%client.isAdmin && !isObject(%client.minigame)) return;
	%client.minigame.reset();	
}

function serverCmdResetMG(%client)
{
	serverCmdResetMinigame(%client);
}