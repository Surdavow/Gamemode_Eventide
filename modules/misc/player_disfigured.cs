datablock PlayerData(PlayerDisfigured : PlayerRenowned) 
{
	uiName = "Disfigured Player";

	killerSpawnMessage = "...";

	killerChaseLvl1Music = "musicData_OUT_DisfiguredNear";
	killerChaseLvl2Music = "musicData_OUT_DisfiguredChase";

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

function PlayerDisfigured::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	switch(%trig)
	{
		case 0: if(%press) %obj.KillerMelee(%this,3.5);
	}
}

function PlayerDisfigured::onPeggFootstep(%this,%obj)
{
	serverplay3d("huntress_walking" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
}

function PlayerDisfigured::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("1.15 1.15 1.15");
	%obj.mountImage("smallBleedImage",1);
	KillerSpawnMessage(%obj);
}

function PlayerDisfigured::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");	
	%obj.unhideNode("skirthip");
	%obj.unhideNode("headskin");
	%obj.unhideNode("larmslim");
	%obj.unhideNode("rarm");
	%obj.unhideNode("skirttrimr");
	%obj.unhideNode("skirttriml");
	%obj.unhideNode("rhand");
	%obj.unhideNode("femchest");

	%dressColor = "0.50 0.38 0.27 1";
	%skinColor = "0.7 1 0.63 1";
	%bloodColor = "0.36 0.07 0.07 1";

	%obj.setFaceName("disfiguredface");
	%obj.setDecalName("disfigureddecal");
	%obj.setNodeColor("rarm",%skinColor);
	%obj.setNodeColor("larmslim",%bloodColor);
	%obj.setNodeColor("femchest",%dressColor);
	%obj.setNodeColor("skirthip",%dressColor);
	%obj.setNodeColor("skirttrimr",%dressColor);
	%obj.setNodeColor("skirttriml",%dressColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	//%obj.unhideNode("femchest_blood_front");
	//%obj.unhideNode("Rhand_blood");
}

function PlayerDisfigured::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead") %obj.playaudio(0,"huntress_pain" @ getRandom(1, 1) @ "_sound");
}