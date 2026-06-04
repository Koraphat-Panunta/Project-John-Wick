using UnityEngine;

public partial class EnemyConstrainAnimationNodeManager 
{
    public bool isBodyConstriantEnable
    {
        get
        {
            if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
                return true;

            if (this.enemy._currentWeapon == null)
                return false;

            if ((this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>())
                return false;

            if (this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;


            return false;
        }
    }

    public bool isRightArmConstraintEnable
    {
        get 
        {
            if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
                return true;

            if (this.enemy._currentWeapon == null)
                return false;

            if ((this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>())
                return false;

            if (this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;


            return false;
        }
    }

    public bool isLeftArmConstraintEnable
    {
        get
        {
            if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
                return true;

            if (this.enemy._currentWeapon == null)
                return false;

            if ((this.enemy._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>())
                return false;

            if (this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;

           
            return false;
        }
    }

    public bool isWeaponGripConstraintEnable
    {
        get
        {
            if (this.enemy._currentWeapon == null)
                return false;

            

            if (this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || this.enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;

            return false;
        }
    }
}
