function adjustObjectScopeOnClient(%object, %client, %scopeOn)
{
    if(%scopeOn)
    {
        %object.scopeToClient(%client);
    }
    else
    {
        %object.clearScopeToClient(%client);
    }
}

function adjustObjectScopeToAll(%object, %scopeOn, %exclusions)
{
    //Resort to a default value if the user didn't provide one.
    if(%scopeOn != true && %scopeOn != false)
    {
        %scopeOn = false;
    }

    if(isObject(%exclusions))
    {
        adjustObjectScopeToAllExclusions(%object, %scopeOn, %exclusions);
        return;
    }

    for(%i = 0; %i < ClientGroup.getCount(); %i++) 	
    {
        %client = ClientGroup.getObject(%i);
        adjustObjectScopeOnClient(%object, %client, %scopeOn);
    }
}

//You can ignore this and just use the above function.
function adjustObjectScopeToAllExclusions(%object, %scopeOn, %exclusions)
{
    for(%i = 0; %i < ClientGroup.getCount(); %i++) 	
    {
        %excluded = false;
        %client = ClientGroup.getObject(%i);

        if(%exclusions.getClassName() $= "SimGroup" || %exclusions.getClassName() $= "SimSet" || %exclusions.getClassName() $= "ScriptGroup")
        {
            //For groups of clients.
            for(%j = 0; %j < %exclusions.getCount(); %j++)
            {
                %excludedClient = %exclusions.getObject(%i);
                if(%client == %excludedClient)
                {
                    %excluded = true;
                    break;
                }
            }
        }
        else if(%exclusions.getClassName() $= "GameConnection" && %client == %exclusions)
        {
            //For single clients.
            %excluded = true;
        }

        if(%excluded)
        {
            adjustObjectScopeOnClient(%object, %client, !%scopeOn); //The selected scope operation is reversed here, as they are the exception.
        }
        else
        {
            adjustObjectScopeOnClient(%object, %client, %scopeOn);
        }
    }
}