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
}
