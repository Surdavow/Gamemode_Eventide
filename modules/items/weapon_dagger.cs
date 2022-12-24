function brickdaggerData::onPlant(%this, %obj)
{	
	Parent::onAdd(%this,%obj);
	%obj.setrendering(0);
}

function brickdaggerData::onloadPlant(%this, %obj) 
{ 
	brickCandleData::onPlant(%this, %obj); 
}

function daggerImage::onReady(%this, %obj, %slot)
{
	%obj.playthread(1, "root");
}

function daggerImage::onFire(%this, %obj, %slot)
{
	if(%obj.getstate() $= "Dead") return;
	DaggerCheck(%obj,%this,%slot);
	%obj.playthread(1, "shiftTo");
	
}

function daggerImage::onPreFire(%this, %obj, %slot)
{	
	%obj.playthread(1, "shiftAway");
}

function DaggerCheck(%obj,%this,%slot)
{   
	%pos = %obj.getMuzzlePoint(%slot);
	%radius = 3;
	%searchMasks = $TypeMasks::StaticObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::FxBrickObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);
	while (%target = containerSearchNext())
   	{
      	if(%target == %obj) continue;
	
      	%len = 2 * getWord(%obj.getScale (), 2);
      	%vec = %obj.getMuzzleVector(%slot);
      	%ray = containerRayCast(%pos,%target.getposition(),%searchMasks,%obj);


     	if(vectorDist(%pos,posFromRaycast(%ray)) > 2) continue;

		%p = new projectile()
		{
			datablock = "daggerProjectile";
			initialPosition = posFromRaycast(%ray);
		};
		%p.explode();        

     	if(%ray.getType() & $TypeMasks::FxBrickObjectType || %ray.getType() & $TypeMasks::StaticObjectType || %ray.getType() & $TypeMasks::VehicleObjectType)
     	{
			serverPlay3D("sworddaggerhitEnv" @ getRandom(1,2) @ "_sound",posFromRaycast(%ray));
			continue;
     	}

		else if(%ray.getType() & $TypeMasks::PlayerObjectType)
		{
			if(%ray.getstate() $= "Dead") continue;

			%damage = %ray.getdatablock().maxDamage/8;

			if(vectorDot(%ray.getforwardvector(),%obj.getforwardvector()) > 0.65) %damageclamp = mClamp(%damagepower*1.5, 40, %ray.getdatablock().maxDamage);
			else %damageclamp = mClamp(%damagepower, 40, %ray.getdatablock().maxDamage);
			serverPlay3D("sworddaggerhitPL" @ getRandom(1,2) @ "_sound",posFromRaycast(%ray));
			
			if(isObject(getMiniGameFromObject(%obj,%ray)) && checkHoleBotTeams(%obj,%ray))
			{
				%ray.damage(%obj, posFromRaycast(%ray), %damageclamp, $DamageType::Default);
				%ray.applyimpulse(posFromRaycast(%ray),vectoradd(vectorscale(%vec,1000),"0 0 750"));				
			}
		}
   } 
   return;
}