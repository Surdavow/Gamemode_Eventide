package Eventide_Minigame
{
	function Slayer_MiniGameSO::endRound(%minigame, %winner, %resetTime)
	{
		Parent::endRound(%minigame, %winner, %resetTime);

		// Disable local chat at the end of the round, let everyone banter at the end.
		$MinigameLocalChat = false;
		%minigame.bottomprintall("<font:impact:20>\c3Local chat disabled",4);
		%minigame.playSound("round_end_sound");
	}

    function MiniGameSO::Reset(%minigame,%client)
	{
		if (ClientGroup.getCount() && $Pref::Server::MapRotation::enabled)
  		{
			if ($Pref::Server::MapRotation::ResetCount >= $Pref::Server::MapRotation::minreset) 
			{
				%minigame.playSound("item_get_sound");			
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

			for(%i = 0; %i < getWordCount($Eventide_SurvivorClasses); %i++)
			%minigame.survivorClass[getWord($Eventide_SurvivorClasses,%i)] = 0;

			%minigame.schedule(33,assignSurvivorClasses);
			
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
		
		if(isObject($EventideRitualBrick)) 
		{
			$EventideRitualBrick.ritualsPlaced = 0;
			$EventideRitualBrick.gemcount = 0;
			$EventideRitualBrick.candlecount = 0;
		}

		%minigame.randomizeEventideItems(true);

		if (strlwr(%minigame.title) $= "eventide" && $Pref::Server::ChatMod::lchatEnabled) 
		$MinigameLocalChat = true;
    }

    function MinigameSO::endGame(%minigame,%client)
    {
        Parent::endGame(%minigame,%client);

		//Disable local chat
		$MinigameLocalChat = false;
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

$Eventide_SurvivorClasses = "mender runner hoarder fighter tinkerer";

function MiniGameSO::assignSurvivorClasses(%minigame)
{	
	// Return if there are no teams
	if(!isObject(%teams = %minigame.teams) || !%teams.getCount())
	return;
	
	// Create a temporary simset to hold the team members
	%memberSet = new SimSet();

	// Loop through each team and add the team members to the temporary simset
	for(%i = 0; %i < %teams.getCount(); %i++)
	if(isObject(%team = %teams.getObject(%i)) && strlwr(%team.name $= "survivors"))
	{
		for(%j = 0; %j < %team.numMembers; %j++) if(isObject(%team.member[%j].player))
		%memberSet.add(%team.member[%j]);	
	}

	// Return if there are no team members
	if(!%memberset.getCount())
	{
		%memberset.delete();
		return;
	}

	// Assign the survivor classes to a random team member, preventing duplicates
	for(%i = 0; %i < getWordCount($Eventide_SurvivorClasses); %i++)
	{
		%randomMember = %memberSet.getObject(getRandom(0,%memberSet.getCount()-1));
		if(%minigame.survivorClass[getWord($Eventide_SurvivorClasses,%i)] == %randomMember || %randommember.player.survivorClass !$= "") 
		continue;

		%randomMember.player.survivorClass = getWord($Eventide_SurvivorClasses,%i);
		%minigame.survivorClass[getWord($Eventide_SurvivorClasses,%i)] = %randomMember;

		if(%randomMember.player.getdataBlock().getName() $= "EventidePlayer")
		%randomMember.player.getDatablock().assignClass(%randomMember.player,getWord($Eventide_SurvivorClasses,%i));
	}

	// Delete the temporary simset
	%memberSet.delete();
}