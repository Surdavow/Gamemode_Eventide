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