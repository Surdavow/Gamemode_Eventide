$ShopTitlesList = "Title Color Bitmap Font";

if(isObject(EventideTitlesShopMenu)) EventideTitlesShopMenu.delete();
new ScriptObject(EventideTitlesShopMenu)
{
    menuName = "Eventide Shop - Titles";
    isCenterprintMenu = true;
    justify = "<just:right>";
    menuOptionCount = getWordCount($ShopTitlesList)+1;
};

for (%i = 0; %i < getWordCount($ShopTitlesList); %i++) 
{    
    if(%i <= 1) %price = " - 25 Points";
    else %price = " - 50 Points";
    
    EventideTitlesShopMenu.menuOption[%i] = getWord($ShopTitlesList,%i) @ %price;
    EventideTitlesShopMenu.menuFunction[%i] = "BuyTitle";
}

EventideTitlesShopMenu.menuOption[getWordCount($ShopTitlesList)] = "Return";
EventideTitlesShopMenu.menuFunction[getWordCount($ShopTitlesList)] = "returnToMainShopMenu";

function BuyTitle(%client,%menu,%option)
{
    %client.exitCenterprintMenu();
    if(%client.hasTitleAccess[%option]) return commandToClient(%client, 'messageboxOK', "Error", "You already have this! Check your title option with /st" SPC strlwr(getWord($ShopTitlesList,%option)));
    else
    {
        if(%option <= 1) %minScore = 25;
        else %minScore = 50;
            
        if(%client.score > %minScore)
        {
            %client.incScore(-%minScore);
            %client.hasTitleAccess[%option] = true;
            commandToClient(%client, 'messageboxOK', "Success", "Successfully purchased! Check your title option with /st" SPC strlwr(getWord($ShopTitlesList,%option)));
        }
        else return commandToClient(%client, 'messageboxOK', "Error", "Not enough points!");
    }
}

function servercmdst(%client,%type,%input)
{
    switch$(strlwr(%type))
    {
        case "title":   if(!%client.hasTitleAccess[0]) return messageClient(%client, '', "\c0You haven't purchased this ability!");

                        for(%i = 0; %i < strLen(%input); %i++)
                        if(strStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-", getSubStr(%input, %i, 1)) == -1 || strLen(%input) > 15)
                        {
                            messageClient(%client, '', "\c0The title needs to be less than 15 characters and cannot contain illegal characters!");
                            messageClient(%client, '', "\c0Allowed characters: ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-");
                            return;
                        }                        

                        return %client.customtitle = strreplace(%input,"_"," ");

        case "color":   if(!%client.hasTitleAccess[1]) return messageClient(%client, '', "\c0You haven't purchased this ability!");
        
                        for(%i = 0; %i < strLen(strlwr(%input)); %i++)
                        if(strStr("abcdefghijklmnopqrstuvwxyz0123456789", getSubStr(strlwr(%input), %i, 1)) == -1 || strLen(strlwr(%input)) != 6)
                        return messageClient(%client, '', "\c0The color needs to be a HEX value!");

                        return %client.customtitlecolor = strupr(%input);                                  

        case "bitmap":  if(!%client.hasTitleAccess[2]) return messageClient(%client, '', "\c0You haven't purchased this ability!");
                        
                        return %client.startCenterprintMenu(EventideSetTitleBitmapMenu);                        

        case "font":    if(!%client.hasTitleAccess[3]) return messageClient(%client, '', "\c0You haven't purchased this ability!");
        
                        return %client.startCenterprintMenu(EventideSetTitleFontMenu);                        
    }
}

$TitleBitmapList = "skull blue_ribbon car bomb trophy star generic demobanner lagicon gglogo150 greenlight-steam-logo title";

if(isObject(EventideSetTitleBitmapMenu)) EventideSetTitleBitmapMenu.delete();
new ScriptObject(EventideSetTitleBitmapMenu)
{
    menuName = "Title - Set Font";
    isCenterprintMenu = true;
    justify = "<just:right>";
    menuOptionCount = getWordCount($TitleBitmapList)+1;
};

for (%i = 0; %i < getWordCount($TitleBitmapList); %i++) 
{        
    EventideSetTitleBitmapMenu.menuOption[%i] = strreplace(getWord($TitleBitmapList,%i),"_"," ");
    EventideSetTitleBitmapMenu.menuFunction[%i] = "EventideSetCustomBitmap";
}

EventideSetTitleBitmapMenu.menuOption[getWordCount($TitleBitmapList)] = "Exit";
EventideSetTitleBitmapMenu.menuFunction[getWordCount($TitleBitmapList)] = "exitCenterprintMenu";

function EventideSetCustomBitmap(%client,%menu,%option)
{    
    if(%option <= 6) return %client.customtitlebitmap = "<bitmap:base/client/ui/CI/" @ strreplace(getWord($TitleBitmapList,%option),"_","") @ ".png>";
    else return %client.customtitlebitmap = "<bitmap:base/client/ui/" @ strreplace(getWord($TitleBitmapList,%option),"_","") @ ".png>";
}

$TitleFontList = "Impact Arial Constantia Georgia Courier_New Comic_Sans_MS Corbel Cambria";

if(isObject(EventideSetTitleFontMenu)) EventideSetTitleFontMenu.delete();
new ScriptObject(EventideSetTitleFontMenu)
{
    menuName = "Title - Set Font";
    isCenterprintMenu = true;
    justify = "<just:right>";
    menuOptionCount = getWordCount($TitleFontList)+1;
};

for (%i = 0; %i < getWordCount($TitleFontList); %i++) 
{        
    EventideSetTitleFontMenu.menuOption[%i] = strreplace(getWord($TitleFontList,%i),"_"," ");
    EventideSetTitleFontMenu.menuFunction[%i] = "EventideSetCustomFont";
}

EventideSetTitleFontMenu.menuOption[getWordCount($TitleFontList)] = "Exit";
EventideSetTitleFontMenu.menuFunction[getWordCount($TitleFontList)] = "exitCenterprintMenu";

function EventideSetCustomFont(%client,%menu,%option)
{    
    return %client.customtitlefont = strreplace(getWord($TitleFontList,%option),"_"," ");
}