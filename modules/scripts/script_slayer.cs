//
// Documentation for making a custom Slayer gamemode: https://bitbucket.org/Greek2me/slayer/wiki/Modding#rst-header-creating-a-custom-game-mode
//

//
// Slayer gamemode template.
//

$Eventide_SlayerTemplateObject = new ScriptGroup(Slayer_GameModeTemplateSG)
{
    // Game mode settings
    className = "Slayer_Eventide";
    uiName = "Eventide";
    useTeams = true;
    isEventide = true;
    // disable_teamCreation = true;

    // Team settings
    teams_minTeams = 2;
    // teams_maxTeams = 4;

    // Default minigame settings
    default_title = "Eventide";
    default_isDefaultMinigame = true;
    default_botDamage = true;
    default_selfDamage = false;
    default_vehicleDamage = true;
    default_fallingDamage = true;
    default_teams_allySameColors = true;
    default_playerDatablock = nameToID("EventidePlayer");
    default_lives = 1;

    default_enableBuilding = false;
    default_enablePainting = false;
    default_enableWand = false;

    default_bKills_enable = false;
    default_clearStats = false;
    default_timeBetweenRounds = 10;

    default_startEquip0 = 0;
    default_startEquip1 = 0;
    default_startEquip2 = 0;

    // Locked minigame settings
    locked_hostName = "Muna";
    locked_weaponDamage = true;
    locked_chat_enableTeamChat = true;

    //Teams need to be shuffled manually.
    //I'm planning to make a queueing system where everyone get's to play the killer consistently.
    locked_teams_lock = true;
    locked_teams_shuffleTeams = false;

    // Teams
    new ScriptObject()
    {
        // Prevent team from being deleted
        disable_delete = true;
        disable_edit = false;

        default_syncLoadout = true;
        default_uniform = 0;
        default_maxPlayers = -1;

        // Locked team settings
        locked_name = "Survivors";
        locked_lives = 1;
        locked_color = Slayer_Support::getClosestPaintColor("1 1 1 1"); //White.
        locked_sort = false;
        locked_lock = true;
    };

    new ScriptObject()
    {
        // Prevent team from being deleted
        disable_delete = true;
        disable_edit = false;

        default_syncLoadout = true;
        default_uniform = 0;
        default_playerDatablock = NameToID("PlayerNoJet");
        default_maxPlayers = 1;

        // Locked team settings
        locked_name = "Hunters";
        locked_lives = 1;
        locked_color = Slayer_Support::getClosestPaintColor("0 0 0 1"); //Black.
    };
};
//Store players who have yet to play Hunter.
$Eventide_HunterQueue = new SimSet(Eventide_HunterQueue);

function Slayer_Eventide::preMinigameReset(%this, %callingClient)
{
    clearCurrentKillers();
}

