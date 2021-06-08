# Collision Hull Generator - Convex Decomposition for Unity

## Usage

1. In the Unity editor, click the "Window->Convex Hull Generator" menu item.
2. The Convex Hull Generator window will appear. Resize and/or dock the window
   so you can see all of the settings.
3. Select an input GameObject to generate convex hulls for. The GameObject must
   have a MeshRenderer with a mesh assigned.
4. Choose the generation options. If you are creating convex hulls for physics
   you probably want to generate colliders. Creating renderers for the convex
   hulls is usually used for visual debugging. Creating an asset is useful
   for exporting the convex hull mesh data outside the current Unity project.
5. If you are generating colliders, optionally select a physic material to apply
   to all generated convex hulls.
6. Choose the maximum number of convex hulls and vertices per hull to generate.
   This is a tradeoff between accuracy and runtime performance.
7. If you are an advanced user or curious, open the Advanced dropdown and browse
   the additional settings. Hover over setting names for tooltips with
   additional information. Some of these settings require knowledge of the
   underlying V-HACD 2.0 algorithm to fully understand, but you can
   trial-and-error to discover what the performance tradeoffs are and what
   produces the best results for certain input meshes.
8. Press "Bake"! This will generate a child GameObject called "BakedColliders"
   which contains one child GameObject for each convex hull. You can modify
   these as you please to apply any refinements.
9. Your GameObject should be ready for collision detection. Feel free to attach
   a RigidBody component to the parent object or any other customization you
   please.

## Release Notes

### 1.1

- MacOS and Linux editor support
- Active GameObject now tracks current selection in the editor, mirroring the
  behavior of the Inspector panel
- Better V-HACD default settings for common Unity use cases: fewer convex hulls,
  higher resolution, normalize meshes
- Improvements to asset generation, now compatible with prefabs

### 1.0

- Initial release, Win64 editor support

## More Information

Website: https://catmoneyrecords.com/
Support: john@catmoneyrecords.com
