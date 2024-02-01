function KillerSpawnMessage(%obj)
{
	if(!isObject(%obj) || !isObject(%minigame = getMiniGameFromObject(%obj))) return;

	if(%obj.firstMessageSpawn) return;
	%obj.firstMessageSpawn = true;
	
	%random = getRandom(1,4);
	switch(%random)
	{
		case 1: %message = "The hunter has arrived.";
		case 2: %message = "Ready yourselves, the hunter has arrived.";
		case 3: %message = "Prepare yourselves, it is coming...";
		case 4: %message = %obj.getdataBlock().killerSpawnMessage;
	}

	%minigame.chatmsgall("<font:Impact:30>\c0" @ %message);
	for(%i = 0; %i < %minigame.numMembers; %i++) if(isObject(%member = %minigame.member[%i])) %member.play2D("render_wind_sound");	
}

function Player::KillerMelee(%obj,%datablock,%radius)
{	
	if(!%obj.isInvisible && %obj.lastclawed+1250 < getSimTime() && %obj.getEnergyLevel() >= %this.maxEnergy/8)
	{
		%obj.lastclawed = getSimTime();							
		%obj.playthread(2,"activate2");
		
		if(%datablock.killermeleesound !$= "")
		{
			%obj.stopaudio(0);
			%obj.playaudio(0,%datablock.killermeleesound @ getRandom(1,%datablock.killermeleesoundamount) @ "_sound");		
		}		

		for(%i = 0; %i < clientgroup.getCount(); %i++)//Can't use container radius search anymore :(
		{
			if(isObject(%nearbyplayer = clientgroup.getObject(%i).player))
			{
				if(%nearbyplayer == %obj || %nearbyplayer.getDataBlock().classname $= "PlayerData" || VectorDist(%nearbyplayer.getPosition(), %obj.getPosition()) > %radius) 
				continue;

				%hit = %nearbyplayer;

				%line = vectorNormalize(vectorSub(%hit.getPosition(),%obj.getEyePoint()));
				%dot = vectorDot(%obj.getEyeVector(), %line);
				%obscure = containerRayCast(%obj.getEyePoint(),%hit.getPosition(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);

				if(minigameCanDamage(%obj,%hit) == 1)
				if(!isObject(%obscure) && %dot > 0.65)						
				{
					if(vectorDist(%obj.getposition(),%hit.getposition()) < %radius)
					{
						switch$(%obj.getdataBlock().getName())
						{
							case "PlayerSkinWalker":	if(!isObject(%obj.victim) && %hit.getdataBlock().isDowned)
														{
															if(%hit.getDamagePercent() > 0.5)
															{
																if(isObject(%hit.client)) 
																{
																	%obj.stunned = true;
																	%hit.client.setControlObject(%hit.client.camera);
																	%hit.client.camera.setMode("Corpse",%hit);
																}
																%obj.victim = %hit;
																%obj.victimreplicatedclient = %hit.client;																
																%obj.playthread(1,"eat");
																%obj.playthread(2,"talk");
																%obj.playaudio(1,"skinwalker_grab_sound");
																%obj.mountobject(%hit,6);
																%hit.schedule(2250,kill);
																%hit.setarmthread("activate2");
																%hit.schedule(2250,spawnExplosion,"goryExplosionProjectile",%hit.getScale()); 
																%hit.schedule(2300,delete);        
																%obj.schedule(2250,playthread,1,"root");
																%obj.schedule(2250,playthread,2,"root");
																%obj.schedule(2250,setField,victim,0);
																%this.schedule(2250,EventideAppearance,%obj,%obj.client);
																return;
															}
															else 
															{
																%obj.client.centerPrint("<font:Impact:30>\c3Your victim needs to be below 50% health first!<br>Victim Health:" SPC %hit.getdataBlock().maxDamage-%hit.getDamageLevel(),4);
																continue;
															}
														}

							case "PlayerSkullwolf":	if(%hit.getdataBlock().isDowned)
													{
														if(%hit.getDamagePercent() > 0.5)
														{
															%obj.getdataBlock().eatVictim(%obj,%hit);
															return;
														}
														else 
														{
															%obj.client.centerPrint("<font:Impact:30>\c3Your victim needs to be below 50% health first!<br>Victim Health:" SPC %hit.getdataBlock().maxDamage-%hit.getDamageLevel(),4);
															continue;
														}														
													}
						}
						
						if(isObject(%obj.hookrope)) %obj.hookrope.delete();

						if(%hit.getdataBlock().isDowned) continue;
						
						if(%datablock.killermeleehitsound !$= "")
						{
							%obj.stopaudio(3);
							%obj.playaudio(3,%datablock.killermeleehitsound @ getRandom(1,%datablock.killermeleehitsoundamount) @ "_sound");		
						}						

						%obj.setEnergyLevel(%obj.getEnergyLevel()-%this.maxEnergy/8);
						%hit.setvelocity(vectorscale(VectorNormalize(vectorAdd(%obj.getForwardVector(),"0" SPC "0" SPC "0.15")),15));								
						%hit.damage(%obj, %hit.getWorldBoxCenter(), 50*getWord(%obj.getScale(),2), $DamageType::Default);
						%hit.spawnExplosion(pushBroomProjectile,"2 2 2");
						if(%datablock.killerHitProjectile !$= "") %hit.spawnExplosion(%datablock.killerHitProjectile,%obj.getScale());						

						%obj.setTempSpeed(0.5);
						%obj.schedule(2000,setTempSpeed,1);
					}
				}				
			}
			
		}
	}
}

function Player::onKillerLoop(%obj)
{
    if (!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getDataBlock().isKiller || !isObject(getMinigamefromObject(%obj)))
		return;

    %this = %obj.getDataBlock();

	// Container loop
	for (%i = 0; %i < clientgroup.getCount(); %i++)
	{
		%nearbyplayer = clientgroup.getObject(%i).player;

		if (!isObject(%nearbyplayer) || %nearbyplayer == %obj || %nearbyplayer.getClassName() !$= "Player" || VectorDist(%nearbyplayer.getPosition(), %obj.getPosition()) > 40)
			continue;		

		if (!isObject(getMinigamefromObject(%nearbyplayer)) || %nearbyplayer.getDataBlock().isKiller)
			continue;		

		%line = vectorNormalize(vectorSub(%nearbyplayer.getMuzzlePoint(2), %obj.getEyePoint()));
		%dot = vectorDot(%obj.getEyeVector(), %line);
		%killerclient = %obj.client;
		%victimclient = %nearbyplayer.client;

		// Chase behavior
		if (%dot > 0.45 && !isObject(containerRayCast(%obj.getEyePoint(), %nearbyplayer.getMuzzlePoint(2), $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType, %obj)) && !%obj.isInvisible)
		{			
			%obj.isChasing = true;
						
			// Set chase music and timers					
			if (isObject(%victimclient))
			{				
				if(%nearbyplayer.chaseLevel != 2)
				{
					%victimclient.SetChaseMusic(%obj.getDataBlock().killerChaseLvl2Music,true);
					%nearbyplayer.chaseLevel = 2;
				}
				%victimclient.player.TimeSinceChased = getSimTime();
				cancel(%victimclient.StopChaseMusic);
				%victimclient.StopChaseMusic = %victimclient.schedule(6000, StopChaseMusic);
			}

			if (isObject(%killerclient))
			{
				if(%obj.chaseLevel != 2)
				{
					%killerclient.SetChaseMusic(%obj.getDataBlock().killerChaseLvl2Music,false);
					%obj.chaseLevel = 2;
				}
				cancel(%killerclient.StopChaseMusic);
				%killerclient.StopChaseMusic = %killerclient.schedule(6000, StopChaseMusic);
			}							
		}
		else
		{
			if (isObject(%victimclient) && %victimclient.player.TimeSinceChased + 6000 < getSimTime())
			{
				if(%nearbyplayer.chaseLevel != 1)
				{
					%victimclient.SetChaseMusic(%obj.getDataBlock().killerChaseLvl1Music,false);
					%nearbyplayer.chaseLevel = 1;
				}
				cancel(%victimclient.StopChaseMusic);
				%victimclient.StopChaseMusic = %victimclient.schedule(6000, StopChaseMusic);
			}

			if (isObject(%killerclient))
			{
				if(%obj.chaseLevel != 1)
				{
					%killerclient.SetChaseMusic(%obj.getDataBlock().killerChaseLvl1Music,false);
					%obj.chaseLevel = 1;
				}
				cancel(%killerclient.StopChaseMusic);
				%killerclient.StopChaseMusic = %killerclient.schedule(6000, StopChaseMusic);
			}
		}
	}


    // Idle sounds
    if (%obj.lastKillerIdle + getRandom(6000, 8500) < getSimTime())
    {
        %obj.lastKillerIdle = getSimTime();

        // Play sounds based on chase state
        if (%obj.isChasing)
        {
            if (!%obj.isInvisible && %this.killerChaseSound !$= "")
            {
                %obj.playThread(3, "plant");
                %obj.playAudio(0, %this.killerChaseSound @ getRandom(1, %this.killerChaseSoundAmount) @ "_sound");
            }

            if (!%obj.raiseArms && %this.killerRaiseArms)
            {
                %obj.playThread(1, "armReadyBoth");
                %obj.raiseArms = true;
            }
        }
        else
        {
            if (!%obj.isInvisible && %this.killerIdleSound !$= "")
            {
                %obj.playThread(3, "plant");
                %obj.playAudio(0, %this.killerIdleSound @ getRandom(1, %this.killerIdleSoundAmount) @ "_sound");
            }

            if (%obj.raiseArms)
            {
                %obj.playThread(1, "root");
                %obj.raiseArms = false;
            }
        }
    }

    cancel(%obj.onKillerLoop); // Prevent duplicate processes
    %obj.onKillerLoop = %obj.schedule(1500, onKillerLoop);
}

function Player::KillerGhostLightCheck(%obj)
{	
	if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getdataBlock().isKiller || !isObject(getMinigamefromObject(%obj))) return;
	
	if(!%obj.isInvisible)
	{
		if(isObject(%obj.effectbot) && !isObject(%obj.effectbot.light))
		{
			%obj.effectbot.light = new fxLight ("")
			{
				dataBlock = %obj.getdataBlock().killerlight;
				source = %obj.effectbot;
			};

			MissionCleanup.add(%obj.effectbot.light);
			%obj.effectbot.light.attachToObject(%obj.effectbot);		
			%obj.effectbot.light.setNetFlag(6,true);
			%obj.effectbot.setNetFlag(6,true);

			for(%i = 0; %i < clientgroup.getCount(); %i++) 
			if(isObject(%client = clientgroup.getObject(%i))) 
			{
				if(%obj == %client.player)
				{
					%obj.effectbot.light.ScopeToClient(%client);
					%obj.effectbot.ScopeToClient(%client);
				}
				else 
				{
					%obj.effectbot.light.clearScopeToClient(%client);
					%obj.effectbot.clearScopeToClient(%client);
				}
			}			
		}
	}
	else if(isObject(%obj.effectbot.light)) %obj.effectbot.light.delete();	
}

function GameConnection::SetChaseMusic(%client,%songname,%ischasing)
{
    if(!isObject(%client) || !isObject(%songname)) return;    
    if(isObject(%client.EventidemusicEmitter)) %client.EventidemusicEmitter.delete();					

    %client.EventidemusicEmitter = new AudioEmitter()
    {
        position = "9e9 9e9 9e9";
        profile = %songname;
        volume = 1;
        type = 10;
        useProfileDescription = false;
        is3D = false;
    };
    MissionCleanup.add(%client.EventidemusicEmitter);
    %client.EventidemusicEmitter.scopeToClient(%client);
		
	if(isObject(%client.player) && %client.player.getdataBlock().getName() $= "EventidePlayer")
	%client.player.getdataBlock().TunnelVision(%client.player,%ischasing);
}

function GameConnection::StopChaseMusic(%client)
{
    if(!isObject(%client)) return;
    if(isObject(%client.EventidemusicEmitter)) %client.EventidemusicEmitter.delete();

	if(isObject(%client.player) && %client.player.getdataBlock().getName() $= "EventidePlayer")
	%client.player.getdataBlock().TunnelVision(%client.player,false);

    %client.musicChaseLevel = 0;
}