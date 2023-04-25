exec("./datablocks_misc.cs");
exec("./player_eventide.cs");
exec("./player_renowned.cs");
exec("./player_angler.cs");
exec("./player_grabber.cs");
exec("./player_shire.cs");
exec("./bot_shirezombie.cs");
exec("./player_skinwalker.cs");
exec("./player_skullwolf.cs");

function Player::SetSpeedModifier(%obj,%a)
{
	if(%obj.Speed_Modifier $= "")
	{
		%obj.Speed_Modifier = 1;
	}

	if(%a <= 0)
	{
		return;
	}

	%prev = %obj.Speed_Modifier;
	%curr = %obj.Speed_Modifier = %a;
	%mod = (1 / %prev) * %curr;
	%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() * %mod);
	%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() * %mod);
	%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() * %mod);
	%obj.setMaxCrouchForwardSpeed(%obj.getMaxCrouchForwardSpeed() * %mod);
	%obj.setMaxCrouchBackwardSpeed(%obj.getMaxCrouchBackwardSpeed() * %mod);
	%obj.setMaxCrouchSideSpeed(%obj.getMaxCrouchSideSpeed() * %mod);
	%obj.setMaxUnderwaterForwardSpeed(%obj.getMaxUnderwaterForwardSpeed() * %mod);
	%obj.setMaxUnderwaterBackwardSpeed(%obj.getMaxUnderwaterBackwardSpeed() * %mod);
	%obj.setMaxUnderwaterSideSpeed(%obj.getMaxUnderwaterSideSpeed() * %mod);
}

function Player::AddMoveSpeedModifier(%obj,%a)
{
	if(%obj.Speed_Modifier $= "")
	{
		%obj.Speed_Modifier = 1;
	}

	%obj.SetSpeedModifier(%obj.Speed_Modifier + %a);
}

function Eventide_MinigameConditionalCheck(%objA,%objB,%exemptDeath)//exemptdeath is to skip checking if the victim is dead
{
	if((%objA.getClassName() $= "Player" || %objA.getClassName() $= "AIPlayer") && (%objB.getClassName() $= "Player" || %objB.getClassName() $= "AIPlayer"))
	{
		if(%exemptDeath) 
		{
			if(%objA.getstate() !$= "Dead" && %objA.getdataBlock().isKiller && !%objB.getdataBlock().isKiller)		
			if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB) 
			return true;
		}
		else 
		if(%objA.getstate() !$= "Dead" && %objB.getstate() !$= "Dead" && %objA.getdataBlock().isKiller && !%objB.getdataBlock().isKiller)
		{
			if(isObject(%minigameA = getMinigamefromObject(%objA)) && isObject(%minigameB = getMinigamefromObject(%objB)) && %minigameA == %minigameB)
			return true;
		}
	}

	return false;
}

function Player::SetTempSpeed(%obj,%slowdowndivider)
{						
	if(!isObject(%obj) || %obj.getstate() $= "Dead") return;

	%datablock = %obj.getDataBlock();
	%obj.setMaxForwardSpeed(%datablock.MaxForwardSpeed*%slowdowndivider);
	%obj.setMaxSideSpeed(%datablock.MaxSideSpeed*%slowdowndivider);
	%obj.setMaxBackwardSpeed(%datablock.maxBackwardSpeed*%slowdowndivider);

	%obj.setMaxCrouchForwardSpeed(%datablock.maxForwardCrouchSpeed*%slowdowndivider);
  	%obj.setMaxCrouchBackwardSpeed(%datablock.maxSideCrouchSpeed*%slowdowndivider);
  	%obj.setMaxCrouchSideSpeed(%datablock.maxSideCrouchSpeed*%slowdowndivider);

 	%obj.setMaxUnderwaterBackwardSpeed(%datablock.MaxUnderwaterBackwardSpeed*%slowdowndivider);
  	%obj.setMaxUnderwaterForwardSpeed(%datablock.MaxUnderwaterForwardSpeed*%slowdowndivider);
  	%obj.setMaxUnderwaterSideSpeed(%datablock.MaxUnderwaterForwardSpeed*%slowdowndivider);
}

