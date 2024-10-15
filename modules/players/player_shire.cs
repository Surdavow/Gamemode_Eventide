datablock PlayerData(PlayerShire : PlayerRenowned) 
{
	uiName = "Shire Player";
	isKiller = true;

	killerSpawnMessage = "A hooded figure channels a blinding wrath.";
	
	// Weapon: Axe
	hitprojectile = KillerSharpHitProjectile;
	hitobscureprojectile = KillerAxeClankProjectile;
	
	meleetrailoffset = "0.6 1.4 0.7";

	rightclickicon = "color_blind";
	leftclickicon = "color_melee";

	killerChaseLvl1Music = "musicData_Eventide_HexNear";
	killerChaseLvl2Music = "musicData_Eventide_HexChase";

	killeridlesound = "shire_idle";
	killeridlesoundamount = 9;

	killerchasesound = "shire_chase";
	killerchasesoundamount = 5;

	killermeleesound = "shire_melee";
	killermeleesoundamount = 3;	
	
	killerweaponsound = "shire_weapon";
	killerweaponsoundamount = 5;

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
	killerlight = "NoFlarePLight";

	rechargeRate = 0.3;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 6.55;
	maxBackwardSpeed = 3.74;
	maxSideSpeed = 5.61;
	//+10% Speed
	jumpForce = 0;
};

function DarknessProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
    if((%col.getType() & $TypeMasks::PlayerObjectType) && minigameCanDamage(%obj.sourceObject,%col) == 1) %col.mountImage("DarkBlindPlayerImage",3);
}

function DarkBlindPlayerImage::onBlind(%this, %obj, %slot)
{
	%obj.ShireBlind++;
	%obj.setDamageFlash(1);	

	if(%obj.ShireBlind >= 3)
	{
		%obj.unmountImage(3);
		return;
	}
}

function DarkBlindPlayerImage::onMount(%this, %obj, %slot)
{
	%obj.playaudio(3,"shire_blind_sound");	
	%obj.markedForShireZombify = true;
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
		case 0: if(%obj.getEnergyLevel() >= 25 && %press)
		{
			%obj.KillerMelee(%this,4.5);
			%obj.faceConfigShowFace("Attack");
			return;
		}
		
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
					%obj.setNodeColor(lHand, "1 1 1 1");
			
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
								initialPosition = vectorAdd(%obj.getEyePoint(),"0 0 0.45");
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

function PlayerShire::onPeggFootstep(%this,%obj)
{
	serverplay3d("shire_walking" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
}

function PlayerShire::onNewDatablock(%this, %obj)
{
	//Face system functionality.
	%obj.createEmptyFaceConfig($Eventide_FacePacks["shire"]);
	%facePack = %obj.faceConfig.getFacePack();
	%obj.faceConfig.face["Neutral"] = %facePack.getFaceData(compileFaceDataName(%facePack, "Neutral"));
	%obj.faceConfig.setFaceAttribute("Neutral", "length", -1);

	%obj.faceConfig.face["Attack"] = %facePack.getFaceData(compileFaceDataName(%facePack, "Attack"));
	%obj.faceConfig.setFaceAttribute("Attack", "length", 500);

	%obj.faceConfig.face["Pain"] = %facePack.getFaceData(compileFaceDataName(%facePack, "Pain"));
	%obj.faceConfig.setFaceAttribute("Pain", "length", 1000);

	//Everything else.
	Parent::onNewDatablock(%this,%obj);	

	%obj.setScale("1.15 1.15 1.15");
	%obj.mountImage("meleeAxeImage",0);
	%obj.mountImage("newhoodieimage",3,2,addTaggedString("darkpurple"));
	KillerSpawnMessage(%obj);
}

function PlayerShire::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");	
	%obj.unhideNode("pants");
	%obj.unhideNode("headskin");
	%obj.unhideNode("larm");
	%obj.unhideNode("rarm");
	%obj.unhideNode("rshoe");
	%obj.unhideNode("lshoe");
	%obj.unhideNode("lhand");
	%obj.unhideNode("rhand");
	%obj.unhideNode("femchest");

	%hoodieColor = "0.22 0.11 0.3 1";
	%pantsColor = "0.075 0.075 0.075 1";
	%skinColor = "1 1 1 1";

	if(isObject(%obj.faceConfig))
	{
		%obj.faceConfigShowFaceTimed("Neutral", -1);
	}
	%obj.setDecalName("robe");
	%obj.setNodeColor("rarm",%hoodieColor);
	%obj.setNodeColor("larm",%hoodieColor);
	%obj.setNodeColor("femchest",%hoodieColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor("rshoe",%pantsColor);
	%obj.setNodeColor("lshoe",%pantsColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	%obj.unHideNode("hoodie2");
	%obj.setNodeColor("hoodie2",%hoodieColor);

}

function PlayerShire::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead")
	{
		%obj.playaudio(0,"shire_pain" @ getRandom(1, 4) @ "_sound");
		%obj.faceConfigShowFace("Pain");
	}
}