package Eventide_LocalChat
{
    function ServerCmdStartTalking(%client)
	{
		if($MinigameLocalChat) return;
		else parent::ServerCmdStartTalking(%client);
	}

    function serverCmdMessageSent(%client,%message)
	{
        %client.clanPrefix = "";
        %client.clanSuffix = "";

		if(!$MinigameLocalChat)
		return Parent::ServerCmdMessageSent(%client, %message);

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);
		if(%message !$= "") ChatMod_LocalChat(%client, %message);		

		%client.lastMessageSent = %message;
		echo(%client.name @ ": " @ getSubStr(%message, 0, strlen(%message)));		
	}

	function ServerCmdTeamMessageSent(%client, %message)
	{
		%client.clanPrefix = "";
        %client.clanSuffix = "";

		if(!$MinigameLocalChat)
		return Parent::ServerCmdTeamMessageSent(%client, %message);

        %client.lastMessageSent = %client;

		%message = ChatMod_processMessage(%client,%message,%client.lastMessageSent);
		if(!%message) return;
		
		if(isObject(%client.player))
		{        
			for (%i = 0; %i < %inventoryToolCount; %i++) if (isObject(%obj.tool[%i]) && %obj.tool[%i].getName() $= "RadioItem")
			{
				ChatMod_RadioMessage(%client, %message);
				ChatMod_LocalChat(%client, %message);
                %client.player.playThread(3,talk);
			    %client.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);
                return;
			}
            
			return messageClient(%client,'',"\c5Find a radio to use team chat.");		
		}
		else return messageClient(%client,'',"\c5Respawn and find a radio to use team chat.");	
	}
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_LocalChat)) deactivatePackage(Eventide_LocalChat);
activatePackage(Eventide_LocalChat);

function chatMessageClientRP(%target,%pre,%name,%suf,%text)
{
    %msgString = '\c7%1\c3%2\c7%3\c6: %4';
    chatMessageClient(%target, "", "",  "", %msgString, %pre, %name, %suf, %text);
}

function ChatMod_processMessage(%client, %message, %lastMessageSent) 
{    
    %message = trim(stripMLControlChars(%message));
    if (%message $= "") return false;
    
    // Check for duplicate messages (spam)
    if (%message $= %lastMessageSent && !%client.isAdmin) 
    {
        messageClient(%client, '', "Do not spam.");
        return false;
    }
    
    // Process URLs
    for (%i = 0; %i < getWordCount(%message); %i++) 
    {
        %word = getWord(%message, %i);
        if (getSubStr(%word, 0, 7) $= "http://") %message = setWord(%message, %i, "<a:" @ %word @ ">" @ %word @ "</a>");        
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

function ChatMod_GetMessagePrefix(%message) 
{   
    // Return if the message is empty
    if(!strLen(%message)) return "" TAB "" TAB "";

    // Global message
    if(getSubStr(%message, 0, 1) $= "&") return "\c6[\c4GLOBAL\c6] " TAB "global" TAB getSubStr(%message, 1, strLen(%message) - 1);

    // Shouting are we?!!!
    if(strstr(%message,"!") != -1 || (strCmp(%message, strUpr(%message)) == 0 && strlen(stripChars(%message, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")) != strlen(%message))) 
    return "\c3(SHOUT) " TAB "shout" TAB %message;

    // Or whispering...
    if(strstr(%message,"...") != -1) return "\c3(whisper) " TAB "whisper" TAB %message;

    // Or just a normal message
    return "" TAB "" TAB %message;
}

function ChatMod_LocalChat(%client, %message) 
{
    if (!isObject(%client)) return;
    
    //If the player is a skinwalker, use the victim replicated client instead for impersonation
	if(isObject(%player = %client.player) && isObject(%player.victimreplicatedclient)) %tempclient = %player.victimreplicatedclient;
	else %tempclient = %client;

    // Process message to determine type and content
    %result = ChatMod_GetMessagePrefix(%message);
    %namePrefix = getField(%result, 0);
    %messageType = getField(%result, 1);
    %message = getField(%result, 2);

    if(%message $= "") return;

    // Global chat handling
    if (%messageType $= "global") 
    {
        messageAll('', %namePrefix @ "\c3" @ %tempclient.name @ "\c6: " @ %message);
        return;
    }

    // Configure chat distance based on message type
    %chatDistance = 30;
    if (%messageType $= "whisper") %chatDistance *= 2;
    else if (%messageType $= "shout") %chatDistance *= 0.5;

    // Show the chat bubble or notification above the player
    ChatMod_ShowShapeName(%tempclient.player, %message, %messageType);

    if(isObject(%tempclient.player))
    {
        %tempclient.player.schedule(mCeil(strLen(%message)/6*300),playthread,3,root);
        %tempclient.player.playThread(3,talk);
    }    

    // Send messages to nearby players
    for (%i = 0; %i < ClientGroup.getCount(); %i++) 
    {
        if (!isObject(%targetClient = ClientGroup.getObject(%i))) continue;

        %color = (%tempclient.customtitlecolor $= "") ? "FFFFFF" : %tempclient.customtitlecolor;
        %bitmap = (%tempclient.customtitlebitmap $= "") ? "" : %tempclient.customtitlebitmap;

		if (%tempclient.customtitle !$= "") 
		%prefix = %bitmap @ "<color:" @ %color @ ">" @ %tempclient.customtitle SPC "";
		
		else %prefix = (%tempclient.customtitlebitmap !$= "") ? (%bitmap @ "") : "";

        %prefix = isObject(%tempclient.player) ? %prefix : "\c7[DEAD] ";

        // If the target client is dead, send either a dead player message or a normal message depending on player status
        if (!isObject(%targetClient.player)) 
        chatMessageClientRP(%targetClient, %prefix, %namePrefix @ %tempclient.name, "", %message);
        
        
        // If the target client and player client are alive, check distance before sending a message
        else if (isObject(%tempclient.player) && isObject(%targetClient.player))
        {
            %playerDistance = vectorDist(%tempclient.player.getTransform(), %targetClient.player.getTransform());
            if (%playerDistance >= %chatDistance) continue;

            chatMessageClientRP(%targetClient, %prefix,%namePrefix @ %tempclient.name, "", %message);
        }
    }
}

function ChatMod_RadioMessage(%client, %message)
{
	//If the player is a skinwalker, use the victim replicated client instead for impersonation
	if(isObject(%player = %client.player) && isObject(%player.victimreplicatedclient)) %tempclient = %player.victimreplicatedclient;
	else %tempclient = %client;
    
    //If the player doesn't have a radio ID, generate one
    if(!%client.player.radioID) %client.player.radioID = getRandom(100, 10000);

	%pre = "\c4[Radio] ";	
	%name = getSubStr(%tempclient.name, 0, 1) @ "." @ %client.player.radioID;
	%message = "\c4" @ %message;

	for(%i = 0; %i < clientGroup.getCount(); %i++)
    if(isObject(%target = clientGroup.getObject(%i)) && isObject(%target.player))
    {
        %target.player.playaudio(3,"radio_message_sound");
        chatMessageClientRP(%target,%pre,%name,"",%message);		
    }
}