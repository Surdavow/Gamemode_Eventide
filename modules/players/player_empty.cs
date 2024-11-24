datablock fxLightData(blankBillboard)
{
	LightOn = false;
	flareOn = true;
	ConstantSizeOn = true;
	flarebitmap = "base/data/shapes/blank.png";
	ConstantSize = 2;    
	nearSize = 2;
	farSize = 2;
	farDistance = 9999;
	LinkFlare = false;
	AnimOffsets = false;
    FadeTime = 99999;
	
	blendMode = 1;
	flareColor = "1 1 1 1";	
};

datablock fxLightData(downedBillboard : blankBillboard)
{
	flarebitmap = "./icons/icon_downed.png";
};

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
		
		// Mount the image if there is one assigned
		if(%obj.imageToMount !$= "") 
		{
			if(%obj.imageColor !$= "") %obj.mountImage(%obj.imageToMount,0,1,%obj.imageColor);
			else %obj.mountImage(%obj.imageToMount,0);
		}

		// Mount the light if there is one assigned
		if(%obj.lightToMount !$= "")
		{
			%billboard = new fxLight ("")
			{
				dataBlock = %obj.lightToMount;
				source = %source;
			};
			
			%obj.lightToMount = %billboard;
			MissionCleanup.add(%billboard);
			%billboard.setTransform(%obj.getTransform());
			%billboard.attachToObject(%obj);

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
	if(isObject(%obj.lightToMount)) %obj.lightToMount.delete();
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