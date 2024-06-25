datablock TSShapeConstructor(PuppetMasterDTS) {
	baseShape = "./models/puppetmaster.dts";
	sequence0 = "./models/puppetmaster.dsq";
	sequence1 = "./models/puppetmaster_melee.dsq";
};

datablock PlayerData(PlayerPuppetMaster : PlayerRenowned) 
{
	uiName = "Puppet Master Player";
    shapeFile = PuppetMasterDTS.baseShape;
	
	// Weapon: Katana
	hitprojectile = KillerSharpHitProjectile;
	hitobscureprojectile = KillerKatanaClankProjectile;	
	meleetrailskin = "base";
	meleetrailoffset = "0.3 1.4 0.7"; 	
	meleetrailangle1 = "0 90 0";
	meleetrailangle2 = "0 -90 0";
	meleetrailscale = "4 4 2";		

	killerlight = "NoFlareRLight";	

	killerChaseLvl1Music = "musicData_Eventide_PuppetMasterNear";
	killerChaseLvl2Music = "musicData_Eventide_PuppetMasterChase";

	killeridlesound = "puppetmaster_idle";
	killeridlesoundamount = 3;

	killerchasesound = "puppetmaster_idle";
	killerchasesoundamount = 3;

	killermeleesound = "puppetmaster_melee";
	killermeleesoundamount = 3;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;

	rightclickicon = "color_puppet";
	leftclickicon = "color_melee";
	
	showEnergyBar = true;
	renderFirstPerson = false;
	rechargeRate = 0.3;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 6.25;
	maxBackwardSpeed = 3.57;
	maxSideSpeed = 5.36;
	//+5% Speed
	cameramaxdist = 3;
	maxfreelookangle = 2.5;
	boundingBox = "4.8 4.8 10.1";
	crouchBoundingBox = "4.8 4.8 3.8";
	jumpForce = 0;
};

function PlayerPuppetMaster::onNewDatablock(%this,%obj)
{
    Parent::onNewDataBlock(%this,%obj);

	%obj.schedule(1,setEnergyLevel,0);
	%obj.setScale("1.15 1.15 1.15");
	%obj.mountImage("meleePuppetMasterDaggerImage",0);
	%obj.unHideNode("ALL");
	KillerSpawnMessage(%obj);
}

function PlayerPuppetMaster::onTrigger(%this,%obj,%triggerNum,%bool)
{		
	if(%bool) switch(%triggerNum)
	{
		case 0:	if(%obj.getEnergyLevel() >= 25)
				%obj.KillerMelee(%this,4.25);
				
		case 4: if(%obj.getEnergyLevel() == %this.maxEnergy)
				{
					if(!isObject(PuppetGroup)) new SimGroup(PuppetGroup);
					MissionCleanup.add(PuppetGroup);

					if(PuppetGroup.getCount() < 3)
					{
						%puppet = new Player()
						{
							dataBlock = "PuppetMasterPuppet";
							source = %obj;
							minigame = getMiniGameFromObject(%obj);					
						};
						%obj.mountObject(%puppet,8);
						PuppetGroup.add(%puppet);

						%puppet.unmount();
						%obj.playthread(3,"leftrecoil");

						%puppet.setVelocity(vectorscale(%obj.getEyeVector(),25));
						%puppet.position = %obj.getmuzzlePoint(1);
						%puppet.schedule(2000,setActionThread,sit,1);
						%obj.client.centerprint("<color:FFFFFF><font:impact:30>Press your plant brick key to control the puppet!" ,3);
						%obj.setEnergyLevel(0);
					}
					else %obj.client.centerprint("<color:FFFFFF><font:impact:30>Only 3 puppets can be placed." ,3);
				}
				else %obj.client.centerprint("<color:FFFFFF><font:impact:30>You need more energy to summon a puppet." ,3);

		default:
	}
	Parent::onTrigger(%this,%obj,%triggerNum,%bool);
}

function PlayerPuppetMaster::onPeggFootstep(%this,%obj)
{
	serverplay3d("puppetmaster_walking" @ getRandom(1,4) @ "_sound", %obj.getHackPosition());

	if(getRandom(1,5) == 5) %obj.spawnExplosion("singleBoneProjectile","1 1 1");
}

function PlayerPuppetMaster::onDamage(%this,%obj,%delta)
{
	Parent::onDamage(%this,%obj,%delta);
	if(%delta > 5) %obj.playaudio(0,"puppetmaster_pain" @ getRandom(1,3) @ "_sound");
}

function PlayerPuppetMaster::onDisabled(%this,%obj,%state)
{
	Parent::onDisabled(%this,%obj,%state);
	if(isObject(PuppetGroup)) PuppetGroup.delete();
}

function PlayerPuppetMaster::onRemove(%this,%obj)
{
	Parent::onRemove(%this,%obj);
	if(isObject(PuppetGroup)) PuppetGroup.delete();
}

function PlayerPuppetMaster::EventideAppearance(%this,%obj,%client)
{
	%obj.unHideNode("ALL");	
	%bonecolor = "0.9 0.9 0.9 1";
	%obj.setNodeColor("head",%bonecolor);
	%obj.setNodeColor("rarmslim",%bonecolor);
	%obj.setNodeColor("larmslim",%bonecolor);
	%obj.setNodeColor("lshoe",%bonecolor);
	%obj.setNodeColor("rshoe",%bonecolor);
	%obj.setNodeColor("lhand",%bonecolor);
	%obj.setNodeColor("chest",%bonecolor);
	%obj.setNodeColor("pants",%bonecolor);
	%obj.setNodeColor("rhand",%bonecolor);
	%obj.setHeadUp(0);
}

function PlayerPuppetMaster::onImpact(%this, %obj, %col, %vec, %force)
{
	if(%obj.getState() !$= "Dead") 
	{				
		%zvector = getWord(%vec,2);
		if(%zvector > %this.minImpactSpeed) %obj.playthread(3,"land");
	}
	
	Parent::onImpact(%this, %obj, %col, %vec, %force);	
}