function Eventide_Melee(%this,%obj,%radius)
{
	if(!%obj.isInvisible && %obj.lastclawed+500 < getSimTime() && %obj.getEnergyLevel() >= %this.maxEnergy/8)
	{
		switch$(%obj.getdataBlock().getName())
		{
			case "PlayerRenowned": %obj.playaudio(3,"renowned_melee" @ getRandom(0,2) @ "_sound");
			default:
		}

		%obj.lastclawed = getSimTime();							
		%obj.playthread(2,"activate2");
		%oscale = getWord(%obj.getScale(),2);
		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
		initContainerRadiusSearch(%obj.getPosition(),15,%mask);
		while(%hit = containerSearchNext())
		{
			if(%hit == %obj) continue;
			%line = vectorNormalize(vectorSub(%hit.getWorldBoxCenter(),%obj.getWorldBoxCenter()));
			%dot = vectorDot(%obj.getEyeVector(), %line);
			%obscure = containerRayCast(%obj.getEyePoint(),%hit.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);

			if((!isObject(%obscure) && %dot > 0.65 && vectorDist(%obj.getposition(),%hit.getposition()) < %radius) || vectorDist(%obj.getposition(),%hit.getposition()) < 1)
			if(Eventide_MinigameConditionalCheck(%obj,%hit,true))
			{						
				if(%hit.getstate() $= "Dead")
				{
					if(%obj.getdataBlock().getName() $= "PlayerSkullwolf") 
					{
						%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");	
						%obj.playthread(3,"plant");
						%obj.setEnergyLevel(%obj.getEnergyLevel()+%this.maxEnergy/6);
						%hit.spawnExplosion("goryExplosionProjectile",%hit.getScale());
						%hit.schedule(50,delete);
					}
					continue;
				}

				switch$(%obj.getdataBlock().getName())
				{
					case "PlayerAngler": 	%obj.playaudio(0,"angler_Atk" @ getRandom(0,2) @ "_sound");
											%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");
											if(isObject(%obj.hookrope)) %obj.hookrope.delete();																			
					case "PlayerSkullWolf": %obj.playaudio(0,"skullwolf_Atk" @ getRandom(0,6) @ "_sound");
											%obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");
					case "PlayerShire":		serverPlay3d("melee_axe_0" @ getRandom(1,2) @ "_sound", %hit.getPosition());			
					case "PlayerGrabber": serverPlay3d("melee_machete_0" @ getRandom(1,2) @ "_sound", %hit.getPosition());
										serverPlay3d("melee_machete_0" @ getRandom(1,2) @ "_sound", %hit.getPosition());
					case "PlayerRenowned": serverPlay3d("melee_tanto_0" @ getRandom(1,3) @ "_sound", %hit.getPosition());
										serverPlay3d("melee_tanto_0" @ getRandom(1,3) @ "_sound", %hit.getPosition());										
					case "PlayerSkinwalker": %obj.playaudio(3,"skullwolf_hit" @ getRandom(1,3) @ "_sound");
					default:
				}

				%obj.setEnergyLevel(%obj.getEnergyLevel()-%this.maxEnergy/8);
				%hit.setvelocity(vectorscale(VectorNormalize(vectorAdd(%obj.getForwardVector(),"0" SPC "0" SPC "0.15")),15));								
				%hit.damage(%obj, %hit.getWorldBoxCenter(), 25*%oscale, $DamageType::Default);
				%hit.spawnExplosion(pushBroomProjectile,"2 2 2");

				%obj.setTempSpeed(0.65);
				%obj.schedule(500,setTempSpeed,1);
			}								
		}
	}
}

