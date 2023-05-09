registerInputEvent("fxDtsBrick", "onGaze", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");

function Player::gazeLoop(%obj)
{
	if(!isObject(%obj) || !isObject(%obj.client) || %obj.getState() $= "Dead" || %obj.getdataBlock().getName() !$= "EventidePlayer") return;

	cancel(%obj.gazeLoop);
	%obj.gazeLoop = %obj.schedule(33, "gazeLoop");

	if(!$Pref::Server::GazeEnabled) return;

	%hit = firstWord(containerRaycast(%obj.getEyePoint(),vectorAdd(%obj.getEyePoint(),vectorScale(%obj.getEyeVector(), $Pref::Server::GazeRange)),$TypeMasks::FxBrickObjectType,%obj));
	if(isObject(%hit))
	{
		$InputTarget_Self = %hit;
		$InputTarget_Player = %obj;
		$InputTarget_Client = %obj.client;
		if(isObject(%minigame = getMinigameFromObject(%obj)))
		$InputTarget_Minigame = %minigame;
		else $InputTarget_Minigame = 0;

		%hit.processInputEvent("onGaze", %gazer);
	}
}