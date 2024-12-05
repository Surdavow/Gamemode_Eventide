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
	stateTransitionOnTriggerDown[1] = "Change";
	
	stateName[2] = "Change";
	stateTransitionOnTimeout[2] = "Check";
	stateScript[2] = "onRadioChange";
	stateTimeoutvalue[2] = 0.3;
	
	stateName[3] = "Check";
	stateSequence[3] = "Check";
	stateTransitionOnTriggerUp[3] = "Ready";
	stateSequence[3] = "Ready";
};

function RadioImage::onMount(%this,%obj,%slot)
{	
	%obj.client.centerPrint("<font:Impact:30>\c3Keep the radio in your inventory to broadcast to other survivors!",3);
	%obj.playaudio(3,"radio_change_sound");
}