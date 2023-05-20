if(isObject(EventideSetTitleBitmapMenu)) EventideSetTitleBitmapMenu.delete();
new ScriptObject(EventideSetTitleBitmapMenu)
{
    isCenterprintMenu = 1;
    menuName = "Title - Set Bitmap";

    menuOption[0] = "Arrow";
    menuFunction[0] = "EventideSetCustomBitmap";
    menuOption[1] = "Gun";
    menuFunction[1] = "EventideSetCustomBitmap";
    menuOption[2] = "Skull";
    menuFunction[2] = "EventideSetCustomBitmap";
    menuOption[3] = "Blue Ribbon";
    menuFunction[3] = "EventideSetCustomBitmap";
    menuOption[4] = "Car";
    menuFunction[4] = "EventideSetCustomBitmap";
    menuOption[5] = "Bomb";
    menuFunction[5] = "EventideSetCustomBitmap";
    menuOption[6] = "Trophy";
    menuFunction[6] = "EventideSetCustomBitmap";
    menuOption[7] = "Star";
    menuFunction[7] = "EventideSetCustomBitmap";
    menuOption[8] = "Exit";
    menuFunction[8] = "exitCenterprintMenu";

    justify = "<just:right>";
    menuOptionCount = 9;
};

function EventideSetCustomBitmap(%client,%menu,%option)
{    
    switch(%option)
    {
        case 0: %client.customtitlebitmap = "<bitmap:add-ons/weapon_bow/CI_arrow.png>";
        case 1: %client.customtitlebitmap = "<bitmap:add-ons/weapon_gun/CI_gun.png>";
        case 2: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/skull.png>";
        case 3: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/blueRibbon.png>";
        case 4: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/car.png>";
        case 5: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/bomb.png>";
        case 6: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/trophy.png>";
        case 7: %client.customtitlebitmap = "<bitmap:base/client/ui/CI/star.png>";
    }
}