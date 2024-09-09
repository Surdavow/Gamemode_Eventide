registerOutputEvent("GameConnection", "Escape", "", false);

function Player::SetSpeedModifier(%obj,%a)
{
	if(%obj.Speed_Modifier $= "") %obj.Speed_Modifier = 1;	

	if(%a <= 0) return;	

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
	if(%obj.Speed_Modifier $= "") %obj.Speed_Modifier = 1;	

	%obj.SetSpeedModifier(%obj.Speed_Modifier + %a);
}

function Player::SetTempSpeed(%obj,%speedMultiplier)
{						
	if(!isObject(%obj) || %obj.getstate() $= "Dead") return;

	%this = %obj.getDataBlock();

	%obj.setMaxForwardSpeed(%datablock.MaxForwardSpeed*%speedMultiplier);
	%obj.setMaxSideSpeed(%datablock.MaxSideSpeed*%speedMultiplier);
	%obj.setMaxBackwardSpeed(%datablock.maxBackwardSpeed*%speedMultiplier);

	%obj.setMaxCrouchForwardSpeed(%datablock.maxForwardCrouchSpeed*%speedMultiplier);
  	%obj.setMaxCrouchBackwardSpeed(%datablock.maxSideCrouchSpeed*%speedMultiplier);
  	%obj.setMaxCrouchSideSpeed(%datablock.maxSideCrouchSpeed*%speedMultiplier);

 	%obj.setMaxUnderwaterBackwardSpeed(%datablock.MaxUnderwaterBackwardSpeed*%speedMultiplier);
  	%obj.setMaxUnderwaterForwardSpeed(%datablock.MaxUnderwaterForwardSpeed*%speedMultiplier);
  	%obj.setMaxUnderwaterSideSpeed(%datablock.MaxUnderwaterForwardSpeed*%speedMultiplier);	
}

registerInputEvent("fxDtsBrick", "onGaze", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
function Armor::GazeLoop(%this,%obj)
{		
	// Some conditions to return early if one is met
	if (!$Pref::Server::GazeEnabled || !isObject(%obj) || %obj.getState() $= "Dead" || !isObject(%minigame = getMinigamefromObject(%obj))) return;

	%eye = %obj.getEyePoint();
	%end = vectorAdd(%obj.getEyePoint(),vectorScale(%obj.getEyeVector(),$Pref::Server::GazeRange));
	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType;
	%hit = containerRayCast (%eye, %end, %mask, %obj);
	%obj.gazingPlayer = false;
	%obj.gazing = 0;
	
	if (isObject(%hit))
	{
		if(%hit.getType() & $TypeMasks::FxBrickObjectType)
		{
			$InputTarget_Self = %hit;
			$InputTarget_Player = %obj;
			$InputTarget_Client = (isObject(%client = %obj.client) ? %client : 0);
			$InputTarget_Minigame = %minigame;
			%hit.processInputEvent("onGaze", %gazer);
		}

		if(%hit.getType() & $TypeMasks::PlayerObjectType) %obj.gazingPlayer = true;
		%obj.gazing = %hit;
	}	

	// Cancel and reschedule the loop to avoid any overlapping schedules
	cancel(%obj.GazeLoop);
	%obj.GazeLoop = %this.schedule(33,GazeLoop,%obj);	
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

function GameConnection::Escape(%client)
{
	if(!isObject(%minigame = getMinigameFromObject(%client))) return %client.centerprint("This only works in minigames!",1);
	if(strlwr(%client.slyrTeam.name) !$= "survivors") return %client.centerprint("Only survivors can escape the map!",1);
	
	%client.escaped = true;

	// Iterate through each member of the survivor's team
	for (%i = 0; %i < %client.slyrTeam.numMembers; %i++) 
	{
    	if (!isObject(%member = %client.slyrTeam.member[%i])) continue;

    	%living += isObject(%member.player) ? 1 : 0;
    	%escaped += %member.escaped ? 1 : 0;
	}

	//Announce the escape and award points
	%minigame.chatmsgall("<font:Impact:30>\c3" @ %client.name SPC "\c3has escaped!");
	%client.incscore(10);
	
	// Kill the player
	%client.player.delete();
	%client.lives = 0;
	%client.setdead(1);

	// Force the player into spectator mode
	%client.camera.setmode("Spectator",%client);
	%client.setcontrolobject(%client.camera);	

	if(%escaped >= %living) return %minigame.endRound(%client.slyrTeam);
}
