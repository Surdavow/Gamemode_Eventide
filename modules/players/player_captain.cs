datablock PlayerData(PlayerCaptain : PlayerRenowned) 
{
	uiName = "Sky Captain Player";
	isKiller = true;
	
	// Weapon: Combat Knife
	hitprojectile = KillerSharpHitProjectile;
	hitobscureprojectile = KillerMacheteClankProjectile;

	killerChaseLvl1Music = "musicData_Eventide_CaptainNear";
	killerChaseLvl2Music = "musicData_Eventide_CaptainChase";

	leftclickicon = "color_melee";
    rightclickicon = "";

	killeridlesound = "";
	killeridlesoundamount = 1;

    killerchasesound = "";
	killerchasesoundamount = 1;

    killermeleesound = "";
	killermeleesoundamount = 1;

    killernearsound = "captain_looking";
	killernearsoundamount = 4;

    killertauntsound = "captain_kill";
    killertauntsoundamount = 7;

	killerfoundvictimsound = "captain_foundvictim";
	killerfoundvictimsoundamount = 7;

    killerlostvictimsound = "captain_lostvictim";
	killerlostvictimsoundamount = 6;

    killerthreatenedsound = "captain_threatened";
	killerthreatenedsoundamount = 3;

    killerattackedsound = "captain_attacked";
	killerattackedsoundamount = 6;

    killerweaponchargedsound = "captain_rocketequip";
    killerweaponchargedsoundamount = 3;

    killerspawnsound = "captain_spawn";
    killerspawnsoundamount = 4;
	
    killerweapon = "blackKnifeImage";
	killerweaponsound = "captain_weapon";
	killerweaponsoundamount = 1;
	
	killerlight = "NoFlareRLight";

	rechargeRate = 0.3;
	maxTools = 1;
	maxWeapons = 1;
	maxForwardSpeed = 7.35;
	maxBackwardSpeed = 7.35;
	maxSideSpeed = 7.35;
    airControl = 1.0;
	jumpForce = 0;

    //Crouching speed that's fast, but slow enough to not to chase people with.
    maxForwardCrouchSpeed = 5.88;
	maxBackwardCrouchSpeed = 5.88;
	maxSideCrouchSpeed = 5.88;

    gazeTickRate = 50;
    gazeFullyCharged = 5000;
};

//
// Appearance, initialization.
//

function PlayerCaptain::EventideAppearance(%this,%obj,%client)
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
    %obj.unhideNode("helmet");
    %obj.unHideNode("armor");
    %obj.unhideNode("visor");
    %obj.unhideNode("epauletsRankA");

	%bodyColor = "0 0 0 1";
    %secondaryBodyColor = "0.0514019 0.0514019 0.102804 1";
	%visorColor = "0.667 0.000 0.000 0.700";
	%insigniaColor = "0.388235 0 0.117647 1";

	%obj.setFaceName("smiley");
	%obj.setDecalName("AAA-None");
	%obj.setNodeColor("rarm", %secondaryBodyColor);
	%obj.setNodeColor("larm", %secondaryBodyColor);
	%obj.setNodeColor("chest", %secondaryBodyColor);
	%obj.setNodeColor("pants", %secondaryBodyColor);
	%obj.setNodeColor("rshoe", %bodyColor);
	%obj.setNodeColor("lshoe", %bodyColor);
	%obj.setNodeColor("rhand", %bodyColor);
	%obj.setNodeColor("lhand", %bodyColor);
	%obj.setNodeColor("headskin", %bodyColor);
    %obj.setNodeColor("helmet", %bodyColor);
    %obj.setNodeColor("armor", %bodyColor);
    %obj.setNodeColor("visor", %visorColor);
    %obj.setNodeColor("epauletsRankA", %insigniaColor);
}

