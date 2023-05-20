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

        case 1: if(%client.canChangeTitleBitmap)
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

        case 2: if(%client.canChangeTitleColor)
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