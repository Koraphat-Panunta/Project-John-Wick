using UnityEngine;

public partial class EnemyConstrainAnimationNodeManager 
{
    public bool isBodyConstriantEnable 
    {
        get 
        {
            if(aimDownSightBodyNodeSelector.Precondition()
                ||painStateProceduralBodyConstraintNodeLeaf.Precondition())
                return true;
            return false;
        }
    }

    public bool isRightArmConstraintEnable
    {
        get 
        {
            if(enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
                return true;
            return false;
        }
    }

    public bool isLeftArmConstraintEnable
    {
        get
        {
            if (enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
                return true;
            return false;
        }
    }
}
