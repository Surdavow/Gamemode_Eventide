// List of required add-ons
%requiredAddOns = "Support_CustomCDN Gamemode_Slayer Event_BrickText Item_Medical Brick_Halloween Server_EnvironmentZones Weapon_Rocket_Launcher Projectile_GravityRocket Weapon_Gun";

// Iterate through the array and check each add-on
for (%i = 0; %i < getWordCount(%requiredAddOns); %i++) 
{
    %addOn = getWord(%requiredAddOns,%i);
    if (ForceRequiredAddOn(%addOn) == $Error::AddOn_NotFound) 
    {
        return error(%addOn @ " is required for Gamemode_Eventide to work");
    }
}

// Execute essential scripts and preferences first
exec("./prefs.cs");
exec("./modules/scripts/module_scripts.cs");

exec("./modules/misc/module_misc.cs");
exec("./modules/bricks/module_bricks.cs");
exec("./modules/items/module_items.cs");
exec("./modules/players/module_players.cs");