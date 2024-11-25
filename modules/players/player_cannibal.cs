datablock PlayerData(PlayerCannibal : PlayerRenowned) 
{
	uiName = "Cannibal Player";

	killerSpawnMessage = "...";
	
	hitprojectile = KillerRoughHitProjectile;
	hitobscureprojectile = "";
	meleetrailskin = "ragged";

	killerChaseLvl1Music = "musicData_Eventide_CannibalNear";
	killerChaseLvl2Music = "musicData_Eventide_CannibalChase";

	killeridlesound = "";
	killeridlesoundamount = 1;

	killerchasesound = "";
	killerchasesoundamount = 1;

	killermeleesound = "";
	killermeleesoundamount = 1;
	
	killerweaponsound = "cannibal_weapon";
	killerweaponsoundamount = 4;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
	killerlight = "NoFlarePLight";

	rightclickicon = "color_grab";
	leftclickicon = "color_melee";	

	rechargeRate = 0.3;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 7.35;
	maxBackwardSpeed = 4.2;
	maxSideSpeed = 6.3;
	jumpForce = 0;
};

function PlayerCannibal::onTrigger(%this, %obj, %trig, %press)
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	if(%press && !%trig && %obj.getEnergyLevel() >= 25)
	return %obj.KillerMelee(%this,4);
	
}

function PlayerCannibal::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.mountImage("meleeKnifeImage",0);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("1 1 1");
	KillerSpawnMessage(%obj);
}

function PlayerCannibal::EventideAppearance(%this,%obj,%client)
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
	%obj.unhideNode("chest");

	%hoodieColor = "0.05 0.05 0.05 1";
	%pantsColor = "0.05 0.05 0.05 1";
	%skinColor = "0.83 0.73 0.66 1";

	%obj.setFaceName("Cannibal");
	%obj.setDecalName("jgtuxedo");
	%obj.setNodeColor("rarm",%hoodieColor);
	%obj.setNodeColor("larm",%hoodieColor);
	%obj.setNodeColor("chest",%hoodieColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor("rshoe",%pantsColor);
	%obj.setNodeColor("lshoe",%pantsColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	%obj.unhideNode("chest_blood_front");
	%obj.unhideNode("Lhand_blood");
	%obj.unhideNode("Rhand_blood");
	%obj.unhideNode("lshoe_blood");
	%obj.unhideNode("rshoe_blood");
	
	//Set blood colors.
	%obj.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("lhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("rhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_back", "0.7 0 0 1");
}