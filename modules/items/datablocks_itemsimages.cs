datablock ItemData(bookItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/book.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Book";
	iconName = "./icons/icon_book";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = bookImage;
	canDrop = true;
};

datablock ShapeBaseImageData(bookImage)
{
    shapeFile = "./models/book.dts";
    emap = true;

    mountPoint = 0;
    offset = "-0.5 0 0";
    correctMuzzleVector = false;
    eyeOffset = "0 0 0";
    className = "WeaponImage";

    item = bookItem;
    ammo = " ";
    projectile = "";
    projectileType = Projectile;

    melee = true;
    doRetraction = false;
    armReady = false;
    doColorShift = bookItem.doColorShift;
    colorShiftColor = bookItem.colorShiftColor;

    stateName[0]                     = "Activate";
};

datablock ItemData(gem1Item)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/gem1.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Gem Variant 1";
	iconName = "./icons/icon_gem";
	doColorShift = true;
	colorShiftColor = "1 0.5 0.5 1";

	image = gem1Image;
	canDrop = true;
};

datablock ShapeBaseImageData(gem1Image)
{
    shapeFile = "./models/gem1.dts";
    emap = true;

    mountPoint = 0;
    offset = "-0.1 0.125 0";
	offsetrotation = "0 0 0";
    correctMuzzleVector = false;
    eyeOffset = "0 0 0";
    className = "WeaponImage";

    item = gem1Item;
    ammo = " ";
    projectile = "";
    projectileType = Projectile;

    melee = true;
    doRetraction = false;
    armReady = true;
    doColorShift = gem1Item.doColorShift;
    colorShiftColor = gem1Item.colorShiftColor;

    stateName[0]                     = "Activate";
};

datablock ItemData(gem2Item : gem1Item)
{
	shapeFile = "./models/gem2.dts";
	uiName = "Gem Variant 2";
	doColorShift = true;
	colorShiftColor = "0.5 1 0.5 1";
	image = gem2Image;
};

datablock ShapeBaseImageData(gem2Image : gem1Image)
{
    shapeFile = "./models/gem2.dts";
    item = gem2Item;
    doColorShift = gem2Item.doColorShift;
    colorShiftColor = gem2Item.colorShiftColor;	
};

datablock ItemData(gem3Item : gem1Item)
{
	shapeFile = "./models/gem3.dts";
	uiName = "Gem Variant 3";
	doColorShift = true;
	colorShiftColor = "0.5 0.5 1 1";	
	image = gem3Image;
};

datablock ShapeBaseImageData(gem3Image : gem1Image)
{
    shapeFile = "./models/gem3.dts";
    item = gem3Item;
    doColorShift = gem3Item.doColorShift;
    colorShiftColor = gem3Item.colorShiftColor;		
};

datablock ItemData(gem4Item : gem1Item)
{
	shapeFile = "./models/gem4.dts";
	uiName = "Gem Variant 4";
	doColorShift = true;
	colorShiftColor = "0.6 0.25 0.25 1";
	image = gem4Image;
};

datablock ShapeBaseImageData(gem4Image : gem1Image)
{
    shapeFile = "./models/gem4.dts";
    item = gem4Item;
    doColorShift = gem4Item.doColorShift;
    colorShiftColor = gem4Item.colorShiftColor;	
};

datablock ItemData(gem5Item : gem1Item)
{
	shapeFile = "./models/gem5.dts";
	uiName = "Gem Variant 5";
	doColorShift = true;
	colorShiftColor = "0.25 0.25 0.6 1";		
	image = gem5Image;
};

datablock ShapeBaseImageData(gem5Image : gem1Image)
{
    shapeFile = "./models/gem5.dts";
    item = gem5Item;
    doColorShift = gem5Item.doColorShift;
    colorShiftColor = gem5Item.colorShiftColor;		
};

