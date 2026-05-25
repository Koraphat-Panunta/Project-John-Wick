using UnityEngine;

public interface I_OCM_Node:IDamageVisitor,INodeLeaf
{
    public NodePhase _curPhase { get; }
    public string _stateName { get; }
    public I_OCM_Attack_Able gunFuAble { get; set; }
    public I_Got_OCM_Attacked_Able gotGunFuAttackedAble { get; set; }
    
}
