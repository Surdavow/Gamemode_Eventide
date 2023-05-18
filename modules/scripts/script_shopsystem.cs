package Eventide_CustomTitles
{
	function serverCmdMessageSent(%client,%msg)
	{
		if(%client.customtitlecolor $= "") %color = "FFFFFF";
        else %color = %client.customtitlecolor;

        if(%client.customtitlefont $= "") %font = "Palatino Linotype";
        else %font = %client.customtitlefont;

        if(%client.customtitlebitmap $= "") %bitmap = "";
        else %bitmap = %client.customtitlebitmap;

        %client.clanPrefix = %bitmap @ "<color:" @ %color @ ">" @ "<font:" @ %font @ ":25>" @ %client.customtitle SPC "";
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

    menuOption[0] = "Title - 50 points";
    menuFunction[0] = "BuyTitle";
    menuOption[1] = "Color - 25 points";
    menuFunction[1] = "BuyTitle";    
    menuOption[2] = "Bitmaps";
    menuFunction[2] = "BuyTitle";
    menuOption[3] = "Font";
    menuFunction[3] = "BuyTitle";    
    menuOption[4] = "Return";
    menuFunction[4] = "returnToMainShopMenu";

    justify = "<just:right>";
    menuOptionCount = 5;
};

function BuyTitle(%client,%menu,%option)
{
    switch(%option)
    {
        case 0: if(%client.score < 25) messageClient(%client, '', "\c0You don't have enough to buy a title.");                    
                else 
                {
                    commandToClient(%client, 'messageboxOK', "Success!", "Use the /st command to change your title!");
                    %client.canChangeTitle = true;
                    %client.incScore(-25);
                }

                return;

        case 1: if(%client.score < 50) messageClient(%client, '', "\c0You don't have enough to buy a title bitmap.");
                else 
                {
                    commandToClient(%client, 'messageboxOK', "Success!", "Use the /st bitmap command to change your title!");
                    %client.canChangeTitleBitmap = true;
                    %client.incScore(-50);
                }

                return;

        case 2: if(%client.score < 25) messageClient(%client, '', "\c0You don't have enough to buy a title color.");
                else 
                {
                    commandToClient(%client, 'messageboxOK', "Success!", "Use the /st color command to change your title color! Make sure it's HEX!");
                    %client.canChangeTitleColor = true;
                    %client.incScore(-25);
                }

                return;

        case 3: if(%client.score < 50) messageClient(%client, '', "\c0You don't have enough to buy a title Font.");
                else 
                {
                    commandToClient(%client, 'messageboxOK', "Success!", "Use the /st font command to change your title Font!");
                    %client.canChangeTitleFont = true;
                    %client.incScore(-50);
                }
                
                return;
    }
}

function servercmdst(%client,%type,%input)
{
    switch$(%type)
    {
        case "title":   if(!%client.canChangeTitle) return;

                        %allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

                        for(%i = 0; %i < strLen(%input); %i++)
                        if(strStr(%allowed, getSubStr(%input, %i, 1)) == -1)
                        {
                           %forbidden = true;
                           break;
                        }

                        %input = strreplace(%input,"_"," ");

                        if(%forbidden || strLen(%input) > 15)
                        {
                           messageClient(%client, '', "\c0The title needs to be less than 15 characters and cannot contain illegal characters!");
                           return;
                        }

                        %client.customtitle = %input;
                        return;

        case "color":   if(!%client.canChangeTitleColor) return;
        
                        %allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                        for(%i = 0; %i < strLen(%input); %i++)
                        if(strStr(%allowed, getSubStr(%input, %i, 1)) == -1)
                        {
                            %forbidden = true;
                            break;
                        }

                        if(%forbidden || strLen(%input) > 6)
                        {
                            messageClient(%client, '', "\c0The color needs to be a HEX color!");
                            return;
                        }

                        %client.customtitlecolor = %input;
                        return;
                
        case "font":    if(!%client.canChangeTitleFont) return;
        
                        %client.startCenterprintMenu(EventideSetTitleFontMenu);
                        return;

        case "Bitmap":  if(!%client.canChangeTitleBitmap) return;
                        
                        %client.startCenterprintMenu(EventideSetTitleBitmapMenu);
                        return;                        
    }
}

if(isObject(EventideSetTitleFontMenu)) EventideSetTitleFontMenu.delete();
new ScriptObject(EventideSetTitleFontMenu)
{
    isCenterprintMenu = 1;
    menuName = "Title - Set Font";

    menuOption[0] = "Impact";
    menuFunction[0] = "EventideSetCustomFont";
    menuOption[1] = "Comic Sans MS";
    menuFunction[1] = "EventideSetCustomFont";
    menuOption[2] = "Arial";
    menuFunction[2] = "EventideSetCustomFont";    
    menuOption[3] = "Constantia";
    menuFunction[3] = "EventideSetCustomFont";
    menuOption[4] = "Exit";
    menuFunction[4] = "exitCenterprintMenu";

    justify = "<just:right>";
    menuOptionCount = 5;
};

function EventideSetCustomFont(%client,%menu,%option)
{    
    switch(%option)
    {
        case 0: %client.customtitlefont = "Impact";
        case 1: %client.customtitlefont = "Comic Sans MS";
        case 2: %client.customtitlefont = "Arial";
        case 3: %client.customtitlefont = "Constantia";
    }
}

if(isObject(EventideSetTitleBitmapMenu)) EventideSetTitleBitmapMenu.delete();
new ScriptObject(EventideSetTitleBitmapMenu)
{
    isCenterprintMenu = 1;
    menuName = "Title - Set Bitmap";

    menuOption[0] = "Arrow";
    menuFunction[0] = "EventideSetCustomBitmap";
    menuOption[1] = "Gun";
    menuFunction[1] = "EventideSetCustomBitmap";
    menuOption[2] = "Exit";
    menuFunction[2] = "exitCenterprintMenu";

    justify = "<just:right>";
    menuOptionCount = 3;
};

function EventideSetCustomBitmap(%client,%menu,%option)
{    
    switch(%option)
    {
        case 0: %client.customtitlebitmap = "<bitmap:add-ons/weapon_bow/CI_arrow.png>";
        case 1: %client.customtitlebitmap = "<bitmap:add-ons/weapon_gun/CI_gun.png>";
    }
}