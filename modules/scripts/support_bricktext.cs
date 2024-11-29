datablock StaticShapeData(BrickTextEmptyShape)
{
    shapefile = "base/data/shapes/empty.dts";
};

package BrickText
{
    function fxDtsBrick::onDeath(%this)
    {
        if (isObject(%this.textShape)) %this.textShape.delete();
        Parent::onDeath(%this);
    }

    function fxDtsBrick::onRemove(%this)
    {
        if (isObject(%this.textShape)) %this.textShape.delete();
        Parent::onRemove(%this);
    }
};

ActivatePackage(BrickText);
