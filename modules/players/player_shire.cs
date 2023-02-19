function GlowFaceImage::onGlow(%this,%obj,%slot)
{

}

function DarknessProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
    if(Eventide_MinigameConditionalCheck(%obj.sourceObject,%col)) 
	{
		%col.mountImage(DarkBlindPlayerImage,%i);
		%col.markedForShireZombify = true;
		%col.setDamageFlash(20);
	}
}

function DarkBlindPlayerImage::onMount(%this, %obj, %slot)
{
	%obj.playaudio(3,"shire_blind_sound");
	parent::onMount(%this, %obj, %slot);
}

function DarkBlindPlayerImage::Dismount(%this, %obj, %slot)
{
	%obj.unmountImage(%slot);
}

function PlayerShire::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%trig == 4 && %obj.getEnergyLevel() == %this.maxEnergy)
	{
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
	}
	else if(%press && %obj.getEnergyLevel() < %this.maxEnergy/2.5) %obj.playthread(0,"undo");

}

function PlayerShire::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);	
	%obj.setScale("1.1 1.1 1.1");
	%this.idlesounds(%obj);
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

function PlayerShire::idlesounds(%this,%obj)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead" || %obj.getdataBlock() !$= %this) return;
	
	%pos = %obj.getPosition();
	%radius = 100;
	%searchMasks = $TypeMasks::PlayerObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);

	%obj.playaudio(0,"shire_Idle" @ getRandom(0,8) @ "_sound");

	while((%targetid = containerSearchNext()) != 0 )
	{
		if(%targetid == %obj) continue;
		%line = vectorNormalize(vectorSub(%targetid.getWorldBoxCenter(),%obj.getWorldBoxCenter()));
		%dot = vectorDot(%obj.getEyeVector(), %line);
		%obscure = containerRayCast(%obj.getEyePoint(),%targetid.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
		if(%dot > 0.55 && !isObject(%obscure) && Eventide_MinigameConditionalCheck(%obj,%targetid,false)) %detectedvictims++;
	}

	if(%detectedvictims) if(!%obj.isInvisible) %obj.playaudio(0,"shire_Close" @ getRandom(0,3) @ "_sound");
	else if(!%obj.isInvisible) %obj.playaudio(0,"shire_Amb" @ getRandom(0,7) @ "_sound");
	%obj.playthread(3,"plant");	
	cancel(%obj.idlesoundsched);
	%obj.idlesoundsched = %this.schedule(getRandom(8000,12000),idlesounds,%obj);
}