function Armor::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");
	%obj.unHideNode((%client.chest ? "femChest" : "chest"));	
	%obj.unHideNode((%client.rhand ? "rhook" : "rhand"));
	%obj.unHideNode((%client.lhand ? "lhook" : "lhand"));
	%obj.unHideNode((%client.rarm ? "rarmSlim" : "rarm"));
	%obj.unHideNode((%client.larm ? "larmSlim" : "larm"));
	%obj.unHideNode("headskin");

	if($pack[%client.pack] !$= "none")
	{
		%obj.unHideNode($pack[%client.pack]);
		%obj.setNodeColor($pack[%client.pack],%client.packColor);
	}
	if($secondPack[%client.secondPack] !$= "none")
	{
		%obj.unHideNode($secondPack[%client.secondPack]);
		%obj.setNodeColor($secondPack[%client.secondPack],%client.secondPackColor);
	}
	if($hat[%client.hat] !$= "none")
	{
		%obj.unHideNode($hat[%client.hat]);
		%obj.setNodeColor($hat[%client.hat],%client.hatColor);
	}
	if(%client.hip)
	{
		%obj.unHideNode("skirthip");
		%obj.unHideNode("skirttrimleft");
		%obj.unHideNode("skirttrimright");
	}
	else
	{
		%obj.unHideNode("pants");
		%obj.unHideNode((%client.rleg ? "rpeg" : "rshoe"));
		%obj.unHideNode((%client.lleg ? "lpeg" : "lshoe"));
	}

	%obj.setHeadUp(0);
	if(%client.pack+%client.secondPack > 0) %obj.setHeadUp(1);
	if($hat[%client.hat] $= "Helmet")
	{
		if(%client.accent == 1 && $accent[4] !$= "none")
		{
			%obj.unHideNode($accent[4]);
			%obj.setNodeColor($accent[4],%client.accentColor);
		}
	}
	else if($accent[%client.accent] !$= "none" && strpos($accentsAllowed[$hat[%client.hat]],strlwr($accent[%client.accent])) != -1)
	{
		%obj.unHideNode($accent[%client.accent]);
		%obj.setNodeColor($accent[%client.accent],%client.accentColor);
	}

	if (%obj.bloody["lshoe"]) %obj.unHideNode("lshoe_blood");
	if (%obj.bloody["rshoe"]) %obj.unHideNode("rshoe_blood");
	if (%obj.bloody["lhand"]) %obj.unHideNode("lhand_blood");
	if (%obj.bloody["rhand"]) %obj.unHideNode("rhand_blood");
	if (%obj.bloody["chest_front"]) %obj.unHideNode((%client.chest ? "fem" : "") @ "chest_blood_front");
	if (%obj.bloody["chest_back"]) %obj.unHideNode((%client.chest ? "fem" : "") @ "chest_blood_back");

	%obj.setFaceName(%client.faceName);
	%obj.setDecalName(%client.decalName);

	%obj.setNodeColor("headskin",%client.headColor);	
	%obj.setNodeColor("chest",%client.chestColor);
	%obj.setNodeColor("femChest",%client.chestColor);
	%obj.setNodeColor("pants",%client.hipColor);
	%obj.setNodeColor("skirthip",%client.hipColor);	
	%obj.setNodeColor("rarm",%client.rarmColor);
	%obj.setNodeColor("larm",%client.larmColor);
	%obj.setNodeColor("rarmSlim",%client.rarmColor);
	%obj.setNodeColor("larmSlim",%client.larmColor);
	%obj.setNodeColor("rhand",%client.rhandColor);
	%obj.setNodeColor("lhand",%client.lhandColor);
	%obj.setNodeColor("rhook",%client.rhandColor);
	%obj.setNodeColor("lhook",%client.lhandColor);	
	%obj.setNodeColor("rshoe",%client.rlegColor);
	%obj.setNodeColor("lshoe",%client.llegColor);
	%obj.setNodeColor("rpeg",%client.rlegColor);
	%obj.setNodeColor("lpeg",%client.llegColor);
	%obj.setNodeColor("skirttrimright",%client.rlegColor);
	%obj.setNodeColor("skirttrimleft",%client.llegColor);

	//Set blood colors.
	%obj.setNodeColor("lshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("rshoe_blood", "0.7 0 0 1");
	%obj.setNodeColor("lhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("rhand_blood", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("chest_blood_back", "0.7 0 0 1");
	%obj.setNodeColor("femchest_blood_front", "0.7 0 0 1");
	%obj.setNodeColor("femchest_blood_back", "0.7 0 0 1");
}

function Player::gazeLoop(%obj)
{
		cancel(%obj.gazeLoop);   //Kill off duplicate processes if any are trailing behind us.
		if(!isObject(%obj) || !isObject(%client = %obj.client)) return;
		if(%obj.getState() $= "Dead")
		{
			if(isObject(%obj.client.lastGazed))
			{
				$InputTarget_Player = %obj;
				$InputTarget_Client = %obj.client;
			if($Server::LAN || getMinigameFromObject(%last = %obj.client.lastGazed) == getMinigameFromObject(%obj.client))
				$InputTarget_Minigame = getMinigameFromObject(%obj.client);
			else
				$InputTarget_Minigame = 0;
			if(%last.getClassName() $= "fxDtsBrick")
			{
				$InputTarget_Self = %last;
				%last.processInputEvent("onGazeStop", %client);
			}
			else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
			{
				$InputTarget_Self = %last.spawnBrick;
				$InputTarget_Bot = %last;
				%last.spawnBrick.processInputEvent("onGazeStop_Bot");
			}
			%obj.client.lastGazed = "";
		}
	}
	%obj.gazeLoop = %obj.schedule(10, "gazeLoop");
	%eye = vectorScale(%obj.getEyeVector(), $Pref::Server::GazeRange);
	%pos = %obj.getEyePoint();
	%mask = $TypeMasks::TerrainObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::FxBrickObjectType;
	%hit = firstWord(containerRaycast(%pos, vectorAdd(%pos, %eye), %mask, %obj));
	%client = %obj.client;
	%last = %client.lastGazed;
	if(%client.cantGaze && isObject(%last))
	{
		//These events are really fucking abusable - admin wand immunity.
		//Disabled the wand immunity because it breaks events.
		//if(%obj.hasWandImmunity(%last))
		//	return;
		if(%last.getClassName() $= "fxDtsBrick")
		{
			$InputTarget_Player = %obj;
			$InputTarget_Client = %client;
			if($Server::LAN || getMinigameFromObject(%last) == getMinigameFromObject(%client))
				$InputTarget_Minigame = getMinigameFromObject(%client);
			else
				$InputTarget_Minigame = 0;
			if(%last.getClassName() $= "fxDtsBrick")
			{
				$InputTarget_Self = %last;
				%last.processInputEvent("onGazeStop", %client);
			}
			else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
			{
				$InputTarget_Self = %last.spawnBrick;
				$InputTarget_Bot = %last;
				%last.spawnBrick.processInputEvent("onGazeStop_Bot", %client);
			}
		}
		%client.lastGazed = "";
		commandToClient(%client, 'clearBottomPrint');
	}
	if(!%client.cantGaze)
	{
		if(!isObject(%hit))
		{
			if(isObject(%last))
			{
				//These events are really fucking abusable - admin wand immunity.
				//Disabled the wand immunity because it breaks events.
				//if(%obj.hasWandImmunity(%last))
				//	return;
				$InputTarget_Player = %obj;
				$InputTarget_Client = %client;
				if($Server::LAN || getMinigameFromObject(%last) == getMinigameFromObject(%client))
					$InputTarget_Minigame = getMinigameFromObject(%client);
				else
					$InputTarget_Minigame = 0;
				if(%last.getClassName() $= "fxDtsBrick")
				{
					$InputTarget_Self = %last;
					%last.processInputEvent("onGazeStop", %client);
				}
				else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
				{
					$InputTarget_Self = %last.spawnBrick;
					$InputTarget_Bot = %last;
					%last.spawnBrick.processInputEvent("onGazeStop_Bot", %client);
				}
				%client.lastGazed = "";
				commandToClient(%client, 'clearBottomPrint');
			}
			return;
		}
		if(%hit != %last)
		{
			if(%hit.getClassName() $= "fxDtsBrick")
			{
				if(isObject(%last))
				{
					$InputTarget_Player = %obj;
					$InputTarget_Client = %client;
					if($Server::LAN || getMinigameFromObject(%last) == getMinigameFromObject(%client))
						$InputTarget_Minigame = getMinigameFromObject(%client);
					else
						$InputTarget_Minigame = 0;
					if(%last.getClassName() $= "fxDtsBrick")
					{
						$InputTarget_Self = %last;
						%last.processInputEvent("onGazeStop", %client);
					}
					else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
					{
						$InputTarget_Self = %last.spawnBrick;
						$InputTarget_Bot = %last;
						%last.spawnBrick.processInputEvent("onGazeStop_Bot", %client);
					}
				}
				if($Pref::Server::GazeMode & 2)
				{
					//These events are really fucking abusable - admin wand immunity.
					if(%obj.hasWandImmunity(%hit))
						return;
					$InputTarget_Self = %hit;
					$InputTarget_Player = %obj;
					$InputTarget_Client = %client;
					if($Server::LAN || getMinigameFromObject(%hit) == getMinigameFromObject(%client))
						$InputTarget_Minigame = getMinigameFromObject(%client);
					else
						$InputTarget_Minigame = 0;
					%hit.processInputEvent("onGazeStart", %client);
				}
			}
			else if(%hit.getClassName() $= "AIPlayer" && isObject(%spawn = %hit.spawnBrick))
			{
				if(isObject(%last))
				{
					$InputTarget_Player = %obj;
					$InputTarget_Client = %client;
					if($Server::LAN || getMinigameFromObject(%last) == getMinigameFromObject(%client))
						$InputTarget_Minigame = getMinigameFromObject(%client);
					else
						$InputTarget_Minigame = 0;
					if(%last.getClassName() $= "fxDtsBrick")
					{
						$InputTarget_Self = %last;
						%last.processInputEvent("onGazeStop", %client);
					}
					else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
					{
						$InputTarget_Self = %last.spawnBrick;
						$InputTarget_Bot = %last;
						%last.spawnBrick.processInputEvent("onGazeStop_Bot", %client);
					}
				}
				if($Pref::Server::GazeMode & 1)
				{
					//These events are really fucking abusable - admin wand immunity.
					if(%obj.hasWandImmunity(%hit))
						return;
					$InputTarget_Self = %spawn;
					$InputTarget_Player = %obj;
					$InputTarget_Client = %client;
					$InputTarget_Bot = %hit;
					if($Server::Lan || getMinigameFromObject(%hit) == getMinigameFromObject(%client))
						$InputTarget_Minigame = getMinigameFromObject(%client);
					else
						$InputTarget_Minigame = 0;
					%spawn.processInputEvent("onGazeStart_Bot", %client);
				}
			}
			else
				commandToClient(%client, 'clearBottomPrint');
			%client.lastGazed = %hit;
		}
		%name = %hit.getGazeName(%client);
		if(%name !$= "")
			if((%hit.getClassName() $= "Player" && $Pref::Server::GazeMode & 1) || (%hit.getClassName() $= "fxDtsBrick" && $Pref::Server::GazeMode & 2))
			{
				%client.gazing = 1;
				%client.bottomPrint("\c6"@%name, 2);
				%client.gazing = 0;
			}
	}
}

function Player::onKillerLoop(%obj)
{    
    if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getdataBlock().isKiller || !isObject(getMinigamefromObject(%obj))) return;

	if(%obj.getClassName() $= "Player") %obj.KillerGhostLightCheck();

	%this = %obj.getdataBlock();
	%pos = %obj.getPosition();
	%radius = 100;
	%searchMasks = $TypeMasks::PlayerObjectType;	

    InitContainerRadiusSearch(%obj.getPosition(), 40, $TypeMasks::PlayerObjectType);
    while(%scan = containerSearchNext())//Detect if players exist and if the killer can see them
    {
        if(%scan == %obj || !isObject(getMinigamefromObject(%scan)) || %scan.getdataBlock().isKiller) continue;
        %line = vectorNormalize(vectorSub(%scan.getmuzzlePoint(2),%obj.getEyePoint()));
        %dot = vectorDot(%obj.getEyeVector(), %line);
        %killerclient = %obj.client;
        %victimclient = %scan.client;

        //Not very efficient and messy code right now, will redo this sometime
        if(ContainerSearchCurrRadiusDist() <= 17 && %dot > 0.45 && !isObject(containerRayCast(%obj.getEyePoint(),%scan.getmuzzlePoint(2),$TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType,%obj)))
        {
            %killercansee[%cansee++] = %scan;
            %chasing = true;            
            
            if(!%obj.isInvisible)
            {
                if(isObject(%victimclient))
                {
                    if(%victimclient.musicChaseLevel != 2 && %killercansee[%cansee] == %scan)
                    {
                        %victimclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl2Music);
                        %victimclient.musicChaseLevel = 2;
                    }

                    %victimclient.player.TimeSinceChased = getSimTime();
                    cancel(%victimclient.StopChaseMusic);
                    %victimclient.StopChaseMusic = %victimclient.schedule(6000,StopChaseMusic);
                }

                if(isObject(%killerclient))
                {
                    if(%killerclient.musicChaseLevel != 2)
                    {
                        %killerclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl2Music);
                        %killerclient.musicChaseLevel = 2;
                    }

                    cancel(%killerclient.StopChaseMusic);
                    %killerclient.StopChaseMusic = %killerclient.schedule(6000,StopChaseMusic);
                }
            }
            else
            {
                if(isObject(%victimclient))
                {
                    if(%victimclient.musicChaseLevel != 1 && %killercansee[%cansee] == %scan)
                    {
                        %victimclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl1Music);
                        %victimclient.musicChaseLevel = 1;
                    }

                    %victimclient.player.TimeSinceChased = getSimTime();
                    cancel(%victimclient.StopChaseMusic);
                    %victimclient.StopChaseMusic = %victimclient.schedule(6000,StopChaseMusic);
                }

                if(isObject(%killerclient))
                {
                    if(%killerclient.musicChaseLevel != 1)
                    {
                        %killerclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl1Music);
                        %killerclient.musicChaseLevel = 1;
                    }

                    cancel(%killerclient.StopChaseMusic);
                    %killerclient.StopChaseMusic = %killerclient.schedule(6000,StopChaseMusic);
                }            
            }
        }
        else
        {
            if(isObject(%victimclient) && %killercansee[%cansee] != %scan && %victimclient.player.TimeSinceChased+6000 < getSimTime())
            {
                if(%victimclient.musicChaseLevel != 1)
                {
                    %victimclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl1Music);
                    %victimclient.musicChaseLevel = 1;
                }

                cancel(%victimclient.StopChaseMusic);
                %victimclient.StopChaseMusic = %victimclient.schedule(6000,StopChaseMusic);
            }

            if(isObject(%killerclient) && !%chasing)
            {
                if(%killerclient.musicChaseLevel != 1)
                {
                    %killerclient.SetChaseMusic(%obj.getdataBlock().killerChaseLvl1Music);
                    %killerclient.musicChaseLevel = 1;
                }

                cancel(%killerclient.StopChaseMusic);
                %killerclient.StopChaseMusic = %killerclient.schedule(6000,StopChaseMusic);
            }
        }
    }

	if(%obj.lastKillerIdle+getRandom(4500,6000) < getSimTime())//Idle sounds are now in this function instead of a separate function
	{
		%obj.lastKillerIdle = getSimTime();

		if(%killercansee[%cansee])//Requires the container loop to run in order for this to work, if this part is true then there are players the killer is after
		{
			if(!%obj.isInvisible && %this.killerchasesound !$= "")
			{
				%obj.playthread(3,"plant");
				%obj.playaudio(0,%this.killerchasesound @ getRandom(1,%this.killerchasesoundamount) @ "_sound");
			}

			if(!%obj.raisearms && %this.killerraisearms)
			{
				%obj.playthread(1,"armReadyBoth");
				%obj.raisearms = true;
			}
		}
		else//Return to idle behaviors
		{
			if(!%obj.isInvisible && %this.killeridlesound !$= "")
			{
				%obj.playthread(3,"plant");
				%obj.playaudio(0,%this.killeridlesound @ getRandom(1,%this.killeridlesoundamount) @ "_sound");	
			}

			if(%obj.raisearms)
			{
				%obj.playthread(1,"root");
				%obj.raisearms = false;
			}
		}		
	}

    cancel(%obj.onKillerLoop);//Prevent duplicate processes
    %obj.onKillerLoop = %obj.schedule(1500,onKillerLoop);
}