function PlayerCaptain::onNewDatablock(%this, %obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.mountImage(%this.killerweapon, $LeftHandSlot);
	%obj.setScale("1 1 1");

    //Start up the Gaze mechanic.
    %obj.gazeTickRate = %this.gazeTickRate;
    %obj.gazeFullyCharged = %this.gazeFullyCharged;
    %obj.SCMissleCount = 0;
    %obj.trackingStatus = "\c0OFFLINE";
    %obj.SkyCaptainGaze(%obj);

    //Spawn-in voice-line.
    %soundType = %this.killerspawnsound;
    %soundAmount = %this.killerspawnsoundamount;
    if(%soundType !$= "")
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

//
// Custom HUD.
//

function PlayerCaptain::killerGUI(%this, %obj, %client)
{	
    if(!isObject(%obj) || !isObject(%client))
    {
        return;
    }

    if(isObject(%obj.trackingCandidate))
    {
        if(isObject(%obj.trackingCandidate.client))
        {
            %trackingCandidateName = %obj.trackingCandidate.client.name;
        }
        else
        {
            %trackingCandidateName = %obj.trackingCandidate.getClassName();
        }
    }

    if(%obj.SCMissleCount < 1)
    {
        %ammoDisplay = "\c0" @ %obj.SCMissleCount;
    }
    else
    {
        %ammoDisplay = "\c3" @ %obj.SCMissleCount;
    }

    if(%obj.isTracking)
    {
        %trackingDisplay = "\c3CALIBRATING";
    }
    else if(%obj.trackingCandidate)
    {
        %trackingDisplay = "\c2ONLINE";
    }
    else
    {
        %trackingDisplay = "\c0OFFLINE";
    }

	// Some dynamic varirables
	%energylevel = %obj.getEnergyLevel();
	%leftclickstatus = (%energylevel >= 25) ? "hi" : "lo";
	%rightclickstatus = (%energylevel == %this.maxEnergy) ? "hi" : "lo";
	%leftclicktext = (%this.leftclickicon !$= "") ? "<just:left>\c6Left click" : "";
	%trackingStatus = "<just:right>\c9[ \c6Tracking system: " @ %trackingDisplay @ " \c9]";	

	// Regular icons
	%leftclickicon = (%this.leftclickicon !$= "") ? "<just:left><bitmap:" @ $iconspath @ %leftclickstatus @ %this.leftclickicon @ ">" : "";
	%ammoCounter = (%obj.SCMissleCount !$= "") ? "<just:right>\c9[ \c6Ammunition: " @ %ammoDisplay @ " \c9]" : "<just:right>\c9[---]";

	%client.bottomprint("<font:Consolas:24>" @ %leftclicktext @ %trackingStatus @ "<br>" @ %leftclickicon @ %ammoCounter, 1);
}

//
// Voice-line handlers.
//

function PlayerCaptain::onKillerChaseStart(%this, %obj, %chasing)
{
    //Mark kills for the below end-of-chase voice line.
    if(!isObject(%obj.incapsAchieved))
    {
        %obj.incapsAchieved = new SimSet();
        %obj.threatsRecieved = new SimSet();
    }

    %soundType = %this.killerfoundvictimsound;
    %soundAmount = %this.killerfoundvictimsoundamount;
    if(%soundType !$= "" && (getSimTime() > (%obj.lastKillerSoundTime + 10000)))
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

function Armor::onKillerChase(%this, %obj, %chasing)
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

function PlayerCaptain::onKillerChaseEnd(%this, %obj)
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
    %obj.threatsRecieved.delete();
}

function Armor::onExitStun(%this, %obj)
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

//
// Stealth and "snowball" mechanics.
//

function PlayerCaptain::onTrigger(%this, %obj, %trig, %press)
{
	Parent::onTrigger(%this, %obj, %trig, %press);

    //When Sky Captain crouches, make him "invisible". No music.
    //Also reduced voice lines, but that'll be handled seperately.
    if(%trig == 3 && %press)
    {
        //"isCrouched" would be ideal here, but it turns it is broken.
        %obj.isInvisible = true;
    }
    else if(%trig == 3 && !%press)
    {
        %obj.isInvisible = false;
    }
	
    //Slight edit to make it so Sky Captain can't melee with an item (homing rocket launcher) out.
	if(%press && !%trig && %obj.getEnergyLevel() >= 25 && !isObject(%obj.getMountedImage($RightHandSlot)))
	{
		%this.killerMelee(%obj, 4);
		return;
	}
}

function PlayerCaptain::onIncapacitateVictim(%this, %obj, %victim, %killed)
{
    parent::onIncapacitateVictim(%this, %obj, %victim, %killed);
    //Reward Sky Captain for his kill/down with a homing rocket.
    if(%obj.SCMissleCount $= "")
    {
        %obj.SCMissleCount = 1;
    }
    else
    {
        %obj.SCMissleCount++;
    }

    //If the victim was our tracking candidate, clear them so we can get another one.
    if(isObject(%obj.trackingCandidate) && %obj.trackingCandidate.getID() == %victim.getID())
    {
        %obj.trackingCandidate = "";
        %this.killerGUI(%obj, %obj.client); //Update the killer's GUI immediately.
    }

    //Mark the kill on a temporary SimSet. Used for a voice-line mechanic in `onKillerChase`.
    if(isObject(%obj.incapsAchieved))
    {
        %obj.incapsAchieved.add(%obj.client);
    }

    //Play a voice-line taunting the victim.
    %soundType = %this.killertauntsound;
    %soundAmount = %this.killertauntsoundamount;
    if(%soundType !$= "") 
    {
        %obj.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
        %obj.lastKillerSoundTime = getSimTime();
    }
}

//
// Largely copied and pasted from script_killers. I needed to make a change so Sky Captain can melee when crouched, and instantly
// incaps unaware players.
function PlayerCaptain::killerMelee(%this, %obj, %radius)
{
	if(%obj.getState() $= "Dead" || %obj.getEnergyLevel() < %this.maxEnergy/8 || %obj.lastMeleeTime+1250 > getSimTime()) 
	{
		return;
	}
		
	%obj.lastMeleeTime = getSimTime();
	%meleeAnim = (%this.shapeFile $= EventideplayerDts.baseShape) ? getRandom(1,4) : getRandom(1,2);
	%hackPos = %obj.getHackPosition();
	%obj.setEnergyLevel(%obj.getEnergyLevel()-%this.maxEnergy/6);	
	%obj.playthread(2,"melee" @ %meleeAnim);							

	if(%this.meleetrailskin !$= "") 
	{
		%meleetrailangle = %this.meleetrailangle[%meleeAnim];
		%obj.spawnKillerTrail(%this.meleetrailskin,%this.meleetrailoffset,%meleetrailangle,%this.meleetrailscale);		
	}	

	if(%this.killerMeleesound !$= "") 
	{
		serverPlay3D(%this.killerMeleesound @ getRandom(1,%this.killerMeleesoundamount) @ "_sound",%hackPos);
	}
	
	if(%this.killerWeaponSound !$= "") 
	{
		serverPlay3D(%this.killerWeaponSound @ getRandom(1,%this.killerWeaponSoundamount) @ "_sound",%hackPos);
	}	

	initContainerRadiusSearch(%obj.getMuzzlePoint(0), %radius, $TypeMasks::PlayerObjectType);		
	while(%hit = containerSearchNext())
	{
		if(%hit == %obj || %hit == %obj.effectbot || VectorDist(%obj.getPosition(),%hit.getPosition()) > %radius || %hit.stunned) 
		{
			continue;
		}

		%typemasks = $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType;
		%obscure = containerRayCast(%obj.getEyePoint(),%hit.getPosition(),%typemasks, %obj);
		%dot = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%hit.getHackPosition(),%obj.getPosition())));				

		if(isObject(%obscure) && %this.hitobscureprojectile !$= "")
		{								
			%c = new Projectile()
			{
				dataBlock = %this.hitobscureprojectile;
				initialPosition = posfromraycast(%obscure);
				sourceObject = %obj;
				client = %obj.client;
			};
			
			MissionCleanup.add(%c);
			%c.explode();
			return;
		}

		if(%dot < 0.4)
		{
			continue;
		}

		if((%hit.getType() && $TypeMasks::PlayerObjectType) && minigameCanDamage(%obj, %hit))								
		{	
			if(%hit.getdataBlock().isDowned) 
			{
				continue;
			}
			
			if(%this.killerMeleehitsound !$= "")
			{
				%obj.stopaudio(3);
				%obj.playaudio(3,%this.killerMeleehitsound @ getRandom(1,%this.killerMeleehitsoundamount) @ "_sound");		
			}

			if(%this.hitprojectile !$= "")
			{
				%effect = new Projectile()
				{
					dataBlock = %this.hitprojectile;
					initialPosition = %hit.getHackPosition();
					initialVelocity = vectorNormalize(vectorSub(%hit.getHackPosition(), %obj.getEyePoint()));
					scale = %obj.getScale();
					sourceObject = %obj;
				};
				
				MissionCleanup.add(%effect);
				%effect.explode();
			}

			
			%hit.setvelocity(vectorscale(VectorNormalize(vectorAdd(%obj.getForwardVector(),"0" SPC "0" SPC "0.15")),15));

            if(%hit.timeGazedUpon >= %obj.gazeFullyCharged)
            {
                //The victim is unaware and has been gazed at for over 5 seconds, insta-down/kill.
                %hit.damage(%obj, %hit.getHackPosition(), %hit.getDataBlock().maxDamage, $DamageType::Default);
            }
            else
            {
                //Sky Captain is weaker, he does half the damage of other killers in a straight-up confrontation.
                %hit.damage(%obj, %hit.getHackPosition(), 25*getWord(%obj.getScale(),2), $DamageType::Default);	
            }								
			
			%obj.setTempSpeed(0.3);	
			%obj.schedule(2500,setTempSpeed,1);
		}			
	}	
}

