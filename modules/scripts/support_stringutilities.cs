$colorNames = "red burgundy darkorange trueorange lightpink pink magenta violet purple darkpurple blue darkblue lightblue cyan chartreusegreen limegreen mint truegreen darkgreen yellow brown chocolate gray lightgray darkgray lightergray white black pitchblack";
$colorValues["red"] = "1 0 0 1";
$colorValues["burgundy"] = "0.59 0 0.14 1";
$colorValues["darkorange"] = "0.9 0.34 0.08 1";
$colorValues["trueorange"] = "1 0.5 0 1";
$colorValues["lightpink"] = "1 0.75 0.8 1";
$colorValues["pink"] = "1 0.5 1 1";
$colorValues["magenta"] = "1 0 1 1";
$colorValues["violet"] = "0.5 0 1 1";
$colorValues["purple"] = "0.5 0 0.5 1";
$colorValues["darkpurple"] = "0.243 0.093 1 1";
$colorValues["blue"] = "0 0 1 1";
$colorValues["darkblue"] = "0.0 0.14 0.33 1";
$colorValues["lightblue"] = "0.11 0.46 0.77 1";
$colorValues["cyan"] = "0 1 1 1";
$colorValues["chartreusegreen"] = "0.5 1 0 1";
$colorValues["limegreen"] = "0.75 1 0 1";
$colorValues["mint"] = "0.5 1 0.65 1";
$colorValues["truegreen"] = "0 1 0 1";
$colorValues["darkgreen"] = "0 0.5 0.25 1";
$colorValues["yellow"] = "1 1 0 1";
$colorValues["brown"] = "0.39 0.2 0 1";
$colorValues["chocolate"] = "0.22 0.07 0 1";
$colorValues["gray"] = "0.5 0.5 0.5 1";
$colorValues["lightgray"] = "0.75 0.75 0.75 1";
$colorValues["darkgray"] = "0.2 0.2 0.2 1";
$colorValues["lightergray"] = "0.89 0.89 0.89 1";
$colorValues["white"] = "1 1 1 1";
$colorValues["black"] = "0.078 0.078 0.078 1";
$colorValues["pitchblack"] = "0 0 0 1";

function getColorName(%RGBA)
{
    %closestColor = "";
    %minDistance = 99999;

    for(%i = 0; %i < getWordCount($colorNames); %i++)
    {
        %currentColor = getWord($colorNames, %i);
        %currentColorRGBA = $colorValues[%currentColor];
        %distance = VectorDist(%RGBA, %currentColorRGBA);

        if(%distance == 0) return %closestColor;
		else if(%distance < %minDistance)
        {
            %minDistance = %distance;
            %closestColor = %currentColor;
        }
    }

    return %closestColor;
}


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