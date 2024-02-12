datablock ItemData(DCamera)
{
	//category = "Item"; //Weapon?
	//className = "Item"; //Weapon?
	
	shapeFile = "./models/DCamera/DCamera.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = false;
	
	uiName = "DCamera";
	iconName = "";
	doColorShift = false;
	
	image = DCameraImage;
	canDrop = true;
};

datablock ShapeBaseImageData(DCameraImage)
{
	shapeFile = "./models/DCamera/DCamera.dts";
	emap = false;
	mountPoint = 0;
	offset = "0.0 0.0 0.0";
	eyeOffset = 0;
	rotation = eulerToMatrix("0 0 0");
	
	className = "WeaponImage";
	item = DCamera;
	
	armReady = true;
	doColorShift = false;
	
	stateName[0]					= "Activate";
	//stateSound[0]					= weaponSwitchSound;
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

function DCameraImage::onMount(%this,%obj,%slot)
{
	%pl = %obj;
	%pl.playThread(0,"armReady");
}

function DCameraImage::onUnMount(%this,%obj,%slot)
{
	%pl = %obj;
	%pl.playThread(0,"root");
}