using UnityEngine;

public interface IGunFuNode:IDamageVisitor,INodeLeaf
{
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get; set; }
    public IGotGunFuAttackedAble attackedAbleGunFu { get; set; }
    public AnimationClip _animationClip { get; set; }
    
}
