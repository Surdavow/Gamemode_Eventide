function KillerSpawnMessage(%obj)
{
	if(!isObject(%obj) || !isObject(%minigame = getMiniGameFromObject(%obj)) || %obj.firstMessageSpawn) return;
	
	switch(getRandom(1,4))
	{
		case 1: %message = "The hunter has arrived.";
		case 2: %message = "Ready yourselves, the hunter has arrived.";
		case 3: %message = "Prepare yourselves, it is coming.";
		case 4: %message = %obj.getdataBlock().killerSpawnMessage;
	}

	%minigame.chatmsgall("<font:Impact:30>\c0" @ %message);
	for(%i = 0; %i < %minigame.numMembers; %i++) if(isObject(%member = %minigame.member[%i])) %member.play2D("render_wind_sound");	

	%obj.firstMessageSpawn = true;
}

function Player::KillerMelee(%obj,%datablock,%radius)
{	
	if(!%obj.isInvisible && %obj.lastclawed+1250 < getSimTime() && %obj.getEnergyLevel() >= %dataBlock.maxEnergy/8)
	{
		%obj.lastclawed = getSimTime();	
		%obj.setEnergyLevel(%obj.getEnergyLevel()-%dataBlock.maxEnergy/6);						
				
		if(%datablock.shapeFile $= EventideplayerDts.baseShape) %meleeAnim = getRandom(1,4);
		else %meleeAnim = getRandom(1,2);

		switch(%meleeAnim)
		{
			case 1: %meleetrailangle = %datablock.meleetrailangle1;
			case 2: %meleetrailangle = %datablock.meleetrailangle2;
			case 3: %meleetrailangle = %datablock.meleetrailangle3;
			case 4: %meleetrailangle = %datablock.meleetrailangle4;
		}
				
		if(%datablock.meleetrailskin !$= "") %obj.spawnKillerTrail(%datablock.meleetrailskin,%datablock.meleetrailoffset,%meleetrailangle,%datablock.meleetrailscale);		
		if(%datablock.killermeleesound !$= "") serverPlay3D(%datablock.killermeleesound @ getRandom(1,%datablock.killermeleesoundamount) @ "_sound",%obj.getWorldBoxCenter());				
		if(%datablock.killerweaponsound !$= "")serverPlay3D(%datablock.killerweaponsound @ getRandom(1,%datablock.killerweaponsoundamount) @ "_sound",%obj.getWorldBoxCenter());
		%obj.playthread(2,"melee" @ %meleeAnim);

		initContainerRadiusSearch(%obj.getMuzzlePoint(0), %radius, $TypeMasks::PlayerObjectType);		
		while(%hit = containerSearchNext())
		{
			if(%hit == %obj || %hit == %obj.effectbot || VectorDist(%obj.getPosition(),%hit.getPosition()) > %radius) continue;

			%obscure = containerRayCast(%obj.getEyePoint(),%hit.getPosition(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
			%dot = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%hit.getPosition(),%obj.getPosition())));				

			if(isObject(%obscure))
			{								
				if(%dataBlock.hitobscureprojectile !$= "" && %dot > 0.85 && isObject(%obscure))
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
				return;
			}

			if(%dot < 0.6) continue;			

			if((%hit.getType() && $TypeMasks::PlayerObjectType) && minigameCanDamage(%obj,%hit) == true)								
			{
				switch$(%obj.getdataBlock().getName())
				{
					case "PlayerSkinWalker":	if(!isObject(%obj.victim) && %hit.getdataBlock().isDowned)
												{
													if(%hit.getDamagePercent() > 0.05)
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
														%hit.schedule(2295,kill);        
														%hit.schedule(2300,delete);        
														%obj.schedule(2250,playthread,1,"root");
														%obj.schedule(2250,playthread,2,"root");
														%obj.schedule(2250,setField,victim,0);
														%datablock.schedule(2250,EventideAppearance,%obj,%obj.client);
														return;
													}
													else continue;
													
												}

					case "PlayerSkullwolf":	if(%hit.getDamagePercent() > 0.25 && %hit.getdataBlock().isDowned)
											{
												%obj.getdataBlock().eatVictim(%obj,%hit);
												return;
											}
				}
					
				if(isObject(%obj.hookrope)) %obj.hookrope.delete();

				if(%hit.getdataBlock().isDowned) continue;
				
				if(%datablock.killermeleehitsound !$= "")
				{
					%obj.stopaudio(3);
					%obj.playaudio(3,%datablock.killermeleehitsound @ getRandom(1,%datablock.killermeleehitsoundamount) @ "_sound");		
				}						
				
				%hit.setvelocity(vectorscale(VectorNormalize(vectorAdd(%obj.getForwardVector(),"0" SPC "0" SPC "0.15")),15));								
				%hit.damage(%obj, %hit.getHackPosition(), 50*getWord(%obj.getScale(),2), $DamageType::Default);					

				if(%datablock.hitprojectile !$= "")
				{
					%effect = new Projectile()
					{
						dataBlock = %datablock.hitprojectile;
						initialPosition = %hit.getHackPosition();
						initialVelocity = vectorNormalize(vectorSub(%hit.getHackPosition(), %obj.getEyePoint()));
						scale = %obj.getScale();
						sourceObject = %obj;
					};
					
					MissionCleanup.add(%effect);
					%effect.explode();
				}

				%obj.setTempSpeed(0.3);	
				%obj.schedule(2500,setTempSpeed,1);
			}			
		}
	}
}

