using UnityEngine;

public interface IGotGunFuExecuteNodeLeaf : INodeLeaf,IGotGunFuAttackNode
{
    public IGotGunFuAttackedAble _gotExecutedGunFu { get; }
    public IGunFuAble _executerGunFu { get; }
    public GunFuExecute_Single_ScriptableObject _gunFuExecute_Single_ScriptableObject { get; }
}
