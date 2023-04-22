function PlayerRenowned::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	
	%obj.mountImage("meleeTantoImage",0);
	%obj.schedule(1, setEnergyLevel, 0);
	%obj.setScale("1.1 1.1 1.1");
}

function PlayerRenowned::EventideAppearance(%this,%obj,%client)
{
	Parent::EventideAppearance(%this,%obj,%client);
	%obj.setFaceName("renownedface");
	%obj.setDecalName("renowneddecal");
	
	%skinColor = "0.83 0.73 0.66 1";

	if(%obj.chest)
	{
		%obj.hideNode("femchest");
		%obj.unhideNode("chest");		
	}	

	%obj.setNodeColor("headskin",%skinColor);
	%obj.setNodeColor("Rhand",%skinColor);
	%obj.setNodeColor("Lhand",%skinColor);
	%obj.setNodeColor("Rhook",%skinColor);
	%obj.setNodeColor("Lhook",%skinColor);
	%obj.hideNode($hat[%client.hat]);
	%obj.HideNode($accent[%client.accent]);
	%obj.HideNode($secondPack[%client.secondPack]);
	%obj.hideNode($pack[%client.pack]);
	%obj.HideNode("visor");
	%obj.HideNode("rpeg");
	%obj.HideNode("lpeg");
	%obj.unHideNode("rshoe");
	%obj.unHideNode("lshoe");
	%obj.unhideNode("renownedeyes");
	%obj.setHeadUp(0);
}

function PlayerRenowned::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%trig == 0 && %press) Eventide_Melee(%this,%obj,3.5);
	
	if(%trig == 4 && %obj.getEnergyLevel() == %this.maxEnergy)
	{
		if(%press)
		{
			%obj.playthread(2,"armReadyLeft");
			%obj.casttime = getSimTime();
			%obj.channelcasthand = %obj.schedule(500, setNodeColor, lHand, "0.8 0.8 0.5 1");
			%obj.channelcasthandimage = %obj.schedule(500,mountImage,"RenownedCastImage",2);
		}
		else
		{
			%obj.unmountImage(2);
			cancel(%obj.channelcasthand);
			cancel(%obj.channelcasthandimage);
			%this.EventideAppearance(%obj,%obj.client);
	
			if(%obj.casttime+500 < getSimTime())
			{								
				%start = %obj.getEyePoint();			
				%end = VectorAdd (%start, VectorScale (%obj.getEyeVector(),getWord(%obj.getScale(),2)*40));
				%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::ItemObjectType;
				%search = containerRayCast (%start, %end, %mask, %obj);

				if(isObject(%search) && Eventide_MinigameConditionalCheck(%obj,%search,false)) 
				{
					%obj.client.setControlObject(%search);
					%obj.returnObserveSchedule = %obj.client.schedule(4000,setControlObject,%obj);

					%search.client.centerprint("<color:FFFFFF><font:Impact:40>You are being controlled, try to break free!",2);
					%search.Possesser = %obj;
					%search.isPossessed = true;
					%obj.setEnergyLevel(0);
					%obj.playthread(2,"leftrecoil");
					%search.mountImage("RenownedPossessedImage",3);

					switch$(%search.getclassname())
					{
						case "Player": 	%search.client.schedule(4000,setControlObject,%search);
										%search.schedule(4000,unMountImage,3);
										%search.schedule(4000,%search.AntiPossession = 0);

						case "AIPlayer": 	%search.schedule(4000,setControlObject,%search);
											%search.schedule(4000,unMountImage,3);
					}
				}
			}		
		}
	}
	else if(%press && %obj.getEnergyLevel() < 20) %obj.playthread(0,"undo");

}