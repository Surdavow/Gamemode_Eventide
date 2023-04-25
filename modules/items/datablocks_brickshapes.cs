datablock fxDTSBrickData(brickBookData : brick2x2FData)
{
	category = "Special";
	subCategory = "Eventide";
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
	placementSound = "book_place_sound";
};

datablock StaticShapeData(brickdaggerStaticShape)
{
	isInvincible = true;
	shapeFile = "./models/daggerstatic.dts";
	placementSound = "sworddagger_place_sound";
};

datablock fxDTSBrickData(brickdaggerData : brick1x1FData)
{
	category = "Special";
	subCategory = "Eventide";
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
	placementSound = "candle_place_sound";
};

datablock fxDTSBrickData(brickCandleData)
{
	category = "Special";
	subCategory = "Eventide";
	uiName = "Candle";
	iconName = candleItem.iconName;
	brickFile = "./models/1x1x3.blb";

	staticShapeItemMatch = "candleImage";
	staticShape = brickCandleStaticShape;
	shapeBrickPos = "0 0 -0.6";	
	colorShiftColor = "1 1 1 1";	
};

datablock fxDTSBrickData(brickGemData : brick1x1FData)
{
	category = "Special";
	subCategory = "Eventide";
	uiName = "Gem";	
	iconName = gem1Item.iconName;

	staticShapeItemMatch = "gem";
	staticShape = "gem";
};

datablock StaticShapeData(brickGem1StaticShape)
{
	isInvincible = true;
	shapeFile = "./models/gem1.dts";
	placementSound = "gem_place_sound";
};

datablock StaticShapeData(brickGem2StaticShape : brickGem1StaticShape)
{
	shapeFile = "./models/gem2.dts";
};

datablock StaticShapeData(brickGem3StaticShape : brickGem1StaticShape)
{
	shapeFile = "./models/Gem3.dts";
};

datablock StaticShapeData(brickGem4StaticShape : brickGem1StaticShape)
{
	shapeFile = "./models/Gem4.dts";
};

datablock StaticShapeData(brickGem5StaticShape : brickGem1StaticShape)
{
	shapeFile = "./models/Gem5.dts";
};