//centerprint menu system
//modular system written by conan
//version 2.0
//- added justify, fontA/B support, playShiftAudio, playSelectAudio
//- added basic documentation


//to use:
//reference the example centerprint menu ScriptObjects below, as well as the fields
//menus are ScriptObjects with fields
//menu shows up to 5 options at a time, and scrolls the menu as the player shifts their selection
//loops back when going past the end of the menu - going up from the first selection puts you at the last one, and vice versa
//
//to start, call %client.startCenterprintMenu(%menuObject);
//to exit, call %client.exitCenterprintMenu();
//
//centerprint menu is automatically exited when an option is selected
//menu is controlled with brick controls and is automatically exited on death and respawn


//menu fields:
//
//isCenterprintMenu - must be true for it to be displayed
//menuName - the displayed name of the menu
//menuOption[#] - the displayed name of the menu option
//menuFunction[0] - the function called when that selection is confirmed
//					receives the client, menu, and option # as parameters, in that order
//					can be left empty if no callback is needed
//
//justify - <just:left> or <just:right> the menu. by default it is <just:center>
//fontA - font of not-current selection
//fontB - font of current selection
//playShiftAudio - plays MsgUploadEnd tone when changing options
//playSelectAudio - plays MsgAdminForce tone when selecting an option
//
//deleteOnExit - deletes ScriptObject on menu exit. scheduled


//suggested functions to package, if extending or modifying functionality:
//GameConnection::displayCenterprintMenu(%client, %diff)
//	- called every time the menu is refreshed and displayed to client (selection changes)
//	- diff is # of options moved up or down 
//playCenterprintMenuSound(%client, %string)
//	- called when the menu wants to play a sound 
//	- defaults: 'MsgAdminForce' for confirming selection, 'MsgUploadEnd' for moving selection
//	- only called if playSelectAudio, playShiftAudio are enabled (respectively)


$CenterprintMenuSystemVersion += 0; //make sure its a valid number
if ($CenterprintMenuSystemVersion < 2.1 && isFunction(GameConnection, startCenterprintMenu))
{
	echo("Support_CenterprintMenuSystem: Older centerprint menu system detected: v" @ $CenterprintMenuSystemVersion);
	echo("Support_CenterprintMenuSystem: Overwriting with version 2.1");
}
else if ($CenterprintMenuSystemVersion >= 2.1)
{
	echo("Support_CenterprintMenuSystem: Centerprint menu system detected: v" @ $CenterprintMenuSystemVersion);
	echo("Support_CenterprintMenuSystem: Skipping... (v2.1)");
}
$CenterprintMenuSystemVersion = 2.1;

//core//
//put in package to override previous versions which did not have version checking prior to overwrite 

