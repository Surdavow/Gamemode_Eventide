function MiniGameSO::randomizeEventideItems(%minigame,%randomize)
{	    
    if(!isObject(EventideItemSpawnSet) || !EventideItemSpawnSet.getCount()) return;

    $ritualscriptgroup = new Scriptgroup("Ritualscriptgroup");
    
    $ritualscriptgroup.add(new ScriptObject() { item = "candleitem"; });
    $ritualscriptgroup.add(new ScriptObject() { item = "candleitem"; });
    $ritualscriptgroup.add(new ScriptObject() { item = "candleitem"; });
    $ritualscriptgroup.add(new ScriptObject() { item = "candleitem"; });
    $ritualscriptgroup.add(new ScriptObject() { item = "bookItem"; });
    $ritualscriptgroup.add(new ScriptObject() { item = "daggerItem"; });
        
    for(%g = 0; %g < EventideItemSpawnSet.getCount(); %g++) if(isObject(%brick = EventideItemSpawnSet.getObject(%g)))
    {
        %brick.setItem("none");
        %brick.setEmitter("none");
                
        if(%randomize) switch$(strreplace(strlwr(%brick.getname()),"_",""))
        {
            case "ritual":  if(%r)
                            {
                                if(getRandom(0,1) == 1)
                                {
                                    %randomritual = $ritualscriptgroup.getObject(getRandom(0,$ritualscriptgroup.getCount()-1));
                                    %randomritualitem = %randomritual.item;
                                    %brick.setItem(%randomritual);
                                    %randomritual.delete();
                                }
                                %r--;
                            }
                            else if(isObject($ritualscriptgroup)) $ritualscriptgroup.delete();

            case "item":    switch(getRandom(1,5))
                            {
                                case 1: %brick.setItem("CRadioItem");
                                case 2: %brick.setItem("ZombieMedpackItem");
                                case 3: %brick.setItem("SodaItem");
                                case 4: %brick.setItem("FlareItem");
								case 5: %brick.setItem("ZombiePillsItem");
                            }

            case "weapon":  switch(getRandom(1,7))
                            {
                                case 1: %brick.setItem("sm_barStoolItem");
                                case 2: %brick.setItem("sm_bottleItem");
                                case 3: %brick.setItem("sm_chairItem");
                                case 4: %brick.setItem("sm_poolCueItem");
                                case 5: %brick.setItem("sm_chairItem");                                
                                case 6: %brick.setItem("FlareGunItem");
								case 7: %brick.setItem("mine_bearItem");
								case 8: %brick.setItem("sm_foldingChairItem");
                                case 9: %brick.setItem("StunGun");
                            }
            default:                        
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