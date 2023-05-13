function Player::KillerMelee(%obj,%datablock,%radius)
{	
	if(!%obj.isInvisible && %obj.lastclawed+500 < getSimTime() && %obj.getEnergyLevel() >= %this.maxEnergy/8)
	{
		%obj.lastclawed = getSimTime();							
		%obj.playthread(2,"activate2");
		%oscale = getWord(%obj.getScale(),2);
		
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

				if(minigameCanDamage(%obj,%hit))
				if(!isObject(%obscure) && %dot > 0.65)						
				{
					//Temporarily not working right now
					//if(%hit.getstate() $= "Dead" && vectorDist(%obj.getposition(),%hit.getposition()) < %radius*2.5)
					//{
					//	if(%obj.getdataBlock().getName() $= "PlayerSkullwolf") 
					//	{
					//		%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");	
					//		%obj.playthread(3,"plant");
					//		%obj.setEnergyLevel(%obj.getEnergyLevel()+%this.maxEnergy/6);
					//		%hit.spawnExplosion("goryExplosionProjectile",%hit.getScale());
					//		%hit.schedule(50,delete);
					//	}
					//	continue;
					//}

					if(vectorDist(%obj.getposition(),%hit.getposition()) < %radius)
					{
    					if(%obj.getdataBlock().getName() $= "PlayerSkinwalker")
						if(!isObject(%obj.victim) && %hit.getdataBlock().isDowned)
    					{
    					    if(%hit.getDamagePercent() > 0.5)
							{
								if(isObject(%hit.client)) %hit.client.setControlObject(%hit.client.camera);
    					    	%obj.victim = %hit;
    					    	%obj.victimreplicatedclient = %hit.client;
    					    	%obj.playthread(1,"eat");
    					    	%obj.playthread(2,"talk");
    					    	%obj.playaudio(1,"skinwalker_grab_sound");
    					    	%obj.mountobject(%hit,6);
    					    	%hit.schedule(2250,kill);
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
						
						if(%hit.getdataBlock().isDowned) continue;
						
						if(%datablock.killermeleehitsound !$= "")
						{
							%obj.stopaudio(3);
							%obj.playaudio(3,%datablock.killermeleehitsound @ getRandom(1,%datablock.killermeleehitsoundamount) @ "_sound");		
						}

						if(isObject(%obj.hookrope)) %obj.hookrope.delete();

						%obj.setEnergyLevel(%obj.getEnergyLevel()-%this.maxEnergy/8);
						%hit.setvelocity(vectorscale(VectorNormalize(vectorAdd(%obj.getForwardVector(),"0" SPC "0" SPC "0.15")),15));								
						%hit.damage(%obj, %hit.getWorldBoxCenter(), 25*%oscale, $DamageType::Default);
						%hit.spawnExplosion(pushBroomProjectile,"2 2 2");

						%obj.setTempSpeed(0.65);
						%obj.schedule(1000,setTempSpeed,1);
					}
				}				
			}
			
		}
	}
}

