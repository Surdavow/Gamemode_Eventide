datablock TSShapeConstructor(AnglerDTS) {
	baseShape = "./models/angler.dts";
	sequence0 = "./models/angler.dsq";
	sequence1 = "./models/angler_melee.dsq";
};

datablock PlayerData(PlayerAngler : PlayerRenowned) 
{
	uiName = "Angler Player";
	shapeFile = AnglerDTS.baseShape;

	rechargeRate = 0.33;
	maxTools = 0;
	maxWeapons = 0;
	jumpForce = 0;
	maxForwardSpeed = 5.6;
	maxBackwardSpeed = 3.2;
	maxSideSpeed = 4.8;
	//+0% Speed
	boundingBox = "4.5 4.5 9.5";
	crouchBoundingBox = "4.5 4.5 3.6";

	// Weapon: Hook
	hitprojectile = KillerRoughHitProjectile;
	hitobscureprojectile = KillerMacheteClankProjectile;
	meleetrailskin = "ragged";
	meleetrailoffset = "0.3 1.4 0.7"; 	
	meleetrailangle1 = "0 90 0";
	meleetrailangle2 = "0 -90 0";
	meleetrailscale = "4 4 2";
	
	rightclickicon = "color_meathook";
	leftclickicon = "color_melee";
	rightclickspecialicon = "";
	leftclickspecialicon = "";
	
	killerlight = "NoFlareBLight";	

	killerChaseLvl1Music = "musicData_Eventide_AnglerNear";
	killerChaseLvl2Music = "musicData_Eventide_AnglerChase";

	killeridlesound = "angler_idle";
	killeridlesoundamount = 7;

	killerchasesound = "angler_chase";
	killerchasesoundamount = 3;	

	killermeleesound = "angler_melee";
	killermeleesoundamount = 3;
	
	killerweaponsound = "angler_weapon";
	killerweaponsoundamount = 5;

	killermeleehitsound = "skullwolf_hit";
	killermeleehitsoundamount = 3;	
};

datablock StaticShapeData(AnglerHookRope)
{
	shapeFile = "./models/hookrope.dts";
	isHookRope = true;
};

datablock ProjectileData(AnglerHookProjectile)
{
	projectileShapeName = "./models/anglerhookproj.dts";
	directDamage = 0;
	directDamageType = $DamageType::DarkM;

	impactImpulse = 10;
	verticalImpulse = 25;
	explosion = "";
	particleEmitter = "";
	sound = "angler_hookCast_sound";

	muzzleVelocity = 70;
	velInheritFactor = 0;

	armingDelay = 0;
	lifetime = 500;
	fadeDelay = 1000;
	bounceElasticity = 0.5;
	bounceFriction = 0.5;
	isBallistic = false;

	hasLight = false;
	lightRadius = 1;
	lightColor = "0 0 0";
	gravityMod 	= true;

	uiName = "";
};

datablock ShapeBaseImageData(AnglerHookImage)
{
   shapeFile = "./models/anglerhook.dts";
   emap = true;

   mountPoint = 1;
   offset = "0 0 0";
   eyeOffset = 0;
   rotation = "0 0 0";

   correctMuzzleVector = true;

   className = "WeaponImage";

   item = "";
   ammo = " ";
   projectile = "AnglerHookProjectile";
   projectileType = Projectile;

   melee = false;
   armReady = false;

   doColorShift = false;
   colorShiftColor = "0.5 0.5 0.5 1";

	stateName[0]               = "Wait";
	stateTimeoutValue[0]       = 1;
	stateEmitterTime[0]        = 5000;
	stateEmitterTime[0]        = 5;
	stateTransitionOnTimeout[0]= "Wait";
    stateSound[0]               = "angler_chainReturn_sound";
};

function PlayerAngler::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);

	%this.idlesounds(%obj);	
	%obj.mountImage("AnglerHookImage",1);
	%obj.schedule(1, setEnergyLevel, 0);
	%obj.setScale("1.2 1.2 1.2");
	%obj.unHideNode("ALL");
	KillerSpawnMessage(%obj);
}

