package Eventide_Minigame
{
	function Slayer_MiniGameSO::endRound(%minigame, %winner, %resetTime)
	{
		Parent::endRound(%minigame, %winner, %resetTime);

		// Disable local chat at the end of the round, let everyone banter at the end.
		$Pref::Server::ChatMod::lchatEnabled = 0;
		%minigame.bottomprintall("<font:impact:20>\c3Local chat disabled",4);
		
		// Loop through all minigame members to play the round end sound
		for (%i = 0; %i < %minigame.numMembers; %i++) 
		{
    		%member = %minigame.member[%i];
    		if (isObject(%member) && %member.getClassName() $= "GameConnection")
        	%member.play2d("round_end_sound");
		}
	}

    function MiniGameSO::Reset(%minigame,%client)
	{
		if (ClientGroup.getCount() && $Pref::Server::MapRotation::enabled)
  		{
			if ($Pref::Server::MapRotation::ResetCount >= $Pref::Server::MapRotation::minreset) 
			{
				for(%i=0;%i<%minigame.numMembers;%i++) if(isObject(%client = %minigame.member[%i]) && %client.getClassName() $= "GameConnection") 
				%client.play2d("item_get_sound");		 			
			
				nextMap("\c3" SPC $Pref::Server::MapRotation::minreset SPC "rounds have passed, loading the next map!");
   				$Pref::Server::MapRotation::ResetCount = 0;
			}

			$Pref::Server::MapRotation::ResetCount++;
			%minigame.chatMsgAll("\c3Round" SPC $Pref::Server::MapRotation::ResetCount SPC "of" SPC $Pref::Server::MapRotation::minreset);
		}

		Parent::Reset(%minigame, %client);
		
		// Loop through all minigame members to perform some actions
		for (%i=0;%i<%minigame.numMembers;%i++)
		if (isObject(%client = %minigame.member[%i]) && %client.getClassName() $= "GameConnection") 
		{
			// Reset the escape flag
			%client.escaped = 0;
			
			// Remove the Eventide music emitter if it exists and reset the music level
			if (isObject(%client.EventidemusicEmitter))
			{
				%client.EventidemusicEmitter.delete();
				%client.musicChaseLevel = 0;
			}

			// Play the round start sound and announce the minigame
			if (strlwr(%minigame.title) $= "eventide") 
			{
				%minigame.centerprintall("<font:impact:40>\c3Eventide: The Hunt",2);
				%minigame.bottomprintall("<font:impact:20>\c3Local chat is enabled, find a radio to broadcast to other survivors!",4);
				%client.play2d("round_start_sound");
			}
		}

		if(isObject(Eventide_MinigameGroup)) Eventide_MinigameGroup.delete();
		
		if(isObject($EventideRitualBrick)) $EventideRitualBrick.ritualsPlaced = 0;

		%minigame.randomizeEventideItems(true);
		if (strlwr(%minigame.title) $= "eventide") 
		$Pref::Server::ChatMod::lchatEnabled = 1;
    }

    function MinigameSO::endGame(%minigame,%client)
    {
        Parent::endGame(%minigame,%client);

		//Disable local chat
		$Pref::Server::ChatMod::lchatEnabled = 0;
		%minigame.bottomprintall("<font:impact:20>\c3Local chat disabled",4);

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter)) 
		{
			%client.EventidemusicEmitter.delete();
			%client.escaped = false;
		}

		if(isObject(Eventide_MinigameGroup)) 
		Eventide_MinigameGroup.delete();

		%minigame.randomizeEventideItems(false);
    }
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Minigame)) deactivatePackage(Eventide_Minigame);
activatePackage(Eventide_Minigame);

function MiniGameSO::playSound(%minigame,%datablock)
{
	if(!isObject(%minigame) || !isObject(%datablock)) return;
	
	for(%i = 0; %i < %minigame.numMembers; %i++)
	if(isObject(%member = %minigame.member[%i])) %member.play2D(%datablock);	
}
