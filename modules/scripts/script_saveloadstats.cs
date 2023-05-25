$Eventide_ReferenceStats = "score effect customtitle customtitlebitmap customtitlefont customtitlecolor";

for (%i = 0; %i < getWordCount($ShopEffectList); %i++) $Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasEffect" @ %i;
for (%j = 0; %j < getWordCount($ShopInstrumentList); %j++) $Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasInstrument" @ %j;
for (%h = 0; %h < getWordCount($ShopTitleList); %h++) $Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasTitleAccess" @ %h;
for (%h = 0; %h < getWordCount($ShopHatList); %h++) $Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasHat" @ %h;

$Eventide_ReferenceStatsPath = "config/server/eventide/playerstats/";	

function Eventide_storeEventideStats(%client)
{
	if(!isObject(%client)) return;

	%file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt";	
	%readfile = new fileObject();
	%readfile.openForWrite(%file);

	for(%rat = 0; %rat < getWordCount($Eventide_ReferenceStats); %rat++)
	%readfile.writeLine(getWord($Eventide_ReferenceStats,%rat) TAB %client.getField(getWord($Eventide_ReferenceStats,%rat)));

	%readfile.close();
	%readfile.delete();	
}

function Eventide_loadEventideStats(%client)
{
	if(!isObject(%client) || !isFile(%file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt")) return;

	%readfile = new fileObject();
	%readfile.openForRead(%file);

	for(%apc = 0; %apc < getWordCount($Eventide_ReferenceStats); %apc++)
	{
		%line = %readfile.readLine();
		%client.setField(getWord(%line,0),getWord(%line,1));
	}

	%readfile.close();
	%readfile.delete();
}