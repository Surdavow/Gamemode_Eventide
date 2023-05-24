datablock PlayerData(EmptyBot : PlayerStandardArmor)
{
	shapeFile = "base/data/shapes/empty.dts";
	uiName = "";
	className = PlayerData;
};

function EmptyBot::doDismount(%this,%obj,%forced)
{
	return;
}

function EmptyBot::onRemove(%this,%obj)
{
	if(isObject(%obj.light)) %obj.light.delete();
}