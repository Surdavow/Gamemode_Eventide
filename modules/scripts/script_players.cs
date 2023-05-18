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
	%client.camera.setMode("Spectator",%client.player);
	%client.setcontrolobject(%client.camera);
	
	for(%i = 0; %i < %client.slyrTeam.numMembers; %i++)
	{
		if(isObject(%members = %client.slyrTeam.member[%i]) && isObject(%members.player) && %members.escaped) %escaped++;
		if(isObject(%members = %client.slyrTeam.member[%i]) && isObject(%members.player)) %living++;
	}
	

	%client.player.delete();
	%minigame.chatmsgall("<font:Impact:30>\c3" @ %client.name SPC "\c3has escaped!");
	%client.lives = 0;

	if(%escaped >= %living)
	{
		%minigame.endRound(%client.slyrTeam);
		return;
	}	
}