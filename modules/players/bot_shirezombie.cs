
datablock PlayerData(ShireZombieBot : EventidePlayer)
{
	uiName = "";
	isKiller = true;
	maxDamage = 1;
    uiName = "";
    maxDamage = 9999;
};

function ShireZombieBot::applyAppearance(%this,%obj)
{
    Armor::EventideAppearance(%this,%obj,%obj.ghostclient);

    %ghostClient = %obj.ghostclient;
	%zskin = "0.28 0.0 0.52 0.15";
    %headColor = %zskin;
    %obj.setFaceName(%faceName);
	%obj.HideNode("visor");
	%obj.setNodeColor("ALL",%headColor);
    %obj.setFaceName("hexZombie");     
	%obj.startFade(0, 0, true);
}

function ShireZombieBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType)
{
    Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType);
}

function ShireZombieBot::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	
    %obj.setMoveSlowdown(0);
	%this.applyAppearance(%obj,%obj.ghostclient);
    %this.onBotLoop(%obj);
    %obj.mountImage("GlowFaceZombieImage",0);
	%obj.mountImage("ZombieBodyImage",1);    
}

function ShireZombieBot::onDisabled(%this,%obj)
{
	Parent::onDisabled(%this,%obj);
    %obj.playaudio(0,"zombie_death" @ getRandom(1,10) @ "_sound");
}

