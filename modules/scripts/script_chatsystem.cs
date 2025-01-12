package Eventide_LocalChat
{
    function ServerCmdStartTalking(%client)
	{
		// Prevent players being exposed when talking in local chat
        if ($MinigameLocalChat)
        {
            return;
        }
		Parent::ServerCmdStartTalking(%client);
	}

    function serverCmdMessageSent(%client,%message)
	{
        // Reset clan prefix and suffix
        %client.clanPrefix = "";
        %client.clanSuffix = "";
        
        // Do not continue if local chat is disabled
		if (!$MinigameLocalChat)
        {
            return Parent::ServerCmdMessageSent(%client, %message);
        }

        // PortEvalPlus support
        %allow = %client.canEval || ($Pref::Server::ChatEval::SuperAdmin && %client.isSuperAdmin && %client.canEval !$= "0");
		if (%allow && getSubStr(%text, 0, 1) $= "\\")
        {
            return Parent::serverCmdMessageSent(%client, %message);
        }
		
        // Process the message then forward it to the main chat system
		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);
		if (%message !$= "")
        {
            ChatMod_LocalChat(%client,%message);
        }		

        // Update the last message sent and echo the message
		%client.lastMessageSent = %message;
		echo(%client.name @ ": " @ getSubStr(%message, 0, strlen(%message)));		
	}

	function ServerCmdTeamMessageSent(%client, %message)
	{
		// Reset clan prefix and suffix
        %client.clanPrefix = "";
        %client.clanSuffix = "";

        // Do not continue if local chat is disabled
		if (!$MinigameLocalChat)
        {
            return Parent::ServerCmdMessageSent(%client, %message);
        }

        // Update the last message sent and echo the message
		%client.lastMessageSent = %message;
		echo("(TEAMS)" SPC %client.name @ ": " @ getSubStr(%message, 0, strlen(%message)));

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);		
		if (%message !$= "" && isObject(%client.player))
		{
            %client.player.playThread(3,talk);
            %client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);

            %inventoryToolCount = (%client.player.hoarderToolCount) ? %client.player.hoarderToolCount : %client.player.getDataBlock().maxTools;            
			for (%i = 0; %i < %inventoryToolCount; %i++) if (isObject(%client.player.tool[%i]) && %client.player.tool[%i].getName() $= "RadioItem")
			{
				ChatMod_RadioMessage(%client,%client.player,%message);
				ChatMod_LocalChat(%client, %message);                
                return;
			}

			return messageClient(%client,'',"\c5Find a radio to use team chat.");		
		}
		else return messageClient(%client,'',"\c5Respawn and find a radio to use team chat.");	
	}
};

// In case the package is already activated, deactivate it first before reactivating it
if (isPackage(Eventide_LocalChat)) deactivatePackage(Eventide_LocalChat);
activatePackage(Eventide_LocalChat);

/// Processes a chat message for a client before it is sent to the server
/// @param client The client who sent the message
/// @param message The message to be processed
/// @param lastMessageSent The last message sent by the client
/// @return The processed message to be sent to the server
function ChatMod_processMessage(%client, %message, %lastMessageSent) 
{    
    // Trim and clean the message
    %message = trim(stripMLControlChars(%message));
    if (%message $= "")
    {
        return false;
    }

    // Handle spam or duplicate messages
    if (%message $= %lastMessageSent && !%client.isAdmin) 
    {
        messageClient(%client, '', "Do not spam.");
        return false;
    }

    // Process URLs and check for restricted words (etard filter)
    %wordCount = getWordCount(%message);

    for (%i = 0; %i < %wordCount; %i++) 
    {
        %word = getWord(%message, %i);

        // Convert URLs to clickable links
        if (strstr(%word, "://") != -1) 
        {
            %message = setWord(%message, %i, "<a:" @ %word @ ">" @ %word @ "</a>");
        }

        // Check against the restricted word list
        if ($Pref::Server::ETardFilter) 
        {
            for (%j = 0; %j < getWordCount($Pref::Server::ETardList); %j++) 
            {
                // Check if the word matches any of the restricted words
                if (%word $= getWord($Pref::Server::ETardList, %j)) 
                {                    
                    messageClient(%client, '', "This is a civilized game. Please use full words.");
                    return false;
                }
            }
        }
    }

    return %message;
}

