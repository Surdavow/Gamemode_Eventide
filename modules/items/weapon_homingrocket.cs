//
// Support functions.
//

//https://blockdoc.block.land/VectorRotate
//Angle in radians.
//Might need this later.
function VectorRotate(%vec, %axis, %angle)
{
    if (vectorLen(%axis) != 1)
    {
        %axis = vectorNormalize(%axis);
    }

    %proj = vectorScale(%axis, vectorDot(%vec, %axis));
    %ortho = vectorSub(%vec, %proj);
    %w = vectorCross(%axis, %ortho);
    %cos = mCos(%angle);
    %sin = mSin(%angle);
    %x1 = %cos / vectorLen(%ortho);
    %x2 = %sin / vectorLen(%w);
    %rotOrtho = vectorScale(vectorAdd(vectorScale(%ortho, %x1), vectorScale(%w, %x2)), vectorLen(%ortho));
    return vectorAdd(%rotOrtho, %proj);
}

function getClosestIntersectionPoint(%viewingPoint, %lineStartPoint, %lineEndPoint)
{
	%line = VectorSub(%lineEndPoint, %lineStartPoint);
	%numberOfSteps = mCeil(VectorLen(%line)); //The longer the line, the more checks for intersections.
	%typemask = ($TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::TerrainObjectType);

	%segmentSize = VectorScale(%line, 1 / %numberOfSteps);
	for(%i = 0; %i < %numberOfSteps; %i++)
	{
		%possibleIntersection = VectorAdd(%lineStartPoint, VectorScale(%segmentSize, %i));
		
		%raycast = containerRayCast(%viewingPoint, %possibleIntersection, %typemask);
		%viewerToIntersectionCollision = getWord(%raycast, 0);

		if(!isObject(%viewerToIntersectionCollision))
		{
			//We didn't hit anything, the intersection is valid.
			return %possibleIntersection;
		}
	}

	return ""; //No viable intersection point.
}

//
// Projectile data.
//

datablock ParticleData(homingRocketTrailParticle : rocketTrailParticle)
{
   colors[0]     = "1 1 1 0.4";
   colors[1]     = "0.8 0 0 0.5";
   colors[2]     = "0.20 0.20 0.20 0.3";
   colors[3]     = "0.0 0.0 0.0 0.0";
   lifetimeMS = 2000;
};
datablock ParticleEmitterData(homingRocketTrailEmitterB : rocketTrailEmitter)
{
   particles = "homingRocketTrailParticle";
};

datablock ProjectileData(homingRocketLauncherProjectile)
{
   projectileShapeName = "Add-Ons/Projectile_GravityRocket/RocketGravityProjectile.dts";
   directDamage        = 100;
   directDamageType = $DamageType::RocketDirect;
   radiusDamageType = $DamageType::RocketRadius;
   impactImpulse	   = 1000;
   verticalImpulse	   = 1000;
   explosion           = rocketExplosion;
   particleEmitter     = homingRocketTrailEmitterB;

   brickExplosionRadius = 3;
   brickExplosionImpact = false;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 30;             
   brickExplosionMaxVolume = 30;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 60;  //max volume of bricks that we can destroy if they aren't connected to the ground (should always be >= brickExplosionMaxVolume)

   sound = errorSound;

   muzzleVelocity      = 30; //Was originally 50.
   velInheritFactor    = 1.0;

   armingDelay         = 00;
   lifetime            = 4000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = true;
   lightRadius = 5.0;
   lightColor  = "1.0 0.1 0.1";
   
   doColorShift = true;
   colorShiftColor = "0.8 0 0";

};

//
// Scripted behavior.
//

