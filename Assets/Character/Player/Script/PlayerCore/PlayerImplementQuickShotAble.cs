using UnityEngine;

public partial class Player : IQuickShotAble
{

    [SerializeField] public AnimationTriggerEventSCRP quickShotAnimationTriggerEventSCRP;
    [SerializeField] public CastFindingScriptableObject castFindingScriptableObject;
    public IRangeWeaponAdvanceUser _rangeWeaponAdvanceUser => this;

    public INodeManager _nodeManager => this.playerStateNodeManager;

    public bool isTriggerQuickShot;

    public QuickShootRangeWeaponNodeLeaf _quickShootRangeWeaponNodeLeaf => this.playerStateNodeManager.quickShootRangeWeaponNodeLeaf;
}
