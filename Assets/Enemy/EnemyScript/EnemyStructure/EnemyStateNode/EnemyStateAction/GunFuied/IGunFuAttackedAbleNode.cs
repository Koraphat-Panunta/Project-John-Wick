using UnityEngine;

public interface IGunFuAttackedAbleNode:INodeLeaf 
{
    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public bool _isExit { get; set; }
    public AnimationClip _animationClip { get; set; }
}
