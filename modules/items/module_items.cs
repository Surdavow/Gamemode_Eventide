exec("./item_candle.cs");
exec("./item_flaregun.cs");
exec("./item_book.cs");
exec("./item_gem.cs");
exec("./item_radio.cs");
exec("./item_soda.cs");
exec("./item_rope.cs");
exec("./weapon_dagger.cs");
exec("./weapon_killers.cs");

registerInputEvent("fxDTSBrick","onRitualPlaced","Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick","onAllRitualsPlaced","Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

datablock fxDTSBrickData (brickEventideEventCaller : brick2x2Data)
{
	uiName = "Eventide Console";
	Category = "Special";
	Subcategory = "Eventide";
};

function brickEventideEventCaller::onPlant(%this, %obj)
{	
	$EventideEventCaller = %obj;
	Parent::onPlant(%this,%obj);
}

function brickEventideEventCaller::onloadPlant(%this, %obj) 
{ 
	brickEventideEventCaller::onPlant(%this, %obj); 
}

function brickEventideEventCaller::onRemove(%this, %obj)
{	
	$EventideEventCaller = "";
	Parent::onRemove(%this,%obj);
}