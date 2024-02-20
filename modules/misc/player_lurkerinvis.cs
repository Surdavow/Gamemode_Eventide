datablock PlayerData(PlayerLurkerInvis : PlayerRenowned) 
{
	hitprojectile = KillerSharpHitProjectile;
	hitobscureprojectile = KillerAxeClankProjectile;
	
	meleetrailskin = "base";

	killeridlesound = "";
	killeridlesoundamount = 0;

	killerchasesound = "";
	killerchasesoundamount = 0;

	killermeleesound = "";
	killermeleesoundamount = 0;	

	killermeleehitsound = "";
	killermeleehitsoundamount = 3;
	
	killerlight = "NoFlarePLight";

	rightclickicon = "color_handaxe";
	leftclickicon = "color_melee";

	rechargeRate = 0.3;
	maxTools = 1;
	maxWeapons = 1;
	maxForwardSpeed = 8.9;
	maxBackwardSpeed = 5.1;
	maxSideSpeed = 7.7;
	jumpForce = 0;
	
	uiName = "LurkerInvis Player";
	
	killerChaseLvl1Music = "";
	killerChaseLvl2Music = "";
};

function PlayerLurkerInvis::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);
	%obj.schedule(10,onKillerLoop);
	%obj.isInvisible = true;	
	%obj.setScale("0.9 0.9 1.2");
}

function PlayerLurkerInvis::EventideAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");	
}