//
// Make Sky Captain put away his knife when the homing rocket launcher is out. Also, another voice line.
package Gamemode_Eventide_Player_Captain
{
    function ServerCmdUseTool(%client, %slot)
    {
        %player = %client.player;
        if(isObject(%player) && %player.getDataBlock().getName() $= "PlayerCaptain")
        {
            %player.unmountImage($LeftHandSlot);
        }
        parent::ServerCmdUseTool(%client, %slot);

        //"Say hello to my little friend..."
        %playerDatablock = %player.getDataBlock();
        %soundType = %playerDatablock.killerweaponchargedsound;
        %soundAmount = %playerDatablock.killerweaponchargedsoundamount;
        if(%soundType !$= "" && !%player.hasIntroducedWeapon && %player.SCMissleCount > 0) 
        {
            %player.hasIntroducedWeapon = true;
            %player.playAudio(0, %soundType @ getRandom(1, %soundAmount) @ "_sound");
            %player.lastKillerSoundTime = getSimTime();
        }
    }
    function ServerCmdUnUseTool(%client)
    {
        %player = %client.player;
        if(isObject(%player) && %player.getDataBlock().getName() $= "PlayerCaptain")
        {
            %player.mountImage(%player.getDataBlock().killerweapon, $LeftHandSlot);
        }
        parent::ServerCmdUnUseTool(%client);
    }
};
activatePackage(Gamemode_Eventide_Player_Captain);

