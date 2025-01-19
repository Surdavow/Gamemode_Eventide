//
// Documentation for making a custom Slayer gamemode: https://bitbucket.org/Greek2me/slayer/wiki/Modding#rst-header-creating-a-custom-game-mode
//

//
// Slayer gamemode template.
//

new ScriptGroup(Slayer_GameModeTemplateSG)
{
    // Game mode settings
    className = "SlayerEventide";
    uiName = "Eventide";
    useTeams = true;
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

    default_startEquip0 = 0;
    default_startEquip1 = 0;
    default_startEquip2 = 0;

    // Locked minigame settings
    locked_hostName = "Muna";
    locked_weaponDamage = true;
    locked_chat_enableTeamChat = true;

    //Teams need to be shuffled manually.
    //I'm planning to make a queueing system where everyone get's to play the killer consistently.
    locked_teams_autoSort = false;
    locked_teams_lock = true;
    locked_teams_shuffleTeams = false;


    // Teams
    new ScriptObject()
    {
        // Prevent team from being deleted
        disable_delete = true;
        disable_edit = false;

        default_syncLoadout = true;

        // Locked team settings
        locked_name = "Survivors";
        locked_lives = 1;
        locked_color = 0;
        locked_sort = false;
        locked_lock = true;
    };

    new ScriptObject()
    {
        // Prevent team from being deleted
        disable_delete = true;
        disable_edit = false;

        default_syncLoadout = true;
        default_playerDatablock = NameToID("PlayerNoJet");

        // Locked team settings
        locked_name = "Hunters";
        locked_lives = 1;
        locked_color = 1;
        locked_sort = false;
        locked_lock = true;
    };
};