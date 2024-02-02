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
		
		if(%datablock.meleetrailskin !$= "") %obj.spawnKillerTrail(%datablock.meleetrailskin,%datablock.meleetrailoffset,%datablock.meleetrailangle,%datablock.meleetrailscale);		
		if(%datablock.killermeleesound !$= "") serverPlay3D(%datablock.killermeleesound @ getRandom(1,%datablock.killermeleesoundamount) @ "_sound",%obj.getWorldBoxCenter());				
		if(%datablock.killerweaponsound !$= "")serverPlay3D(%datablock.killerweaponsound @ getRandom(1,%datablock.killerweaponsoundamount) @ "_sound",%obj.getWorldBoxCenter());

		initContainerRadiusSearch(%obj.getMuzzlePoint(0), %radius, $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType);		
		while(%hit = containerSearchNext())
		{
			%obscure = containerRayCast(%obj.getMuzzlePoint(0),%hit.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
			%dot = vectorDot(%obj.getMuzzleVector(0),vectorNormalize(vectorSub(%hit.getposition(),%obj.getposition())));

			if(%hit == %obj) continue;

			if(isObject(%obscure))
			{
				if(%dataBlock.hitobscureprojectile !$= "" && %dot > 0.75)
				{
					%c = new Projectile()
					{
						dataBlock = %datablock.hitobscureprojectile;
						initialPosition = posfromraycast(%obscure);
						sourceObject = %obj;
						client = %obj.client;
					};
					MissionCleanup.add(%c);
					%c.explode();
				}
				continue;
			}

			if(%dot < 0.5) continue;
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

			if (isObject(%killerclient) && !%obj.isChasing)
			{
				if(%obj.chaseLevel != 1)
				{
					%killerclient.SetChaseMusic(%obj.getDataBlock().killerChaseLvl1Music,false);
					%obj.chaseLevel = 1;
				}
				cancel(%killerclient.StopChaseMusic);
				%killerclient.StopChaseMusic = %killerclient.schedule(6000, StopChaseMusic);
			}
			%obj.isChasing = false;
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

    %client.player.chaseLevel = 0;
    %client.musicChaseLevel = 0;
}

/// @param	this	player
/// @param	skin	string
/// @param	offset	3-element position
/// @param	angle	3-element euler rotation (in degrees)
/// @param	scale	3-element vector
function Player::spawnKillerTrail(%this, %skin, %offset, %angle, %scale)
{
	%shape = new StaticShape()
	{
		dataBlock = KillerTrailShape;
		scale = %scale;
	};
	
	if(isObject(%shape))
	{
		MissionCleanup.add(%shape);
		
		%shape.setSkinName(%skin);
		
		%rotation = relativeVectorToRotation(%this.getLookVector(), %this.getUpVector());
		%clamped = mClampF(firstWord(%rotation), -89.9, 89.9) SPC restWords(%rotation);
		
		%local = %this.getHackPosition() SPC %clamped;
		%combined = %offset SPC eulerToQuat(%angle);
		%actual = matrixMultiply(%local, %combined);
		
		%shape.setTransform(%actual);
		%shape.playThread(0, "rotate");
		%shape.schedule(1000, delete);
	}
}