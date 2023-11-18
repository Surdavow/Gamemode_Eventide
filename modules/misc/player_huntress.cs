datablock PlayerData(PlayerHuntress : PlayerRenowned) 
{
	uiName = "Huntress Player";

	killerSpawnMessage = "...";

	killerChaseLvl1Music = "musicData_OUT_HuntressNear";
	killerChaseLvl2Music = "musicData_OUT_HuntressChase";

	killeridlesound = "huntress_idle";
	killeridlesoundamount = 9;

	killerchasesound = "huntress_idle";
	killerchasesoundamount = 9;

	killermeleesound = "huntress_attack";
	killermeleesoundamount = 5;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
	killerraisearms = false;
	killerlight = "NoFlarePLight";

	rechargeRate = 0.3;
	maxTools = 1;
	maxWeapons = 1;
	maxForwardSpeed = 5.95;
	maxBackwardSpeed = 3.4;
	maxSideSpeed = 5.1;
	jumpForce = 0;
	PainSound = "huntress_pain";
};

function PlayerHuntress::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	switch(%trig)
	{
		case 0: if(%press) %obj.KillerMelee(%this,3.5);
	}
}

function PlayerHuntress::onPeggFootstep(%this,%obj)
{
	serverplay3d("huntress_walking" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
}

function PlayerHuntress::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("1.15 1.15 1.15");
	%obj.mountImage("Huntressimage",1);
	%obj.mountImage("meleeAxeLImage",2);
	KillerSpawnMessage(%obj);
}

function PlayerHuntress::EventideAppearance(%this,%obj,%client)
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

	%hoodieColor = "0.35 0 0.13 1";
	%pantsColor = "0.075 0.075 0.075 1";
	%skinColor = "0.83 0.73 0.66 1";

	%obj.setFaceName("huntressface");
	%obj.setDecalName("huntressdecal");
	%obj.setNodeColor("rarm",%hoodieColor);
	%obj.setNodeColor("larm",%hoodieColor);
	%obj.setNodeColor("femchest",%hoodieColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor("rshoe",%pantsColor);
	%obj.setNodeColor("lshoe",%pantsColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	%obj.unhideNode("femchest_blood_front");
	%obj.unhideNode("Lhand_blood");
	%obj.unhideNode("Rhand_blood");
	%obj.unhideNode("lshoe_blood");
	%obj.unhideNode("rshoe_blood");
	%obj.mountImage("Huntressimage",1);
	
	//Set blood colors.
	%obj.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("lhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("rhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("femchest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("femchest_blood_back", "0.7 0 0 1");
}

function PlayerHuntress::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead") %obj.playaudio(0,"huntress_pain" @ getRandom(1, 1) @ "_sound");
}

package Player_Huntress
{

    function ShapeBase::pickup(%obj, %item)
{
        if(%obj.getClassName() $= "Player")
{
            if(%obj.getDataBlock().getName() $= "PlayerHuntress")
{
                return;
            }
        }
        parent::pickup(%obj, %item);
    }
    function Player::addItem(%player, %image, %client)
{
        if(!$Player::PlayerHuntress::NoAddItem)
{
            parent::addItem(%player, %image, %client);
        }
    }
};

$Player::NoItemsPickup::NoAddItem = 0;
activatePackage(Player_Huntress);