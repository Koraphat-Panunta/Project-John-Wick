---
name: project-level-design-patterns
description: Recurring TPS level design patterns discovered through research — cover density, arena sizing, spawn rules, red flags
metadata:
  type: project
---

# TPS Level Design Pattern Library — ApexPredator Project

Accumulated from research sessions. Apply these across all blockout work.

## Cover Density Patterns
- 6v6 maps: 8–12 cover objects per engagement zone
- 2v2 Gunfight maps: 4–6 cover objects per engagement zone, 12–13 total map maximum
- Rule of thumb: 1 object per 18–24 sqm of floor space (for small TPS arenas)

## Low Cover Preference
- "Low cover is better than tall cover" — confirmed by both Level Design Book (Uncharted 4 standards) and Gears of War design team
- Tall cover limits firing positions, causes camping, reduces player agency
- For TPS with cover-snap system: low cover gives more animation surface options

## Arena Size Patterns
- Gunfight 2v2: 28–32m long x 18–22m wide
- The Level Design Book standard: 8–12 seconds spawn-to-objective travel time
- At 6–7 m/s sprint: 8 sec = ~50m maximum for any spawn-to-objective path

## Spawn Safety Rules (2v2 specifically)
- Minimum 22–26m spawn-to-spawn separation
- First contact occurs at 8–10m from spawn after 3–4 seconds of movement
- Spawn must provide 2 escape vectors (forward + lateral)
- Spawn cover must block LOS to opposing spawn on all angles including prone

## Verticality Rules
- Elevated positions should be high-risk/high-reward — no full cover at the top
- Vertical positions dominate within their effective weapon range only
- Bridges and towers: half cover (0.9m parapet) on edges, no hard walls
- Center bridge position should be a commitment, not a safe hold

## Red Flags That Appear Repeatedly
1. Single access point to vertical position = structural imbalance
2. Cover facing all one direction = no flanking, only standoffs
3. Full cover at elevated positions = camping exploit
4. Spawn with direct sightline to center = spawn kills
5. Dead ends in timed round modes = player frustration with no agency

## Best Reference Games by Level Type
- 2v2 arena with bridge: CoD MW 2019 Docks (closest structural match)
- Freestanding elevated platform: CoD MW 2019 King
- Vertical dominance design: CoD MW 2019 Rust (tower), Returnal (height as skill gate)
- Multi-floor interior TPS: The Last of Us Part II hotel level
- Cover-snap system level design: Gears of War series (any encounter map)
