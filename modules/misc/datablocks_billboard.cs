datablock fxLightData(blankBillboard)
{
	LightOn = false;

	flareOn = true;
	flarebitmap = "base/data/shapes/blank.png";
	ConstantSize = 1.5;
    ConstantSizeOn = true;
    FadeTime = inf;

	LinkFlare = false;
	blendMode = 1;
	flareColor = "1 1 1 1";

	AnimOffsets = true;
	startOffset = "0 0 0";
	endOffset = "0 0 0";
};

datablock fxLightData(downedBillboard : blankBillboard)
{
	flarebitmap = "./icons/icon_downed.png";
};