datablock fxDTSBrickData (brickEventideItemSpawnData:brick1x1fData)
{
	category = "Special";
	subCategory = "Eventide";
	uiName = "Eventide Item Spawn";
	alwaysShowWireFrame = false;
};

function brickEventideItemSpawnData::onPlant(%data,%obj)
{
	Parent::onPlant(%data,%obj);
	%obj.setrendering(0);
	%obj.setcolliding(0);
	%obj.setraycasting(0);

    //Always check if the simset exists first, and to add it to the mission clean up for later if necessary
    if(!isObject(EventideItemSpawnSet))
    {
        new SimSet("EventideItemSpawnSet");
        missionCleanup.add(EventideItemSpawnSet);
    }
    else EventideItemSpawnSet.add(%obj);
}

function brickEventideItemSpawnData::onloadPlant(%data, %obj)
{
	brickEventideItemSpawnData::onPlant(%this, %obj);
}