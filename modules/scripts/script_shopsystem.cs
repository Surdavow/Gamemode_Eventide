package Eventide_CustomTitles
{
	function serverCmdMessageSent(%client,%msg)
	{
		if(%client.customtitlecolor $= "") %color = "FFFFFF";
        else %color = %client.customtitlecolor;

        if(%client.customtitlefont $= "") %font = "Arial";
        else %font = %client.customtitlefont;

        %client.clanPrefix = "<color:" @ %color @ ">" @ "<font:" @ %font @ ":25>" @ %client.customtitle SPC "";
		Parent::serverCmdMessageSent(%client,%msg);
	}
};
activatepackage(Eventide_CustomTitles);

function servercmdshop(%client)
{
    %client.startCenterprintMenu(EventideShopMainMenu);
}

function exitCenterprintMenu(%client)
{
    %client.exitCenterprintMenu();
}

function returnToMainShopMenu(%client)
{
    %client.exitCenterprintMenu();
    %client.startCenterprintMenu(EventideShopMainMenu);
}

function openOptionShop(%client,%menu,%option)
{
    %client.exitCenterprintMenu();

    switch(%option)
    {
        case 0: %type = "EventideHatShopMenu";
        case 1: %type = "EventideEffectsShopMenu";
        case 2: %type = "EventideTitlesShopMenu";
    }

    %client.startCenterprintMenu(%type);
}

function PromptCustomTitle(%client)
{
    switch(%client.canChangeTitle)
    {
        case true: messageClient(%client, '', "\c0You already purchased the ability to set titles.");
        case false: commandToClient(%client, 'messageBoxYesNo', "Custom Title - 25 points", "Are you sure you want to buy a custom title?",'BuyCustomTitle');
    }    
}

function serverCmdBuyCustomTitle(%client)
{
    if(%client.score < 25) messageClient(%client, '', "\c0You don't have enough to buy a title.");
    else 
    {
        commandToClient(%client, 'messageboxOK', "Success!", "Use the /settitle command to change your title!");
        %client.canChangeTitle = true;
        %client.incScore(-25);
    }
}

function servercmdsettitle(%client,%title)
{
    %allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

	for(%i = 0; %i < strLen(%title); %i++)
	if(strStr(%allowed, getSubStr(%title, %i, 1)) == -1)
	{
		%forbidden = true;
		break;
	}

    %title = strreplace(%title,"_"," ");
	
	if(%forbidden || strLen(%title) > 15)
	{
		messageClient(%client, '', "\c0The title needs to be less than 15 characters and cannot contain illegal characters!");
		return;
	}

    %client.customtitle = %title;
}

function PromptCustomTitleBitmap(%client)
{
    switch(%client.canChangeTitleBitmap)
    {
        case true: messageClient(%client, '', "\c0You already purchased the ability to set title bitmaps.");
        case false: commandToClient(%client, 'messageBoxYesNo', "Custom Title Bitmap - 50 points", "Are you sure you want to buy a custom title bitmap?",'BuyCustomTitleBitmap');
    }    
}

function serverCmdBuyCustomTitleBitmap(%client)
{
    if(%client.score < 50) messageClient(%client, '', "\c0You don't have enough to buy a title bitmap.");
    else 
    {
        commandToClient(%client, 'messageboxOK', "Success!", "Use the /settitlebitmap command to change your title!");
        %client.canChangeTitleBitmap = true;
        %client.incScore(-50);
    }
}

function PromptCustomTitleColor(%client)
{
    switch(%client.canChangeTitleColor)
    {
        case true: messageClient(%client, '', "\c0You already purchased the ability to set title colors.");
        case false: commandToClient(%client, 'messageBoxYesNo', "Custom Title Color - 25 points", "Are you sure you want to buy a custom title color?",'BuyCustomTitleColor');
    }    
}

function serverCmdBuyCustomTitleColor(%client)
{
    if(%client.score < 25) messageClient(%client, '', "\c0You don't have enough to buy a title color.");
    else 
    {
        commandToClient(%client, 'messageboxOK', "Success!", "Use the /settitlecolor command to change your title color! Make sure it's HEX!");
        %client.canChangeTitleColor = true;
        %client.incScore(-25);
    }
}

function servercmdsettitlecolor(%client,%color)
{
    %allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

	for(%i = 0; %i < strLen(%color); %i++)
	if(strStr(%allowed, getSubStr(%color, %i, 1)) == -1)
	{
		%forbidden = true;
		break;
	}
	
	if(%forbidden || strLen(%color) > 6)
	{
		messageClient(%client, '', "\c0The color needs to be a HEX color!");
		return;
	}

    %client.customtitlecolor = %color;
}

