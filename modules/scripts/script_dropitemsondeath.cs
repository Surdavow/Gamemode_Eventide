package Eventide_DropItemsOnDeath
{
	function gameConnection::onDeath(%client,%source,%killer,%type,%location)
	{
		Parent::onDeath(%client,%source,%killer,%type,%location);					
	}

	function ServerCmdDropTool (%client, %position)
	{
		%player = %client.Player;
		if (!isObject (%player))
		{
			return;
		}
		%item = %player.tool[%position];
		if (isObject (%item))
		{
			if (%item.canDrop == 1)
			{
				%zScale = getWord (%player.getScale (), 2);
				%muzzlepoint = VectorAdd (%player.getPosition (), "0 0" SPC 1.5 * %zScale);
				%muzzlevector = %player.getEyeVector ();
				%muzzlepoint = VectorAdd (%muzzlepoint, %muzzlevector);
				%playerRot = rotFromTransform (%player.getTransform ());
				%thrownItem = new Item ("")
				{
					dataBlock = %item;
				};
				%thrownItem.setScale (%player.getScale ());
				MissionCleanup.add (%thrownItem);
				%thrownItem.setTransform (%muzzlepoint @ " " @ %playerRot);
				%thrownItem.setVelocity (VectorScale (%muzzlevector, 20 * %zScale));
				%thrownItem.miniGame = %client.miniGame;
				%thrownItem.bl_id = %client.getBLID ();
				%thrownItem.setCollisionTimeout (%player);
				if(!isObject(DroppedItemSet))
				{
					new SimSet(DroppedItemSet);
					missioCleanUp.add(DroppedItemSet);
				}
				DroppedItemSet.add(%thrownItem);				
				if (%item.className $= "Weapon")
				{
					%player.weaponCount -= 1;
				}
				%player.tool[%position] = 0;
				messageClient (%client, 'MsgItemPickup', '', %position, 0);
				if (%player.getMountedImage (%item.image.mountPoint) > 0)
				{
					if (%player.getMountedImage (%item.image.mountPoint).getId () == %item.image.getId ())
					{
						%player.unmountImage (%item.image.mountPoint);
					}
				}
			}
		}
	}	

	function Armor::onDisabled(%this, %obj, %state)
	{
		Parent::onDisabled(%this, %obj, %state);

		for(%i=0;%i<%obj.getDatablock().maxTools;%i++)
		{
			if(isObject(%item = %obj.tool[%i]))
			{
				%pos = %obj.getPosition();
				%posX = getWord(%pos,0);
				%posY = getWord(%pos,1);
				%posZ = getWord(%pos,2);
				%vec = %obj.getVelocity();
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
				%item.BL_ID = %obj.client.BL_ID;
				%item.minigame = getMiniGameFromObject(%obj);
				%item.spawnBrick = -1;
				%item.setVelocity(%itemVec);						

				if(!isObject(DroppedItemSet))
				{
					new SimSet(DroppedItemSet);
					missioCleanUp.add(DroppedItemSet);
				}
				DroppedItemSet.add(%item);
			}
		}	
	}

    function MiniGameSO::Reset(%minigame,%client)
	{        
		Parent::Reset(%minigame,%client);

		for(%i = 0; %i <= DroppedItemSet.getCount(); %i++) 
		if(isObject(%item = DroppedItemSet.getObject(%i))) %item.delete();		
	}	
};
activatePackage(Eventide_DropItemsOnDeath);