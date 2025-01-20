using UnityEngine;
    
public abstract class GunFu_Interaction_NodeLeaf : PlayerActionNodeLeaf, IGunFuNode
{
    protected GunFu_Interaction_NodeLeaf(Player player) : base(player)
    {

    }
    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);
        _timer = 0;
        base.Enter();
    }
    public override void Exit()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuExit);
        base.Exit();
    }
    public override void Update()
    {
        _timer += Time.deltaTime;
        base.Update();
    }
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public virtual bool _isExit { get => _timer >= _animationClip.length * _exitTime_Normalized; set { } }
    public bool _isTransitionAble { get => _timer >= _animationClip.length * _transitionAbleTime_Nornalized; set { } }
    public AnimationClip _animationClip { get; set; }

}

