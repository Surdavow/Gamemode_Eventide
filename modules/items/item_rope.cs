datablock ItemData(Rope)
{
	//category = "Item"; //Weapon?
	//className = "Item"; //Weapon?
	
	shapeFile = "./models/Rope.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = false;
	
	uiName = "Rope";
	iconName = "";
	doColorShift = false;
	
	image = RopeImage;
	canDrop = true;
};

datablock ShapeBaseImageData(RopeImage)
{
	shapeFile = "./models/Rope.dts";
	emap = false;
	mountPoint = 0;
	offset = "0.0 0.0 0.0";
	eyeOffset = 0;
	rotation = eulerToMatrix("0 0 0");
	
	className = "WeaponImage";
	item = Rope;
	
	armReady = true;
	doColorShift = false;
	
	stateName[0]					= "Activate";
	stateSound[0]					= weaponSwitchSound;
	stateTimeoutValue[0]			= 0.15;
	stateSequence[0]				= "Ready";
	stateTransitionOnTimeout[0]		= "Ready";

	stateName[1]					= "Ready";
	stateAllowImageChange[1]		= true; //false?
	stateScript[1]					= "onReady";
	stateTransitionOnTriggerDown[1]	= "Use";
	
	stateName[2]					= "Use";
	stateScript[2]					= "onUse";
	stateTransitionOnTriggerUp[2]	= "Ready";
};

function RopeImage::onMount(%this,%obj,%slot)
{
	%pl = %obj;
	%pl.playThread(0,"armReady");
}

function RopeImage::onUnMount(%this,%obj,%slot)
{
	%pl = %obj;
	%pl.playThread(0,"root");
}