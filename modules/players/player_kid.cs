//
// Datablock.
//

datablock PlayerData(PlayerKid : PlayerRenowned) 
{
	uiName = "Kid Player";
	
	hitprojectile = KillerRoughHitProjectile;
	hitobscureprojectile = "";
	meleetrailskin = "ragged";

	killerChaseLvl1Music = "musicData_Eventide_KidNear";
	killerChaseLvl2Music = "musicData_Eventide_KidChase";

	killeridlesound = "";
	killeridlesoundamount = 1;

	killerchasesound = "";
	killerchasesoundamount = 1;

	killermeleesound = "";
	killermeleesoundamount = 1;
	
	killernearsound = "kid_looking";
	killernearsoundamount = 7;

    killertauntsound = "kid_kill";
    killertauntsoundamount = 5;

	killerfoundvictimsound = "kid_foundvictim";
	killerfoundvictimsoundamount = 8;

    killerlostvictimsound = "kid_lostvictim";
	killerlostvictimsoundamount = 5;

    killerattackedsound = "kid_attacked";
	killerattackedsoundamount = 4;

	killerpainsound = "kid_pain";
	killerpainsoundamount = 6;
	
	killerweaponsound = "shire_weapon";
	killerweaponsoundamount = 5;	

	killermeleehitsound = "banhammer";
	killermeleehitsoundamount = 3;

	killerwinsound = "kid_win";
    killerwinsoundamount = 2;

    killerlosesound = "kid_lose";
    killerlosesoundamount = 3;
	
	killerlight = "NoFlareGLight";

	rightclickicon = "color_dash";
	leftclickicon = "color_melee";	

	rechargeRate = 0.3;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 7.35;
	maxBackwardSpeed = 4.2;
	maxSideSpeed = 6.3;
	jumpForce = 0;
	
	gazeTickRate = 50;
};

function PlayerKid::onRemove(%this, %obj)
{
    if(isObject(%obj.incapsAchieved))
    {
        %obj.incapsAchieved.delete();
    }
    if(isObject(%obj.threatsReceived))
    {
        %obj.threatsReceived.delete();
    }
    parent::onRemove(%this, %obj);
}

//
// Appearance and initialization.
//

function PlayerKid::onNewDatablock(%this,%obj)
{
	//Face system functionality.
	%obj.createFaceConfig($Eventide_FacePacks["kid"]);
	%obj.faceConfig.setFaceAttribute("Attack", "length", 500);
	%obj.faceConfig.setFaceAttribute("Pain", "length", 1000);

	//Everything else.
	Parent::onNewDatablock(%this, %obj);
	%obj.setScale("1 1 1");
	%obj.mountImage("kidsHammerImage", $RightHandSlot);
	%obj.gazeTickRate = %this.gazeTickRate;
	%obj.isTeleportReady = false;
	%obj.KidGaze();
}

