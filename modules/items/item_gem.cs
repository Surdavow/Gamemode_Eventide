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

	staticShape = brickGem1StaticShape;
	isRitual = true;
	isGemRitual = true;

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
	staticShape = brickGem2StaticShape;
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
	staticShape = brickGem3StaticShape;
    doColorShift = gem3Item.doColorShift;
    colorShiftColor = gem3Item.colorShiftColor;		
};

datablock ItemData(gem4Item : gem1Item)
{
	shapeFile = "./models/gem4.dts";
	uiName = "Gem Variant 4";
	doColorShift = true;
	colorShiftColor = "1 0.83 0.51 1";
	image = gem4Image;
};

datablock ShapeBaseImageData(gem4Image : gem1Image)
{
    shapeFile = "./models/gem4.dts";
    item = gem4Item;
	staticShape = brickGem4StaticShape;
    doColorShift = gem4Item.doColorShift;
    colorShiftColor = gem4Item.colorShiftColor;	
};

datablock ItemData(gem5Item : gem1Item)
{
	shapeFile = "./models/gem5.dts";
	uiName = "Gem Variant 5";
	doColorShift = true;
	colorShiftColor = "0.78 0.51 1 1";
	image = gem5Image;
};

datablock ShapeBaseImageData(gem5Image : gem1Image)
{
    shapeFile = "./models/gem5.dts";
    item = gem5Item;
	staticShape = brickgem5StaticShape;
    doColorShift = gem5Item.doColorShift;
    colorShiftColor = gem5Item.colorShiftColor;	
};