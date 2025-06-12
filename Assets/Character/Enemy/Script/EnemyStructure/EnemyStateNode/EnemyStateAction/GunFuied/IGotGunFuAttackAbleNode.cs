using UnityEngine;

public interface IGotGunFuAttackAbleNode:INodeLeaf 
{
    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public AnimationClip _animationClip { get; set; }
}