function Player::onKillerLoop(%obj)
{    
    if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getdataBlock().isKiller || !isObject(getMinigamefromObject(%obj))) return;

	if(%obj.getClassName() $= "Player") %obj.KillerGhostLightCheck();

	%this = %obj.getdataBlock();

	for(%i = 0; %i < clientgroup.getCount(); %i++)//Can't use container radius search anymore :(
	if(isObject(%nearbyplayer = clientgroup.getObject(%i).player))
	{			
		if(%nearbyplayer == %obj || %nearbyplayer.getDataBlock().classname $= "PlayerData" || VectorDist(%nearbyplayer.getPosition(), %obj.getPosition()) > 40) 
		continue;

		%scan = %nearbyplayer;
    
        if(!isObject(getMinigamefromObject(%scan)) || %scan.getdataBlock().isKiller) continue;
        %line = vectorNormalize(vectorSub(%scan.getmuzzlePoint(2),%obj.getEyePoint()));
        %dot = vectorDot(%obj.getEyeVector(), %line);
        %killerclient = %obj.client;
        %victimclient = %scan.client;

        //Not very efficient and messy code right now, will redo this sometime
        if(%dot > 0.45 && !isObject(containerRayCast(%obj.getEyePoint(),%scan.getmuzzlePoint(2),$TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType,%obj)))
        {
            %killercansee[%cansee++] = %scan;
            %chasing = true;            
            
            if(!%obj.isInvisible && VectorDist(%nearbyplayer.getPosition(), %obj.getPosition()) < 20)
            {
                if(isObject(%victimclient))
                {
                    if(%victimclient.musicChaseLevel != 2 && %killercansee[%cansee] == %scan)
                    {
                        %victimclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl2Music);
                        %victimclient.musicChaseLevel = 2;
                    }

                    %victimclient.player.TimeSinceChased = getSimTime();
                    cancel(%victimclient.StopChaseMusic);
                    %victimclient.StopChaseMusic = %victimclient.schedule(6000,StopChaseMusic);
                }

                if(isObject(%killerclient))
                {
                    if(%killerclient.musicChaseLevel != 2)
                    {
                        %killerclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl2Music);
                        %killerclient.musicChaseLevel = 2;
                    }

                    cancel(%killerclient.StopChaseMusic);
                    %killerclient.StopChaseMusic = %killerclient.schedule(6000,StopChaseMusic);
                }
            }
            else
            {
                if(isObject(%victimclient))
                {
                    if(%victimclient.musicChaseLevel != 1 && %killercansee[%cansee] == %scan)
                    {
                        %victimclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl1Music);
                        %victimclient.musicChaseLevel = 1;
                    }

                    %victimclient.player.TimeSinceChased = getSimTime();
                    cancel(%victimclient.StopChaseMusic);
                    %victimclient.StopChaseMusic = %victimclient.schedule(6000,StopChaseMusic);
                }

                if(isObject(%killerclient))
                {
                    if(%killerclient.musicChaseLevel != 1)
                    {
                        %killerclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl1Music);
                        %killerclient.musicChaseLevel = 1;
                    }

                    cancel(%killerclient.StopChaseMusic);
                    %killerclient.StopChaseMusic = %killerclient.schedule(6000,StopChaseMusic);
                }            
            }
        }
        else
        {
            if(isObject(%victimclient) && %killercansee[%cansee] != %scan && %victimclient.player.TimeSinceChased+6000 < getSimTime())
            {
                if(%victimclient.musicChaseLevel != 1)
                {
                    %victimclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl1Music);
                    %victimclient.musicChaseLevel = 1;
                }

                cancel(%victimclient.StopChaseMusic);
                %victimclient.StopChaseMusic = %victimclient.schedule(6000,StopChaseMusic);
            }

            if(isObject(%killerclient) && !%chasing)
            {
                if(%killerclient.musicChaseLevel != 1)
                {
                    %killerclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl1Music);
                    %killerclient.musicChaseLevel = 1;
                }

                cancel(%killerclient.StopChaseMusic);
                %killerclient.StopChaseMusic = %killerclient.schedule(6000,StopChaseMusic);
            }
        }
    }

	if(%obj.lastKillerIdle+getRandom(4500,6000) < getSimTime())//Idle sounds are now in this function instead of a separate function
	{
		%obj.lastKillerIdle = getSimTime();

		if(%killercansee[%cansee])//Requires the container loop to run in order for this to work, if this part is true then there are players the killer is after
		{
			if(!%obj.isInvisible && %this.killerchasesound !$= "")
			{
				%obj.playthread(3,"plant");
				%obj.playaudio(0,%this.killerchasesound @ getRandom(1,%this.killerchasesoundamount) @ "_sound");
			}

			if(!%obj.raisearms && %this.killerraisearms)
			{
				%obj.playthread(1,"armReadyBoth");
				%obj.raisearms = true;
			}
		}
		else//Return to idle behaviors
		{
			if(!%obj.isInvisible && %this.killeridlesound !$= "")
			{
				%obj.playthread(3,"plant");
				%obj.playaudio(0,%this.killeridlesound @ getRandom(1,%this.killeridlesoundamount) @ "_sound");	
			}

			if(%obj.raisearms)
			{
				%obj.playthread(1,"root");
				%obj.raisearms = false;
			}
		}		
	}

    cancel(%obj.onKillerLoop);//Prevent duplicate processes
    %obj.onKillerLoop = %obj.schedule(1500,onKillerLoop);
}

function Player::KillerGhostLightCheck(%obj)
{	
	if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getdataBlock().isKiller || !isObject(getMinigamefromObject(%obj))) return;
	
	if(!%obj.isInvisible)
	{
		if(!isObject(%obj.lightbot))
		{
			%obj.lightbot = new Player() 
			{ 
				dataBlock = "EmptyLightBot";
				source = %obj;			
			};
			%obj.mountObject(%obj.lightbot,5);
			MissionCleanup.add(%obj.lightbot);

			%obj.lightbot.light = new fxLight ("")
			{
				dataBlock = %obj.getdataBlock().killerlight;
				source = %obj.lightbot;
			};
			MissionCleanup.add(%obj.lightbot.light);
			%obj.lightbot.light.setTransform(%obj.lightbot.getTransform());
			%obj.lightbot.light.attachToObject(%obj.lightbot);
		}

		%obj.lightbot.light.setNetFlag(6,true);
		for(%i = 0; %i < clientgroup.getCount(); %i++)
		if(isObject(%client = clientgroup.getObject(%i)) && %client.player != %obj) 
		%obj.lightbot.light.schedule(10,clearScopeToClient,%client);
	}
	else
	{
		if(isObject(%obj.lightbot.light)) %obj.lightbot.light.delete();
		if(isObject(%obj.lightbot)) %obj.lightbot.delete();	
	}
}

function GameConnection::SetChaseMusic(%client,%songname)
{
    if(!isObject(%client) || !isObject(%songname)) return;
    
    if(isObject(%client.EventidemusicEmitter))
    %client.EventidemusicEmitter.delete();

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
}

function GameConnection::StopChaseMusic(%client)
{
    if(!isObject(%client)) return;
    if(isObject(%client.EventidemusicEmitter)) %client.EventidemusicEmitter.delete();

    %client.musicChaseLevel = 0;
}