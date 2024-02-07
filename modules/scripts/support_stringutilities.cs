function getString(%s,%sep,%a)
{
	for(%i = 0; %i <= %a; %i++) %s = nextToken(%s,"t",%sep);
	return %t;
}

function getStringCount(%s,%sep)
{
	%c = 0;
	while(%s !$= "")
	{
		%s = nextToken(%s,"",%sep);
		%c++;
	}
	return %c;
}

function getStrings(%s,%sep,%a,%b)
{
	for(%i = 0; %i < %a; %i++)
	{
		%s = nextToken(%s,"",%sep);
	}

	for(%i = 0; %i < %b; %i++)
	{
		%s = nextToken(%s,"t",%sep);
		%r = %r @ %t @ %sep;
	}
	return getSubStr(%r,0,strLen(%r) - 1);
}

function removeString(%s,%sep,%a)
{
	for(%i = 0; %i < %a; %i++)
	{
		%s = nextToken(%s,"t",%sep);
		%r = %r @ %t @ %sep;
	}

	return %r @ %s;
}

function setString(%s,%sep,%a,%v)
{
	%temp = "";
	%s = nextToken(%s,"t",%sep);
	for(%i = 0; %i < %a; %i++)
	{
		%r = %r @ %t @ %sep;
		%s = nextToken(%s,"t",%sep);
	}

	return %r @ %v @ %sep @ %s;
}

function findString(%s,%sep,%v)
{
	%temp = "";
	%a = getWordCount(%s);
	%s = nextToken(%s,"t",%sep);
	for(%i = 0; %i < %a; %i++)
	{
		if(%t $= %v)
		{
			return %i;
		}
		%s = nextToken(%s,"t",%sep);
	}
	return -1;
}

function sortStrings(%s,%sep,%comp)
{
	%count = getStringCount(%s,%sep);

	%ss = getString(%s,%sep,0);
	%sortedCount = 1; 
	for(%i = 1; %i < %count; %i++)
	{
		%v1 = getString(%s,%sep,%i);
		for(%j = 0; %j < %sortedCount; %j++)
		{
			%v2 = getString(%ss,%sep,%j);
			%result = eval("return " @ %comp @ ";");
			if(%result $= "") return;
			if(%result)
			{
				%ss = setString(%ss,%sep,%j,%v1 @ %sep @ %v2);
				%sortedCount++;
				break;
			}
		}

		if(%j == %sortedCount)
		{
			%ss = %ss @ %sep @ %v1;
			%sortedCount++;
		}
	}
	return %ss;
}

function stringList(%s,%listSep,%comma,%type)
{
	%wcount = getStringCount(%s,%listSep);
	if(%wcount <= 1)
	{
		return %s;
	}

	for(%j = 0; %j < %wcount; %j++)
	{
		%lists = "";
		if(%j <= %wCount - 2)
		{
			if(%wCount > 2)
			{
				%lists = %comma;
			}
			
			if(%j == %wCount - 2)
			{
				%lists = %lists SPC %type;
			}
		}
		%w = getString(%s,%listSep,%j);
		%s = setString(%s,%listSep,%j,%w @ %lists);
	}
	return %s;
}

function sortRecords(%s,%comp)
{
	%count = getRecordCount(%s);

	%ss = getRecord(%s,0);
	%sortedCount = 1; 
	for(%i = 1; %i < %count; %i++)
	{
		%v1 = getRecord(%s,%i);
		for(%j = 0; %j < %sortedCount; %j++)
		{
			%v2 = getRecord(%ss,%j);
			%result = eval("return " @ %comp @ ";");
			if(%result $= "") return;
			if(%result)
			{
				%ss = setRecord(%ss,%j,%v1 NL %v2);
				%sortedCount++;
				break;
			}
		}

		if(%j == %sortedCount)
		{
			%ss = %ss NL %v1;
			%sortedCount++;
		}
	}
	return %ss;
}

function sortFields(%s,%comp)
{
	%count = getFieldCount(%s);

	%ss = getField(%s,0);
	%sortedCount = 1; 
	for(%i = 1; %i < %count; %i++)
	{
		%v1 = getField(%s,%i);
		for(%j = 0; %j < %sortedCount; %j++)
		{
			%v2 = getField(%ss,%j);
			%result = eval("return " @ %comp @ ";");
			if(%result $= "") return;
			if(%result)
			{
				%ss = setField(%ss,%j,%v1 TAB %v2);
				%sortedCount++;
				break;
			}
		}

		if(%j == %sortedCount)
		{
			%ss = %ss TAB %v1;
			%sortedCount++;
		}
	}
	return %ss;
}

function sortWords(%s,%comp)
{
	%v1 = 0;
	%v2 = 0;
	%eval = "return " @ %comp @ ";";
	if(eval(%eval) $= "") return;

	%count = getWordCount(%s);

	%ss = getWord(%s,0);
	%sortedCount = 1; 
	for(%i = 1; %i < %count; %i++)
	{
		%v1 = getWord(%s,%i);
		for(%j = 0; %j < %sortedCount; %j++)
		{
			%v2 = getWord(%ss,%j);
			if(eval(%eval))
			{
				%ss = setWord(%ss,%j,%v1 SPC %v2);
				%sortedCount++;
				break;
			}
		}

		if(%j == %sortedCount)
		{
			%ss = %ss SPC %v1;
			%sortedCount++;
		}
	}
	return %ss;
}
