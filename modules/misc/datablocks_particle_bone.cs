datablock DebrisData(singleBoneDebris)
{
   emitters = "";

	shapeFile = "./models/bone.dts";
	lifetime = 10;
	spinSpeed			= 2000.0;
	minSpinSpeed = -100.0;
	maxSpinSpeed = 100.0;
	elasticity = 0.5;
	friction = 0.4;
	numBounces = 2;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 1;
};

datablock ExplosionData(singleBoneExplosion)
{
	soundProfile		= "";
   	explosionShape = "";
	lifeTimeMS = 10;
	debris = singleBoneDebris;
	debrisNum = 1;
	debrisNumVariance = 1;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 180;
	debrisVelocity = 8;
	debrisVelocityVariance = 6;
	particleEmitter = "";

	faceViewer     = true;
	explosionScale = "1 1 1";
};

datablock ProjectileData(singleBoneProjectile)
{
	uiname							= "";
	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;
	explosion						= singleBoneExplosion;

};