$ItemEmitterDatablock = "playerTeleportEmitterB";

function cleanUpParticles()
{
	for (%i = 0; %i < MissionCleanup.getCount(); %i++) 
	{
		if(isObject(%obj = MissionCleanup.getObject(%i)))
		{
			if(%obj.getClassName() $= "ParticleEmitterNode")
		}		
	}
}

function itemEmitterLoop(%obj, %emitterNode)
{
	if (!isObject(%obj))
	{
		if (isObject(%emitterNode)) %emitterNode.delete();		
		return;
	}

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