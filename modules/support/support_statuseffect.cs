function SimObject::StatusEffect(%obj,%type,%id,%data)
{
	%e = "";
	%start = 0;
	while((%sep = strPos(%data,"=",%start)) != -1)
	{
		%end = strPos(%data,"\n",%sep);
		if(%end == -1)
		{
			%end = strLen(%data) - 1;
		}
		%valLen = %end - %sep;
		%nameLen = %sep - %start;

		%name = trim(getSubStr(%data,%start,%nameLen));
		%val = trim(getSubStr(%data,%sep + 1,%valLen));

		%e = %e @ %name @ "=\"" @ %val @ "\";";

		%start = %end + 1;
	}
	return eval("return new ScriptObject(){superClass = \"StatusEffect\";class = %type;obj = %obj;name = %id;" @ %e @ "};");
}

function SimObject::StatusEffect_FindName(%obj,%name,%offset)
{
	%group = %obj.StatusEffect_Group;
	if(!isObject(%group))
	{
		return 0;
	}

	%count = %group.getCount();
	for(%i = %offset + 0; %i < %count; %i++)
	{
		%curr = %group.getObject(%i);
		if(%curr.name $= %name)
		{
			return %curr;
		}
	}
	return 0;
}

function SimObject::StatusEffect_FindType(%obj,%type,%offset)
{
	%group = %obj.StatusEffect_Group;
	if(!isObject(%group))
	{
		return 0;
	}

	%count = %group.getCount();
	for(%i = %offset + 0; %i < %count; %i++)
	{
		%curr = %group.getObject(%i);
		if(%curr.type $= %type)
		{
			return %curr;
		}
	}
	return 0;
}

function StatusEffect::Duration(%e,%duration)
{	
	if(%duration > 0)
	{
		cancel(%e.DespawnSchedule);
		%e.DespawnTime = %duration + getSimTime();
		%e.DespawnSchedule = %e.schedule(%duration,"delete");
	}
	return %e;
}

function StatusEffect::GetDuration(%e,%duration)
{	
	if(%e.DespawnTime > 0)
	{
		return %e.DespawnTime - getSimTime();
	}
	return 0;
}

function StatusEffect::OnAdd(%e)
{
	%obj = %e.obj;
	if(!%obj.StatusEffect_Group)
	{
		%obj.StatusEffect_Group = new ScriptGroup();
	}
	%obj.StatusEffect_Group.add(%e);

	%class = %e.SuperClass;
	if(%e.class !$= "")
	{
		%class = %e.class;
	}
	if(isFunction(%class, "Spawn"))
	{
		%e.spawn(%e.obj);
	}
}

function StatusEffect::OnRemove(%e)
{
	%class = %e.SuperClass;
	if(%e.class !$= "")
	{
		%class = %e.class;
	}
	if(isFunction(%class, "Despawn"))
	{
		%e.Despawn(%e.obj);
	}
}

package StatusEffect
{
	function Armor::OnRemove(%data,%obj)
	{
		if(isObject(%obj.StatusEffect_Group))
		{
			%obj.StatusEffect_Group.DeleteAll();
			%obj.StatusEffect_Group.delete();
		}
		parent::OnRemove(%data,%obj);
	}
};
activatePackage("StatusEffect");