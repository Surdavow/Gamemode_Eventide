datablock ItemData(RadioItem)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "./models/radio.dts";
	uiName = "Radio";
	image = RadioImage;
	canDrop = true;
};

datablock ShapeBaseImageData(RadioImage)
{
	className = "WeaponImage";
	projectileType = "Projectile";
	projectile = "";
	item = "RadioItem";
	mountpoint = 0;
	shapefile = RadioItem.shapeFile;
	stateName[0] = "Activate";
	stateTimeoutValue[0] = "0";
	stateTransitionOnTimeout[0] = "Ready";	
	
	stateName[1] = "Ready";
	stateTimeoutValue[1] = 0;
};

function RadioImage::onMount(%this,%obj,%slot)
{	
	%obj.client.centerPrint("<font:Impact:30>\c3Keep the radio in your inventory to broadcast to other survivors!",3);
	%obj.playaudio(3,"radio_change_sound");
}