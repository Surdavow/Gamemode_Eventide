datablock PlayerData(PlayerGenocide : PlayerRenowned) 
{
	uiName = "Genocide Player";

	hitprojectile = KillerRoughHitProjectile;
	hitobscureprojectile = "";
	meleetrailskin = "base";
	meleetrailoffset = "0.3 1.4 0.7"; 	
	meleetrailscale = "4 4 3";

	killerChaseLvl1Music = "musicData_Eventide_GenocideNear";
	killerChaseLvl2Music = "musicData_Eventide_GenocideChase";

	killeridlesound = "";
	killeridlesoundamount = 1;

	killerchasesound = "";
	killerchasesoundamount = 1;

	killermeleesound = "genocide_attack";
	killermeleesoundamount = 5;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;
	
    killernearsound = "genocide_looking";
	killernearsoundamount = 7;

    killertauntsound = "genocide_kill";
    killertauntsoundamount = 7;

	killerfoundvictimsound = "genocide_foundvictim";
	killerfoundvictimsoundamount = 5;

    killerlostvictimsound = "genocide_lostvictim";
	killerlostvictimsoundamount = 5;

    killerthreatenedsound = "genocide_threatened";
	killerthreatenedsoundamount = 3;

    killerdesperatesound = "genocide_desperate";
	killerdesperatesoundamount = 3;

    killerattackedsound = "genocide_attacked";
	killerattackedsoundamount = 5;

    killerspawnsound = "genocide_spawn";
    killerspawnsoundamount = 3;

    killerwinsound = "genocide_win";
    killerwinsoundamount = 3;

    killerlosesound = "genocide_lose";
    killerlosesoundamount = 2;
	
	killerlight = "NoFlarePLight";
	
	leftclickicon = "color_melee";
	rightclickicon = "color_random_item";

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
	PlayerCannibal::onTrigger(%this, %obj, %trig, %press);
}

function PlayerGenocide::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.mountImage("shovelImage",1);
	%obj.setScale("1 1 1");
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
	%obj.unhideNode("postal");
	
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

function PlayerGenocide::onKillerChaseStart(%this, %obj, %chasing)
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

function PlayerGenocide::onKillerChase(%this, %obj, %chasing)
{
	if(!%chasing && !%obj.isInvisible)
    {
        //A victim is nearby but Sky Captain can't see them yet. Say some quips.
        %soundType = %this.killernearsound;
        %soundAmount = %this.killernearsoundamount;
        if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + getRandom(15000, 25000))))
        {
            %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
            %obj.lastKillerSoundTime = getSimTime();
        }
    }
}

function PlayerGenocide::onKillerChaseEnd(%this, %obj)
{
	//If Sky Captain doesn't get any kills during a chase, play a voice line marking his dismay.
    if(%obj.incapsAchieved.getCount() == 0 && !%obj.isInvisible)
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

function PlayerGenocide::onExitStun(%this, %obj)
{
    //"You DARE..."
	%soundType = %this.killerattackedsound;
    %soundAmount = %this.killerattackedsoundamount;
    if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 5000)))
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerGenocide::onAllRitualsPlaced(%this, %obj)
{
    //"Will NOTHING stop you!?"
    %soundType = %this.killerdesperatesound;
    %soundAmount = %this.killerdesperatesoundamount;
    if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 5000)))
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerGenocide::onRoundEnd(%this, %obj, %won)
{
    //Plays a taunt if Sky Captain wins, or despair if he loses.
    %soundType = %won ? %this.killerwinsound : %this.killerlosesound;
    %soundAmount = %won ? %this.killerwinsoundamount : %this.killerlosesoundamount;
    if(%soundType !$= "")
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function PlayerGenocide::onIncapacitateVictim(%this, %obj, %victim, %killed)
{
	//Play a voice-line taunting the victim.
    %soundType = %this.killertauntsound;
    %soundAmount = %this.killertauntsoundamount;
    if(%soundType !$= "") 
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function Player::GenocideGaze(%this, %obj)
{
    if(!isObject(%obj) || %obj.isDisabled())
    {
        return;
    }

    %currentPosition = %obj.getPosition();
    %maximumDistance = $EnvGuiServer::VisibleDistance;
    %mask = $TypeMasks::PlayerObjectType;

    initContainerRadiusSearch(%currentPosition, %maximumDistance, %mask);
    while(%foundPlayer = ContainerSearchNext())
    {
        %killerPosition = %obj.getHackPosition();
        %killerDatablock = %obj.getDataBlock();
        %victimPosition = %foundPlayer.getHackPosition();
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
        else if(isObject(getWord(ContainerRayCast(%victimPosition, %killerPosition, %obstructions), 0)))
        {
            //The killer and victim are phyiscally blocked, skip.
            continue;
        }
        else if(%obj.isChasing && %foundPlayer.chaseLevel == 2)
        {
            //Nowhere better to put this: if the player has a weapon, have Sky Captain play a voice line acknowledging it.
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

            %victimEquippedItem = %foundPlayer.getMountedImage($RightHandSlot);
            if(isObject(%victimEquippedItem) && (%victimEquippedItem.isWeapon || %victimEquippedItem.className $= "WeaponImage"))
            {
                //"You think that will help you!?"
                %soundType = %killerDatablock.killerthreatenedsound;
                %soundAmount = %killerDatablock.killerthreatenedsoundamount;
                if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 5000)))
                {
                    %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
                    %obj.lastKillerSoundTime = getSimTime();
                    %obj.threatsReceived.add(%foundPlayer); //Ensure Sky Captain does not acknowledge any further weapons. Less annoying.
                }
            }
            continue;
        }
	}
}

function PlayerGenocide::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%obj.getState() !$= "Dead")
	{
		%obj.playaudio(0,"genocide_pain" @ getRandom(1, 6) @ "_sound");
	}
}