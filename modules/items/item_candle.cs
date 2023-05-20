datablock fxLightData(CandleLight)
{
	uiName = "Candle Light";

	LightOn = true;
	radius = 7.5;
	brightness = 1;
	color = "1 0.75 0 1";

	FlareOn			= true;
    FlareBitmap = "./models/candleLightFlare";
    FlareColor = "0.25 0.2 0";
	AnimColor		= false;
	AnimBrightness	= true;
	AnimOffsets		= true;
	AnimRotation	= false;
	LinkFlare		= false;
	LinkFlareSize	= true;
	MinColor		= "1 1 0";
	MaxColor		= "0 0 1";
	MinBrightness	= 0.0;
	MaxBrightness	= 5.0;
	MinRadius		= 0.1;
	MaxRadius		= 10;
	StartOffset		= "0 0 0";
	EndOffset		= "0 0 0";
	MinRotation		= 0;
	MaxRotation		= 359;

	SingleColorKeys	= false;
	RedKeys			= "AWTCFAH";
	GreenKeys		= "AWTCFAH";
	BlueKeys		= "AWTCFAH";
	
	BrightnessKeys	= "DEDEDFGF";
	RadiusKeys		= "AZAAAAA";
	OffsetKeys		= "AZAAAAA";
	RotationKeys	= "AZAAAAA";

	ColorTime		= 1.0;
	BrightnessTime	= 1.0;
	RadiusTime		= 1.0;
	OffsetTime		= 1.0;
	RotationTime	= 1.0;

	LerpColor		= true;
	LerpBrightness	= true;
	LerpRadius		= true;
	LerpOffset		= false;
	LerpRotation	= false;
};

datablock PlayerData(EmptyCandleBot : PlayerStandardArmor)
{
	shapeFile = "base/data/shapes/empty.dts";
	uiName = "";
	className = PlayerData;
};

datablock PlayerData(EmptyLightBot : PlayerStandardArmor)
{
	shapeFile = "base/data/shapes/empty.dts";
	uiName = "";
	className = PlayerData;
};

datablock PlayerData(CandleItemProp : PlayerStandardArmor)
{
	shapeFile = "./models/candle.dts";
	uiName = "";
    renderFirstPerson = false;
	className = PlayerData;
};

datablock ItemData(candleItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/candle.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Candle";
	iconName = "./icons/icon_candle";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	image = candleImage;
	canDrop = true;
};

datablock ShapeBaseImageData(candleImage)
{
    shapeFile = "base/data/shapes/empty.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0.0";
    correctMuzzleVector = false;
    eyeOffset = "0 0 0";
    className = "WeaponImage";

    item = candleItem;
    ammo = " ";
    projectile = "";
    projectileType = Projectile;

    staticShape = "brickCandleStaticShape";
    isRitual = true;

    melee = true;
    doRetraction = false;
    armReady = true;
    doColorShift = candleItem.doColorShift;
    colorShiftColor = candleItem.colorShiftColor;

    stateName[0]                     = "Activate";
    stateScript[0]                  = "onActivate";
    stateTimeoutValue[0]             = 0.5;
    stateTransitionOnTimeout[0]      = "Ready";

    stateName[1]                     = "Ready";
};

datablock StaticShapeData(brickCandleStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/candlestatic.dts";
	placementSound = "candle_place_sound";
};

function brickCandleStaticShape::Light(%this,%obj)
{   
    if(!isObject(%obj)) return;

    %obj.light = new fxlight()
    {
        dataBlock = "CandleLight";
        enable = true;
    };
    %obj.playAudio(1,"candleignite_sound");
    %obj.light.settransform(vectoradd(%obj.gettransform(),"0 0 1.125") SPC getwords(%obj.gettransform(),3,6));
}

function brickCandleStaticShape::onRemove(%this,%obj)
{
    if(isObject(%obj.light)) %obj.light.delete();
}

function brickCandleStaticShape::onAdd(%this,%obj)
{
    %obj.schedule(33,playaudio,3,%this.placementSound);
    %this.schedule(500,Light,%obj);
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

function EmptyLightBot::onRemove(%this,%obj)
{
    if(isObject(%obj.light)) %obj.light.delete();
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