function Player::onKillerLoop(%obj)
{
    if (!isObject(%obj) || %obj.getState() $= "Dead" || !isObject(getMinigamefromObject(%obj)))
	return;

    %this = %obj.getDataBlock();

	// In case the player is not a killer, such as the skinwalker mimicking a player
	if(%obj.getDataBlock().isKiller)
	{
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

			if($Pref::Server::Eventide::chaseMusicEnabled)
			{
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
	}

	// Bottom print gui
	if (isObject(%client = %obj.client)) 
	%this.bottomprintgui(%obj,%client);

    cancel(%obj.onKillerLoop); // Prevent duplicate processes
    %obj.onKillerLoop = %obj.schedule(500, onKillerLoop);
}

function Armor::bottomprintgui(%this,%obj,%client)
{	
	if(!isObject(%obj) || !isObject(%client)) return;
	
	%iconpath = "Add-ons/Gamemode_Eventide/modules/misc/icons/";
	%energylevel = %obj.getEnergyLevel();

	// Some dynamic varirables
	%leftclickstatus = (%obj.getEnergyLevel() >= 25) ? "hi" : "lo";
	%rightclickstatus = (%obj.getEnergyLevel() == %this.maxEnergy) ? "hi" : "lo";
	%leftclicktext = (%this.leftclickicon !$= "") ? "<just:left>\c6Left click" : "";
	%rightclicktext = (%this.rightclickicon !$= "") ? "<just:right>\c6Right click" : "";		

	// Regular icons
	%leftclickicon = (%this.leftclickicon !$= "") ? "<just:left><bitmap:" @ %iconpath @ %leftclickstatus @ %this.leftclickicon @ ">" : "";
	%rightclickicon = (%this.rightclickicon !$= "") ? "<just:right><bitmap:" @ %iconpath @ %rightclickstatus @ %This.rightclickicon @ ">" : "";

	%client.bottomprint(%leftclicktext @ %rightclicktext @ "<br>" @ %leftclickicon @ %rightclickicon, 1);
}

function Player::KillerGhostLightCheck(%obj)
{	
	if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getdataBlock().isKiller || !isObject(getMinigamefromObject(%obj))) return;
	
	if(!%obj.isInvisible)
	{
			%obj.light = new fxLight ("")
			{
				dataBlock = %obj.getdataBlock().killerlight;
				source = %obj;
			};

			MissionCleanup.add(%obj.light);
			%obj.light.attachToObject(%obj);		
			%obj.light.setNetFlag(6,true);

			for(%i = 0; %i < clientgroup.getCount(); %i++) 
			if(isObject(%client = clientgroup.getObject(%i))) 
			{
				if(%obj == %client.player)
				{
					%obj.light.ScopeToClient(%client);
					%obj.ScopeToClient(%client);
				}
				else 
				{
					%obj.light.clearScopeToClient(%client);
					%obj.clearScopeToClient(%client);
				}
			}			
	}
	else if(isObject(%obj.light)) %obj.light.delete();	
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
