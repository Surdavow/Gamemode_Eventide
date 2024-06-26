package Eventide_Minigame
{
	function Slayer_MiniGameSO::endRound(%this, %winner, %resetTime)
	{
		Parent::endRound(%this, %winner, %resetTime);
		
		if (strlwr(%this.title) $= "eventide")
		{
			
			for (%i=0;%i<%this.numMembers;%i++) if (isObject(%client = %this.member[%i]) && %client.getClassName() $= "GameConnection") 
			%client.play2d("round_end_sound");		 						
		}
	}

    function MiniGameSO::Reset(%minigame,%client)
	{
        Parent::Reset(%minigame,%client);

		if ($Pref::Server::MapRotation::ResetCount > $Pref::Server::MapRotation::minreset) 
		{
			%msg = "<font:arial:26><color:FFFF00>Map Rotator\c6 -\c3" SPC $Pref::Server::MapRotation::minreset SPC "\c6rounds have passed, time to get a fresh map!";
			nextMap(%msg);
		}

		if (ClientGroup.getCount() >= 0) 
		$Pref::Server::MapRotation::ResetCount++;

		Parent::Reset(%obj, %client);		

		if(strlwr(%minigame.title) $= "eventide")
		{
			for(%i=0;%i<%minigame.numMembers;%i++)
			if(isObject(%client = %minigame.member[%i]) && %client.getClassName() $= "GameConnection") 
			{
				%client.play2d("round_start_sound");		 			
				%client.escaped = false;
			}
			%minigame.centerprintall("<font:impact:40>\c3Eventide: The Hunt begins",2);
		}

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter))
        {
            %client.EventidemusicEmitter.delete();
            %client.musicChaseLevel = 0;
        }

		if(isObject(Shire_BotGroup)) Shire_BotGroup.delete();
		if(isObject(EventideShapeGroup)) EventideShapeGroup.delete();
		if(isObject(DroppedItemGroup)) DroppedItemGroup.delete();

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
		if(isObject(Shire_BotGroup)) Shire_BotGroup.delete();

		%minigame.randomizeEventideItems(false);
    }
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Minigame)) deactivatePackage(Eventide_Minigame);
activatePackage(Eventide_Minigame);