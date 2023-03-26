if(isFile("add-ons/Gamemode_Eventide/modules/players/models/face.ifl"))//Faces
{
	%write = new FileObject();
	%write.openForWrite("add-ons/Gamemode_Eventide/modules/players/models/face.ifl");
	%write.writeLine("base/data/shapes/player/faces/smiley.png");
	%write.writeLine("Add-Ons/Face_Default/smileyRedBeard2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyRedBeard.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate3.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyPirate1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyOld.png");
	%write.writeLine("Add-Ons/Face_Default/smileyFemale1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyEvil2.png");
	%write.writeLine("Add-Ons/Face_Default/smileyEvil1.png");
	%write.writeLine("Add-Ons/Face_Default/smileyCreepy.png");
	%write.writeLine("Add-Ons/Face_Default/smileyBlonde.png");
	%write.writeLine("Add-Ons/Face_Default/memeYaranika.png");
	%write.writeLine("Add-Ons/Face_Default/memePBear.png");
	%write.writeLine("Add-Ons/Face_Default/memeHappy.png");
	%write.writeLine("Add-Ons/Face_Default/memeGrinMan.png");
	%write.writeLine("Add-Ons/Face_Default/memeDesu.png");
	%write.writeLine("Add-Ons/Face_Default/memeCats.png");
	%write.writeLine("Add-Ons/Face_Default/memeBlockMongler.png");
	%write.writeLine("Add-Ons/Face_Default/asciiTerror.png");
	
	%decalpath = "add-ons/Gamemode_Eventide/modules/players/models/faces/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		eval("addExtraResource(\""@ %decalfile @ "\");");
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource("add-ons/Gamemode_Eventide/modules/players/models/face.ifl");
}

if(isFile("add-ons/Gamemode_Eventide/modules/players/models/decal.ifl"))//Decals
{
	%write = new FileObject();
	%write.openForWrite("add-ons/Gamemode_Eventide/modules/players/models/decal.ifl");
	%write.writeLine("base/data/shapes/players/decals/AAA-none.png");
	%write.writeLine("Add-Ons/Decal_WORM/worm_engineer.png");
	%write.writeLine("Add-Ons/Decal_WORM/worm-sweater.png");
	%write.writeLine("Add-Ons/Decal_PlayerFitNE/zhwindnike.png");
	%write.writeLine("Add-Ons/Decal_Jirue/LinkTunic.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Knight.png");
	%write.writeLine("Add-Ons/Decal_Jirue/HCZombie.png");
	%write.writeLine("Add-Ons/Decal_Jirue/DrKleiner.png");
	%write.writeLine("Add-Ons/Decal_Jirue/DKnight.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Chef.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Archer.png");
	%write.writeLine("Add-Ons/Decal_Jirue/Alyx.png");
	%write.writeLine("Add-Ons/Decal_Hoodie/Hoodie.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-Old.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-New.png");
	%write.writeLine("Add-Ons/Decal_Default/Space-Nasa.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Suit.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Prisoner.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Police.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Pilot.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-DareDevil.png");
	%write.writeLine("Add-Ons/Decal_Default/Mod-Army.png");
	%write.writeLine("Add-Ons/Decal_Default/Meme-Mongler.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-YARLY.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Tunic.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Rider.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-ORLY.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Lion.png");
	%write.writeLine("Add-Ons/Decal_Default/Medieval-Eagle.png");
	
	%decalpath = "add-ons/Gamemode_Eventide/modules/players/models/decals/*.png";
	for(%decalfile = findFirstFile(%decalpath); %decalfile !$= ""; %decalfile = findNextFile(%decalpath))
	{
		eval("addExtraResource(\""@ %decalfile @ "\");");
		%write.writeLine(%decalfile);
	}

	%write.close();
	%write.delete();
	addExtraResource("add-ons/Gamemode_Eventide/modules/players/models/decal.ifl");
}

datablock TSShapeConstructor(EventideplayerDts) {
	baseShape = "./models/Eventideplayer.dts";
	sequence0 = "./models/default.dsq";
};

datablock TSShapeConstructor(SkinwalkerDTS)
{
    baseShape  = "./models/skinwalker.dts";
    sequence0  = "./models/skinwalker.dsq";
};

datablock PlayerData(EventidePlayer : PlayerStandardArmor)
{
	shapeFile = EventideplayerDts.baseShape;
	uiName = "Eventide Player";
	uniformCompatible = true;//For slayer uniform compatibility
	isEventideModel = true;
	showEnergyBar = true;
	canJet = false;
	rechargeRate = 0.375;
	maxTools = 3;
	maxWeapons = 3;
	jumpDelay = 31;
	jumpForce = 10 * 85;
	cameramaxdist = 2.1;
	maxfreelookangle = 2.25;
};

datablock PlayerData(EventidePlayerDowned : EventidePlayer)
{	
	maxForwardSpeed = 0;
   	maxBackwardSpeed = 0;
   	maxSideSpeed = 0;
   	maxForwardCrouchSpeed = 0;
   	maxBackwardCrouchSpeed = 0;
   	maxSideCrouchSpeed = 0;
   	jumpForce = 0;
	rechargerate = 0;
	isDowned = true;
	uiName = "";
};

