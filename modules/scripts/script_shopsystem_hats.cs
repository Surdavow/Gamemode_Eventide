if(isObject(EventideHatShopMenu)) EventideHatShopMenu.delete();
new ScriptObject(EventideHatShopMenu)
{
    isCenterprintMenu = 1;
    menuName = "Eventide Shop - Hats";

    menuOption[0] = "Cap";
    menuFunction[0] = "";
    menuOption[1] = "Fancy";
    menuFunction[1] = "";
    menuOption[2] = "Straw Hat";
    menuFunction[2] = "";    
    menuOption[3] = "Mask";
    menuFunction[3] = "";
    menuOption[4] = "Top";
    menuFunction[4] = "";
    menuOption[5] = "Return";
    menuFunction[5] = "returnToMainShopMenu";

    justify = "<just:right>";
    menuOptionCount = 6;
};