function PlayerKid::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");	
	%obj.unhideNode("pants");
	%obj.unhideNode("headskin");
	%obj.unhideNode("larm");
	%obj.unhideNode("rarm");
	%obj.unhideNode("rshoe");
	%obj.unhideNode("lshoe");
	%obj.unhideNode("lhand");
	%obj.unhideNode("rhand");
	%obj.unhideNode("chest");
	%obj.unhideNode("scouthat");
	%obj.unhideNode("quiver");
	%obj.unhideNode("plume");

	%skinColor = "1 0.88 0.61 1";
	%shirtColor = "0 0.14 0.33 1";
	%pantsColor = "0.2 0.2 0.2 1";
	%shoeColor = "0.33 0.22 0.12 1";

	%obj.setDecalName("worm-sweater");
	%obj.setNodeColor("rarm",%shirtColor);
	%obj.setNodeColor("larm",%shirtColor);
	%obj.setNodeColor("chest",%shirtColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor("rshoe",%shoeColor);
	%obj.setNodeColor("lshoe",%shoeColor);
	%obj.setNodeColor("scouthat",%shirtColor);
	%obj.setNodeColor("quiver",%shoeColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	%obj.unhideNode("chest_blood_front");
	%obj.unhideNode("Lhand_blood");
	%obj.unhideNode("Rhand_blood");
	%obj.unhideNode("lshoe_blood");
	%obj.unhideNode("rshoe_blood");
	
	//Set blood colors.
	%obj.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("lhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("rhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_back", "0.7 0 0 1");
}

//
// Trap.
//

datablock StaticShapeData(PlayerKidTrap)
{
	shapeFile = "base/data/shapes/empty.dts";
	tickRate = 100;
};

function PlayerKidTrap::tick(%this, %obj)
{
	%killer = %obj.killer;

	if(!isObject(%obj))
	{
		//The tripmine no longer exists, stop.
		return;
	}
	else if(!isObject(%killer) || %killer.getState() $= "Dead")
	{
		//No killer? No point. Delete yourself now.
		%obj.delete();
		return;
	}
	else if(!%killer.isTeleportReady)
	{
		//If the Kid is not ready to teleport, don't bother checking if he can.
		%this.schedule(%obj.tickRate, "tick", %obj);
		return;
	}

	//First, check if a player is within range and the trap can be detonated.
	%currentPosition = %obj.getPosition();
    %maximumDistance = 1.5; //3 studs.
	%obstructions = ($TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType);

    initContainerRadiusSearch(%currentPosition, %maximumDistance, $TypeMasks::PlayerObjectType);
    while(%foundPlayer = ContainerSearchNext())
	{
		%playerDatablock = %foundPlayer.getDatablock();
		if(%foundPlayer.isKiller | %playerDatablock.isKiller)
		{
			//If the found player is a killer (probably us) ignore them.
			continue;
		}
		else if(%playerDatablock.isDowned)
        {
            //Victim is downed, skip.
            continue;
        }
        else if(ContainerRayCast(%foundPlayer.getPosition(), %currentPosition, %obstructions))
        {
            //The trap and victim are phyiscally blocked, skip.
            continue;
        }

		//Play an audio que.
		serverPlay3D("kid_power" @ getRandom(1, 3) @ "_sound", %obj.getPosition());

		//The found player is a valid target, activate the trap.
		%killer.setTransform(%obj.getPosition());
		%killer.teleportEffect();
		%obj.delete();

		return;
	}

	%this.schedule(%obj.tickRate, "tick", %obj);
}

function PlayerKidTrap::onAdd(%this, %obj)
{
	%this.schedule(%obj.tickRate, "tick", %obj);
}

function PlayerKidTrap::onRemove(%this, %obj)
{
	if(isObject(%obj.trapEmitter))
	{
		%obj.trapEmitter.delete();
	}
}

function Player::createTrap(%obj, %pos)
{
	if(isObject(%obj.kidTrap))
	{
		%obj.kidTrap.delete();
	}

	%obj.kidTrap = new StaticShape()
	{
		datablock = PlayerKidTrap;
		killer = %obj;
		timePlaced = getSimTime();
		tickRate = PlayerKidTrap.tickRate;
	};
	%obj.kidTrap.setTransform(%pos);

	%obj.kidTrap.trapEmitter = new ParticleEmitterNode()
	{
		datablock = TenthEmitterNode;
		emitter = KidBinaryEmitter0;
	};
	%obj.kidTrap.trapEmitter.setTransform(%obj.kidTrap.getTransform());
	serverPlay3D("kid_trapspawn" @ getRandom(1, 3) @ "_sound", %obj.kidTrap.getPosition());
}

//
// Special ability, throwing a lightning bolt.
//

datablock ShapeBaseImageData(PlayerKidTrapPrepareImage)
{
	shapeFile = "Add-Ons/Gamemode_Eventide/modules/items/models/emptyWeapon.dts";
	emap = true;

	mountPoint = $LeftHandSlot;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = eulerToMatrix( "0 0 0" );
	armReady = true;

	class = "ItemData";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.15;
	stateTransitionOnTimeout[0]       = "Ready";
	stateEmitter[0] = KidBinaryEmitter0;
	stateEmitterTime[0] = 600;

	stateName[1] = "Ready";
	stateAllowImageChange[1] = true;
	stateSequence[1] = "Ready";
};

datablock ProjectileData(PlayerKidTrapProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";
	particleEmitter = KidBinaryEmitter0;
	explosion = KidBinaryExplosion;
	explodeOnDeath = true;

	brickExplosionRadius = 0;
	brickExplosionImpact = 0;             //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;             
	brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground (should always be >= brickExplosionMaxVolume)

	collideWithPlayers = false;

	sound = "kid_trapfly_sound";

	muzzleVelocity      = 65;
	velInheritFactor    = 1.0;
	gravityMod = 1.0;

	armingDelay         = 0;
	lifetime            = 30000;
	fadeDelay           = 29500;
	bounceElasticity    = 0.99;
	bounceFriction      = 0.00;
	isBallistic         = true;

	useEmitterColors = true;
	hasLight    = true;
	lightRadius = 1.0;
	lightColor  = "0 1 0.5";

	uiName = "Kid's Binary Wave"; 
};

function PlayerKidTrapProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity)
{
	parent::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity);
	
	%killer = %obj.sourceObject;
	if(isObject(%killer) && %killer.getState() !$= "Dead")
	{
		%killer.createTrap(%pos);
	}
}