package Gamemode_Eventide_Homing_Rocket
{
	function Projectile::onAdd(%obj)
	{
		Parent::onAdd(%obj);
		if(%obj.getDataBlock().getID() != homingRocketLauncherProjectile.getID())
		{
			//Not a homing rocket.
			return;
		}
		if(%obj.completedTrackingDelay)
		{
			//The homing rocket is already tracking, don't waste our time with this logic.
			%obj.schedule(%obj.trackingTickRate, homingRocketTick);
			return;
		}

		%sourceObject = %obj.sourceObject;
		if(!%sourceObject.getDataBlock().isKiller)
		{
			return;
		}
		else if(!%sourceObject.trackingCandidate && !%obj.trackingTarget)
		{
			return;
		}

		%killerDataBlock = %obj.sourceObject.getDatablock();

		//Get homing rocket preferences from the killer's datablock.
		%trackingDelay = (%killerDataBlock.homingRocketTrackingDelay !$= "") ? %killerDataBlock.homingRocketTrackingDelay : 250;
		%trackingTickRate = (%killerDataBlock.homingRocketTickRate !$= "") ? %killerDataBlock.homingRocketTickRate : 75;
		%bounceLimit = (%killerDataBlock.homingRocketBounceLimit !$= "") ? %killerDataBlock.homingRocketBounceLimit : 5;

		//Assign the rocket preferences to the rocket itself, so we don't have to run this logic every tick.
		%obj.trackingDelay = %trackingDelay;
		%obj.trackingTickRate = %trackingTickRate;
		%obj.bounceLimit = %bounceLimit;
		%obj.trackingTarget = %sourceObject.trackingCandidate;

		//Begin rocket tracking.
		%obj.schedule(%trackingDelay, homingRocketTick);
	}
};
activatePackage(Gamemode_Eventide_Homing_Rocket);

