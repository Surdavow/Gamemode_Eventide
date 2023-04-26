registerInputEvent("fxDtsBrick", "onGaze", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");

function Player::gazeLoop(%obj)
{
	//This should only work for players
	if(!isObject(%obj) || !isObject(%obj.client) || %obj.getState() $= "Dead") return;

	cancel(%obj.gazeLoop);//Kill off duplicate processes	
	%obj.gazeLoop = %obj.schedule(10, "gazeLoop");

	if(!$Pref::Server::GazeEnabled) return;
	
	%hit = firstWord(containerRaycast(%obj.getEyePoint(),vectorAdd(%obj.getEyePoint(), vectorScale(%obj.getEyeVector(), $Pref::Server::GazeRange)), $TypeMasks::FxBrickObjectType, %obj));

	if(isObject(%hit))
	{
		$InputTarget_Self = %hit;
		$InputTarget_Player = %obj;
		$InputTarget_Client = %obj.client;
		if($Server::LAN || getMinigameFromObject(%hit) == getMinigameFromObject(%obj)) $InputTarget_Minigame = getMinigameFromObject(%obj);
		else $InputTarget_Minigame = 0;
		%hit.processInputEvent("onGaze", %gazer);
	}	
}


package Eventide_Gaze
{
	function Armor::onNewDatablock(%this,%obj)
	{
		Parent::onNewDatablock(%this,%obj);
		%obj.gazeLoop();
	}
};

if(isPackage(Eventide_Gaze)) deactivatePackage(Eventide_Gaze);
activatePackage(Eventide_Gaze);