function Player::KillerGhostLightCheck(%obj)
{	
	if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getdataBlock().isKiller || !isObject(getMinigamefromObject(%obj))) return;
	
	if(!%obj.isInvisible)
	{
		if(!isObject(%obj.lightbot))
		{
			%obj.lightbot = new Player() 
			{ 
				dataBlock = "EmptyLightBot";
				source = %obj;			
			};
			%obj.mountObject(%obj.lightbot,5);
			MissionCleanup.add(%obj.lightbot);

			%obj.lightbot.light = new fxLight ("")
			{
				dataBlock = %obj.getdataBlock().killerlight;
				source = %obj.lightbot;
			};
			MissionCleanup.add(%obj.lightbot.light);
			%obj.lightbot.light.setTransform(%obj.lightbot.getTransform());
			%obj.lightbot.light.attachToObject(%obj.lightbot);
		}

		%obj.lightbot.light.setNetFlag(6,true);
		for(%i = 0; %i < clientgroup.getCount(); %i++)
		if(isObject(%client = clientgroup.getObject(%i)) && %client.player != %obj) 
		%obj.lightbot.light.schedule(10,clearScopeToClient,%client);
	}
	else
	{
		if(isObject(%obj.lightbot.light)) %obj.lightbot.light.delete();
		if(isObject(%obj.lightbot)) %obj.lightbot.delete();	
	}
}

