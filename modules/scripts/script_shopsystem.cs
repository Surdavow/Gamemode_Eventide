function servercmdshop(%client,%a,%b)
{
    switch$(strlwr(%a))
    {
	    case "killers":  messageClient(%client, '', "\c7--------------------------------------------------------------------------------");        
                        messageClient(%client, '', "<tab:140>\c3All available\c6" SPC %a SPC "\c3options:");
                        messageClient(%client, '', "<tab:140>\c6Grabber");
                        messageClient(%client, '', "<tab:140>\c6Skinwalker");
                        messageClient(%client, '', "<tab:140>\c6Shire");
                        messageClient(%client, '', "<tab:140>\c6Angler");
                        messageClient(%client, '', "<tab:140>\c6Puppet Master");
                        messageClient(%client, '', "<tab:140>\c6Renowened");
                        messageClient(%client, '', "<tab:140>\c6Skullwolf");
                        messageClient(%client, '', "<tab:140>\c6Effect");
                        messageClient(%client, '', "\c7--------------------------------------------------------------------------------");

	    case "survivors":    switch$(strlwr(%b))
                            {
                                case "hat":    messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
                                                messageClient(%client, '', "<tab:140>\c6/Use the /buy command with the same options if you want to buy these.");
                                                messageClient(%client, '', "\c7--------------------------------------------------------------------------------");   
                                                messageClient(%client, '', "<tab:140>\c3All available\c6" SPC %b SPC "\c3options:");
                                                messageClient(%client, '', "<tab:140>\c6Cowboy - 10 points");
                                                messageClient(%client, '', "<tab:140>\c6Fancy - 10 points");
                                                messageClient(%client, '', "<tab:140>\c6Hoodie - 10 points");
                                                messageClient(%client, '', "<tab:140>\c6Cap - 10 points");
                                                messageClient(%client, '', "\c7--------------------------------------------------------------------------------");                       

                                default:    messageClient(%client, '', "\c7--------------------------------------------------------------------------------");        
                                            messageClient(%client, '', "<tab:140>\c3All available\c6" SPC %a SPC "\c3options:");
                                            messageClient(%client, '', "<tab:140>\c6Hats");
                                            messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
                            }

        case "effects": messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
                        messageClient(%client, '', "<tab:140>\c6/Use the /buy command with the same options if you want to buy these.");
                        messageClient(%client, '', "\c7--------------------------------------------------------------------------------");  
                        messageClient(%client, '', "<tab:140>\c3All available\c6" SPC %a SPC "\c3options:");                                                
                        messageClient(%client, '', "<tab:140>\c6Hearts - 15 points");
                        messageClient(%client, '', "<tab:140>\c6Stinky - 15 points");                                                
                        messageClient(%client, '', "<tab:140>\c6Heals - 15 points");
                        messageClient(%client, '', "<tab:140>\c6Electric - 25 points");
                        messageClient(%client, '', "<tab:140>\c6Fire - 25 points");
                        messageClient(%client, '', "<tab:140>\c6Sparkles - 25 points");                                                
                        messageClient(%client, '', "\c7--------------------------------------------------------------------------------");                                  

        case "titles":   messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
                        messageClient(%client, '', "<tab:140>\c6Titles are like a clan tag you can set, with different fonts as well");
                        messageClient(%client, '', "<tab:140>\c6All fonts are 30 points");
                        messageClient(%client, '', "<tab:140>\c6Useage: \c3/buy title \"insert text here\" fontname");
                        messageClient(%client, '', "\c7--------------------------------------------------------------------------------");                        
                        messageClient(%client, '', "<tab:140>\c3All available\c6 font \c3options:");                        
                        messageClient(%client, '', "<tab:140>\c6IMPACT");
                        messageClient(%client, '', "<tab:140>\c6Arial");
                        messageClient(%client, '', "<tab:140>\c6Elephant");
                        messageClient(%client, '', "<tab:140>\c6Constantia");
                        messageClient(%client, '', "<tab:140>\c6Perpetua");

        case "bitmaps":   messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
                        messageClient(%client, '', "<tab:140>\c6Similar to titles, bitmaps are another form of customization to put in your name. Use the /buy option with similar options when you made your choice");               
                        messageClient(%client, '', "\c7--------------------------------------------------------------------------------");                        
                        messageClient(%client, '', "<tab:140>\c3All available\c6" SPC %a SPC "\c3options:");                        
                        messageClient(%client, '', "<tab:140>\c6Fire - 25 points");
                        messageClient(%client, '', "<tab:140>\c6Sword - 25 points");
                        messageClient(%client, '', "<tab:140>\c6Shield - 25 points");
                        messageClient(%client, '', "<tab:140>\c6Candle - 25 points");
                        messageClient(%client, '', "<tab:140>\c6Book - 25 points");
                        messageClient(%client, '', "<tab:140>\c6Gem - 25 points");

        default:    messageClient(%client, '', "<tab:140>\c6/Want to buy some stuff? Here's a quick set of options.");
                    messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
	                messageClient(%client, '', "<tab:140>\c6/shop killers");
                    messageClient(%client, '', "<tab:140>\c6/shop survivors");
                    messageClient(%client, '', "<tab:140>\c6/shop titles");
                    messageClient(%client, '', "<tab:140>\c6/shop bitmaps");                    
                    messageClient(%client, '', "<tab:140>\c6/shop effects");
	                messageClient(%client, '', "\c7--------------------------------------------------------------------------------");
    }
}