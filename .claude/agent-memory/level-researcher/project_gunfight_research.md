---
name: project-gunfight-research
description: CoD MW 2019 Gunfight 2v2 map design research — dimensions, cover rules, bridge/stair specs, sightline patterns
metadata:
  type: project
---

# CoD MW 2019 Gunfight 2v2 Research — Key Findings

Research completed 2026-05-14 for the ApexPredator TPS project (Project-John-Wick).

**Why:** User needs a ProBuilder blockout with ground floor + central elevated bridge + stairs, calibrated to CoD Gunfight scale.
**How to apply:** Use these as authoritative baselines for any future Gunfight-style arena blockout work on this project.

## Verified Unit Conversion (CoD Series)
- CoD unit = 1 inch = 0.0254m
- Standing cover: 48 units = 1.22m
- Crouch cover: 36 units = 0.91m
- One-story wall (ceiling): 112 units = 2.84m (~3.0m with slab)
- SMG effective range: 512 units = ~13m
- Rifle effective range: 1024 units = ~26m
- Stair step: 6 rise / 8 deep units = 0.15m rise / 0.20m run

## Map Dimensions (Unity meters)
- Total length (spawn axis): 28–32m
- Total width: 18–22m
- Bridge height above ground: 3.5–4.0m
- Bridge deck width: 3.0–4.0m
- Bridge length: 10–14m (center third of map only)
- Stair rise: 3.5–4.0m, run: 4.5–6.0m, width: 2.5m minimum

## Best Reference Maps
- **Docks**: two buildings + bridge connecting them — closest to the requested layout
- **King**: freestanding raised platform with stairs at each end
- **Rust**: vertical power position design, under-structure CQB zone
- **Atrium**: symmetrical spawn-mirrored design, minimal cover philosophy

## Cover Rules (2v2)
- Total objects: 12–13 max for a ~600sqm map
- NO full cover on bridge surface — half cover (0.9m) only
- Spawn cover: 1 full (1.3m) + 1 half (0.9m) per team
- Flank lanes: 2 half-cover objects each
- Center floor: 1 full + 1–2 half
- Never align all covers parallel to spawn axis — rotate 1/3 of pieces 30–45 degrees

## Spawn Rules
- Spawn-to-spawn: 22–26m minimum
- Sprint time to center: 3.1–3.7 seconds at 7 m/s — first contact at ~8–10m from each spawn
- Spawn must have 2 exit vectors (forward + lateral)
- OT flag always at center floor under the bridge

## Top Red Flags
1. Bridge with full-height cover = camping bunker
2. Single staircase = one-sided round control
3. Stair width < 2.0m = bottleneck exploit
4. Spawn with direct center sightline = spawn kills
5. Bridge centered off-axis = structural imbalance every round

See [[project-john-wick-level-design]] for broader project context.
