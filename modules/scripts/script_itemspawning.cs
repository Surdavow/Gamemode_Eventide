function MiniGameSO::randomizeEventideItems(%minigame)
{	    
    if(!isObject(EventideItemSpawnSet) || !EventideItemSpawnSet.getCount()) return;

    %rsg = new ScriptGroup();
    %rbs = new SimSet();
    %rsg.add(new ScriptObject("script_candleitem"));
    %rsg.add(new ScriptObject("script_candleitem"));
    %rsg.add(new ScriptObject("script_candleitem"));
    %rsg.add(new ScriptObject("script_candleitem"));
    %rsg.add(new ScriptObject("script_bookItem"));
    %rsg.add(new ScriptObject("script_daggerItem"));
        
    //Main function to spawn items, clear effects and prepare ritual brick for the next condition check
    for(%g = 0; %g < EventideItemSpawnSet.getCount(); %g++) if(isObject(%brick = EventideItemSpawnSet.getObject(%g)))
    {
        %brick.setItem("none");
        %brick.setEmitter("none");

        switch$(strlwr(%brick.getname()))
        {
            case "_ritual": //This main loop only iterates through one brick at a time which causes problems if we try to randomize ritual spawns, only add it to the simset for the next loop
                            %rbs.add(%brick);

            case "_item":   switch(getRandom(1,8))
                            {
                                case 1: %brick.setItem("CRadioItem");
                                case 2: %brick.setItem("ZombieMedpackItem");
                                case 3: %brick.setItem("SodaItem");
                                case 4: %brick.setItem("FlareItem");
								case 5: %brick.setItem("GauzeItem");
                                default:
                            }

            case "_weapon": switch(getRandom(1,24))
                            {
                                case 1: %brick.setItem("sm_barStoolItem");
                                case 2: %brick.setItem("sm_bottleItem");
                                case 3: %brick.setItem("sm_chairItem");
                                case 4: %brick.setItem("sm_poolCueItem");
                                case 5: %brick.setItem("sm_chairItem");                                
                                case 6: %brick.setItem("FlareGunItem");
								case 7: %brick.setItem("mine_bearItem");
								case 8: %brick.setItem("grenade_flashbangItem");
                                case 9: %brick.setItem("StunGun");
                                default:
                            }
            default:                        
        }

        if(isObject(%brick.item)) %brick.setEmitter("SparkleGroundEmitter");
    }
    
    // Get a random ritual and brick, and set the item of the brick to the ritual, then delete the script object and remove the brick from the set
    while(%rsg.getCount()) 
    {                 
        //If the ritual brick set fails to meet the amount of rital script group, stop the loop
        if(!%rbs.getCount()) break;     
        
        %randomritual = %rsg.getObject(getRandom(0,%rsg.getCount()-1));
        %randomritualbrick = %rbs.getObject(getRandom(0,%rbs.getCount()-1));
        %randomritualitem = strreplace(%randomritual.getName(),"script_","");        
        %randomritualbrick.setItem(%randomritualitem);
		%randomritualbrick.setEmitter("SparkleGroundEmitter");

        %randomritual.delete();
        %rbs.remove(%randomritualbrick);
    }

     // Always clean up ScriptGroup and SimSet, so adding them to the mission cleanup won't be necessary
    %rsg.delete();
    %rbs.delete();
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

    //Always check if the simset exists first, and to add it to the mission clean up for later if necessary
    if(!isObject(EventideItemSpawnSet))
    {
        new SimSet("EventideItemSpawnSet");
        missionCleanup.add(EventideItemSpawnSet);
    }
    EventideItemSpawnSet.add(%obj);
}

function brickEventideItemSpawnData::onloadPlant(%data, %obj)
{
	brickEventideItemSpawnData::onPlant(%this, %obj);
}