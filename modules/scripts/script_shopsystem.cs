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

        if(%client.customtitle !$= "") %client.clanPrefix = %bitmap @ "<color:" @ %color @ ">" @ "<font:" @ %font @ ":25>" @ %client.customtitle SPC "";
        else %client.clanPrefix = %bitmap @ "";
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
        case 3: %type = "EventideInstrumentsShopMenu";
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
    menuOption[3] = "Instruments";
    menuFunction[3] = "openOptionShop";
    menuOption[4] = "Exit";
    menuFunction[4] = "exitCenterprintMenu";    

    justify = "<just:right>";
    menuOptionCount = 5;
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

    menuOption[0] = "Heart - 25 Points";
    menuFunction[0] = "BuyEffect";
    menuOption[1] = "Stinky - 25 Points";
    menuFunction[1] = "BuyEffect";
    menuOption[2] = "Confetti - 25 Points";
    menuFunction[2] = "BuyEffect";
    menuOption[3] = "Electric - 50 Points";
    menuFunction[3] = "BuyEffect";
    menuOption[4] = "Fire - 50 Points";
    menuFunction[4] = "BuyEffect";
    menuOption[5] = "Sparkle - 50 Points";
    menuFunction[5] = "BuyEffect";
    menuOption[6] = "Return";
    menuFunction[6] = "returnToMainShopMenu";

    justify = "<just:right>";
    menuOptionCount = 7;
};

function BuyEffect(%client,%menu,%option)
{
    %client.exitCenterprintMenu();
    if(%client.hasEffect[%option]) return commandToClient(%client, 'messageboxOK', "Error", "You already have this! Check your effects with /effect");
    else
    {
        if(%option < 3)
        {
            if(%client.score > 25)
            {
                %client.incScore(-25);
                %client.hasEffect[%option] = true;
                commandToClient(%client, 'messageboxOK', "Success", "Successfully purchased this effect! It will be applied automatically, remove or reapply it with /effect");
            }
            else return;
        }
        else if(%option < 6)
        {
            if(%client.score > 50)
            {
                %client.incScore(-50);
                %client.hasEffect[%option] = true;
                commandToClient(%client, 'messageboxOK', "Success", "Successfully purchased this effect! It will be applied automatically, remove or reapply it with /effect");
            }
            else return;
        }
    }  

    switch(%option)
    {
        case 0: %client.player.mountImage("HeartStatusImage",2);
                %client.effect = "HeartStatusImage";
        case 1: %client.player.mountImage("StinkyStatusImage",2);
                %client.effect = "StinkyStatusImage";
        case 2: %client.player.mountImage("ConfettiStatusImage",2);
                %client.effect = "ConfettiStatusImage";
        case 3: %client.player.mountImage("ElectricStatusImage",2);
                %client.effect = "ElectricStatusImage";
        case 4: %client.player.mountImage("FireStatusImage",2);
                %client.effect = "FireStatusImage";
        case 5: %client.player.mountImage("SparkleStatusImage",2);
                %client.effect = "SparkleStatusImage";
    }
}

function servercmdeffect(%client,%type)
{    
    if(!isObject(%client.player)) return;

    switch$(strlwr(%type))
    {        
        case "heart":   if(%client.hasEffect[0]) 
                        {
                            %client.player.mountImage("HeartStatusImage",2);
                            %client.effect = "HeartStatusImage";
                        }

        case "stinky":  if(%client.hasEffect[1]) 
                        {
                            %client.player.mountImage("StinkyStatusImage",2);
                            %client.effect = "StinkyStatusImage";
                        }

        case "confetti":    if(%client.hasEffect[2]) 
                            {
                                %client.player.mountImage("ConfettiStatusImage",2);
                                %client.effect = "ConfettiStatusImage";
                            }

        case "electric":    if(%client.hasEffect[3]) 
                            {
                                %client.player.mountImage("ElectricStatusImage",2);
                                %client.effect = "ElectricStatusImage";
                            }

        case "fire":    if(%client.hasEffect[4]) 
                        {
                            %client.player.mountImage("FireStatusImage",2);
                            %client.effect = "FireStatusImage";
                        }

        case "sparkle": if(%client.hasEffect[5]) 
                        {
                            %client.player.mountImage("SparkleStatusImage",2);
                            %client.effect = "SparkleStatusImage";
                        }
        case "none":    %client.player.unmountimage(2);
                        %client.effect = "";

        default:    messageClient(%client, '', "<tab:280>\c6Effects List");
                    messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
                    if(%client.hasEffect[0]) messageClient(%client, '', "<tab:280>\c6Heart");
                    if(%client.hasEffect[1]) messageClient(%client, '', "<tab:280>\c6Stinky");
                    if(%client.hasEffect[2]) messageClient(%client, '', "<tab:280>\c6Confetti");
                    if(%client.hasEffect[3]) messageClient(%client, '', "<tab:280>\c6Electric");
                    if(%client.hasEffect[4]) messageClient(%client, '', "<tab:280>\c6Fire");
                    if(%client.hasEffect[5]) messageClient(%client, '', "<tab:280>\c6Sparkle");
                    messageClient(%client, '', "<tab:280>\c6None");

    }
}

