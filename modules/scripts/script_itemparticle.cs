datablock ShapeBaseImageData(TeleportBEmitterImage)
{
  shapeFile = "base/data/shapes/empty.dts";
  
  stateName[0] = "Loop1";
  stateEmitterTime[0] = 1;
  stateEmitter[0] = "PlayerTeleportEmitterB"; //the default orb emitter?
  stateTimeoutValue[0] = 1;
  stateTransitionOnTimeout[0] = "Loop2";

  stateName[1] = "Loop2";
  stateEmitterTime[1] = 1;
  stateEmitter[1] = "PlayerTeleportEmitterB"; //the default orb emitter?
  stateTimeoutValue[1] = 1;
  stateTransitionOnTimeout[0] = "Loop1";
};

package EmitterOnAdd
{
  function ItemData::onAdd(%this, %obj)
  {
    %obj.mountImage(PlayerTeleportEmitterBImage, 0);
    return parent::onAdd(%this, %obj);
  }
};
activatePackage(EmitterOnAdd);