package Eventide_Items
{
	function Armor::onCollision(%this, %obj, %col, %vec, %speed)
	{
		if(!%obj.getDataBlock().isEventideModel && !isObject(getMinigameFromObject(%obj))) 
		{
			Parent::onCollision(%this, %obj, %col, %vec, %speed);
			return;
		}

		if (%obj.getState() $= "Dead" || %col.getClassName() !$= "Item") return;	

		%inventoryToolCount = (%obj.hoarderToolCount) ? %obj.hoarderToolCount : %obj.getDataBlock().maxTools;

		for (%i = 0; %i < %inventoryToolCount; %i++)
		if (%obj.tool[%i] == %col.getDataBlock()) return;				
			
		%obj.pickup(%col);
	}

	function ItemData::onPickup(%this, %obj, %user, %amount)
	{
		if(!%obj.getDataBlock().isEventideModel && !isObject(getMinigameFromObject(%obj))) 
		{
			Parent::Pickup(%obj,%item);
			return;
		}

		if (!%obj.canPickup || !isObject(%client = %user.client) || getSimTime() - %client.lastF8Time < 5000)
		return;		

		%inventoryToolCount = (%user.hoarderToolCount) ? %user.hoarderToolCount : %user.getDataBlock().maxTools;
		%canUse = miniGameCanUse(%user, %obj) ? 1 : 0;

		if (!%canUse)
		{
			if (isObject(%obj.spawnBrick)) %ownerName = %obj.spawnBrick.getGroup().name;
			%msg = %ownerName @ " does not trust you enough to use this item.";
	
			if ($lastError == $LastError::Trust) 
			%msg = %ownerName @ " does not trust you enough to use this item.";

			else if ($lastError == $LastError::MiniGameDifferent) 
			%msg = isObject(%client.miniGame) ? "This item is not part of the mini-game." : "This item is part of a mini-game.";

			else if ($lastError == $LastError::MiniGameNotYours) 
			%msg = "You do not own this item.";

			else if ($lastError == $LastError::NotInMiniGame) 
			%msg = "This item is not part of the mini-game.";
	
			commandToClient(%client, 'CenterPrint', %msg, 1);
			return;
		}

		%freeslot = -1;
		for (%i = 0; %i < %inventoryToolCount; %i++)		
		if (%user.tool[%i] == 0)
		{
			%freeslot = %i;
			break;
		}
		
		if (%freeslot != -1)
		{
			if (%obj.isStatic()) %obj.Respawn();			
			else %obj.delete();
			
			%user.tool[%freeslot] = %this;
			messageClient(%client, 'MsgItemPickup', '', %freeslot, %this.getId());
			
			return 1;
		}
	}	
	
	function Player::Pickup(%obj,%item)
	{		
		if (!%obj.getDataBlock().isEventideModel && !isObject(getMinigameFromObject(%obj))) 
		return Parent::Pickup(%obj,%item);		

		// Skinwalker players and killers can't pick up items
		if (%obj.isSkinwalker || %obj.getDataBlock().isKiller) return;

		// Hoarder class support
		%inventoryToolCount = (%obj.hoarderToolCount) ? %obj.hoarderToolCount : %obj.getDataBlock().maxTools;

		// Check if the player already has the item
		for (%i = 0; %i < %inventoryToolCount; %i++)
		if (%item.getDataBlock() == %obj.tool[%i]) return;

		// Check for an available slot in the inventory
		for (%i = 0; %i < %inventoryToolCount; %i++)
		if (!isObject(%obj.tool[%i])) 
		{
			if(%item.getDataBlock().getName() $= "CRadioItem") 
			%obj.client.centerPrint("\c4Radio equipped, keep it in your inventory to chat with other survivors.",4);
			
			%item.canPickup = false;
			%obj.tool[%i] = %item.getDataBlock();
			messageClient(%obj.client, 'MsgItemPickup', '', %i, %item.getDataBlock());
			%item.spawnBrick.setEmitter();
			%item.delete();
			return;
		}

		// Return if no slots are available (inventory is full)
		return;
	}
	
	function ItemData::onAdd(%this, %obj)	
	{				
		if(!%obj.static) itemEmitterLoop(%obj);
		parent::onAdd(%this,%obj);
	}

	function ItemData::onRemove(%this, %obj)
	{
		if(isObject(%obj.emitter)) %obj.emitter.delete();
		parent::onRemove(%this,%obj);
	}	

	function Item::schedulePop (%obj)
	{
		if(!isObject(Eventide_MinigameGroup))
		{
			new SimGroup(Eventide_MinigameGroup);
			missionCleanUp.add(Eventide_MinigameGroup);
		}
		Eventide_MinigameGroup.add(%obj);

		if(MiniGameGroup.getCount() || (isObject(Slayer_MiniGameHandlerSG) && Slayer_MiniGameHandlerSG.getCount())) return;
		else return Parent::schedulePop(%obj);		
	}	

	// Item ammo support
	function serverCmdLight(%client)
	{
		if(isObject(%client.player) && ItemAmmo_Unload(%client.player))
		{
			%client.player.setImageLoaded(0,ItemAmmo_HasAmmo(%client.player));
			return "";
		}
		parent::serverCmdLight(%client);
	}
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_Items)) deactivatePackage(Eventide_Items);
activatePackage(Eventide_Items);

