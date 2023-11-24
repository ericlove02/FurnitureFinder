The prefabs in this folder all have the collision handler, rigidbody, and box collider added to them, but are still missing two things:
- size the box collider around the object in the parent of the object
- add the outline (cakeslice) component to all of the children that have meshes.
-- this can be easily done be ctrl + A in the hierarchy the deselecting the parent, then adding the component.

If an object does not have color or has broken meshes/textures, skip for now. We will need to replace later.