function fcbn(%cl)
{
	return findclientbyname(%cl);
}

function ServerCmdLocalChatHelp(%cl)
{
	messageClient(%cl, '', "\c3Modifiers (put at beginning of message):");
	if(ChatMod_getGlobalChatPerm(%cl)) messageClient(%cl, '', "\c4& \c6- Global chats the message");
	messageClient(%cl, '', "\c4ALL CAPS or ! \c6- Shouts your message; range is a multiple of the default chat range");
	messageClient(%cl, '', "\c4. \c6- Whispers your message; range is a multiple of the default chat range");
	messageClient(%cl, '', "\c4Team chat \c6- If you have your radio out, sends your message over the radio and over local chat via typing in team chat");
}

function ServerCmdChatHelp(%cl)
{
	ServerCmdLocalChatHelp(%cl);
}

function Chatmod_resetRadioChannels()
{
	for (%i = 0; %i < clientGroup.getCount(); %i++)
	if(isObject(%client = clientGroup.getObject(%i)) && isObject(%player = %client.player) && isObject(%player.getMountedImage(0)) && %player.getMountedImage(0).getName() $= "CRadioImage")
	{
		%client.centerPrint("\c5More channels are being broadcasted! Check what channels are available.",4);
		%player.playaudio(3,"radio_change_sound");
	}
}

function ChatMod_getGlobalChatPerm(%cl)
{
	switch($Pref::Server::ChatMod::lchatGlobalChatLevel)
	{
		case 1: return %cl.isAdmin;
		case 2: return %cl.isSuperAdmin;
		default: return true;
	}
}

function chatMessageClientRP(%target,%pre,%name,%suf,%text)
{
    %msgString = '\c7%1\c3%2\c7%3\c6: %4';
    chatMessageClient(%target, "", "",  "", %msgString, %pre, %name, %suf, %text);
}

function chatMessageClientRPSize(%target,%pre,%name,%suf,%text,%size)
{
	%fontmod = "<font:Palatino Linotype:" @ %size @ ">";
	chatMessageClientRP(%target, %fontmod@"\c7"@%pre, %fontmod@"\c3"@%name, %fontmod@"\c7"@%suf, %fontmod@"\c6"@%text);
}

function ChatMod_processMessage(%client,%message,%lastMessageSent)
{
	%message = trim(stripMLControlChars(%message)); //removes trailing spaces and html tags.

	for(%i = 0; %i < getWordCount(%message); %i++)
	{
		%word = getWord(%message, %i);
		if(getSubStr(%word, 0, 7) $= "http://" ) %message = setWord(%message, %i, "<a:" @ %word @ ">" @ %word @ "</a>");
	}

	//etard
	if($Pref::Server::ETardFilter) for(%i=0; %i < getWordCount(%message); %i++)
	{
		%word = getWord(%message, %i);
		for(%j=0; %j<getwordCount($Pref::Server::ETardList); %j++) if(%word $= getWord($Pref::Server::ETardList,%j)) 
		%etardcheck = true;
	}	

	//conditional check
	if(%etardcheck || (%message $= %lastMessageSent && !%client.isAdmin) || %message $= "")
	{
		if(%etardcheck) messageClient(%client, '', "This is a civilized game. Please use full words.");
		else if(%message $= %lastMessageSent && !%client.isAdmin) messageClient(%client, '', "Do not spam.");		
		return false;
	}

	return %message;
}

