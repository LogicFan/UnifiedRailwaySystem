# Unified Railway System

This is a mod for Cities: Skylines. The goal of this mod is let Train, Metro and Tram share the railway.

Here is the current demo.

![first demo](demo.PNG)

Warning: This is still an in-development mod.

# Code Structure

All source code is under UnifiedRailwaySystem folder.

- URSMod.cs: The entry point of the mod
- URSUtil.cs: All necessary utility for this mod.
- Patch/URSPatch.cs: The entry point of patches.
- Patch/*.cs: All the patch files, each method patch has comment about the usage and relate patches.
- Track/URSTrack.cs: The entry point of all change made for tracks.
- Track/*.cs: All the track-changing related code.
- Vehicle/Vehilce.cs: The entry point of all change made for vehicles.
- Vehicle/*.cs: All the vehicle-chaning related code.
- Building/URSBuilding.cs: The entry point of all chage made for buildings.
- Building/*.cs: All the building-changing related code.