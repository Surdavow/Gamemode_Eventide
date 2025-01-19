function serverCmdResetMinigame(%client)
{
	if (!%client.isAdmin && !isObject(%client.minigame))
	{
		return;
	}
	%client.minigame.reset();	
}

function serverCmdResetMG(%client)
{
	serverCmdResetMinigame(%client);
}

function MiniGameSO::checkDownedSurvivors(%minigame)
{
	// This will only work team based Slayer minigames, this will check if all survivors are incapacitated and end the round if they are
	if (!isObject(%teams = %minigame.teams))
	{
		return;
	}
					
	for (%i = 0; %i < %teams.getCount(); %i++) 
	{
		%team = %teams.getObject(%i);
		if (!isObject(%team)) 
		{
			continue;
		}
		
		%teamNameLower = strlwr(%team.name);
		if (strstr(%teamNameLower, "hunter") != -1) 
		{ 
			%hunterteam = %team;
		}

		if (strstr(%teamNameLower, "survivor") != -1) 
		{
			for (%j = 0; %j < %team.numMembers; %j++) 
			{
				%member = %team.member[%j].player;
				if (isObject(%member) && !%member.getdataBlock().isDowned) 
				{
					%livingcount++;
				}
			}
		}
	}

	if (!%livingcount)
	{
		%minigame.endRound(%hunterteam);
		return;
	}
}

function MiniGameSO::playSound(%minigame,%datablock)
{
	if (!isObject(%minigame) || !isObject(%datablock)) return;
	
	for (%i = 0; %i < %minigame.numMembers; %i++)
	if (isObject(%member = %minigame.member[%i])) %member.play2D(%datablock);	
}

$Eventide_SurvivorClasses = "mender runner hoarder fighter tinkerer";

function MiniGameSO::assignSurvivorClasses(%minigame)
{	
	// Return if there are no teams
	if (!isObject(%teams = %minigame.teams) || !%teams.getCount())
	return;
	
	// Create a temporary simset to hold the team members
	%memberSet = new SimSet();

	// Loop through each team and add the team members to the temporary simset
	for (%i = 0; %i < %teams.getCount(); %i++)
	if (isObject(%team = %teams.getObject(%i)) && strlwr(%team.name $= "survivors"))
	{
		for (%j = 0; %j < %team.numMembers; %j++) if (isObject(%team.member[%j].player))
		%memberSet.add(%team.member[%j]);	
	}

	// Return if there are no team members
	if (!%memberset.getCount())
	{
		%memberset.delete();
		return;
	}

	// Shuffle the survivor class list
	%newClassList = shuffleWords($Eventide_SurvivorClasses);

	// Assign the survivor classes to a random team member, preventing duplicates
	for (%i = 0; %i < getWordCount(%newClassList); %i++)
	{
		%randomMember = %memberSet.getObject(getRandom(0,%memberSet.getCount()-1));
		if (%minigame.survivorClass[getWord(%newClassList,%i)] || %randommember.player.survivorClass !$= "") continue;

		%randomMember.player.survivorClass = getWord(%newClassList,%i);
		%minigame.survivorClass[getWord(%newClassList,%i)] = %randomMember;

		if (%randomMember.player.getdataBlock().getName() $= "EventidePlayer") 
		%randomMember.player.getDatablock().assignClass(%randomMember.player,getWord(%newClassList,%i));
	}

	// Delete the temporary simset
	%memberSet.delete();
}