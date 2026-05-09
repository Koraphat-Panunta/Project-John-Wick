using UnityEngine;

public interface IQuickShotAble 
{
    public IRangeWeaponAdvanceUser _rangeWeaponAdvanceUser { get; }
    public INodeManager _nodeManager { get; }
    public QuickShootRangeWeaponNodeLeaf _quickShootRangeWeaponNodeLeaf { get; }
    public Vector3 _quickShotTargetPos { get 
        {
            return this._quickShootRangeWeaponNodeLeaf.targetQuickShot;
        }
    }
    public bool _isQucikShot { get => _nodeManager.TryGetCurNodeLeaf<QuickShootRangeWeaponNodeLeaf>(); }
}
