if(LoadRequiredAddOn("Item_Medical") != $Error::None) return;

package Eventide_Medical_Package
{
    function ZombieMedpackImage::healLoop(%this, %obj)
    {
    	%bandageSlot = %obj.currTool;
    	%client = %obj.client;
    	%tool = %obj.tool[%bandageSlot];
    
    	if(isObject(%tool) && %tool.getID() == %this.item.getID())
    	{
    		%time = 3.4;
    		%obj.zombieMedpackUse += 0.1;
    
    		if(%obj.zombieMedpackUse >= %time)
    		{
    				%obj.pseudoHealth = 0;
    				%obj.setDamageLevel(0);
                    %obj.downedamount = 0;
    				%obj.tool[%bandageSlot] = 0;
    				%obj.weaponCount--;
    				%obj.setDatablock("EventidePlayer");
					if(%obj.survivorclass $= "fighter") %obj.pseudoHealth = 75;
    
    				if(isObject(%client))
    				{
    					messageClient(%client, 'MsgItemPickup', '', %bandageSlot, 0);
    					%client.setControlObject(%obj);
    				}

    				%obj.client.play2d("MEDPACK_Finish");    
    				%obj.unMountImage(%slot);
    				%obj.playThread(0, "root");
    				%obj.playThread(1, "root");
    
    				%client.zombieMedpackting = false;
    				%obj.zombieMedpackUse = false;
    				cancel(%obj.zombieMedpackSched);
    		}
    		else
    		{    		    			
				if((%obj.zombieMedpackUse * 10) % 10 == 0) 
				switch(getRandom(0, 1))
				{
					case 1: %obj.playThread(0, "activate");
					case 2: %obj.playThread(3, "activate2");
				}    			
    
    			if(isObject(%client))
    			{
    				%bars = "<color:ffaaaa>";
    				%div = 20;
    				%tens = mFloor((%obj.zombieMedpackUse / %time) * %div);
    
    				for(%a = 0; %a < %div; %a++)
    				{
    					if(%a == (%div - %tens)) %bars = %bars @ "<color:aaffaa>";
    					%bars = %bars @ "|";
    				}
    
    				commandToClient(%client, 'centerPrint', %bars, 0.25);
    			}

    			cancel(%obj.zombieMedpackSched);
    			%obj.zombieMedpackSched = %this.schedule(100, "healLoop", %obj);    
    		}
    	}
    	else
    	{
    		if(isObject(%client))
    		{
    			commandToClient(%client, 'centerPrint', "<color:ffaaaa>Heal Aborted!", 1);
    			%client.setControlObject(%obj);
    			%client.player.stopAudio(1);
    		}
    		cancel(%obj.zombieMedpackSched);
    	}
    }

    function GauzeImage::healLoop(%this, %obj)
    {
    	%bandageSlot = %obj.currTool;
    	%client = %obj.client;
    	%tool = %obj.tool[%bandageSlot];
    
    	if(isObject(%tool) && %tool.getID() == %this.item.getID())
    	{
    		%time = 2.4;
    		%obj.GauzeUse += 0.1;
    
    		if(%obj.GauzeUse >= %time)
    		{
    				%obj.pseudoHealth = 0;
    				%obj.addhealth(50);
                    %obj.downedamount = 0;
    				%obj.tool[%bandageSlot] = 0;
    				%obj.weaponCount--;
					if(%obj.survivorclass $= "fighter") %obj.pseudoHealth = 75;
    
    				if(isObject(%client))
    				{
    					messageClient(%client, 'MsgItemPickup', '', %bandageSlot, 0);
    					%client.setControlObject(%obj);
    				}

    				%obj.client.play2d("MEDPACK_Finish");
    
    				%obj.unMountImage(%slot);
    				%obj.playThread(0, "root");
    				%obj.playThread(1, "root");
    
    				%client.Gauzing = false;
    				%obj.GauzeUse = false;
    				cancel(%obj.GauzeSched);
    		}
    		else
    		{
				if((%obj.zombieMedpackUse * 10) % 10 == 0) 
				switch(getRandom(0, 1))
				{
					case 1: %obj.playThread(0, "activate");
					case 2: %obj.playThread(3, "activate2");
				}
    
    			if(isObject(%client))
    			{
    				%bars = "<color:ffaaaa>";
    				%div = 20;
    				%tens = mFloor((%obj.GauzeUse / %time) * %div);
    
    				for(%a = 0; %a < %div; %a++)
    				{
    					if(%a == (%div - %tens)) %bars = %bars @ "<color:aaffaa>";
    					%bars = %bars @ "|";
    				}
    
    				commandToClient(%client, 'centerPrint', %bars, 0.25);
    			}
    			cancel(%obj.GauzeSched);
    			%obj.GauzeSched = %this.schedule(100, "healLoop", %obj);
    
    		}
    	}
    	else
    	{
    		if(isObject(%client))
    		{
    			commandToClient(%client, 'centerPrint', "<color:ffaaaa>Heal Aborted!", 1);
    			%client.setControlObject(%obj);
    			%client.player.stopAudio(1);
    		}
    		cancel(%obj.GauzeSched);
    	}
    }
};
activatePackage(Eventide_Medical_Package);