datablock PlayerData(PlayerRenowned : EventidePlayer) 
{
	uiName = "Renowned Player";	

	killerSpawnMessage = "A droll of the mind arrives with death in close pursuit.";
	
	// Weapon: Katana
	hitprojectile = KillerSharpHitProjectile;
	hitobscureprojectile = KillerKatanaClankProjectile;	
	meleetrailskin = "base";
	meleetrailoffset = "0.3 1.4 0.7"; 	
	meleetrailangle1 = "0 90 0";
	meleetrailangle2 = "0 -90 0";
	meleetrailangle3 = "0 0 0";
	meleetrailangle4 = "0 180 0";
	meleetrailscale = "4 4 2";		

	rechargeRate = 0.26;
	maxDamage = 9999;
	maxTools = 0;
	maxWeapons = 0;
	maxForwardSpeed = 7.32;
	maxBackwardSpeed = 4.18;
	maxSideSpeed = 6.27;
	useCustomPainEffects = true;
	jumpSound = "";
	PainSound		= "";
	DeathSound		= "";
	renderFirstPerson = false;
	firstpersononly = true;
	showenergybar = true;
	jumpForce = 0;

	isKiller = true;
	killerraisearms = false;
	killerlight = "NoFlareYLight";	

	killerChaseLvl1Music = "musicData_OUT_RenownedNear";
	killerChaseLvl2Music = "musicData_OUT_RenownedChase";

	killeridlesound = "renowned_idle";
	killeridlesoundamount = 8;

	killerchasesound = "renowned_chase";
	killerchasesoundamount = 6;

	killermeleesound = "renowned_melee";
	killermeleesoundamount = 3;
	
	killerweaponsound = "renowned_weapon";
	killerweaponsoundamount = 5;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
};

function PlayerRenowned::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	
	%obj.mountImage("meleeTantoImage",0);
	%obj.schedule(1, setEnergyLevel, 0);
	%obj.setScale("1.05 1.05 1.05");
	%obj.mountImage("renownedeyesimage",2);
	KillerSpawnMessage(%obj);
}

