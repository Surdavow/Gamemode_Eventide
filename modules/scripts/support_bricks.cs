package Eventide_Bricks
{
	function fxDTSBrick::onActivate(%obj, %player, %client, %pos, %vec)
	{
		Parent::onActivate(%obj,%player,%client,%pos,%vec);

		if(isFunction(%obj.getDataBlock().getName(),onActivate)) %obj.getDataBlock().onActivate(%obj,%player,%client,%pos,%vec);
	}

	function fxDTSBrick::onRemove(%data, %brick)
	{		
		Parent::OnRemove(%data,%brick);
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
	}

	function fxDTSBrick::onDeath(%data, %brick)
	{		
	   	Parent::onDeath(%data, %brick);
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
	}	
	
	function serverCmdUseTool(%client, %tool)
	{		
		if(!%client.player.victim) return parent::serverCmdUseTool(%client, %tool);
	}
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Bricks)) deactivatePackage(Eventide_Bricks);
activatePackage(Eventide_Bricks);