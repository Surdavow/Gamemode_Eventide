////////////////////////////////////////////////////////
//weapon image//////////////////////////////////////////
////////////////////////////////////////////////////////

datablock ShapeBaseImageData(meleeTantoImage)
{
   // Basic Item properties
   shapeFile = "./tanto.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0;
   rotation = eulerToMatrix( "0 0 0" );

   raycastWeaponRange = 3;
   raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
   				$TypeMasks::PlayerObjectType |	//AI/Players
   				$TypeMasks::StaticObjectType |	//Static Shapes
   				$TypeMasks::TerrainObjectType |	//Terrain
   				$TypeMasks::VehicleObjectType;	//Vehicles
   raycastExplosionProjectile = hammerProjectile;
   raycastExplosionSound = "";
   raycastExplosionPlayerSound = meleeTantoHit2Sound;
   raycastExplosionBrickSound = meleeTantoHit1Sound;
   raycastDirectDamage = 40;
   raycastDirectDamageType = $DamageType::sword;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = meleeTantoItem;
   ammo = " ";
   projectile = meleeTantoProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = false;
   colorShiftColor = meleeTantoItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
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
	stateSound[3]			= meleeTantoSwingSound;
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

function meleeTantoImage::onAim(%this,%obj,%slot)
{
	%obj.playThread(1,ArmReadyRight);
}

function meleeTantoImage::onDrop(%this,%obj,%slot)
{
	%obj.playThread(1,root);
}

function meleeTantoImage::onFire(%this,%obj,%slot)
{
	Parent::onFire(%this, %obj, %slot);
	%obj.playThread(2,shiftto);
	%obj.playThread(0,shiftleft);
}

function meleeTantoImage::onFireDown(%this,%obj,%slot)
{
	%obj.playThread(2,shiftaway);
}

function meleecaneImage::onRaycastDamage(%this,%obj,%slot,%col,%pos,%normal,%shotVec,%crit)
{
	parent::onRaycastDamage(%this,%obj,%slot,%col,%pos,%normal,%shotVec,%crit);
}