datablock ItemData(RadioItem)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "./models/radio.dts";
	iconName = "./icons/RadioIcon.png";
	uiName = "Radio";
	image = RadioImage;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
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
	stateSound[0] = "radio_change_sound";
	stateTimeoutValue[0] = "0";
	stateTransitionOnTimeout[0] = "Ready";	
	
	stateName[1] = "Ready";
	stateTimeoutValue[1] = 0;
};

function RadioImage::onMount(%this, %obj, %slot)
{	
	if(!%obj.radioInformed && isObject(%obj.client))
	{
		%obj.client.centerPrint("<font:Impact:25>\c3Keep the radio in your inventory to <br>\c3broadcast to other survivors!",3);
		%obj.radioInformed = true;
	}
}