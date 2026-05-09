# Plan: Animation Orchestrator — co-locate animation + constraint registration

## Context

The character's visual layer runs as **two independent MonoBehaviour managers** on both Player and Enemy:

- **AnimationManager** ([PlayerAnimationManager.cs](Assets/Character/Player/Script/Animation/PlayerAnimationManager/PlayerAnimationManager.cs), [EnemyAnimationManager.cs](Assets/Character/Enemy/Script/EnemyAnimation/AnomationNodeTree/EnemyAnimationManager.cs)) — discrete state machine via `NodeSelector`. One active state at a time (sprint, crouch, dodge, melee attack, hit, etc.).
- **ConstrainAnimationManager** ([PlayerConstrainAnimationManager.cs](Assets/Character/Player/Script/Animation/PlayerConstraintAnimation/CoreScript/PlayerConstrainAnimationManager.cs), [EnemyConstrainAnimationNodeManager.cs](Assets/Character/Enemy/Script/EnemyAnimation/ProceduralAnimate/EnemyConstrainAnimationNodeManager.cs)) — five parallel `NodeComponentManager` instances (body / right hand / left hand / legs / head). All-of-many weighted overlays (lookat, IK, lean, head look).

**Pain when adding a feature** (e.g., parry visuals):
1. Add a `PlayAnimationNodeLeaf` to `PlayerAnimationManager`'s state tree.
2. Separately add an `ArmIKConstraintRefTransformNodeLeaf` (or similar) to the right-hand region of `PlayerConstrainAnimationManager`.
3. Mirror on Enemy side in two more files.
4. Each manager has its own `OnNotify<T>` observer, so reaction wiring is duplicated.
5. People forget one half. There is no single contract that says "this feature owns these animation + constraint nodes."

**What is NOT actually broken:**
- Update/FixedUpdate cadence — both managers self-drive via Unity. Wrapping them in a custom Update loop adds complexity for no win.
- The shape mismatch — state machine vs parallel weighted overlay are genuinely different paradigms. **Do not collapse them into one node tree.** That trades one well-understood split for one god-class.

## Design (locked-in)

- **Orchestrator wrapper** per character. Owns references to both sub-managers; exposes a single registration surface and observer fan-out. Sub-managers keep their distinct shapes.
- **Shared base + per-character override.** Generic base class encapsulates lifecycle and feature-registration; Player/Enemy subclasses bind concrete sub-manager types.

## Recommended approach

### 1. New `IAnimationFeature` contract

A feature is a self-contained bundle of "I need an animation state plus zero-or-more constraint nodes." File:

`Assets/Character/Shared/Script/Animation/IAnimationFeature.cs`

```csharp
public interface IAnimationFeature
{
    string _featureName { get; }
    void RegisterAnimationStates(IAnimationStateRegistry animationRegistry);
    void RegisterConstraintStates(IConstraintRegistry constraintRegistry);
    void OnObservedNotify<T>(object source, T payload);
}
```

Plus thin registry interfaces so a feature can add nodes without seeing the manager internals:

`IAnimationStateRegistry` exposes `AddState(NodeLeaf, parentSelector)`-style methods that map onto current animation manager calls.
`IConstraintRegistry` exposes `AddBodyConstraint`, `AddRightHandConstraint`, `AddLeftHandConstraint`, `AddLegsConstraint`, `AddHeadConstraint` — one method per existing region in the constraint manager.

Both registries are implemented as adapters wrapping the existing managers — no internal manager refactor.

### 2. Shared orchestrator base

`Assets/Character/Shared/Script/Animation/AnimationOrchestratorBase.cs`

```csharp
public abstract class AnimationOrchestratorBase<TAnim, TConstraint> : MonoBehaviour, IInitializedAble
    where TAnim : MonoBehaviour, IInitializedAble
    where TConstraint : MonoBehaviour, IInitializedAble
{
    [SerializeField] protected TAnim animationManager;
    [SerializeField] protected TConstraint constraintManager;

    private readonly List<IAnimationFeature> features = new();
    protected abstract IAnimationStateRegistry CreateAnimationRegistry();
    protected abstract IConstraintRegistry CreateConstraintRegistry();
    protected abstract void SubscribeObserverFanOut();

    public TAnim _animationManager => animationManager;
    public TConstraint _constraintManager => constraintManager;

    public virtual void Initialized()
    {
        animationManager.Initialized();
        constraintManager.Initialized();

        var animRegistry = CreateAnimationRegistry();
        var conRegistry = CreateConstraintRegistry();

        foreach (var f in features)
        {
            f.RegisterAnimationStates(animRegistry);
            f.RegisterConstraintStates(conRegistry);
        }

        SubscribeObserverFanOut();
    }

    public void RegisterFeature(IAnimationFeature feature) => features.Add(feature);

    protected void DispatchToFeatures<T>(object source, T payload)
    {
        for (int i = 0; i < features.Count; i++)
            features[i].OnObservedNotify(source, payload);
    }
}
```

### 3. Player + Enemy concrete orchestrators

