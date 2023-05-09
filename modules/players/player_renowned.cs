datablock PlayerData(PlayerRenowned : EventidePlayer) 
{
	uiName = "Renowned Player";	

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
	firstpersononly = true;

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

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
};

function PlayerRenowned::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	
	%obj.mountImage("meleeTantoImage",0);
	%obj.schedule(1, setEnergyLevel, 0);
	%obj.setScale("1.05 1.05 1.05");
}

function PlayerRenowned::EventideAppearance(%this,%obj,%client)
{
	Parent::EventideAppearance(%this,%obj,%client);
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
	%obj.setNodeColor("Rhook",%skinColor);
	%obj.setNodeColor("Lhook",%skinColor);
	%obj.hideNode($hat[%client.hat]);
	%obj.HideNode($accent[%client.accent]);
	%obj.HideNode($secondPack[%client.secondPack]);
	%obj.hideNode($pack[%client.pack]);
	%obj.HideNode("visor");
	%obj.HideNode("rpeg");
	%obj.HideNode("lpeg");
	%obj.unHideNode("rshoe");
	%obj.unHideNode("lshoe");
	%obj.unhideNode("renownedeyes");
	%obj.setHeadUp(0);
}

function PlayerRenowned::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%trig == 0 && %press) %obj.KillerMelee(%this,3.5);
	
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

				if(isObject(%search) && Eventide_MinigameConditionalCheck(%obj,%search,false))
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