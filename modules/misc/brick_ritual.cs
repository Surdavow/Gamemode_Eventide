datablock fxDTSBrickData (brickEventideRitual : brick16x16fData)
{
	uiName = "Ritual Shape";
	Category = "Special";
	Subcategory = "Eventide";
    iconName = "Add-Ons/Gamemode_Eventide/modules/misc/icons/icon_ritual";
};

datablock StaticShapeData(brickEventideRitualStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/rittualstatic.dts";

	gemPos1 = "2.28 2.875 0.1";
	gemPos2 = "-2.28 2.875 0.1";
	gemPos3 = "2.1 -2.725 0.1";
	gemPos4 = "-2.1 -2.725 0.1";
	candlePos1 = "0 -3.5 0.375";
	candlePos2 = "0 3.5 0.375";
	candlePos3 = "-3.25 1.1 0.375";
	candlePos4 = "3.25 1.1 0.375";
	bookPos = "0 0.65 0.1";
	daggerPos = "0 -0.65 0.1";
};

datablock fxLightData(RitualLight)
{
	uiName = "Ritual Light";
	LightOn = true;
	radius = 10;
	brightness = 5;
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

function brickEventideRitual::onActivate(%this, %obj, %player, %client, %pos, %vec)
{
	if(isObject(%obj.ritualshape) && isObject(%item = %player.getMountedImage(0)) && %item.isRitual)
	{		
		switch$(%item.staticShape)
		{
			case "brickCandleStaticShape": 	for(%i = 1; %i <= 4; %i++) 
											if(!isObject(%obj.candleshape[%i]))											
											{													
												%obj.candleshape[%i] = new StaticShape() { datablock = %item.staticShape; };
												%obj.candleshape[%i].settransform(vectoradd(%obj.gettransform(),%obj.ritualshape.getdatablock().candlePos[%i] SPC getWords(%obj.gettransform,3,6)));
												%interactiveshape = %obj.candleshape[%i];
												break;
											}
											else %candlecount++;

											if(%candlecount >= 4) return;

			case "brickBookStaticShape":	if(isObject(%obj.bookshape)) return;
											
											%obj.bookshape = new StaticShape() { datablock = %item.staticShape; };
											%transformdelta = %obj.ritualshape.getdatablock().bookPos SPC getWords(%obj.gettransform,3,6);
											%obj.bookshape.settransform(vectoradd(%obj.gettransform(),%transformdelta));
											%interactiveshape = %obj.bookshape;
											

			case "brickdaggerStaticShape":	if(isObject(%obj.daggershape)) return;
											
											%obj.daggershape = new StaticShape() { datablock = %item.staticShape; };
											%transformdelta = %obj.ritualshape.getdatablock().daggerPos SPC "0 90 0";
											%obj.daggershape.settransform(vectoradd(%obj.gettransform(),%transformdelta));
											%interactiveshape = %obj.daggershape;
											%interactiveshape.setnodecolor("ALL",%item.colorShiftColor);																			
		}

		if(%item.isGemRitual)		
		{
			for(%j = 1; %j <= 4; %j++) 
			if(!isObject(%obj.gemshape[%j]))
			{	
				%obj.gemshape[%j] = new StaticShape() { datablock = %item.staticShape; };
				%obj.gemshape[%j].settransform(vectoradd(%obj.gettransform(),%obj.ritualshape.getdatablock().gempos[%j] SPC getWords(%obj.gettransform,3,6)));
				%interactiveshape = %obj.gemshape[%j];
				%interactiveshape.setnodecolor("ALL",%item.colorShiftColor);
				break;
			}
			else %gemcount++;

			if(%gemcount >= 4) return;
		}
		
		%player.Tool[%player.currTool] = 0;
		messageClient(%player.client, 'MsgItemPickup', '', %player.currTool, 0);
		serverCmdUnUseTool(%player.client);			

		//Trigger an event if the eventide console exists
		if(isObject($EventideEventCaller))
		{
			$InputTarget_["Self"] = $EventideEventCaller;
			$InputTarget_["Player"] = %player;
			$InputTarget_["Client"] = %player.client;
			$InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
			$EventideEventCaller.processInputEvent("onRitualPlaced", %client);
		}			

		if(!isObject(EventideShapeGroup))
		{
			new ScriptGroup(EventideShapeGroup);
			missionCleanup.add(EventideShapeGroup);
		}
		EventideShapeGroup.add(%interactiveshape);

		if(EventideShapeGroup.getCount() >= $EventideRitualAmount)
		{				
			if(isObject(%minigame = getMiniGameFromObject(%player)))
			{
				%minigame.centerprintall("<font:Impact:40>\c3All rituals are complete!",3);
	
				for(%i = 0; %i < %minigame.numMembers; %i++) 
				if(isObject(%member = %minigame.member[%i])) %member.play2D("round_start_sound");
			}

			if(isObject(DroppedItemGroup)) DroppedItemGroup.delete();
	
			//lets call that console brick
			if(isObject($EventideEventCaller))
			{
				$InputTarget_["Self"] = $EventideEventCaller;
				$InputTarget_["Player"] = %player;
				$InputTarget_["Client"] = %player.client;			
				$InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
				$EventideEventCaller.processInputEvent("onAllRitualsPlaced",%client);
			}
		}		
	}
}

function brickEventideRitual::onPlant(%this, %obj)
{	
	Parent::onPlant(%this,%obj);

	%obj.setRendering(0);
	%obj.setColliding(0);
	%obj.setRaycasting(1);
	//%obj.setemitter("laseremittera");
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

	$EventideRitualAmount += 10;
}

function brickEventideRitual::onloadPlant(%this, %obj) 
{ 
	brickEventideRitual::onPlant(%this, %obj);
}

function brickEventideRitual::onDeath(%this, %obj)
{	
	Parent::onDeath(%this,%obj);

	if(isObject(%obj.ritualshape)) %obj.ritualshape.delete();
	if(isObject(%obj.bookshape)) %obj.bookshape.delete();
	if(isObject(%obj.daggershape)) %obj.daggershape.delete();
	for(%i = 1; %i <= 4; %i++) if(isObject(%obj.candleshape[%i])) %obj.candleshape[%i].delete();
	for(%j = 1; %j <= 4; %j++) if(isObject(%obj.gemshape[%j])) %obj.gemshape[%j].delete();

	$EventideRitualAmount = mClamp($EventideRitualAmount-8, 0, $EventideRitualAmount);
}

function brickEventideRitual::onRemove(%this, %obj)
{	
	Parent::onRemove(%this,%obj);
	
	if(isObject(%obj.ritualshape)) %obj.ritualshape.delete();
	if(isObject(%obj.bookshape)) %obj.bookshape.delete();
	if(isObject(%obj.daggershape)) %obj.daggershape.delete();
	for(%i = 1; %i <= 4; %i++) if(isObject(%obj.candleshape[%i])) %obj.candleshape[%i].delete();
	for(%j = 1; %j <= 4; %j++) if(isObject(%obj.gemshape[%j])) %obj.gemshape[%j].delete();

	$EventideRitualAmount = mClamp($EventideRitualAmount-8, 0, $EventideRitualAmount);	
}