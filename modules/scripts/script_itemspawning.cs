package Eventide_ItemSpawning
{
    function MiniGameSO::Reset(%minigame,%client)
	{		
		Parent::Reset(%minigame,%client);

		%currTime = getSimTime();
		if(%obj.lastResetTime + 5000 > %currTime) return;
		%minigame.lastResetTime = %currTime;	
		%minigame.randomizeEventideItems();
	}
};
activatePackage(Eventide_ItemSpawning);

function MiniGameSO::randomizeEventideItems(%minigame)
{
	if(isObject(EventideItemSpawnSet) && EventideItemSpawnSet.getCount())
	for(%g = 0; %g < EventideItemSpawnSet.getCount(); %g++) 
	if(isObject(%brick = EventideItemSpawnSet.getObject(%g)))
    {
        %brick.setEmitter("SparkleGroundEmitter");
        
        if(strstr(strlwr(%brick.getname()),"flaregun") != -1) %brick.setItem("FlareGunItem");
        if(strstr(strlwr(%brick.getname()),"candle") != -1) %brick.setItem("candleItem");		
        if(strstr(strlwr(%brick.getname()),"book") != -1) %brick.setItem("bookItem");
        if(strstr(strlwr(%brick.getname()),"gem") != -1) %brick.setItem("gem" @ getRandom(1,4) @ "Item");		    
        if(strstr(strlwr(%brick.getname()),"radio") != -1) %brick.setItem("CRadioItem");
        if(strstr(strlwr(%brick.getname()),"rope") != -1) %brick.setItem("Rope");
        if(strstr(strlwr(%brick.getname()),"soda") != -1) %brick.setItem("SodaItem");
        if(strstr(strlwr(%brick.getname()),"bar stool") != -1) %brick.setItem("sm_barStoolItem");
        if(strstr(strlwr(%brick.getname()),"bottle") != -1) %brick.setItem("sm_bottleItem");
        if(strstr(strlwr(%brick.getname()),"chair") != -1) %brick.setItem("sm_chairItem");
        if(strstr(strlwr(%brick.getname()),"dagger") != -1) %brick.setItem("daggerItem");
        if(strstr(strlwr(%brick.getname()),"folding chair") != -1) %brick.setItem("sm_foldingChairItem");
        if(strstr(strlwr(%brick.getname()),"poolcue") != -1) %brick.setItem("sm_poolCueItem");
        if(strstr(strlwr(%brick.getname()),"bear trap") != -1) %brick.setItem("bearTrapItem");
    }
}

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

    if(!isObject(EventideItemSpawnSet))
    {
        new SimSet(EventideItemSpawnSet);
        missionCleanup.add(EventideItemSpawnSet);
    }
    EventideItemSpawnSet.add(%obj);
}

function brickEventideItemSpawnData::onloadPlant(%data, %obj)
{
	brickEventideItemSpawnData::onPlant(%this, %obj);
}

function brickEventideItemSpawnData::onDeath(%this,%obj)
{
    Parent::onDeath(%this, %obj);
}