function Slayer_Eventide::onMinigameReset(%this, %callingClient)
{
    %mini = %this.minigame;
    %survivorTeam = %mini.teams.getTeamFromName("Survivors");
	%hunterTeam = %mini.teams.getTeamFromName("Hunters");

    if(isObject(Eventide_MinigameGroup)) 
    {
        Eventide_MinigameGroup.delete();
    }

    //Reset the ritual circle.
    if(isObject($EventideRitualBrick)) 
    {
        $EventideRitualBrick.ritualsPlaced = 0;
        $EventideRitualBrick.gemcount = 0;
        $EventideRitualBrick.candlecount = 0;
    }

    //Sprinkle items around the map.
    %mini.randomizeEventideItems(true);

    if(isObject($Eventide_HunterQueue))
    {
        $Eventide_HunterQueue.delete();
    }
    $Eventide_HunterQueue = new SimSet(Eventide_HunterQueue);

    %queuedPlayers = $Eventide_HunterQueue.getCount();
    %numberOfMinigameMembers = %mini.numMembers["GameConnection"];

    //Decide who will be the Hunter, based on who hasn't already played the hunter.
    %selectedHunter = "";
    if(%queuedPlayers == 0)
    {
        //The queue is empty. Fill it up with everyone, then pick a random person to play hunter.
        %selectedHunter = %mini.member["GameConnection", getRandom(0, (%numberOfMinigameMembers - 1))];
        for(%i = 0; %i < %numberOfMinigameMembers; %i++)
        {
            %client = %mini.member["GameConnection", %i];
            if(%client.getID() != %selectedHunter.getID())
            {
                $Eventide_HunterQueue.add(%client);
            }

            //Since we're already here, mark each person as unescaped and stop their chase music initally.
            %client.escaped = false;
            %client.StopChase();
        }
    }
    else
    {
        //Some people still haven't played Hunter, decide which one gets to go next.
        %selectedHunter = $Eventide_HunterQueue.getObject(getRandom(0, ($Eventide_HunterQueue.getCount() - 1)));
        $Eventide_HunterQueue.remove(%selectedHunter);

        //Mark everyone as unescaped and stop their chase music initially.
        for(%i = 0; %i < %numberOfMinigameMembers; %i++)
        {
            %client = %mini.member["GameConnection", %i];
            %client.escaped = false;
            %client.StopChase();
        }
    }

    //Now, assign everyone a new team - Hunter if they got chosen, Survivor if not.
    %oldNotify = %mini.teams_notifyMemberChanges;
	%mini.teams_notifyMemberChanges = false; //Prevent chat message spam as people get assigned to their new team.
    for(%i = 0; %i < %numberOfMinigameMembers; %i++)
    {
        %client = %mini.member["GameConnection", %i];
        if(%client.getID() == %selectedHunter.getID())
        {
            %hunterTeam.addMember(%client, "", true);
        }
        else
        {
            %survivorTeam.addMember(%client, "", true);
        }
    }
	%mini.teams_notifyMemberChanges = %oldNotify;

    //Assign the new survivors a class.
    for(%i = 0; %i < getWordCount($Eventide_SurvivorClasses); %i++)
    {
        %mini.survivorClass[getWord($Eventide_SurvivorClasses, %i)] = 0;
    }
    %mini.assignSurvivorClasses();

    //Inform everyone that local chat is enabled.
    if($Pref::Server::ChatMod::lchatEnabled)
    {
        %mini.bottomprintall("<font:impact:25>\c3Local chat is enabled, find a radio to broadcast to other survivors!",4);
    }

    //Play a decorative chime to let people know a new round has started.
    %mini.playSound("round_start_sound");
}

function Slayer_Eventide::onRoundEnd(%this, %winner, %nameList)
{
    %mini = %this.minigame;

    //Disable local chat at the end of the round, let everyone banter at the end.
    if($MinigameLocalChat)
    {
        $MinigameLocalChat = false;
        %mini.bottomprintall("<font:impact:20>\c3Local chat disabled.",4);
    }
    
    //If the killers won, call an event on them saying they did. If not, call an event for that as well.
    %killers = getCurrentKillers();
    for(%i = 0; %i < %killers.getCount(); %i++)
    {
        %killer = %killers.getObject(%i);
        %killerTeam = %killer.getTeam();
        if(isObject(%killer.player) && isObject(%killer))
        {
            %won = (%winner.getClassName() $= "Slayer_TeamSO" && %winner.getId() == %killerTeam.getId()) || (%winner.getClassName() $= "GameConnection" && %winner.getId() == %killer.getId());
            %killerDataBlock = %killer.getDataBlock();
            %killerDatablock.onRoundEnd(%killer, %won);
        }
    }

    //Play a chime to let people know the round has ended.
    %mini.playSound("round_end_sound");
}

//The minigame is ending, do some cleanup.
function Slayer_Eventide::onGameModeEnd(%this)
{
    %mini = %this.minigame;

    //Disable local chat.
    if($MinigameLocalChat)
    {
        $MinigameLocalChat = false;
        %mini.bottomprintall("<font:impact:30>\c3Local chat disabled",4);
    }

    for(%i = 0; %i < %mini.numMembers; %i++)
    {
        %client = %mini.member[%i];
        if(isObject(%client) && isObject(%client.EventidemusicEmitter)) 
        {
            %client.EventidemusicEmitter.delete();
        }
        %client.escaped = false;
    }

    if(isObject(Eventide_MinigameGroup))
    {
        Eventide_MinigameGroup.delete();
    }

    %mini.randomizeEventideItems(false);
}