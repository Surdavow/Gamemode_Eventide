//$CustomCDN::CDN_to_clients
//Example: http://cloudf.blockland.us/blobs
//Variable is not set so that another add-on can set it

package CustomCDNServer
{
	function GameConnection::onConnectRequest(%client, %netAddress, %LANname, %blid, %clanPrefix, %clanSuffix, %clientNonce,
		%g, %h, %i, %j, %k, %l, %m, %n, %o, %p)
	{
		%ret = Parent::onConnectRequest(%client, %netAddress, %LANname, %blid, %clanPrefix, %clanSuffix, %clientNonce,
			%g, %h, %i, %j, %k, %l, %m, %n, %o, %p);
			
		
		%client.customCDN = false;

		for(%cnt = 0; %cnt < getLineCount(%h); %cnt++)
		{
			%line = getLine(%h, %cnt);
			if(getField(%line, 0) $= "CustomCDN")
			{
				%client.customCDN = true;
				%client.customCDNUrl = $CustomCDN::CDN_to_clients;
				%client.customCDNversion = getField(%line, 1);
				break;
			}
		}
		return %ret;
	}

	function GameConnection::autoAdminCheck(%client)
	{
		if(%client.customCDN)
		{
			if(%client.customCDNUrl $= "")
				warn("[CustomCDN] $CustomCDN::CDN_to_clients is not set; not giving client custom CDN");
			else
			{
				warn("[CustomCDN] Telling client custom CDN: "@ %client.customCDNUrl);
				commandToClient(%client, 'SetCustomCDN', %client.customCDNUrl);
			}
		}
		else
			warn("[CustomCDN] Client has no custom CDN support");

		return Parent::autoAdminCheck(%client);
	}

};
activatePackage(CustomCDNServer);
