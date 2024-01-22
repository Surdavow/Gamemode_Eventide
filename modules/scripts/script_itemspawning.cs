function MiniGameSO::randomizeEventideItems(%minigame)
{	    
    if(!isObject(EventideItemSpawnSet) || !EventideItemSpawnSet.getCount()) return;

    $rsg = new Scriptgroup("RitualScriptGroup");
    $rsg.add(new ScriptObject() { item = "candleitem"; });
    $rsg.add(new ScriptObject() { item = "candleitem"; });
    $rsg.add(new ScriptObject() { item = "candleitem"; });
    $rsg.add(new ScriptObject() { item = "candleitem"; });
    $rsg.add(new ScriptObject() { item = "bookItem"; });
    $rsg.add(new ScriptObject() { item = "daggerItem"; });
        
    for(%g = 0; %g < EventideItemSpawnSet.getCount(); %g++) if(isObject(%brick = EventideItemSpawnSet.getObject(%g)))
    {
        %brick.setItem("none");
        %brick.setEmitter("none");
                
        switch$(strreplace(strlwr(%brick.getname()),"_",""))
        {
            case "ritual":  %ritualbricklist[%rbl++] = %brick;

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

    if(isObject($rsg))
    {
        while($rsg.getCount()) 
        {                                                            
            %randomritual = $rsg.getObject(getRandom(0,$rsg.getCount()-1));
            
        %randomritualitem = %randomritual.item;            
        %ritualbrickrandomcount = getRandom(1, %rbl);
        %ritualbrick = %ritualbricklist[%ritualbrickrandomcount];
        %ritualbrick.setItem(%randomritualitem);
        %randomritual.delete();

        // Remove the chosen ritual brick from the list
        for (%i = %ritualbrickrandomcount; %i < %rbl; %i++) 
        {
            %ritualbricklist[%i] = %ritualbricklist[%i + 1];
        }

        %rbl--;

        }
        $rsg.delete();
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