function Projectile::homingRocketTick(%this)
{
	%sourceObject = %this.sourceObject;
	%target = %this.trackingTarget;

	//No shooter or no target, let the rocket drift off.
	if(!isObject(%this) || !isObject(%target) || %target.isDisabled() || !isObject(%sourceObject))
	{
		return;
	}

	//Get the direction of the rocket.
	%currentVelocity = %this.getVelocity();

	if(VectorLen(%currentVelocity) == 0)
	{
		//The rocket isn't moving, it probably bugged out. Just make it explode.
		%this.explode();
		return;
	}

	%currentPosition = %this.getPosition();
	if(VectorDist(%currentPosition, %sourceObject.getPosition()) > 64) //1 Torque Units = 2 studs.
	{
		//To help prevent across-the-map sniping with homing rockets, cause the rocket to explode if it ventures too far from it's spawn point.
		%this.explode();
		return;
	}

	%muzzleVelocity = %this.getDataBlock().muzzleVelocity; //Movement speed in Torque Units per second.

	%finalVelocity = "";

	if(%this.pathPoint && VectorDist(%currentPosition, %this.pathPoint) > 1)
	{
		//If the rocket has a previously defined path point and has not yet reached it, go there.
		%vectorTowardsPathPoint = VectorSub(%this.pathPoint, %currentPosition);
		%finalVelocity = VectorScale(VectorNormalize(%vectorTowardsPathPoint), %muzzleVelocity);
	}
	else
	{
		//We either don't have a pathing point or already reached it, let's see if we can make it to the target.
		%this.pathPoint = ""; 

		%targetPosition = %target.getHackPosition();

		//Is the path to the target blocked?
		%typemask = ($TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::TerrainObjectType);
		%raycast = containerRayCast(%currentPosition, %targetPosition, %typemask);
		%targetPathObjectHit = getWord(%raycast, 0);

		//The path directly to the target is blocked, we need to find a way around.
		if(isObject(%targetPathObjectHit))
		{
			%northVector = "0 1 0";
			%southVector = "0 -1 0";
			%eastVector = "1 0 0";
			%westVector = "-1 0 0";
			%upVector = "0 0 1";
			%downVector = "0 0 -1";

			%maximumLineDistance = $EnvGuiServer::VisibleDistance;

			//
			// On each axis (the vectors above,) check if there is a clear path of intersection between it and the rocket. If so, lead the rocket there.
			//

			//Programming this made me hate my own guts.
			if(!%finalVelocity)
			{
				%targetUpPoint = VectorAdd(%targetPosition, VectorScale(%upVector, %maximumLineDistance));
				if(!isObject(getWord(containerRaycast(%targetPosition, %targetUpPoint, %typemask), 0)))
				{
					%upAttempt = getClosestIntersectionPoint(%currentPosition, %targetPosition, %targetUpPoint);
					if(%upAttempt)
					{
						%this.pathPoint = %upAttempt;
						%finalVelocity = VectorScale(VectorNormalize(VectorSub(%upAttempt, %currentPosition)), %muzzleVelocity);
					}
				}
			}

			if(!%finalVelocity)
			{
				%targetNorthPoint = VectorAdd(%targetPosition, VectorScale(%northVector, %maximumLineDistance));
				if(!isObject(getWord(containerRaycast(%targetPosition, %targetNorthPoint, %typemask), 0)))
				{
					%northAttempt = getClosestIntersectionPoint(%currentPosition, %targetPosition, %targetNorthPoint);
					if(%northAttempt)
					{
						%this.pathPoint = %northAttempt;
						%finalVelocity = VectorScale(VectorNormalize(VectorSub(%northAttempt, %currentPosition)), %muzzleVelocity);
					}
				}
			}
			
			if(!%finalVelocity)
			{
				%targetSouthPoint = VectorAdd(%targetPosition, VectorScale(%southVector, %maximumLineDistance));
				if(!isObject(getWord(containerRaycast(%targetPosition, %targetSouthPoint, %typemask), 0)))
				{
					%southAttempt = getClosestIntersectionPoint(%currentPosition, %targetPosition, %targetSouthPoint);
					if(%southAttempt)
					{
						%this.pathPoint = %southAttempt;
						%finalVelocity = VectorScale(VectorNormalize(VectorSub(%southAttempt, %currentPosition)), %muzzleVelocity);
					}
				}
			}

			if(!%finalVelocity)
			{
				%targetEastPoint = VectorAdd(%targetPosition, VectorScale(%eastVector, %maximumLineDistance));
				if(!isObject(getWord(containerRaycast(%targetPosition, %targetEastPoint, %typemask), 0)))
				{
					%eastAttempt = getClosestIntersectionPoint(%currentPosition, %targetPosition, %targetEastPoint);
					if(%eastAttempt)
					{
						%this.pathPoint = %eastAttempt;
						%finalVelocity = VectorScale(VectorNormalize(VectorSub(%eastAttempt, %currentPosition)), %muzzleVelocity);
					}
				}
			}

			if(!%finalVelocity)
			{
				%targetWestPoint = VectorAdd(%targetPosition, VectorScale(%westVector, %maximumLineDistance));
				if(!isObject(getWord(containerRaycast(%targetPosition, %targetWestPoint, %typemask), 0)))
				{
					%westAttempt = getClosestIntersectionPoint(%currentPosition, %targetPosition, %targetWestPoint);
					if(%westAttempt)
					{
						%this.pathPoint = %westAttempt;
						%finalVelocity = VectorScale(VectorNormalize(VectorSub(%westAttempt, %currentPosition)), %muzzleVelocity);
					}
				}
			}

			if(!%finalVelocity)
			{
				%targetDownPoint = VectorAdd(%targetPosition, VectorScale(%downVector, %maximumLineDistance));
				if(!isObject(getWord(containerRaycast(%targetPosition, %targetDownPoint, %typemask), 0)))
				{
					%downAttempt = getClosestIntersectionPoint(%currentPosition, %targetPosition, %targetDownPoint);
					if(%downAttempt)
					{
						%this.pathPoint = %downAttempt;
						%finalVelocity = VectorScale(VectorNormalize(VectorSub(%downAttempt, %currentPosition)), %muzzleVelocity);
					}
				}
			}
		}
	}

	//No obstructions or no alternative path found, just go for the target.
	if(!%finalVelocity || VectorLen(%finalVelocity) == 0)
	{
		%vectorTowardsTarget = VectorNormalize(VectorSub(%targetPosition, %currentPosition));
		%initialVelocity = VectorScale(%vectorTowardsTarget, %muzzleVelocity);
		%velocityAdjustment = VectorSub(%initialVelocity, %currentVelocity);
		%finalVelocity = VectorAdd(%currentVelocity, %velocityAdjustment);
	}

	//Setting the velocity of a rocket isn't possible, so we can do this instead.
	%newRocket = new Projectile()
	{
		dataBlock = %this.dataBlock;
		initialPosition = %currentPosition;
		initialVelocity = %finalVelocity;
		sourceObject = %this.sourceObject;
		client = %this.client;
		sourceSlot = %this.sourceSlot;
		originPoint = %this.originPoint;
		reflectTime = %this.reflectTime;

		//Custom data for rocket tracking, inherited from the original rocket.
		completedTrackingDelay = true;
		trackingDelay = %this.trackingDelay;
		trackingTickRate = %this.trackingTickRate;
		trackingTarget = %this.trackingTarget;
		pathPoint = %this.pathPoint;
		bounceCount = %this.bounceCount;
		bounceLimit = %this.bounceLimit;
	};
	
	//Remove the old rocket to give the illusion of a single rocket.
	if(isObject(%newRocket))
	{
		MissionCleanup.add(%newRocket);
		%newRocket.setScale(%this.getScale());
		%this.delete();
	}
}

