datablock ItemData(CRadioItem)
{
	category = "Tools";
	className = "Weapon";
	shapeFile = "./models/radio.dts";
	uiName = "Radio";
	image = CRadioImage;
	canDrop = true;
};

datablock ShapeBaseImageData(CRadioImage)
{
	className = "WeaponImage";
	projectileType = "Projectile";
	projectile = "";
	item = "CRadioItem";
	mountpoint = 0;
	shapefile = CRadioItem.shapeFile;
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

function CRadioImage::onMount(%t,%o,%s)
{
	Parent::onMount(%t,%o,%s);
	
	%o.RadioChannel = (%o.RadioChannel+1) % ($Pref::Server::ChatMod::radioNumChannels);

	for(%i = 0; %i <= %o.getDataBlock().maxTools; %i++)
	if(%o.tool[%i] $= %t.item.getID()) %itemslot = %i;
	
	if($Pref::Server::ChatMod::radioNumChannels > 1)
	{
		if(isObject(%o.client)) %o.client.centerPrint("\c4Radio equipped!",2);

		%o.radioEquipped = true;
		%o.stopAudio(3);
		%o.playaudio(3,"radio_change_sound");

		if(isObject(%o.client))
		{
			%o.tool[%itemslot] = 0;
			messageClient(%o.client,'MsgItemPickup','',%itemslot,0);
		}
		if(isObject(%o.getMountedImage(%t.mountPoint))) %o.unmountImage(%t.mountPoint);		
	}
}