function ShireZombieBot::onBotLoop(%this,%obj)
{
    if(isObject(%obj) && %obj.getState() !$= "Dead") %obj.BotLoopSched = %this.schedule(500,onBotLoop,%obj);
    else return;

    %target = %obj.target;
    %currentTime = getSimTime();

    if(!%target && %obj.lastSearchTime < %currentTime)//Let's find a target if we don't have one
    {
        %obj.lastSearchTime = %currentTime+1500;//Scan every 1500 ms

        initContainerRadiusSearch(%obj.getPosition(), 75, $TypeMasks::PlayerObjectType);
        while((%scan = containerSearchNext()) != 0)
        {
            if(%scan == %obj || %scan.getdataBlock().isKiller || %scan.getDataBlock().className $= "PlayerData") continue;

            %line = vectorNormalize(vectorSub(%scan.getposition(),%obj.getposition()));
            %dot = vectorDot(%obj.getEyeVector(), %line );
            %obscure = containerRayCast(%obj.getEyePoint(),vectorAdd(%scan.getPosition(),"0 0 1.9"),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);

            if(!isObject(%obscure) && %dot > 0.5 && vectorDist(%obj.getposition(),%scan.getposition()) < 75)//Distance should be less than 75, and they can see them   
            {
                %obj.target = %scan;
                %target = %obj.target;            
            }
        }
    }

    //Conditions to check if target is still valid
    if(!isObject(%target) || %target.getState() $= "Dead" || %target.getDataBlock().isKiller) 
    {
        %obj.target = 0;//They either do not exist or are dead so clear the target
        %obj.setMoveY(0);
        %obj.setMoveX(0);
    }
    else if(isObject(%target) && %target.getState() !$= "Dead")
    {
        %dot = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%target.getposition(), %obj.getposition())));
        %obscure = containerRayCast(%obj.getEyePoint(),vectorAdd(%target.getPosition(),"0 0 1.9"),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);        

        if(!isObject(%obscure) && %dot > 0.5 && vectorDist(%obj.getposition(),%target.getposition()) < 50) %obj.cannotSeeTarget = 0;//We see them again, reset countdown
        else %obj.cannotSeeTarget++;//Cannot see, increase the countdown

        if(%obj.cannotSeeTarget >= 15)//Can no longer be seen and acquired, so clear the target and reset the countdown
        {
            %obj.target = 0;
            %obj.cannotSeeTarget = 0;
            %obj.clearMoveX();
            %obj.clearMoveY();            
        }
    }

    if(isObject(%target))
    {
        if(vectorDist(%obj.getposition(),%target.getposition()) < 10)//Raise arms if we are close enough
        {
            if(!%obj.raisearms)
            {
                %obj.playthread(1,"ArmReadyBoth");
                %obj.raisearms = true;
            }

            if(vectorDist(%obj.getposition(),%target.getposition()) < 5) %obj.playthread(2,"activate2");//let's start swinging            
        }
        else if(%obj.raisearms)//Too far, lower the arms once more
        {
            %obj.playthread(1,"root");
            %obj.raisearms = false;
        }
        
        if(%obj.lastTargetTime < %currentTime)//Tick every 3.5 seconds
        {
            %obj.playaudio(0,"zombie_chase" @ getRandom(0,10) @ "_sound");
            %obj.lastTargetTime = %currentTime+3500;
            %obj.setMoveY(1);
            if(getRandom(1,3) == 1) %obj.setMoveX(getRandom(-100,100)*0.01);
            else %obj.setMoveX(0);
            %obj.setHeadAngle(0);
            %obj.setMoveObject(%obj.target);
            %obj.setAimObject(%obj.targt);
        }
    }
    else if(%obj.lastIdleTime < %currentTime)//If there's no target, idle around
    {
        %obj.lastIdleTime = %currentTime+4000;        

        if(%obj.raisearms)
        {
            %obj.playthread(1,"root");
            %obj.raisearms = false;
        }

        %obj.playaudio(0,"zombie_idle" @ getRandom(0,4) @ "_sound");

        switch(getRandom(1,4))//We either look around, move, or clear our movement
        {
            case 1: %obj.maxYawSpeed = getRandom(3,8);
	                %obj.maxPitchSpeed = getRandom(3,8);

    	            %xPos = getRandom(1,5);
	                if(getRandom(0,1)) %xPos = -%xPos;

    	            %yPos = getRandom(1,5);
    	            if(getRandom(0,1)) %yPos = -%yPos;

                    %obj.setaimlocation(vectorAdd(%obj.getEyePoint(),%xPos SPC %yPos SPC getrandom(1,-1)));

                    if(getRandom(1,4) == 1)//Twitch the head around
                    {
                        %obj.setHeadAngleSpeed(0.25);
                        %obj.setHeadAngle(getRandom(-90,90));
                    }
                    else %obj.setHeadAngle(0);

            case 4: %obj.maxYawSpeed = getRandom(3,10);
                    %obj.maxPitchSpeed = getRandom(3,10);

                    %speed = getRandom(50,100)*0.01;
                    if(getRandom(1,5) == 1) %speed = -%speed;
                    %obj.setMoveY(%speed);

    	            %xPos = getRandom(1,5);
	                if(getRandom(0,1)) %xPos = -%xPos;

    	            %yPos = getRandom(1,5);
    	            if(getRandom(0,1)) %yPos = -%yPos;

                    %obj.setaimlocation(vectorAdd(%obj.getEyePoint(),%xPos SPC %yPos SPC 0));
                           
            default: %obj.clearMoveY();
                     %obj.clearMoveX();
        }
    }
}

function ShireZombieBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
    %obj.addhealth(%damage);
}

function ShireZombieBot::onCollision(%this,%obj,%col,%normal,%speed)
{
	Parent::onCollision(%this,%obj,%col,%normal,%speed);

    if(%obj.getState() !$= "Dead")
    if((%col.getType() & $TypeMasks::PlayerObjectType) && %col.getState() !$= "Dead" && !%col.getdataBlock().isKiller) 
    {
        %col.damage(%obj,%col.getWorldBoxCenter(), 30, $DamageType::Default);        
        %col.SetTempSpeed(0.5);
        %col.schedule(1000,SetTempSpeed,1);
        %col.playthread(3,"plant");

        %obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");
        cancel(%obj.BotLoopSched);
        %obj.playthread(3,"activate2");
        %obj.setMoveX(0);
        %obj.setMoveY(-0.25);
        %obj.BotLoopSched = %this.schedule(2000,onBotLoop,%obj);                 
    }
}

function ShireZombieBot::onRemove(%this,%obj)
{    
    Parent::onRemove(%this,%obj);
    %obj.spawnExplosion("PlayerSootProjectile","1.5 1.5 1.5");
}