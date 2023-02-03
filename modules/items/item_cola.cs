datablock ItemData(blockoColaItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./models/blockoColaItem.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Blocko Cola";
	iconName = "./models/1x2Icon.png";
	doColorShift = true;
	colorShiftColor = "0.76 0.07 0.05 1";

	 // Dynamic properties defined by the scripts
	image = blockoColaImage;
	canDrop = true;
};

datablock ShapeBaseImageData(blockoColaImage)
{
   // Basic Item properties
   shapeFile = "./models/blockoCola.dts";
   emap = true;
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = "0 0 0";
   correctMuzzleVector = false;
   className = "WeaponImage";
   item = blockoColaItem;
   ammo = " ";
   projectile = "";
   projectileType = "";

   melee = false;
   armReady = true;

   doColorShift = true;
   colorShiftColor = blockoColaItem.colorShiftColor;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateAllowImageChange[1] = true;
};