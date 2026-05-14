---
name: probuilder-api
description: ProBuilder 6.0.8 correct static API for editor mesh generation — which methods to use and pitfalls to avoid
metadata:
  type: reference
---

## ProBuilder 6.0.8 — Verified API for Editor Mesh Generation

**Package hash:** `com.unity.probuilder@2e7e30b1490d`

### Box / Cube

```csharp
ProBuilderMesh pb = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(w, h, d));
```

- `PivotLocation.Center`: transform.position = geometric centre of the mesh.
- `PivotLocation.FirstVertex`: transform.position = a corner vertex (use for items that must sit on the ground at a known corner).
- Always call `pb.ToMesh(); pb.Refresh();` after parenting/moving.

### Stairs

```csharp
ProBuilderMesh pb = ShapeGenerator.GenerateStair(
    PivotLocation.FirstVertex,
    new Vector3(width, height, depth),
    stepCount,
    buildSides);
```

- `size.x` = width of staircase (across steps)
- `size.y` = total rise
- `size.z` = total run (depth)
- `FirstVertex` places pivot at bottom-front corner. Mesh occupies local [0..x, 0..y, 0..z].
- To orient stairs in world X direction: set `size.x = STAIR_DEPTH, size.z = STAIR_WIDTH`.
- Mirror: rotate 180° around Y.

### DO NOT USE for sized shapes

- `ShapeFactory.Instantiate<Cube>()` — returns default unit cube; `ProBuilderShape.UpdateShape()` and `stepsCount` are `internal` in v6. Cannot resize from outside the package without reflection.
- `ShapeGenerator.GenerateBox` — does not exist in v6. The method is `GenerateCube`.

### Other useful generators

```csharp
ShapeGenerator.GeneratePlane(PivotLocation.Center, width, height, widthCuts, heightCuts, Axis.Up)
ShapeGenerator.GenerateCylinder(PivotLocation.Center, axisCuts, radius, height, heightCuts)
```

### After any mesh operation

```csharp
pb.ToMesh();
pb.Refresh();
```

**Why:** `ToMesh()` rebuilds the underlying `UnityEngine.Mesh`. `Refresh()` recalculates normals, tangents, UVs, and collider bounds.
