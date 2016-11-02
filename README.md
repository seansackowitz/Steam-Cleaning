# Steam-Cleaning
Observes multiple directories for file changes, and keeps only the most recent files.

### Uses
I wrote this program to help Steam Cloud Sync along with not trying to sync 1GB of Skyrim save files. What this does then is delete all but the most recent files in a directory.

## Setup
The data.xml file is where you will make all your changes for directories and whatnot.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Carpets>
  <Carpet Path="C:\Users\Sean\Desktop\Test" Filter="*.txt" Keep="2" />
</Carpets>
```
| Attribute|Purpose|
| --- | --- |
| **Path** | The directory to look at. |
| **Filter** | What the file names should contain. An asterisk is a wildcard. |
| **Keep** | The number of files to keep. |

To keep more than one directory clean, just add more ```<Carpet />```
