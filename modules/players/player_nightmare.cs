datablock PlayerData(PlayerNightmare : PlayerRenowned) 
{
	uiName = "Nightmare Player";

	killerSpawnMessage = "...";
	
	// Weapon: Knife
	hitprojectile = KillerRoughHitProjectile;
	hitobscureprojectile = "";
	meleetrailskin = "ragged";

	killerChaseLvl1Music = "musicData_Eventide_NightmareNear";
	killerChaseLvl2Music = "musicData_Eventide_NightmareChase";

	killeridlesound = "nightmare_idle";
	killeridlesoundamount = 9;

	killerchasesound = "nightmare_chase";
	killerchasesoundamount = 7;

	killermeleesound = "";
	killermeleesoundamount = 0;
	
	killerweaponsound = "shire_weapon";
	killerweaponsoundamount = 5;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
	killerlight = "NoFlarePLight";

	rightclickicon = "";
	leftclickicon = "color_melee";	

	rechargeRate = 0.3;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 7.35;
	maxBackwardSpeed = 4.2;
	maxSideSpeed = 6.3;
	jumpForce = 0;
};

function PlayerNightmare::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.mountImage("ChainsawImage",0);
	%obj.schedule(10,onKillerLoop);
	%obj.setScale("1 1 1");
	KillerSpawnMessage(%obj);
}

function PlayerNightmare::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	if(%press) switch(%trig)
	{
		case 0: if(%obj.getEnergyLevel() >= 25) %obj.KillerMelee(%this,4);
				return;
	}
}

function PlayerNightmare::EventideAppearance(%this,%obj,%client)
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
	%obj.unhideNode("shoulderPads");
	%obj.unhideNode("pack");

	%hoodieColor = "0 0 0 1";
	%pantsColor = "0 0 0 1";
	%skinColor = "0 0 0 1";

	%obj.setFaceName("memeGrinMan");
	%obj.setDecalName("chef");
	%obj.setNodeColor("rarm",%hoodieColor);
	%obj.setNodeColor("larm",%hoodieColor);
	%obj.setNodeColor("chest",%hoodieColor);
	%obj.setNodeColor("shoulderPads",%hoodieColor);
	%obj.setNodeColor("pack",%hoodieColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor("rshoe",%pantsColor);
	%obj.setNodeColor("lshoe",%pantsColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand", "0.85 0 0 1");
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