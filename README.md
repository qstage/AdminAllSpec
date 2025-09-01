# AdminAllSpec

!rcon jointeam 1

or

!rcon sm_spectate

no specific command to become a spectator —

the plugin takes care of this automatically. when you are already a spectator and decide to watch other 

players — it allows you to see everyone, not just those on your team or alive. 

The plugin will recognize the player as an authorized admin. 

      • They will be able to spectate any player, even after dying. 

      • It is not necessary to add another flag like — it already covers everything.

Allows admins to spec both teams with `mp_forcecamera 1`.

Original: https://forums.alliedmods.net/showthread.php?t=182412

## ConVars
**`css_adminallspec_flag`** - Admin flag (default: *@css/generic*)

## Requirments
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/)

## Installation
- Download the newest release from [Releases](https://github.com/qstage/AdminAllSpec/releases)
- Move the `/gamedata` folder to a folder `/counterstrikesharp`
- Make a folder in `/plugins` named `/AdminAllSpec`.
- Put the plugin files in to the new folder.
- Restart your server.
