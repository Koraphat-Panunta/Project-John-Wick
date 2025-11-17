using UnityEngine;

public interface IGunFuNode:IDamageVisitor,INodeLeaf
{
    public string _stateName { get; }
    public IGunFuAble gunFuAble { get; set; }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get; set; }
    
}
