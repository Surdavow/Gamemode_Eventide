function Eventide_isInGame(%a, %b, %exempt)
{
    return    (%a.getType() & $TypeMasks::PlayerObjectType) &&    // is %a is a player?
            (%b.getType() & $TypeMasks::PlayerObjectType) &&    // is %b is a player?
            %a.isEnabled() &&                                    // is %a alive?
            (%exempt ? true : %b.isEnabled()) &&                // is %b alive?
            isObject(%miniA = getMinigameFromObject(%a)) &&        // does %a have a minigame?
            isObject(%miniB = getMinigameFromObject(%a)) &&     // does %b have a minigame?
            %miniA = %miniB &&                                    // are they in the same minigame?
            %a.getDataBlock().isKiller &&                        // is %a the killer?
            !%b.getDataBlock().isKiller;                        // is %b /not/ the killer?
}

package Eventide_MainPackage
{
	function ServerCmdDropTool(%client,%slot)
	{
		if(!isObject(getMinigameFromObject(%client))) return Parent::ServerCmdDropTool(%client, %slot);		

		if(isObject(EventideShapeGroup) && EventideShapeGroup.getCount() >= $EventideRitualAmount) return Parent::ServerCmdDropTool(%client, %slot);
	
		if(isObject(%player = %client.Player) && isObject(%item = %player.tool[%slot]))
		{
			if(%item.meleeHealth !$= "")
			{
				$MeleeToolHealthMod = %player.meleeHealth[%item.getName()];
				return parent::servercmdDropTool(%client,%slot);
			}

			if(isObject(%player.getMountedImage(%item.image.mountPoint)) && %player.getMountedImage(0).getName() $= "sm_foldingChairAttackImage") %player.unMountImage(0);			

			if(!%item.image.isRitual) return Parent::ServerCmdDropTool(%client, %slot);
			if(!%item.canDrop || !%player.getDatablock().isEventideModel) return Parent::ServerCmdDropTool(%client, %slot);			


			%zScale = getWord (%player.getScale (), 2);
			%muzzlepoint = VectorAdd (%player.getPosition (), "0 0" SPC 1.5 * %zScale);
			%muzzlevector = %player.getEyeVector ();
			%muzzlepoint = VectorAdd (%muzzlepoint, %muzzlevector);
			%playerRot = rotFromTransform (%player.getTransform ());
			%thrownItem = new Item ("")
			{
				dataBlock = %item;
			};
			%thrownItem.setScale (%player.getScale ());
			MissionCleanup.add (%thrownItem);
			%thrownItem.setTransform (%muzzlepoint @ " " @ %playerRot);
			%thrownItem.setVelocity (VectorScale (%muzzlevector, 20 * %zScale));
			%thrownItem.miniGame = %client.miniGame;
			%thrownItem.bl_id = %client.getBLID ();
			%thrownItem.setCollisionTimeout(%player);
			if(!isObject(DroppedItemGroup))
			{
				new SimGroup(DroppedItemGroup);
				missionCleanUp.add(DroppedItemGroup);
			}
			DroppedItemGroup.add(%thrownItem);				
			if(%item.className $= "Weapon") %player.weaponCount -= 1;
			
			%player.tool[%slot] = 0;
			messageClient (%client, 'MsgItemPickup', '', %slot, 0);			
			if(isObject(%player.getMountedImage(%item.image.mountPoint))) %player.unmountImage (%item.image.mountPoint);			
		}
	}

	function Armor::onImpact(%this, %obj, %col, %vec, %force)
	{
		Parent::onImpact(%this, %obj, %col, %vec, %force);

		if(%force < 40) serverPlay3D("impact_medium" @ getRandom(1,3) @ "_sound",%obj.getPosition());
		else serverPlay3D("impact_hard" @ getRandom(1,3) @ "_sound",%obj.getPosition());

		%oScale = getWord(%obj.getScale(),2);
		%forcescale = %force/25 * %oscale;
		%obj.spawnExplosion(pushBroomProjectile,%forcescale SPC %forcescale SPC %forcescale);

		if(%obj.getState() !$= "Dead" && getWord(%vec,2) > %obj.getdataBlock().minImpactSpeed)
        serverPlay3D("impact_fall_sound",%obj.getPosition());		
	}

	function onObjectCollisionTest(%obj, %col)//This function is part of the ObjectCollision.dll
	{
		if(isObject(%obj) && isObject(%col))
		{
			if((%obj.getType() & $TypeMasks::PlayerObjectType) && (%col.getType() & $TypeMasks::PlayerObjectType))
			if(%obj.getdataBlock().getName() $= "PlayerGrabberNoJump" || (%obj.getdataBlock().getName() $= "PlayerSkinwalker" && %col.getDamagePercent() < 0.5)) return true;
			else return false;
		}		
		
		return true;		
	}

	function ServerCmdPlantBrick (%client)
	{
		if(isObject(%client.player) && %client.player.getdataBlock().getName() $= "PlayerPuppetMaster" && isObject(PuppetGroup))
		{	
			if(%client.puppetnumber $= "") %client.puppetnumber = 0;

			if(isObject(PuppetGroup.getObject(%client.puppetnumber))) 
			{
				%client.getcontrolObject().schedule(1500,setActionThread,sit,1);
				%client.setcontrolobject(PuppetGroup.getObject(%client.puppetnumber));
				%client.puppetnumber++;
			}
			else
			{
				%client.getcontrolObject().schedule(1500,setActionThread,sit,1);
				%client.setcontrolobject(%client.player);
				%client.puppetnumber = 0;
			}			
		}

		Parent::ServerCmdPlantBrick(%client);
	}

	function player::setDamageFlash(%obj,%value)
	{
		if(!isObject(%obj.getControllingClient())) return;
		
		if(!%obj.ShireBlind && %value > 0.2) %value = 0.2;
		Parent::setDamageFlash(%obj,%value);
	}

	function Armor::onNewDatablock(%this,%obj)
	{
		Parent::onNewDatablock(%this,%obj);

		if(%this != %obj.getDatablock() && %this.maxTools != %obj.client.lastMaxTools)
		{
			%obj.client.lastMaxTools = %this.maxTools;
			commandToClient(%obj.client,'PlayGui_CreateToolHud',%this.maxTools);
		}
		
		if(isObject(%obj.client) && %this.maxTools != %obj.client.lastMaxTools)
		{
			%obj.client.lastMaxTools = %this.maxTools;
			commandToClient(%obj.client,'PlayGui_CreateToolHud',%this.maxTools);
			for(%i=0;%i<%this.maxTools;%i++)
			{
				if(isObject(%obj.tool[%i])) messageClient(%obj.client,'MsgItemPickup',"",%i,%obj.tool[%i].getID(),1);
				else messageClient(%obj.client,'MsgItemPickup',"",%i,0,1);
			}
		}		

		%obj.schedule(10,onKillerLoop);
	}

	function Armor::onDisabled(%this, %obj, %state)
	{
        Parent::onDisabled(%this, %obj, %state);

        if(isObject(%client = %obj.client) && isObject(%client.EventidemusicEmitter))
		{
			%client.EventidemusicEmitter.delete();        
        	%client.musicChaseLevel = 0;		
		}
		
		if(isObject(%killer = %obj.killer))
		{
			%killer.ChokeAmount = 0;
			%killer.victim = 0;
			%killer.playthread(3,"activate2");
			%obj.dismount();
			%obj.setVelocity(vectorscale(vectorAdd(%killer.getForwardVector(),"0 0 0.25"),15));		
		}
    }

	function Armor::onRemove(%this,%obj)
	{
		Parent::onRemove(%this,%obj);

		for(%i = 0; %i < %obj.getMountedObjectCount(); %i++) 
		if(isObject(%obj.getMountedObject(%i)) && (%obj.getMountedObject(%i).getDataBlock().className $= "PlayerData")) %obj.getMountedObject(%i).delete();
	}

	function fxDTSBrick::onActivate(%obj, %player, %client, %pos, %vec)
	{
		Parent::onActivate(%obj,%player,%client,%pos,%vec);

		if(isFunction(%obj.getDataBlock().getName(),onActivate)) %obj.getDataBlock().onActivate(%obj,%player,%client,%pos,%vec);
	}

	function fxDTSBrick::onRemove(%data, %brick)
	{
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
		Parent::OnRemove(%data,%brick);
	}

	function fxDTSBrick::onDeath(%data, %brick)
	{
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
	   	Parent::onDeath(%data, %brick);
	}	
	
	function serverCmdUseTool(%client, %tool)
	{		
		if(!%client.player.victim) return parent::serverCmdUseTool(%client, %tool);
	}

	function Slayer_MiniGameSO::endRound(%this, %winner, %resetTime)
	{
		Parent::endRound(%this, %winner, %resetTime);
		
		if(strlwr(%this.title) $= "eventide")
		{
			for(%i=0;%i<%this.numMembers;%i++)
			if(isObject(%client = %this.member[%i]) && %client.getClassName() $= "GameConnection") %client.play2d("round_end_sound");		 						
		}
	}

    function MiniGameSO::Reset(%minigame,%client)
	{
        Parent::Reset(%minigame,%client);

		if(strlwr(%minigame.title) $= "eventide")
		{
			for(%i=0;%i<%minigame.numMembers;%i++)
			if(isObject(%client = %minigame.member[%i]) && %client.getClassName() $= "GameConnection") 
			{
				%client.play2d("round_start_sound");		 			
				%client.escaped = false;
			}
			%minigame.centerprintall("<font:impact:40>\c3Eventide: The Hunt begins",2);
		}

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter))
        {
            %client.EventidemusicEmitter.delete();
            %client.musicChaseLevel = 0;
        }

		if(isObject(Shire_BotGroup)) Shire_BotGroup.delete();
		if(isObject(EventideShapeGroup)) EventideShapeGroup.delete();
		if(isObject(DroppedItemGroup)) DroppedItemGroup.delete();
    }

    function MinigameSO::endGame(%minigame,%client)
    {
        Parent::endGame(%minigame,%client);

        for(%i=0;%i<%minigame.numMembers;%i++)
        if(isObject(%client = %minigame.member[%i]) && isObject(%client.EventidemusicEmitter)) 
		{
			%client.EventidemusicEmitter.delete();
			%client.escaped = false;
		}
		if(isObject(Shire_BotGroup)) Shire_BotGroup.delete();    
    }

	function GameConnection::setControlObject(%this,%obj)
	{
		Parent::setControlObject(%this,%obj);
		if(%obj == %this.player && %obj.getDatablock().maxTools != %this.lastMaxTools)
		{
			%this.lastMaxTools = %obj.getDatablock().maxTools;
			commandToClient(%this,'PlayGui_CreateToolHud',%obj.getDatablock().maxTools);
		}
	}
	
	function gameConnection::applyBodyColors(%client) 
	{
		parent::applyBodyColors(%client);
		if(isObject(%player = %client.player) && %player.getDataBlock().isEventideModel) %player.getDataBlock().EventideAppearance(%player,%client);
	}
	function gameConnection::applyBodyParts(%client) 
	{
		parent::applyBodyParts(%client);
		if(isObject(%player = %client.player) && fileName(%player.getDataBlock().shapeFile) $= "Eventideplayer.dts") %player.getDataBlock().EventideAppearance(%player,%client);
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

	function Player::ActivateStuff (%player)//Not parenting, I made an overhaul of this function so it might cause compatibility issues...
	{
		Parent::ActivateStuff(%player);
		
		if(isObject(%player) && %player.getState() !$= "Dead" && isFunction(%player.getDataBlock().getName(),onActivate)) 
		%player.getDataBlock().onActivate(%player);
	}

	function ItemData::onAdd(%db,%item)
	{
		if($MeleeToolHealthMod !$= "")
		{
			%item.meleeHealth = $MeleeToolHealthMod;
			$MeleeToolHealthMod = "";
		}
		parent::onAdd(%db,%item);
	}
	function player::pickup(%pl,%item)
	{
		%db = %item.getDatablock();
		if(%item.meleeHealth !$= "")
			%pl.meleeHealth[%db.getName()] = %item.meleeHealth;
		else if(%item.getDatablock().meleeHealth !$= "")
			%pl.meleeHealth[%db.getName()] = %db.meleeHealth;
		parent::pickup(%pl,%item);
	}	
};

