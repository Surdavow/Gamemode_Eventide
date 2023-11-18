datablock PlayerData(PlayerDisfigured : PlayerRenowned) 
{
	uiName = "Disfigured Player";

	killerSpawnMessage = "...";

	killerChaseLvl1Music = "musicData_OUT_DisfiguredNear";
	killerChaseLvl2Music = "musicData_OUT_DisfiguredChase";

	killeridlesound = "disfigured_idle";
	killeridlesoundamount = 5;

	killerchasesound = "disfigured_idle";
	killerchasesoundamount = 5;

	killermeleesound = "disfigured_attack";
	killermeleesoundamount = 3;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
	killerraisearms = false;
	killerlight = "NoFlarePLight";

	rechargeRate = 0.3;
	runForce = "810";
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 7.7;
	maxBackwardSpeed = 4.4;
	maxSideSpeed = 6.6;
	jumpForce = 0;
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
	%obj.mountImage("BleedImage",1);
	%obj.mountImage("FogImage",2);
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

	%dressColor = "0.2 0.2 0.2 1";
	%skinColor = "0.63 0.71 1 1";
	%bloodColor = "0.36 0.07 0.07 1";

	%obj.setFaceName("disfiguredface");
	%obj.setDecalName("disfigureddecal");
	%obj.setNodeColor("rarm",%skinColor);
	%obj.setNodeColor("larmslim",%bloodColor);
	%obj.setNodeColor("femchest",%dressColor);
	%obj.setNodeColor("skirthip",%dressColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	//%obj.unhideNode("femchest_blood_front");
	//%obj.unhideNode("Rhand_blood");
}

function PlayerDisfigured::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead") %obj.playaudio(0,"disfigured_pain" @ getRandom(1, 1) @ "_sound");
}