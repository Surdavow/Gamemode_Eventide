$ItemEmitterDatablock = "playerTeleportEmitterB"; // put the emitter datablock name here, inside the quotes
// for reference: \$itemEmitterDatablock = %hit.emitter.emitter

package ItemEmitterEffects
{
	function ItemData::onAdd(%this, %obj) // triggered every time an item is created
	{
		// checks for if the item is spawned from a spawn brick (ie repeat pickup) or is static (ie doesn't have gravity/fall)
		if (!isObject(%obj.spawnBrick) && !%obj.static) 
		{
			itemEmitterLoop(%obj); // starts the loop to create and move the emitterNode 
		}
		return parent::onAdd(%this, %obj);
	}
};
activatePackage(ItemEmitterEffects);

function itemEmitterLoop(%obj, %emitterNode)
{
	// failsafe: deletes the emitter when the item is gone
	if (!isObject(%obj))
	{
		if (isObject(%emitterNode))
		{
			%emitterNode.delete();
		}
		return;
	}
	// failsafe 2: can't stack looping function calls multiple times on the same item
	cancel(%obj.itemEmitterLoopSchedule);

	if (!isObject(%emitterNode))
	{
		// creates emitter node using either the emitter node setting, or the default GenericEmitterNode
		%nodeData = $ItemEmitterDatablock.pointEmitterNode;
		if (!isObject(%nodeData))
		{
			%nodeData = GenericEmitterNode;
		}
		%emitterNode = new ParticleEmitterNode ("")
		{
			dataBlock = %nodeData;
			emitter = $ItemEmitterDatablock;
		};
		%emitterNode.setEmitterDataBlock($ItemEmitterDatablock);
		MissionCleanup.add(%emitterNode);
	}
	%emitterNode.setTransform(%obj.getTransform()); // moves the node
	%emitterNode.inspectPostApply(); //sends updated position to clients
	%obj.emitter = %emitterNode; // just for convenience if some script wants to use this? idk

	%obj.itemEmitterLoopSchedule = schedule(50, 0, itemEmitterLoop, %obj, %emitterNode); // schedule a repeat call to loop this function
}