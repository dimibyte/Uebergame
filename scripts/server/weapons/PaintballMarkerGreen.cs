//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// Particles
// ----------------------------------------------------------------------------
datablock ParticleData(PaintDustGreen)
{
   textureName          = "art/particles/impactDrops.png";
   dragCoefficient      = "0.957967";
   gravityCoefficient   = "0.197803";
   windCoefficient      = 0;
   inheritedVelFactor   = "0";
   constantAcceleration = "0";
   lifetimeMS           = "200";
   lifetimeVarianceMS   = "100";
   spinRandomMin = -180.0;
   spinRandomMax =  180.0;
   useInvAlpha   = true;

   colors[0]     = "0.109804 0.643137 0.00392157 1";
   colors[1]     = "0.14902 0.780392 0.00784314 1";
   colors[2]     = "0.227451 0.878431 0.00784314 1";

   sizes[0]      = "0.399805";
   sizes[1]      = "0.698895";
   sizes[2]      = "1.0987";

   times[0]      = "0.1";
   times[1]      = "0.494118";
   times[2]      = "1.0";
   animTexName = "art/particles/impactDrops.png";
};

datablock ParticleEmitterData(PaintExplosionGreenEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = "1";
   velocityVariance = 1.0;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles = "PaintDustGreen";
   blendStyle = "NORMAL";
   ambientFactor = "0.66";
};
//-----------------------------------------------------------------------------
// Explosion
//-----------------------------------------------------------------------------
datablock ExplosionData(PaintExplosionGreen)
{
   soundProfile = PaintballImpactSoundList;
   lifeTimeMS = 65;

   // Volume particles
   particleEmitter = PaintExplosionGreenEmitter;
   particleDensity = 4;
   particleRadius = 0.3;

   // Point emission
   emitter[0] = BulletDirtSprayEmitter;
   emitter[1] = BulletDirtSprayEmitter;
   emitter[2] = BulletDirtRocksEmitter;
};

//-----------------------------------------------------------------------------
// Projectile Object
//-----------------------------------------------------------------------------
datablock ProjectileData( PaintballProjectileGreen )
{
   projectileShapeName = "art/shapes/weapons/paintball/paintball_green.dts";

   directDamage        = 200;
   radiusDamage        = 0;
   damageRadius        = 0;
   areaImpulse         = 0.5;
   impactForce         = 1;
   damageType          = $DamageType::Paintball;

   explosion           = PaintExplosionGreen;
   decal               = greenPaintSplatterDecal;

   muzzleVelocity      = 33;
   velInheritFactor    = 0.25;

   armingDelay         = 0;
   lifetime            = 12000;
   fadeDelay           = 0;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod          = 0.66;
};

function PaintballProjectileGreen::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
   // Apply impact force from the projectile.
   
   // Apply damage to the object all shape base objects
   if ( %col.getType() & $TypeMasks::GameBaseObjectType )
      %col.damage(%obj,%pos,%this.directDamage,"Paint");
}

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the SoldierWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(PaintballMarkerGreen)
{
   // Mission editor category
   category = "Weapon";

   // Hook into Item Weapon class hierarchy. The weapon namespace
   // provides common weapon handling functions in addition to hooks
   // into the inventory system.
   className = "Weapon";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/paintball/paintball_marker_01_green.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   PreviewImage = 'ryder.png';

   // Dynamic properties defined by the scripts
   pickUpName = "A green PaintballMarker weapon";
   description = "PaintballMarkerGreen";
   image = PaintballMarkerGreenWeaponImage;
   reticle = "crossHair";
};

datablock ShapeBaseImageData(PaintballMarkerGreenWeaponImage : PaintballMarkerBlueWeaponImage)
{
   // Overwrite necessary values to make it green
   shapeFile = "art/shapes/weapons/paintball/paintball_marker_01_green.dts";
   shapeFileFP = "art/shapes/weapons/paintball/paintball_marker_01_green.dts";

   item = PaintballMarkerGreen;
   
   ironSight = PaintballMarkerGreenIronSightImage;

   projectile = PaintballProjectileGreen;
};

datablock ShapeBaseImageData( PaintballMarkerGreenIronSightImage : PaintballMarkerGreenWeaponImage )
{
   firstPerson = false;
   useEyeNode = true;
   animateOnServer = false;
   useEyeOffset = false;
   eyeOffset = "0 0.18 -0.147";
   eyeRotation = "0 0 0 0";

   projectileSpread = "0.005";
   parentImage = "PaintballMarkerGreenWeaponImage";

   stateTimeoutValue[1]             = 0.25;
   stateWaitForTimeout[1]           = true;
   stateSequence[1]                 = "idle";
   stateSound[1]                    = "";
   stateTransitionOnTimeout[1]      = "Ready";
   stateAllowImageChange[1]         = false; 
};

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Paintballer");
SmsInv.AddWeapon(PaintballMarkerGreen, "Green Marker", 1);
