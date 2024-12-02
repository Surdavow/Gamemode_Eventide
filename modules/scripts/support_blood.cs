if(LoadRequiredAddOn("Script_Blood") != $Error::None) return;

package Eventide_DSBloodPackage 
{
	function MiniGameSO::reset(%this, %client) 
    {
		Parent::reset(%this, %client);
		if (isObject(DecalGroup)) DecalGroup.deleteAll();		
	}
	
	function Armor::damage(%this, %obj, %src, %pos, %damage, %type) 
    {
		if ((getSimTime() - %obj.spawnTime < $Game::PlayerInvulnerabilityTime) || (%src == %obj && %type != $DamageType::Fall && %type != $DamageType::Impact))
		return Parent::damage(%this, %obj, %src, %pos, %damage, %type);

		if (!%damage) return;

		if (isObject(%src))
		{
			%source = %src;

			if (isObject(%src.sourceObject)) %source = %src.sourceObject;
			if (%pos $= "") %pos = %obj.getHackPosition();
			%vector = (%src.getType() & $TypeMasks::PlayerObjectType) ? %src.getForwardVector() : vectorScale(%src.normal, - 1);
			%norm = vectorNormalize(vectorSub(%pos, %source.getEyePoint()));
			%dot = vectorDot(%obj.getForwardVector(), %vector);

			if (getRandom(1, 2) == 1 && vectorDist(%pos, %source.getEyePoint()) < 10)
			{
				%source.bloody["rhand"] = true;
				if(getRandom(1, 2) == 1) %source.bloody["lhand"] = true;
				%source.bloody["chest_front"] = true;
				if (isObject(%source.client)) %source.client.applyBodyParts();
			}

			if (getRandom(1))
			{
				%obj.bloody["chest_"@ (%dot > 0? "back": "front")] = true;
				if (isObject(%obj.client)) %obj.client.applyBodyParts();
			}
		}

		if (%obj.getDamageLevel() + %damage > %this.maxDamage) %fatal = true;
		
		if ($Blood::DripOnDamage) 
        {
			%time = %obj.getDamagePercent() * 10;
			%time = mClampF(%time, 0, 10);
			%obj.startDrippingBlood(%time);
		}

		if ($Blood::SprayOnDamage && !%fatal) 
		{
			if ($Blood::Effects) createBloodSplatterExplosion(%pos, %norm, "1 1 1");
			%obj.doSplatterBlood(getRandom(1, 4), %pos, %vector);
		}
		else if ($Blood::SprayOnDeath)
		{
			if ($Blood::Effects) createBloodSplatterExplosion(%pos, %norm, "1 1 1");
			%obj.doSplatterBlood(7, %pos, %vector);
		}

		Parent::damage(%this, %obj, %src, %pos, %damage, %type);
	}

	function Armor::onEnterLiquid(%data, %obj, %coverage, %type)
	{
        if(%obj.getdatablock().getName() $= "PlayerRender" || !isObject(%obj.client)) return Parent::onEnterLiquid(%data, %obj, %coverage, %type);

		%obj.bloody["lshoe"] = false;
		%obj.bloody["rshoe"] = false;
		%obj.bloody["lhand"] = false;
		%obj.bloody["rhand"] = false;
		%obj.bloody["chest_front"] = false;
		%obj.bloody["chest_back"] = false;
		%obj.bloody["chest_lside"] = false;
		%obj.bloody["chest_rside"] = false;
		%obj.bloodyFootprints = 0;

		%obj.client.applyBodyParts();
	}
};
if (isPackage(DSBloodPackage)) deactivatePackage("DSBloodPackage");
if ($Blood::doDamageCheck == true) activatePackage("Eventide_DSBloodPackage");