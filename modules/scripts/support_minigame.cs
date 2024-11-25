package Eventide_Minigame
{
	//TODO: Make dead players be able to chat with living players, preferably by setting a dead chat round variable to true.
	function Slayer_MiniGameSO::endRound(%minigame, %winner, %resetTime)
	{
		Parent::endRound(%minigame, %winner, %resetTime);
		
		if (strlwr(%minigame.title) !$= "eventide") return;
		
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
		if (ClientGroup.getCount() > 0 && $Pref::Server::MapRotation::enabled)
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
		
		for (%i=0;%i<%minigame.numMembers;%i++)
		if (isObject(%client = %minigame.member[%i]) && %client.getClassName() $= "GameConnection") 
		{
			if (isObject(%client.EventidemusicEmitter))
			{
				%client.EventidemusicEmitter.delete();
				%client.musicChaseLevel = 0;
			}
	 			
			%client.escaped = false;

			if (strlwr(%minigame.title) $= "eventide") 
			{
				%minigame.centerprintall("<font:impact:40>\c3Eventide: The Hunt begins",2);
				%client.play2d("round_start_sound");
			}
		}

		if(isObject(Eventide_MinigameGroup)) Eventide_MinigameGroup.delete();
		if(isObject(Eventide_ShapeGroup)) Eventide_ShapeGroup.delete();
		
		if($EventideRitualBrick)
		{
			$EventideRitualBrick.gemcount = 0;
			$EventideRitualBrick.candlecount = 0;
		}

		%minigame.randomizeEventideItems(true);
    }

    function MinigameSO::endGame(%minigame,%client)
    {
        Parent::endGame(%minigame,%client);

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter)) 
		{
			%client.EventidemusicEmitter.delete();
			%client.escaped = false;
		}

		if(isObject(Eventide_MinigameGroup)) Eventide_MinigameGroup.delete();
		if(isObject(Eventide_ShapeGroup)) Eventide_ShapeGroup.delete();

		%minigame.randomizeEventideItems(false);
    }
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Minigame)) deactivatePackage(Eventide_Minigame);
activatePackage(Eventide_Minigame);
