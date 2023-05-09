function serverCmdClearEventideShapes(%client)
{
	if(!%client.isAdmin || !isObject(EventideShapeGroup) || !EventideShapeGroup.getCount()) return;
	
	MessageAll('MsgClearBricks', "\c3" @ %client.getPlayerName () @ "\c1\c0 cleared all Eventide shapes.");
	EventideShapeGroup.delete();
}

function AddBrickToRitualSet(%obj)
{
	if(!isObject(EventideRitualSet))
	{
	    new SimSet(EventideRitualSet);
	    missionCleanup.add(EventideRitualSet);
	}

	if(isObject(EventideRitualSet) && !%obj.isMember(EventideRitualSet)) EventideRitualSet.add(%obj);
}

function serverCmdGetRitualCount(%client)
{
	if(!%client.isAdmin || !isObject(EventideShapeGroup) || !EventideRitualSet.getCount()) return;
	%client.chatmessage("\c2" @ EventideRitualSet.getCount() SPC "\c2ritual placements exist");
}

function fxDTSBrick::ShowEventideProp(%obj,%player,%isGem,%image)
{
	if(!%isGem) %datablock = %obj.getdataBlock().staticShape;
	else %datablock = %image.staticShape;
	
	%interactiveshape = new StaticShape()
	{
		datablock = %datablock;
		spawnbrick = %obj;
	};
	
	%obj.interactiveshape = %interactiveshape;
	if(%isGem) %interactiveshape.setNodeColor("ALL",%image.colorShiftColor);
	%interactiveshape.schedule(10,playAudio,3,%interactiveshape.getDataBlock().placementSound);
	%interactiveshape.settransform(vectoradd(%obj.gettransform(),%obj.getdatablock().shapeBrickPos) SPC getwords(%obj.gettransform(),3,6));

	if(!isObject(EventideShapeGroup))
	{
		new ScriptGroup(EventideShapeGroup);
		missionCleanup.add(EventideShapeGroup);
	}

	EventideShapeGroup.add(%interactiveshape);//Important to keep please
	if(EventideShapeGroup.getCount() >= EventideRitualSet.getCount()) 
	{				
		if(isObject(%minigame = getMiniGameFromObject(%player)))
		{
			%minigame.centerprintall("<font:Impact:40>\c3All" SPC EventideRitualSet.getCount() SPC "\c3rituals have been placed!",3);

			for(%i = 0; %i < %minigame.numMembers; %i++) 
			if(isObject(%member = %minigame.member[%i])) %member.play2D("round_start_sound");
		}

		if(isObject($EventideEventCaller))//lets call that console brick
		{
			$InputTarget_["Self"] = $EventideEventCaller;
			$InputTarget_["Player"] = %player;
			$InputTarget_["Client"] = %player.client;			
			$InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
			$EventideEventCaller.processInputEvent("onAllRitualsPlaced",%client);
		}
	}
}