function PlayerKid::onTrigger(%this, %obj, %trig, %press) 
{		
	Parent::onTrigger(%this, %obj, %trig, %press);

	if(%press)
	{
		if(!%trig && %obj.getEnergyLevel() >= 25 && !%obj.isPreparingTrap)
		{
			//Melee attack.
			%this.killerMelee(%obj, 4.5);
			%obj.faceConfigShowFace("Attack");
		}
		else if(%trig == 4 && %obj.getEnergyLevel() == %this.maxEnergy)
		{
			//Arm a glitch wave in the killer's left hand. When space is unpressed, it will be thrown to lay a trap.
			%obj.isPreparingTrap = true;
			%obj.playThread(1, armReadyLeft);
			%obj.mountImage("PlayerKidTrapPrepareImage", $LeftHandSlot);
			%obj.playAudio(1, "kid_powerready_sound");
		}
	}
	else
	{
		if(%trig == 4 && %obj.isPreparingTrap)
		{
			%obj.isPreparingTrap = false;
			%obj.setEnergyLevel(0);
			if(isObject(%obj.getMountedImage($LeftHandSlot)))
			{
				%obj.unmountImage($LeftHandSlot);
			}
			%obj.playThread(1, root);

			//Throw a projectile from the killer's left hand. When it hits something, a trap will be placed there.
			%obj.playAudio(1, "kid_trapthrow_sound");
			new Projectile()
			{
				dataBlock = PlayerKidTrapProjectile;
				originPoint = %obj.getPosition();
				initialPosition = %obj.getMuzzlePoint($LeftHandSlot);
				initialVelocity = VectorScale(%obj.getMuzzleVector($LeftHandSlot), PlayerKidTrapProjectile.muzzleVelocity);
				sourceObject = %obj;
			};
		}
	}
}

package Eventide_Kid
{
	function serverCmdLight(%client)
    {
		%player = %client.player;
		%playerDatablock = %player.getDataBlock();
        if(isObject(%player) && %playerDatablock.getName() $= "PlayerKid")
		{
			%player.isTeleportReady = !%player.isTeleportReady; //Toggle the variable.
			%playerDatablock.killerGUI(%player, %client); //Update the GUI immediately.
			
			//Display a message to the Kid, telling him if his trap is on or off.
			if(%player.isTeleportReady)
			{
				%client.centerPrint("\c6Your trap is set to \c2ON\c6.", 5);
			}
			else
			{
				%client.centerPrint("\c6Your trap is set to \c0OFF\c6.", 5);
			}
		}
    }
};
if(isPackage(Eventide_Kid))
{
	deactivatePackage(Eventide_kid);
}
activatePackage(Eventide_Kid);

//
// Voice-line handlers.
//

function PlayerKid::onKillerChaseStart(%this, %obj, %chasing)
{
    //Mark kills for the below end-of-chase voice line.
    if(!isObject(%obj.incapsAchieved))
    {
        %obj.incapsAchieved = new SimSet();
    }
    if(!isObject(%obj.threatsReceived))
    {
        %obj.threatsReceived = new SimSet();
    }

    %soundType = %this.killerfoundvictimsound;
    %soundAmount = %this.killerfoundvictimsoundamount;
    if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 10000)))
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerKid::onKillerChase(%this, %obj, %chasing)
{
	if(!%chasing)
    {
        //A victim is nearby but the Kid can't see them yet. Say some quips.
        %soundType = %this.killernearsound;
        %soundAmount = %this.killernearsoundamount;
        if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + getRandom(15000, 25000))))
        {
            %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
            %obj.lastKillerSoundTime = getSimTime();
        }
    }
}

function PlayerKid::onKillerChaseEnd(%this, %obj)
{
	//If Postal Dude doesn't get any kills during a chase, play a voice line marking his dismay.
    if(%obj.incapsAchieved.getCount() == 0)
    {
        %soundType = %this.killerlostvictimsound;
        %soundAmount = %this.killerlostvictimsoundamount;
        if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + getRandom(5000, 15000))))
        {
            %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
            %obj.lastKillerSoundTime = getSimTime();
        }
    }

    //Need to clear the list. Deleting it is simple and safe.
    %obj.incapsAchieved.delete();
}

