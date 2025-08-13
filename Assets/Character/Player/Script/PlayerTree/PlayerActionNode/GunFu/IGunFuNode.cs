using UnityEngine;

public interface IGunFuNode:IDamageVisitor,INodeLeaf
{
    public float _timer { get; set; }
    public string _stateName { get; }
    public IGunFuAble gunFuAble { get; set; }
    public IGotGunFuAttackedAble gotGunExecutedAble { get; set; }
    public AnimationClip _animationClip { get; set; }
    
}
