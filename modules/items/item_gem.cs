function brickGem1Data::onPlant(%this, %obj)
{	
	Parent::onPlant(%this,%obj);
	%obj.setrendering(0);
}

function brickGem1Data::onloadPlant(%this, %obj) 
{ 
	brickGem1Data::onPlant(%this, %obj); 
}

function brickGem1StaticShape::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	%obj.setnodeColor("ALL",%obj.spawnbrick.getDataBlock().colorShiftColor);
}

function brickGem2Data::onPlant(%this, %obj)
{	
	brickGem1Data::onPlant(%this,%obj);
}

function brickGem2Data::onloadPlant(%this, %obj) 
{ 
	brickGem1Data::onPlant(%this, %obj); 
}

function brickGem2StaticShape::onAdd(%this,%obj)
{
	brickGem1StaticShape::onAdd(%this,%obj);
}

function brickGem3Data::onPlant(%this, %obj)
{	
	brickGem1Data::onPlant(%this,%obj);
}

function brickGem3Data::onloadPlant(%this, %obj) 
{ 
	brickGem1Data::onPlant(%this, %obj); 
}

function brickGem3StaticShape::onAdd(%this,%obj)
{
	brickGem1StaticShape::onAdd(%this,%obj);
}

function brickGem4Data::onPlant(%this, %obj)
{	
	brickGem1Data::onPlant(%this,%obj);
}

function brickGem4Data::onloadPlant(%this, %obj) 
{ 
	brickGem1Data::onPlant(%this, %obj); 
}

function brickGem4StaticShape::onAdd(%this,%obj)
{
	brickGem1StaticShape::onAdd(%this,%obj);
}

function brickGem5Data::onPlant(%this, %obj)
{	
	brickGem1Data::onPlant(%this,%obj);
}

function brickGem5Data::onloadPlant(%this, %obj) 
{ 
	brickGem1Data::onPlant(%this, %obj); 
}

function brickGem5StaticShape::onAdd(%this,%obj)
{
	brickGem1StaticShape::onAdd(%this,%obj);
}