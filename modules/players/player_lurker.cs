datablock TSShapeConstructor(LurkerDts)
{
	baseShape = "./models/lurker/player/m.dts";
	sequence0 = "./models/lurker/player/m_root.dsq root";
	sequence1 = "./models/lurker/player/m_run.dsq run";
	sequence2 = "./models/lurker/player/m_run.dsq walk";
	sequence3 = "./models/lurker/player/m_back.dsq back";
	sequence4 = "./models/lurker/player/m_side.dsq side";
	sequence5 = "./models/lurker/player/m_crouch.dsq crouch";
	sequence6 = "./models/lurker/player/m_crouchRun.dsq crouchRun";
	sequence7 = "./models/lurker/player/m_crouchBack.dsq crouchBack";
	sequence8 = "./models/lurker/player/m_crouchSide.dsq crouchSide";
	sequence9 = "./models/lurker/player/m_look.dsq look";
	sequence10 = "./models/lurker/player/m_headSide.dsq headside";
	sequence11 = "./models/lurker/player/m_headup.dsq headUp";
	sequence12 = "./models/lurker/player/m_standjump.dsq jump";
	sequence13 = "./models/lurker/player/m_standjump.dsq standjump";
	sequence14 = "./models/lurker/player/m_fall.dsq fall";
	sequence15 = "./models/lurker/player/m_root.dsq land";
	sequence16 = "./models/lurker/player/m_armAttack.dsq armAttack";
	sequence17 = "./models/lurker/player/m_armReadyLeft.dsq armReadyLeft";
	sequence18 = "./models/lurker/player/m_armReadyRight.dsq armReadyRight";
	sequence19 = "./models/lurker/player/m_armReadyBoth.dsq armReadyBoth";
	sequence20 = "./models/lurker/player/m_spearReady.dsq spearready";
	sequence21 = "./models/lurker/player/m_spearThrow.dsq spearThrow";
	sequence22 = "./models/lurker/player/m_talk.dsq talk";
	sequence23 = "./models/lurker/player/m_death1.dsq death1";
	sequence24 = "./models/lurker/player/m_shiftUp.dsq shiftUp";
	sequence25 = "./models/lurker/player/m_shiftDown.dsq shiftDown";
	sequence26 = "./models/lurker/player/m_shiftAway.dsq shiftAway";
	sequence27 = "./models/lurker/player/m_shiftTo.dsq shiftTo";
	sequence28 = "./models/lurker/player/m_shiftLeft.dsq shiftLeft";
	sequence29 = "./models/lurker/player/m_shiftRight.dsq shiftRight";
	sequence30 = "./models/lurker/player/m_rotCW.dsq rotCW";
	sequence31 = "./models/lurker/player/m_rotCCW.dsq rotCCW";
	sequence32 = "./models/lurker/player/m_undo.dsq undo";
	sequence33 = "./models/lurker/player/m_plant.dsq plant";
	sequence34 = "./models/lurker/player/m_sit.dsq sit";
	sequence35 = "./models/lurker/player/m_wrench.dsq wrench";
	sequence36 = "./models/lurker/player/m_activate.dsq activate";
	sequence37 = "./models/lurker/player/m_activate2.dsq activate2";
	sequence38 = "./models/lurker/player/m_leftrecoil.dsq leftrecoil";
};

datablock PlayerData(PlayerLurker : PlayerRenowned) 
{
	isEventideModel = true;
	shapeFile = "./models/lurker/player/m.dts";
	useCustomPainEffects = true;
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
function servercmdheed(%c,%t){%c.player.setArmThread(%t);}
function servercmdthread(%c,%t,%s){%c.player.playThread(%s,%t);}
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
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);	
	%obj.setScale("1 1 1");
	%obj.isInvisible = false;
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