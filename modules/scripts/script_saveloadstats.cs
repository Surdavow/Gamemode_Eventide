$Eventide_ReferenceStats = "score effect customtitle customtitlebitmap customtitlefont";

for (%i = 0; %i < getWordCount($ShopEffectList); %i++) $Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasEffect" @ %i;
for (%j = 0; %j < getWordCount($ShopInstrumentList); %j++) $Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasInstrument" @ %j;
for (%h = 0; %h < getWordCount($ShopTitlesList); %h++) $Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasTitleAccess" @ %h;

$Eventide_ReferenceStatsPath = "config/server/eventide/playerstats/";	

function Eventide_storeEventideStats(%client)
{
	if(!isObject(%client)) return;

	%file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt";	
	%readfile = new fileObject();
	%readfile.openForWrite(%file);

	for(%rat = 0; %rat < getWordCount($Eventide_ReferenceStats); %rat++)
	if(%client.getField(getWord($Eventide_ReferenceStats,%rat)) !$= "") %readfile.writeLine(%client.getField(getWord($Eventide_ReferenceStats,%rat)));
	else %readfile.writeLine("");

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
		if(%line !$= "") %client.setField(getWord($Eventide_ReferenceStats,%apc),%line);	
	}

	%readfile.close();
	%readfile.delete();
}

package Eventide_StatsLogger
{
	function GameConnection::onConnect(%client)
	{
		parent::onConnect(%client);
		schedule(100,0,Eventide_loadEventideStats,%client);
	}

	function GameConnection::onClientEnterGame(%client)
	{
		parent::onClientEnterGame(%client);
		Eventide_loadEventideStats(%client);		
	}	

	function GameConnection::onClientLeaveGame(%client)
	{
		parent::onClientLeaveGame(%client);
		Eventide_storeEventideStats(%client);		
	}
};
if(isPackage(Eventide_StatsLogger)) deactivatePackage(Eventide_StatsLogger);
activatePackage(Eventide_StatsLogger);