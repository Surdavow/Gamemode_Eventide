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
        if(%option < 3) %minScore = 25;
        else if(%option < 6) %minScore = 50;
            
        if(%client.score > %minScore)
        {
            %client.incScore(-%minScore);
            %client.hasEffect[%option] = true;
            commandToClient(%client, 'messageboxOK', "Success", "Successfully purchased! Check your effect with /effect");
        }
        else return;
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
