datablock PlayerData(PlayerMastermind : PlayerRenowned) 
{
	uiName = "Mastermind Player";

	hitprojectile = KillerRoughHitProjectile;
	hitobscureprojectile = "";
	meleetrailskin = "base";
	meleetrailoffset = "0.3 1.4 0.7"; 	
	meleetrailscale = "4 4 3";

	killerChaseLvl1Music = "musicData_Eventide_MastermindNear";
	killerChaseLvl2Music = "musicData_Eventide_MastermindChase";

	killeridlesound = "";
	killeridlesoundamount = 1;

	killerchasesound = "";
	killerchasesoundamount = 1;

	killermeleesound = "mastermind_melee";
	killermeleesoundamount = 5;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
    killernearsound = "mastermind_looking";
	killernearsoundamount = 4;

    killertauntsound = "mastermind_kill";
    killertauntsoundamount = 4;

	killerfoundvictimsound = "mastermind_foundvictim";
	killerfoundvictimsoundamount = 4;

    killerlostvictimsound = "mastermind_lostvictim";
	killerlostvictimsoundamount = 2;

    killerthreatenedsound = "mastermind_threatened";
	killerthreatenedsoundamount = 3;

    killerdesperatesound = "mastermind_desperate";
	killerdesperatesoundamount = 3;

    killerattackedsound = "mastermind_attacked";
	killerattackedsoundamount = 4;

    killerspawnsound = "mastermind_spawn";
    killerspawnsoundamount = 1;

    killerwinsound = "";
    killerwinsoundamount = 1;

    killerlosesound = "";
    killerlosesoundamount = 1;
	
	killerlight = "NoFlarePLight";
	
	leftclickicon = "color_melee";
	rightclickicon = "color_slowmo";

	rechargeRate = 0.3;
	maxTools = 1;
	maxWeapons = 1;
	maxForwardSpeed = 7.35;
	maxBackwardSpeed = 4.2;
	maxSideSpeed = 6.3;
	jumpForce = 0;

    gazeTickRate = 50;
};

function PlayerMastermind::onTrigger(%this, %obj, %trig, %press) 
{		
	Parent::onTrigger(%this, %obj, %trig, %press);
		
	if(%press && !%trig && %obj.getEnergyLevel() >= 25)
	{
		%this.killerMelee(%obj,4);
	}
}

function PlayerMastermind::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.mountImage("meleeKnifeImage",1);
	%obj.mountImage("overcoatImage",2);
	%obj.setScale("1 1 1");

    %obj.gazeTickRate = %this.gazeTickRate;
    %obj.MastermindGaze();
}

function PlayerMastermind::onRemove(%this, %obj)
{
    if(isObject(%obj.incapsAchieved))
    {
        %obj.incapsAchieved.delete();
    }
    if(isObject(%obj.threatsReceived))
    {
        %obj.threatsReceived.delete();
    }
    parent::onRemove(%this, %obj);
}

function PlayerMastermind::EventideAppearance(%this,%obj,%client)
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

	%shirtColor = "0.05 0.05 0.05 1";
	%pantsColor = "0.05 0.05 0.05 1";
	%skinColor = "0.83 0.73 0.66 1";
	%shoesColor = "0.05 0.05 0.05 1";

	%obj.setFaceName("smiley");
	%obj.setDecalName("brickadiashirt25");
	%obj.setNodeColor("rarm",%shirtColor);
	%obj.setNodeColor("larm",%shirtColor);
	%obj.setNodeColor("chest",%shirtColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor("rshoe",%shoesColor);
	%obj.setNodeColor("lshoe",%shoesColor);
	%obj.setNodeColor("rhand",%skinColor);
	%obj.setNodeColor("lhand",%skinColor);
	%obj.setNodeColor("headskin",%skinColor);
	
	//Set blood colors.
	%obj.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("lhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("rhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_back", "0.7 0 0 1");
}

//
// Voice-line handlers.
//

function PlayerMastermind::onKillerChaseStart(%this, %obj, %chasing)
{
    //Mark kills for the below end-of-chase voice line.
    if(!isObject(%obj.incapsAchieved))
    {
        %obj.incapsAchieved = new SimSet();
    }
    if(!isObject(%obj.threatsReceived))
    {
        %obj.threatsReceived = new SimSet();
    }

    %soundType = %this.killerfoundvictimsound;
    %soundAmount = %this.killerfoundvictimsoundamount;
    if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 10000)))
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerMastermind::onKillerChase(%this, %obj, %chasing)
{
	if(!%chasing)
    {
        //A victim is nearby but Postal Dude can't see them yet. Say some quips.
        %soundType = %this.killernearsound;
        %soundAmount = %this.killernearsoundamount;
        if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + getRandom(15000, 25000))))
        {
            %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
            %obj.lastKillerSoundTime = getSimTime();
        }
    }
}

