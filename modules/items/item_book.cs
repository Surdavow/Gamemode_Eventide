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

datablock StaticShapeData(brickBookStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/book.dts";
	placementSound = "book_place_sound";
};

function bookImage::onUnmount(%this,%obj,%slot)
{    
    Parent::onUnmount(%this,%obj,%slot);
    %obj.playAudio(1,"bookconceal_sound");
    %obj.playthread(2,"plant");
}

function bookImage::onMount(%this,%obj,%slot)
{    
    Parent::onMount(%this,%obj,%slot);
    %obj.playAudio(1,"bookequip_sound");
    %obj.playthread(1,"armReadyBoth");
    %obj.playthread(2,"plant");
}