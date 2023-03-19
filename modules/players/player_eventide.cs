function EventidePlayer::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(1,setEnergyLevel,0);
	%obj.setScale("1 1 1");
}

function EventidePlayer::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%press)
	{ 
		switch(%trig)
		{
			case 0:	%ray = containerRayCast(%obj.getEyePoint(), vectoradd(%obj.getEyePoint(),vectorscale(%obj.getEyeVector(),5*getWord(%obj.getScale(),2))),$TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType,%obj);
					if(isObject(%ray) && (%ray.getClassName() $= "Player" || %ray.getClassName() $= "AIPlayer") && %ray.getdataBlock().isDowned)
					{
						%obj.isSaving = %ray;
						%obj.playthread(2,"armReadyRight");
						%ray.isBeingSaved = true;
						%this.SaveVictim(%obj,%ray,%press);
					}
			case 4: if(%obj.isSkinwalker && %obj.getEnergyLevel() >= %this.maxEnergy && !isObject(%obj.victim) && !isEventPending(%obj.monstertransformschedule)) 
					PlayerSkinwalker.monstertransform(%obj,true);
		}
	}
	else if(isObject(%obj.isSaving)) %this.SaveVictim(%obj,%obj.isSaving,0);
}

function EventidePlayer_SaveCounterPrint(%client,%amount)
{
    if(!isobject(%client)) return;
	
	%addsymbol = "";
    %symbol = "|";
	for(%i = 0; %i < %amount; %i++) %addsymbol = %addsymbol @ %symbol;

    %client.centerprint("<color:FFFFFF><font:impact:40> Get up! <br><color:00e100>" @ %addsymbol,1);
}

function EventidePlayer::SaveVictim(%this,%obj,%victim,%bool)
{
	if(%bool && vectorDist(%obj.getPosition(),%victim.getPosition()) < 5)
	{		
		if(%obj.savevictimcounter <= 4)
		{
			%obj.savevictimcounter++;
			EventidePlayer_SaveCounterPrint(%obj.client,%obj.savevictimcounter);
			EventidePlayer_SaveCounterPrint(%victim.client,%obj.savevictimcounter);

			cancel(%obj.SaveVictimSched);
			%obj.SaveVictimSched = %this.schedule(1000,SaveVictim,%obj,%victim,%bool);
		}
		else
		{
			%obj.savevictimcounter = 0;
			if(isObject(%obj.client)) %obj.client.centerprint("<color:FFFFFF><font:impact:40>You revived" SPC %victim.client.name,1);
			if(isObject(%victim.client)) %victim.client.centerprint("<color:FFFFFF><font:impact:40>You were revived by" SPC %obj.client.name,1);
			%victim.setHealth(25);
			%victim.setDatablock("EventidePlayer");
			%victim.playthread(0,"root");
			return;
		}					
	}
	else
	{
		cancel(%obj.SaveVictimSched);
		%obj.isSaving = 0;
		%obj.savevictimcounter = 0;
		%victim.isBeingSaved = false;
		%obj.playthread(2,"root");
		return;
	}	
}

function EventidePlayer::EventideAppearance(%this,%obj,%client)
{
    if(%obj.isSkinwalker && isObject(%obj.victimreplicatedclient)) Parent::EventideAppearance(%this,%obj,%obj.victimreplicatedclient);
    else Parent::EventideAppearance(%this,%obj,%client);
}

function EventidePlayer::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType)
{
    if(%obj.getState() !$= "Dead" && %damage+%obj.getdamageLevel() >= %this.maxDamage && %damage < mFloor(%this.maxDamage/1.33))
    {        
        %obj.setDatablock("EventidePlayerDowned");
        %obj.setHealth(100);
        return;
    }

    Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType);
}

function EventidePlayerDowned::onNewDataBlock(%this,%obj)
{		
	%this.DownLoop(%obj);
    %obj.playthread(0,sit);
	Parent::onNewDataBlock(%this,%obj);
}

function EventidePlayerDowned::DownLoop(%this,%obj)
{ 
	if(isobject(%obj) && %obj.getstate() !$= "Dead" && %obj.getdataBlock().isDowned)
	{
		if(!%obj.isBeingSaved)
		{
			%obj.addhealth(-5);
			%obj.setdamageflash(0.25);

			if(%obj.lastcry+10000 < getsimtime())
			{
				%obj.lastcry = getsimtime();
				%obj.playaudio(0,"grabber_scream_sound");
				%obj.playthread(3,"plant");
			}			
		}
	
		cancel(%obj.downloop);
		%obj.downloop = %this.schedule(1000,DownLoop,%obj);
	}
	else return;
}

function EventidePlayer::onDisabled(%this,%obj)
{
	EventidePlayerDowned::onDisabled(%this,%obj);
}

function EventidePlayerDowned::onDisabled(%this,%obj)
{	
	Parent::onDisabled(%this,%obj);

	if(isObject(%obj.client)) %obj.ghostclient = %obj.client;
}

function EventidePlayer::onRemove(%this,%obj)
{
	EventidePlayerDowned::onRemove(%this,%obj);
}

function EventidePlayerDowned::onRemove(%this,%obj)
{	
	Parent::onRemove(%this,%obj);

	if(%obj.markedForShireZombify)
	if(isObject(%obj.ghostclient) && isObject(%obj.ghostClient.minigame))
	{
		%bot = new AIPlayer()
		{
			dataBlock = "ShireZombieBot";
			minigame = %obj.ghostClient.minigame;
			ghostclient = %obj.ghostclient;
		};

		%bot.setTransform(%obj.getTransform());
		%obj.spawnExplosion("goryExplosionProjectile",%obj.getScale());

		if(!isObject(Shire_BotGroup))
		{
    		new SimGroup(Shire_BotGroup);
    		missionCleanup.add(Shire_BotGroup);
			Shire_BotGroup.add(%bot);
		}
		else if(!Shire_BotGroup.isMember(%bot)) Shire_BotGroup.add(%bot);
	}
}