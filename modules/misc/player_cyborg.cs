datablock PlayerData(PlayerCyborg : PlayerRenowned) 
{
	uiName = "Cyborg Player";

	killerSpawnMessage = "...";
	
	hitprojectile = "";
	hitobscureprojectile = "";	
	meleetrailskin = "";

	killerChaseLvl1Music = "musicData_Eventide_CyborgNear";
	killerChaseLvl2Music = "musicData_Eventide_CyborgChase";
	killeridlesound = "";
	killeridlesoundamount = 0;
	killerchasesound = "";
	killerchasesoundamount = 0;
	killermeleesound = "";
	killermeleesoundamount = 0;	
	killermeleehitsound = "";
	killermeleehitsoundamount = 3;
	
	killerraisearms = false;
	killerlight = "NoFlarePLight";
	
	leftclickicon = "";
	rightclickicon = "";

	rechargeRate = 0.3;
	maxTools = 3;
	maxWeapons = 3;
	maxForwardSpeed = 5.95;
	maxBackwardSpeed = 3.4;
	maxSideSpeed = 5.1;
	jumpForce = 0;
};

function PlayerCyborg::onPeggFootstep(%this,%obj)
{
	serverplay3d("huntress_walking" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
}

function PlayerCyborg::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("1 1 1");
	KillerSpawnMessage(%obj);
}

function PlayerCyborg::EventideAppearance(%this,%obj,%client)
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

	%hoodieColor = "0.36 0.07 0.07 1";
	%pantsColor = "0.075 0.07 0.07 1";
	%skinColor = "0.83 0.73 0.66 1";

	%obj.setFaceName("cyborgface");
	%obj.setDecalName("francis");
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