function PromptCustomTitleFont(%client)
{
    switch(%client.canChangeTitleFont)
    {
        case true: messageClient(%client, '', "\c0You already purchased the ability to set title Fonts.");
        case false: commandToClient(%client, 'messageBoxYesNo', "Custom Title Font - 50 points", "Are you sure you want to buy a custom title Font?",'BuyCustomTitleFont');
    }    
}

function serverCmdBuyCustomTitleFont(%client)
{
    if(%client.score < 50) messageClient(%client, '', "\c0You don't have enough to buy a title Font.");
    else 
    {
        commandToClient(%client, 'messageboxOK', "Success!", "Use the /settitleFont command to change your title Font!");
        %client.canChangeTitleFont = true;
        %client.incScore(-50);
    }
}

function servercmdsettitleFont(%client,%font)
{
    %allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

	for(%i = 0; %i < strLen(%font); %i++)
	if(strStr(%allowed, getSubStr(%font, %i, 1)) == -1)
	{
		%forbidden = true;
		break;
	}
	
	if(%forbidden || strLen(%font) > 15)
	{
		messageClient(%client, '', "\c0Use an actual font, don't use illegal characters!");
		return;
	}

    %client.customtitleFont = %font;
}

function TitleShopOptions(%client,%menu,%option)
{
    %client.exitCenterprintMenu();

    switch(%option)
    {
        case 0: PromptCustomTitle(%client);
        case 1: PromptCustomTitleBitmap(%client);
        case 2: PromptCustomTitleFont(%client);
        case 3: PromptCustomTitleColor(%client);
    }

    %client.startCenterprintMenu(%type);
}

if(isObject(EventideShopMainMenu)) EventideShopMainMenu.delete();
new ScriptObject(EventideShopMainMenu)
{
    isCenterprintMenu = 1;
    menuName = "Eventide Shop";

    menuOption[0] = "Hats";
    menuFunction[0] = "openOptionShop";
    menuOption[1] = "Effects";
    menuFunction[1] = "openOptionShop";    
    menuOption[2] = "Custom Title";
    menuFunction[2] = "openOptionShop";
    menuOption[3] = "Exit";
    menuFunction[3] = "exitCenterprintMenu";    

    justify = "<just:right>";
    menuOptionCount = 4;
};

if(isObject(EventideHatShopMenu)) EventideHatShopMenu.delete();
new ScriptObject(EventideHatShopMenu)
{
    isCenterprintMenu = 1;
    menuName = "Eventide Shop - Hats";

    menuOption[0] = "Cap";
    menuFunction[0] = "";
    menuOption[1] = "Fancy";
    menuFunction[1] = "";
    menuOption[2] = "Straw Hat";
    menuFunction[2] = "";    
    menuOption[3] = "Mask";
    menuFunction[3] = "";
    menuOption[4] = "Top";
    menuFunction[4] = "";
    menuOption[5] = "Return";
    menuFunction[5] = "returnToMainShopMenu";

    justify = "<just:right>";
    menuOptionCount = 6;
};

if(isObject(EventideEffectsShopMenu)) EventideEffectsShopMenu.delete();
new ScriptObject(EventideEffectsShopMenu)
{
    isCenterprintMenu = 1;
    menuName = "Eventide Shop - Effects";

    menuOption[0] = "Hearts";
    menuFunction[0] = "EventideBuyEffect";
    menuOption[1] = "Stinky";
    menuFunction[1] = "EventideBuyEffect";
    menuOption[2] = "Heals";
    menuFunction[2] = "EventideBuyEffect";    
    menuOption[3] = "Electric";
    menuFunction[3] = "EventideBuyEffect";
    menuOption[4] = "Fire";
    menuFunction[4] = "EventideBuyEffect";
    menuOption[5] = "Sparkles";
    menuFunction[5] = "";
    menuOption[6] = "Return";
    menuFunction[6] = "returnToMainShopMenu";

    justify = "<just:right>";
    menuOptionCount = 7;
};

if(isObject(EventideTitlesShopMenu)) EventideTitlesShopMenu.delete();
new ScriptObject(EventideTitlesShopMenu)
{
    isCenterprintMenu = 1;
    menuName = "Eventide Shop - Titles";

    menuOption[0] = "Title";
    menuFunction[0] = "TitleShopOptions";
    menuOption[1] = "Bitmaps";
    menuFunction[1] = "TitleShopOptions";
    menuOption[2] = "Font";
    menuFunction[2] = "TitleShopOptions";    
    menuOption[3] = "Color";
    menuFunction[3] = "TitleShopOptions";
    menuOption[4] = "Return";
    menuFunction[4] = "returnToMainShopMenu";

    justify = "<just:right>";
    menuOptionCount = 7;
};