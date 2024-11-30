datablock PlayerData(PlayerLurker : PlayerRenowned) 
{
	uiName = "Lurker Player";

	killerSpawnMessage = "...";
	
	// Weapon: Fist
	hitprojectile = KillerRoughHitProjectile;
	hitobscureprojectile = "";
	meleetrailskin = "ragged";

	killerChaseLvl1Music = "musicData_Eventide_LurkerNear";
	killerChaseLvl2Music = "musicData_Eventide_LurkerChase";

	killeridlesound = "";
	killeridlesoundamount = 5;

	killerchasesound = "";
	killerchasesoundamount = 5;

	killermeleesound = "";
	killermeleesoundamount = 3;
	
	killerweaponsound = "lurker_weapon";
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
	PlayerCannibal::onTrigger(%this, %obj, %trig, %press);
}

function PlayerLurker::onPeggFootstep(%this,%obj)
{
	serverplay3d("lurker_walking" @ getRandom(1,6) @ "_sound", %obj.getHackPosition());
}

function PlayerLurker::onNewDatablock(%this,%obj)
{
	%obj.createEmptyFaceConfig($Eventide_FacePacks["knight"]);
	%facePack = %obj.faceConfig.getFacePack();
	%obj.faceConfig.face["Neutral"] = %facePack.getFaceData(compileFaceDataName(%facePack, "Neutral"));
	%obj.faceConfig.setFaceAttribute("Neutral", "length", -1);
	
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("1.2 1.2 1.2");
	%obj.isInvisible = false;
	
	if(isObject(%obj.faceConfig))
	%obj.faceConfigShowFaceTimed("Neutral", -1);
}

function PlayerLurker::EventideAppearance(%this,%obj,%client)
{	
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