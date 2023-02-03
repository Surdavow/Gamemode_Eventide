datablock ItemData(dollarItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./models/dollarItem.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Dollar";
	iconName = "./models/1x2Icon.png";
	doColorShift = true;
	colorShiftColor = "0.00 0.60 0.27 1";

	 // Dynamic properties defined by the scripts
	image = dollarImage;
	canDrop = true;
};

datablock ShapeBaseImageData(dollarImage)
{
   // Basic Item properties
   shapeFile = "./models/dollar.dts";
   emap = true;
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = "0 0 0";
   correctMuzzleVector = false;
   className = "WeaponImage";
   item = dollarItem;
   ammo = " ";
   projectile = "";
   projectileType = "";

   melee = false;
   armReady = true;

   doColorShift = true;
   colorShiftColor = dollarItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateAllowImageChange[1] = true;
};