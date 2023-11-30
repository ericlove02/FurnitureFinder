To create a furniture prefab, start by getting the model file for the object. This might be a 3ds or dae file.
If an object does not have color or has broken meshes/textures, skip for now. We will need to replace later.
Put the file into the Objects folder by dragging it in. Then drag the object from the Objects into any scenes hierarchy.
Drag the game object from the hierarchy into the Prefabs > Furniture folder. This will create a prefab file.
Delete the game object from the hierarchy, then double click on the prefab file from the folder, this will open its hierarchy.

With the prefab hierarchy open, select the Parent.
In the inspector, click add component and configure each of the following:
- Rigidbody, then UNCHECK Use Gravity, and CHECK Freeze Position for XYZ and Freeze Rotation for XYZ
- Collision Handler script
- Box Collider. User the Size and Center XYZ settings to encapsulate the object, this will be the area that the user can click

Next select all of the children of the prefab. You can ctrl + A in the hierarchy then deselect the parent:
- add Outline (cakeslice) component
If there is no children, add the outline to the parent.

Once a prefab is done, go to the ARScene and select the XR Origin from the hierarchy. 
In the object handler component, drop down the Furniture Prefabs array.
Drag your new prefab to the index that matches the FUR_ID - 1.

We will also need the sprites of all the objects. Get the pngs of the objects and put them in the Sprites folder.
Drag them into the Furniture Sprites array to the index that matches FUR_ID - 1.
