datablock TSShapeConstructor(SkinwalkerDTS)
{
    baseShape  = "./models/skinwalker.dts";
    sequence0  = "./models/skinwalker.dsq";
};

datablock PlayerData(PlayerSkinwalker : PlayerStandardArmor)
{
	isEventideModel = true;
	isKiller = true;
    firstPersonOnly = true;
	showEnergyBar = true;
	canJet = false;	
	useCustomPainEffects = true;
	shapeFile = SkinwalkerDTS.baseShape;
	killerraisearms = true;
	killerlight = "NoFlareRLight";

	renderFirstPerson = false;

	killerSpawnMessage = "Blockheads are the warmest place to hide.";
	
	// Weapon: Claws
	killerHitProjectile = KillerRoughHitProjectile;
	// killerObscureProjectile = ;
	
	killerChaseLvl1Music = "musicData_OUT_SkinwalkerNear";
	killerChaseLvl2Music = "musicData_OUT_SkinwalkerChase";
	
	killeridlesound = "skinwalker_idle";
	killeridlesoundamount = 5;
	
	killerchasesound = "skinwalker_attack";
	killerchasesoundamount = 5;

	killermeleesound = "skinwalker_chase";
	killermeleesoundamount = 3;    

	killerweaponsound = "skinwalker_weapon";
	killerweaponsoundamount = 4;	

	killermeleehitsound = "melee_tanto";
	killermeleehitsoundamount = 3;    

	PainSound = "skinwalker_pain_sound";
	DeathSound = "skinwalker_death_sound";
	JumpSound = "";
	uiName = "Skinwalker Player";	
	
	rechargeRate = 0.375;	
	maxDamage = 9999;
	jumpForce = 9;
	maxForwardSpeed = 6.27;
	maxBackwardSpeed = 3.58;
	maxSideSpeed = 5.38;
	boundingBox = "4.5 4.5 9.5";
	crouchBoundingBox = "4.5 4.5 3.6";
	maxItems   = 0;
	maxWeapons = 0;
	maxTools = 0;	
};

function PlayerSkinwalker::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);    	
	%obj.setScale("1.2 1.2 1.2");

    %obj.stopaudio(0);
    %obj.playthread(0,"roar");
    
    %obj.schedule(1,playaudio,0,"skinwalker_roar_sound");
    %obj.schedule(1, setEnergyLevel,0);    
    for(%i = 0; %i < getRandom(1,2); %i++) %obj.schedule(50,spawnExplosion,"goryExplosionProjectile",%obj.getScale());    

	KillerSpawnMessage(%obj);
    %obj.isSkinwalker = true;
}

function PlayerSkinwalker::EventideAppearance(%this,%obj,%client)
{
    if(isObject(%obj.victimreplicatedclient)) %clientappearance = %obj.victimreplicatedclient;
    else %clientappearance = %client;

    %obj.unHideNode("ALL");
    %obj.setNodeColor($hat[$clientappearance.hat],%client.hatColor);
	%obj.setFaceName(%clientappearance.faceName);
	%obj.setDecalName(%clientappearance.decalName);
	%obj.setNodeColor("head",%clientappearance.headColor);	
	%obj.setNodeColor("chestskinwalker",%clientappearance.chestColor);
	%obj.setNodeColor("pants",%clientappearance.hipColor);
	%obj.setNodeColor("rarm",%clientappearance.rarmColor);
	%obj.setNodeColor("Larm",%clientappearance.larmColor);
	%obj.setNodeColor("lhand",%clientappearance.lhandColor);
	%obj.setNodeColor("rhand",%clientappearance.rhandColor);
	%obj.setNodeColor("rshoe",%clientappearance.rlegColor);
	%obj.setNodeColor("lshoe",%clientappearance.llegColor);
}

function PlayerSkinwalker::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%press) switch(%trig)
	{
		case 0:
				%obj.KillerMelee(%this,4);
				%obj.spawnKillerTrail("raggedClaw", "0.3 1.4 0.7", "0 45 0", "4 4 3");
			
		case 4: if(%obj.getEnergyLevel() >= %this.maxEnergy && !isObject(%obj.victim) && !isEventPending(%obj.monstertransformschedule)) 
                %this.monstertransform(%obj,false);
	}
}

function PlayerSkinwalker::onPeggFootstep(%this,%obj)
{
	serverplay3d("skinwalker_walking" @ getRandom(1,5) @ "_sound", %obj.getHackPosition());
}

function PlayerSkinwalker::monstertransform(%this,%obj,%bool,%count)
{
    if(!isObject(%obj)) return;

    if(%count <= 10)
    {
        if(!%obj.changeaudio) 
        {
            %obj.playaudio(3,"skinwalker_change_sound");
            %obj.changeaudio = true;
        }
        %obj.playthread(0,"plant");
        %obj.monstertransformschedule = %this.schedule(100,monstertransform,%obj,%bool,%count+1);
    }
    else 
    {
        switch(%bool)
        {
            case true: %obj.setdatablock("PlayerSkinwalker");
            case false: if(isObject(%obj.lightbot.light)) %obj.lightbot.light.delete();
                        if(isObject(%obj.lightbot)) %obj.lightbot.delete();	
                        %obj.setdatablock("EventidePlayer");
        }
        %obj.changeaudio = false;
    }
}

package Player_Skinwalker
{

    function ShapeBase::pickup(%obj, %item)
{
        if(%obj.getClassName() $= "Player")
{
            if(%obj.getDataBlock().getName() $= "PlayerSkinwalker")
{
                return;
            }
        }
        parent::pickup(%obj, %item);
    }
    function Player::addItem(%player, %image, %client)
{
        if(!$Player::PlayerSkinwalker::NoAddItem)
{
            parent::addItem(%player, %image, %client);
        }
    }
};

$Player::NoItemsPickup::NoAddItem = 0;
activatePackage(Player_Skinwalker);