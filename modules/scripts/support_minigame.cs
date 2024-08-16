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

		if(isObject(PuppetGroup)) PuppetGroup.delete();
		if(isObject(Shire_BotGroup)) Shire_BotGroup.delete();
		if(isObject(EventideShapeGroup)) EventideShapeGroup.delete();
		if(isObject(DroppedItemGroup)) DroppedItemGroup.delete();
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
		if(isObject(Shire_BotGroup)) Shire_BotGroup.delete();
		if(isObject(PuppetGroup)) PuppetGroup.delete();

		%minigame.randomizeEventideItems(false);
    }
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Minigame)) deactivatePackage(Eventide_Minigame);
activatePackage(Eventide_Minigame);
