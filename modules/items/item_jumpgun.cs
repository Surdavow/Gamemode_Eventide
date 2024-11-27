

if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
    if(!$RTB::RTBR_ServerControl_Hook)
    {
        exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
    }

    RTB_registerPref("Admin only", "Jumpgun", "Jumpgun::AdminOnly", "bool", "Tool_Jumpgun", false, 0);
    RTB_registerPref("Scan Length", "Jumpgun", "Jumpgun::Length", "int 0 5000", "Tool_Jumpgun", 2000, 0);
    RTB_registerPref("Scan Length for Admins", "Jumpgun", "Jumpgun::AdminLength", "int 0 5000", "Tool_Jumpgun", 2000, 0);
    RTB_registerPref("Cooldown", "Jumpgun", "Jumpgun::Cooldown", "int 0 1000000", "Tool_Jumpgun", 500, 0);
    RTB_registerPref("Cooldown for Admins", "Jumpgun", "Jumpgun::CooldownAdmin", "int 0 1000000", "Tool_Jumpgun", 500, 0);
}
else
{
    $Jumpgun::AdminOnly = 0;
    $Jumpgun::Length = 20;
    $Jumpgun::AdminLength = 20;
    $Jumpgun::Cooldown = 3000;
    $Jumpgun::CooldownAdmin = 3000;
}

datablock ItemData(jumpgunItem)
{
 	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/Dark.dts";
	rotate = false;
	mass = 0.2;
	density = 0.1;
	elasticity = 0.8;
	friction = 0.2;
	emap = true;

	uiName = "Shadowport";
	iconName = "./Icon_Darkness";
	doColorShift = false;
	colorShiftColor = "0 0 0 1";

	image = DarkMImage;
	canDrop = true;
};

datablock ShapeBaseImageData(jumpgunImage)
{
    shapeFile = jumpgunItem.shapeFile;
    emap = true;
    mountPoint = 0;
    offset = "0 0 0";
    eyeOffset = 0;
    rotation = eulerToMatrix( "0 0 0" ); 
    correctMuzzleVector = false;
    className = "WeaponImage";
    item = jumpgunItem;
    ammo = " ";
    projectile = "";
    projectileType = Projectile;
    melee = false;
    armReady = true;
    doColorShift = true;
    colorShiftColor = "0.3 0.3 0.3 1.000";

    stateName[0]			= "Activate";
    stateTimeoutValue[0]		= 0.1;
    stateTransitionOnTimeout[0]		= "Ready";

    stateName[1]			= "Ready";
    stateTransitionOnTriggerDown[1]	= "Fire";
    stateAllowImageChange[1]		= true;
    stateSequence[1]			= "Ready";
	stateEmitter[1]			= DarkAmbientEmitter;
	stateEmitterTime[1]            = 600;

    stateName[2]			= "Fire";
    stateTransitionOnTimeout[2]		= "Reload";
    stateTimeoutValue[2]		= 0.1;
    stateFire[2]			= true;
    stateAllowImageChange[2]		= false;
    stateScript[2]			= "onFire";
    stateWaitForTimeout[2]		= true;

    stateName[3]			= "Reload";
    stateSequence[3]			= "Reload";
    stateTransitionOnTriggerUp[3]	= "Ready";
    stateSequence[3]			= "Ready";
};

datablock ParticleData(jumpgunJumpExplosionParticle:SpawnExplosionParticle1)
{
    lifetimeMS=2000;
    constantacceleration=-0.9;
    gravityCoefficient = -0.1;
    colors[0]="0.200000 0.200000 0.200000 0.496063";    
    colors[1]="0.180000 0.180000 0.180000 0.496063";
    colors[2]="0.140000 0.140000 0.140000 0.496063";
    colors[3]="0.100000 0.100000 0.100000 0.1";
    sizes[3]=0.1;
};

datablock ParticleEmitterData(jumpgunJumpExplosionEmitter:SpawnExplosionEmitter1)
{
    particles=jumpgunJumpExplosionParticle;
    lifetimeMS = 40;
};

datablock ExplosionData(jumpgunJumpExplosion:SpawnExplosion)
{
    Emitter[0]=jumpgunJumpExplosionEmitter;
};

datablock ProjectileData(jumpgunJumpProjectile:spawnProjectile)
{
    Explosion=jumpgunJumpExplosion;
};

