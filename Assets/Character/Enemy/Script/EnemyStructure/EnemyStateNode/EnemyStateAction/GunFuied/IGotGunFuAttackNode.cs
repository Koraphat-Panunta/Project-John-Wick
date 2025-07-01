using UnityEngine;

public interface IGotGunFuAttackNode:INodeLeaf 
{
    public float _timer { get; set; }
    public AnimationClip _animationClip { get; set; }
}
