package Eventide_MainPackage
{
	function onMissionEnded(%this, %a, %b, %c, %d)
	{
		$PFGlassInit = false;
		$PFRTBInit = false;
		return Parent::onMissionEnded(%this, %a, %b, %c, %d);
	}

	function getBrickGroupFromObject(%obj)
	{
		if(isObject(%obj) && %obj.getClassName() $= "AIPlayer")		
		switch$(%obj.getDataBlock().getName())			
		{
			case "ShireZombieBot": return %obj.ghostclient.brickgroup;
			case "PuppetMasterPuppet": return %obj.client.brickgroup;
		}		

		Parent::getBrickGroupFromObject(%obj);
	}

	function ServerLoadSaveFile_End()
	{
		$Pref::Server::MapRotation::MapChange = false;

		for(%a = 0; %a < ClientGroup.getCount(); %a++)
		{
			%client = ClientGroup.getObject(%a);
			%client.spawnPlayer();
		}

		Parent::ServerLoadSaveFile_End();
	}	
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_MainPackage)) deactivatePackage(Eventide_MainPackage);
activatePackage(Eventide_MainPackage);