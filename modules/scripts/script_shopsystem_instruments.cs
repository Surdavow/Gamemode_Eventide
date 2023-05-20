$ShopInstrumentList = "GuitarImage BanjoImage HarmonicaImage ViolinImage KeytarImage FluteImage ElectricGuitarImage ElectricBassImage";

if(isObject(EventideInstrumentsShopMenu)) EventideInstrumentsShopMenu.delete();
new ScriptObject(EventideInstrumentsShopMenu)
{
    menuName = "Eventide Shop - Instruments";
    isCenterprintMenu = true;
    justify = "<just:right>";
};

for(%i = 0; %i <= getWordCount($ShopInstrumentList); %i++) 
{    
    EventideInstrumentsShopMenu.menuOption[%i] = strreplace(getWord($ShopInstrumentList,%i),"Image","") @ " - 50 Points";
    EventideInstrumentsShopMenu.menuFunction[%i] = "BuyInstrument";    
}

EventideInstrumentsShopMenu.menuOption[getWordCount($ShopInstrumentList)] = "Return";
EventideInstrumentsShopMenu.menuFunction[getWordCount($ShopInstrumentList)] = "returnToMainShopMenu";
EventideInstrumentsShopMenu.menuOptionCount = getWordCount($ShopInstrumentList)+1;

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
    else return commandToClient(%client, 'messageboxOK', "Error", "Not enough points!");
}

function serverCmdInstrument(%client,%instrument)
{    
    if(!isObject(%client) || !isObject(%client.player)) return;
    
    for(%i = 0; %i <= getWordCount($ShopInstrumentList); %i++)
    if(strlwr(%instrument) $= strreplace(strlwr(getWord($ShopInstrumentList,%i)),"image","") && %client.hasInstrument[%i]) return %client.player.mountImage($ShopInstrument[%i],0);            
    else %instrumentmismatch = true;

    if(%instrumentmismatch) 
    {
        messageClient(%client, '', "<tab:280>\c6Your Instruments List");
        messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
        for(%i = 0; %i <= getWordCount($ShopInstrumentList); %i++) if(%client.hasInstrument[%i]) 
        messageClient(%client, '', "<tab:280>\c6" @ strreplace(getWord($ShopInstrumentList,%i),"Image",""));
        %client.player.unmountImage(0);
        return;
    }
}