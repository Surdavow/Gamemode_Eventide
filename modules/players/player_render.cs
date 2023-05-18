datablock PlayerData(PlayerRender : PlayerRenowned) 
{
	uiName = "Render Player";

	firstpersononly = false;
	thirdpersononly = true;
	showEnergyBar = false;
	cameramaxdist = 4;

	killerChaseLvl1Music = "musicData_OUT_GrabberNear";
	killerChaseLvl2Music = "musicData_OUT_GrabberChase";

	killeridlesound = "";
	killeridlesoundamount = 9;

	killerchasesound = "";
	killerchasesoundamount = 4;

	killermeleesound = "";
	killermeleesoundamount = 0;	

	killermeleehitsound = "";
	killermeleehitsoundamount = 3;
	
	killerraisearms = false;
	killerlight = "NoFlareRLight";

	rechargeRate = 0.3;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 4;
	maxBackwardSpeed = 2;
	maxSideSpeed = 3;
};

function PlayerRender::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	switch(%trig)
	{
		case 0:
		case 4: if(!%obj.isInvisible)
				{ 
					if(!isEventPending(%obj.disappearsched)) %this.disappear(%obj,1);					
				}
				else 
				{
					cancel(%obj.reappearsched);
					%this.reappear(%obj,0);
				}
		default:
	}	
}

function PlayerRender::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%this.EventideAppearance(%obj);
	%this.Prepperizer(%obj);
	KillerSpawnMessage(%obj);
}

function PlayerRender::EventideAppearance(%this,%obj)
{
	%obj.hideNode("ALL");
	%obj.unhidenode("chest");
	%obj.unhidenode("pants");
	%obj.unhidenode("LShoe");
	%obj.unhidenode("RShoe");
	%obj.unhidenode("LArm");
	%obj.unhidenode("LHand");
	%obj.unhidenode("RArm");
	%obj.unhidenode("RHand");
	%obj.unhidenode("headskin");
	%obj.setnodecolor("chest", "0 0 0 1");
	%obj.setnodecolor("headskin", "0 0 0 1");
	%obj.setnodecolor("pants", "0 0 0 1");
	%obj.setnodecolor("LShoe", "0 0 0 1");
	%obj.setnodecolor("RShoe", "0 0 0 1");
	%obj.setnodecolor("LArm", "0 0 0 1");
	%obj.setnodecolor("LHand", "0 0 0 1");
	%obj.setnodecolor("RArm", "0 0 0 1");
	%obj.setnodecolor("RHand", "0 0 0 1");
	%obj.setdecalname("AAA-None");
	%obj.setfacename("asciiTerror");
}

function PlayerRender::onRemove(%this,%obj)
{
	Parent::onRemove(%this,%obj);
	if(isObject(%obj.light)) %obj.light.delete();
}

