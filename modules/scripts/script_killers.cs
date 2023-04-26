function Eventide_Melee(%this,%obj,%radius)
{
	if(!%obj.isInvisible && %obj.lastclawed+500 < getSimTime() && %obj.getEnergyLevel() >= %this.maxEnergy/8)
	{
		switch$(%obj.getdataBlock().getName())
		{
			case "PlayerRenowned": %obj.playaudio(3,"renowned_melee" @ getRandom(0,2) @ "_sound");
			default:
		}

		%obj.lastclawed = getSimTime();							
		%obj.playthread(2,"activate2");
		%oscale = getWord(%obj.getScale(),2);
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
		initContainerRadiusSearch(%obj.getPosition(),15,%mask);
		while(%hit = containerSearchNext())
		{
			if(%hit == %obj) continue;
			%line = vectorNormalize(vectorSub(%hit.getWorldBoxCenter(),%obj.getWorldBoxCenter()));
			%dot = vectorDot(%obj.getEyeVector(), %line);
			%obscure = containerRayCast(%obj.getEyePoint(),%hit.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);

			if((!isObject(%obscure) && %dot > 0.65 && vectorDist(%obj.getposition(),%hit.getposition()) < %radius) || vectorDist(%obj.getposition(),%hit.getposition()) < 1)
			if(Eventide_MinigameConditionalCheck(%obj,%hit,true))
			{						
				if(%hit.getstate() $= "Dead")
				{
					if(%obj.getdataBlock().getName() $= "PlayerSkullwolf") 
					{
						%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");	
						%obj.playthread(3,"plant");
						%obj.setEnergyLevel(%obj.getEnergyLevel()+%this.maxEnergy/6);
						%hit.spawnExplosion("goryExplosionProjectile",%hit.getScale());
						%hit.schedule(50,delete);
					}
					continue;
				}

				switch$(%obj.getdataBlock().getName())
				{
					case "PlayerAngler": 	%obj.playaudio(0,"angler_Atk" @ getRandom(0,2) @ "_sound");
											%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");
											if(isObject(%obj.hookrope)) %obj.hookrope.delete();																			
					case "PlayerSkullWolf": %obj.playaudio(0,"skullwolf_Atk" @ getRandom(0,6) @ "_sound");
											%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");
					case "PlayerShire":		serverPlay3d("melee_axe_0" @ getRandom(1,2) @ "_sound", %hit.getPosition());			
					case "PlayerGrabber": serverPlay3d("melee_machete_0" @ getRandom(1,2) @ "_sound", %hit.getPosition());
										serverPlay3d("melee_machete_0" @ getRandom(1,2) @ "_sound", %hit.getPosition());
					case "PlayerRenowned": serverPlay3d("melee_tanto_0" @ getRandom(1,3) @ "_sound", %hit.getPosition());
										serverPlay3d("melee_tanto_0" @ getRandom(1,3) @ "_sound", %hit.getPosition());										
					case "PlayerSkinwalker": %obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");
					default:
				}

				%obj.setEnergyLevel(%obj.getEnergyLevel()-%this.maxEnergy/8);
				%hit.setvelocity(vectorscale(VectorNormalize(vectorAdd(%obj.getForwardVector(),"0" SPC "0" SPC "0.15")),15));								
				%hit.damage(%obj, %hit.getWorldBoxCenter(), 25*%oscale, $DamageType::Default);
				%hit.spawnExplosion(pushBroomProjectile,"2 2 2");

				%obj.setTempSpeed(0.65);
				%obj.schedule(500,setTempSpeed,1);
			}								
		}
	}
}

function Player::onKillerLoop(%obj)
{    
    if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getdataBlock().isKiller || !isObject(getMinigamefromObject(%obj))) return;

	if(%obj.getClassName() $= "Player") %obj.KillerGhostLightCheck();

	%this = %obj.getdataBlock();
	%pos = %obj.getPosition();
	%radius = 100;
	%searchMasks = $TypeMasks::PlayerObjectType;	

    InitContainerRadiusSearch(%obj.getPosition(), 40, $TypeMasks::PlayerObjectType);
    while(%scan = containerSearchNext())//Detect if players exist and if the killer can see them
    {
        if(%scan == %obj || !isObject(getMinigamefromObject(%scan)) || %scan.getdataBlock().isKiller) continue;
        %line = vectorNormalize(vectorSub(%scan.getmuzzlePoint(2),%obj.getEyePoint()));
        %dot = vectorDot(%obj.getEyeVector(), %line);
        %killerclient = %obj.client;
        %victimclient = %scan.client;

        //Not very efficient and messy code right now, will redo this sometime
        if(ContainerSearchCurrRadiusDist() <= 17 && %dot > 0.45 && !isObject(containerRayCast(%obj.getEyePoint(),%scan.getmuzzlePoint(2),$TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType,%obj)))
        {
            %killercansee[%cansee++] = %scan;
            %chasing = true;            
            
            if(!%obj.isInvisible)
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