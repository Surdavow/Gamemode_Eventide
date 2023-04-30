datablock TSShapeConstructor(PuppetMasterDTS) {
	baseShape = "./models/puppetmaster.dts";
	sequence0 = "./models/puppetmaster.dsq";
};

datablock PlayerData(PlayerPuppetMaster : PlayerRenowned) 
{
	uiName = "Puppet Master Player";

    shapeFile = PuppetMasterDTS.baseShape;

	killerChaseLvl1Music = "musicData_OUT_PuppetMasterNear";
	killerChaseLvl2Music = "musicData_OUT_PuppetMasterChase";
	killeridlesound = "puppetmaster_idle";
	killeridlesoundamount = 3;
	killerchasesound = "puppetmaster_idle";
	killerchasesoundamount = 3;
	killerraisearms = false;
	killerlight = "NoFlareRLight";
	showEnergyBar = true;

	firstpersononly = true;
	rechargeRate = 0.75;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 6.65;
	maxBackwardSpeed = 3.8;
	maxSideSpeed = 5.7;
	cameramaxdist = 3;
	maxfreelookangle = 2.5;
	boundingBox = "4.8 4.8 10.1";
	crouchBoundingBox = "4.8 4.8 3.8";
};

function PlayerPuppetMaster::onNewDatablock(%this,%obj)
{
    Parent::onNewDataBlock(%this,%obj);
	%obj.schedule(1,setEnergyLevel,0);
	%obj.setScale("1.05 1.05 1.05");
	%obj.mountImage("meleePuppetMasterDaggerImage",0);
}

function PlayerPuppetMaster::onTrigger(%this,%obj,%triggerNum,%bool)
{	
	Parent::onTrigger(%this,%obj,%triggerNum,%bool);
	
	if(%bool) switch(%triggerNum)
	{
		case 0: Eventide_Melee(%this,%obj,3.5);
		case 4: if(%obj.getEnergyLevel() == %this.maxEnergy)
				{
					if(!isObject(PuppetGroup)) new SimGroup(PuppetGroup);
					MissionCleanup.add(PuppetGroup);

					if(PuppetGroup.getCount() < 3)
					{
						%puppet = new AIPlayer()
						{
							dataBlock = "PuppetMasterPuppet";
							minigame = %obj.minigame;
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
}

function PlayerPuppetMaster::onPeggFootstep(%this,%obj)
{
	serverplay3d("puppetmaster_walking" @ getRandom(1,4) @ "_sound", %obj.getHackPosition());
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