function PlayerRender::Prepperizer(%this,%obj)
{
	if(!isObject(%obj) || %obj.isInvisible) return;

	if(%obj.lastSearch < getSimTime())
	{
		%obj.lastSearch = getSimTime()+100;

		if(isObject(ClientGroup))
		for(%i = 0; %i < ClientGroup.getCount(); %i++) if(isObject(%client = ClientGroup.getObject(%i)))
		if(isObject(%player = %client.player))
		{
			if(%player == %obj) continue;

			%line = vectorNormalize(vectorSub(%obj.getPosition(),%player.getEyePoint()));
			%dot = vectorDot(%player.getEyeVector(), %line);
			%obscure = containerRayCast(%player.getEyePoint(),%obj.getPosition(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
			
			if(!isObject(%obscure) && %dot > 0.5 && minigameCanDamage(%obj,%player) == 1)
			{				
				%closeness = 1/(VectorDist(%obj.getPosition(),%player.getPosition())*0.01);
				%player.damage(%obj,%player.getWorldBoxCenter(), mClampF(%closeness,1,15), $DamageType::Default);
				%player.markedforRenderDeath = true;
				%client.play2d("render_blind_sound");
				%player.setWhiteOut(%closeness*0.005);
			}
		}
		

	}

	%obj.setScale(getRandom(70,110)*0.01 SPC getRandom(70,110)*0.01 SPC getRandom(100,110)*0.01);

	if(getRandom(1,10) == 1) %obj.setShapeName (getRandom(1,999999), 8564862);
	else %obj.setShapeName ("", 8564862);
	
	if(getRandom(1,10) == 1) %obj.setfacename("smiley");
	else %obj.setfacename("asciiTerror");

	if(getRandom(1,10) == 1) %obj.hideNode("headskin");
	else %obj.unhidenode("headskin");

	if(getRandom(1,10) == 1) %obj.hideNode("chest");
	else %obj.unhidenode("chest");	

	if(getRandom(1,10) == 1) %obj.hideNode("rhand");
	else %obj.unhidenode("rhand");	

	if(getRandom(1,10) == 1) %obj.hideNode("lhand");
	else %obj.unhidenode("lhand");

	if(getRandom(1,10) == 1) %obj.hideNode("rshoe");
	else %obj.unhidenode("rshoe");	

	if(getRandom(1,10) == 1) %obj.hideNode("lshoe");
	else %obj.unhidenode("lshoe");		

	if(getRandom(1,4) == 1) %obj.setnodecolor("ALL","0 0 0 0.1");
	else %obj.setnodecolor("ALL","0 0 0 1");

	if(getRandom(1,4) == 1) 
	{
		switch(getRandom(1,2))
		{
			case 1: %obj.setArmThread("death1");
			case 2: %obj.setArmThread("sit");
			case 3: %obj.setArmThread("crouch");
		}				
	}
	else %obj.setArmThread("look");

	if(getRandom(1,5) == 1)
	{
		%obj.light = new fxLight ("")
		{
			dataBlock = "NegativePlayerLight";
			source = %obj;
		};
		MissionCleanup.add(%obj.light);
		%obj.light.setTransform(%obj.getTransform());
		%obj.light.attachToObject(%obj);
		%obj.light.schedule(1000,delete);
	}
	else if(isObject(%obj.light)) %obj.light.delete();

	if(getRandom(1,50) == 1)
	{
		%obj.stopaudio(3);
		%obj.playaudio(3,"render_glitch" @ getRandom(1,4) @ "_sound");	
	}
	
	cancel(%obj.Prepperizer);
	%obj.Prepperizer = %this.schedule(33,Prepperizer,%obj);
}

function PlayerRender::disappear(%this,%obj,%alpha)
{
	if(!isObject(%obj) || isEventPending(%obj.reappearsched)) return;

	if(%alpha == 1)
	{
		cancel(%obj.Prepperizer);
		%obj.setArmThread("look");
		%obj.stopaudio(3);
		%obj.playaudio(1,"render_disappear_sound");		
		%this.EventideAppearance(%obj);
		%obj.setScale("1 1 1");
	}
	
	%alpha = mClampF(%alpha-0.05,0,1);

	if(%alpha == 0)
	{
		%obj.HideNode("ALL");
		%obj.stopaudio(0);
		%obj.setmaxforwardspeed(10);
		%obj.isInvisible = true;	
		return;
	}
	else %obj.setNodeColor("ALL","0.05 0.05 0.05" SPC %alpha);
	

	%obj.disappearsched = %this.schedule(25, disappear, %obj, %alpha);	
}

function PlayerRender::reappear(%this,%obj,%alpha)
{
	if(!isObject(%obj) || isEventPending(%obj.disappearsched)) return;

	if(%alpha == 0) 
	{
		%this.EventideAppearance(%obj,%obj.client);
		%obj.isInvisible = false;
		%obj.playaudio(1,"render_appear_sound");
		%obj.setmaxforwardspeed(%this.maxForwardSpeed);
	}

	%alpha = mClampF(%alpha+0.05,0,1);		
	%obj.setNodeColor("ALL","0.05 0.05 0.05" SPC %alpha);
	%obj.setTempSpeed(0.375);
	if(%alpha == 1) 
	{
		%obj.setTempSpeed(1);
		%this.EventideAppearance(%obj);
		%this.Prepperizer(%obj);
		return;
	}

	%obj.reappearsched = %this.schedule(20, reappear, %obj, %alpha);	
}