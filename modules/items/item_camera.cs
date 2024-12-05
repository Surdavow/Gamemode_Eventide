datablock ItemData(DCamera)
{
	shapeFile = "./models/DCamera/DCamera.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = false;
	
	uiName = "Camera";
	iconName = "./icons/icon_camera";
	doColorShift = false;
	
	image = DCameraImage;
	canDrop = true;
};

datablock ExplosionData(DCameraExplosion)
{
   explosionShape = "Add-Ons/Weapon_Rocket_Launcher/explosionSphere1.dts";
   lifeTimeMS = 150;

   explosionScale = "1 1 1";

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 1;
   lightStartColor = "1 1 1";
   lightEndColor = "0 0 0";

   //impulse
   impulseRadius = 0;
   impulseForce = 0;

   damageRadius = 0;
   radiusDamage = 0;

   uiName = "DCamera";
};

datablock ShapeBaseImageData(DCameraImage)
{
	shapeFile = "./models/DCamera/DCamera.dts";
	emap = false;
	mountPoint = 0;
	offset = "-0.1 0.0 0.0";
	eyeOffset = 0;
	rotation = eulerToMatrix("0 0 0");
	
	className = "WeaponImage";
	item = DCamera;
	
	armReady = true;
	doColorShift = false;
	
	stateName[0]                     = "Equip";
    stateTimeoutValue[0]             = 0;
    stateTransitionOnTriggerDown[0] = "Initiate";
    stateSound[0]					= weaponSwitchSound;

    stateName[1]                = "Initiate";    
    stateTimeoutValue[1]        = 0;
    stateTransitionOnTimeout[1] = "Wait";
    stateScript[1]              = "onInitiate";

    stateName[2]                = "Wait";    
    stateTimeoutValue[2]        = 0.1;
    stateTransitionOnTimeout[2] = "Detonate";

    stateName[3]                = "Detonate";    
	stateExplosion[3]			= DCameraExplosion;
    stateTimeoutValue[3]        = 0;
    stateScript[3]              = "onDetonate";
};

function DCameraImage::onInitiate(%this, %obj, %slot)
{
    %obj.playthread(2,"shiftRight");
}

function DCameraImage::onDetonate(%this, %obj, %slot)
{
	serverPlay3D("camera_flash_sound",%obj.getPosition());

    %obj.cameraLightBot = new Player() { datablock = "emptyPlayer";};
    %obj.mountobject(%obj.cameraLightBot,0);
    %obj.light = new fxLight() { datablock = "brightLight"; };
    %obj.light.attachtoObject(%obj.cameraLightBot);

    %obj.light.schedule(100,delete);
    %obj.cameraLightBot.schedule(150,delete);

    if(isObject(%obj.client))
    {
        for(%i = 0; %i <= %obj.getDataBlock().maxTools; %i++)
	    if(%obj.tool[%i] $= %this.item.getID()) %itemslot = %i;
        %obj.tool[%itemslot] = 0;
        messageClient(%obj.client,'MsgItemPickup','',%itemslot,0);
    }
    if(isObject(%obj.getMountedImage(%this.mountPoint))) %obj.unmountImage(%this.mountPoint);     

    for(%i = 0; %i < clientgroup.getCount(); %i++)//Can't use container radius search anymore :(
    {        
        if(isObject(%nearbyplayer = clientgroup.getObject(%i).player))
        {    
            if(%nearbyplayer.getClassName() !$= "Player" || VectorDist(%nearbyplayer.getPosition(), %obj.getPosition()) > 15) 
            continue;

            if(%nearbyplayer.getDataBlock().isKiller) %nearbyplayer.setwhiteout(4);
            else %nearbyplayer.setwhiteout(0.375);
        }
    }      
}

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

