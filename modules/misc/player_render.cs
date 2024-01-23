datablock PlayerData(PlayerRender : PlayerRenowned) 
{
	uiName = "Render Player";

	killerSpawnMessage = "A silent anomally reemerges from the past.";

	firstpersononly = true;

	showEnergyBar = true;
	rechargeRate = 0.6;

	killerChaseLvl1Music = "";
	killerChaseLvl2Music = "";

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
	jumpForce = 0;
};

function PlayerRender::onImpact(%this, %obj, %col, %vec, %force)
{
	if(%force > %this.minImpactSpeed) %obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");	
}

function PlayerRender::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	if(%press) switch(%trig)
	{
		case 0:	//Attack				
				
		case 4: //Invisibility
				if(!%obj.isInvisible)
				{ 					
					if(!isEventPending(%obj.disappearsched)) 
					{
						%this.disappear(%obj,1);
						%obj.setEnergylevel(0);
					}

				}
				else if(%obj.getEnergyLevel() == %this.maxEnergy)
				{
					cancel(%obj.reappearsched);
					%this.reappear(%obj,0);
					if(!isEventPending(reappearsched)) %obj.setEnergylevel(0);										
				}
	}	
}

function PlayerRender::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);

	if(isObject(%obj.light)) %obj.light.delete();
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

function Player::PrepperizerEffect(%obj)
{	
	if(!isObject(%obj) || %obj.isInvisible) return;

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

	if(getRandom(1,10) == 1) 
	{
		switch(getRandom(1,5))
		{
			case 1: %obj.setArmThread("death1");
			case 2: %obj.setArmThread("sit");
			case 3: %obj.setArmThread("crouch");
			case 4: %obj.setArmThread("standjump");			
			case 5: %obj.setArmThread("talk");			
		}
	}
	else %obj.setArmThread("look");

	if(getRandom(1,10) == 1)
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
}

function PlayerRender::Prepperizer(%this,%obj)
{
	if(!isObject(%obj) || %obj.getdataBlock() != %this) return;

	if(%obj.lastSearch < getSimTime())
	{
		%obj.lastSearch = getSimTime()+100;		
		
		if(isObject(ClientGroup)) for(%i = 0; %i < ClientGroup.getCount(); %i++) if(isObject(%client = ClientGroup.getObject(%i)))
		if(isObject(%player = %client.player))
		{
			if(%player == %obj) continue;			

			%line = vectorNormalize(vectorSub(%obj.getPosition(),%player.getEyePoint()));
			%dot = vectorDot(%player.getEyeVector(), %line);
			%obscure = containerRayCast(%player.getEyePoint(),%obj.getPosition(),$TypeMasks::FxBrickObjectType, %obj);
			
			if(%obj.lastNearPlayer + getRandom(5000, 10000) < getSimTime())
			{
				%obj.lastNearPlayer = getSimTime();
				missionCleanup.add(new projectile()
				{
					dataBlock        = "PrepperProjectile";
					initialVelocity  = 0;
					initialPosition  = vectorAdd(%player.getPosition(), getRandom(-5,5) SPC getRandom(-5,5) SPC getRandom(0,5));
				});			
			}	

			if(!%obj.isInvisible)
			{			
				if(!isObject(%obscure) && %dot > 0.5 && minigameCanDamage(%obj,%player) == 1 && !%player.getDataBlock().isDowned)
				{				
					%closeness = 8/(VectorDist(%obj.getPosition(),%player.getPosition())*0.25);
					%player.damage(%obj,%player.getWorldBoxCenter(), mClampF(%closeness,1,15), $DamageType::Default);
					%player.markedforRenderDeath = true;
					%client.play2d("render_blind_sound");
					%player.setWhiteOut((%closeness/10)+%player.getdamagepercent()*0.25);
				}
			}
		}		
	}

	%obj.PrepperizerEffect();	
	cancel(%obj.Prepperizer);
	%obj.Prepperizer = %this.schedule(33,Prepperizer,%obj);
}

function PlayerRender::disappear(%this,%obj,%alpha)
{
	if(!isObject(%obj) || isEventPending(%obj.reappearsched)) return;

	if(%alpha == 1)
	{
		%obj.setShapeName ("", 8564862);
		%obj.setArmThread("look");
		%obj.stopaudio(3);
		%obj.playaudio(1,"render_disappear_sound");		
		%this.EventideAppearance(%obj);
		%obj.setScale("1 1 1");
	}
	
	%alpha = mClampF(%alpha-0.05,0,1);

	if(%alpha == 0)
	{
		%obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");
		%obj.HideNode("ALL");
		%obj.stopaudio(0);
		%obj.setTempSpeed(2.5);
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
		%obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");
		%this.EventideAppearance(%obj,%obj.client);
		%obj.isInvisible = false;
		%obj.playaudio(1,"render_appear_sound");
		%obj.setmaxforwardspeed(%this.maxForwardSpeed);
	}

	%alpha = mClampF(%alpha+0.15,0,1);		
	%obj.setNodeColor("ALL","0.05 0.05 0.05" SPC %alpha);
	if(%alpha == 1) 
	{
		%obj.setTempSpeed(0.1);
		%this.EventideAppearance(%obj);
		%this.Prepperizer(%obj);
		%obj.setmaxforwardspeed(1);
		return;
	}

	%obj.reappearsched = %this.schedule(33, reappear, %obj, %alpha);	
}