package Eventide_DropItemsOnDeath
{
	function gameConnection::onDeath(%client,%source,%killer,%type,%location)
	{
		Parent::onDeath(%client,%source,%killer,%type,%location);

		if(isObject(%client.minigame) && isObject(%client.player))		
		for(%i=0;%i<%client.player.getDatablock().maxTools;%i++)
		{
			%item = %client.player.tool[%i];
			if(isObject(%item))
			{
				%pos = %client.player.getPosition();
				%posX = getWord(%pos,0);
				%posY = getWord(%pos,1);
				%posZ = getWord(%pos,2);
				%vec = %client.player.getVelocity();
				%vecX = getWord(%vec,0);
				%vecY = getWord(%vec,1);
				%vecZ = getWord(%vec,2);
				%item = new Item()
				{
					dataBlock = %item;
					position = %pos;
				};
				%itemVec = %vec;
				%itemVec = vectorAdd(%itemVec,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
				%item.BL_ID = %client.BL_ID;
				%item.minigame = %client.minigame;
				%item.spawnBrick = -1;
				%item.setVelocity(%itemVec);						

				if(!isObject(DroppedItemSet))
				{
					new SimSet(DroppedItemSet);
					missioCleanUp.add(DroppedItemSet);
				}
				DroppedItemSet.add(%item);

			}
			%client.player.tool[%i] = "";
		}							
	}

    function MiniGameSO::Reset(%minigame,%client)
	{        
		Parent::Reset(%minigame,%client);

		for(%i = 0; %i < DroppedItemSet.getCount(); %i++) 
		if(isObject(%item = DroppedItemSet.getObject(%i))) %item.delete();		
	}	
};
activatePackage(Eventide_DropItemsOnDeath);