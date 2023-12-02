datablock ParticleData(PeeParticle)
{
	textureName          = "base/data/particles/dot";
	dragCoefficient      = 0.0;
	gravityCoefficient   = 0.75;
	inheritedVelFactor   = 1.0;
	windCoefficient      = 0;
	constantAcceleration = 0;
	lifetimeMS           = 5000;
	lifetimeVarianceMS   = 0;
	spinSpeed     = 0;
	spinRandomMin = -90.0;
	spinRandomMax =  90.0;
	useInvAlpha   = false;

	colors[0]	= "1.0 1 0.0 1.0";
	colors[1]	= "1.0 1 0.0 1.0";
	colors[2]	= "1.0 1 1.0 0.0";

	sizes[0]	= 0.1;
	sizes[1]	= 0.1;
	sizes[2]	= 0.1;

	times[0]	= 0.0;
	times[1]	= 0.9;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(PeeEmitter)
{
   ejectionPeriodMS = 8;
   periodVarianceMS = 0;
   ejectionVelocity = 6.0;
   ejectionOffset   = 0;
   velocityVariance = 0.2;
   thetaMin         = 0;
   thetaMax         = 2;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = true;

   particles = PeeParticle;

   useEmitterColors = true;

	uiName = "";
};
datablock ItemData(PeeingItem:PrintGun)
{
    uiname = "Pee";
    image = PeeingItemImage;
    colorshiftColor = "1 1 0 1";
};
datablock ShapeBaseImageData(PeeingItemImage:PrintGunImage)
{
   projectile = "";
   mountpoint = 0;
   firstPerson = 0;
   correctMuzzleVector = 1;
   armready = 0;
   showBricks = 0;
   shapeFile="base/data/shapes/empty.dts";
   stateSound[2] = "";
   stateEmitter[2] = "";
   stateTimeoutValue[2] = 1;
   stateAllowImageChange[2] = 0;
};
datablock ShapeBaseImageData(PeeingImage)
{
   mountpoint = 7;
   armready = 0;
   emap = false;
   shapeFile="base/data/shapes/empty.dts";
   offset = "0 0 0.2";
   rotation = eulertoMatrix("10 0 0");
   stateName[0]					= "Start";
	stateTransitionOnTimeout[0]		= "Emitter";
	stateTimeoutValue[0]			= "0.01";
	stateName[1] = "Emitter";
	stateTransitionOnTimeout[1] = "Done";
	stateWaitForTimeout[1] = "1";
	stateTimeoutValue[1]	= "1";
	stateEmitter[1] = "PeeEmitter";
	stateEmitterTime[1] = "1";
	stateName[2]					= "Done";
	stateScript[2]					= "onDone";
};
function PeeingImage::onDone(%this,%obj,%slot)
{
	%obj.unMountImage(%slot);
}
function PeeingItemImage::onMount(%this,%obj,%slot)
{
   %obj.playThread(1,"root");
}
function PeeingItemImage::onFire(%data,%obj,%slot){%obj.emote(PeeingImage,1);}