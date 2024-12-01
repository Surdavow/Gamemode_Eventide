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

	rightclickicon = "";
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
	%obj.createEmptyFaceConfig($Eventide_FacePacks["lurker"]);
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

function PlayerLurker::EventideAppearance(%this,%obj,%funcclient)
{	
	%obj.hideNode("ALL");
	%obj.unHideNode((%funcclient.chest ? "femChest" : "chest"));	
	%obj.unHideNode((%funcclient.rhand ? "rhook" : "rhand"));
	%obj.unHideNode((%funcclient.lhand ? "lhook" : "lhand"));
	%obj.unHideNode((%funcclient.rarm ? "rarmSlim" : "rarm"));
	%obj.unHideNode((%funcclient.larm ? "larmSlim" : "larm"));
	%obj.unHideNode("headskin");

	if($pack[%funcclient.pack] !$= "none")
	{
		%obj.unHideNode($pack[%funcclient.pack]);
		%obj.setNodeColor($pack[%funcclient.pack],%funcclient.packColor);
	}
	if($secondPack[%funcclient.secondPack] !$= "none")
	{
		%obj.unHideNode($secondPack[%funcclient.secondPack]);
		%obj.setNodeColor($secondPack[%funcclient.secondPack],%funcclient.secondPackColor);
	}

	if(%funcclient.hat)
	{
		%hatName = $hat[%funcclient.hat];
		%funcclient.hatString = %hatName;

		if(%funcclient.hat == 1)
		{
			if(%funcclient.accent) %newhat = "helmet";
			else %newhat = "hoodie1";
			%obj.unHideNode(%newhat);
			%obj.setNodeColor(%newhat,%funcclient.hatColor);
		}
		else
		{
			%obj.unHideNode(%hatName);
			%obj.setNodeColor(%hatName,%funcclient.hatColor);
		}			
	}
	
	if(%funcclient.hip)
	{
		%obj.unHideNode("skirt");
	}
	else
	{
		%obj.unHideNode("pants");
		%obj.unHideNode((%funcclient.rleg ? "rpeg" : "rshoe"));
		%obj.unHideNode((%funcclient.lleg ? "lpeg" : "lshoe"));
	}
	
	%obj.setDecalName(%funcclient.decalName);

	%obj.setNodeColor("headskin",%funcclient.headColor);	
	%obj.setNodeColor("chest",%funcclient.chestColor);
	%obj.setNodeColor("femChest",%funcclient.chestColor);
	%obj.setNodeColor("pants",%funcclient.hipColor);
	%obj.setNodeColor("skirt",%funcclient.hipColor);	
	%obj.setNodeColor("rarm",%funcclient.rarmColor);
	%obj.setNodeColor("larm",%funcclient.larmColor);
	%obj.setNodeColor("rarmSlim",%funcclient.rarmColor);
	%obj.setNodeColor("larmSlim",%funcclient.larmColor);
	%obj.setNodeColor("rhand",%funcclient.rhandColor);
	%obj.setNodeColor("lhand",%funcclient.lhandColor);
	%obj.setNodeColor("rhook",%funcclient.rhandColor);
	%obj.setNodeColor("lhook",%funcclient.lhandColor);	
	%obj.setNodeColor("rshoe",%funcclient.rlegColor);
	%obj.setNodeColor("lshoe",%funcclient.llegColor);
	%obj.setNodeColor("rpeg",%funcclient.rlegColor);
	%obj.setNodeColor("lpeg",%funcclient.llegColor);
	
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