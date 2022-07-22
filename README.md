# AdaptiveAudioOutput

This is a small C# program that keeps track of a configured set of programs to change the default audio output when they launch. I hacked this together in a night using C# because it had the easiest interface to Windows Management Instrumentation (WMI) which gives async notifications when a particular process starts.

# Use Cases
My use cases are:
- Change audio output to my wireless headset when a game is launched
- Change audio output to my wireless headset when a meeting software is launched

# Configuration
There is one file you need to configure which maps the process name to the sound device. This file is called `process_mapping.json`. Here's an example:
```json
{
    // This is the default device when no procresses are started. __default__ is a special value that tells the program to get the default windows audio input and use that as default
    "defaultDevice": "__default__",
    // "defaultDevice": "Realtek(R) Audio\Device\Speakers\Render",
    "processes": {
        "r5apex.exe": "2- SteelSeries Arctis 1 Wireless\\Device\\Speakers\\Render",
        "Discord.exe": "2- SteelSeries Arctis 1 Wireless\\Device\\Speakers\\Render",
        "notepad.exe": "2- SteelSeries Arctis 1 Wireless\\Device\\Speakers\\Render"
    }
}
```

You must set the:
- Defaut Device
  - Using a special value like `__default__` or
  - The actual string to identify the output.
- Processes
  - This maps the process name to the device to change to.


## Getting Device names
On Startup the program prints valid device names and their device ids. All you'll need to do is copy these device ids and paste it
into the `process_mapping.json` file under a process.exe

## Getting exe names
- This is the file name of the program being run.

# The need for Admin Privileges 
This program needs admin privileges because windows doesn't allow normal user programs to read WMI information.
I read WMI information because I need to detect procresses that start up whenever they do.
Hence this program will be able to read the programs that start on your computer while using it but if it isn't in the 
mappings config file it will ignore it.

For the programmers out there you can find where I use this info at: [ProgramWatcher.cs](./AdaptiveAudioOutput/ProgramWatcher.cs)

# Dependencies
All Dependencies are included (thanks to their free licenses).
- [Newtonsoft.JSON](https://www.newtonsoft.com/json)
- [SoundVolumeView by NirSoft](https://www.nirsoft.net/utils/sound_volume_view.html)
  - Honestly amazing utilities

## SoundVolumeView 
Special shout out to SoundVolumeView for the functionality to actually change the default audio out as well as get the default audio device. Without this software it wouldn't exist.
