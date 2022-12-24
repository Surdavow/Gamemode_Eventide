function PlayerSkinwalker::onNewDatablock(%this,%obj)
{
	Parent::onNewDatablock(%this,%obj);    	
	%obj.setScale("1.25 1.25 1.25");
    %obj.playthread(0,"roar");
    
    %obj.schedule(1,playaudio,0,"skinwalker_roar_sound");
    %obj.schedule(1, setEnergyLevel,0);    
    for(%i = 0; %i < getRandom(1,2); %i++)
    %obj.schedule(50,spawnExplosion,"goryExplosionProjectile",%obj.getScale());    

    %obj.isSkinwalker = true;
}

function PlayerSkinwalker::EventideAppearance(%this,%obj,%client)
{
    if(isObject(%obj.victimreplicatedclient)) %clientappearance = %obj.victimreplicatedclient;
    else %clientappearance = %client;

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

function PlayerSkinwalker::onCollision(%this,%obj,%col)
{
	Parent::onCollision(%this,%obj,%col);

    if(!isObject(%obj.victim) && Eventide_MinigameConditionalCheck(%obj,%col,false))
    {
        if(isObject(%col.client)) %col.client.setControlObject(%col.client.camera);
        %obj.victim = %col;
        %obj.victimreplicatedclient = %col.client;
        %obj.playthread(1,"eat");
        %obj.playthread(2,"talk");
        %obj.playaudio(1,"skinwalker_grab_sound");
        %obj.mountobject(%col,6);
        %col.schedule(2250,kill);
        %col.schedule(2250,spawnExplosion,"goryExplosionProjectile",%col.getScale()); 
        %col.schedule(2300,delete);        
        %obj.schedule(2250,playthread,1,"root");
        %obj.schedule(2250,playthread,2,"root");
        %obj.schedule(2250,setField,victim,0);
        %this.schedule(2250,EventideAppearance,%obj,%obj.client);
    }
}

function PlayerSkinwalker::onTrigger(%this, %obj, %trig, %press) 
{
	Parent::onTrigger(%this, %obj, %trig, %press);
	
	if(%press) switch(%trig)
	{
		case 0:	Eventide_Melee(%this,%obj,3.5);
		case 4: if(%obj.getEnergyLevel() >= %this.maxEnergy && !isObject(%obj.victim) && !isEventPending(%obj.monstertransformschedule)) 
                %this.monstertransform(%obj,false);
	}
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
            case false: %obj.setdatablock("EventidePlayer");
        }
        %obj.changeaudio = false;
    }
}

function PlayerSkinwalker::onDisabled(%this, %obj, %state) //makes bots have death sound and animation and runs the required bot hole command
{
	Parent::onDisabled(%this, %obj, %state);
}

function PlayerSkinwalker::idlesounds(%this,%obj)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead" || %obj.getdataBlock() !$= %this) return;
}
