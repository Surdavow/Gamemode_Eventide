datablock TSShapeConstructor(PuppetMasterPuppetDTS) 
{
	baseShape = "./models/puppet.dts";
	sequence0 = "./models/default.dsq";
};

datablock PlayerData(PuppetMasterPuppet : PlayerRenowned)
{
	uiName = "";
	shapeFile = PuppetMasterPuppetDTS.baseShape;

	killerHitProjectile = KillerHitProjectile;	
	killermeleesound = "puppetmasterpuppet_idle";
	killermeleesoundamount = 1;	
	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;

	rechargeRate = 0.35;
	runForce = 5616;
	maxForwardSpeed = 10.47;
	maxBackwardSpeed = 5.98;
	maxSideSpeed = 8.58;
	maxDamage = 50;
	showenergybar = true;
};

function PuppetMasterPuppet::onNewDatablock(%this,%obj)
{
	%obj.setScale("0.7 0.7 0.7");
	%obj.schedule(1,setEnergyLevel,100);
	%obj.mountImage("meleeKnifeImage",0);
	%this.EventideAppearance(%obj,ClientGroup.getObject(getRandom(0,ClientGroup.getCount()-1)));
}

function PuppetMasterPuppet::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
			
	switch(%trig)
	{
		case 0: %this.killerMelee(%obj,3.5);
		
		case 4: if(%obj.getEnergyLevel() == %this.maxEnergy)
				if(%press)
				{
					%obj.casttime = getSimTime();
					%obj.chargejumpsound = %obj.schedule(33,playaudio,1,"puppet_jumpCharge_sound");
				}
				else
				{
					cancel(%obj.chargejumpsound);
					
					if(%obj.casttime+350 < getSimTime())
					{
						%obj.setEnergyLevel(0);
						%obj.playthread(3,"rightrecoil");
						serverPlay3d("puppet_jump_sound", %obj.getEyePoint());
						%obj.setVelocity(vectorscale(%obj.getEyeVector(),16));
					}
				}
		default:
	}
}

function PuppetMasterPuppet::onImpact(%this, %obj, %col, %vec, %force)
{
	return;
}

function PuppetMasterPuppet::onDisabled(%this,%obj)
{
	Parent::onDisabled(%this,%obj);
	
	%obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");
	%obj.sourceclient.setcontrolobject(%obj.source);
	%obj.source.mountimage("sm_stunImage",2);
	%obj.delete();
}

function PuppetMasterPuppet::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");
	%obj.unHideNode((%client.chest ? "femChestpuppet" : "chestpuppet"));	
	%obj.unHideNode((%client.rhand ? "rhook" : "rhandpuppet"));
	%obj.unHideNode((%client.lhand ? "lhook" : "lhandpuppet"));
	%obj.unHideNode((%client.rarm ? "rarmSlim" : "rarm"));
	%obj.unHideNode((%client.larm ? "larmSlim" : "larm"));
	%obj.unHideNode("headpuppet");
	%obj.unHideNode("buttoneyes");

	if($pack[%client.pack] !$= "none")
	{
		%obj.unHideNode($pack[%client.pack]);
		%obj.setNodeColor($pack[%client.pack],%client.packColor);
	}
	if($secondPack[%client.secondPack] !$= "none")
	{
		%obj.unHideNode($secondPack[%client.secondPack]);
		%obj.setNodeColor($secondPack[%client.secondPack],%client.secondPackColor);
	}
	if($hat[%client.hat] !$= "none")
	{
		%obj.unHideNode($hat[%client.hat]);
		%obj.setNodeColor($hat[%client.hat],%client.hatColor);
	}
	if(%client.hip)
	{
		%obj.unHideNode("skirthip");
		%obj.unHideNode("skirttrimleft");
		%obj.unHideNode("skirttrimright");
	}
	else
	{
		%obj.unHideNode("pantspuppet");
		%obj.unHideNode((%client.rleg ? "rpeg" : "rshoe"));
		%obj.unHideNode((%client.lleg ? "lpeg" : "lshoe"));
	}

	%obj.setHeadUp(0);
	if(%client.pack+%client.secondPack > 0) %obj.setHeadUp(1);
	if($hat[%client.hat] $= "Helmet")
	{
		if(%client.accent == 1 && $accent[4] !$= "none")
		{
			%obj.unHideNode($accent[4]);
			%obj.setNodeColor($accent[4],%client.accentColor);
		}
	}
	else if($accent[%client.accent] !$= "none" && strpos($accentsAllowed[$hat[%client.hat]],strlwr($accent[%client.accent])) != -1)
	{
		%obj.unHideNode($accent[%client.accent]);
		%obj.setNodeColor($accent[%client.accent],%client.accentColor);
	}

	%obj.setFaceName(%client.faceName);
	%obj.setDecalName(%client.decalName);

	%obj.setNodeColor("headpuppet",%client.headColor);	
	%obj.setNodeColor("chestpuppet",%client.chestColor);
	%obj.setNodeColor("femChestpuppet",%client.chestColor);
	%obj.setNodeColor("pantspuppet",%client.hipColor);
	%obj.setNodeColor("skirthip",%client.hipColor);	
	%obj.setNodeColor("rarm",%client.rarmColor);
	%obj.setNodeColor("larm",%client.larmColor);
	%obj.setNodeColor("rarmSlim",%client.rarmColor);
	%obj.setNodeColor("larmSlim",%client.larmColor);
	%obj.setNodeColor("rhandpuppet",%client.rhandColor);
	%obj.setNodeColor("lhandpuppet",%client.lhandColor);
	%obj.setNodeColor("rhook",%client.rhandColor);
	%obj.setNodeColor("lhook",%client.lhandColor);	
	%obj.setNodeColor("rshoe",%client.rlegColor);
	%obj.setNodeColor("lshoe",%client.llegColor);
	%obj.setNodeColor("rpeg",%client.rlegColor);
	%obj.setNodeColor("lpeg",%client.llegColor);
	%obj.setNodeColor("skirttrimright",%client.rlegColor);
	%obj.setNodeColor("skirttrimleft",%client.llegColor);
	%obj.setNodeColor("buttoneyes","0.1 0.1 0.1 1");
}