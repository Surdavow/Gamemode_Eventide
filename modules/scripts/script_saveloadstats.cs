$Eventide_ReferenceStats = "score customtitle customtitlebitmap customtitlecolor";

// Concatenate conditions for each instrument index
for (%j = 0; %j < getWordCount($ShopInstrumentList); %j++) {
	$Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasInstrument" @ %j;
}

// Concatenate conditions for each title setting
for (%h = 0; %h < getWordCount($ShopTitleList); %h++) {
	$Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasTitleAccess" @ %h;
}

$Eventide_ReferenceStatsPath = "config/server/eventide/playerstats/";	

function Eventide_storeEventideStats(%client)
{
	if(!isObject(%client)) 
	{
		return;
	}

	%file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt";	
	%readfile = new fileObject();
	%readfile.openForWrite(%file);

	for(%rat = 0; %rat < getWordCount($Eventide_ReferenceStats); %rat++) 
	{
		%readfile.writeLine(getWord($Eventide_ReferenceStats,%rat) TAB %client.getField(getWord($Eventide_ReferenceStats,%rat)));
	}
	

	%readfile.close();
	%readfile.delete();	
}

function Eventide_loadEventideStats(%client)
{
	if(!isObject(%client) || !isFile(%file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt")) 
	{
		return;
	}

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

package Eventide_SaveLoadStats
{
	function gameConnection::autoAdminCheck(%client) 
	{		
		Parent::autoAdminCheck(%client);
		scheduleNoQuota(1000,0,Eventide_loadEventideStats,%client);
	}

	function GameConnection::onClientLeaveGame(%client)
	{
		Parent::onClientLeaveGame(%client);
		Eventide_storeEventideStats(%client);	
	}
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_SaveLoadStats)) deactivatePackage(Eventide_SaveLoadStats);
activatePackage(Eventide_SaveLoadStats);