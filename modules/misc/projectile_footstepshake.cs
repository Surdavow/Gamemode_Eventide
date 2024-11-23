datablock ExplosionData(Eventide_footstepShakeExplosion)
{
	explosionShape = "";
	lifeTimeMS = 150;
	
	faceViewer     = true;
	explosionScale = "1 1 1";
	
	shakeCamera = true;
	camShakeFreq = "7.0 7.0 7.0";
	camShakeAmp = "0.25 0.25 0.25";
	camShakeDuration = 0.375;
	camShakeRadius = 100;
	
	damageRadius = 0;
	radiusDamage = 0;
	
	impulseRadius = 0;
	impulseForce = 0;
};

datablock ProjectileData(Eventide_footstepShakeProjectile)
{
	projectileShapeName = "";
	directDamage        = 0;
	directDamageType    = $DamageType::EarthquakeDirectProj;
	radiusDamageType    = $DamageType::EarthquakeRadiusProj;
	
	
	brickExplosionRadius = 0;
	brickExplosionImpact = false;
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;
	
	impactImpulse	   = 0;
	verticalImpulse	   = 0;
	explosion           = Eventide_footstepShakeExplosion;
	
	muzzleVelocity      = 0;
	velInheritFactor    = 1;
	
	armingDelay         = 00;
	lifetime            = 1;
	fadeDelay           = 0;
	bounceElasticity    = 0.0;
	bounceFriction      = 0.0;
	isBallistic         = false;
	gravityMod		   = 0.0;
	
	hasLight    = false;
	lightRadius = 10;
	lightColor  = "0.0 1.0 1.0";
	
	explodeOnDeath = 1;
};