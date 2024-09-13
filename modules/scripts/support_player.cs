$Player::NoItemsPickup::NoAddItem = 0;

package Eventide_Player
{

	function ShapeBase::pickup(%obj, %item)
	{
        if(%obj.getClassName() $= "Player" && %obj.getDataBlock().getName() $= "PlayerSkinwalker")
		return;                   
        
        Parent::pickup(%obj, %item);
    }

    function Player::addItem(%player, %image, %client)
	{
		if(!$Player::PlayerSkinwalker::NoAddItem)
		Parent::addItem(%player, %image, %client);		
    }

	function Observer::onTrigger (%this, %obj, %trigger, %state)
	{		
		if(isObject(%client = %obj.getControllingClient ()) && isObject(%player = %client.Player)) 
		if(%player.stunned) return;
		
		Parent::onTrigger (%this, %obj, %trigger, %state);
	}

	function Armor::onImpact(%this, %obj, %col, %vec, %force)
	{
		Parent::onImpact(%this, %obj, %col, %vec, %force);

		if(%obj.isInvisible) return;

		if(%force < 40) serverPlay3D("impact_medium" @ getRandom(1,3) @ "_sound",%obj.getPosition());
		else serverPlay3D("impact_hard" @ getRandom(1,3) @ "_sound",%obj.getPosition());

		%oScale = getWord(%obj.getScale(),2);
		%forcescale = %force/25 * %oscale;
		%obj.spawnExplosion(pushBroomProjectile,%forcescale SPC %forcescale SPC %forcescale);

		if(%obj.getState() !$= "Dead" && getWord(%vec,2) > %obj.getdataBlock().minImpactSpeed)
        serverPlay3D("impact_fall_sound",%obj.getPosition());		
	}

	function player::setDamageFlash(%obj,%value)
	{
		if(!isObject(%obj.getControllingClient())) return;
		
		if(!%obj.ShireBlind && %value > 0.2) %value = 0.2;
		Parent::setDamageFlash(%obj,%value);
	}

	function Armor::onNewDatablock(%this,%obj)
	{		
		Parent::onNewDatablock(%this,%obj);

		%this.GazeLoop(%obj);
		%this.schedule(500,KillerCheck,%obj);

		if(%this != %obj.getDatablock() && %this.maxTools != %obj.client.lastMaxTools)
		{
			%obj.client.lastMaxTools = %this.maxTools;
			commandToClient(%obj.client,'PlayGui_CreateToolHud',%this.maxTools);
		}
		
		if(isObject(%obj.client) && %this.maxTools != %obj.client.lastMaxTools)
		{
			%obj.client.lastMaxTools = %this.maxTools;
			commandToClient(%obj.client,'PlayGui_CreateToolHud',%this.maxTools);
			for(%i=0;%i<%this.maxTools;%i++)
			{
				if(isObject(%obj.tool[%i])) messageClient(%obj.client,'MsgItemPickup',"",%i,%obj.tool[%i].getID(),1);
				else messageClient(%obj.client,'MsgItemPickup',"",%i,0,1);
			}
		}

		if(%this.rideable || isEventPending(%obj.peggstep)) return;
		%obj.touchcolor = "";
		%obj.surface = parseSoundFromNumber($Pref::Server::PF::defaultStep, %obj);
		%obj.isSlow = 0;
		%obj.peggstep = schedule(50,0,PeggFootsteps,%obj);
	}

	function Armor::onDisabled(%this, %obj, %state)
	{
        Parent::onDisabled(%this, %obj, %state);

		for(%i = 0; %i < %obj.getMountedObjectCount(); %i++) 
		if(isObject(%obj.getMountedObject(%i)) && (%obj.getMountedObject(%i).getDataBlock().className $= "PlayerData")) %obj.getMountedObject(%i).delete();		

        if(isObject(%client = %obj.client) && isObject(%client.EventidemusicEmitter))
		{
			%client.EventidemusicEmitter.delete();        
        	%client.musicChaseLevel = 0;		
		}
		
		if(isObject(%killer = %obj.killer))
		{
			%killer.ChokeAmount = 0;
			%killer.victim = 0;
			%killer.playthread(3,"activate2");
			%obj.dismount();
			%obj.setVelocity(vectorscale(vectorAdd(%killer.getForwardVector(),"0 0 0.25"),15));		
		}
    }

	function Armor::onRemove(%this,%obj)
	{
		Parent::onRemove(%this,%obj);

		for(%i = 0; %i < %obj.getMountedObjectCount(); %i++) 
		if(isObject(%obj.getMountedObject(%i)) && (%obj.getMountedObject(%i).getDataBlock().className $= "PlayerData")) %obj.getMountedObject(%i).delete();
	}

	//Not parenting, I made an overhaul of this function so it might cause compatibility issues...
	function Player::ActivateStuff (%player)
	{
		Parent::ActivateStuff(%player);
		
		if(isObject(%player) && %player.getState() !$= "Dead" && isFunction(%player.getDataBlock().getName(),onActivate)) 
		return %player.getDataBlock().onActivate(%player);
	}	
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Player)) deactivatePackage(Eventide_Player);
activatePackage(Eventide_Player);