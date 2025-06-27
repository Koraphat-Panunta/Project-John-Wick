using UnityEngine;

public partial class EnemyAnimationManager 
{
   
    protected INodeManager enemyStateManager => enemy.enemyStateManagerNode;
    protected INodeManager enemyWeaponManuver => enemy._weaponManuverManager;
    protected bool isEnableUpperLayer
    {
        get
        {
            if (enemyWeaponManuver.TryGetCurNodeLeaf<RestWeaponManuverLeafNode>())
                return false;

            if (enemyStateManager.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>()
                || enemyStateManager.TryGetCurNodeLeaf<IGotGunFuAttackAbleNode>()
                )
                return false;

            return true;
        }
    }
    protected bool isPerformReload
    {
        get => enemyWeaponManuver.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
            || enemyWeaponManuver.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
            ;
    }
    protected bool isDrawSwitchWeapon
    {
        get
        {
            if (enemyWeaponManuver.TryGetCurNodeLeaf<PrimaryToSecondarySwitchWeaponManuverLeafNode>()
                || enemyWeaponManuver.TryGetCurNodeLeaf<SecondaryToPrimarySwitchWeaponManuverLeafNode>()
                || enemyWeaponManuver.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>()
                || enemyWeaponManuver.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>()
                || enemyWeaponManuver.TryGetCurNodeLeaf<HolsterPrimaryWeaponManuverNodeLeaf>()
                || enemyWeaponManuver.TryGetCurNodeLeaf<HolsterSecondaryWeaponManuverNodeLeaf>()
                )
                return true;
            return false;
        }
    }
}
