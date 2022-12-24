function PlayerRenowned::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	
	%obj.schedule(1, setEnergyLevel, 0);
	%obj.setScale("1.1 1.1 1.1");
	%this.idlesounds(%obj);
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
	%obj.unhideNode("renownedeyes");
	%obj.setHeadUp(0);
}

function PlayerRenowned::idlesounds(%this,%obj)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead" || %obj.getdataBlock() !$= %this) return;
	
	%pos = %obj.getPosition();
	%radius = 100;
	%searchMasks = $TypeMasks::PlayerObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);

	%obj.playaudio(0,"renowned_Idle" @ getRandom(0,7) @ "_sound");

	while((%targetid = containerSearchNext()) != 0 )
	{
		if(%targetid == %obj) continue;
		%line = vectorNormalize(vectorSub(%targetid.getWorldBoxCenter(),%obj.getWorldBoxCenter()));
		%dot = vectorDot(%obj.getEyeVector(), %line);
		%obscure = containerRayCast(%obj.getEyePoint(),%targetid.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
		if(%dot > 0.55 && !isObject(%obscure) && Eventide_MinigameConditionalCheck(%obj,%targetid,false)) %detectedvictims++;
	}

	if(%detectedvictims) if(!%obj.isInvisible) %obj.playaudio(0,"renowned_Close" @ getRandom(0,5) @ "_sound");
	else if(!%obj.isInvisible) %obj.playaudio(0,"renowned_Amb" @ getRandom(0,7) @ "_sound");
	%obj.playthread(3,"plant");	
	cancel(%obj.idlesoundsched);
	%obj.idlesoundsched = %this.schedule(getRandom(3000,4000),idlesounds,%obj);
}

function PlayerRenowned::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
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
					%obj.client.schedule(4000,setControlObject,%obj);
					%obj.setEnergyLevel(0);
					%obj.playthread(2,"leftrecoil");
					%search.mountImage("RenownedPossessedImage",3);

					switch$(%search.getclassname())
					{
						case "Player": 	%search.client.schedule(4000,setControlObject,%search);
										%search.schedule(4000,unMountImage,3);										

						case "AIPlayer": 	%search.schedule(4000,setControlObject,%search);
											%search.schedule(4000,unMountImage,3);
					}
				}
			}		
		}
	}
	else if(%press && %obj.getEnergyLevel() < 20) %obj.playthread(0,"undo");

}