function ChatMod_LocalChat(%client, %message)
{
	//Check if were yelling or whispering
	if(getSubStr(%message, 0, 1) $= ".") 
	{		
		%message = strreplace(%message, getSubStr(%message, 0, 1), "");
		%nameprefix = "\c3(whisper) ";
		%isWhisper = true;
	}
	else if(((strstr(%message, "!") >= 0)) || (strCmp(%message, strUpr(%message)) == 0 && strlen(stripChars(%message, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")) != strlen(%message)))
	{
		%nameprefix = "\c3(SHOUT) ";
		%isYell = true;
	}

	//If were the skinwalker and we have a replicated victim
	if(isObject(%player = %client.player) && isObject(%player.victimreplicatedclient)) %tempclient = %player.victimreplicatedclient;
	else %tempclient = %client;	

	for(%i=0; %i<clientGroup.getCount(); %i++)
	{
		if(isObject(%targetClient=clientGroup.getObject(%i)) && isObject(%targetClient.player))
		{
			%playerdistance = vectorDist(%client.player.getTransform(),%targetClient.player.getTransform());
			%localchatdistance = (%isWhisper ? ($Pref::Server::ChatMod::lChatDistance*$Pref::Server::ChatMod::lchatWhisperMultiplier) : %localchatdistance) || (%isYell ? ($Pref::Server::ChatMod::lChatDistance*$Pref::Server::ChatMod::lchatShoutMultiplier) : $Pref::Server::ChatMod::lchatDistance);
			%diff = $Pref::Server::ChatMod::lchatSizeMax - $Pref::Server::ChatMod::lchatSizeMin;
			%size = mFloor(((%localchatdistance-%playerdistance) / %localchatdistance * %diff)) + $Pref::Server::ChatMod::lchatSizeMin;			
			
			if(%playerdistance < %localchatdistance)
			if(!$Pref::Server::ChatMod::lchatSizeModEnabled) chatMessageClientRP(%targetClient, %tempclient.clanPrefix, %namePrefix @ %tempclient.name @ %nameSuffix, %tempclient.clanSuffix, %message);
			else chatMessageClientRPSize(%targetClient, %tempclient.clanPrefix, %namePrefix @ %tempclient.name @ %nameSuffix, %tempclient.clanSuffix, %message, %size); 
		}
	}
}

function ChatMod_TeamLocalChat(%client, %message)
{
	//Check if were yelling or whispering
	if(getSubStr(%message, 0, 1) $= ".") 
	{		
		%message = strreplace(%message, getSubStr(%message, 0, 1), "");
		%nameprefix = "\c3(whisper) ";
		%isWhisper = true;
	}
	else if(((strstr(%message, "!") >= 0)) || (strCmp(%message, strUpr(%message)) == 0 && strlen(stripChars(%message, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")) != strlen(%message)))
	{
		%nameprefix = "\c3(SHOUT) ";
		%isYell = true;
	}

	//If were the skinwalker and we have a replicated victim
	if(isObject(%player = %client.player) && isObject(%player.victimreplicatedclient)) %tempclient = %player.victimreplicatedclient;
	else %tempclient = %client;

	for(%i=0; %i<clientGroup.getCount(); %i++)
	if(isObject(%targetClient=clientGroup.getObject(%i)) && isObject(%targetClient.player) && isObject(%targetClient.minigame) && %targetClient.minigame == %client.minigame)
	{
		%playerdistance = vectorDist(%client.player.getTransform(),%targetClient.player.getTransform());
		%localchatdistance = (%isWhisper ? ($Pref::Server::ChatMod::lChatDistance*$Pref::Server::ChatMod::lchatWhisperMultiplier) : %localchatdistance) || (%isYell ? ($Pref::Server::ChatMod::lChatDistance*$Pref::Server::ChatMod::lchatShoutMultiplier) : $Pref::Server::ChatMod::lchatDistance);
		%diff = $Pref::Server::ChatMod::lchatSizeMax - $Pref::Server::ChatMod::lchatSizeMin;
		%size = mFloor(((%localchatdistance-%playerdistance) / %localchatdistance * %diff)) + $Pref::Server::ChatMod::lchatSizeMin;		

		if(%playerdistance < %localchatdistance)
		{
			if(isObject(Slayer))
			{			
				if(%client.getTeam() == %targetClient.getTeam())
				if(!$Pref::Server::ChatMod::lchatSizeModEnabled) chatMessageClientRP(%targetClient, %tempclient.clanPrefix, %namePrefix @ %tempclient.name @ %nameSuffix, %tempclient.clanSuffix, %message);
				else chatMessageClientRPSize(%targetClient, %tempclient.clanPrefix, %namePrefix @ %tempclient.name @ %nameSuffix, %tempclient.clanSuffix, %message, %size);				
			}
			else if(!$Pref::Server::ChatMod::lchatSizeModEnabled) chatMessageClientRP(%targetClient, %tempclient.clanPrefix, %namePrefix @ %tempclient.name @ %nameSuffix, %tempclient.clanSuffix, %message);
			else chatMessageClientRPSize(%targetClient, %tempclient.clanPrefix, %namePrefix @ %tempclient.name @ %nameSuffix, %tempclient.clanSuffix, %message, %size);			
		}
	}
}

function ChatMod_RadioMessage(%client, %message, %isTeamMessage)
{
	//If were the skinwalker and we have a replicated victim
	if(isObject(%player = %client.player) && isObject(%player.victimreplicatedclient)) %tempclient = %player.victimreplicatedclient;
	else %tempclient = %client;

	if(getSubStr(%message, 0, 1) $= ".") 
	%message = strreplace(%message, getSubStr(%message, 0, 1), "");	

	%pre = "\c4[Channel "@ (%client.player.RadioChannel+1) @"]";	

	if(!%client.player.radioID) %client.player.radioID = getRandom(100, 10000);
	%name = getSubStr(%tempclient.name, 0, 1) @ "." @ %client.player.radioID;	
	%message = "\c4" @ %message;

	for(%i = 0; %i < clientGroup.getCount(); %i++)
	{
		if(isObject(%target = clientGroup.getObject(%i)) && isObject(%target.player))
		if(%target.player.RadioChannel == %client.player.RadioChannel && %target.player.RadioOn) 
		{
			if(%isTeamMessage && isObject(Slayer) && %client.getTeam() $= %target.getTeam())
			{
				chatMessageClientRP(%target,%pre,%name,"",%message);
				%target.player.playaudio(3,"radio_message_sound");
			}
			else
			{
				chatMessageClientRP(%target,%pre,%name,"",%message);
				%target.player.playaudio(3,"radio_message_sound");
			}
		}
	}
}

package Eventide_ChatSystem
{
	function ServerCmdTeamMessageSent(%client, %message)
	{
		if(!$Pref::Server::ChatMod::lchatEnabled)
		{
			Parent::ServerCmdTeamMessageSent(%client, %message);
			return;
		}

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);

		if(%message !$= "0")
		{
			if(isObject(%client.player))
			{
				%client.player.playThread(3,talk);
				%client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);

				if(%client.player.RadioOn) ChatMod_RadioMessage(%client, %message, true);
				if(isObject(%client.minigame)) ChatMod_TeamLocalChat(%client, %message);
				else if(!%client.player.RadioOn) messageClient(%client,'',"\c5You must be in a minigame to team chat.");
			}
			else messageClient(%client,'',"\c5You are dead. You must respawn to use team chat.");
			%client.lastMessageSent = %client;
		}			
	}

	function ServerCmdMessageSent(%client, %message)
	{		
		if(!$Pref::Server::ChatMod::lchatEnabled)
		{
			Parent::ServerCmdMessageSent(%client, %message);
			return;
		}		

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);

		if(%message !$= "0")
		{
			if(ChatMod_getGlobalChatPerm(%client) && getSubStr(%message, 0, 1) $= "&") 
			{
				messageAll('', "\c6[\c4GLOBAL\c6] \c3" @ %client.name @ "\c6: " @ getSubStr(%message, 1, strlen(%message)));
				if(isObject(%client.player))
				{				
					%client.player.playThread(3,talk);
					%client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);
				}
			}
			else if(isObject(%client.player))
			{
				ChatMod_LocalChat(%client, %message);
				%client.player.playThread(3,talk);
				%client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);
			}
			else for(%i=0; %i<clientGroup.getCount(); %i++) if(isObject(%targetClient = clientGroup.getObject(%i)) && !isObject(%targetClient.player)) 
			chatMessageClientRP(%targetClient, "", "\c7[DEAD] "@ %client.name, "", %message);
		}		
		%client.lastMessageSent = %message;
	}

	function ServerCmdStartTalking(%client)
	{
		if($Pref::Server::ChatMod::lchatEnabled) return;
		else parent::ServerCmdStartTalking(%client);
	}
};

if(isPackage(Eventide_ChatSystem)) deactivatePackage(Eventide_ChatSystem);
activatePackage(Eventide_ChatSystem);