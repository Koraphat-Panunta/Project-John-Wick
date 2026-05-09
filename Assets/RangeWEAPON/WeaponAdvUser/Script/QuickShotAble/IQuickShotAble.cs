using UnityEngine;

public interface IQuickShotAble 
{
    public IRangeWeaponAdvanceUser _rangeWeaponAdvanceUser { get; }
    public INodeManager _nodeManager { get; }
    public QuickShootRangeWeaponNodeLeaf _quickShootRangeWeaponNodeLeaf { get; }
    public Vector3 _quickShotTargetPos { get 
        {
            try
            {
                return this._quickShootRangeWeaponNodeLeaf.targetQuickShot;
            }
            catch
            {
                return _rangeWeaponAdvanceUser._pointingPos;
            }
            
        }
    }
    public bool _isQucikShot { get => _nodeManager.TryGetCurNodeLeaf<QuickShootRangeWeaponNodeLeaf>(); }
}
