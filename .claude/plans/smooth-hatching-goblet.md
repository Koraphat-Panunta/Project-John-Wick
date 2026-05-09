# Enemy System Map — Reference Document

## Context

The user asked me to read every class involved with the Enemy object and explain it, as a precursor to follow-up questions. No concrete change has been requested yet. This file captures the system map for reference; once a specific change is identified through Q&A, a separate plan will be drafted.

The user's working directory is the worktree `E:\Unity\Project-ApexPredator\Project-John-Wick\.claude\worktrees\serene-heyrovsky-706fa9`. The `Enemy_Driver` / `Enemy_Soldier_Body` split exists in the ResearchingScene hierarchy but is **not yet supported by any C# code** in this branch — that gap is one of the topics likely to come up.

## Top-level architecture

```
Character (abstract MonoBehaviour, Assets/Character/Shared/Script/Character.cs)
 └─ SubjectEnemy (abstract; observer pattern, Assets/Character/Enemy/Script/SubjectEnemy.cs)
     └─ Enemy (12 partial files in Assets/Character/Enemy/Script/CoreEnemy/)
```

The `Enemy` GameObject hosts:
- `EnemyStateManagerNode` — state machine root
- `EnemyWeaponManuver` — weapon action tree (mirrors player)
- `EnemyMovement` (subclass of `MovementCompoent`) — NavMesh + physics
- `MotionControlManager` — toggles code-driven vs ragdoll motion
- `FieldOfView` — vision cone sensor
- `EnemyDecision` → `EnemyRoleBasedDecision` → role node managers (Chaser, Overwatch)
- HP / posture / stagger gauges (partial classes implement `IPostureAble`, `IStaggerAble`)
- Ragdoll surface via `IRagdollAble` (root, hipsBone, bones[], rigidbodies[])

## The 4 logical layers

1. **AI Decision** — `EnemyRoleBasedDecision` picks role → role node manager runs `EnemyActionNodeManager` tactics (`ApprouchingTargetEnemyActionNodeLeaf`, `DisarmTargetWeaponEnemyActionNodeLeaf`, `GuardingEnemyActionNodeLeaf`). Outputs *commands* only.
2. **Blackboard** (`EnemyBlackBoard`, partial class) — fields like `moveInputVelocity_WorldCommand`, `lookRotationCommand`, `isSprintCommand`, `_triggerDodge`, `_triggerHitedGunFu`, `_triggerGunFu`, `_isPainTrigger`, `isDead`. AI writes; state machine reads.
3. **State machine** (`EnemyStateManagerNode`) — selectors + ~25 leaves:
   - `enemyStanceSelector` → `standSelector` (idle/move/sprint), `crouchSelector` (idle/move)
   - `gunFuSelector` → all "Got…" leaves
   - `zeroPostureSelector` → `FallDown_EnemyState_NodeLeaf`, `GetUpStateNodeLeaf`
   - `painStateNodeLeaf`, `EnemyDodgeRollStateNodeLeaf`, `EnemyDeadStateNode`
4. **Components / feedback** — damage, posture, ragdoll, weapon system. Partial classes glue these into `Enemy`.

## Player ↔ Enemy interaction pairing ("Got" pattern)

Player offensive node sets `enemy._triggerHitedGunFu = true` and `enemy.curAttackerGunFuNode = this`. Enemy's mirrored `Got…` leaf precondition reads `curAttackerGunFuNode is XYZ_NodeLeaf`.

| Player initiates | Enemy responds | Sync mechanism |
|---|---|---|
| `GunFuExecute_Single_…NodeLeaf` (×6) | `GotGunFuExecuteNodeLeaf` | `SubjectAnimationInteract` pair |
| `GunFuExecute_OnGround_…NodeLeaf` | `GotExecuteOnGround_NodeLeaf` | Same + ragdoll orientation match |
| `GunFuHit1/2/3_NodeLeaf` | `GotGunFuHitNodeLeaf` (matched by `_stateName`) | Direct state name match |
| `WeaponDisarm_GunFuInteraction_NodeLeaf` | `WeaponGotDisarmedGunFuGotInteractNodeLeaf` | Weapon type match |
| `HumanShield_GunFu_NodeLeaf` | `HumandShield_GotInteract_NodeLeaf` | `SubjectAnimationInteract` |
| `HumanShieldExit_GunFu_NodeLeaf` | `HumanShield_Exit_GotInteract_NodeLeaf` | Same |
| `RestrainGunFuStateNodeLeaf` | `GotRestrictNodeLeaf` | `SubjectAnimationInteract` |
| Enemy initiates `EnemySpinKickGunFuNodeLeaf` | Player → `GunFuHitNodeLeaf("DodgeSpinKick")` | Reverse direction of same pattern |

## Known gaps / oddities

- **No code support for the `Enemy_Driver` / `Enemy_Soldier_Body` split.** `EnemyDriverSetUp.cs` is 13 lines holding bone refs only.
- **No `EnemyAnimationNodeManager`** parallel to the player's. Enemy animations are `animator.CrossFade()` calls inside individual leaves' `Enter()`.
- **`staggerGauge` is aliased to HP** in `EnemyImplementIStaggerAble` — design intent unclear.
- **`AIAgent` is referenced by `Enemy.cs`** but the source file wasn't located in scope; may live in a shared lib.
- **No animation callback path** from a body GameObject's Animator to the Enemy logic GameObject. This is the "OnAnimatorMove forwarder" problem already discussed.

## Next steps

User is in Q&A mode. When a specific change emerges (split refactor, GunFu pairing fix, ragdoll improvement, etc.), draft a focused implementation plan as a separate document or replace this file's content.
