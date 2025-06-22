using UnityEngine;

public interface IGunFuNode:IDamageVisitor,INodeLeaf
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get; set; }
    public IGunFuGotAttackedAble attackedAbleGunFu { get; set; }
    public AnimationClip _animationClip { get; set; }
    
}
