datablock PlayerData(PlayerRender : PlayerRenowned) 
{
	uiName = "Render Player";

	killerSpawnMessage = "A silent anomally reemerges from the past.";

	firstpersononly = true;

	showEnergyBar = true;
	rechargeRate = 0.6;

	killerChaseLvl1Music = "";
	killerChaseLvl2Music = "";

	killeridlesound = "render_idle";
	killeridlesoundamount = 19;

	killerchasesound = "render_idle";
	killerchasesoundamount = 19;

	killermeleesound = "";
	killermeleesoundamount = 0;	

	killermeleehitsound = "";
	killermeleehitsoundamount = 3;
	
	killerlight = "NoFlareRLight";

	rightclickicon = "color_vanish";
	leftclickicon = "color_headache";
	rightclickspecialicon = "";
	leftclickspecialicon = "";

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

function PlayerRender::bottomprintgui(%this,%obj,%client)
{	
	%iconpath = "Add-ons/Gamemode_Eventide/modules/misc/icons/";
	%energylevel = %obj.getEnergyLevel();

	// Some dynamic varirables
	%leftclickstatus = (%obj.getEnergyLevel() >= 50 && isObject(%obj.gazing)) ? "hi" : "lo";
	%rightclickstatus = (%obj.getEnergyLevel() == %this.maxEnergy) ? "hi" : "lo";
	%leftclicktext = (%this.leftclickicon !$= "") ? "<just:left>\c6Left click" : "";
	%rightclicktext = (%this.rightclickicon !$= "") ? "<just:right>\c6Right click" : "";		

	// Regular icons
	%leftclickicon = (%this.leftclickicon !$= "") ? "<just:left><bitmap:" @ %iconpath @ %leftclickstatus @ %this.leftclickicon @ ">" : "";
	%rightclickicon = (%this.rightclickicon !$= "") ? "<just:right><bitmap:" @ %iconpath @ %rightclickstatus @ %This.rightclickicon @ ">" : "";

	%client.bottomprint(%leftclicktext @ %rightclicktext @ "<br>" @ %leftclickicon @ %rightclickicon, 1);
}

function PlayerRender::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	if(%press) switch(%trig)
	{
		case 0:	if(!%obj.isInvisible && %obj.getEnergyLevel() >= 100)
				{
					%startpos = %obj.getMuzzlePoint(0);
					%endpos = %obj.getMuzzleVector(0);	
					%hit = containerRayCast(%startpos,vectorAdd(%startpos,VectorScale(%endpos,50)),$TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType,%obj);

					if(isObject(%hit) && (%hit.getType() & $TypeMasks::PlayerObjectType) && minigameCanDamage(%obj,%hit))
					{
						%obj.setEnergyLevel(%obj.getEnergyLevel()-100);
						%obj.playaudio(0,"render_pain_sound");
						%hit.playaudio(2,"render_turn");
						%hit.mountImage("RenderTurnImage",3);
						//serverPlay3D("renowned_charged_sound",%hit.getPosition());						
						//%hit.mountImage("sm_stunImage",2);
						loopTurn(%hit, getRandom(-1, 1));
					}
				}
				
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
					if(!isEventPending(reappearsched)) %obj.setEnergylevel(66);										
				}
	}	
}

