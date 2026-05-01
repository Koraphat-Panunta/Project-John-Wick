using UnityEngine;

public static class ReturnGotAttackNodeFromStateMachine 
{
    public static IGotGunFuAttackNode GetAttackNode(INodeManager nodeManager)
    {
        if(nodeManager.TryGetCurNodeLeaf<IGotGunFuAttackNode>(out IGotGunFuAttackNode gotGunFuAttackNode))
            return gotGunFuAttackNode;

        return null;
    }
}
