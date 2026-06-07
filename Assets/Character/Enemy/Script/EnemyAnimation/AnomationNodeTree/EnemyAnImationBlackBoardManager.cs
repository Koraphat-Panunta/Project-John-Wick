using UnityEngine;

public partial class EnemyAnimationManager 
{

    public PlayPoseAnimationScriptableObject painStatePoseAnimationSCRP;

    protected INodeManager enemyStateManager => enemy.stateManagerNode;
    protected INodeManager enemyWeaponManuver => enemy._weaponManuverManager;
   
    protected bool isEnableUpperBodyLayer
    {
        get
        {
            if (isPerformReload) return true;
            if (isDrawSwitchWeapon) return true;
            return false;
        }
    }
    protected bool isEnableUpperArmLayer 
    {
        get
        {
            if (this.enemy._currentWeapon == null)
                return false;

            if ((this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>())
                return false;

            if ((this.enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || (this.enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || (this.enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || (this.enemy.enemyStateManagerNode as INodeManager).TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;

            return false;
        }
    }
    protected bool isPerformReload
    {
        get 
        {
            if(enemyWeaponManuver.TryGetCurNodeLeaf<IReloadNode>())
                return true;
            return false;
        }
    }
    protected bool isDrawSwitchWeapon
    {
        get
        {
            if (enemyWeaponManuver.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>()
                || enemyWeaponManuver.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>()
                || enemyWeaponManuver.TryGetCurNodeLeaf<HolsterPrimaryWeaponManuverNodeLeaf>()
                || enemyWeaponManuver.TryGetCurNodeLeaf<HolsterSecondaryWeaponManuverNodeLeaf>()
                )
                return true;
            return false;
        }
    }
}
