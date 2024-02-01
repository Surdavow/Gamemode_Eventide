// Generic weapon trails:
// - Axe and katana use the same generic straight trail shape.
// - Axe's trail can be scaled up a bit to make it chunkier.
// - Machete can use the ragged trail texture from the ragged claw trail (use mesh skinning).

%root = filePath($Con::File);

addExtraResource(%root @ "/particles/base.trail.png");
addExtraResource(%root @ "/particles/ragged.trail.png");

// Killer claw trails:
// Both a ragged and straight claw trail shape (use mesh skinning).

addExtraResource(%root @ "/particles/baseClaw.trail.png");
addExtraResource(%root @ "/particles/raggedClaw.trail.png");

datablock StaticShapeData(KillerTrailShape)
{
	shapeFile = "./particles/skinnableTrail.dts";
};

// Base blood effects, either to be combined with other hit effects or used by themselves.

datablock ParticleData(KillerBloodRingParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "./hitRing";

	useInvAlpha = true;

	colors[0]	= "0.65 0 0 1";
	colors[1]	= "0.40 0 0 1";

	sizes[0]	= 1;
	sizes[1]	= 3.35;

	times[0]	= 0;
	times[1]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1;
	gravityCoefficient = 0;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 200;
	lifetimeVarianceMS = 0;

	spinSpeed = 1000;
	spinRandomMin = -250;
	spinRandomMax = 250;
};
datablock ParticleEmitterData(KillerBloodRingEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerBloodRingParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 150;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0;
	velocityVariance = 0;
	
	ejectionOffset = 0;
	
	thetaMin = 89;
	thetaMax = 90;
	
	phiReferenceVel = 0;
	phiVariance = 0;
	
	uiName = "Killer's Blood Ring";
};

