datablock PlayerData(PlayerShire : PlayerRenowned) 
{
	uiName = "Shire Player";

	killerChaseLvl1Music = "musicData_OUT_ShireNear";
	killerChaseLvl2Music = "musicData_OUT_ShireChase";
	killeridlesound = "shire_idle";
	killeridlesoundamount = 9;
	killerchasesound = "shire_chase";
	killerchasesoundamount = 4;
	killerraisearms = false;
	killerlight = "NoFlarePLight";

	rechargeRate = 0.3;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 6.93;
	maxBackwardSpeed = 3.96;
	maxSideSpeed = 5.94;
};

function DarknessProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
    if(Eventide_MinigameConditionalCheck(%obj.sourceObject,%col)) %col.mountImage(DarkBlindPlayerImage,3);			
}

function DarkBlindPlayerImage::onBlind(%this, %obj, %slot)
{
	%obj.ShireBlind++;
	%obj.setDamageFlash(1);	

	if(%obj.ShireBlind >= 5)
	{
		%obj.unmountImage(3);
		return;
	}
}

function DarkBlindPlayerImage::onMount(%this, %obj, %slot)
{
	%obj.playaudio(3,"shire_blind_sound");	
	%col.markedForShireZombify = true;
	parent::onMount(%this, %obj, %slot);
}

function DarkBlindPlayerImage::onunMount(%this, %obj, %slot)
{	
	parent::onunMount(%this, %obj, %slot);
	%obj.ShireBlind = 0;
}

function PlayerShire::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	switch(%trig)
	{
		case 0: if(%press) Eventide_Melee(%this,%obj,3.5);

		case 4: if(%obj.getEnergyLevel() == %this.maxEnergy)
				if(%press)
				{
					%obj.mountImage(GlowFaceImage, 1);					
					%obj.casttime = getSimTime();
					%obj.channelcasthand = %obj.schedule(500, setNodeColor, lHand, "0.5 0.33 0.68 1");
					%obj.channelcasthandimage = %obj.schedule(500,mountImage,DarkCastImage,2);
				}
				else
				{
					%obj.unmountImage(1);
					%obj.unmountImage(2);
					cancel(%obj.channelcasthand);
					cancel(%obj.channelcasthandimage);
					%obj.setNodeColor(lHand, %obj.client.lhandcolor);
			
					if(%obj.casttime+500 < getSimTime())
					{
						%obj.setEnergyLevel(0);
						%obj.playthread(2,"leftrecoil");
						serverPlay3d("shire_cast_sound", %obj.getEyePoint());
			
						%velocity = vectorAdd(vectorscale(%obj.getEyeVector(),50),"0 0 2.5");
						%shellcount = 16;
						for(%shell=0; %shell<%shellcount; %shell++)
						{
							%x = (getRandom() - 0.5) * 5 * $pi * 0.005;
							%y = (getRandom() - 0.5) * 5 * $pi * 0.005;
							%z = (getRandom() - 0.5) * 5 * $pi * 0.005;
							%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
							%velocity = MatrixMulVector(%mat, %velocity);
			
							%p = new projectile()
							{
								dataBlock = "DarknessProjectile";
								initialVelocity = %velocity;
								initialPosition = vectorAdd(%obj.getMuzzlePoint(1),"0 0 0.45");
								sourceObject = %obj;
								client = %obj.client;
							};
							MissionCleanup.add(%p);
						}
					}		
				}
		default:
	}	
}

function PlayerShire::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);	
	%obj.setScale("1.1 1.1 1.1");
	%obj.mountImage("meleeAxeImage",0);
}

function PlayerShire::EventideAppearance(%this,%obj,%client)
{
	Parent::EventideAppearance(%this,%obj,%client);
	
	%obj.hideNode($pack[%client.pack]);
	%obj.hideNode($secondPack[%client.secondPack]);
	%obj.hideNode($accent[%client.accent]);
	%obj.hideNode($hat[%client.hat]);
	%obj.HideNode("visor");
	%obj.unhideNode("newhoodie");
	
	if(!%obj.chest)
	{
		%obj.hideNode("chest");
		%obj.unhideNode("femchest");
	}

	%obj.setFaceName("shire");
	%obj.setDecalName("hoodie");

	%hoodieColor = "0.22 0.11 0.3 1";
	%pantsColor = "0.075 0.075 0.075 1";	

	%obj.setNodeColor("newhoodie",%hoodieColor);
	%obj.setNodeColor((%client.rarm ? "rarmSlim" : "rarm"),%hoodieColor);
	%obj.setNodeColor((%client.larm ? "larmSlim" : "larm"),%hoodieColor);
	%obj.setNodeColor((%client.chest ? "chest" : "femchest"),%hoodieColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor((%client.rleg ? "rpeg" : "rshoe"),%pantsColor);
	%obj.setNodeColor((%client.lleg ? "lpeg" : "lshoe"),%pantsColor);

	%obj.setHeadUp(0);
}

function PlayerShire::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead") %obj.playaudio(0,"shire_pain" @ getRandom(1, 4) @ "_sound");
}