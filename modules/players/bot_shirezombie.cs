function ShireZombieBot::applyAppearance(%this,%obj)
{
    Armor::EventideAppearance(%this,%obj,%obj.ghostclient);

    %ghostClient = %obj.ghostclient;
	%skin = %ghostClient.headColor;
	%zskin = getWord(%skin,0)/2.75 SPC getWord(%skin,1)/1.5 SPC getWord(%skin,2)/2.75 SPC 1;
    %headColor = %zskin;
    %chestColor = %ghostClient.chestColor;
    %rarmcolor = %ghostClient.rarmColor;
    %larmcolor = %ghostClient.larmColor;
    %rhandcolor = %ghostClient.rhandColor;
    %lhandcolor = %ghostClient.lhandColor;
    %hipcolor = %ghostClient.hipColor;
    %rlegcolor = %ghostClient.rlegColor;
    %llegColor = %ghostClient.llegColor;

    if(%ghostClient.chestColor $= %skin) %chestColor = %zskin;
    if(%ghostClient.rArmColor $= %skin) %rarmcolor = %zskin;
    if(%ghostClient.lArmColor $= %skin) %larmcolor = %zskin;
    if(%ghostClient.rhandColor $= %skin) %rhandcolor = %zskin;
    if(%ghostClient.lhandColor $= %skin) %lhandcolor = %zskin;
    if(%ghostClient.hipColor $= %skin) %hipcolor = %zskin;
    if(%ghostClient.rLegColor $= %skin) %rlegcolor = %zskin;
    if(%ghostClient.lLegColor $= %skin) %llegColor = %zskin;	

    %obj.setFaceName(%faceName);
    %obj.setNodeColor("headskin",%headcolor);
    %obj.setNodeColor((%client.chest ? "femChest" : "chest"),%chestcolor);
    %obj.setNodeColor("pants",%hipcolor);
    %obj.setNodeColor("rarm",%rarmcolor);
    %obj.setNodeColor("larm",%larmcolor);
    %obj.setNodeColor("rhand",%rhandcolor);
    %obj.setNodeColor("lhand",%lhandcolor);
    %obj.setNodeColor("rshoe",%rlegcolor);
    %obj.setNodeColor("lshoe",%llegcolor);
    %obj.setNodeColor("pants",%hipcolor);    
    %obj.setFaceName("asciiterror");        
}

function ShireZombieBot::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	
    %obj.setMoveSlowdown(0);
	%this.applyAppearance(%obj,%obj.ghostclient);
    %this.onBotLoop(%obj);
}

function ShireZombieBot::onDisabled(%this,%obj)
{
	Parent::onDisabled(%this,%obj);
    %obj.playaudio(0,"zombie_death_" @ getRandom(1,10) @ "_sound");
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
            if(%scan == %obj) continue;

            %line = vectorNormalize(vectorSub(%scan.getposition(),%obj.getposition()));
            %dot = vectorDot(%obj.getEyeVector(), %line );
            %obscure = containerRayCast(%obj.getEyePoint(),vectorAdd(%scan.getPosition(),"0 0 1.9"),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);

            if(!isObject(%obscure) && %dot > 0.5 && vectorDist(%obj.getposition(),%scan.getposition()) < 75 && !%scan.getDataBlock().isKiller)//Distance should be less than 75, and they can see them   
            {
                %obj.target = %scan;            
                %target = %obj.target;
            }
        }
    }

    //Conditions to check if target is still valid
    if(!isObject(%target) || %target.getState() $= "Dead") %obj.target = 0;//They either do not exist or are dead so clear the target
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
            %obj.playaudio(0,"zombie_attack_" @ getRandom(1,15) @ "_sound");
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

        %obj.playaudio(0,"zombie_amb_" @ getRandom(1,5) @ "_sound");

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

    if(isObject(%sourceObject.sourceObject))
    if(!%sourceObject.sourceObject.isKiller) %obj.target = %sourceObject.sourceObject;
}

function ShireZombieBot::onCollision(%this,%obj,%col,%normal,%speed)
{
	Parent::onCollision(%this,%obj,%col,%normal,%speed);

    if((%col.getType() & $TypeMasks::PlayerObjectType) && %col.getState() !$= "Dead" && !%col.getdataBlock().isKiller) 
    {
        %col.damage(%obj,%col.getWorldBoxCenter(), 15, $DamageType::Default);
        %col.ShiredSlowDown(2.5);
        %obj.delete();
    }
}

function ShireZombieBot::onRemove(%this,%obj)
{    
    Parent::onRemove(%this,%obj);
    %obj.spawnExplosion("goryExplosionProjectile",%obj.getScale());
}

function Player::ShiredSlowDown(%obj,%slowdowndivider)
{						
	if(!isObject(%obj) || %obj.getstate() $= "Dead") return;

	%datablock = %obj.getDataBlock();
	%obj.setMaxForwardSpeed(%datablock.MaxForwardSpeed/%slowdowndivider);
	%obj.setMaxSideSpeed(%datablock.MaxSideSpeed/%slowdowndivider);
	%obj.setMaxBackwardSpeed(%datablock.maxBackwardSpeed/%slowdowndivider);

	%obj.setMaxCrouchForwardSpeed(%datablock.maxForwardCrouchSpeed/%slowdowndivider);
  	%obj.setMaxCrouchBackwardSpeed(%datablock.maxSideCrouchSpeed/%slowdowndivider);
  	%obj.setMaxCrouchSideSpeed(%datablock.maxSideCrouchSpeed/%slowdowndivider);

 	%obj.setMaxUnderwaterBackwardSpeed(%datablock.MaxUnderwaterBackwardSpeed/%slowdowndivider);
  	%obj.setMaxUnderwaterForwardSpeed(%datablock.MaxUnderwaterForwardSpeed/%slowdowndivider);
  	%obj.setMaxUnderwaterSideSpeed(%datablock.MaxUnderwaterForwardSpeed/%slowdowndivider);

	cancel(%obj.resetSpeedSched);
	%obj.resetSpeedSched = %obj.schedule(2000,ShiredSlowDown,1);
}