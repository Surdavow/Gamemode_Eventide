function PlayerGrabber::onNewDatablock(%this,%obj)
{
	%obj.schedule(10,KillerScanCheck);

	if(!isObject(%obj.client)) applyDefaultCharacterPrefs(%obj);
	else applyCharacterPrefs(%obj.client);
	%obj.schedule(1,setEnergyLevel,0);
	%obj.setScale("1.15 1.15 1.15");
}

function PlayerGrabber::checkVictim(%this,%obj)
{
	if(!%obj.victim) %obj.playthread(3,"root");
	%obj.setdatablock("PlayerGrabber");
}

function PlayerGrabber::onTrigger(%this,%obj,%triggerNum,%bool)
{	
	Parent::onTrigger(%this,%obj,%triggerNum,%bool);
	
	if(%bool && %obj.getState() !$= "Dead")
	switch(%triggerNum)
	{
		case 4: if(!isObject(%obj.victim))
				{
					if(!%obj.isCrouched() && %obj.getEnergyLevel() >= %this.maxEnergy && getWord(%obj.getVelocity(),2) == 0)
					{
						%obj.setdatablock("PlayerGrabberNoJump");					
						%obj.setVelocity(vectorscale(%obj.getForwardVector(),35));
						%obj.playaudio(1,"grabber_lunge_sound");
						%obj.playthread(3,"armReadyLeft");
						%this.schedule(500,checkVictim,%obj);
					}
				}
				else if(%obj.lastChokeTime < getSimTime() && isObject(%obj.victim))
				{					
					if(%obj.ChokeAmount < 4)
					{
						if(isObject(%obj.victim) && %obj.victim.getState() !$= "Dead") %obj.victim.damage(%obj, %obj.getmuzzlePoint(1), 8, $DamageType::Default);

						%obj.lastChokeTime = getSimTime()+250;	
						%obj.playthread(0,"plant");
						%obj.ChokeAmount++;
					}
					else 
					{
						%obj.ChokeAmount = 0;
						%obj.setEnergyLevel(0);
						%obj.victim.unmount();
						%obj.victim.playthread(0,"root");
						%obj.playthread(3,"leftrecoil");
						%obj.victim.setVelocity(vectorscale(vectorAdd(%obj.getEyeVector(),"0 0 0.005"),24));				
						%obj.victim.position = %obj.getEyePoint();

						switch$(%obj.victim.getClassName())
						{
							case "AIPlayer":	%obj.victim.startholeloop();
												%obj.victim.hRunAwayFromPlayer(%obj);
							case "Player":	%obj.victim.client.schedule(100,setControlObject,%obj.victim);
						}
						
						%obj.victim.killer = 0;
						%obj.victim = 0;
					}
				}

		default:
	}
}

function PlayerGrabber::EventideAppearance(%this,%obj,%client)
{
	Parent::EventideAppearance(%this,%obj,%client);
	%obj.setDecalName("classicshirt");
	%shirtColor = "0.28 0.21 0.12 1";
	%pantsColor = "0.075 0.075 0.075 1";
	%obj.HideNode($hat[%client.hat]);
	%obj.HideNode($accent[%client.accent]);
	%obj.HideNode($secondPack[%client.secondPack]);
	%obj.hideNode($pack[%client.pack]);
	%obj.HideNode("visor");
	%obj.unHideNode("jasonmask");
	%obj.setNodeColor("jasonmask","0.75 0.75 0.75 1");	
	%obj.setNodeColor($hat[%client.hat],%shirtColor);
	%obj.setNodeColor((%client.rarm ? "rarmSlim" : "rarm"),%shirtColor);
	%obj.setNodeColor((%client.larm ? "larmSlim" : "larm"),%shirtColor);
	%obj.setNodeColor((%client.chest ? "femChest" : "chest"),%shirtColor);
	%obj.setNodeColor("pants",%pantsColor);
	%obj.setNodeColor((%client.rleg ? "rpeg" : "rshoe"),%pantsColor);
	%obj.setNodeColor((%client.lleg ? "lpeg" : "lshoe"),%pantsColor);
	%obj.setNodeColor((%client.lleg ? "lpeg" : "lshoe"),%pantsColor);
	%obj.setHeadUp(0);
}

function PlayerGrabberNoJump::onCollision(%this,%obj,%col,%vec,%speed)
{
	Parent::onCollision(%this,%obj,%col,%vec,%speed);

	if(!isObject(%obj.victim) && Eventide_MinigameConditionalCheck(%obj,%col,false))
	{
		if(minigameCanDamage(%obj,%col))
		{
			ServerCmdUnUseTool (%obj.client);
			%obj.victim = %col;
			%col.killer = %obj;
			%obj.mountObject(%col,8);
			%col.playaudio(0,"grabber_scream_sound");

			switch$(%col.getClassName())
			{
				case "AIPlayer": %col.stopholeloop();
				case "Player": 	%col.client.camera.setOrbitMode(%col, %col.getTransform(), 0, 5, 0, 1);
								%col.client.setControlObject(%col.client.camera);
			}
		}							
	}	
}

function PlayerGrabberNoJump::onNewDatablock(%this,%obj)
{
	PlayerGrabber::onNewDatablock(%this,%obj);
}

function PlayerGrabberNoJump::checkVictim(%this,%obj)
{
	PlayerGrabber::checkVictim(%this,%obj);
	%obj.setdatablock("PlayerGrabber");
}

function PlayerGrabberNoJump::onTrigger(%this,%obj,%triggerNum,%bool)
{	
	PlayerGrabber::onTrigger(%this,%obj,%triggerNum,%bool);
}

function PlayerGrabberNoJump::EventideAppearance(%this,%obj,%client)
{
	PlayerGrabber::EventideAppearance(%this,%obj,%client);
}