`PlayerAnimationOrchestrator : AnimationOrchestratorBase<PlayerAnimationManager, PlayerConstrainAnimationManager>` — implements `SubscribeObserverFanOut` by registering itself as `IObserverPlayer` on `Player`, then routes `OnNotify<T>` into `DispatchToFeatures` plus the existing manager-side handlers (kept intact for now).

`EnemyAnimationOrchestrator : AnimationOrchestratorBase<EnemyAnimationManager, EnemyConstrainAnimationNodeManager>` — same pattern with `IObserverEnemy`.

### 4. Migration — incremental, no big-bang

Phase A (foundation, no behavior change):
- Add the four new files (interface, base, two subclasses, two registry adapters).
- Attach orchestrator MonoBehaviour to Player and Enemy GameObjects. Wire its serialized fields to the existing animation + constraint manager components.
- Replace `playerAnimationManager.Initialized()` + `playerConstrainAnimationManager.Initialized()` calls in [Player.cs] / [BodySetup.cs] with a single `playerAnimationOrchestrator.Initialized()`.
- All existing in-manager state authoring stays put; the feature list is empty. **System runs identically.**

Phase B (opt-in feature extraction):
- When adding the *next* feature (e.g., parry visuals), author it as an `IAnimationFeature` and `RegisterFeature(...)` on the orchestrator instead of editing two managers.
- Existing features can be migrated lazily, one at a time, when next touched. No mass refactor required.

## Files (revised — Phase A is purely additive)

**New (4):**
- `Assets/Character/Shared/Script/Animation/IAnimationFeature.cs`
- `Assets/Character/Shared/Script/Animation/AnimationOrchestratorBase.cs`
- `Assets/Character/Player/Script/Animation/PlayerAnimationOrchestrator.cs`
- `Assets/Character/Enemy/Script/EnemyAnimation/EnemyAnimationOrchestrator.cs`

**Modified:** none. Orchestrator self-registers as observer in its own `Start()`. Existing `Initialized()` flows stay untouched.

Drop the separate registry interfaces — features access managers directly via `orchestrator._animationManager` / `orchestrator._constraintManager`.

**Untouched in Phase A:**
- All node trees inside `PlayerAnimationManager`, `EnemyAnimationManager`, `PlayerConstrainAnimationManager`, `EnemyConstrainAnimationNodeManager`.
- Update/FixedUpdate of the sub-managers (Unity continues to drive them).
- All existing observer logic in the sub-managers.

## Verification

1. Phase A: open scene, run, confirm Player and Enemy animate identically — no visual change. Console should show one orchestrator `Initialized` log instead of two manager logs.
2. Confirm `playerAnimationManager.Initialized()` is no longer called from `Player.cs` directly (only via orchestrator). Same for Enemy via `BodySetup.cs`.
3. Phase B test: write a tiny `DebugParryAnimationFeature : IAnimationFeature` that registers a no-op state and a low-weight body constraint, register it on `PlayerAnimationOrchestrator`. Confirm both registrations land via Debug.Log on Enter of each.

## Trade-offs

**Why this and not a full unified node tree:**
- A single tree forcing both exclusive (state) and parallel (constraint) semantics requires a hybrid node base that supports both. That's a new abstraction layer with subtle bugs around weight blending vs state transitions. The cost dwarfs the gain.

**Why not just a facade method:**
- A `Player.UpdateAnimation()` that calls both managers hides the dual call but doesn't change the feature-add workflow. People still edit two managers.

**Cost of this plan:**
- Five small new files + two existing-file edits in Phase A. No risk to current animations.
- New mental model — the team has to internalize "features go through orchestrator now." Worth it because Phase B is opt-in.

## Critical files

- New: `Assets/Character/Shared/Script/Animation/AnimationOrchestratorBase.cs`
- New: `Assets/Character/Shared/Script/Animation/IAnimationFeature.cs`
- New: `Assets/Character/Player/Script/Animation/PlayerAnimationOrchestrator.cs`
- New: `Assets/Character/Enemy/Script/EnemyAnimation/EnemyAnimationOrchestrator.cs`
- Modified: `Player.cs` (init call swap)
- Modified: [BodySetup.cs](Assets/Character/Enemy/Script/AutoSetupBodyPart/BodySetup.cs) (init call swap)
- Reference (untouched): [PlayerAnimationManager.cs](Assets/Character/Player/Script/Animation/PlayerAnimationManager/PlayerAnimationManager.cs), [PlayerConstrainAnimationManager.cs](Assets/Character/Player/Script/Animation/PlayerConstraintAnimation/CoreScript/PlayerConstrainAnimationManager.cs), [EnemyAnimationManager.cs](Assets/Character/Enemy/Script/EnemyAnimation/AnomationNodeTree/EnemyAnimationManager.cs), [EnemyConstrainAnimationNodeManager.cs](Assets/Character/Enemy/Script/EnemyAnimation/ProceduralAnimate/EnemyConstrainAnimationNodeManager.cs)
