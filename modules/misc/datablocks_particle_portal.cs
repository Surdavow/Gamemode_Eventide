datablock ParticleData (PortalParticle)
{
	textureName = "./particles/portal";
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 200;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "0.6 0.6 0.6 0.0";
	colors[1] = "1   1   1 1.0";
	colors[2] = "0.6 0.6 0.6 0.0";
	sizes[0] = 3.2;
	sizes[1] = 3.2;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData (PortalEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = PortalParticle;
	doFalloff = 0;
	doDetail = 0;
	pointEmitterNode = FifthEmitterNode;
	
	uiName = "Portal";
};

datablock ParticleData (PlayerPortalParticle)
{
	textureName = "./particles/portal2";
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	lifetimeMS = 200;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = 0;
	colors[0] = "0.6 0.6 0.6 0.0";
	colors[1] = "1   1   1 1.0";
	colors[2] = "0.6 0.6 0.6 0.0";
	sizes[0] = 3.2;
	sizes[1] = 3.2;
	sizes[2] = 0;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};
datablock ParticleEmitterData (PlayerPortalEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	ejectionOffset = 0.1;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = PlayerPortalParticle;
	doFalloff = 0;
	doDetail = 0;
	pointEmitterNode = FifthEmitterNode;
	
	uiName = "PlayerPortal";
};
datablock ShapeBaseImageData(PlayerPortalImage) 
{
	shapeFile			= "base/data/shapes/empty.dts";
	mountPoint			= 2;
	offset = "0 -0.5 -0.25";
	correctMuzzleVector	= false;
	stateName[0]				= "Portal";
	stateEmitter[0]				= PlayerPortalEmitter;
	stateEmitterTime[0]			= 1000;
	stateWaitForTimeout[0]		= true;
	stateTimeoutValue[0]		= 1000;
	stateTransitionOnTimeout[0]	= "Portal";
	stateScript[0]				= "onPortal";
};