# Architecture of the plugin

The plugin is written in C# with two parts: editor and runtime.

The procedure goes as follows:
1. Analysis of the `.inputaction` file, done by `Parser.cs` and `Analysis.cs` in the editor namespace
2. User selection of parts to keep
3. Generation of the prefab, done by `PrefabCreator.cs` in the editor namespace

## Editor
In the editor, the plugin reads a `.inputaction` file from the Unity projects and produces a graphical user window for the user. This window contains the info extracted from the file and the user can specify which parts to keep in the final prefab.

## Runtime 
At runtime, the plugin responds to runtime inputs to the `Input Rebinder Canvas`. 