if(isObject(EventideTitlesShopMenu)) EventideTitlesShopMenu.delete();
new ScriptObject(EventideTitlesShopMenu)
{
    isCenterprintMenu = 1;
    menuName = "Eventide Shop - Titles";

    menuOption[0] = "Title - 50 points";
    menuFunction[0] = "BuyTitle";
    menuOption[1] = "Color - 25 points";
    menuFunction[1] = "BuyTitle";    
    menuOption[2] = "Bitmaps - 50 Points";
    menuFunction[2] = "BuyTitle";
    menuOption[3] = "Font - 50 Points";
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
        case 0: if(%client.canChangeTitle)
                {                    
                    commandToClient(%client, 'messageboxOK', "Nope", "You have this already!");                
                    return;
                }
        
                if(%client.score < 25)
                {
                    commandToClient(%client, 'messageboxOK', "Fail!", "Not enough!");
                    return;
                }
                else 
                {
                    commandToClient(%client, 'messageboxOK', "Success!", "Use the /st command to change your title!");
                    %client.canChangeTitle = true;
                    %client.incScore(-25);
                }

                return;

        case 1: if(%client.canChangeTitleColor)
                {                    
                    commandToClient(%client, 'messageboxOK', "Nope", "You have this already!");
                    return;
                }
        
                if(%client.score < 50)
                {
                    commandToClient(%client, 'messageboxOK', "Fail!", "Not enough!");
                    return;
                }
                else 
                {
                    commandToClient(%client, 'messageboxOK', "Success!", "Use the /st bitmap command to change your title!");
                    %client.canChangeTitleBitmap = true;
                    %client.incScore(-50);
                }

                return;

        case 2: if(%client.canChangeTitleBitmap)
                {                    
                    commandToClient(%client, 'messageboxOK', "Nope", "You have this already!");
                    return;
                }
        
                if(%client.score < 25)
                {
                    commandToClient(%client, 'messageboxOK', "Fail!", "Not enough!");
                    return;
                }                
                else 
                {
                    commandToClient(%client, 'messageboxOK', "Success!", "Use the /st color command to change your title color! Make sure it's HEX!");
                    %client.canChangeTitleColor = true;
                    %client.incScore(-25);
                }

                return;

        case 3: if(%client.canChangeTitleFont)
                {                    
                    commandToClient(%client, 'messageboxOK', "Nope", "You have this already!");
                    return;
                }
        
                if(%client.score < 50) messageClient(%client, '', "\c0You don't have enough to buy a title Font.");
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
        case "title":   if(!%client.canChangeTitle) return messageClient(%client, '', "\c0You havent purchased this ability!");

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

        case "color":   if(!%client.canChangeTitleColor) return messageClient(%client, '', "\c0You havent purchased this ability!");
        
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
                
        case "font":    if(!%client.canChangeTitleFont) return messageClient(%client, '', "\c0You havent purchased this ability!");
        
                        %client.startCenterprintMenu(EventideSetTitleFontMenu);
                        return;

        case "Bitmap":  if(!%client.canChangeTitleBitmap) return messageClient(%client, '', "\c0You havent purchased this ability!");
                        
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
    menuOption[4] = "Georgia";
    menuFunction[4] = "EventideSetCustomFont";
    menuOption[5] = "Courier New";
    menuFunction[5] = "EventideSetCustomFont";
    menuOption[6] = "Exit";
    menuFunction[6] = "exitCenterprintMenu";

    justify = "<just:right>";
    menuOptionCount = 7;
};

function EventideSetCustomFont(%client,%menu,%option)
{    
    switch(%option)
    {
        case 0: %client.customtitlefont = "Impact";
        case 1: %client.customtitlefont = "Comic Sans MS";
        case 2: %client.customtitlefont = "Arial";
        case 3: %client.customtitlefont = "Constantia";
        case 4: %client.customtitlefont = "Georgia";
        case 4: %client.customtitlefont = "Courier New";
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
    menuOption[2] = "Skull";
    menuFunction[2] = "EventideSetCustomBitmap";
    menuOption[3] = "Blue Ribbon";
    menuFunction[3] = "EventideSetCustomBitmap";
    menuOption[4] = "Car";
    menuFunction[4] = "EventideSetCustomBitmap";
    menuOption[5] = "Bomb";
    menuFunction[5] = "EventideSetCustomBitmap";
    menuOption[6] = "Trophy";
    menuFunction[6] = "EventideSetCustomBitmap";
    menuOption[7] = "Star";
    menuFunction[7] = "EventideSetCustomBitmap";
    menuOption[8] = "Exit";
    menuFunction[8] = "exitCenterprintMenu";

    justify = "<just:right>";
    menuOptionCount = 9;
};

function EventideSetCustomBitmap(%client,%menu,%option)
{    
    switch(%option)
    {
        case 0: %client.customtitlebitmap = "<bitmap:add-ons/weapon_bow/CI_arrow.png>";
        case 1: %client.customtitlebitmap = "<bitmap:add-ons/weapon_gun/CI_gun.png>";
        case 2: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/skull.png>";
        case 3: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/blueRibbon.png>";
        case 4: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/car.png>";
        case 5: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/bomb.png>";
        case 6: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/trophy.png>";
        case 7: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/star.png>";
    }
}