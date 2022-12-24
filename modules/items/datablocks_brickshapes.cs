datablock fxDTSBrickData(brickBookData : brick2x2FData)
{
	category = "Special";
	subCategory = "Misc";
	uiName = "Book";
	iconName = bookItem.iconName;
	

	staticShapeItemMatch = "bookImage";
	staticShape = "brickBookStaticShape";
	shapeBrickPos = "0 0 -0.05";
	colorShiftColor = "1 1 1 1";
};

datablock StaticShapeData(brickBookStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/book.dts";
};

datablock StaticShapeData(brickdaggerStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/daggerstatic.dts";
};

datablock fxDTSBrickData(brickdaggerData : brick1x1FData)
{
	category = "Special";
	subCategory = "Misc";
	uiName = "dagger";
	iconName = daggerItem.iconName;
	
	staticShapeItemMatch = "daggerImage";
	staticShape = "brickdaggerStaticShape";
	shapeBrickPos = "0 0 0";
	colorShiftColor = "1 1 1 1";	
};

datablock StaticShapeData(brickCandleStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/candlestatic.dts";
};

datablock fxDTSBrickData(brickCandleData)
{
	category = "Special";
	subCategory = "Misc";
	uiName = "Candle";
	iconName = candleItem.iconName;
	brickFile = "./models/1x1x3.blb";

	staticShapeItemMatch = "candleImage";
	staticShape = brickCandleStaticShape;
	shapeBrickPos = "0 0 -0.6";	
	colorShiftColor = "1 1 1 1";	
};

datablock StaticShapeData(brickGem1StaticShape)
{
	isInvincible = true;
	shapeFile = "./models/gem1.dts";
};

datablock fxDTSBrickData(brickGem1Data : brick1x1FData)
{
	category = "Special";
	subCategory = "Misc";
	uiName = "Gem Variant 1";	
	iconName = gem1Item.iconName;

	staticShapeItemMatch = "gem1Image";
	staticShape = "brickGem1StaticShape";
	shapeBrickPos = "0 0 0";
	colorShiftColor = gem1Item.colorShiftColor;
};

datablock StaticShapeData(brickGem2StaticShape : brickGem1StaticShape)
{
	shapeFile = "./models/gem2.dts";
};

datablock fxDTSBrickData(brickGem2Data : brickGem1Data)
{
	uiName = "Gem Variant 2";
	staticShapeItemMatch = "gem2Image";
	staticShape = "brickGem2StaticShape";
	colorShiftColor = gem2Item.colorShiftColor;
};

datablock StaticShapeData(brickGem3StaticShape : brickGem1StaticShape)
{
	shapeFile = "./models/Gem3.dts";
};

datablock fxDTSBrickData(brickGem3Data : brickGem1Data)
{
	uiName = "Gem Variant 3";
	staticShapeItemMatch = "gem3Image";
	staticShape = "brickGem3StaticShape";
	colorShiftColor = Gem3Item.colorShiftColor;
};

datablock StaticShapeData(brickGem4StaticShape : brickGem1StaticShape)
{
	shapeFile = "./models/Gem4.dts";
};

datablock fxDTSBrickData(brickGem4Data : brickGem1Data)
{
	uiName = "Gem Variant 4";
	staticShapeItemMatch = "gem4Image";
	staticShape = "brickGem4StaticShape";
	colorShiftColor = Gem4Item.colorShiftColor;
};

datablock StaticShapeData(brickGem5StaticShape : brickGem1StaticShape)
{
	shapeFile = "./models/Gem5.dts";
};

datablock fxDTSBrickData(brickGem5Data : brickGem1Data)
{
	uiName = "Gem Variant 5";
	staticShapeItemMatch = "gem5Image";
	staticShape = "brickGem5StaticShape";
	colorShiftColor = Gem5Item.colorShiftColor;
};