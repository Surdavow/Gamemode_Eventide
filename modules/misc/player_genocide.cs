datablock PlayerData(PlayerGenocide : PlayerRenowned) 
{
	uiName = "Genocide Player";

	killerSpawnMessage = "...";

	killerChaseLvl1Music = "musicData_OUT_GenocideNear";
	killerChaseLvl2Music = "musicData_OUT_GenocideChase";

	killeridlesound = "genocide_idle";
	killeridlesoundamount = 18;

	killerchasesound = "genocide_chase";
	killerchasesoundamount = 18;

	killermeleesound = "genocide_attack";
	killermeleesoundamount = 5;	

	killermeleehitsound = "genocide_hit";
	killermeleehitsoundamount = 3;
	
	killerraisearms = false;
	killerlight = "NoFlarePLight";

	rechargeRate = 0.3;
	maxTools = 1;
	maxWeapons = 1;
	maxForwardSpeed = 7.35;
	maxBackwardSpeed = 4.2;
	maxSideSpeed = 6.3;
	jumpForce = 0;
};

function PlayerGenocide::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	switch(%trig)
	{
		case 0: if(%press) %obj.KillerMelee(%this,3.5);
	}
}

function PlayerGenocide::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("1 1 1");
	%obj.mountImage("Genocideimage",2);

	%randomitem[%i++] = "mine_IncendiaryImage";
    %randomitem[%i++] = "CatARImage";
    %randomitem[%i++] = "PeeingItemImage";
    %randomitem[%i++] = "PetitionImage";
    %randomitem[%i++] = "bookItem";
    %randomitem[%i++] = "grenade_flashbangImage";
    %randomitem[%i++] = "grenade_riotImage";
	
	%obj.mountImage(%randomitem[getRandom(1,%i)],0);
	KillerSpawnMessage(%obj);
}

function PlayerGenocide::EventideAppearance(%this,%obj,%client)
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

	%shirtColor = "0.22 0.28 0.28 1";
	%pantsColor = "0.06 0.08 0.2 1";
	%skinColor = "0.83 0.73 0.66 1";
	%shoesColor = "0.1 0.08 0.07 1";

	%obj.setFaceName("smiley");
	%obj.setDecalName("genocidedecal");
	%obj.setNodeColor("rarm",%shirtColor);
	%obj.setNodeColor("larm",%shirtColor);
	%obj.setNodeColor("chest",%shirtColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor("rshoe",%shoesColor);
	%obj.setNodeColor("lshoe",%shoesColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	%obj.unhideNode("chest_blood_front");
	%obj.unhideNode("Lhand_blood");
	%obj.unhideNode("Rhand_blood");
	%obj.unhideNode("lshoe_blood");
	%obj.unhideNode("rshoe_blood");
	%obj.mountImage("Genocideimage",1);
	
	//Set blood colors.
	%obj.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("lhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("rhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_back", "0.7 0 0 1");
}

function PlayerGenocide::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead") %obj.playaudio(0,"genocide_pain" @ getRandom(1, 11) @ "_sound");
}