function ItemAmmo_AmmoImage::onSelect(%data,%obj,%slot)
{
	%selected = %obj.ItemAmmo_Selected;
	if(getWord(%selected,0) $= %data.item.getId() && getWord(%selected,1) == %obj.currTool)
	{
		%obj.playThread(2,"shiftTo");
		ItemAmmo_Select(%obj);
		
	}
	else
	{
		%obj.playThread(2,"shiftAway");
		ItemAmmo_Select(%obj,%data.item);
	}
}

function ItemAmmo_ContainerImage::onSelect(%data,%obj,%slot)
{
	ItemAmmo_AmmoImage::onSelect(%data,%obj,%slot);
}

function ItemAmmo_ContainerImage::PopAmmo(%data,%obj,%slot)
{
	if(!%data.ItemAmmo_Init)
	{
		
	}

	%d = %obj.ItemDI(%obj.currTool);
	%list = %d.ItemAmmo_storage;
	%count = getWordCount(%list);
	for(%i = 0; %i < %count; %i++)
	{
		%curr = getWord(%list,%i);
		if(%curr.ItemAmmo_projectile !$= "")
		{
			%d.ItemAmmo_storageUnits -= %curr.ItemAmmo_storageUnits;
			%d.ItemAmmo_storage = removeWord(%d.ItemAmmo_storage,%i);

			%data = %d.DataInstance(%i);
			%d.DataInstance_set(%i);
			return %curr SPC %data;
		}
	}
}

function ItemAmmo_WeaponImage::OnMount(%data,%obj,%slot)
{
	%obj.setImageLoaded(%slot,ItemAmmo_HasAmmo(%obj));
	ItemAmmo_Display(%obj);
}

function ItemAmmo_WeaponImage::getReload(%data,%obj,%slot)
{
	%obj.setImageAmmo(%slot,ItemAmmo_CanUseAmmo(%obj));
}

function ItemAmmo_WeaponImage::onFire(%data,%obj,%slot)
{
	%projDB = getWord(ItemAmmo_PopAmmo(%obj),0).ItemAmmo_projectile;
	%pos = %obj.getMuzzlePoint(0);
	%vel = vectorScale(%obj.getMuzzleVector(0),%projDB.muzzleVelocity);
	%vel = vectorAdd(%vel,vectorScale(%obj.getVelocity(),%projDB.velInheritFactor));
	%p = new projectile()
	{
		dataBlock = %projDB;
		initialPosition = %pos;
		initialVelocity = %vel;
		sourceObject = %obj;
		sourceSlot = %slot;
		client = %obj.client;
	};
	%obj.setImageLoaded(%slot,ItemAmmo_HasAmmo(%obj));
	ItemAmmo_Display(%obj);
}

function ItemAmmo_WeaponImage::onDryFire(%data,%p,%slot)
{
	ItemAmmo_Display(%obj);
	return "";
}

function ItemAmmo_WeaponImage::OnReloadFail(%data,%obj,%slot)
{
	ItemAmmo_Display(%obj);
	return "";
}

function ItemAmmo_WeaponImage::OnReloadFinish(%data,%obj,%slot)
{
	%obj.setImageAmmo(%slot,false);
	ItemAmmo_PushSelected(%obj);
	%obj.setImageLoaded(%slot,ItemAmmo_HasAmmo(%obj));
	ItemAmmo_Display(%obj);
}

function ItemAmmo_Select(%obj,%item)
{
	if(isObject(%item))
	{
		%item = %item.getId();
	}
	%obj.ItemAmmo_Selected = %item SPC %obj.currTool;
	ItemAmmo_Display(%obj);
}	

function ItemAmmo_Display(%obj)
{
	%s = "";

	%c = %obj.client;
	%selected = %obj.ItemAmmo_Selected;
	%name = getWord(%selected,0).uiName;
	if(isObject(%c))
	{
		if(ItemAmmo_HasAmmo(%obj))
		{
			%s = %s @ "<just:right>\c3Loaded (" @ %obj.ItemDI(%obj.currTool).ItemAmmo_storageUnits @
			"/" @ %obj.tool[%obj.currTool].ItemAmmo_maxStorageUnits @ ")\n";
		}
		
		if(%name !$= "")
		{
			%s = %s @ "<just:right>\c3" @ %name SPC "Prepared\n";
		}
		%c.centerPrint(%s,4);
	}
}

