#### A simple marquee screen saver.

* there is a preview window in the screen saver settings 
* you can supply a text file with multiple lines of text. the screen saver will cycle through each line of the text file.
* or set a single line message without the a text file
* set font, font size, and font colour
* change marquee speed
* set simple background colour
* or set a background image

##### Install:

1. download and install .NET 8.0 SDK (https\://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.420-windows-x64-installer)
2. from within marquee3\marquee3\ run:
   1. ```css
      dotnet publish .\marquee3.csproj -c Release -r win-x64 -f net8.0-windows --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o .\publish
      ```
3. rename marquee3\marquee3\publish\marquee3.exe to marquee3.scr
4. copy marquee3.scr to C:\Windows\System32\\
5. now in Windows open Screen Saver Settings and configure as desired.

 

##### i might:

* add an option to randomize the order lines from the text file are displayed
* vertical position is randomized. i might add an option to either set the vertical position or randomize it
* i also might add a compiled .scr file to the releases section in case anyone wants to use this who isn't a developer