datablock ItemData(candleItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/candle.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Candle";
	iconName = "./icons/icon_candle";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = candleImage;
	canDrop = true;
};

datablock ShapeBaseImageData(candleImage)
{
    shapeFile = "base/data/shapes/empty.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0.0";
    correctMuzzleVector = false;
    eyeOffset = "0 0 0";
    className = "WeaponImage";

    item = candleItem;
    ammo = " ";
    projectile = "";
    projectileType = Projectile;

    melee = true;
    doRetraction = false;
    armReady = true;
    doColorShift = candleItem.doColorShift;
    colorShiftColor = candleItem.colorShiftColor;

    stateName[0]                     = "Activate";
    stateScript[0]                  = "onActivate";
    stateTimeoutValue[0]             = 0.5;
    stateTransitionOnTimeout[0]      = "Ready";

    stateName[1]                     = "Ready";
    stateScript[1]                  = "onReady";
    stateTransitionOnTriggerDown[1]  = "Light";

    stateName[2]			= "Light";
    stateScript[2]                  = "onLight";
    stateTimeoutValue[2]            = 25;
    stateTransitionOnTimeout[2]     = "Extinguish";

    stateName[3]			= "Extinguish";
    stateScript[3]                  = "onExtinguish";
    stateTimeoutValue[3]            = 0;
    stateTransitionOnTimeout[3]     = "Ready";    
};

datablock ItemData(daggerItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/dagger.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Ceremonial Dagger";
	iconName = "./icons/icon_dagger";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = daggerImage;
	canDrop = true;
};

datablock ShapeBaseImageData(daggerImage)
{
    shapeFile = "./models/dagger.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";
    correctMuzzleVector = false;
    eyeOffset = "0 0 0";
    className = "WeaponImage";

    item = daggerItem;
    ammo = " ";
    projectile = "";
    projectileType = Projectile;

    melee = true;
    doRetraction = false;
    armReady = false;
    doColorShift = daggerItem.doColorShift;
    colorShiftColor = daggerItem.colorShiftColor;

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = WeaponSwitchsound;

	stateName[1]                     = "Ready";
	stateScript[1]                  = "onReady";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]					= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.15;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateFire[3]                    = false;
	stateScript[3]                  = "onFire";
	stateSound[3]					= "sworddaggerswing_sound";
	stateTimeoutValue[3]            = 0.1;
	stateEmitter[3]					= "";
	stateEmitterNode[3]             = "muzzlePoint";
	stateEmitterTime[3]             = "0.225";

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "StopFire";

	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Break";
	stateTimeoutValue[5]            = 0.1925;
	stateScript[5]                  = "onStopFire";
	stateEmitter[5]					= "";
	stateEmitterNode[5]             = "muzzlePoint";
	stateEmitterTime[5]             = "0.1";

	stateName[6]                    = "Break";
	stateTransitionOnTimeout[6]     = "Ready";
	stateTimeoutValue[6]            = 0.1;
};

datablock ItemData(CRadioItem)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "./models/radio.dts";
	uiName = "Radio";
	image = CRadioImage;
	canDrop = true;
};

datablock ShapeBaseImageData(CRadioImage)
{
	className = "WeaponImage";
	projectileType = "Projectile";
	projectile = "";
	item = "CRadioItem";
	mountpoint = 0;
	shapefile = CRadioItem.shapeFile;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = "0";
	stateTransitionOnTimeout[0] = "Ready";	
	
	stateName[1] = "Ready";
	stateTimeoutValue[1] = 0;
	stateTransitionOnTriggerDown[1] = "Change";
	
	stateName[2] = "Change";
	stateTransitionOnTimeout[2] = "Check";
	stateScript[2] = "onRadioChange";
	stateTimeoutvalue[2] = 0.3;
	
	stateName[3] = "Check";
	stateSequence[3] = "Check";
	stateTransitionOnTriggerUp[3] = "Ready";
	stateSequence[3] = "Ready";
};

