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

function CRadioImage::onMount(%this,%obj,%slot)
{	
	if(!isObject(%obj) || %obj.radioEquipped) return;

	%obj.RadioChannel = (%obj.RadioChannel+1) % ($Pref::Server::ChatMod::radioNumChannels);

	for(%i = 0; %i <= %obj.getDataBlock().maxTools; %i++)
	if(%obj.tool[%i] $= %this.item.getID()) %itemslot = %i;
	
	%obj.radioEquipped = true;
	%obj.stopAudio(3);
	%obj.playaudio(3,"radio_change_sound");
	%obj.client.centerPrint("\c4Radio is now enabled, use team chat to broadcast on the radio channel to other survivors.",4);

	if(isObject(%obj.client))
	{
		%obj.tool[%itemslot] = 0;
		messageClient(%obj.client,'MsgItemPickup','',%itemslot,0);
	}
	if(isObject(%obj.getMountedImage(%this.mountPoint))) %obj.unmountImage(%this.mountPoint);		
}