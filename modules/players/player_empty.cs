datablock PlayerData(emptyPlayer : playerStandardArmor)
{
	shapeFile = "base/data/shapes/empty.dts";
	boundingBox = "0.01 0.01 0.01";
	crouchboundingBox = "0.01 0.01 0.01";
	isEmptyPlayer = true;
	deathSound = "";
	painSound = "";
	useCustomPainEffects = true;
	mountSound = "";
	jumpSound = "";
	uiName = "";
	className = PlayerData;
	shapeFile = "base/data/shapes/empty.dts";
	uiName = "";
};

function emptyPlayer::onAdd(%this, %obj) 
{
	%obj.setDamageLevel(%this.maxDamage);

	if(isObject(%source = %obj.source) && %obj.slotToMountBot)
	{
		%source.mountObject(%obj,%obj.slotToMountBot);

		// Mount the light if there is one assigned
		if(%obj.light !$= "" || !isObject(%obj.light))
		{
			%billboard = new fxLight ("")
			{
				dataBlock = %obj.light;
				source = %source;
			};

			%obj.light = %billboard;
			%billboard.setTransform(%obj.getTransform());
			%billboard.attachToObject(%obj);
						
			if(!isObject(Eventide_MinigameGroup)) missionCleanUp.add(new SimGroup(Eventide_MinigameGroup));
			Eventide_MinigameGroup.add(%billboard);

			// Force the light to be visible only to the survivors, and not the killers
			for(%i = 0; %i < clientgroup.getCount(); %i++) if(isObject(%client.player))		
			{
				if(isObject(%client = clientgroup.getObject(%i))) 
				%billboard.ScopeToClient(%client);

				else if (%client.player.getdataBlock().isKiller || %client.player $= %source) 
				%billboard.ClearScopeToClient(%client);
			}
		}
	}
	else
	{
		%obj.delete();
		return;
	}
}

function emptyPlayer::onRemove(%this, %obj)
{
	if(isObject(%obj.light)) %obj.light.delete();
}
function emptyPlayer::doDismount(%this, %obj, %forced) 
{
	return;
}
function emptyPlayer::onDisabled(%this, %obj) 
{
	return;
}