function jumpgunImage::onfire(%this, %obj, %slot)
{
    if(!%obj.jumpgun_canUse())
      return;
    
    %dir = vectorNormalize(%obj.geteyevector());
    %pos = %obj.getEyePoint();
    %end = vectorAdd(%pos, vectorScale(%dir, %obj.jumpgun_getLength()));
    %types = $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType |
             $TypeMasks::StaticShapeObjectType | $TypeMasks::TerrainObjectType;
    %result = containerRaycast(%pos, %end, %types);
    if(!%result)
      return;

    %pos = getwords(%result, 1, 3);
    %normal = getwords(%result, 4, 6);
    %newpos=vectoradd(%pos,vectorscale(%normal,3));

//New code: Ugly but precise
    %box = %obj.getWorldBox();
    %size = vectorScale(vectorSub(getWords(%box, 3, 5), getWords(%box, 0, 2)), 0.30);
    %center = %obj.getWorldBoxCenter();
    %dif = vectorScale(vectorSub(%center, %obj.getPosition()), 0.30);
    %newpos = vectoradd(%newpos,"0 0 -1");
    
    %types = $TypeMasks::FxBrickAlwaysObjectType |
             $TypeMasks::InteriorObjectType      |
             $TypeMasks::StaticShapeObjectType   |
             $TypeMasks::TerrainObjectType;

    initContainerBoxSearch(vectorAdd(%newpos, %dif), %size, %types);
    
    while((%next = containerSearchNext()))
    {
        %class = %next.getClassName();

        if(%class $= "fxDTSBrick")
        {
            if(%next.isColliding())
              return;
            else
              continue;
        }
        %found = 1;
    }

    if(%found)
    {
        //Found something that needs a more detailed check.
        //Unfortunately, only raycasts check geometry, and non-bricks
        //Often have space within their bounding boxes

        //This is ugly, but hopefully it works. If only there was an
        //initShapeBoxSearch() or something like it, that used the
        //geometry of interiors, terrains, and static shapes in it's
        //search.
        %maxX = getWord(%size, 0)/2;
        %maxY = getWord(%size, 1)/2;
        %maxZ = getWord(%size, 2)/2;
        %minX = -%maxX;
        %minY = -%maxY;
        %minZ = -%maxZ;
        %center = vectorAdd(%newpos, %dif);
        %minX += getWord(%center, 0);
        %maxX += getWord(%center, 0);
        %minY += getWord(%center, 1);
        %maxY += getWord(%center, 1);
        %minZ += getWord(%center, 2);
        %maxZ += getWord(%center, 2);
        %types = $TypeMasks::InteriorObjectType    |
                 $TypeMasks::StaticShapeObjectType |
                 $TypeMasks::TerrainObjectType;
        if(0)//I hope you *never* need this code. For debugging things like the player bounds box.
        {
            scanEmote(%minX SPC %minY SPC %minZ);
            scanEmote(%maxX SPC %minY SPC %minZ);
            scanEmote(%minX SPC %maxY SPC %minZ);
            scanEmote(%maxX SPC %maxY SPC %minZ);
            scanEmote(%minX SPC %minY SPC %maxZ);
            scanEmote(%maxX SPC %minY SPC %maxZ);
            scanEmote(%minX SPC %maxY SPC %maxZ);
            scanEmote(%maxX SPC %maxY SPC %maxZ);
        }
        for(%x = %minX; %x < %maxX; %x += 0.2)
        {
            for(%y = %minY; %y < %maxY; %y += 0.2)
            {
                %start = %x SPC %y SPC %minZ;
                %end = %x SPC %y SPC %maxZ;
                if(containerRaycast(%start, %end, %types))
                  return;
            }
        }
    }


    jumpgunEmote(%obj);
    %obj.setvelocity(vectorsub(%obj.getvelocity(), vectorscale(vectornormalize(vectorsub(%obj.getposition(), %pos)), 10)));
    %obj.settransform(%newpos);
    jumpgunEmote(%obj);

    %obj.Jumpgun_lastJump = getSimTime();

    return;

//old code: Concise but you could teleport half into the terrain. Ugly but fully functional wins, unfortunately.

    initcontainerradiussearch(%newpos, 1 , $TypeMasks::FxBrickObjectType);
    if(!isobject(containerSearchNext()))
    {
        jumpgunEmote(%obj);
        %obj.setvelocity(vectorsub(%obj.getvelocity(),vectorscale(vectornormalize(vectorsub(%obj.getposition(),%pos)),10)));
        %obj.settransform(vectoradd(%newpos,"0 0 -1"));
        jumpgunEmote(%obj);
    }
}

function jumpgunEmote(%player)
{
    new projectile()
    {
        datablock = jumpgunJumpProjectile;
        initialposition = %player.getEyePoint();
        initialvelocity = "0 0 0";
    };
}

function Player::Jumpgun_canUse(%this)
{
    %client = %this.client;
    if(!isobject(%client))
      return false;

    %admin = %client.isAdmin || %client.isSuperAdmin;

    if($Jumpgun::AdminOnly && !%admin)
      return false;

    if(%this.Jumpgun_lastJump)
    {
        %time = getSimTime() - %this.Jumpgun_lastJump;
        if(%admin)
        {
            if(%time < $Jumpgun::CooldownAdmin)
              return false;
        }
        else
        {
            if(%time < $Jumpgun::Cooldown)
              return false;
        }
    }

    return true;
}

function Player::Jumpgun_getLength(%this)
{
    %client = %this.client;
    if(!isobject(%client))
      return 0;

    if(%client.isAdmin || %client.isSuperAdmin)
      return $Jumpgun::AdminLength;

    return $Jumpgun::Length;
}

//function scanEmote(%position)
//{
//    new particleEmitterNode()
//    {
//        datablock=GenericEmitterNode;
//        position=%position;
//        emitter = LaserEmitterA;
//        velocity = 0;
//        pointPlacement = true;
//    };
//}
