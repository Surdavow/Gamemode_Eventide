datablock fxLightData(blankBillboard)
{
	LightOn = false;
	flareOn = true;
	ConstantSizeOn = true;
	flarebitmap = "base/data/shapes/blank.png";
	ConstantSize = 2;    
	nearSize = 2;
	farSize = 2;
	farDistance = 9999;
	LinkFlare = false;
	AnimOffsets = false;
    FadeTime = 99999;
	
	blendMode = 1;
	flareColor = "1 1 1 1";	
};

datablock fxLightData(downedBillboard : blankBillboard)
{
	flarebitmap = "./icons/icon_downed.png";
};