$ShopHat[%g = 0] = "CapHatImage";
$ShopHat[%g++] = "FancyHatImage";
$ShopHat[%g++] = "StrawHatImage";
$ShopHat[%g++] = "MaskHatImage";
$ShopHat[%g++] = "TopHatImage";
$ShopHatAmount = %g;

if(isObject(EventideHatShopMenu)) EventideHatShopMenu.delete();
new ScriptObject(EventideHatShopMenu)
{
    menuName = "Eventide Shop - Hats";
    isCenterprintMenu = true;
    justify = "<just:right>";
};

for (%i = 0; %i <= $ShopHatAmount; %i++) 
{    
    EventideHatShopMenu.menuOption[%i] = strreplace($ShopHat[%i],"Image","") @ " - 10 Points";
    EventideHatShopMenu.menuFunction[%i] = "BuyHat";    
}

EventideHatShopMenu.menuOption[$ShopHatAmount+1] = "Return";
EventideHatShopMenu.menuFunction[$ShopHatAmount+1] = "returnToMainShopMenu";
EventideHatShopMenu.menuOptionCount = $ShopHatAmount+2;