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

if(isObject(EventideInstrumentsShopMenu)) EventideInstrumentsShopMenu.delete();
new ScriptObject(EventideInstrumentsShopMenu)
{
    isCenterprintMenu = 1;
    menuName = "Eventide Shop - Instruments";

    menuOption[0] = "Guitar - 50 Points";
    menuFunction[0] = "BuyInstrument";
    menuOption[1] = "Banjo - 50 Points";
    menuFunction[1] = "BuyInstrument";
    menuOption[2] = "Harmonica - 50 Points";
    menuFunction[2] = "BuyInstrument";
    menuOption[3] = "Violin - 50 Points";
    menuFunction[3] = "BuyInstrument";
    menuOption[4] = "Keytar - 50 Points";
    menuFunction[4] = "BuyInstrument";
    menuOption[5] = "Flute - 50 Points";
    menuFunction[5] = "BuyInstrument";
    menuOption[6] = "Electric - 50 Points";
    menuFunction[6] = "BuyInstrument";
    menuOption[7] = "Bass - 50 Points";
    menuFunction[7] = "BuyInstrument";
    menuOption[8] = "Return";
    menuFunction[8] = "returnToMainShopMenu";    

    justify = "<just:right>";
    menuOptionCount = 9;
};

function BuyInstrument(%client,%menu,%option)
{
    %client.exitCenterprintMenu();
    if(%client.hasInstrument[%option]) return commandToClient(%client, 'messageboxOK', "Error", "You already have this! Check your instrument with /instrument");
    else if(%client.score > 50)
    {
        %client.incScore(-50);
        %client.hasInstrument[%option] = true;
        commandToClient(%client, 'messageboxOK', "Success", "Successfully purchased this instrument! Check your instrument with /instrument");
    }
    else return;
}

function servercmdinstrument(%client,%type)
{    
    if(!isObject(%client.player)) return;
    
    %image[%g = 0] = "GuitarImage";
    %image[%g++] = "BanjoImage";
    %image[%g++] = "HarmonicaImage";
    %image[%g++] = "ViolinImage";
    %image[%g++] = "KeytarImage";
    %image[%g++] = "FluteImage";
    %image[%g++] = "ElectricGuitarImage";
    %image[%g++] = "ElectricBassImage";

    if(%type $= "")
    {            
        messageClient(%client, '', "<tab:280>\c6Instruments List");
        messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
        for(%i = 0; %i <= %g; %i++)  
        if(%client.hasInstrument[%i]) messageClient(%client, '', "<tab:280>\c6" @ strreplace(%image[%i],"Image",""));
    }
    else
    {
        for(%i = 0; %i <= %g; %i++)  
        if(strlwr(%type) $= strlwr(strreplace(%image[%i],"Image","")))
        {
            if(%client.hasInstrument[%i]) 
            {
                %client.player.mountImage(%image[%i],0);
                return;
            }
            else %typemismatch++;
        }
        else %typemismatch++;

        if(%typemismatch) return messageClient(%client, '', "That item does not exist from the list or you do not own it.","");
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