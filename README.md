# DisableExtensions
Disable extensions upon closing ArcMap 10.4+ for the purpose of automatically releasing extension licenses when the user is no longer using ArcMap, so that others using the same license manager may be able to use the extensions.

- Disable.cs = C# code used to generate the dll.
- DisableExtensions.dll = compiled dll that can be used by ArcMap 10.4+ users

## Installation
*Prerequisite: You should have [ArcMap](https://desktop.arcgis.com/en/arcmap) 10.4+ installed.  This dll has been tested on 10.4.1 and 10.7.1 ArcMap.*

1. Copy the dll to your desired location on your local machine.
2. Open up an elevated command prompt (Run as administrator), then run: `"C:\Program Files (x86)\Common Files\ArcGIS\bin\ESRIRegAsm" "C:\YourFolder\DisableExtensions.dll" /p:desktop`

- To unregister, run `"C:\Program Files (x86)\Common Files\ArcGIS\bin\ESRIRegAsm" "C:\YourFolder\DisableExtensions.dll" /p:desktop /u`
