$ShopHatList = "CapHatImage FancyHatImage StrawHatImage MaskHatImage TopHatImage";

if(isObject(EventideHatShopMenu)) EventideHatShopMenu.delete();
new ScriptObject(EventideHatShopMenu)
{
    menuName = "Eventide Shop - Hats";
    isCenterprintMenu = true;
    justify = "<just:right>";
    menuOptionCount = getWordCount($ShopHatList)+1;
};

for(%i = 0; %i <= getWordCount($ShopHatList); %i++) 
{    
    EventideHatShopMenu.menuOption[%i] = strreplace(getWord($ShopHatList,%i),"Image","") @ " - 50 Points";
    EventideHatShopMenu.menuFunction[%i] = "BuyInstrument";
}

EventideHatShopMenu.menuOption[getWordCount($ShopHatList)] = "Return";
EventideHatShopMenu.menuFunction[getWordCount($ShopHatList)] = "returnToMainShopMenu";