//
// Gaze mechanic.
//

function Player::SkyCaptainGaze(%this, %obj)
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
        %victimPosition = %foundPlayer.getHackPosition();
        %obstructions = ($TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType);

        if(%foundPlayer.isKiller || %foundPlayer.getDataBlock().isKiller)
        {
            //We found ourselves, skip. Future-proofing in case multiple-killer setups become a thing.
            continue;
        }
        else if(%foundPlayer.getDataBlock().getName() $= "EventidePlayerDowned")
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
            //Victim is someone we're in a chase with, no stealth here. Skip.
            %obj.trackingCandidate = "";
            %foundPlayer.timeGazedUpon = 0;
            continue;
        }
        else if(isObject(%obj.trackingCandidate) && %obj.trackingCandidate.getID() == %foundPlayer.getID())
        {
            //We've already gazed upon this player for long enough, don't waste time.
            continue;
        }

        //These vector shinnanigans are less accurate, but faster to execute than using more raycasts.
        %maximumFOVDot = 0.95;

        %killerEyeVector = %obj.getEyeVector();
        %victimEyeVector = %foundPlayer.getEyeVector();

        //Is the victim looking at us?
        %victimSightLine = VectorNormalize(VectorSub(%killerPosition, %victimPosition));
        %victimEyeDot = VectorDot(%victimEyeVector, %victimSightLine);

        //For the dot product, 0 is everything in a 180-degree view in front of the victim, 1 is looking at directly at the killer.
        //We'll do 0.95 for a bit of fault tolerance.
        if(%victimEyeDot >= %maximumFOVDot)
        {
            //The victim is looking at the killer, reset their gaze and disable tracking.
            %foundPlayer.timeGazedUpon = 0;
        }
        else
        {
            //If the victim isn't looking at us, are we looking at them?
            %killerSightLine = VectorNormalize(VectorSub(%victimPosition, %killerPosition));
            %killerEyeDot = VectorDot(%killerEyeVector, %killerSightLine);

            if(%killerEyeDot >= %maximumFOVDot)
            {
                //We're looking at them.

                //Signal to the player via the GUI that they are currently tracking a player.
                %foundTrackingTarget = true;

                if(%foundPlayer.timeGazedUpon $= "")
                {
                    %foundPlayer.timeGazedUpon = 50;
                }
                else
                {
                    %foundPlayer.timeGazedUpon += 50;
                }

                //Play a little beep to Sky Captain every 10 ticks, to let him know the tracking is working.
                if((%foundPlayer.timeGazedUpon % 1000) == 0)
                {
                    %obj.client.play2D("captain_trackingbeep_sound");
                }

                if(%foundPlayer.timeGazedUpon >= %obj.gazeFullyCharged && !isObject(%obj.trackingCandidate))
                {
                    //We've tracked the victim for long enough, set them as our tracking candidate for homing rockets.
                    %obj.trackingCandidate = %foundPlayer;

                    //Update the killer's GUI immediately.
                    %obj.getDataBlock().killerGUI(%obj, %obj.client);

                    //Play a tune to notify Sky Captain that homing rockets are available.
                    %killerClient = %obj.client;
                    %killerClient.play2D("captain_trackingfinished_sound");
                    %killerClient.centerprint("<font:Consolas:18>\n\n\n\n\n\n\n\n\c6Homing rockets are now available.\n\c6Open your inventory to select the SC Rocket Launcher.\n\c6Gain ammo for it by getting downs or kills with your knife.", 10);
                }
                else
                {
                    //Tell Sky Captain to keep looking, while remaining undetected.
                    %obj.client.centerprint("<font:Consolas:18>\n\n\n\n\n\n\n\n\c6Remain undetected.\n\c6After enough calibrating, your tracking system will become available.\n\c6This unlocks instant downs or kills with your knife, and homing rockets.", 10);
                }
            }
        }
    }

    if(%foundTrackingTarget)
    {
        %obj.isTracking = true;
    }
    else
    {
        %obj.isTracking = false;
    }

    %obj.schedule(%obj.gazeTickRate, SkyCaptainGaze, %obj);
}