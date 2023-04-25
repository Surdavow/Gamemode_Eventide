exec("./datablocks_itemsimages.cs");
exec("./datablocks_brickshapes.cs");
exec("./datablocks_misc.cs");
exec("./item_book.cs");
exec("./item_candle.cs");
exec("./item_gem.cs");
exec("./item_radio.cs");
exec("./item_cola.cs");
exec("./item_dollar.cs");
exec("./item_rope.cs");
exec("./weapon_dagger.cs");

function serverCmdClearEventideShapes(%client)
{
	if(!%client.isAdmin || !isObject(EventideShapeGroup) || !EventideShapeGroup.getCount()) return;
	
	MessageAll('MsgClearBricks', "\c3" @ %client.getPlayerName () @ "\c1\c0 cleared all Eventide shapes.");
	EventideShapeGroup.delete();	
}

if(!isObject(EventideRitualSet))
{
    new SimSet(EventideRitualSet);
    missionCleanup.add(EventideRitualSet);
}

function AddBrickToRitualSet(%obj)
{
	if(isObject(EventideRitualSet) && !%obj.isMember(EventideRitualSet)) EventideRitualSet.add(%obj);
}

function serverCmdGetRitualCount(%client)
{
	if(!%client.isAdmin || !isObject(EventideShapeGroup) || !EventideRitualSet.getCount()) return;
	%client.chatmessage("\c2" @ EventideRitualSet.getCount() SPC "\c2ritual placements exist");
}

function fxDTSBrick::ShowEventideProp(%obj,%player)
{
	%interactiveshape = new StaticShape()
	{
		datablock = %obj.getdataBlock().staticShape;
		spawnbrick = %obj;
	};
	
	%obj.interactiveshape = %interactiveshape;
	%interactiveshape.settransform(vectoradd(%obj.gettransform(),%obj.getdatablock().shapeBrickPos) SPC getwords(%obj.gettransform(),3,6));
	%interactiveshape.schedule(5,playAudio,3,%interactiveshape.getDataBlock().placementSound);

	if(!isObject(EventideShapeGroup))
	{
		new ScriptGroup(EventideShapeGroup);
		missionCleanup.add(EventideShapeGroup);
	}

	EventideShapeGroup.add(%interactiveshape);
	if(EventideShapeGroup.getCount() >= EventideRitualSet.getCount()) 
	{
		MessageAll ('', "\c2All" SPC EventideRitualSet.getCount() SPC "rituals have been placed!");

		if(isObject(ClientGroup) && ClientGroup.GetCount())
		for(%i = 0; %i < ClientGroup.getCount(); %i++) if(isObject(%client = ClientGroup.getObject(%i))) %client.play2D("OUT_roundStart_sound");

		if(isObject($EventideEventCaller))
		{
			$InputTarget_["Self"] = $EventideEventCaller;
			$InputTarget_["Player"] = %player;
			$InputTarget_["Client"] = %player.client;			
			$InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
			$EventideEventCaller.processInputEvent("onAllRitualsPlaced",%client);
		}
	}
}

function fxDTSBrick::ShowEventidePropGem(%obj,%player,%image)
{
	%interactiveshape = new StaticShape()
	{
		datablock = %image.staticShape;
		spawnbrick = %obj;
	};
	
	%obj.interactiveshape = %interactiveshape;
	%interactiveshape.settransform(getwords(%obj.gettransform(),0,6));
	%interactiveshape.schedule(5,playAudio,3,%interactiveshape.getDataBlock().placementSound);
	%interactiveshape.setNodeColor("ALL",%image.colorShiftColor);

	if(!isObject(EventideShapeGroup))
	{
		new ScriptGroup(EventideShapeGroup);
		missionCleanup.add(EventideShapeGroup);
	}

	EventideShapeGroup.add(%interactiveshape);
	if(EventideShapeGroup.getCount() >= EventideRitualSet.getCount()) 
	{
		MessageAll ('', "\c2All" SPC EventideRitualSet.getCount() SPC "rituals have been placed!");

		if(isObject(ClientGroup) && ClientGroup.GetCount())
		for(%i = 0; %i < ClientGroup.getCount(); %i++) if(isObject(%client = ClientGroup.getObject(%i))) %client.play2D("OUT_roundStart_sound");

		if(isObject($EventideEventCaller))
		{
			$InputTarget_["Self"] = $EventideEventCaller;
			$InputTarget_["Player"] = %player;
			$InputTarget_["Client"] = %player.client;			
			$InputTarget_["MiniGame"] = getMiniGameFromObject(%player);
			$EventideEventCaller.processInputEvent("onAllRitualsPlaced",%client);
		}
	}
}