datablock ShapeBaseImageData(meleeTantoImage)
{
   	// Basic Item properties
   	shapeFile = "./models/tanto.dts";
   	emap = true;
   	mountPoint = 0;
   	offset = "0 0 0";
   	eyeOffset = 0;
   	rotation = eulerToMatrix( "0 0 0" );
   	correctMuzzleVector = true;
   	className = "WeaponImage";
   	item = "";
   	ammo = "";
   	projectile = "";
   	projectileType = Projectile;
   	melee = false;
   	armReady = true;
   	doColorShift = false;

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 1;
	stateTransitionOnTimeout[0]       = "Ready";
	stateScript[0]                  = "onAim";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]                     = "Ready";
	stateTimeoutValue[1]             = 3;
	stateTransitionOnTimeout[1]       = "ReadyDown";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateWaitForTimeout[1]			= false;
	stateAllowImageChange[1]         = true;
	stateSequence[1]	= "Ready";

	stateName[7]                     = "ReadyDown";
	stateSound[7]					= weaponSwitchSound;
	stateTransitionOnTriggerDown[7]  = "AimBeat";
	stateAllowImageChange[7]         = true;
	stateScript[7]                  = "onDrop";
	stateSequence[7]	= "Ready";

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Smoke";
	stateTimeoutValue[2]            = 0.15;
	stateScript[2]                  = "onFireDown";
	stateWaitForTimeout[2]			= true;
   	stateSequence[2]                = "Fire";

	stateName[3] 			= "Smoke";
	stateScript[3]                  = "onFire";
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateTimeoutValue[3]            = 0.15;
	stateSound[3]			= "";
	stateTransitionOnTimeout[3]     = "CoolDown";

   	stateName[5] = "CoolDown";
   	stateTimeoutValue[5]            = 0.9;
	stateTransitionOnTimeout[5]     = "Reload";
   	stateSequence[5]                = "clickDown";

	stateName[4]			= "Reload";
	stateTransitionOnTriggerUp[4]     = "Ready";
	stateSequence[4]	= "TrigDown";

   	stateName[6]   = "NoAmmo";
   	stateTransitionOnAmmo[6] = "Ready";

	stateName[8]                     = "AimBeat";
	stateTimeoutValue[8]             = 0.05;
	stateTransitionOnTimeout[8]       = "Fire";
	stateAllowImageChange[8]         = true;
	stateScript[8]                  = "onAim";
	stateSequence[8]	= "clickDown";
};

datablock ShapeBaseImageData(meleeMacheteImage : meleeTantoImage)
{
   shapeFile = "./models/machete.dts";
   mountPoint = 0;
};

datablock ShapeBaseImageData(meleePuppetMasterDaggerImage : meleeTantoImage)
{
   shapeFile = "./models/puppetmasterdagger.dts";
   mountPoint = 0;
};

datablock ShapeBaseImageData(meleeAxeImage : meleeTantoImage)
{
   shapeFile = "./models/axe.dts";
};

datablock ShapeBaseImageData(meleeAxeLImage : meleeTantoImage)
{
   shapeFile = "./models/axe2.dts";
   mountPoint = 1;
};

datablock ShapeBaseImageData(ShovelImage : meleeTantoImage)
{
   shapeFile = "./models/Shovel.dts";
   mountPoint = 1;
};

function meleeTantoImage::onFire(%this,%obj,%slot)
{
	return;
}

function meleePuppetMasterDaggerImage::onFire(%this,%obj,%slot)
{
	meleeTantoImage::onFire(%this,%obj,%slot);
}

function meleeAxeImage::onFire(%this,%obj,%slot)
{
	meleeTantoImage::onFire(%this,%obj,%slot);
}

function meleeMacheteImage::onFire(%this,%obj,%slot)
{
	meleeTantoImage::onFire(%this,%obj,%slot);
}