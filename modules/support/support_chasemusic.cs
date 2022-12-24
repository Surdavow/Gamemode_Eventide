function Player::KillerScanCheck(%obj)
{
    if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getdataBlock().isKiller || !getMinigamefromObject(%obj)) return;

    InitContainerRadiusSearch(%obj.getPosition(), 60, $TypeMasks::PlayerObjectType);
    while(%scan = containerSearchNext())
    {
        if(%scan == %obj || !isObject(getMinigamefromObject(%scan)) || %scan.getdataBlock().isKiller) continue;
        %line = vectorNormalize(vectorSub(%scan.getmuzzlePoint(2),%obj.getEyePoint()));
        %dot = vectorDot(%obj.getEyeVector(), %line);
        %killerclient = %obj.client;
        %victimclient = %scan.client;


        if(ContainerSearchCurrRadiusDist() <= 50 && %dot > 0.45 && !isObject(containerRayCast(%obj.getEyePoint(),%scan.getmuzzlePoint(2),$TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType,%obj)))
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

    cancel(%obj.KillerScanCheck);
    %obj.KillerScanCheck = %obj.schedule(1500,KillerScanCheck);
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