if(isPackage(Eventide_MainPackage)) deactivatePackage(Eventide_MainPackage);
activatePackage(Eventide_MainPackage);

if($Pref::Swol_SMMelee_Prefix $= "") $Pref::Swol_SMMelee_Prefix = 0;
if($Pref::Swol_SMMelee_DmgMod $= "") $Pref::Swol_SMMelee_DmgMod = 1;

function swolMelee_onFire(%db,%pl,%slot)
{
	%item = %db.item;
	if(%item.meleeHealth != 0)
		if(%pl.meleeHealth[%item] $= "")
			%pl.meleeHealth[%item] = %item.meleeHealth;
	%mask = ($TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticObjectType );
	%search = containerRayCast(%pl.getEyePoint(),vectorAdd(%pl.getEyePoint(),vectorScale(%pl.getMuzzleVector(%slot),%item.meleeRange)),%mask,%pl);
	%victim = getWord(%search,0);
	if(%item.meleeSound_SwingCnt != 0)
		serverPlay3d(%item.meleeSound_Swing[getRandom(0,%item.meleeSound_SwingCnt-1)],%pl.getPosition());
	%smash = 0;
	if(isObject(%victim))
	{
		%hit = getWords(%search,1,3);
		if(%victim.getClassName() $= "fxDTSBrick")
		{
			if((%bdb = %victim.getDatablock()).destroyBySM)
			{
				%dbType = %bdb.destroyBySMProj;
				%p = new Projectile()
				{
					datablock = %dbType;
					initialPosition = %hit;
					sourceObject = %pl;
					sourceSlot = %slot;
					client = %pl.client;
				};
				%p.explode();
				if(%bdb.destroyBySMSoundCnt != 0)
					serverPlay3d(%bdb.destroyBySMSound[getRandom(0,%bdb.destroyBySMSoundCnt-1)],%hit);
				%victim.disappear((%bdb.destroyBySMDisappearTime $= "" ? 10 : %bdb.destroyBySMDisappearTime));
			}
		}
		if(%item.meleeHealth != 0)
		{
			%pl.meleeHealth[%item]--;
			if(%pl.meleeHealth[%item] <= 0)
			{
				if(isObject(%item.meleeExplosion_Smash))
				{
					%p = new Projectile()
					{
						datablock = %item.meleeExplosion_Smash;
						initialPosition = %hit;
						sourceObject = %pl;
						sourceSlot = %slot;
						client = %pl.client;
					};
					%p.explode();
				}
				if(%item.meleeSound_SmashCnt != 0)
					serverPlay3d(%item.meleeSound_Smash[getRandom(0,%item.meleeSound_SmashCnt-1)],%hit);
				if(!%item.noMeleeRemove)
				{
					%pl.tool[%pl.currTool] = 0;
					if(isObject(%cl = %pl.client))
						messageClient(%cl,'MsgItemPickup','',%pl.currTool,0,1);
					%pl.unMountImage(%slot);
				}
				if(isFunction(%db.getName(),callback_smash))
					%db.callback_smash(%pl,%slot,%pl.currTool);
				%smash = 1;
			}
		}
		if(%pl.smashOverride)
			%smash = 1;
		if(%victim.getClassName() $= "Player" || %victim.getClassName() $= "AiPlayer")
		{
			if(isObject(%cl = %pl.client))
			{
				if(minigameCanDamage(%cl,%victim) == 1)
				{
					%victim.damage(%pl,%victim.getPosition(),(%smash ? %item.meleeDamageBreak : %item.meleeDamageHit)*$Pref::Swol_SMMelee_DmgMod,%item.meleeDamageType);
					if(%item.meleeVelocity !$= "")
						%victim.setVelocity(getWords(vectorScale(%pl.getMuzzleVector(%slot),%item.meleeVelocity),0,1) SPC 5);
				}
			}
			else
			{
				if(minigameCanDamage(%pl,%victim) == 1)
				{
					%victim.damage(%pl,%victim.getPosition(),(%smash ? %db.item.meleeDamageBreak : %db.item.meleeDamageHit)*$Pref::Swol_SMMelee_DmgMod,%db.item.meleeDamageType);
					if(%item.meleeVelocity !$= "")
						%victim.setVelocity(getWords(vectorScale(%pl.getMuzzleVector(%slot),%item.meleeVelocity),0,1) SPC 5);
				}
			}
			if(isFunction(%db.getName(),callback_hitPlayer))
					%db.callback_hitPlayer(%pl,%slot,%hit,%victim,%smash);
			if(%item.meleeSound_HitCnt != 0)
				serverPlay3d(%item.meleeSound_Hit[getRandom(0,%item.meleeSound_HitCnt-1)],%hit);
		}
		else
		{
			if(isFunction(%db.getName(),callback_hitSolid))
				%db.callback_hitSolid(%pl,%slot,%hit,%victim,%smash);
			if(%item.meleeSound_HitSolidCnt != 0)
				serverPlay3d(%item.meleeSound_HitSolid[getRandom(0,%item.meleeSound_HitSolidCnt-1)],%hit);
			else if(%item.meleeSound_HitCnt != 0)
				serverPlay3d(%item.meleeSound_Hit[getRandom(0,%item.meleeSound_HitCnt-1)],%hit);
		}
		if(isFunction(%db.getName(),callback_hit))
			%db.callback_hit(%pl,%slot,%hit,%victim,%smash);
		if(isObject(%item.meleeExplosion_Hit))
		{
			%p = new Projectile()
			{
				datablock = %item.meleeExplosion_Hit;
				initialPosition = %hit;
				sourceObject = %pl;
				sourceSlot = %slot;
				client = %pl.client;
			};
			%p.explode();
		}
	}
	else
	{
		if(isFunction(%db.getName(),callback_hitNothing))
			%db.callback_hitNothing(%pl,%slot);
	}
}
function player::sm_stun(%pl,%delay,%minor)
{
	cancel(%pl.unStunSched);
	if(%pl.getDamagePercent() >= 1.0)
		return;
	%pl.mountImage(sm_stunImage,3);
	%pl.prevDb = %pl.getDatablock();
	if(%minor)
		%pl.setMoveSpeedRatio(%pl.prevDb.maxForwardSpeed*0.7);
	else
	{
		%pl.setDatablock(sm_playerFrozen);
		if(!isObject(%pl.client))
		{
			%pl.playThread(1,sit);
			%pl.unSit = 1;
		}
		else
		{
			%pl.setActionThread("sit",1);
		}
	}
	if(!%minor)
		%pl.setVelocity("0 0 0");
	%pl.unStunSched = %pl.schedule(%delay,sm_unStun,%minor);
	%pl.setWhiteout(mClampF(%delay/1000,0,1));
	if(isObject(%cl = %pl.client) && !%minor)
	{
		%cl.setControlObject(%cam = %cl.camera);
		%cam.setMode("corpse",%pl);
		%cam.setTransform(%pl.getEyeTransform());
		%cam.tmpCont = new aiPlayer()
		{
			datablock = playerStandardArmor;
			position = "0 0 -5";
		};
		%cam.setControlObject(%cam.tmpCont);
	}
}
function player::sm_unStun(%pl,%minor)
{
	cancel(%pl.unStunSched);
	if(%pl.getMountedImage(3).getName() $= sm_stunImage)
		%pl.unMountImage(3);
	if(%pl.unSit)
		%pl.playThread(1,root);
	if(%pl.getDamagePercent() < 1.0)
	{
		if(%minor)
			%pl.setMoveSpeedRatio(%pl.prevDb.maxForwardSpeed);
		else
			%pl.setDatablock(%pl.prevDb);
		if(isObject(%cl = %pl.client) && !%minor)
		{
			%cl.setControlObject(%pl);
			if(isObject(%cl.camera.tmpCont))
				%cl.camera.tmpCont.delete();
		}
	}
}
function player::setMoveSpeedRatio(%pl,%speed)
{
	%pl.setMaxForwardSpeed(%speed);
	%pl.setMaxBackwardSpeed((4/7)*%speed);
	%pl.setMaxSideSpeed((6/7)*%speed);
	%pl.setMaxCrouchForwardSpeed((3/7)*%speed);
	%pl.setMaxCrouchSideSpeed((2/7)*%speed);
	%pl.setMaxCrouchBackwardSpeed((2/7)*%speed);
}
function sm_addDamageType(%name){%name = strLwr(%name);addDamageType("SM_" @ %name,addTaggedString("%1 <bitmap:Add-ons/Weapon_Package_SMelee/icon/ci_" @ %name @ ">"),addTaggedString("%2 <bitmap:Add-ons/Weapon_Package_SMelee/icon/ci_" @ %name @ "> %1"),1,1);}