package Support_CenterprintMenuSystemPackage 
{
	function GameConnection::startCenterprintMenu(%cl, %menuObj)
	{
		if (!isObject(%menuObj))
		{
			error("ERROR: startCenterprintMenu cannot find menu \"" @ %menuObj @ "\" to display!");
			return;
		}
		else if (!%menuObj.isCenterprintMenu)
		{
			error("ERROR: %menuObj not a centerprint menu!");
			return;
		}

		if (%cl.lastMessagedBrickControls + 20 < $Sim::Time)
		{
			messageClient(%cl, '', "\c5Use brick controls/numpad to navigate the centerprint menu.");
		}
		%cl.lastMessagedBrickControls = $Sim::Time;

		%cl.isInCenterprintMenu = 1;
		%cl.centerprintMenu = %menuObj.getID();
		%cl.currOption = 0;

		%cl.displayCenterprintMenu();
	}

	function GameConnection::exitCenterprintMenu(%cl)
	{
		%cl.centerprint("", 1);
		%cl.isInCenterprintMenu = 0;
		%menu = %cl.centerprintMenu;
		%cl.centerprintMenu = "";

		if (%menu.deleteOnExit)
		{
			%menu.schedule(1, delete);
		}
	}

	function GameConnection::displayCenterprintMenu(%cl, %diff)
	{
		if (!isObject(%cl.centerprintMenu) || !%cl.isInCenterprintMenu)
		{
			%cl.centerprint("", 1);
			return;
		}

		%menu = %cl.centerprintMenu;

		%menuName = %menu.menuName;
		%optionIDX = %cl.currOption = %cl.currOption + 0; //make sure its a number and not empty string
		if (%diff < 0)
		{
			%diff = %menu.menuOptionCount + %diff;
		}
		%optionIDX = %cl.currOption = (%optionIDX + %diff) % %menu.menuOptionCount;

		if (%diff != 0 && %menu.playShiftAudio)
		{
			playCenterprintMenuSound(%cl, 'MsgUploadEnd');
		}

		//display up to 5 options
		%min = %optionIDX;
		%max = %optionIDX;
		%dist = %max - %min;
		while (%dist < 4)
		{
			if (%menu.menuOptionCount <= 0)
			{
				break;
			}
			
			if (%min > 0)
				%min--;
			if (%max < %menu.menuOptionCount - 1) {
                
				%max++;
            }
            
			if (%min == 0 && %max == %menu.menuOptionCount - 1) //menu is less than 5 options
				%dist = 50;
			else
				%dist = %max - %min;
		}

		%fontA = %menu.fontA $= "" ? "<font:Arial:18>\c6" : %menu.fontA;
		%fontB = %menu.fontB $= "" ? "<font:Arial Bold:18>\c3" : %menu.fontB;

		%line = %menu.justify @ "\c3" @ %menuName @ " <br>" @ %fontA;
		for (%i = 0; %i <= getMin(%dist, 6); %i++)
		{
			%option = %i + %min;
			if (%option == %optionIDX)
			{
				%line = %line @ %fontB @ %menu.menuOption[%option] @ " \n" @ %fontA;
			}
			else
			{
				%line = %line @ %menu.menuOption[%option] @ " \n\c6";
			}
		}

		%cl.centerprint(%line);
	}

	function GameConnection::confirmCenterprintMenu(%cl)
	{
		if (!%cl.isInCenterprintMenu)
		{
			return;
		}

		%menu = %cl.centerprintMenu;
		%option = %cl.currOption;

		%cl.exitCenterprintMenu();
		%func = %menu.menuFunction[%option];
		if (%menu.playSelectAudio)
		{
			playCenterprintMenuSound(%cl, 'MsgAdminForce');
		}

		if (%func !$= "" && !isFunction(%func))
		{
			error("ERROR: confirmCenterprintMenu: cannot find function " @ %func @ "!");
			return;
		}
		else
		{
			call(%func, %cl, %menu, %option);
		}
	}

	function playCenterprintMenuSound(%cl, %sound)
	{
		%ret = parent::playCenterprintMenuSound(%cl, %sound);

		if (!%ret)
		{
			messageClient(%cl, %sound);
		}
	}

	function reopenCenterprintMenu(%cl, %menu, %option)
	{
		%cl.startCenterprintMenu(%menu);
		%cl.displayCenterprintMenu(%option);
	}
};
activatePackage(Support_CenterprintMenuSystemPackage);

//package//

//leave menus on death/spawn
package centerprintMenuSystem
{
	function GameConnection::onDeath(%cl, %a, %b, %c, %d, %e, %f)
	{
		if (%cl.isInCenterprintMenu)
		{
			%cl.exitCenterprintMenu();
		}

		return parent::onDeath(%cl, %a, %b, %c, %d, %e, %f);
	}

	function GameConnection::spawnPlayer(%cl)
	{
		if (%cl.isInCenterprintMenu)
		{
			%cl.exitCenterprintMenu();
		}

		return parent::spawnPlayer(%cl);
	}

	function serverCmdShiftBrick(%cl, %x, %y, %z)
	{
		if (%cl.isInCenterprintMenu && %x != 0)
		{
			if (%cl.lastCenterprintShift + 0.1 > $Sim::Time && mAbs(%cl.repeatCenterprintShift) > 30)
			{
				%x *= 5;
			}
			else if (%cl.lastCenterprintShift + 0.1 > $Sim::Time)
			{
				%cl.repeatCenterprintShift += -1 * %x;
			}
			else
			{
				%cl.repeatCenterprintShift = 0;
			}
			%cl.lastCenterprintShift = $Sim::Time;
			%cl.displayCenterprintMenu(-1 * %x);
			return;
		}

		return parent::serverCmdShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdSuperShiftBrick(%cl, %x, %y, %z)
	{
		if (%cl.isInCenterprintMenu && %x != 0)
		{
			if (%cl.lastCenterprintShift + 0.1 > $Sim::Time && mAbs(%cl.repeatCenterprintShift) > 30)
			{
				%x *= 5;
			}
			else if (%cl.lastCenterprintShift + 0.1 > $Sim::Time)
			{
				%cl.repeatCenterprintShift += -1 * %x;
			}
			else
			{
				%cl.repeatCenterprintShift = 0;
			}
			%cl.lastCenterprintShift = $Sim::Time;
			%cl.displayCenterprintMenu(-1 * %x);
			return;
		}

		return parent::serverCmdSuperShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdPlantBrick(%cl)
	{
		if (%cl.isInCenterprintMenu)
		{
			%cl.confirmCenterprintMenu();
			return;
		}

		return parent::serverCmdPlantBrick(%cl);
	}

	function serverCmdCancelBrick(%cl)
	{
		if (%cl.isInCenterprintMenu)
		{
			%cl.exitCenterprintMenu();
			return;
		}

		return parent::serverCmdCancelBrick(%cl);
	}
};
activatePackage(centerprintMenuSystem);
