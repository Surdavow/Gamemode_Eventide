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
	if(isObject(%client = clientGroup.getObject(%i)) && isObject(%player = %client.player) && %player.radioEquipped)
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

// Helper function to check message type and return both type and prefix
function ChatMod_GetMessagePrefix(%message) 
{
    // Default to normal message type
    %type = "normal";

    // Check for whisper (if the first character is '.')
    if (getSubStr(%message, 0, 1) $= ".") {
        // Validate that there are characters after the '.'
        if (strLen(%message) <= 1) {
            return "" TAB "" TAB ""; // Invalid message
        }
        %type = "whisper";
        %message = getSubStr(%message, 1, strLen(%message) - 1); // Remove the first character (.)
        return "\c3(whisper) " TAB %type TAB %message;
    }
    
    // Check for shouting (if the first character is '!')
    if (getSubStr(%message, 0, 1) $= "!") {
        // Validate that there are characters after the '!'
        if (strLen(%message) <= 1) {
            return "" TAB "" TAB ""; // Invalid message
        }
        %type = "shout";
        %message = getSubStr(%message, 1, strLen(%message) - 1); // Remove the first character (!)
        return "\c3(SHOUT) " TAB %type TAB %message;
    }

    // Return normal by default
    return "" TAB %type TAB %message;
}

// Helper function to display and schedule removal of shape name
function ChatMod_ShowShapeName(%player, %message, %messageType) 
{
    // Set distance based on message type
    %distance = (%messageType $= "whisper") ? 5 : ((%messageType $= "shout") ? 20 : 10);

    %player.setShapeNameDistance(%distance);
    %player.setShapeName(%message, "8564862");

    cancel(%player.shapeNameSchedule);
    %player.shapeNameSchedule = %player.schedule(5000, "setShapeName", "", "8564862");
}

function ChatMod_LocalChat(%client, %message) 
{
    if (!isObject(%client.player)) return;

    // Get message type, prefix, and updated message
    %result = ChatMod_GetMessagePrefix(%message);
    %namePrefix = getField(%result, 0); // Extract prefix
    %messageType = getField(%result, 1); // Extract message type
    %message = getField(%result, 2); // Extract the updated message without the leading character

    // If the message is invalid, stop processing
    if (%messageType $= "" || %message $= "") {
        return; // Don't send empty or invalid messages
    }
    
    // Calculate chat distance based on type
    %chatDistance = $Pref::Server::ChatMod::lChatDistance;
    if (%messageType $= "whisper")
        %chatDistance *= $Pref::Server::ChatMod::lChatWhisperMultiplier;
    else if (%messageType $= "shout")
        %chatDistance *= $Pref::Server::ChatMod::lChatShoutMultiplier;

    // Handle skinwalker replication
    %sourceClient = (isObject(%client.player.victimReplicatedClient)) ? 
        %client.player.victimReplicatedClient : %client;
    
    // Show message above player's head with appropriate distance
    ChatMod_ShowShapeName(%client.player, %message, %messageType);
    
    // Send messages to clients in range
    for (%i = 0; %i < ClientGroup.getCount(); %i++) 
    {
        %targetClient = ClientGroup.getObject(%i);
        if (!isObject(%targetClient)) 
            continue;
        
        if (isObject(%targetClient.player)) 
        {
            %playerDistance = vectorDist(%client.player.getTransform(), %targetClient.player.getTransform());
            if (%playerDistance >= %chatDistance)
                continue;
                
            // Calculate message size if enabled
            if ($Pref::Server::ChatMod::lChatSizeModEnabled) 
            {
                %sizeDiff = $Pref::Server::ChatMod::lChatSizeMax - $Pref::Server::ChatMod::lChatSizeMin;
                %size = mFloor(((%chatDistance - %playerDistance) / %chatDistance * %sizeDiff)) + 
                    $Pref::Server::ChatMod::lChatSizeMin;
                    
                chatMessageClientRPSize(%targetClient, %sourceClient.clanPrefix, 
                    %namePrefix @ %sourceClient.name @ %nameSuffix, 
                    %sourceClient.clanSuffix, %message, %size);
            } else {
                chatMessageClientRP(%targetClient, %sourceClient.clanPrefix, 
                    %namePrefix @ %sourceClient.name @ %nameSuffix, 
                    %sourceClient.clanSuffix, %message);
            }
        } else {
            // Simple message for clients without players
            chatMessageClientRP(%targetClient, %sourceClient.clanPrefix, 
                %namePrefix @ %sourceClient.name @ %nameSuffix, 
                %sourceClient.clanSuffix, %message);
        }
    }
}

function ChatMod_RadioMessage(%client, %message, %isTeamMessage)
{
	//If were the skinwalker and we have a replicated victim
	if(isObject(%player = %client.player) && isObject(%player.victimreplicatedclient)) %tempclient = %player.victimreplicatedclient;
	else %tempclient = %client;

	if(getSubStr(%message, 0, 1) $= ".") %message = strreplace(%message, getSubStr(%message, 0, 1), "");	

	%pre = "\c4[Radio]";	

	if(!%client.player.radioID) %client.player.radioID = getRandom(100, 10000);
	%name = getSubStr(%tempclient.name, 0, 1) @ "." @ %client.player.radioID;	
	%message = "\c4" @ %message;

	for(%i = 0; %i < clientGroup.getCount(); %i++)
	{
		if(isObject(%target = clientGroup.getObject(%i)) && isObject(%target.player))
		if(%target.player.radioEquipped) 
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