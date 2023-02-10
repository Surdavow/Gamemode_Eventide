// ._________________________________________________.
// |     ___   _   ____. .____   .   .   _   .__     |
// |    /     / \     /  |       |\ /|  / \  |  \    |
// |   {  _  {---}   /   |__     | v | {   } |   }   |
// |   {   \ |   |  /    |       |   | {   } |   }   |
// |    \__/ |   | {____ |____   |   |  \_/  |__/    |
// |_________________________________________________|
// |                                                 |
// |  Build 2.3.1  Scripted by Xalos (BL_ID: 11239)  |
// |_________________________________________________|

registerInputEvent("fxDtsBrick", "onGaze", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
registerInputEvent("fxDtsBrick", "onGazeStart", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
registerInputEvent("fxDtsBrick", "onGazeStop", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
registerInputEvent("fxDtsBrick", "onGaze_Bot", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame\tBot Player");
registerInputEvent("fxDtsBrick", "onGazeStart_Bot", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame\tBot Player");
registerInputEvent("fxDtsBrick", "onGazeStop_Bot", "Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame\tBot Player");
registerOutputEvent("fxDtsBrick", "setGazeName", "string 200 255", 0);

function fxDtsBrick::setGazeName(%brick, %name)
{
	%brick.gazeName = %name;
}

function GameConnection::startGazing(%client)
{
	%client.cantGaze = 0;
}

function GameConnection::stopGazing(%client)
{
	cancel(%client.startGazing);
	%client.cantGaze = 1;
}

function Player::gazeLoop(%obj)
{
	cancel(%obj.gazeLoop);   //Kill off duplicate processes if any are trailing behind us.
	if(!isObject(%obj) || !isObject(%client = %obj.client)) return;
	if(%obj.getState() $= "Dead")
	{
		if(isObject(%obj.client.lastGazed))
		{
			$InputTarget_Player = %obj;
			$InputTarget_Client = %obj.client;
			if($Server::LAN || getMinigameFromObject(%last = %obj.client.lastGazed) == getMinigameFromObject(%obj.client))
				$InputTarget_Minigame = getMinigameFromObject(%obj.client);
			else
				$InputTarget_Minigame = 0;
			if(%last.getClassName() $= "fxDtsBrick")
			{
				$InputTarget_Self = %last;
				%last.processInputEvent("onGazeStop", %client);
			}
			else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
			{
				$InputTarget_Self = %last.spawnBrick;
				$InputTarget_Bot = %last;
				%last.spawnBrick.processInputEvent("onGazeStop_Bot");
			}
			%obj.client.lastGazed = "";
		}
	}
	%obj.gazeLoop = %obj.schedule(10, "gazeLoop");
	%eye = vectorScale(%obj.getEyeVector(), $Pref::Server::GazeRange);
	%pos = %obj.getEyePoint();
	%mask = $TypeMasks::TerrainObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::FxBrickObjectType;
	%hit = firstWord(containerRaycast(%pos, vectorAdd(%pos, %eye), %mask, %obj));
	%client = %obj.client;
	%last = %client.lastGazed;
	if(%client.cantGaze && isObject(%last))
	{
		//These events are really fucking abusable - admin wand immunity.
		//Disabled the wand immunity because it breaks events.
		//if(%obj.hasWandImmunity(%last))
		//	return;
		if(%last.getClassName() $= "fxDtsBrick")
		{
			$InputTarget_Player = %obj;
			$InputTarget_Client = %client;
			if($Server::LAN || getMinigameFromObject(%last) == getMinigameFromObject(%client))
				$InputTarget_Minigame = getMinigameFromObject(%client);
			else
				$InputTarget_Minigame = 0;
			if(%last.getClassName() $= "fxDtsBrick")
			{
				$InputTarget_Self = %last;
				%last.processInputEvent("onGazeStop", %client);
			}
			else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
			{
				$InputTarget_Self = %last.spawnBrick;
				$InputTarget_Bot = %last;
				%last.spawnBrick.processInputEvent("onGazeStop_Bot", %client);
			}
		}
		%client.lastGazed = "";
		commandToClient(%client, 'clearBottomPrint');
	}
	if(!%client.cantGaze)
	{
		if(!isObject(%hit))
		{
			if(isObject(%last))
			{
				//These events are really fucking abusable - admin wand immunity.
				//Disabled the wand immunity because it breaks events.
				//if(%obj.hasWandImmunity(%last))
				//	return;
				$InputTarget_Player = %obj;
				$InputTarget_Client = %client;
				if($Server::LAN || getMinigameFromObject(%last) == getMinigameFromObject(%client))
					$InputTarget_Minigame = getMinigameFromObject(%client);
				else
					$InputTarget_Minigame = 0;
				if(%last.getClassName() $= "fxDtsBrick")
				{
					$InputTarget_Self = %last;
					%last.processInputEvent("onGazeStop", %client);
				}
				else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
				{
					$InputTarget_Self = %last.spawnBrick;
					$InputTarget_Bot = %last;
					%last.spawnBrick.processInputEvent("onGazeStop_Bot", %client);
				}
				%client.lastGazed = "";
				commandToClient(%client, 'clearBottomPrint');
			}
			return;
		}
		if(%hit != %last)
		{
			if(%hit.getClassName() $= "fxDtsBrick")
			{
				if(isObject(%last))
				{
					$InputTarget_Player = %obj;
					$InputTarget_Client = %client;
					if($Server::LAN || getMinigameFromObject(%last) == getMinigameFromObject(%client))
						$InputTarget_Minigame = getMinigameFromObject(%client);
					else
						$InputTarget_Minigame = 0;
					if(%last.getClassName() $= "fxDtsBrick")
					{
						$InputTarget_Self = %last;
						%last.processInputEvent("onGazeStop", %client);
					}
					else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
					{
						$InputTarget_Self = %last.spawnBrick;
						$InputTarget_Bot = %last;
						%last.spawnBrick.processInputEvent("onGazeStop_Bot", %client);
					}
				}
				if($Pref::Server::GazeMode & 2)
				{
					//These events are really fucking abusable - admin wand immunity.
					if(%obj.hasWandImmunity(%hit))
						return;
					$InputTarget_Self = %hit;
					$InputTarget_Player = %obj;
					$InputTarget_Client = %client;
					if($Server::LAN || getMinigameFromObject(%hit) == getMinigameFromObject(%client))
						$InputTarget_Minigame = getMinigameFromObject(%client);
					else
						$InputTarget_Minigame = 0;
					%hit.processInputEvent("onGazeStart", %client);
				}
			}
			else if(%hit.getClassName() $= "AIPlayer" && isObject(%spawn = %hit.spawnBrick))
			{
				if(isObject(%last))
				{
					$InputTarget_Player = %obj;
					$InputTarget_Client = %client;
					if($Server::LAN || getMinigameFromObject(%last) == getMinigameFromObject(%client))
						$InputTarget_Minigame = getMinigameFromObject(%client);
					else
						$InputTarget_Minigame = 0;
					if(%last.getClassName() $= "fxDtsBrick")
					{
						$InputTarget_Self = %last;
						%last.processInputEvent("onGazeStop", %client);
					}
					else if(%last.getClassName() $= "AIPlayer" && isObject(%last.spawnBrick))
					{
						$InputTarget_Self = %last.spawnBrick;
						$InputTarget_Bot = %last;
						%last.spawnBrick.processInputEvent("onGazeStop_Bot", %client);
					}
				}
				if($Pref::Server::GazeMode & 1)
				{
					//These events are really fucking abusable - admin wand immunity.
					if(%obj.hasWandImmunity(%hit))
						return;
					$InputTarget_Self = %spawn;
					$InputTarget_Player = %obj;
					$InputTarget_Client = %client;
					$InputTarget_Bot = %hit;
					if($Server::Lan || getMinigameFromObject(%hit) == getMinigameFromObject(%client))
						$InputTarget_Minigame = getMinigameFromObject(%client);
					else
						$InputTarget_Minigame = 0;
					%spawn.processInputEvent("onGazeStart_Bot", %client);
				}
			}
			else
				commandToClient(%client, 'clearBottomPrint');
			%client.lastGazed = %hit;
		}
		%name = %hit.getGazeName(%client);
		if(%name !$= "")
			if((%hit.getClassName() $= "Player" && $Pref::Server::GazeMode & 1) || (%hit.getClassName() $= "fxDtsBrick" && $Pref::Server::GazeMode & 2))
			{
				%client.gazing = 1;
				%client.bottomPrint("\c6"@%name, 2);
				%client.gazing = 0;
			}
	}
}

// Scripters wanting to change the gazing system should package this
function SimObject::getGazeName(%this, %gazer)
{
	switch$(%this.getClassName())
	{
		case "Player": 	if(%this.client == %gazer || %gazer.player == %this || !($Pref::Server::GazeMode & 1)) return "";
						return %this.client.name;
		case "fxDtsBrick": if(!($Pref::Server::GazeMode & 2)) return "";

							//These events are really fucking abusable - admin wand immunity.
							if(isObject(%gazer.player) && !%gazer.player.hasWandImmunity(%this))
							{
								$InputTarget_Self = %this;
								$InputTarget_Player = %gazer.player;
								$InputTarget_Client = %gazer;
								if($Server::LAN || getMinigameFromObject(%this) == getMinigameFromObject(%gazer)) $InputTarget_Minigame = getMinigameFromObject(%gazer);
								else $InputTarget_Minigame = 0;
								%this.processInputEvent("onGaze", %gazer);
							}
							return %this.gazeName;

		case "AIPlayer": if(!($Pref::Server::GazeMode & 1)) return "";
						 //These events are really fucking abusable - admin wand immunity.
						if(isObject(%spawn = %this.spawnBrick) && isObject(%gazer.player) && !%gazer.player.hasWandImmunity(%spawn))
						{
							$InputTarget_Self = %spawn;
							$InputTarget_Player = %gazer.player;
							$InputTarget_Client = %gazer;
							$InputTarget_Bot = %this;
							if($Server::LAN || getMinigameFromObject(%spawn) == getMinigameFromObject(%gazer)) $InputTarget_Minigame = getMinigameFromObject(%gazer);
							else $InputTarget_Minigame = 0;
							%spawn.processInputEvent("onGaze_Bot", %gazer);
						}

		default: return "";
	}
}

function Player::hasWandImmunity(%pl, %brick)
{
	if(!isObject(%tool = %pl.getMountedImage(0))) return 0;
	if(%tool.getName() $= "AdminWandImage") return 1;
	if(%tool.getName() $= "WandImage" && isObject(%brick) && isObject(%pl.client) && %brick.getGroup().getID() == %pl.client.brickgroup.getID()) return 1;
	return 0;
}

package Eventide_Gaze
{
	function GameConnection::createPlayer(%clientient)
	{
		Parent::createPlayer(%clientient);
		if(isObject(%clientient.player)) %clientient.player.gazeLoop();
	}
	
	function GameConnection::bottomPrint(%client, %msg, %time)
	{
		if(!%client.gazing)
		{
			%client.stopGazing();
			if(%time <= 0) %client.startGazing = %client.schedule(60000, "startGazing");   //Cap time of one minute. That's long enough for any useful info.
			else %client.startGazing = %client.schedule(%time * 1000, "startGazing");
		}
		Parent::bottomPrint(%client, %msg, %time);
	}
};
activatePackage("Eventide_Gaze");