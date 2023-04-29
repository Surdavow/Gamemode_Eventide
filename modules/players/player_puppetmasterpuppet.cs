datablock PlayerData(PuppetMasterPuppet : EventidePlayer)
{
	uiName = "";
	maxTools = 0;
	maxWeapons = 0;
	thirdpersononly = true;
	isKiller = true;
};

function PuppetMasterPuppet::onNewDatablock(%this,%obj)
{
	%obj.setScale("0.5 0.5 0.5");
	%this.EventideAppearance(%obj,ClientGroup.getObject(getRandom(0,ClientGroup.getCount()-1)));
}

function PuppetMasterPuppet::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
			
	if(%press) switch(%trig)
	{
		case 0:	//left click
		case 4: //right click
	}
}

function PuppetMasterPuppet::EventideAppearance(%this,%obj,%client)
{
    Parent::EventideAppearance(%this,%obj,%client);
}