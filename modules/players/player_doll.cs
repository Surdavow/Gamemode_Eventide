datablock TSShapeConstructor(DollDTS) 
{
	baseShape = "./models/puppet.dts";
	sequence0 = "./models/default.dsq";
};

datablock PlayerData(PlayerDoll : PlayerRenowned)
{
	uiName = "PlayerDoll";
	shapeFile = DollDTS.baseShape;

	// Weapon: magic
	hitprojectile = KillerSharpHitProjectile;
	hitobscureprojectile = KillerAxeClankProjectile;
	meleetrailskin = "magic";
	meleetrailoffset = "0.1 1.1 0.5"; 	
	meleetrailangle1 = "0 90 0";
	meleetrailangle2 = "0 -90 0";
	meleetrailscale = "4 4 3";

	killerChaseLvl1Music = "musicData_Eventide_DollNear";
	killerChaseLvl2Music = "musicData_Eventide_DollChase";

	killeridlesound = "doll_idle";
	killeridlesoundamount = 20;

	killerchasesound = "doll_chase";
	killerchasesoundamount = 8;
	
	killerweaponsound = "disfigured_weapon";
	killerweaponsoundamount = 4;	

	killermeleesound = "doll_melee";
	killermeleesoundamount = 0;

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
	leftclickicon = "color_melee";
	
	killerlight = "NoFlarePLight";

	maxTools = 0;
	maxWeapons = 0;
	firstpersononly = true;
	rechargeRate = 0.3;
	runForce = 5616;
	maxForwardSpeed = 8.05;
	maxBackwardSpeed = 4.6;
	maxSideSpeed = 6.9;
	//+15% Speed 8.05, 4.6, 6.9
	maxDamage = 9999;
	jumpforce = 0;
};

function PlayerDoll::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("0.9 0.9 0.9");
	%obj.schedule(1,setEnergyLevel,0);
	KillerSpawnMessage(%obj);
}

function PlayerDoll::onTrigger(%this, %obj, %trig, %press) 
{			
	if(%press) switch(%trig)
	{
		case 0: if(%obj.getEnergyLevel() >= 25) return %obj.KillerMelee(%this,4.5);
	}
	Parent::onTrigger(%this, %obj, %trig, %press);	
}

function PlayerDoll::onPeggFootstep(%this,%obj)
{
	serverplay3d("huntress_walking" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
}

function PlayerDoll::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");
	%obj.unHideNode("femChestpuppet");	
	%obj.unHideNode("rhandpuppet");
	%obj.unHideNode("lhandpuppet");
	%obj.unHideNode("rarmSlim");
	%obj.unHideNode("larmSlim");
	%obj.unHideNode("headpuppet");
	%obj.unHideNode("buttoneyes");
	%obj.unHideNode("pantspuppet");
	%obj.unHideNode("rshoe");
	%obj.unHideNode("lshoe");

	%obj.setFaceName("smileyfST");
	%obj.setDecalName("sweater");

	%obj.setNodeColor("headpuppet",%client.headColor);	
	%obj.setNodeColor("femChestpuppet",%client.chestColor);
	%obj.setNodeColor("pantspuppet",%client.hipColor);
	%obj.setNodeColor("rarmSlim",%client.rarmColor);
	%obj.setNodeColor("larmSlim",%client.larmColor);
	%obj.setNodeColor("rhandpuppet",%client.rhandColor);
	%obj.setNodeColor("lhandpuppet",%client.lhandColor);
	%obj.setNodeColor("rshoe",%client.rlegColor);
	%obj.setNodeColor("lshoe",%client.llegColor);
	%obj.setNodeColor("buttoneyes","0.1 0.1 0.1 1");
}