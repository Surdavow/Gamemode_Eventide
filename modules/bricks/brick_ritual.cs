datablock fxDTSBrickData (brickEventideRitual : brick16x16fData)
{
	uiName = "Ritual Shape";
	Category = "Special";
	Subcategory = "Eventide";
    iconName = "Add-Ons/Gamemode_Eventide/modules/bricks/icons/icon_ritual";
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

function brickEventideRitual::BrickText(%this, %obj, %name, %color, %distance, %client)
{
    %player = %client.player;

    if (isObject(%obj.textShape))
    {
        %obj.textShape.setShapeName(%name);
        %obj.textShape.setShapeNameColor(%color);
        %obj.textShape.setShapeNameDistance(%distance);
        %obj.bricktext = %name;
    }
    else
    {
        %obj.textShape = new StaticShape()
        {
            datablock = BrickTextEmptyShape;
            position = vectorAdd(%obj.getPosition(), "0 0" SPC %obj.getDatablock().brickSizeZ / 9 + "0.166");
            scale = "0.1 0.1 0.1";
        };

        %obj.textShape.setShapeName(%name);
        %obj.textShape.setShapeNameColor(%color);        
        %obj.textShape.setShapeNameDistance(%distance);
        %obj.bricktext = %name;
    }
}

function brickEventideRitual::ritualCheck(%this,%obj)
{
	if(!isObject(%obj)) return;

	%obj.BrickText("Place your rituals by dropping them here!", "1 1 0", "25");	

	if(%obj.ritualsPlaced < 10 && isObject(%minigame = getMiniGameFromObject($EventideEventCaller.getGroup().client)))
	{
		initContainerRadiusSearch(%obj.getPosition(), 2.5, $TypeMasks::ItemObjectType | $TypeMasks::PlayerObjectType);		
		while(%scan = containerSearchNext())
		{
			// If the player is near the ritual and the ritual is not complete, give a message.
			if((%scan.getType() & $TypeMasks::PlayerObjectType)&& isObject(Eventide_MinigameGroup) && Eventide_MinigameGroup.getCount() < 10)
			{
				%obj.BrickText("Rituals needed: " @ 10-%obj.ritualsPlaced, "1 1 0", "20");
				continue;
			}

			// Make sure the item is above the ritual
			if(getWord(%scan.getPosition(),2) < getWord(%obj.getPosition(),2)) continue;

			%itemimage = %scan.getdatablock().image;
			if(!%itemimage.isRitual) continue;			

			if(!isObject(Eventide_MinigameGroup))
			{
				new ScriptGroup(Eventide_MinigameGroup);
				missionCleanup.add(Eventide_MinigameGroup);
			}

			if(%itemimage.isGemRitual)		
			{
				if(%obj.gemcount <= 4 && !isObject(%obj.gemshape[%obj.gemcount+1]))
				{	
					%obj.gemcount++;
					%obj.gemshape[%obj.gemcount] = new StaticShape() { datablock = %itemimage.staticShape; };
					%obj.gemshape[%obj.gemcount].settransform(vectoradd(%obj.gettransform(),%obj.ritualshape.getdatablock().gempos[%obj.gemcount] SPC getWords(%obj.gettransform,3,6)));
					%interactiveshape = %obj.gemshape[%obj.gemcount];
					%interactiveshape.setnodecolor("ALL",%itemimage.colorShiftColor);
					Eventide_MinigameGroup.add(%interactiveshape);
				}
				else continue;			
			}
			else switch$(%itemimage.staticShape)
			{
				case "brickCandleStaticShape":  if(%obj.candlecount <= 4 && !isObject(%obj.candleshape[%obj.candlecount+1]))											
												{
													%obj.candlecount++;										
													%obj.candleshape[%obj.candlecount] = new StaticShape() { datablock = %itemimage.staticShape; };
													%obj.candleshape[%obj.candlecount].settransform(vectoradd(%obj.gettransform(),%obj.ritualshape.getdatablock().candlePos[%obj.candlecount] SPC getWords(%obj.gettransform,3,6)));
													%interactiveshape = %obj.candleshape[%obj.candlecount];
													Eventide_MinigameGroup.add(%interactiveshape);
												}
												else continue;

				case "brickBookStaticShape":	if(isObject(%obj.bookshape)) continue;

												%obj.bookshape = new StaticShape() { datablock = %itemimage.staticShape; };
												%transformdelta = %obj.ritualshape.getdatablock().bookPos SPC getWords(%obj.gettransform,3,6);
												%obj.bookshape.settransform(vectoradd(%obj.gettransform(),%transformdelta));
												%interactiveshape = %obj.bookshape;
												Eventide_MinigameGroup.add(%interactiveshape);


				case "brickdaggerStaticShape":	if(isObject(%obj.daggershape)) continue;

												%obj.daggershape = new StaticShape() { datablock = %itemimage.staticShape; };
												%transformdelta = %obj.ritualshape.getdatablock().daggerPos SPC "0 90 0";
												%obj.daggershape.settransform(vectoradd(%obj.gettransform(),%transformdelta));
												%interactiveshape = %obj.daggershape;
												%interactiveshape.setnodecolor("ALL",%itemimage.colorShiftColor);
												Eventide_MinigameGroup.add(%interactiveshape);													
			}

			//Trigger an event if the eventide console exists
			if(isObject($EventideEventCaller))
			{
				$InputTarget_["Self"] = $EventideEventCaller;
				$InputTarget_["MiniGame"] = getMiniGameFromObject($EventideEventCaller.getGroup().client);
				$EventideEventCaller.processInputEvent("onRitualPlaced", $EventideEventCaller.getGroup().client);
			}

			%obj.ritualsPlaced++;
			%scan.delete();

			if(%obj.ritualsPlaced >= 10)
			{
				%minigame.centerprintall("<font:Impact:40>\c3All rituals are complete!",3);
				%minigame.playSound("round_start_sound");				

				if(isObject($EventideEventCaller))
				{
					$InputTarget_["Self"] = $EventideEventCaller;		
					$InputTarget_["MiniGame"] = getMiniGameFromObject($EventideEventCaller.getGroup().client);
					$EventideEventCaller.processInputEvent("onAllRitualsPlaced",$EventideEventCaller.getGroup().client);
				}
			}
		}
	}

	// Cancel the ritual check to prevent duplicate calls.
	cancel(%obj.ritualCheck);
	%obj.ritualCheck = %this.schedule(500,ritualCheck,%obj);
}

function brickEventideRitual::clearRituals(%this, %obj)
{	
	Parent::onRemove(%this,%obj);
	
	%obj.ritualshape.delete();
	%obj.bookshape.delete();
	%obj.daggershape.delete();
}

function brickEventideRitual::onPlant(%this, %obj)
{	
	Parent::onPlant(%this,%obj);

	%this.ritualCheck(%obj);
	$EventideRitualBrick = %obj;

	%obj.setRendering(0);
	%obj.setColliding(0);
	%obj.setRaycasting(1);
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

function brickEventideRitual::onRemove(%this, %obj)
{	
	Parent::onRemove(%this,%obj);
	
	if(isObject(%obj.ritualshape)) %obj.ritualshape.delete();
	if(isObject(%obj.bookshape)) %obj.bookshape.delete();
	if(isObject(%obj.daggershape)) %obj.daggershape.delete();
	for(%candlecount = 1; %candlecount <= 4; %candlecount++) if(isObject(%obj.candleshape[%candlecount])) %obj.candleshape[%candlecount].delete();
	for(%gemcount = 1; %gemcount <= 4; %gemcount++) if(isObject(%obj.gemshape[%gemcount])) %obj.gemshape[%gemcount].delete();
}

function brickEventideRitual::onDeath(%this, %obj)
{	
	Parent::onRemove(%this,%obj);
}