using UnityEngine;

public interface IGotGunFuExecuteNodeLeaf : INodeLeaf,IGotGunFuAttackNode
{
    public IGotGunFuAttackedAble _gotExecutedGunFu { get; }
    public IGunFuAble _executerGunFu { get; }
    public GotExecutedStateName _gotExecutedStateName { get; }
}