function PlayerKid::onExitStun(%this, %obj)
{
	%soundType = %this.killerattackedsound;
    %soundAmount = %this.killerattackedsoundamount;
    if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 5000)))
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerKid::onIncapacitateVictim(%this, %obj, %victim, %killed)
{
	//Play a voice-line taunting the victim.
    %soundType = %this.killertauntsound;
    %soundAmount = %this.killertauntsoundamount;
    if(%soundType !$= "") 
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }

    //Mark the kill on a temporary SimSet. Used for a voice-line mechanic in `onKillerChaseEnd`.
    if(!isObject(%obj.incapsAchieved))
    {
        %obj.incapsAchieved = new SimSet();
    }
    if(isObject(%victim.client))
    {
        %obj.incapsAchieved.add(%victim.client);
    }
    else
    {
        //Add in a dummy object for holebot support.
        %obj.incapsAchieved.add(
            new ScriptObject() 
            {
                player = %victim;
                name = %victim.getClassName();
            }
        );
    }

	%victimClient = %victim.client;
	if(%killed && isObject(%obj.client) && isObject(%victimClient))
	{
		%victim.delete();
		MessageAll('MsgAdminForce', '\c3%1\c2 permanently banned \c3%2\c2 (ID: %3) - \c2"%4"', %obj.client.name, %victimClient.name, %victimClient.bl_id, "0wn3d");
	}
}

function PlayerKid::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);

	if(%obj.getState() !$= "Dead") 
	{
		%obj.faceConfigShowFace("Pain");
		%soundType = %this.killerpainsound;
		%soundAmount = %this.killerpainsoundamount;
		if(%soundType !$= "")
		{
			%obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
			%obj.lastKillerSoundTime = getSimTime();
		}
	}
}

function PlayerKid::onDisabled(%this, %obj)
{
	parent::onDisabled(%this, %obj);
	
	%soundType = %this.killerlosesound;
	%soundAmount = %this.killerlosesoundamount;
	if(%soundType !$= "")
	{
		%obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
		%obj.lastKillerSoundTime = getSimTime();
	}
}

function Player::KidGaze(%obj)
{
    if(!isObject(%obj) || %obj.isDisabled())
    {
        return;
    }

    %currentPosition = %obj.getPosition();
    %maximumDistance = $EnvGuiServer::VisibleDistance;

    initContainerRadiusSearch(%currentPosition, %maximumDistance, $TypeMasks::PlayerObjectType);
    while(%foundPlayer = ContainerSearchNext())
    {
        %killerPosition = %obj.getEyePoint();
        %killerDatablock = %obj.getDataBlock();
        %victimPosition = %foundPlayer.getEyePoint();
        %victimDatablock = %foundPlayer.getDataBlock();
        %obstructions = ($TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType);

        if(%foundPlayer.isKiller || %victimDatablock.isKiller)
        {
            //We found ourselves, skip. Future-proofing in case multiple-killer setups become a thing.
            continue;
        }
        else if(%victimDatablock.isDowned)
        {
            //Victim is downed, skip.
            continue;
        }
        else if(ContainerRayCast(%victimPosition, %killerPosition, %obstructions))
        {
            //The killer and victim are phyiscally blocked, skip.
            continue;
        }
        else if(!%obj.isChasing || %foundPlayer.chaseLevel != 2)
        {
			//The victim is not being chased, they are irrelevant here. Skip.
			continue;
		}

		//The victim does not have any items equipped, don't bother with this.
		%victimEquippedItem = %foundPlayer.getMountedImage($RightHandSlot);
		if(!%victimEquippedItem)
		{
			continue;
		}

		//Nowhere better to put this: if the player has a weapon, have Shire play a voice line acknowledging it.
		%alreadyThreatenedKiller = false;
		for(%i = 0; %i < %obj.threatsReceived.getCount(); %i++)
		{
			if(%obj.threatsReceived.getObject(%i).getId() == %foundPlayer.getId())
			{
				%alreadyThreatenedKiller = true;
				break;
			}
		}
		if(%alreadyThreatenedKiller)
		{
			//They already threatened us, skip playing any more voice lines.
			continue;
		}

		//They have an item equipped and it's a weapon, have the Kid react to it.
		if(%victimEquippedItem.isWeapon || %victimEquippedItem.className $= "WeaponImage")
		{
			%soundType = %killerDatablock.killerthreatenedsound;
			%soundAmount = %killerDatablock.killerthreatenedsoundamount;
			if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 5000)))
			{
				%obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
				%obj.lastKillerSoundTime = getSimTime();
				%obj.threatsReceived.add(%foundPlayer); //Ensure the Kid does not acknowledge any further weapons. Less annoying.
			}
		}

		continue;
	}

    %obj.schedule(%obj.gazeTickRate, KidGaze);
}