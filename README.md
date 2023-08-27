# Overwatch Version 0.2.0
  Overwatch is a file- and screentime-tracking software for your own.

  It tracks:
  - file changes
  - file deletions
  - file creations 
  - file renamings 
  - file errors
  - your screentime (including AFK)

All private informations from you are saved locally on your machine, rather in a textfile, or into a locally stored database.

# Configuration
 
 Properties:
 - CurrentVersion
 - AfkTimeBeginning
	 - *This is the time from which the actionless time is counted as AFK (in miliseconds)*
 - RunOnStartup
	 - *Should the program start within the boot sequence of the PC?*
 - TrackedPath
	 - *The tracked directory*
 - DisplayInfos
	 - *Should the program display infos at the beginning?*
 - SettingsPath
	 - *Path for the ``Settings.cfg`` file (should the program change automatically for you)*
 - LogPath
	 - *Path for the ``Logs.cfg`` file (should the program change automatically for you)*
 - LogInDB
	 - *Should the infos be logged into the database? Change to false, if you want the infos to be logged into ``logs.cfg``.*

Just set your properties in the `TempSettingsSave.cfg` file, for example:


```
! This is a comment
CurrentVersion = 0.2.0
AfkTimeBeginning = 600000 
RunOnStartup = true
TrackedPath = C:\
DisplayInfos = true
...
LogInDB = true
```
This configuration sets your AFK Limit to 10 minutes, starts the program within the boot sequence of the computer, tracks the C:\\ directory, displays infos at the beginning and logs everything into the SQLite database.

For displaying informations from the database, select `entries.db` as the database and run
```sql
SELECT * FROM SessionInfo
```
(You need a database displaying software like [HeidiSQL](https://github.com/HeidiSQL/HeidiSQL))

## How do I use Overwatch?

Just start ``bin/Debug/Overwatch.exe`` (please leave the other files in the folder)
``TempSettingsSave.cfg``, ``entries.db``, ``Settings.cfg`` and ``Logs.cfg`` are in the same folder