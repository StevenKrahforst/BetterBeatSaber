# Better Beat Saber

A mostly visual enhancement mod *(depends on if u like RGB and Bloom/Glow, just look at the preview video)*

**Beat Saber Version:** 1.29.1 - latest *(1.36.2 atm)*

**Preview:** https://www.youtube.com/watch?v=O8POX8QZyrE

**Discord:** https://discord.gg/cyVXhNr2Ct

Any questions, ideas, suggestions, bugs etc? Create an Issue on GitHub or write me.

## Features
- Colorizes and synchronizes alot of different stuff like Bombs, Obstacles (Walls), Sliders (Arcs), UI *(mostly the HUD)*, Note Cut Particles, Saber Burn Marks, Note Outlines, Bomb Outlines, ReeSabers Sabers, HSV and more
- Integration for SignalRGB, FPS Counter, PBOT, FC Percentage, Menu Pillars, Custom Notes *(Outlines)*
- Some little Tweaks like Note/Bomb size, hiding the Level and/or Menu Environment and more
- Outlines are configurable (Visibility *(Only for VR or Desktop or Both)*, Width, Glow/Bloom)

> [!NOTE]
> There are a few things which are only configurable from the config file

## Requirements
- BSIPA
- BSML
- SiraUtil

## Compiling

You have to have [.NET Framework v4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework) installed to compile this project.

**Steps:**

1. Clone the repository
2. Open the project in Visual Studio or Jetbrains Rider
3. You need to change the `BeatSaberDirectory` property inside the `Directory.Build.props` and also set the target Beat Saber version there (`BeatSaberVersion`)
4. Build the project