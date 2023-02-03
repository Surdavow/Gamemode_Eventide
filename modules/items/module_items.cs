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

function serverCmdClearEventideShapes (%client)
{
	if(!%client.isAdmin || !isObject(EventideShapeGroup) || !EventideShapeGroup.getCount()) return;
	
	MessageAll('MsgClearBricks', "\c3" @ %client.getPlayerName () @ "\c1\c0 cleared all Eventide shapes.");
	EventideShapeGroup.delete();	
}

function fxDTSBrick::ShowEventideProp(%obj)
{
	%interactiveshape = new StaticShape()
	{
		datablock = %obj.getdataBlock().staticShape;
		spawnbrick = %obj;
	};
	
	%obj.interactiveshape = %interactiveshape;
	%interactiveshape.settransform(vectoradd(%obj.gettransform(),%obj.getdatablock().shapeBrickPos) SPC getwords(%obj.gettransform(),3,6));

	if(!isObject(EventideShapeGroup)) new ScriptGroup(EventideShapeGroup);
	EventideShapeGroup.add(%interactiveshape);
}