function GameConnection::SetChaseMusic(%client,%songname)
{
    if(!isObject(%client) || !isObject(%songname)) return;
    
    if(isObject(%client.EventidemusicEmitter))
    %client.EventidemusicEmitter.delete();

    %client.EventidemusicEmitter = new AudioEmitter()
    {
        position = "9e9 9e9 9e9";
        profile = %songname;
        volume = 1;
        type = 10;
        useProfileDescription = false;
        is3D = false;
    };
    MissionCleanup.add(%client.EventidemusicEmitter);
    %client.EventidemusicEmitter.scopeToClient(%client);
}

function GameConnection::StopChaseMusic(%client)
{
    if(!isObject(%client)) return;
    if(isObject(%client.EventidemusicEmitter)) %client.EventidemusicEmitter.delete();

    %client.musicChaseLevel = 0;
}

function GameConnection::Escape(%client)
{
	if(!isObject(%minigame = getMinigameFromObject(%client))) return %client.centerprint("This only works in minigames!",1);
	if(strlwr(%client.slyrTeam.name) !$= "survivors") return %client.centerprint("Only survivors can escape the map!",1);

	%client.escaped = true;
	%client.camera.setMode("Spectator",%client.player);
	%client.setcontrolobject(%client.camera);
	%client.player.delete();
	%minigame.chatmsgall("<font:Impact:30>\c3" @ %client.name SPC "\c3has escaped!");
	%client.lives = 0;

	for(%i = 0; %i < %client.slyrTeam.numMembers; %i++) if(isObject(%members = %client.slyrTeam.member[%i]) && %members.escaped) %escaped++;

	if(%escaped >= %client.slyrTeam.numMembers) 
	{
		%minigame.endRound(%client.slyrTeam);
		%minigame.chatmsgall("<font:Impact:30>\c3All survivors have escaped!");
		for(%i = 0; %i < %client.slyrTeam.numMembers; %i++) if(isObject(%members = %client.slyrTeam.member[%i])) %members.play2D("fallen_survivor_sound");
		return;
	}	
}