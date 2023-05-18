package Eventide_ItemSpawning
{
    function MiniGameSO::Reset(%minigame,%client)
	{		
		Parent::Reset(%minigame,%client);
		%minigame.randomizeEventideItems(true);
	}

    function MiniGameSO::Endgame(%minigame,%client)
	{		
		Parent::Endgame(%minigame,%client);
		%minigame.randomizeEventideItems(false);
	}    
};
if(isPackage(Eventide_ItemSpawning)) deactivatePackage(Eventide_ItemSpawning);
activatePackage(Eventide_ItemSpawning);

function MiniGameSO::randomizeEventideItems(%minigame,%randomize)
{	    
    if(!isObject(EventideItemSpawnSet) || !EventideItemSpawnSet.getCount()) return;

    //Set a ritual list of items to reference later in the function
    %randomritual[%rrl++] = "candleItem";
    %randomritual[%rrl++] = "candleItem";
    %randomritual[%rrl++] = "candleItem";
    %randomritual[%rrl++] = "candleItem";
    %randomritual[%rrl++] = "bookItem";
    %randomritual[%rrl++] = "daggerItem";
        
    for(%g = 0; %g < EventideItemSpawnSet.getCount(); %g++) if(isObject(%brick = EventideItemSpawnSet.getObject(%g)))
    {
        %brick.setItem("none");
        %brick.setEmitter("none");
                
        if(%randomize) switch$(strreplace(strlwr(%brick.getname()),"_",""))
        {
            case "ritual":  if(%rrl)
                            {
                                //Choose a random ritual from the list
                                %randomnumber = getRandom(1,%rrl);
                                %randomritual = %randomritual[%randomnumber];
                                %brick.setItem(%randomritual);

                                //Remove it from the list
                                %randomritual[%randomnumber] = %randomritual[%rrl];
                                %rrl--;
                            }                      

            case "item":    if(%itemcount++ < 6) switch(getRandom(1,4))
                            {
                                case 1: %brick.setItem("CRadioItem");
                                case 2: %brick.setItem("ZombieMedpackItem");
                                case 3: %brick.setItem("SodaItem");
                                case 4: %brick.setItem("FlareGunItem");
                            }

            case "weapon":  if(%weaponcount++ < 6) switch(getRandom(1,8))
                            {
                                case 1: %brick.setItem("sm_barStoolItem");
                                case 2: %brick.setItem("sm_bottleItem");
                                case 3: %brick.setItem("sm_chairItem");
                                case 4: %brick.setItem("sm_foldingChairItem");
                                case 5: %brick.setItem("sm_poolCueItem");
                                case 6: %brick.setItem("bearTrapItem");
                                case 7: %brick.setItem("sm_chairItem");                                
                            }
        }


        if(isObject(%brick.item)) %brick.setEmitter("SparkleGroundEmitter");
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