datablock PlayerData(PuppetMasterPuppet : EventidePlayer)
{
	uiName = "";
	maxTools = 0;
	maxWeapons = 0;
	firstpersononly = true;
	isKiller = true;
	maxForwardSpeed = 13.06;
	maxBackwardSpeed = 7.46;
	maxSideSpeed = 11.20;
	maxDamage = 10;
};

function PuppetMasterPuppet::onNewDatablock(%this,%obj)
{
	%obj.setScale("0.6 0.6 0.6");
	%this.EventideAppearance(%obj,ClientGroup.getObject(getRandom(0,ClientGroup.getCount()-1)));
}

function PuppetMasterPuppet::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
			
	if(%press) switch(%trig)
	{
		case 0:	Eventide_Melee(%this,%obj,3.5);
		case 4: //right click
	}
}

function PuppetMasterPuppet::EventideAppearance(%this,%obj,%client)
{
    Parent::EventideAppearance(%this,%obj,%client);

	%obj.HideNode("headskin");
	%obj.unHideNode("headpuppet");

	%obj.HideNode((%client.chest ? "femChest" : "chest"));	
	%obj.HideNode((%client.rhand ? "rhook" : "rhand"));
	%obj.HideNode((%client.lhand ? "lhook" : "lhand"));

	%obj.unHideNode((%client.chest ? "femChestpuppet" : "chestpuppet"));	
	%obj.unHideNode((%client.rhand ? "rhook" : "rhandpuppet"));
	%obj.unHideNode((%client.lhand ? "lhook" : "lhandpuppet"));	

	if(!%client.hip)
	{
		%obj.HideNode("pants");
		%obj.unHideNode("pantspuppet");
	}

	%obj.setNodeColor("headpuppet",%client.headColor);	
	%obj.setNodeColor("chestpuppet",%client.chestColor);
	%obj.setNodeColor("femChestpuppet",%client.chestColor);
	%obj.setNodeColor("pantspuppet",%client.hipColor);
	%obj.setNodeColor("rhandpuppet",%client.rhandColor);
	%obj.setNodeColor("lhandpuppet",%client.lhandColor);
}