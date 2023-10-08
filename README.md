# Locomotive Mesh Splitter

This Derail Valley mod is a framework that allows multiple mods to alter a locomotive or train car's mesh. It does the hard work of accessing and altering the meshes.

Right now it only splits up LOD0 of the S282 locomotive mesh, but the S060 is planned at some point. If other locomotives are widely requested I can consider adding them.

Users can install the mod like any other Derail Valley mod - drag and drop it into Unity Mod Manager.

This mod won't do anything on its own. It's a framework to help other mods edit different parts of the S282 at the same time.

## For mod developers

Instead of just one mesh for the S282, with this mod all S282's are spawned with the mesh split into many game objects.
The easiest way to see this is to use the [Runtime Unity Editor](https://github.com/ManlyMarco/RuntimeUnityEditor), which allows you to see how every `GameObject` is structured.

With [Runtime Unity Editor](https://github.com/ManlyMarco/RuntimeUnityEditor) installed:

1. Go near an S282 (or spawn one in if there's not one around).
2. Open the Runtime Unity Editor (By default the shortcut is F12, this conflicts with Steam's screenshot key so you might want to rebind it. I rebound it to F9).
3. Select "Object Browser" in the toolbar on the bottom of the screen.
4. Scroll down to "LocoS282A(Clone)". If there are multiple, the most recently spawned locomotive should be at the top.
5. Expand the tree by double clicking items in the list.

   Instead of one mesh for the S282A body per LOD (`LocoS282A_Body/Static_LOD0/s282_locomotive_body`), there should be many in `LocoS282A_Body/Static_LOD0/s282a_split_body(Clone)`.

6. Click on an item in the Object Browser to edit its position, rotation, scale, etc. You may need to scroll to the right.

With this you can experiment and figure out all the different parts of the mesh. I've tried to give them intuitive names.

**Note:**
The brake shoe meshes have moved to `LocoS282A_Body/MovingParts_LOD0/DriveMechanism L/s282a_brake_shoes(Clone)` and `LocoS282A_Body/MovingParts_LOD0/DriveMechanism R/s282a_brake_shoes(Clone)`.

The headlight is part of the smokebox door, and is in `LocoS282A_Body/Static_LOD0/s282a_split_smokebox_door(Clone)/s282a_headlight`.



## License
Source code is distributed under the MIT license.
See [LICENSE](LICENSE) for more information.
