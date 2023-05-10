datablock fxDTSBrickData (brickEventideRitual : brick16x16fData)
{
	uiName = "Ritual Shape";
	Category = "Special";
	Subcategory = "Eventide";
    iconName = "Add-Ons/Gamemode_EVentide/modules/bricks/icons/icon_ritual";
};

datablock StaticShapeData(brickEventideRitualStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/rittualstatic.dts";

	//Positions are important!
	gem1Pos = "";
	gem2Pos = "";
	gem3Pos = "";
	gem4Pos = "";
	gem5Pos = "";
	candle1Pos = "";
	candle2Pos = "";
	candle3Pos = "";
	candle4Pos = "";
	candle5Pos = "";
	bookPos = "";
	daggerPos = "";
};

datablock fxLightData(RitualLight)
{
	uiName = "Ritual Light";
	LightOn = true;
	radius = 15;
	brightness = 10;
	color = "1 0.1 0.9";
	FlareOn			= false;
	FlareTP			= false;
	Flarebitmap		= "";
	FlareColor		= "1 1 1";
	ConstantSizeOn	= false;
	ConstantSize	= 1;
	NearSize		= 1;
	FarSize			= 0.5;
	NearDistance	= 10.0;
	FarDistance		= 30.0;
	FadeTime		= 0.1;
};

function brickEventideRitual::onPlant(%this, %obj)
{	
	Parent::onPlant(%this,%obj);	

	%obj.setRendering(0);
	%obj.setColliding(0);
	%obj.setRaycasting(1);
	%obj.setemitter("laseremittera");
	%obj.setColor(17);

	%obj.ritualshape = new StaticShape()
	{
		datablock = "brickEventideRitualStaticShape";
		spawnbrick = %obj;
	};

	%obj.light = new fxlight()
	{
		dataBlock = "RitualLight";
		enable = true;
	};
	%obj.light.settransform(vectoradd(%obj.gettransform(),"0 0 0.25") SPC getwords(%obj.gettransform(),3,6));
	%obj.isLightOn = true;	

	%obj.ritualshape.position = %obj.getposition();
}

function brickEventideRitual::onloadPlant(%this, %obj) 
{ 
	brickEventideRitual::onPlant(%this, %obj);
}

function brickEventideRitual::onDeath(%this, %obj)
{	
	Parent::onDeath(%this,%obj);
	if(isObject(%obj.ritualshape)) %obj.ritualshape.delete();
}

function brickEventideRitual::onRemove(%this, %obj)
{	
	Parent::onRemove(%this,%obj);
	if(isObject(%obj.ritualshape)) %obj.ritualshape.delete();
}