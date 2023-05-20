$ShopEffectList = "HeartStatusImage StinkyStatusImage ConfettiStatusImage ElectricStatusImage SparkleStatusImage FireStatusImage";

if(isObject(EventideEffectsShopMenu)) EventideEffectsShopMenu.delete();
new ScriptObject(EventideEffectsShopMenu)
{
    menuName = "Eventide Shop - Effects";
    isCenterprintMenu = true;
    justify = "<just:right>";
};

for (%i = 0; %i < getWordCount($ShopEffectList); %i++) 
{    
    if(%i < 3) %price = " - 25 Points";
    else if (%i < 6) %price = " - 50 Points";
    
    EventideEffectsShopMenu.menuOption[%i] = strreplace(getWord($ShopEffectList,%i),"StatusImage","") @ %price;
    EventideEffectsShopMenu.menuFunction[%i] = "BuyEffect";
}

EventideEffectsShopMenu.menuOption[getWordCount($ShopEffectList)] = "Return";
EventideEffectsShopMenu.menuFunction[getWordCount($ShopEffectList)] = "returnToMainShopMenu";
EventideEffectsShopMenu.menuOptionCount = getWordCount($ShopEffectList)+1;

function BuyEffect(%client,%menu,%option)
{
    %client.exitCenterprintMenu();
    if(%client.hasEffect[%option]) return commandToClient(%client, 'messageboxOK', "Error", "You already have this! Check your effects with /effect");
    else
    {
        if(%option < 3) %minScore = 25;
        else if(%option < 6) %minScore = 50;
            
        if(%client.score > %minScore)
        {
            %client.incScore(-%minScore);
            %client.hasEffect[%option] = true;
            commandToClient(%client, 'messageboxOK', "Success", "Successfully purchased! Check your effect with /effect");
        }
        else return commandToClient(%client, 'messageboxOK', "Error", "Not enough points!");
    }  
}

function servercmdeffect(%client,%effect)
{    
    if(!isObject(%client) || !isObject(%client.player)) return;
    
    for(%i = 0; %i < getWordCount($ShopEffectList); %i++)
    if(strlwr(%effect) $= strreplace(strlwr(getWord($ShopEffectList,%i)),"statusimage","") && %client.hasEffect[%i])
    {
        %client.player.mountImage(getWord($ShopEffectList,%i),2);
        %client.effect = getWord($ShopEffectList,%i);
        return;
    }
    else %effectmismatch = true;

    if(%effectmismatch) 
    {
        messageClient(%client, '', "<tab:280>\c6Your Effects List");
        messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
        for(%i = 0; %i <= getWordCount($ShopEffectList); %i++) if(%client.hasEffect[%i]) 
        messageClient(%client, '', "<tab:280>\c6" @ strreplace(getWord($ShopEffectList,%i),"StatusImage",""));        
        %client.player.unmountImage(2);
        %client.effect = 0;
        return;
    }
}
