if(isObject(EventideSetTitleFontMenu)) EventideSetTitleFontMenu.delete();
new ScriptObject(EventideSetTitleFontMenu)
{
    isCenterprintMenu = 1;
    menuName = "Title - Set Font";

    menuOption[0] = "Impact";
    menuFunction[0] = "EventideSetCustomFont";
    menuOption[1] = "Comic Sans MS";
    menuFunction[1] = "EventideSetCustomFont";
    menuOption[2] = "Arial";
    menuFunction[2] = "EventideSetCustomFont";    
    menuOption[3] = "Constantia";
    menuFunction[3] = "EventideSetCustomFont";
    menuOption[4] = "Georgia";
    menuFunction[4] = "EventideSetCustomFont";
    menuOption[5] = "Courier New";
    menuFunction[5] = "EventideSetCustomFont";
    menuOption[6] = "Exit";
    menuFunction[6] = "exitCenterprintMenu";

    justify = "<just:right>";
    menuOptionCount = 7;
};

function EventideSetCustomFont(%client,%menu,%option)
{    
    switch(%option)
    {
        case 0: %client.customtitlefont = "Impact";
        case 1: %client.customtitlefont = "Comic Sans MS";
        case 2: %client.customtitlefont = "Arial";
        case 3: %client.customtitlefont = "Constantia";
        case 4: %client.customtitlefont = "Georgia";
        case 4: %client.customtitlefont = "Courier New";
    }
}