function PlayerRenowned::EventideAppearance(%this,%obj,%client)
{
	if(%obj.isSkinwalker && isObject(%obj.victimreplicatedclient)) %funcclient = %obj.victimreplicatedclient;
    else %funcclient = %client;	
	
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
	
	%obj.unHideNode("pants");
	%obj.unHideNode((%funcclient.rleg ? "rpeg" : "rshoe"));
	%obj.unHideNode((%funcclient.lleg ? "lpeg" : "lshoe"));

	%obj.setHeadUp(0);
	if(%funcclient.pack+%funcclient.secondPack > 0) %obj.setHeadUp(1);

	if (%obj.bloody["lshoe"]) %obj.unHideNode("lshoe_blood");
	if (%obj.bloody["rshoe"]) %obj.unHideNode("rshoe_blood");
	if (%obj.bloody["lhand"]) %obj.unHideNode("lhand_blood");
	if (%obj.bloody["rhand"]) %obj.unHideNode("rhand_blood");
	if (%obj.bloody["chest_front"]) %obj.unHideNode((%funcclient.chest ? "fem" : "") @ "chest_blood_front");
	if (%obj.bloody["chest_back"]) %obj.unHideNode((%funcclient.chest ? "fem" : "") @ "chest_blood_back");

	%obj.setFaceName(%funcclient.faceName);
	%obj.setDecalName(%funcclient.decalName);

	%obj.setNodeColor("headskin",%funcclient.headColor);	
	%obj.setNodeColor("chest",%funcclient.chestColor);
	%obj.setNodeColor("femChest",%funcclient.chestColor);
	%obj.setNodeColor("pants",%funcclient.hipColor);
	%obj.setNodeColor("rarm",%funcclient.rarmColor);
	%obj.setNodeColor("larm",%funcclient.larmColor);
	%obj.setNodeColor("rarmSlim",%funcclient.rarmColor);
	%obj.setNodeColor("larmSlim",%funcclient.larmColor);
	%obj.setNodeColor("rhand",%funcclient.rhandColor);
	%obj.setNodeColor("lhand",%funcclient.lhandColor);
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
	%obj.setNodeColor("femchest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("femchest_blood_back", "0.7 0 0 1");
	
	%obj.setFaceName("renownedface");
	%obj.setDecalName("renowneddecal");
	
	%skinColor = "0.83 0.73 0.66 1";

	if(%obj.chest)
	{
		%obj.hideNode("femchest");
		%obj.unhideNode("chest");		
	}	

	%obj.setNodeColor("headskin",%skinColor);
	%obj.setNodeColor("Rhand",%skinColor);
	%obj.setNodeColor("Lhand",%skinColor);
	%obj.hideNode($hat[%client.hat]);
	%obj.HideNode($accent[%client.accent]);
	%obj.HideNode($secondPack[%client.secondPack]);
	%obj.hideNode($pack[%client.pack]);
	%obj.HideNode("visor");
	%obj.HideNode("rpeg");
	%obj.HideNode("lpeg");
	%obj.unHideNode("rshoe");
	%obj.unHideNode("lshoe");
	%obj.mountImage("renownedeyesimage",2);
	%obj.setHeadUp(0);

	%obj.unHideNode("renownedeyes");
	//%obj.setNodeColor("renownedeyes","1 1 1 1");
}

function PlayerRenowned::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%trig == 0 && %press) %obj.KillerMelee(%this,4);			
	
	if(%trig == 4 && %obj.getEnergyLevel() == %this.maxEnergy)
	{
		if(%press)
		{
			%obj.playthread(2,"armReadyLeft");
			%obj.casttime = getSimTime();
			%obj.channelcasthand = %obj.schedule(500, setNodeColor, lHand, "0.8 0.8 0.5 1");
			%obj.channelcasthandimage = %obj.schedule(500,mountImage,"RenownedCastImage",2);
		}
		else
		{
			%obj.unmountImage(2);
			cancel(%obj.channelcasthand);
			cancel(%obj.channelcasthandimage);
			%this.EventideAppearance(%obj,%obj.client);
	
			if(%obj.casttime+500 < getSimTime())
			{								
				%start = %obj.getEyePoint();
				%end = VectorAdd(%start,VectorScale(%obj.getEyeVector(),getWord(%obj.getScale(),2)*40));
				%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::ItemObjectType;
				%search = containerRayCast (%start, %end, %mask, %obj);

				if(isObject(%search) && minigameCanDamage(%obj,%search))
				{
					%obj.client.setControlObject(%search);
					%obj.returnObserveSchedule = %obj.schedule(4000,ClearRenownedEffect);

					%search.client.centerprint("<color:FFFFFF><font:Impact:40>You are being controlled, press E to break free!",2);
					%search.Possesser = %obj;
					%search.isPossessed = true;
					%obj.setEnergyLevel(0);
					%obj.playthread(2,"leftrecoil");
					%search.mountImage("RenownedPossessedImage",3);
					%search.schedule(4000,ClearRenownedEffect);
				}
				else %obj.setEnergyLevel(%obj.getEnergyLevel()-50);
			}		
		}
	}
	else if(%press && %obj.getEnergyLevel() < 20) %obj.playthread(0,"undo");
}

function Player::ClearRenownedEffect(%obj)
{
	if(!isObject(%obj) || !(%obj.getType() & $TypeMasks::PlayerObjectType)) return;
	
	%obj.AntiPossession = "";
	%obj.Possesser = "";
	%obj.isPossessed = "";
	%obj.unMountImage(3);
	serverPlay3d("renowned_spellBreak_sound", %obj.getPosition());

	switch$(%obj.getclassname())
	{
		case "Player": 	%obj.client.setControlObject(%obj);
						%obj.client.camera.setMode("Observer");
		case "AIPlayer": %obj.setControlObject(%obj);
	}
}