function loopTurn(%pl, %direction, %currTotal)
{
  cancel(%pl.loopTurnSchedule);
  
  %rotationChange = 0.04;
  %currTotal += %rotationChange;

  %transform = %pl.getTransform();
  %currZDir = getWord(%transform, 5) >= 0 ? 1.0 : -1.0;
  %transform = setWord(%transform, 6, getWord(%transform, 6) + %direction * %currZDir * %rotationChange);
  %pl.setTransform(%transform);

  if (%currTotal > 3.14159)
	%pl.playaudio(2,render_turnComplete);
	%hit.unmountImage("RenderTurnImage",3);
    return;

  %pl.loopTurnSchedule = schedule(50, %pl, loopTurn, %pl, %direction, %currTotal);
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
	if (!isObject(%obj) || %obj.isInvisible) return;

	// Set random scale
	%scaleX = getRandom(70, 110) * 0.01;
	%scaleY = getRandom(70, 110) * 0.01;
	%scaleZ = getRandom(100, 110) * 0.01;
	%obj.setScale(%scaleX SPC %scaleY SPC %scaleZ);

	// Set random shape name
	if (getRandom(1, 10) == 1) %obj.setShapeName(getRandom(1, 999999), 8564862);
	else %obj.setShapeName("", 8564862);

	// Set random face name
	%faceName = getRandom(1, 10) == 1 ? "smiley" : "asciiTerror";
	%obj.setFaceName(%faceName);

	//// Define node names
	//%nodeNames = "headskin chest rhand lhand rshoe lshoe";
//
	//// Randomly hide/unhide nodes
	//for (%i = 0; %i < getFieldCount(%nodeNames); %i++)
	//{
	//	%nodeName = getField(%nodeNames, %i);
	//	%hide = getRandom(1, 10) == 1 ? true : false;
	//	%color = getRandom(1, 4) == 1 ? "0 0 0 0.1" : "0 0 0 1";
	//	%obj.setNodeColor(%nodeNmae, %color);
	//	if (%hide) %obj.hideNode(%nodeName);
	//	else %obj.unhideNode(%nodeName);
	//}

	// Set random arm thread
	if (getRandom(1, 10) == 1)
	{
		%thread = "";
		switch (getRandom(1, 5))
		{
			case 1: %thread = "death1";
			case 2: %thread = "sit";
			case 3: %thread = "crouch";
			case 4: %thread = "standjump";			
			case 5: %thread = "talk";
		}
		%obj.setArmThread(%thread);
	}
	else %obj.setArmThread("look");

	// Handle light effects
	if (isObject(%obj.light)) %obj.light.delete();
	
	if (getRandom(1, 10) == 1)
	{
		%obj.light = new fxLight("")
		{
			dataBlock = "NegativePlayerLight";
			source = %obj;
		};
		MissionCleanup.add(%obj.light);
		%obj.light.setTransform(%obj.getTransform());
		%obj.light.attachToObject(%obj);
		%obj.light.schedule(1000, delete);
	}
}

function PlayerRender::Prepperizer(%this,%obj)
{
	if(!isObject(%obj) || %obj.getdataBlock() != %this) return;

	if(%obj.lastSearch < getSimTime())
	{
		if(!isEventPending(%obj.disappearsched) && %obj.stunned) 
		{
			%this.disappear(%obj,1);
			%obj.setEnergylevel(0);
			cancel(%obj.Prepperizer);
			%obj.playaudio(0,"render_death_sound");
			return;
		}

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
				
				if(VectorDist(%obj.getPosition(),%player.getPosition()) < 25)
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

					if(%player.stunned) %player.damage(%obj,%player.getWorldBoxCenter(), mClampF(%closeness,1,15)/20, $DamageType::Default);
					else %player.damage(%obj,%player.getWorldBoxCenter(), mClampF(%closeness,1,15), $DamageType::Default);
					
					%player.markedforRenderDeath = true;
					%client.play2d("render_blind_sound");
					%player.setWhiteOut((%closeness/10)+%player.getdamagepercent()*0.25);
				}

				if(%player.getdamagepercent() >= 0.75 && %obj.lastlaughtime < getSimTime())
				{
					%obj.lastlaughtime = getSimTime()+5000;
					%obj.playaudio(1,"render_laugh_sound");
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
		%obj.unmountImage(2);
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
	}

	%alpha = mClampF(%alpha+0.15,0,1);		
	%obj.setNodeColor("ALL","0.05 0.05 0.05" SPC %alpha);
	if(%alpha == 1) 
	{
		%obj.setTempSpeed(1);
		%this.EventideAppearance(%obj);
		%this.Prepperizer(%obj);
		return;
	}

	%obj.reappearsched = %this.schedule(33, reappear, %obj, %alpha);	
}

function PlayerRender::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead") %obj.playaudio(0,"render_hurt" @ getRandom(1, 4) @ "_sound");
}