//Make the rocket bounce, to compensate for bad/blocked tracking.
function homingRocketLauncherProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%col.getClassName() $= "Player" || %col.getClassName() $= "AIPlayer" || %obj.bounceCount > %obj.bounceLimit)
	{
		%obj.explode();
	}
	else
	{
		%obj.bounceCount++;

		%vel = %obj.getLastImpactVelocity();
		%factor = 1.0;

		%bounceVel = VectorSub(%vel, VectorScale(%normal, VectorDot(%vel, %normal) * 2));
		%bounceVel = VectorScale(%bounceVel, %factor);
		if (VectorLen(%bounceVel) > 200)
		{
			%bounceVel = VectorScale(VectorNormalize(%bounceVel), 200);
		}

		%newRocket = new Projectile()
		{
			dataBlock = %this;
			initialPosition = %pos;
			initialVelocity = %bounceVel;
			sourceObject = %obj.sourceObject;
			client = %obj.client;
			sourceSlot = %obj.sourceSlot;
			originPoint = %obj.originPoint;
			reflectTime = %obj.reflectTime;

			//Custom data for rocket tracking, inherited from the original rocket.
			completedTrackingDelay = false;
			trackingDelay = %obj.trackingDelay;
			trackingTickRate = %obj.trackingTickRate;
			trackingTarget = %obj.trackingTarget;
			pathPoint = %obj.pathPoint;
			bounceCount = %obj.bounceCount;
			bounceLimit = %obj.bounceLimit;
		};
		MissionCleanup.add(%newRocket);
	}
	Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
}

//
// Item data.
//

datablock ItemData(homingRocketLauncherItem : rocketLauncherItem)
{
	shapeFile = "./models/homerocket.dts";
	uiName = "SC Rocket L.";
	image = homingRocketLauncherImage;
};

datablock ShapeBaseImageData(homingRocketLauncherImage : rocketLauncherImage)
{
	projectile = homingRocketLauncherProjectile;
	shapeFile = "./models/homerocket.dts";
	minShotTime = 3000;
	
	stateTimeoutValue[0] = 0.7;
	stateTimeoutValue[5] = 1.5;
};


function homingRocketLauncherImage::onFire(%this, %obj, %slot)
{
	//Debug code.
	// %obj.trackingCandidate = mainHoleBotSet.getObject(0);
	// %obj.isKiller = true;
	// %obj.SCMissleCount = 999;

	if((%obj.lastFireTime + %this.minShotTime) > getSimTime())
	{
		//The rocket launcher needs to cool down, to prevent spam.
		return;
	}	
	else if(%obj.SCMissleCount < 1)
	{
		//Sky Captain ran out of ammo. :(
		return;
	}

	//Play a cute little animation.
	%obj.playThread(3, plant);
		
	%obj.SCMissleCount--;
	%obj.lastFireTime = getSimTime();
	
	//Update the killer's ammo counter.
	%playerDatablock = %obj.getDataBlock();
	if(%playerDatablock.getName() $= "PlayerCaptain")
	{
		%playerDatablock.killerGUI(%obj, %obj.client);
	}

	//Determine the starting velocity of the rocket, factoring in the current velocity of the shooter.
	%projectile = %this.projectile;
	%shooterVelocity = %obj.getVelocity();
	%muzzleVector = %obj.getMuzzleVector(%slot);
	%initialRocketPath = VectorScale(%muzzleVector, %projectile.muzzleVelocity);
	%velocityInheritenceDelta = VectorScale(%shooterVelocity, %projectile.velInheritFactor);
	%finalVelocity = VectorAdd(%initialRocketPath, %velocityInheritenceDelta);

	%rocket = new (%this.projectileType)()
	{
		dataBlock = %this.projectile;
		initialVelocity = %finalVelocity;
		initialPosition = %obj.getMuzzlePoint(%slot);
		sourceObject = %obj;
		client = %obj.client;
		sourceSlot = %slot;

		bounceCount = 0;
		//Signals to the "Projectile::onAdd" package that target tracking needs to be initialized.
		completedTrackingDelay = false;
	};
	MissionCleanup.add(%rocket);

	//Get the vertical size of the player, and scale the rocket accordingly.
	%scaleFactor = getWord(%obj.getScale(), 2);
	%rocket.setScale(%scaleFactor SPC %scaleFactor SPC %scaleFactor);

	return %rocket;
}