/// Shows a chat message as a shape name above the player's head.
/// @param player The player to show the message above
/// @param message The message to be shown
/// @param messageType The type of message, either 'whisper', 'shout', or 'normal'. The distance of the
/// shape name depends on the type of message, with whisper being 10, shout being 30, and normal being 20
function ChatMod_ShowShapeName(%player, %message, %messageType) 
{
    if (!isObject(%player)) return;

    // The distance of the shape name depends on the type of message, whisper is 10, shout is 30, normal is 20
    %distance = (%messageType $= "whisper") ? 10 : ((%messageType $= "shout") ? 30 : 20);

    %player.setShapeNameDistance(%distance);
    %player.setShapeNameColor("1 1 1");
    %player.setShapeName(%message, "8564862");

    // Cancel any existing schedule for the shape name, schedule the shape name to be removed after 5 seconds
    cancel(%player.shapeNameSchedule);
    %player.shapeNameSchedule = %player.schedule(5000, "setShapeName", "", "8564862");
}

/// Gets the prefix and type for a chat message, and strips any
/// unnecessary characters from the message.
///
/// @param message The chat message to process
/// @return A tab-separated string containing the prefix, type, and stripped message
function ChatMod_GetMessagePrefix(%message) 
{   
    // Return if the message is empty
    if (!strLen(%message)) return "" TAB "" TAB "";

    // Global message
    if (getSubStr(%message, 0, 1) $= "&") return "\c6[\c4GLOBAL\c6] " TAB "global" TAB getSubStr(%message, 1, strLen(%message) - 1);

    // Shouting...
    if (strstr(%message,"!") != -1 || (strCmp(%message, strUpr(%message)) == 0 && strlen(stripChars(%message, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")) != strlen(%message))) 
    return "\c3(SHOUT) " TAB "shout" TAB %message;

    // Whispering
    if (strstr(%message,"...") != -1) return "\c3(whisper) " TAB "whisper" TAB %message;

    // Normal message
    return "" TAB "" TAB %message;
}

/// Sends a message to a client in the style of a roleplay chat message.
///
/// @param targetClient The client to receive the message
/// @param prefix The prefix to add to the message
/// @param name The name of the player sending the message
/// @param suffix The suffix to add to the message
/// @param text The main content of the message
function chatMessageClientRP(%targetClient, %prefix, %name, %suffix, %text)
{
    %msgString = '\c7%1\c3%2\c7%3\c6: %4';
    chatMessageClient(%targetClient, "", "",  "", %msgString, %prefix, %name, %suffix, %text);
}

/// Local chat system for sending messages between players within a certain distance.
/// 
/// This function handles local chat messages by determining the type of message, 
/// stripping any unnecessary characters, sending the message to nearby players, 
/// and displaying the message above the player's head.
/// 
/// If the player is a skinwalker, the message will be sent using the victim-replicated client's identity for impersonation purposes.
/// 
/// @param %client The client sending the message.
/// @param %message The message content to be sent.
function ChatMod_LocalChat(%client, %message) 
{
    if (!isObject(%client)) return;
    
    //If the player is a skinwalker, use the victim replicated client instead for impersonation
	%tempclient = (isObject(%client.player) && isObject(%player.victimreplicatedclient)) ? %player.victimreplicatedclient : %client;

    // Process message to determine type and content
    %result = ChatMod_GetMessagePrefix(%message);
    %namePrefix = getField(%result, 0);
    %messageType = getField(%result, 1);
    %message = getField(%result, 2);

    if (%message $= "") return;

    // Global chat handling
    if (%messageType $= "global") 
    {
        messageAll('', %namePrefix @ "\c3" @ %tempclient.name @ "\c6: " @ %message);
        return;
    }

    // Configure chat distance based on message type
    %chatDistance = 30;
    if (%messageType $= "whisper")
    {
        %chatDistance *= 2;
    }    
    else if (%messageType $= "shout")
    {
        %chatDistance *= 0.5;
    }

    // Show the chat bubble or notification above the player
    ChatMod_ShowShapeName(%tempclient.player, %message, %messageType);

    if (isObject(%tempclient.player))
    {
        %tempclient.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);
        %tempclient.player.playThread(3,talk);
    }    

    // Send messages to nearby players
    for (%i = 0; %i < ClientGroup.getCount(); %i++) 
    {
        if (!isObject(%targetClient = ClientGroup.getObject(%i))) 
        {
            continue;
        }

        // Set up the server title for the prefix, if the player has one
        %color = (%tempclient.customtitlecolor $= "") ? "FFFFFF" : %tempclient.customtitlecolor;
        %bitmap = (%tempclient.customtitlebitmap $= "") ? "" : %tempclient.customtitlebitmap;

        // Concatenate the tite
        if (%tempclient.customtitle !$= "") 
        {
            %prefix = %bitmap @ "<color:" @ %color @ ">" @ %tempclient.customtitle SPC "";
        }
        // Concatenate the bitmap
        else if (%tempclient.customtitlebitmap !$= "") 
        {
            %prefix = %bitmap @ "";
        }
        // Concatenate nothing
        else 
        {
            %prefix = "";
        }

        %prefix = isObject(%tempclient.player) ? %prefix : "\c7[DEAD] ";

        // If the target client is dead, send either a dead player message or a normal message depending on player status
        if (!isObject(%targetClient.player)) 
        {
            chatMessageClientRP(%targetClient, %prefix, %namePrefix @ %tempclient.name, "", %message);
        }        
        
        // If the target client and player client are alive, check distance before sending a message
        else if (isObject(%tempclient.player) && isObject(%targetClient.player))
        {
            %playerDistance = vectorDist(%tempclient.player.getTransform(), %targetClient.player.getTransform());
            if (%playerDistance >= %chatDistance)
            {
                continue;
            }

            chatMessageClientRP(%targetClient, %prefix,%namePrefix @ %tempclient.name, "", %message);
        }
    }
}

/// Broadcast a message to all players who have a radio item in their inventory within the same game mode.
/// 
/// This function checks each player's inventory for a radio item and sends a formatted message to those players. 
/// If the player is a skinwalker, the message will be sent using the victim-replicated client's identity for impersonation purposes.
///
/// @param %client The client sending the message.
/// @param %player The player associated with the client sending the message.
/// @param %message The message content to be broadcasted.
function ChatMod_RadioMessage(%client, %player, %message)
{
    // Determine the client to use for message impersonation if the player is a skinwalker.
    %tempclient = (isObject(%player.victimreplicatedclient)) ? %player.victimreplicatedclient : %client;
    
    // Assign a random radi ID to the player if they don't have one.
    if (!%player.radioID)
    {
        %player.radioID = getRandom(100, 10000);
    }

    // Prefix and name formatting for the radio message.
    %pre = "\c4[Radio] ";
    %name = getSubStr(%tempclient.name, 0, 1) @ "." @ %player.radioID;
    %message = "\c4" @ %message;

    // Iterate through all clients in the game.
    for (%i = 0; %i < ClientGroup.getCount(); %i++)
    {
        if (isObject(%target = ClientGroup.getObject(%i)) && isObject(%target.player))
        {
            // Determine the tool inventory count for the target player.
            %inventoryToolCount = (%target.player.hoarderToolCount) ? %target.player.hoarderToolCount : %target.player.getDataBlock().maxTools;
            
            // Check if the target player has a radio item in their inventory.
            for (%j = 0; %j < %inventoryToolCount; %j++) 
            {
                if (isObject(%target.player.tool[%j]) && %target.player.tool[%j].getName() $= "RadioItem")
                {
                    // Play a notification sound for the radio message and send the message.
                    %target.player.playaudio(3, "radio_message_sound");
                    chatMessageClientRP(%target, %pre, %name, "", %message);		
                }
            }
        }
    }
}