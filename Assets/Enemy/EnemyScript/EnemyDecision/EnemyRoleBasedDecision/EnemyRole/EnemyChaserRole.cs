using UnityEngine;

public class EnemyChaserRole : EnemyRoleBasedDecision
{
    public override INodeLeaf curNodeLeaf { get; set ; }
    public override INodeSelector startNodeSelector { get ; set ; }
    public override NodeManagerBehavior nodeManagerBehavior { get; set; }

    public override void InitailizedNode()
    {

    }
}
