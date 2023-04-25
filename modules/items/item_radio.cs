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

function CRadioImage::onRadioChange(%t,%o,%s)
{
	%o.RadioChannel = (%o.RadioChannel+1) % ($Pref::Server::ChatMod::radioNumChannels);
	if ($Pref::Server::ChatMod::radioNumChannels > 1)
	{
		if(isObject(%o.client)) %o.client.centerPrint("\c5Connected to channel\c3 "@ (%o.RadioChannel+1),4);	
		%o.stopAudio(3);
		%o.playaudio(3,"radio_change_sound");
	}
}
function CRadioImage::onMount(%t,%o,%s)
{
	Parent::onMount(%t,%o,%s);
	%o.RadioOn = true;

	if(isObject(%o.client)) %o.client.centerPrint("\c4Click to change channels. Chat in the radio using team chat.",4);
	%o.playaudio(3,"radio_mount_sound");
	%o.playThread(0,"armReadyRight");	
}
function CRadioImage::onUnMount(%t,%o,%s)
{
	Parent::onUnMount(%t,%o,%s);
	%o.RadioOn = false;
	%o.RadioChannel = mFloor(%o.RadioChannel);
	%o.playThread(0, "root");
	%o.playaudio(3,"radio_unmount_sound");
}