datablock ParticleData(KillerBloodSprayParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "./hitSpark";
	
	useInvAlpha = true;
	
	colors[0]	= "0.65 0 0 1";
	colors[1]	= "0.40 0 0 1";

	sizes[0]	= 0.75;
	sizes[1]	= 1.3125;
	
	times[0]	= 0;
	times[1]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1;
	gravityCoefficient = 0;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 120;
	lifetimeVarianceMS = 25;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(KillerBloodSprayEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerBloodSprayParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 18;
	periodVarianceMS = 0;
	
	ejectionVelocity = 22;
	velocityVariance = 3;
	
	ejectionOffset = 0.3;
	
	thetaMin = 6;
	thetaMax = 35;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Killer's Blood Spray";
};

datablock ParticleData(KillerBloodSplatterParticle : KillerBloodSprayParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "./hitWave";
	
	sizes[0]	= 1.75;
	sizes[1]	= 3;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 2;

	lifetimeMS = 90;
	lifetimeVarianceMS = 20;
};
datablock ParticleEmitterData(KillerBloodSplatterEmitter : KillerBloodSprayEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerBloodSplatterParticle";

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 60;
	
	ejectionVelocity = 18;
	velocityVariance = 4;
	
	ejectionOffset = 0.5;
	
	thetaMin = 12;
	thetaMax = 60;
	
	uiName = "Killer's Blood Splatter";
};

datablock ParticleData(KillerBloodDropletParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/dot";
	
	useInvAlpha = true;

	colors[0]	= "0.65 0 0 1";
	colors[1]	= "0.40 0 0 1";

	sizes[0]	= 0.25;
	sizes[1]	= 0.1;
	
	times[0]	= 0;
	times[1]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 2.7;
	gravityCoefficient = 0.9;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 660;
	lifetimeVarianceMS = 170;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(KillerBloodDropletEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerBloodDropletParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 20;
	periodVarianceMS = 0;
	
	ejectionVelocity = 16;
	velocityVariance = 4;
	
	ejectionOffset = 0.4;
	
	thetaMin = 2;
	thetaMax = 55;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Killer's Blood Droplet";
};

datablock ParticleData(KillerSlashParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "./thinSlash";
	
	useInvAlpha = false;

	colors[0]	= "1 0.9 0.8 1";
	colors[1]	= "1 0.7 0.6 1";
	colors[2]	= "1 0.3 0.2 0";

	sizes[0]	= 8;
	sizes[1]	= 8;
	sizes[2]	= 8;
	
	times[0]	= 0;
	times[1]	= 0.3;
	times[2]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.2;
	gravityCoefficient = 0;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 0;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(KillerSlashEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerSlashParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 76;
	periodVarianceMS = 0;
	
	ejectionVelocity = -14;
	velocityVariance = 0;
	
	ejectionOffset = 3;
	
	thetaMin = 160;
	thetaMax = 180;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Killer's Slash";
};

// Generic weapon hit effects:
// - Axe, machete, and katana use the same sparkly hit slash effect, combined with the blood effects.

datablock ExplosionData(KillerSharpHitExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	particleEmitter = KillerBloodRingEmitter;
	particleDensity = 1;
	particleRadius = 0;

	emitter[0] = KillerBloodSprayEmitter;
	emitter[1] = KillerBloodDropletEmitter;
	emitter[2] = KillerSlashEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	shakeCamera = true;
	camShakeFreq = "2 3 2";
	camShakeAmp = "1 1 1";
	camShakeDuration = 0.75;
	camShakeRadius = 15;
};
datablock ProjectileData(KillerSharpHitProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = KillerSharpHitExplosion;
	explodeOnDeath = true;
	
	//-------------//
	// Properties: //
	//-------------//
	
	uiName = "Killer's Sharp Hit";
};

datablock ExplosionData(KillerRoughHitExplosion : KillerSharpHitExplosion)
{
	//------------//
	// Rendering: //
	//------------//

	emitter[0] = KillerBloodSplatterEmitter;
};
datablock ProjectileData(KillerRoughHitProjectile : KillerSharpHitProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = KillerRoughHitExplosion;
	
	//-------------//
	// Properties: //
	//-------------//
	
	uiName = "Killer's Rough Hit";
};

// Generic weapon clank effects:
// - Axe: heavy rock shards.

datablock ParticleData(KillerAxeClankDustParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/cloud";
	
	useInvAlpha = true;

	colors[0]	= "1 1 1 0.1";
	colors[1]	= "1 1 1 0.07";
	colors[2]	= "1 1 1 0";

	sizes[0]	= 0.35;
	sizes[1]	= 0.8;
	sizes[2]	= 1.2;
	
	times[0]	= 0;
	times[1]	= 0.2;
	times[2]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.7;
	gravityCoefficient = -0.35;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 700;
	lifetimeVarianceMS = 200;

	spinSpeed = 20;
	spinRandomMin = -20;
	spinRandomMax = 20;
};
datablock ParticleEmitterData(KillerAxeClankDustEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerAxeClankDustParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 2;
	periodVarianceMS = 0;
	
	ejectionVelocity = 8;
	velocityVariance = 2;
	
	ejectionOffset = 0.2;
	
	thetaMin = 80;
	thetaMax = 90;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Axe Clank Dust";
};

datablock ParticleData(KillerAxeClankSprayParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "./hitSpark";
	
	useInvAlpha = false;

	colors[0]	= "1 1 1 1";
	colors[1]	= "1 1 1 1";

	sizes[0]	= 0.5;
	sizes[1]	= 1.25;
	
	times[0]	= 0;
	times[1]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1;
	gravityCoefficient = 0;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 130;
	lifetimeVarianceMS = 20;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(KillerAxeClankSprayEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerAxeClankSprayParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 16;
	periodVarianceMS = 2;

	ejectionVelocity = 14;
	velocityVariance = 0;
	
	ejectionOffset = 0;
	
	thetaMin = 70;
	thetaMax = 80;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Axe Clank Spray";
};

datablock ParticleData(KillerAxeClankChunkParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/chunk";
	
	useInvAlpha = true;

	colors[0]	= "0.2 0.2 0.2 1";
	colors[1]	= "0.1 0.1 0.1 1";

	sizes[0]	= 1;
	sizes[1]	= 0;
	
	times[0]	= 0;
	times[1]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.2;
	gravityCoefficient = 1.7;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 300;

	spinSpeed = 1000;
	spinRandomMin = -125;
	spinRandomMax = 125;
};
datablock ParticleEmitterData(KillerAxeClankChunkEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerAxeClankChunkParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 18;
	periodVarianceMS = 3;
	
	ejectionVelocity = 8;
	velocityVariance = 4;
	
	ejectionOffset = 0.3;
	
	thetaMin = 5;
	thetaMax = 30;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Axe Clank Chunk";
};

datablock ExplosionData(KillerAxeClankExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	emitter[0] = KillerAxeClankDustEmitter;
	emitter[1] = KillerAxeClankSprayEmitter;
	emitter[2] = KillerAxeClankChunkEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	shakeCamera = true;
	camShakeFreq = "10 11 10";
	camShakeAmp = "0.75 0.75 0.75";
	camShakeDuration = 0.75;
	camShakeRadius = 15;
};
datablock ProjectileData(KillerAxeClankProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = KillerAxeClankExplosion;
	explodeOnDeath = true;
	
	//-------------//
	// Properties: //
	//-------------//
	
	uiName = "Axe Clank";
};

// - Machete: smaller rock shards with some sparks.

datablock ParticleData(KillerMacheteClankSprayParticle : KillerAxeClankSprayParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	sizes[0]	= 0.35;
	sizes[1]	= 0.9;

	//-------------//
	// Properties: //
	//-------------//

	lifetimeMS = 100;
	lifetimeVarianceMS = 10;
};
datablock ParticleEmitterData(KillerMacheteClankSprayEmitter : KillerAxeClankSprayEmitter)
{
	//------------//
	// Rendering: //
	//------------//

	particles = "KillerMacheteClankSprayParticle";

	//-------------//
	// Properties: //
	//-------------//

	ejectionVelocity = 10;

	thetaMin = 40;
	thetaMax = 50;

	uiName = "Machete Clank Spray";
};

datablock ParticleData(KillerMacheteClankChunkParticle : KillerAxeClankChunkParticle)
{
	//------------//
	// Rendering: //
	//------------//

	sizes[0]	= 0.7;

	//-------------//
	// Properties: //
	//-------------//
	
	lifetimeMS = 850;
	lifetimeVarianceMS = 250;

	spinSpeed = 900;
};
datablock ParticleEmitterData(KillerMacheteClankChunkEmitter : KillerAxeClankChunkEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerMacheteClankChunkParticle";

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 30;
	periodVarianceMS = 4;
	
	ejectionVelocity = 6;
	velocityVariance = 3.2;
	
	thetaMin = 5;
	thetaMax = 20;
	
	uiName = "Machete Clank Chunk";
};

datablock ParticleData(KillerMacheteClankSparkParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "./pSpark";
	
	useInvAlpha = false;
	
	colors[0]	= "1 0.9 0.8 1";
	colors[1]	= "1 0.7 0.6 1";
	colors[2]	= "1 0.5 0.4 1";
	colors[3]	= "1 0.3 0.2 1";

	sizes[0]	= 0;
	sizes[1]	= 0.7;
	sizes[2]	= 0.5;
	sizes[3]	= 0;
	
	times[0]	= 0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0;
	gravityCoefficient = 2;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 400;
	lifetimeVarianceMS = 200;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(KillerMacheteClankSparkEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerMacheteClankSparkParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 16;
	periodVarianceMS = 4;
	
	ejectionVelocity = 8;
	velocityVariance = 2;
	
	ejectionOffset = 0.2;
	
	thetaMin = 10;
	thetaMax = 35;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Machete Clank Spark";
};

datablock ExplosionData(KillerMacheteClankExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	emitter[0] = KillerMacheteClankSprayEmitter;
	emitter[1] = KillerMacheteClankChunkEmitter;
	emitter[2] = KillerMacheteClankSparkEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	shakeCamera = true;
	camShakeFreq = "5 6 5";
	camShakeAmp = "0.75 0.75 0.75";
	camShakeDuration = 0.75;
	camShakeRadius = 15;
};
datablock ProjectileData(KillerMacheteClankProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = KillerMacheteClankExplosion;
	explodeOnDeath = true;
	
	//-------------//
	// Properties: //
	//-------------//
	
	uiName = "Machete Clank";
};

// - Katana: sharp impact slash with sparks.

datablock ParticleData(KillerKatanaClankSprayParticle : KillerAxeClankSprayParticle)
{
	//------------//
	// Rendering: //
	//------------//

	colors[1]	= "1 0.7 0.6 1";

	sizes[0]	= 0.35;
	sizes[1]	= 0.9;

	//-------------//
	// Properties: //
	//-------------//

	lifetimeMS = 100;
	lifetimeVarianceMS = 10;
};
datablock ParticleEmitterData(KillerKatanaClankSprayEmitter : KillerAxeClankSprayEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerKatanaClankSprayParticle";

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 20;

	ejectionVelocity = 10;

	thetaMin = 30;
	thetaMax = 45;

	uiName = "Katana Clank Spray";
};

datablock ParticleData(KillerKatanaClankChunkParticle : KillerAxeClankChunkParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/nut";
	
	useInvAlpha = false;

	colors[0]	= "1 0.9 0.8 1";
	colors[1]	= "1 0.3 0.2 1";

	sizes[0]	= 0.5;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.5;

	lifetimeMS = 800;

	spinSpeed = 900;
};
datablock ParticleEmitterData(KillerKatanaClankChunkEmitter : KillerAxeClankChunkEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "KillerKatanaClankChunkParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 20;
	periodVarianceMS = 2;

	velocityVariance = 3.5;
	
	ejectionOffset = 0.2;
	
	thetaMin = 10;
	thetaMax = 35;
	
	uiName = "Katana Clank Chunk";
};

datablock ExplosionData(KillerKatanaClankExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	emitter[0] = KillerKatanaClankSprayEmitter;
	emitter[1] = KillerKatanaClankChunkEmitter;
	emitter[2] = KillerMacheteClankSparkEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	shakeCamera = true;
	camShakeFreq = "2 3 2";
	camShakeAmp = "1 1 1";
	camShakeDuration = 0.75;
	camShakeRadius = 15;
};
datablock ProjectileData(KillerKatanaClankProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = KillerKatanaClankExplosion;
	explodeOnDeath = true;
	
	//-------------//
	// Properties: //
	//-------------//
	
	uiName = "Katana Clank";
};

/// relativeVectorToRotation by Xalos.
///
/// @param	for	3-element vector
/// @param	up	3-element vector
function relativeVectorToRotation(%for, %up)
{
    %yaw = mAtan(getWord(%for, 0), -getWord(%for, 1)) + $pi;
    
	if(%yaw >= $pi)
		%yaw -= $m2pi;
    
    %rightAxis = vectorNormalize(vectorCross(%for, "0 0 1"));
	
	if(vectorLen(%rightAxis) == 0)
		%rightAxis = "-1 0 0";
    
	%upAxis = vectorNormalize(vectorCross(%rightAxis, %for));
    
	%rDot = vectorDot(%up, %rightAxis);
    %uDot = vectorDot(%up, %upAxis);
    
    %euler = mAsin(vectorDot(vectorNormalize(%for), "0 0 1")) SPC mAtan(%rDot, %uDot) SPC %yaw;
	%matrix = MatrixCreateFromEuler(%euler);
    return getWords(%matrix, 3, 6);
}

/// getLookVector by Conan & Darksaber.
///
/// @param	this	player
function Player::getLookVector(%this)
{
	%forward = %this.getForwardVector();
	%eye = %this.getEyeVector();
	%up = %this.getUpVector();
	
	// Z-axis relative to the up vector.
	%magnitudeZ = vectorDot(%eye, %up);
	%localZ = vectorScale(%up, %magnitudeZ); 
	
	// Relative X/Y plane (eye vector minus relative Z-axis).
	%localPlane = vectorSub(%eye, %localZ); 
	
	// Y-axis relative to the forward vector.
	%magnitudeY = vectorDot(%localPlane, %forward); 
	%localY = vectorScale(%forward, %magnitudeY); 
	
	// Relative X-axis (X/Y plane minus relative Y-axis).
	%localX = vectorSub(%localPlane, %localY); 
	%magnitudeX = vectorLen(%localX);
	
	%magnitudePlane = mSqrt(%magnitudeX * %magnitudeX + %magnitudeY * %magnitudeY);
	
	return vectorAdd(vectorScale(%forward, %magnitudePlane), %localZ);
}

/// @param	this	player
/// @param	skin	string
/// @param	offset	3-element position
/// @param	angle	3-element euler rotation (in degrees)
/// @param	scale	3-element vector
function Player::spawnKillerTrail(%this, %skin, %offset, %angle, %scale)
{
	%shape = new StaticShape()
	{
		dataBlock = KillerTrailShape;
		scale = %scale;
	};
	
	if(isObject(%shape))
	{
		MissionCleanup.add(%shape);
		
		%shape.setSkinName(%skin);
		
		%rotation = relativeVectorToRotation(%this.getLookVector(), %this.getUpVector());
		%clamped = mClampF(firstWord(%rotation), -89.9, 89.9) SPC restWords(%rotation);
		
		%local = %this.getPosition() SPC %clamped;
		%combined = %offset SPC eulerToQuat(%angle);
		%actual = matrixMultiply(%local, %combined);
		
		%shape.setTransform(%actual);
		%shape.schedule(0, playThread, 0, "rotate");
		%shape.schedule(1000, delete);
	}
}