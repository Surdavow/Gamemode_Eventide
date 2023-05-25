datablock TSShapeConstructor(SkullwolfDTS) 
{
	baseShape = "./models/skullwolf.dts";
	sequence0 = "./models/skullwolf.dsq";
};

datablock PlayerData(PlayerSkullWolf : PlayerRenowned) 
{
	uiName = "Skullwolf Player";
	shapeFile = SkullwolfDTS.baseShape;	

	killerSpawnMessage = "A fanged anomally of the forest has arrived.";

	killerchaselvl1music = "musicData_OUT_SkullWolfNear";
	killerchaselvl2music = "musicData_OUT_SkullWolfChase";

	killeridlesound = "skullwolf_idle";
	killeridlesoundamount = 12;

	killerchasesound = "skullwolf_chase";
	killerchasesoundamount = 4;

	killermeleesound = "skullwolf_melee";
	killermeleesoundamount = 7;

	killermeleehitsound = "skullwolf_hit";
	killermeleehitsoundamount = 3;		
	
	killerraisearms = true;
	killerlight = "NoFlareRLight";

	rechargeRate = 0.25;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 6.84;
	maxBackwardSpeed = 3.91;
	maxSideSpeed = 5.87;
	boundingBox = "4.8 4.8 10.1";
	crouchBoundingBox = "4.8 4.8 3.8";
};

function PlayerSkullWolf::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);	
	%obj.schedule(1, setEnergyLevel, 0);
	%obj.setScale("1.15 1.15 1.15");
	KillerSpawnMessage(%obj);
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

function PlayerSkullWolf::onPeggFootstep(%this,%obj)
{
	serverplay3d("skullwolf_walking" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
}

function PlayerSkullWolf::onImpact(%this, %obj, %col, %vec, %force)
{
	if(%obj.getState() !$= "Dead") 
	{				
		%zvector = getWord(%vec,2);
		if(%zvector > %this.minImpactSpeed) %obj.playthread(3,"land");
	}
	
	Parent::onImpact(%this, %obj, %col, %vec, %force);	
}

function PlayerSkullWolf::reappear(%this,%obj,%alpha)
{
	if(!isObject(%obj) || isEventPending(%obj.disappearsched)) return;

	if(%alpha == 0) 
	{
		%obj.unHideNode("ALL");
		%obj.isInvisible = false;
		%obj.playaudio(1,"skullwolf_uncloak_sound");
		%obj.setmaxforwardspeed(6.84);
		%obj.setEnergyLevel(0);
	}

	%alpha = mClampF(%alpha+0.025,0,1);		
	%obj.setNodeColor("ALL","0.05 0.05 0.05" SPC %alpha);
	%obj.setTempSpeed(0.375);
	if(%alpha == 1) 
	{
		%obj.setTempSpeed(1);
		%obj.unHideNode("ALL");
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
		case 0: %obj.KillerMelee(%this,3.5);
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
	}
}

function PlayerSkullWolf::EventideAppearance(%this,%obj,%client)
{
	%furcolor = "0.05 0.05 0.05 1";
	%obj.setnodecolor("head",%furcolor);		
	%obj.setnodecolor("rarm",%furcolor);
	%obj.setnodecolor("larm",%furcolor);
	%obj.setnodecolor("rhand",%furcolor);
	%obj.setnodecolor("lhand",%furcolor);
	%obj.setnodecolor("femchest",%furcolor);
	%obj.setnodecolor("pants",%furcolor);
	%obj.setnodecolor("rshoe",%furcolor);
	%obj.setnodecolor("lshoe",%furcolor);
}

function PlayerSkullWolf::onDisabled(%this, %obj, %state)
{
	Parent::onDisabled(%this, %obj, %state);
	if(%obj.getState() $= "Dead" && !%obj.isInvisible) %obj.playaudio(0,"skullwolf_death" @ getRandom(0, 6) @ "_sound");
}