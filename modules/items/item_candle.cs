function brickCandleStaticShape::onRemove(%this,%obj)
{
    if(isObject(%obj.spawnbrick)) %obj.spawnbrick.light.delete();
}

function brickCandleData::onPlant(%this, %obj)
{	
	Parent::onPlant(%this,%obj);
	%obj.setrendering(0);
}

function brickCandleData::onloadPlant(%this, %obj) 
{ 
	brickCandleData::onPlant(%this, %obj); 
}

function brickCandleData::ToggleLight(%this,%obj,%bool)
{
    if(%bool)
    {        
        %obj.light = new fxlight()
        {
            dataBlock = "CandleLight";
            enable = true;
        };
        %obj.interactiveshape.playAudio(1,"candleignite_sound");
        %obj.light.settransform(vectoradd(%obj.gettransform(),"0 0 0.9") SPC getwords(%obj.gettransform(),3,6));
        %obj.isLightOn = true;
    }
    else
    {
        if(isObject(%obj.light)) %obj.light.delete();
        %obj.interactiveshape.playAudio(1,"candleextinguish_sound");
        %obj.isLightOn = false;
    }
}

function candleImage::onActivate(%this, %obj, %slot)
{
    %obj.emptycandlebot = new Player() 
    { 
        dataBlock = "EmptyCandleBot";
        source = %obj;
        slotToMountBot = 0;
    };

    %obj.playaudio(0,"WeaponSwitchsound");
	%obj.playthread(2, "plant");
}

function candleImage::onLight(%this, %obj, %slot)
{
	%obj.playthread(2, "shiftRight");
    %obj.emptycandlebot.candlebot.getDataBlock().ToggleLight(%obj.emptycandlebot.candlebot,true);
}

function candleImage::onExtinguish(%this, %obj, %slot)
{
    %obj.emptycandlebot.candlebot.getDataBlock().ToggleLight(%obj.emptycandlebot.candlebot,false);
    if(isObject(%obj.emptycandlebot.candlebot.light)) %obj.emptycandlebot.candlebot.light.delete();
}

function candleImage::onUnmount(%this,%obj,%slot)
{
    if(isObject(%obj.emptycandlebot)) %obj.emptycandlebot.delete();
    Parent::onUnmount(%this,%obj,%slot);
}

function EmptyCandleBot::onAdd(%this, %obj) 
{
	%obj.setDamageLevel(%this.maxDamage);

	if(isObject(%source = %obj.source))
    { 
        %source.mountObject(%obj,%obj.slotToMountBot);

        %obj.candlebot = new Player() 
        { 
            dataBlock = "CandleItemProp";
            source = %obj;
            slotToMountBot = %obj.slotToMountBot;
        };        
    }
	else
	{
		%obj.delete();
		return;
	}
}

function EmptyCandleBot::doDismount(%this, %obj, %forced) 
{
	return;
}

function EmptyCandleBot::onDisabled(%this, %obj) 
{
	return;
}

function EmptyCandleBot::onRemove(%this,%obj)
{
    if(isObject(%obj.candlebot)) %obj.candlebot.delete();
}

function CandleItemProp::onAdd(%this, %obj) 
{
	%obj.setDamageLevel(%this.maxDamage);

	if(isObject(%source = %obj.source)) %source.mountObject(%obj,%obj.slotToMountBot);	
	else
	{
		%obj.delete();
		return;
	}
}

function CandleItemProp::ToggleLight(%this,%obj,%bool)
{
    if(%bool)
    {        
        %obj.light = new fxlight()
        {
            dataBlock = "CandleLight";
            enable = true;
            iconsize = 1;
        };
        %obj.light.attachToObject(%obj);
        %obj.playAudio(0,"candleignite_sound");
    }
    else
    {
        if(isObject(%obj.light)) %obj.light.delete();
        %obj.playAudio(1,"candleextinguish_sound");
    }
}

function CandleItemProp::doDismount(%this, %obj, %forced) 
{
	return;
}

function CandleItemProp::onDisabled(%this, %obj) 
{
	return;
}

function CandleItemProp::onRemove(%this,%obj)
{
    if(isObject(%obj.light)) %obj.light.delete();
}