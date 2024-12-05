package Eventide_Medical_Package
{
    function ZombieMedpackImage::healLoop(%this, %obj)
    {
		Parent::healLoop(%this, %obj);		
		
		if(%obj.zombieMedpackUse >= 3.4)
		{
			%obj.pseudoHealth = (%obj.survivorclass $= "fighter") ? 75 : 0;
			%obj.setHealth(%obj.getDataBlock().maxDamage);
			%obj.hasBeenDowned = false;
		}
    }

    function GauzeImage::healLoop(%this, %obj)
    {
    	Parent::healLoop(%this, %obj);
    
		if(%obj.GauzeUse >= 2.4)
		{
			%obj.pseudoHealth = (%obj.survivorclass $= "fighter") ? 75 : 0;
			%obj.addHealth(%obj.getDataBlock().maxDamage/2);
			%obj.hasBeenDowned = false;
		}
    }
};
activatePackage(Eventide_Medical_Package);