# Project-John-Wick — Claude Memory

## Project Overview

- **Engine:** Unity (C#)
- **Genre:** Third Person Shooter (TPS)

---

## Code Quality Rules

- Always write clean, simple, and readable code
- Avoid over-engineering — solve the problem with the least complexity needed
- Keep classes and methods small and single-responsibility
- Avoid deep inheritance chains; prefer composition over inheritance
- Remove unused variables, methods, and imports before finishing any task
- If you find any system that can be refactored to improve the code, tell me and ask for my approval before doing anything

---

## Before Implementing Any New Feature

1. Search the existing codebase for systems that can be extended or reused
2. Extend existing components if possible — create new ones only when necessary
3. Check if Unity already provides a built-in solution before writing custom code
4. Ask: "Does this already exist somewhere in the project?"

---

## Performance Rules (Unity TPS Specific)

- Avoid allocations in `Update()` — no `new`, no LINQ, no string concatenation per frame
- Use object pooling for bullets, effects, and frequently spawned objects
- Cache component references in `Awake()` or `Start()`, never call `GetComponent()` in `Update()`
- Use Unity's Job System or Burst Compiler for heavy calculations when appropriate
- Prefer `CompareTag()` over `== tag` for tag comparisons
- Use `Physics.SphereCastNonAlloc` / `RaycastNonAlloc` instead of allocating versions

---

## TPS Architecture Guidelines

- Player input should go through an Input System layer, not directly into character logic
- Separate concerns: `PlayerController`, `WeaponSystem`, `CameraController` should be independent
- Use ScriptableObjects for weapon stats, enemy configs, and game balance data
- AI enemies should use a clear state machine: **Idle → Patrol → Chase → Attack → Dead**
- Keep game logic out of MonoBehaviour where possible — use plain C# classes for logic

---

## Naming Conventions

| Type | Convention | Example |
|------|-----------|---------|
| Classes | PascalCase | `PlayerController`, `WeaponSystem` |
| Methods | PascalCase | `FireWeapon()`, `TakeDamage()` |
| Private fields | camelCase with `_` prefix | `_health`, `_moveSpeed` |
| Constants | ALL_CAPS | `MAX_AMMO` |
| Booleans | `is` or `has` prefix | `isGrounded`, `hasAmmo` |

---

## What to Avoid

- **No magic numbers** — use named constants or ScriptableObject values
- **No hardcoded strings** — use constants or tags defined in one place
- **No God classes** — if a class is doing too much, split it
- **No `FindObjectOfType()` at runtime** — use dependency injection or direct references
- **No logic inside Animator or UI scripts** — keep them as thin as possible

---

## Current Work

**Active Feature:** Constrain Animation System test harness (observe constrain nodes in isolation via sliders)
**Status:** Script done — Blocked on Unity MCP (connection revoked) for prefab/scene build + play-mode verification
**Key Files:** `Assets/Character/Player/Script/Animation/PlayerConstraintAnimation/Test/ConstrainAnimationTester.cs`
**Next Step:** Re-enable Unity MCP (Project Settings > AI > Unity MCP), then build a minimal rig prefab (character model + Animator with a single `Locomotion_Stand_Idle` state + RigBuilder/Rig + BodyConstraintManager + RightHand HandArmIKConstraintManager + HumanoidBone + AimPosition child), assign SCRPs (`Body_ADS_Pri`, `rightHand_AimDownSight_PrimaryWeapon_SCRP`, `rifleADSLeaningRotationConstrainScriptableObject`), add `ConstrainAnimationTester`, then enter play mode and sweep sliders.
**Known Issues:** Does NOT run weapon-maneuver nodes; the constrain nodes coupled to IRangeWeaponAdvanceUser/Player are reproduced by subclassing the real base constrain nodes (body) / reproducing math (lean) and a recoil/block layer on the real hand-IK node — no shipping code edited.

---

## Session Notes

*(Use this to log important decisions, design choices, or context that future sessions should know about.)*

- 
