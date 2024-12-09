// Initialize variables to be used later
$Eventide_ReferenceStats = "score customtitle customtitlebitmap customtitlecolor";
$Eventide_ReferenceStatsPath = "config/server/eventide/playerstats/";

// Concatenate conditions for each instrument index
for (%j = 0; %j < getWordCount($ShopInstrumentList); %j++) 
{
	$Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasInstrument" @ %j;
}

// Concatenate conditions for each title setting
for (%h = 0; %h < getWordCount($ShopTitleList); %h++) 
{
	$Eventide_ReferenceStats = $Eventide_ReferenceStats SPC "hasTitleAccess" @ %h;
}

/// Stores the eventide stats for a given client into a file.
/// @param %client The client whose stats are to be stored.</param>
function Client::storeEventideStats(%client)
{
    // Validate if the client object exists
    if(!isObject(%client)) 
    {
        return;
    }

    // Define the file path using the client's BL_ID
    %file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt";	
    %readfile = new fileObject();
    %readfile.openForWrite(%file);

    // Write each reference stat to the file
    for(%rat = 0; %rat < getWordCount($Eventide_ReferenceStats); %rat++) 
    {
        %readfile.writeLine(getWord($Eventide_ReferenceStats,%rat) TAB %client.getField(getWord($Eventide_ReferenceStats,%rat)));
    }
    
    %readfile.close();
    %readfile.delete();	
}

/// Loads the eventide stats for a given client from a file.
/// @param %client The client whose stats are to be loaded.
function Client::loadEventideStats(%client)
{
	// Validate if the client object exists
	if(!isObject(%client) || !isFile(%file = $Eventide_ReferenceStatsPath @ %client.BL_ID @ ".txt")) 
	{
		return;
	}

	// Read each reference stat from the file
	%readfile = new fileObject();
	%readfile.openForRead(%file);

	// Write each reference stat to the client
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
		%client.schedule(1000,loadEventideStats);
	}

	function GameConnection::onClientLeaveGame(%client)
	{
		Parent::onClientLeaveGame(%client);
		%client.storeEventideStats();
	}
};

// In case the package is already activated, deactivate it first before reactivating it
if(isPackage(Eventide_SaveLoadStats)) deactivatePackage(Eventide_SaveLoadStats);
activatePackage(Eventide_SaveLoadStats);