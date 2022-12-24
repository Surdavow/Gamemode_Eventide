exec("./datablocks_misc.cs");
exec("./datablocks_players.cs");
exec("./player_angler.cs");
exec("./player_grabber.cs");
exec("./player_renowned.cs");
exec("./player_shire.cs");
exec("./player_skinwalker.cs");
exec("./player_skullwolf.cs");

function EventidePlayer::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(1, setEnergyLevel, 0);
	%obj.setScale("1 1 1");
}

function EventidePlayer::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%press) switch(%trig)
	{
		case 0:	
		case 4: if(%obj.isSkinwalker && %obj.getEnergyLevel() >= %this.maxEnergy && !isObject(%obj.victim) && !isEventPending(%obj.monstertransformschedule)) 
				PlayerSkinwalker.monstertransform(%obj,true);
	}
}

function EventidePlayer::EventideAppearance(%this,%obj,%client)
{
    if(%obj.isSkinwalker && isObject(%obj.victimreplicatedclient)) Parent::EventideAppearance(%this,%obj,%obj.victimreplicatedclient);
    else Parent::EventideAppearance(%this,%obj,%client);
}

function Eventide_MinigameConditionalCheck(%objA,%objB,%exemptDeath)
{
	if((%objA.getClassName() $= "Player" || %objA.getClassName() $= "AIPlayer") && (%objB.getClassName() $= "Player" || %objB.getClassName() $= "AIPlayer"))
	{
		if(%exemptDeath) 
		{
			if(%objA.getstate() !$= "Dead" && %objA.getdataBlock().isKiller && !%objB.getdataBlock().isKiller)		
			if(%objB.getState() !$= "Dead")
			{
				if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB) return true;
			}
			else return true;
		}
		else if(%objA.getstate() !$= "Dead" && %objB.getstate() !$= "Dead" && %objA.getdataBlock().isKiller && !%objB.getdataBlock().isKiller)
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
		initContainerRadiusSearch(%obj.getEyePoint(),10,%mask);
		while(%hit = containerSearchNext())
		{
			if(%hit == %obj) continue;
			%line = vectorNormalize(vectorSub(%hit.getPosition(),%obj.getPosition()));
			%dot = vectorDot( %obj.getEyeVector(), %line );
			%obscure = containerRayCast(%obj.getEyePoint(),%hit.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
			if(isObject(%obscure) || %dot < 0.65 || vectorDist(%obj.getposition(),%hit.getposition()) > %radius) continue;

			if(Eventide_MinigameConditionalCheck(%obj,%hit,true))
			{
				talk(%hit.getstate());
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

function armor::EventideAppearance(%db,%pl,%cl)
{
	%pl.hideNode("ALL");
	%pl.unHideNode((%cl.chest ? "femChest" : "chest"));	
	%pl.unHideNode((%cl.rhand ? "rhook" : "rhand"));
	%pl.unHideNode((%cl.lhand ? "lhook" : "lhand"));
	%pl.unHideNode((%cl.rarm ? "rarmSlim" : "rarm"));
	%pl.unHideNode((%cl.larm ? "larmSlim" : "larm"));
	%pl.unHideNode("headskin");

	if($pack[%cl.pack] !$= "none")
	{
		%pl.unHideNode($pack[%cl.pack]);
		%pl.setNodeColor($pack[%cl.pack],%cl.packColor);
	}
	if($secondPack[%cl.secondPack] !$= "none")
	{
		%pl.unHideNode($secondPack[%cl.secondPack]);
		%pl.setNodeColor($secondPack[%cl.secondPack],%cl.secondPackColor);
	}
	if($hat[%cl.hat] !$= "none")
	{
		%pl.unHideNode($hat[%cl.hat]);
		%pl.setNodeColor($hat[%cl.hat],%cl.hatColor);
	}
	if(%cl.hip)
	{
		%pl.unHideNode("skirthip");
		%pl.unHideNode("skirttrimleft");
		%pl.unHideNode("skirttrimright");
	}
	else
	{
		%pl.unHideNode("pants");
		%pl.unHideNode((%cl.rleg ? "rpeg" : "rshoe"));
		%pl.unHideNode((%cl.lleg ? "lpeg" : "lshoe"));
	}

	%pl.setHeadUp(0);
	if(%cl.pack+%cl.secondPack > 0) %pl.setHeadUp(1);
	if($hat[%cl.hat] $= "Helmet")
	{
		if(%cl.accent == 1 && $accent[4] !$= "none")
		{
			%pl.unHideNode($accent[4]);
			%pl.setNodeColor($accent[4],%cl.accentColor);
		}
	}
	else if($accent[%cl.accent] !$= "none" && strpos($accentsAllowed[$hat[%cl.hat]],strlwr($accent[%cl.accent])) != -1)
	{
		%pl.unHideNode($accent[%cl.accent]);
		%pl.setNodeColor($accent[%cl.accent],%cl.accentColor);
	}

	if (%pl.bloody["lshoe"]) %pl.unHideNode("lshoe_blood");
	if (%pl.bloody["rshoe"]) %pl.unHideNode("rshoe_blood");
	if (%pl.bloody["lhand"]) %pl.unHideNode("lhand_blood");
	if (%pl.bloody["rhand"]) %pl.unHideNode("rhand_blood");
	if (%pl.bloody["chest_front"]) %pl.unHideNode((%cl.chest ? "fem" : "") @ "chest_blood_front");
	if (%pl.bloody["chest_back"]) %pl.unHideNode((%cl.chest ? "fem" : "") @ "chest_blood_back");

	%pl.setFaceName(%cl.faceName);
	%pl.setDecalName(%cl.decalName);

	%pl.setNodeColor("headskin",%cl.headColor);	
	%pl.setNodeColor("chest",%cl.chestColor);
	%pl.setNodeColor("femChest",%cl.chestColor);
	%pl.setNodeColor("pants",%cl.hipColor);
	%pl.setNodeColor("skirthip",%cl.hipColor);	
	%pl.setNodeColor("rarm",%cl.rarmColor);
	%pl.setNodeColor("larm",%cl.larmColor);
	%pl.setNodeColor("rarmSlim",%cl.rarmColor);
	%pl.setNodeColor("larmSlim",%cl.larmColor);
	%pl.setNodeColor("rhand",%cl.rhandColor);
	%pl.setNodeColor("lhand",%cl.lhandColor);
	%pl.setNodeColor("rhook",%cl.rhandColor);
	%pl.setNodeColor("lhook",%cl.lhandColor);	
	%pl.setNodeColor("rshoe",%cl.rlegColor);
	%pl.setNodeColor("lshoe",%cl.llegColor);
	%pl.setNodeColor("rpeg",%cl.rlegColor);
	%pl.setNodeColor("lpeg",%cl.llegColor);
	%pl.setNodeColor("skirttrimright",%cl.rlegColor);
	%pl.setNodeColor("skirttrimleft",%cl.llegColor);

	//Set blood colors.
	%pl.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%pl.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%pl.setNodeColor("lhand_blood", "0.7 0 0 1");
	%pl.setNodeColor("rhand_blood", "0.7 0 0 1");
	%pl.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%pl.setNodeColor("chest_blood_back", "0.7 0 0 1");
	%pl.setNodeColor("femchest_blood_front", "0.7 0 0 1");
	%pl.setNodeColor("femchest_blood_back", "0.7 0 0 1");
}