$Player::NoItemsPickup::NoAddItem = 0;

package Eventide_Player
{
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

	function Armor::onNewDatablock(%this,%obj)
	{		
		Parent::onNewDatablock(%this,%obj);

		%this.GazeLoop(%obj);

		if(%this != %obj.getDatablock() && %this.maxTools != %obj.client.lastMaxTools && isObject(%obj.client))
		{
			%obj.client.lastMaxTools = %this.maxTools;
			commandToClient(%obj.client,'PlayGui_CreateToolHud',%this.maxTools);
			
			for(%i=0;%i<%this.maxTools;%i++)
			{
				if(isObject(%obj.tool[%i])) messageClient(%obj.client,'MsgItemPickup',"",%i,%obj.tool[%i].getID(),1);
				else messageClient(%obj.client,'MsgItemPickup',"",%i,0,1);
			}
		}
	}

	function Armor::onDisabled(%this, %obj, %state)
	{
        Parent::onDisabled(%this, %obj, %state);

		for(%i = 0; %i < %obj.getMountedObjectCount(); %i++) 
		if(isObject(%obj.getMountedObject(%i)) && (%obj.getMountedObject(%i).getDataBlock().className $= "PlayerData")) 
		%obj.getMountedObject(%i).delete();		

        if(isObject(%client = %obj.client) && isObject(%client.EventidemusicEmitter))
		{
			%client.EventidemusicEmitter.delete();        
        	%client.musicChaseLevel = 0;		
		}
    }

	function Armor::onRemove(%this,%obj)
	{
		Parent::onRemove(%this,%obj);

		for(%i = 0; %i < %obj.getMountedObjectCount(); %i++) 
		if(isObject(%obj.getMountedObject(%i)) && (%obj.getMountedObject(%i).getDataBlock().className $= "PlayerData")) 
		%obj.getMountedObject(%i).delete();
	}

	function Player::ActivateStuff(%player)
	{
		if(!isObject(%player) || !%player.getState() !$= "Dead") return;
		Parent::ActivateStuff(%player);
		
		if(isFunction(%player.getDataBlock().getName(),onActivate)) 
		return %player.getDataBlock().onActivate(%player);
	}	
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Player)) deactivatePackage(Eventide_Player);
activatePackage(Eventide_Player);