datablock PlayerData(PlayerRenowned : EventidePlayer) 
{
	uiName = "Renowned Player";	

	rechargeRate = 0.26;
	maxDamage = 9999;
	maxTools = 1;
	maxWeapons = 1;
	maxForwardSpeed = 6.3;
	maxBackwardSpeed = 3.6;
	maxSideSpeed = 5.4;
	useCustomPainEffects = true;
	jumpSound = "";
	PainSound		= "";
	DeathSound		= "";	
	firstpersononly = true;
	isKiller = true;

	killerChaseLvl1Music = "musicData_OUT_RenownedNear";
	killerChaseLvl2Music = "musicData_OUT_RenownedChase";
	killeridlesound = "renowned_idle";
	killeridlesoundamount = 8;
	killerchasesound = "renowned_chase";
	killerchasesoundamount = 6;
	killerraisearms = false;
	killerlight = "NoFlareYLight";
};

datablock PlayerData(PlayerSkullWolf : PlayerRenowned) 
{
	uiName = "Skullwolf Player";

	killerchaselvl1music = "musicData_OUT_SkullWolfNear";
	killerchaselvl2music = "musicData_OUT_SkullWolfChase";
	killeridlesound = "skullwolf_idle";
	killeridlesoundamount = 12;
	killerchasesound = "skullwolf_chase";
	killerchasesoundamount = 4;
	killerraisearms = true;
	killerlight = "NoFlareRLight";

	rechargeRate = 0.25;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 6.65;
	maxBackwardSpeed = 3.8;
	maxSideSpeed = 5.7;
	boundingBox = "4.8 4.8 10.1";
	crouchBoundingBox = "4.8 4.8 3.8";
};

datablock PlayerData(PlayerShire : PlayerRenowned) 
{
	uiName = "Shire Player";

	killerChaseLvl1Music = "musicData_OUT_ShireNear";
	killerChaseLvl2Music = "musicData_OUT_ShireChase";
	killeridlesound = "shire_idle";
	killeridlesoundamount = 9;
	killerchasesound = "shire_chase";
	killerchasesoundamount = 4;
	killerraisearms = false;
	killerlight = "NoFlarePLight";

	rechargeRate = 0.3;
	maxTools = 1;
	maxWeapons = 1;
};

datablock PlayerData(PlayerAngler : PlayerRenowned) 
{
	uiName = "Angler Player";
	rechargeRate = 0.215;
	maxTools = 0;
	maxWeapons = 0;
	jumpForce = 10 * 75;
	jumpDelay = 32;
	maxForwardSpeed = 6.3;
	maxBackwardSpeed = 3.6;
	maxSideSpeed = 5.4;
	boundingBox = "4.5 4.5 9.5";
	crouchBoundingBox = "4.5 4.5 3.6";

	killerChaseLvl1Music = "musicData_OUT_AnglerNear";
	killerChaseLvl2Music = "musicData_OUT_AnglerChase";
	killeridlesound = "angler_idle";
	killeridlesoundamount = 7;
	killerchasesound = "angler_chase";
	killerchasesoundamount = 3;
	killerraisearms = true;
	killerlight = "NoFlareBLight";
};

datablock PlayerData(PlayerGrabber : PlayerRenowned) 
{
	uiName = "Grabber Player";

	killerChaseLvl1Music = "musicData_OUT_GrabberNear";
	killerChaseLvl2Music = "musicData_OUT_GrabberChase";
	killeridlesound = "";
	killeridlesoundamount = 0;
	killerchasesound = "";
	killerchasesoundamount = 0;
	killerraisearms = false;
	killerlight = "NoFlareRLight";	

	firstpersononly = false;
	rechargeRate = 0.65;
	maxTools = 1;
	maxWeapons = 1;
	maxForwardSpeed = 6.65;
	maxBackwardSpeed = 3.8;
	maxSideSpeed = 5.7;
	cameramaxdist = 3;
	maxfreelookangle = 2.5;
	boundingBox = "4.8 4.8 10.1";
	crouchBoundingBox = "4.8 4.8 3.8";
};

datablock PlayerData(PlayerGrabberNoJump : PlayerGrabber) 
{
	uiName = "";
	jumpForce = 0;
};

datablock PlayerData(PlayerSkinwalker : PlayerStandardArmor)
{
	isEventideModel = true;
	isKiller = true;
    firstPersonOnly = true;
	showEnergyBar = true;
	canJet = false;	
	useCustomPainEffects = true;

	shapeFile = SkinwalkerDTS.baseShape;
	
	killerChaseLvl1Music = "musicData_OUT_SkinwalkerNear";
	killerChaseLvl2Music = "musicData_OUT_SkinwalkerChase";
	killeridlesound = "";
	killeridlesoundamount = 0;
	killerchasesound = "";
	killerchasesoundamount = 0;
	killerraisearms = true;
	killerlight = "NoFlareRLight";	

	PainSound = "skinwalker_pain_sound";
	DeathSound = "skinwalker_death_sound";
	JumpSound = "JumpSound";
	uiName = "Skinwalker Player";	
	
	rechargeRate = 0.375;	
	maxDamage = 9999;
	jumpDelay = 31;
	jumpForce = 10 * 80;
	maxForwardSpeed = 6.3;
	maxBackwardSpeed = 3.6;
	maxSideSpeed = 5.4;
	boundingBox = "4.5 4.5 9.5";
	crouchBoundingBox = "4.5 4.5 3.6";
	maxItems   = 0;
	maxWeapons = 0;
	maxTools = 0;	
};

datablock PlayerData(ShireZombieBot : EventidePlayer)
{
	uiName = "";
	isKiller = true;
};