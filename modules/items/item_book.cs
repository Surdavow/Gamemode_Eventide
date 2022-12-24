function bookImage::onUnmount(%this,%obj,%slot)
{    
    Parent::onUnmount(%this,%obj,%slot);
    %obj.playAudio(1,"bookconceal_sound");
    %obj.playthread(2,"plant");
}

function bookImage::onMount(%this,%obj,%slot)
{    
    Parent::onMount(%this,%obj,%slot);
    %obj.playAudio(1,"bookequip_sound");
    %obj.playthread(1,"armReadyBoth");
    %obj.playthread(2,"plant");
}

function brickBookData::onPlant(%this, %obj)
{	
	Parent::onPlant(%this,%obj);
	%obj.setrendering(0);
}

function brickBookData::onloadPlant(%this, %obj) 
{ 
	brickCandleData::onPlant(%this, %obj); 
}