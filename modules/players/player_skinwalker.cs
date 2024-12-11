datablock TSShapeConstructor(SkinwalkerDTS)
{
    baseShape  = "./models/skinwalker.dts";
    sequence0  = "./models/skinwalker.dsq";
    sequence1  = "./models/skinwalker_melee.dsq";
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
	killerraisearms = true;
	killerlight = "NoFlareRLight";

	renderFirstPerson = false;
	
	// Weapon: Claws
	HitProjectile = KillerRoughHitProjectile;
	hitobscureprojectile = "";
	
	meleetrailskin = "raggedClaw";
	meleetrailoffset = "0.9 1.4 0.7"; 	
	meleetrailangle1 = "0 90 0";
	meleetrailangle2 = "0 -90 0";
	meleetrailscale = "4 4 3";	

	rightclickicon = "color_skinwalker_reveal";
	leftclickicon = "color_melee";
	rightclickspecialicon = "";
	leftclickspecialicon = "color_consume";
	
	killerChaseLvl1Music = "musicData_Eventide_SkinwalkerNear";
	killerChaseLvl2Music = "musicData_Eventide_SkinwalkerChase";
	
	killeridlesound = "skinwalker_idle";
	killeridlesoundamount = 5;
	
	killerchasesound = "skinwalker_attack";
	killerchasesoundamount = 5;

	killermeleesound = "skinwalker_chase";
	killermeleesoundamount = 3;    

	killerweaponsound = "skinwalker_weapon";
	killerweaponsoundamount = 4;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;    

	PainSound = "skinwalker_pain_sound";
	DeathSound = "";
	JumpSound = "";
	uiName = "Skinwalker Player";
	jumpForce = 0;
	
	rechargeRate = 0.375;	
	maxDamage = 9999;
	maxForwardSpeed = 6.16;
	maxBackwardSpeed = 3.52;
	maxSideSpeed = 5.28;
	boundingBox = "4.5 4.5 9.5";
	crouchBoundingBox = "4.5 4.5 3.6";
};

function PlayerSkinwalker::killerGUI(%this,%obj,%client)
{	
	%energylevel = %obj.getEnergyLevel();

	// Some dynamic varirables
	%leftclickstatus = (%obj.getEnergyLevel() >= 25) ? "hi" : "lo";
	%rightclickstatus = (%obj.getEnergyLevel() == %this.maxEnergy) ? "hi" : "lo";
	%leftclicktext = (%this.leftclickicon !$= "") ? "<just:left>\c6Left click" : "";
	%rightclicktext = (%this.rightclickicon !$= "") ? "<just:right>\c6Right click" : "";

	// Regular icons
	%leftclickicon = (%this.leftclickicon !$= "") ? "<just:left><bitmap:" @ $iconspath @ %leftclickstatus @ %this.leftclickicon @ ">" : "";
	%rightclickicon = (%this.rightclickicon !$= "") ? "<just:right><bitmap:" @ $iconspath @ %rightclickstatus @ %This.rightclickicon @ ">" : "";

	// Change them to special if they exist
	if(%obj.getEnergyLevel() >= 25 && %this.leftclickspecialicon !$= "" && isObject(%obj.gazing) && %obj.gazing.getdataBlock().isDowned)
	{		
		%leftclickstatus = (%obj.gazing.getDamagePercent() > 0.05) ? "hi" : "lo";
		%leftclickicon = "<just:left><bitmap:" @ $iconspath @ %leftclickstatus @ %this.leftclickspecialicon @ ">";
	}

	%client.bottomprint(%leftclicktext @ %rightclicktext @ "<br>" @ %leftclickicon @ %rightclickicon, 1);
}

function PlayerSkinwalker::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);

	%obj.setScale("1.2 1.2 1.2");
    %obj.playthread(0,"roar");    
    %obj.playaudio(0,"skinwalker_roar_sound");
    %obj.schedule(1, setEnergyLevel,0);
	for(%i = 0; %i < getRandom(1,2); %i++) %obj.schedule(50,spawnExplosion,"goryExplosionProjectile",%obj.getScale());   
    %obj.isSkinwalker = true;
}

function PlayerSkinwalker::EventideAppearance(%this,%obj,%client)
{
    %clientappearance = isObject(%obj.victimreplicatedclient) ? %obj.victimreplicatedclient : %client;

    %obj.unHideNode("ALL");
    %obj.setNodeColor($hat[$clientappearance.hat],%clientappearance.hatColor);
	%obj.setFaceName(%clientappearance.faceName);
	%obj.setDecalName(%clientappearance.decalName);
	%obj.setNodeColor("head",%clientappearance.headColor);
	%obj.setNodeColor("chestskinwalker",%clientappearance.chestColor);
	%obj.setNodeColor("pants",%clientappearance.hipColor);
	%obj.setNodeColor("rarm",%clientappearance.rarmColor);
	%obj.setNodeColor("Larm",%clientappearance.larmColor);
	%obj.setNodeColor("lhand",%clientappearance.lhandColor);
	%obj.setNodeColor("rhand",%clientappearance.rhandColor);
	%obj.setNodeColor("rshoe",%clientappearance.rlegColor);
	%obj.setNodeColor("lshoe",%clientappearance.llegColor);
}

function PlayerSkinwalker::onTrigger(%this, %obj, %trig, %press) 
{		
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%press) switch(%trig)
	{
		case 0: if(%obj.getEnergyLevel() >= 25)
				{
					%this.killerMelee(%obj, 4);
				}
				

		case 4: if(%obj.getEnergyLevel() >= %this.maxEnergy && !isObject(%obj.victim) && !isEventPending(%obj.monsterTransformschedule))				
				%this.monsterTransform(%obj, false);	
	}
}

function PlayerSkinwalker::onPeggFootstep(%this,%obj)
{
	serverplay3d("skinwalker_walking" @ getRandom(1,5) @ "_sound", %obj.getHackPosition());
	%obj.spawnExplosion("Eventide_footstepShakeProjectile", 0.5 + (getRandom() / 2));
}

function PlayerSkinwalker::monsterTransform(%this,%obj,%bool,%count)
{
    if(!isObject(%obj)) return;

    if(%count <= 10)
    {
        if(!%obj.changeaudio) 
        {
            %obj.playaudio(3,"skinwalker_change_sound");
            %obj.changeaudio = true;
        }

        %obj.playthread(0,"plant");
        %obj.monsterTransformschedule = %this.schedule(100,monsterTransform,%obj,%bool,%count+1);
    }
    else 
    {
        switch(%bool)
        {
            case true: %obj.setdatablock("PlayerSkinwalker");
            case false: %obj.setdatablock("EventidePlayer");
						if(isObject(%obj.light)) %obj.light.delete();                        
        }
		
        %obj.changeaudio = false;
    }
}