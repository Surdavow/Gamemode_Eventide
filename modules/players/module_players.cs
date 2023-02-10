exec("./datablocks_misc.cs");
exec("./datablocks_players.cs");
exec("./player_eventide.cs");
exec("./player_angler.cs");
exec("./player_grabber.cs");
exec("./player_renowned.cs");
exec("./player_shire.cs");
exec("./bot_shirezombie.cs");
exec("./player_skinwalker.cs");
exec("./player_skullwolf.cs");

function Eventide_MinigameConditionalCheck(%objA,%objB,%exemptDeath)//exemptdeath is to skip checking if the victim is dead
{
	if((%objA.getClassName() $= "Player" || %objA.getClassName() $= "AIPlayer") && (%objB.getClassName() $= "Player" || %objB.getClassName() $= "AIPlayer"))
	{
		if(%exemptDeath) 
		{
			if(%objA.getstate() !$= "Dead" && %objA.getdataBlock().isKiller && !%objB.getdataBlock().isKiller)		
			if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB) 
			return true;
		}
		else 
		if(%objA.getstate() !$= "Dead" && %objB.getstate() !$= "Dead" && %objA.getdataBlock().isKiller && !%objB.getdataBlock().isKiller)
		{
			if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB)
			return true;
		}
	}

	return false;
}

function Eventide_Melee(%this,%obj,%radius)
{
	if(!%obj.isInvisible && %obj.lastclawed+500 < getSimTime() && %obj.getEnergyLevel() >= %this.maxEnergy/8)
	{
		%obj.lastclawed = getSimTime();										
		%obj.playthread(2,"activate2");
		%oscale = getWord(%obj.getScale(),2);
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
		initContainerRadiusSearch(%obj.getPosition(),10,%mask);
		while(%hit = containerSearchNext())
		{
			if(%hit == %obj) continue;
			%line = vectorNormalize(vectorSub(%hit.getPosition(),%obj.getPosition()));
			%dot = vectorDot( %obj.getEyeVector(), %line );
			%obscure = containerRayCast(%obj.getEyePoint(),%hit.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
			if(isObject(%obscure) || %dot < 0.65 || vectorDist(%obj.getposition(),%hit.getposition()) > %radius) continue;

			if(Eventide_MinigameConditionalCheck(%obj,%hit,true))
			{
				if(%hit.getstate() $= "Dead")
				{
					if(%obj.getdataBlock().getName() $= "PlayerSkullwolf" && %obj.isCrouched()) 
					{					
						%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");	
						%obj.playthread(3,"plant");
						%obj.setEnergyLevel(%obj.getEnergyLevel()+%this.maxEnergy/6);
						%hit.spawnExplosion("goryExplosionProjectile",%hit.getScale());						
						%hit.schedule(50,delete);
					}
					continue;
				}								

				switch$(%obj.getdataBlock().getName())
				{
					case "PlayerAngler": 	%obj.playaudio(0,"angler_Atk" @ getRandom(0,2) @ "_sound");
											if(isObject(%obj.hookrope)) %obj.hookrope.delete();																						
					case "PlayerSkullWolf": %obj.playaudio(0,"skullwolf_Atk" @ getRandom(0,6) @ "_sound");
					default:
				}
				
				%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");
				%obj.setEnergyLevel(%obj.getEnergyLevel()-%this.maxEnergy/8);
				%hit.setvelocity(vectorscale(VectorNormalize(vectorAdd(%obj.getForwardVector(),"0" SPC "0" SPC "0.15")),15));								
				%hit.damage(%obj, %hit.getWorldBoxCenter(), 25*%oscale, $DamageType::Default);
				%hit.spawnExplosion(pushBroomProjectile,"2 2 2");
			}								
		}
	}
}