function PlayerAngler::EventideAppearance(%this,%obj,%client)
{	
	%skinColor = "0.16 0.27 0.43 1";
	%obj.setNodeColor("chest",%skinColor);
	%obj.setNodeColor("pants",%skinColor);
	%obj.setNodeColor("Lhand",%skinColor);
	%obj.setNodeColor("Rhand",%skinColor);
	%obj.setNodeColor("Larm",%skinColor);
	%obj.setNodeColor("Rarm",%skinColor);
	%obj.setNodeColor("LShoe",%skinColor);
	%obj.setNodeColor("RShoe",%skinColor);
	%obj.setNodeColor("HeadSkin",%skinColor);
}

function PlayerAngler::onImpact(%this, %obj, %col, %vec, %force)
{
	if(%obj.getState() !$= "Dead") 
	{				
		%zvector = getWord(%vec,2);
		if(%zvector > %this.minImpactSpeed) %obj.playthread(3,"land");
	}
	
	Parent::onImpact(%this, %obj, %col, %vec, %force);	
}

function PlayerAngler::onTrigger(%this, %obj, %trig, %press) 
{		
	if(%press) switch(%trig)
	{
		case 0: if(%obj.getEnergyLevel() >= 25) %obj.KillerMelee(%this,4);				
				return;
			
		case 4: if(isObject(%obj.getMountedImage(1)) && %obj.getEnergyLevel() >= %this.maxEnergy/1)
				{
					%p = new projectile()
					{
						dataBlock = "AnglerHookProjectile";
						initialVelocity = vectorScale(%obj.getEyeVector(),50);
						initialPosition = %obj.getEyePoint();
						sourceObject = %obj;
						client = %obj.client;
					};
					MissionCleanup.add(%p);
					%obj.unmountImage(1);
					%obj.playthread(2,"leftrecoil");
					%obj.setEnergyLevel(20);

					if(isObject(%obj.hookrope)) %obj.hookrope.delete();
					else
					{
						%hookrope = new StaticShape()
						{
							dataBlock = AnglerHookRope;
							source = %obj;
							end = %p;
						};	
						%obj.hookrope = %hookrope;
						%p.hookrope = %hookrope;
					}								
				}
	}
	Parent::onTrigger(%this, %obj, %trig, %press);	
}

function PlayerAngler::onPeggFootstep(%this,%obj)
{
	serverplay3d("angler_walking" @ getRandom(1,8) @ "_sound", %obj.getHackPosition());
}

function PlayerAngler::onDisabled(%this, %obj, %state) //makes bots have death sound and animation and runs the required bot hole command
{
	Parent::onDisabled(%this, %obj, %state);
}

function PlayerAngler::idlesounds(%this,%obj)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead" || %obj.getdataBlock() !$= %this) return;
	
	%pos = %obj.getPosition();
	%radius = 100;
	%searchMasks = $TypeMasks::PlayerObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);

	while((%targetid = containerSearchNext()) != 0 )
	{
		if(%targetid == %obj) continue;
		%line = vectorNormalize(vectorSub(%targetid.getWorldBoxCenter(),%obj.getWorldBoxCenter()));
		%dot = vectorDot(%obj.getEyeVector(), %line);
		%obscure = containerRayCast(%obj.getEyePoint(),%targetid.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
		if(%dot > 0.55 && !isObject(%obscure) && minigameCanDamage(%obj,%targetid) == 1) %detectedvictims++;
	}

	if(!%obj.isInvisible)
	{
		if(%detectedvictims) %obj.playaudio(0,"angler_Close" @ getRandom(0,2) @ "_sound");
		else %obj.playaudio(0,"angler_Amb" @ getRandom(0,6) @ "_sound");	
	}
	
	%obj.playthread(3,"plant");	
	cancel(%obj.idlesoundsched);
	%obj.idlesoundsched = %this.schedule(getRandom(4000,6000),idlesounds,%obj);
}

function AnglerHookRope::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	%obj.setNodeColor("ALL","0.5 0.5 0.5 1");
	%this.onHookLoop(%obj);
	MissionCleanup.add(%obj);
}

