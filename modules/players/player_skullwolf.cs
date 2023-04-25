datablock PlayerData(PlayerRenowned : EventidePlayer) 
{
	uiName = "Renowned Player";	

	rechargeRate = 0.26;
	maxDamage = 9999;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 6.65;
	maxBackwardSpeed = 3.8;
	maxSideSpeed = 5.7;
	useCustomPainEffects = true;
	jumpSound = "";
	PainSound		= "";
	DeathSound		= "";	
	firstpersononly = true;
	isKiller = true;

	killerChaseLvl1Music = "musicData_OUT_RenownedNear";
	killerChaseLvl2Music = "musicData_OUT_RenownedChase";
	killeridlesound = "renowned_idle";
	killeridlesoundamount = 8;
	killerchasesound = "renowned_chase";
	killerchasesoundamount = 6;
	killerraisearms = false;
	killerlight = "NoFlareYLight";
};

datablock PlayerData(PlayerSkullWolf : PlayerRenowned) 
{
	uiName = "Skullwolf Player";

	killerchaselvl1music = "musicData_OUT_SkullWolfNear";
	killerchaselvl2music = "musicData_OUT_SkullWolfChase";
	killeridlesound = "skullwolf_idle";
	killeridlesoundamount = 12;
	killerchasesound = "skullwolf_chase";
	killerchasesoundamount = 4;
	killerraisearms = true;
	killerlight = "NoFlareRLight";

	rechargeRate = 0.25;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 6.65;
	maxBackwardSpeed = 3.8;
	maxSideSpeed = 5.7;
	boundingBox = "4.8 4.8 10.1";
	crouchBoundingBox = "4.8 4.8 3.8";
};

function PlayerSkullWolf::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);	
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
		%obj.setmaxforwardspeed(6.65);
		%obj.setEnergyLevel(0);
	}

	%alpha = mClampF(%alpha+0.025,0,1);		
	%obj.setNodeColor("ALL","0.05 0.05 0.05" SPC %alpha);
	%obj.setTempSpeed(0.375);
	if(%alpha == 1) 
	{
		%obj.setTempSpeed(1);
		%this.EventideAppearance(%obj,%obj.client);
		return;
	}

	%obj.reappearsched = %this.schedule(20, reappear, %obj, %alpha);	
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

function PlayerSkullWolf::onDisabled(%this, %obj, %state)
{
	Parent::onDisabled(%this, %obj, %state);
	if(%obj.getState() $= "Dead" && !%obj.isInvisible) %obj.playaudio(0,"skullwolf_death" @ getRandom(0, 6) @ "_sound");
}