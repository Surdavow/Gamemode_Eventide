$Eventide_ReferenceStats = "score canChangeTitle customtitle canChangeTitleColor customtitlecolor canChangeTitleFont customtitlefont canChangeTitleBitmap customtitlebitmap hasEffect0 hasEffect1 hasEffect2 hasEffect3 hasEffect4 hasEffect5 effect";
$Eventide_ReferenceStatsPath = "config/server/eventide/playerstats/";	

function Eventide_storeEventideStats(%client)
{
	if(!isObject(%client)) return;

	%file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt";	
	%readfile = new fileObject();
	%readfile.openForWrite(%file);

	for(%rat = 0; %rat < getWordCount($Eventide_ReferenceStats); %rat++)
	if(%client.getField(getWord($Eventide_ReferenceStats,%rat)) !$= "") %readfile.writeLine(%client.getField(getWord($Eventide_ReferenceStats,%rat)));
	else %readfile.writeLine("0");

	%readfile.close();
	%readfile.delete();	
}

function Eventide_loadEventideStats(%client)
{
	if(!isObject(%client) || !isFile(%file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt")) return;

	%readfile = new fileObject();
	%readfile.openForRead(%file);

	for(%apc = 0; %apc < getWordCount($Eventide_ReferenceStats); %apc++)
	%client.setField(getWord($Eventide_ReferenceStats,%apc),%readfile.readLine());	

	%readfile.close();
	%readfile.delete();
}

package Eventide_StatsLogger
{
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
activatePackage(Eventide_StatsLogger);