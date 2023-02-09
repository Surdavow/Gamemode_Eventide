function PlayerSkullWolf::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	
	%this.idlesounds(%obj);
	%obj.schedule(1, setEnergyLevel, 0);
	%obj.setScale("1.15 1.15 1.15");
}

function PlayerSkullWolf::disappear(%this,%obj,%alpha)
{
	if(!isObject(%obj) || isEventPending(%obj.reappearsched)) return;

	if(%alpha == 1) %obj.playaudio(1,"skullwolf_cloak_sound");
	
	%alpha = mClampF(%alpha-0.025,0,1);

	if(%alpha == 0)
	{
		%obj.HideNode("ALL");
		%obj.stopaudio(0);
		%obj.setmaxforwardspeed(9);
		%obj.isInvisible = true;
		%obj.reappearsched = %this.schedule(12500, reappear, %obj, 0);
		return;
	}
	else 
	{
		%obj.setNodeColor("ALL","0.05 0.05 0.05" SPC %alpha);
		%obj.setEnergyLevel(%alpha*100);
	}

	%obj.disappearsched = %this.schedule(25, disappear, %obj, %alpha);	
}

function PlayerSkullWolf::reappear(%this,%obj,%alpha)
{
	if(!isObject(%obj) || isEventPending(%obj.disappearsched)) return;

	if(%alpha == 0) 
	{
		%this.EventideAppearance(%obj,%obj.client);
		%obj.isInvisible = false;
		%obj.playaudio(1,"skullwolf_uncloak_sound");
		%obj.setmaxforwardspeed(6.16);
	}

	%alpha = mClampF(%alpha+0.025,0,1);		
	%obj.setNodeColor("ALL","0.05 0.05 0.05" SPC %alpha);
	if(%alpha == 1) 
	{
		%this.EventideAppearance(%obj,%obj.client);	
		return;
	}

	%obj.reappearsched = %this.schedule(25, reappear, %obj, %alpha);	
}

function PlayerSkullWolf::idlesounds(%this,%obj)
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
		if(%dot > 0.55 && !isObject(%obscure) && Eventide_MinigameConditionalCheck(%obj,%targetid,false)) %detectedvictims++;
	}

	if(%detectedvictims)
	{
		
		if(!%obj.isInvisible) %obj.playaudio(0,"skullwolf_Close" @ getRandom(0,3) @ "_sound");

		if(!%obj.raisearms)
		{
			%obj.playthread(1,"armReadyBoth");
			%obj.raisearms = true;
		}
	}
	else
	{
		if(!%obj.isInvisible) %obj.playaudio(0,"skullwolf_Amb" @ getRandom(0,11) @ "_sound");	

		if(%obj.raisearms)
		{
			%obj.playthread(1,"root");
			%obj.raisearms = false;
		}
	}
	%obj.playthread(3,"plant");
	
	cancel(%obj.idlesoundsched);
	%obj.idlesoundsched = %this.schedule(getRandom(4000,5000),idlesounds,%obj);
}

function PlayerSkullWolf::onTrigger(%this,%obj,%triggerNum,%bool)
{	
	Parent::onTrigger(%this,%obj,%triggerNum,%bool);
	
	if(%bool)
	switch(%triggerNum)
	{
		case 0: Eventide_Melee(%this,%obj,3.5);
		case 4: if(!%obj.isInvisible)
				{		
					if(%obj.getEnergyLevel() == %this.maxEnergy && !isEventPending(%obj.disappearsched)) 
					%this.disappear(%obj,1);		
				}
				else 
				{
					cancel(%obj.reappearsched);
					%this.reappear(%obj,0);
				}
		default:
	}
}

function PlayerSkullWolf::EventideAppearance(%this,%obj,%client)
{
	Parent::EventideAppearance(%this,%obj,%client);

	%furColor = "0.05 0.05 0.05 1";
	%obj.setDecalName("");

	%obj.HideNode("headskin");
	%obj.HideNode((%client.rhand ? "rhook" : "rhand"));
	%obj.HideNode((%client.lhand ? "lhook" : "lhand"));
	%obj.HideNode($hat[%client.hat]);
	%obj.HideNode($accent[%client.accent]);
	%obj.HideNode($secondPack[%client.secondPack]);
	%obj.hideNode($pack[%client.pack]);
	%obj.HideNode("visor");
	%obj.HideNode("rpeg");
	%obj.HideNode("lpeg");
	%obj.unHideNode("rshoe");
	%obj.unHideNode("lshoe");	
	%obj.unHideNode("skull");
	%obj.unHideNode("fur");
	%obj.unHideNode("RhandClaws");
	%obj.unHideNode("LhandClaws");
	%obj.setNodeColor("skull","1 1 1 1");
	%obj.setNodeColor("fur",%furColor);

	%obj.unhidenode("chest_blood_front");
	%obj.unhidenode("Lhand_blood");
	%obj.unhidenode("Rhand_blood");
	
	%obj.setNodeColor((%client.rarm ? "rarmSlim" : "rarm"),%furColor);
	%obj.setNodeColor((%client.larm ? "larmSlim" : "larm"),%furColor);
	%obj.setNodeColor("RhandClaws",%furColor);
	%obj.setNodeColor("LhandClaws",%furColor);
	%obj.setNodeColor((%client.chest ? "femChest" : "chest"),%furColor);
	%obj.setNodeColor("pants",%furColor);
	%obj.setNodeColor("rshoe",%furColor);
	%obj.setNodeColor("lshoe",%furColor);
	%obj.setHeadUp(0);
}

function PlayerSkullWolf::onDisabled(%this, %obj, %state) //makes bots have death sound and animation and runs the required bot hole command
{
	Parent::onDisabled(%this, %obj, %state);
	if(%obj.getState() $= "Dead" && !%obj.isInvisible) %obj.playaudio(0,"skullwolf_Death" @ getRandom(0, 6) @ "_sound");
}