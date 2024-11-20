datablock PlayerData(PlayerNightmareTeleport : PlayerNightmare) 
{
	maxForwardSpeed = 8.9;
	maxBackwardSpeed = 5.1;
	maxSideSpeed = 7.7;
	
	uiName = "NightmareTeleport Player";
	
	killerChaseLvl1Music = "";
	killerChaseLvl2Music = "";
};

function PlayerNightmareTeleport::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);
	%obj.isInvisible = true;	
	%obj.setScale("1 1 1");
	%obj.unmountImage(0);
}

function PlayerNightmareTeleport::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");
}