function PlayerMastermind::onKillerChaseEnd(%this, %obj)
{
	//If Postal Dude doesn't get any kills during a chase, play a voice line marking his dismay.
    if(%obj.incapsAchieved.getCount() == 0)
    {
        %soundType = %this.killerlostvictimsound;
        %soundAmount = %this.killerlostvictimsoundamount;
        if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + getRandom(5000, 15000))))
        {
            %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
            %obj.lastKillerSoundTime = getSimTime();
        }
    }

    //Need to clear the list. Deleting it is simple and safe.
    %obj.incapsAchieved.delete();
}

function PlayerMastermind::onExitStun(%this, %obj)
{
	%soundType = %this.killerattackedsound;
    %soundAmount = %this.killerattackedsoundamount;
    if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 5000)))
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerMastermind::onAllRitualsPlaced(%this, %obj)
{
    %soundType = %this.killerdesperatesound;
    %soundAmount = %this.killerdesperatesoundamount;
    if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 5000)))
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerMastermind::onRoundEnd(%this, %obj, %won)
{
    //Plays a taunt if Postal Dude wins, or despair if he loses.
    if(%won)
    {
        return;
    }

    %soundType = %won ? %this.killerwinsound : %this.killerlosesound;
    %soundAmount = %won ? %this.killerwinsoundamount : %this.killerlosesoundamount;
    if(%soundType !$= "")
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerMastermind::onIncapacitateVictim(%this, %obj, %victim, %killed)
{
	//Play a voice-line taunting the victim.
    %soundType = %this.killertauntsound;
    %soundAmount = %this.killertauntsoundamount;
    if(%soundType !$= "") 
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }

    //Mark the kill on a temporary SimSet. Used for a voice-line mechanic in `onKillerChaseEnd`.
    if(!isObject(%obj.incapsAchieved))
    {
        %obj.incapsAchieved = new SimSet();
    }
    if(isObject(%victim.client))
    {
        %obj.incapsAchieved.add(%victim.client);
    }
    else
    {
        //Add in a dummy object for holebot support.
        %obj.incapsAchieved.add(
            new ScriptObject() 
            {
                player = %victim;
                name = %victim.getClassName();
            }
        );
    }
}

function Player::MastermindGaze(%obj)
{
    if(!isObject(%obj) || %obj.isDisabled())
    {
        return;
    }

    %currentPosition = %obj.getPosition();
    %maximumDistance = $EnvGuiServer::VisibleDistance;

    initContainerRadiusSearch(%currentPosition, %maximumDistance, $TypeMasks::PlayerObjectType);
    while(%foundPlayer = ContainerSearchNext())
    {
        %killerPosition = %obj.getEyePoint();
        %killerDatablock = %obj.getDataBlock();
        %victimPosition = %foundPlayer.getEyePoint();
        %victimDatablock = %foundPlayer.getDataBlock();
        %obstructions = ($TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType);

        if(%foundPlayer.isKiller || %victimDatablock.isKiller)
        {
            //We found ourselves, skip. Future-proofing in case multiple-killer setups become a thing.
            continue;
        }
        else if(%victimDatablock.isDowned)
        {
            //Victim is downed, skip.
            continue;
        }
        else if(ContainerRayCast(%victimPosition, %killerPosition, %obstructions))
        {
            //The killer and victim are phyiscally blocked, skip.
            continue;
        }
        else if(!%obj.isChasing || %foundPlayer.chaseLevel != 2)
        {
			//The victim is not being chased, they are irrelevant here. Skip.
			continue;
		}

		//The victim does not have any items equipped, don't bother with this.
		%victimEquippedItem = %foundPlayer.getMountedImage($RightHandSlot);
		if(!%victimEquippedItem)
		{
			continue;
		}

		//Nowhere better to put this: if the player has a weapon, have Shire play a voice line acknowledging it.
		%alreadyThreatenedKiller = false;
		for(%i = 0; %i < %obj.threatsReceived.getCount(); %i++)
		{
			if(%obj.threatsReceived.getObject(%i).getId() == %foundPlayer.getId())
			{
				%alreadyThreatenedKiller = true;
				break;
			}
		}
		if(%alreadyThreatenedKiller)
		{
			//They already threatened us, skip playing any more voice lines.
			continue;
		}

		//They have an item equipped and it's a weapon, have Postal Dude react to it.
		if(%victimEquippedItem.isWeapon || %victimEquippedItem.className $= "WeaponImage")
		{
			%soundType = %killerDatablock.killerthreatenedsound;
			%soundAmount = %killerDatablock.killerthreatenedsoundamount;
			if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 5000)))
			{
				%obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
				%obj.lastKillerSoundTime = getSimTime();
				%obj.threatsReceived.add(%foundPlayer); //Ensure Postal Dude does not acknowledge any further weapons. Less annoying.
			}
		}

		continue;
	}

    %obj.schedule(%obj.gazeTickRate, MastermindGaze);
}

function PlayerMastermind::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead")
	{
		%obj.playaudio(0,"mastermind_pain" @ getRandom(1, 3) @ "_sound");
	}
}