function AnglerHookRope::onHookLoop(%this,%obj)//General function to pull victims closer
{		
	if(!isObject(%obj) || (!isObject(%source = %obj.source) || %source.getState() $= "Dead") || !isObject(%end = %obj.end))//Check if the source (killer) and hook are still existing objects and that the killer is not dead
	{
		if(isObject(%obj)) %obj.delete();
		return;
	}

	if((%end.getClassName() $= "Player" || %end.getClassName() $= "AIPlayer") && %end.getState() $= "Dead" || %end.getdataBlock().isDowned)//Check if the end attachment is a player and that it isn't dead, return and delete the object if this doesnt pass
	{
		if(isObject(%obj)) %obj.delete();
		return;
	}

	if(%end.getType() & $TypeMasks::ProjectileObjectType) %endpos = %end.getPosition();//Should only be a projectile when the rope is currently launched
	else if(%end.getType() & $TypeMasks::PlayerObjectType)
	{
		if(vectorDist(%end.getposition(),%source.getposition()) > 2.5)//Adjust the end's velocity to move to the source
		{ 
			if(getWord(%end.getVelocity(), 2) <= 0.75 && !%obj.hitMusic) %end.playthread(1,"activate2");

			%DisSub = vectorSub(%end.getPosition(),%source.getposition());
			%DistanceNormal = vectorNormalize(%DisSub);

			if(getWord(%end.getvelocity(),2) != 0) %force = 15;
			else %force = 10;
			%newvelocity = vectorscale(%DistanceNormal,-%force);

			if(vectorDist(%end.gethackposition(),%source.gethackposition()) > 5)
			%zinfluence = getWord(%end.getVelocity(), 2) + getWord(%newvelocity, 2)/10;
			else %zinfluence = getWord(%end.getVelocity(), 2);

			%end.setVelocity(getWords(%newvelocity, 0, 1) SPC %zinfluence);

			if(%end.lastchokecough+getrandom(250,500) < getsimtime())//Originally part of the L4B Smoker, just some sounds
			{			
				%source.playaudio(0,"angler_melee" @ getRandom(0,2) @ "_sound");
				%source.playthread(2,"leftrecoil");
				%source.playthread(3,"jump");
				%end.lastchokecough = getsimtime();

				if(getWord(%end.getVelocity(), 2) >= 0.75)//If victim is being lifted up for too long, this function will eventually ebgin to damage the victim
				{					
					if(%source.lastdamage+getRandom(500,100) < getsimtime())
					{
						%source.ChokeUpCount++;				
						%source.playthread(3,"plant");
						%source.playthread(2,"Shiftup");
						%source.lastdamage = getsimtime();
					}

					if(%source.ChokeUpCount > 5)
					{
						%end.playaudio(0,"norm_cough" @ getrandom(1,3) @ "_sound");
						%end.playthread(2,"plant");								
						%end.damage(%source, %end.getposition(), %end.getdataBlock().maxDamage/12.5, $DamageType::sourceConstrict);		
					}
				}
			}			
		}
		%endpos = vectorSub(%end.getmuzzlePoint(2),"0 0 0.2");
	} 

	//Adjust the rope stringe
	%head = %source.getmuzzlePoint(1);
	%vector = vectorNormalize(vectorSub(%endpos,%head));
	%relative = "0 1 0";
	%xyz = vectorNormalize(vectorCross(%relative,%vector));
	%u = mACos(vectorDot(%relative,%vector)) * -1;
	%obj.setTransform(vectorScale(vectorAdd(vectorAdd(%head,"0 0 0.5"),%endpos),0.5) SPC %xyz SPC %u);
	%obj.setScale(0.5 SPC vectorDist(%head,%endpos) * 2 SPC 0.5);

	%obj.HookLoop = %this.schedule(33,onHookLoop,%obj);
}

function AnglerHookRope::onRemove(%this,%obj)
{		
	if(isObject(%source = %obj.source)) %source.mountImage("AnglerHookImage",1);
	if(isObject(%end = %obj.end)) %end.ChokeUpCount = 0;
}

function AnglerHookProjectile::onCollision(%this,%proj,%col,%fade,%pos,%normal)
{
	%obj = %proj.sourceObject;
	if(!isObject(%obj.hookrope)) return;
	
	if((%col.getType() & $TypeMasks::PlayerObjectType) && minigameCanDamage(%obj,%col) == 1)
	{
		if(%col.getdataBlock().isDowned) return;
		
		%col.dismount();
		%obj.hookrope.end = %col;
		%col.playaudio(3,"angler_hookCatch_sound");
		%col.damage(%obj, %pos, 10, $DamageType::Default);
		%obj.hookrope.schedule(1750,"delete");
		return;
	}
	else %obj.hookrope.delete();

	Parent::onCollision(%this,%proj,%col,%fade,%pos,%normal);
}