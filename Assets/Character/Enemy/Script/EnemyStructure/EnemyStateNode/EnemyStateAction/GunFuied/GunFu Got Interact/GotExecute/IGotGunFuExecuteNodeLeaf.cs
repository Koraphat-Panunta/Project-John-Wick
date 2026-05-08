using UnityEngine;

public interface IGotGunFuExecuteNodeLeaf : INodeLeaf,IGotGunFuAttackNode
{
    public I_Got_OCM_Attacked_Able _gotExecutedGunFu { get; }
    public I_OCM_Attack_Able _executerGunFu { get; }
    public GotExecutedStateName _gotExecutedStateName { get; }
    public void Releses();
}
