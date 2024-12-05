$ShopInstrumentList = "GuitarImage BanjoImage HarmonicaImage ViolinImage KeytarImage FluteImage ElectricGuitarImage ElectricBassImage";
$ShopTitleList = "Title Color Bitmap";

if(isObject(EventideShopMainMenu)) EventideShopMainMenu.delete();
new ScriptObject(EventideShopMainMenu)
{
    isCenterprintMenu = 1;
    menuName = "Eventide Shop";

    menuOption[0] = "Custom Title";
    menuFunction[0] = "openOptionShop";
    menuOption[1] = "Instruments";
    menuFunction[1] = "openOptionShop";
    menuOption[2] = "Exit";
    menuFunction[2] = "exitCenterprintMenu";    

    justify = "<just:right>";
    menuOptionCount = 5;
};

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
        case 0: %type = "EventideTitlesShopMenu";
        case 1: %type = "EventideInstrumentsShopMenu";
    }
    %client.startCenterprintMenu(%type);
}

if(isObject(EventideInstrumentsShopMenu)) EventideInstrumentsShopMenu.delete();
new ScriptObject(EventideInstrumentsShopMenu)
{
    menuName = "Eventide Shop - Instruments";
    isCenterprintMenu = true;
    justify = "<just:right>";
    menuOptionCount = getWordCount($ShopInstrumentList)+1;    
};

for(%i = 0; %i <= getWordCount($ShopInstrumentList); %i++) 
{    
    EventideInstrumentsShopMenu.menuOption[%i] = strreplace(getWord($ShopInstrumentList,%i),"Image","") @ " - 50 Points";
    EventideInstrumentsShopMenu.menuFunction[%i] = "BuyInstrument";    
}

EventideInstrumentsShopMenu.menuOption[getWordCount($ShopInstrumentList)] = "Return";
EventideInstrumentsShopMenu.menuFunction[getWordCount($ShopInstrumentList)] = "returnToMainShopMenu";

function BuyInstrument(%client,%menu,%option)
{
    %client.exitCenterprintMenu();
    if(%client.hasInstrument[%option]) return commandToClient(%client, 'messageboxOK', "Error", "You already have this! Check your instrument with /instrument");
    else if(%client.score >= 50)
    {
        %client.incScore(-50);
        %client.hasInstrument[%option] = true;
        commandToClient(%client, 'messageboxOK', "Success", "Successfully purchased this instrument! Check your instrument with /instrument");
    }
    else return commandToClient(%client, 'messageboxOK', "Error", "Not enough points!");
}

function serverCmdInstrument(%client,%instrument)
{    
    if(!isObject(%client) || !isObject(%client.player)) return;
    if(strlwr(%instrument) $= "none") return %client.player.unmountImage(0);
    
    for(%i = 0; %i <= getWordCount($ShopInstrumentList); %i++)
    if(strlwr(%instrument) $= strreplace(strlwr(getWord($ShopInstrumentList,%i)),"image","") && %client.hasInstrument[%i]) return %client.player.mountImage(getWord($ShopInstrumentList,%i),0);            

    messageClient(%client, '', "<tab:280>\c6Your Instruments List");
    messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
    for(%i = 0; %i <= getWordCount($ShopInstrumentList); %i++) if(%client.hasInstrument[%i]) 
    messageClient(%client, '', "<tab:280>\c6" @ strreplace(getWord($ShopInstrumentList,%i),"Image",""));
    messageClient(%client, '', "<tab:280>\c6None");
    %client.player.unmountImage(0);
}

if(isObject(EventideTitlesShopMenu)) EventideTitlesShopMenu.delete();
new ScriptObject(EventideTitlesShopMenu)
{
    menuName = "Eventide Shop - Titles";
    isCenterprintMenu = true;
    justify = "<just:right>";
    menuOptionCount = getWordCount($ShopTitleList)+1;
};

for (%i = 0; %i < getWordCount($ShopTitleList); %i++) 
{    
    if(%i <= 1) %price = " - 25 Points";
    else %price = " - 50 Points";
    
    EventideTitlesShopMenu.menuOption[%i] = getWord($ShopTitleList,%i) @ %price;
    EventideTitlesShopMenu.menuFunction[%i] = "BuyTitle";
}

EventideTitlesShopMenu.menuOption[getWordCount($ShopTitleList)] = "Return";
EventideTitlesShopMenu.menuFunction[getWordCount($ShopTitleList)] = "returnToMainShopMenu";

function BuyTitle(%client,%menu,%option)
{
    %client.exitCenterprintMenu();
    if(%client.hasTitleAccess[%option]) return commandToClient(%client, 'messageboxOK', "Error", "You already have this! Check your title option with /st" SPC strlwr(getWord($ShopTitleList,%option)));
    else
    {
        if(%option <= 1) %minScore = 25;
        else %minScore = 50;
            
        if(%client.score >= %minScore)
        {
            %client.incScore(-%minScore);
            %client.hasTitleAccess[%option] = true;
            commandToClient(%client, 'messageboxOK', "Success", "Successfully purchased! Check your title option with /st" SPC strlwr(getWord($ShopTitleList,%option)));
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
                        if(strStr("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", getSubStr(%input, %i, 1)) == -1 || strLen(%input) > 15)
                        {
                            messageClient(%client, '', "\c0The title needs to be one word, than 15 characters and cannot contain illegal characters!");
                            messageClient(%client, '', "\c0Allowed characters: ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
                            return;
                        }                        

                        if(%input !$= "")
                        {
                            %client.customtitle = %input;
                            messageClient(%client, '', "\c2Set title to" SPC %client.customtitle);
                        }
                        else
                        {
                            %client.customtitle = %input;
                            messageClient(%client, '', "\c2Cleared title");                            
                        }
                        return;

        case "color":   if(!%client.hasTitleAccess[1]) return messageClient(%client, '', "\c0You haven't purchased this ability!");
        
                        for(%i = 0; %i < strLen(strlwr(%input)); %i++)
                        if(strStr("abcdefghijklmnopqrstuvwxyz0123456789", getSubStr(strlwr(%input), %i, 1)) == -1 || strLen(strlwr(%input)) != 6)
                        return messageClient(%client, '', "\c0The color needs to be a HEX value!");

                        if(%input !$= "")
                        {
                            %client.customtitlecolor = %input;
                            messageClient(%client, '', "\c2Set color to" SPC %client.customtitlecolor);
                        }
                        else
                        {
                            %client.customtitlecolor = %input;
                            messageClient(%client, '', "\c2Cleared color");                            
                        }                        
                        return;                              

        case "bitmap":  if(!%client.hasTitleAccess[2]) return messageClient(%client, '', "\c0You haven't purchased this ability!");
                        
                        return %client.startCenterprintMenu(EventideSetTitleBitmapMenu);                                   
    }
}

$TitleBitmapList = "skull blue_ribbon car bomb trophy star generic demobanner lagicon gglogo150 greenlight-steam-logo PaintLabel crossHair decalFrame";

if(isObject(EventideSetTitleBitmapMenu)) EventideSetTitleBitmapMenu.delete();
new ScriptObject(EventideSetTitleBitmapMenu)
{
    menuName = "Title - Set Bitmap";
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