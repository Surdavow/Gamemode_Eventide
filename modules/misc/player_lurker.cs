datablock PlayerData(PlayerLurker : PlayerRenowned) 
{
	uiName = "Lurker Player";

	killerSpawnMessage = "...";
	
	// Weapon: Fist
	hitprojectile = KillerRoughHitProjectile;
	hitobscureprojectile = "";
	meleetrailskin = "ragged";

	killerChaseLvl1Music = "musicData_OUT_DisfiguredNear";
	killerChaseLvl2Music = "musicData_OUT_DisfiguredChase";

	killeridlesound = "";
	killeridlesoundamount = 5;

	killerchasesound = "";
	killerchasesoundamount = 5;

	killermeleesound = "";
	killermeleesoundamount = 3;
	
	killerweaponsound = "";
	killerweaponsoundamount = 4;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
	killerlight = "NoFlarePLight";

	rightclickicon = "color_handaxe";
	leftclickicon = "color_melee";

	rechargeRate = 0.3;
	maxTools = 1;
	maxWeapons = 1;
	maxForwardSpeed = 5.95;
	maxBackwardSpeed = 3.4;
	maxSideSpeed = 5.1;
	jumpForce = 0;
};

function PlayerLurker::onTrigger(%this, %obj, %trig, %press) 
{		
	if(%press) switch(%trig)
	{
		case 0: if(%obj.getEnergyLevel() >= 25) %obj.KillerMelee(%this,4);
				return;
	}
	Parent::onTrigger(%this, %obj, %trig, %press);
}

function PlayerLurker::onPeggFootstep(%this,%obj)
{
	serverplay3d("lurker_walking" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
}

function PlayerLurker::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("1.15 1.15 1.15");
	%obj.isInvisible = false;
}

function PlayerLurker::EventideAppearance(%this,%obj,%client)
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

	%hoodieColor = "0.0 0.18 0.1 1";
	%pantsColor = "0.075 0.075 0.075 1";
	%skinColor = "0.83 0.73 0.66 1";

	%obj.setFaceName("Madmanplain");
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

function PlayerLurker::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead") %obj.playaudio(0,"lurker_pain" @ getRandom(1, 1) @ "_sound");
}