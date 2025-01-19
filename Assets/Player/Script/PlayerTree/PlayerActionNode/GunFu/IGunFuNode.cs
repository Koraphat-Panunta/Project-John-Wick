using UnityEngine;

public interface IGunFuNode 
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public bool _isExit { get; set; }
    public bool _isTransitionAble { get; set; }
    public AnimationClip _animationClip { get; set; }
    
}
