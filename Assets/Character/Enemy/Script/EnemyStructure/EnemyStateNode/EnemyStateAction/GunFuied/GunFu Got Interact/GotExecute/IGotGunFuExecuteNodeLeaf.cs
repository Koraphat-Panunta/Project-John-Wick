using UnityEngine;

public interface IGotGunFuExecuteNodeLeaf : INodeLeaf,IGotGunFuAttackNode
{
    public IGotGunFuAttackedAble _gotExecutedGunFu { get; }
    public IGunFuAble _executerGunFu { get; }
    public GunFuExecuteScriptableObject _gunFuExecuteScriptableObject { get; }
}