function ItemAmmo_HasAmmo(%obj)
{
	%d = %obj.ItemDI(%obj.currTool);
	%list = %d.ItemAmmo_storage;
	%count = getWordCount(%list);
	for(%i = 0; %i < %count; %i++)
	{
		%curr = getWord(%list,%i);
		if(%curr.ItemAmmo_projectile !$= "")
		{
			return true;
		}
	}
	return false;
}

function ItemAmmo_PopAmmo(%obj)
{
	%d = %obj.ItemDI(%obj.currTool);
	%list = %d.ItemAmmo_storage;
	%count = getWordCount(%list);
	for(%i = 0; %i < %count; %i++)
	{
		%curr = getWord(%list,%i);
		if(%curr.ItemAmmo_projectile !$= "")
		{
			%d.ItemAmmo_storageUnits -= %curr.ItemAmmo_storageUnits;
			%d.ItemAmmo_storage = removeWord(%d.ItemAmmo_storage,%i);

			%data = %d.DataInstance(%i);
			%d.DataInstance_set(%i);
			return %curr SPC %data;
		}
	}
	return false;
}

function ItemAmmo_CanReload(%obj)
{
	%weapon = %obj.tool[%obj.currTool];
	%ammo = getWord(%obj.ItemAmmo_Selected,0);
	%ammoSlot = getWord(%obj.ItemAmmo_Selected,1);
	return %weapon.ItemAmmo_ammoTypes !$= "" && %ammo.ItemAmmo_ammo !$= "" && %obj.tool[%ammoSlot] == %ammo;
}

function ItemAmmo_CanUseAmmo(%obj)
{
	%weapon = %obj.tool[%obj.currTool];
	%ammo = getWord(%obj.ItemAmmo_Selected,0);
	%ammoSlot = getWord(%obj.ItemAmmo_Selected,1);

	if(findString(%weapon.ItemAmmo_ammoTypes, " ",  %ammo.ItemAmmo_ammo) == -1)
	{
		//ammo mismatch
		return false;
	}

	%d = %obj.ItemDI(%obj.currTool);
	if((%d.ItemAmmo_storageUnits + %ammo.ItemAmmo_storageUnits + 0) > %weapon.ItemAmmo_maxStorageUnits)
	{
		//too full
		return  false;
	}
	return true;
}

function ItemAmmo_PushSelected(%obj)
{
	%weapon = %obj.tool[%obj.currTool];
	%ammo = getWord(%obj.ItemAmmo_Selected,0);
	%ammoSlot = getWord(%obj.ItemAmmo_Selected,1);
	%d = %obj.ItemDI(%obj.currTool);

	ItemAmmo_Select(%obj);

	//add ammo to storage
	%d.ItemAmmo_storageUnits += %ammo.ItemAmmo_storageUnits;
	%d.ItemAmmo_storage = trim(%d.ItemAmmo_storage SPC %ammo);
	%d.DataInstance_Add(%obj.ItemDI(%ammoSlot));
	%obj.ItemDI().DataInstance_Set(%ammoSlot);

	//remove ammo item
	%obj.tool[%ammoSlot] = 0;
	%c = %obj.client;
	if(isObject(%c))
	{
		messageClient (%c, 'MsgItemPickup', '', %ammoSlot, 0);
	}
}

function ItemAmmo_Unload(%p)
{
	if(%p.getMountedImage(0).classname !$= "ItemAmmo_WeaponImage")
	{
		return false;
	}

	if(ItemAmmo_CanReload(%p))
	{
		%p.setImageAmmo(0,true);
		return true;
	}

	if(ItemAmmo_HasAmmo(%p))
	{
		%ammo = ItemAmmo_PopAmmo(%p);
		ItemAmmo_Display(%p);
		%count = %p.getDatablock().maxTools;
		for(%i = 0; %i < %count; %i++)
		{
			if(%p.tool[%i] == 0)
			{
				%p.tool[%i] = getWord(%ammo,0);
				%p.ItemDI().dataInstance_set(%i,getWord(%ammo,1));

				%c = %p.client;
				if(isObject(%c))
				{
					messageClient(%c, 'MsgItemPickup', '', %i, %p.tool[%i]);
				}
				return true;
			}
		}

		%p.tool[100] = getWord(%ammo,0);
		%p.ItemDI().dataInstance_set(100,getWord(%ammo,1));
		%c = %p.client;
		if(isObject(%c))
		{
			ServerCmdDropTool(%c,100);
		}
		return true;
	}

	return false;
}
