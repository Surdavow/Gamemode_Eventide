function brickGemData::onPlant(%this, %obj)
{	
	brickCandleData::onPlant(%this,%obj);
}

function brickGemData::onloadPlant(%this, %obj) 
{ 
	brickGemData::onPlant(%this, %obj); 
}

function brickGem1StaticShape::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
}

function brickGem2StaticShape::onAdd(%this,%obj)
{
	brickGem1StaticShape::onAdd(%this,%obj);
}

function brickGem3StaticShape::onAdd(%this,%obj)
{
	brickGem1StaticShape::onAdd(%this,%obj);
}

function brickGem4StaticShape::onAdd(%this,%obj)
{
	brickGem1StaticShape::onAdd(%this,%obj);
}

function brickGem5StaticShape::onAdd(%this,%obj)
{
	brickGem1StaticShape::onAdd(%this,%obj);
}