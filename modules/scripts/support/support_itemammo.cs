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

package ItemAmmo
{
	function serverCmdLight(%c)
	{
		%p = %c.player;
		if(isObject(%p) && ItemAmmo_Unload(%p))
		{
			%p.setImageLoaded(0,ItemAmmo_HasAmmo(%p));
			return "";
		}
		parent::serverCmdLight(%c);
	}
};
activatePackage("ItemAmmo");