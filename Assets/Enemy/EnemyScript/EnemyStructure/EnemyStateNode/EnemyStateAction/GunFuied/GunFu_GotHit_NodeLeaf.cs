using UnityEngine;

public abstract class GunFu_GotHit_NodeLeaf : EnemyStateLeafNode,IGunFuAttackedAbleNode
{
    protected Animator animator;
    protected string stateName;
    public GunFu_GotHit_NodeLeaf(Enemy enemy,GunFu_GotHit_ScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy)
    {
        _exitTime_Normalized = gunFu_GotHit_ScriptableObject.ExitTime_Normalized;
        _animationClip = gunFu_GotHit_ScriptableObject.AnimationClip;
        this.animator = enemy.animator;
        stateName = gunFu_GotHit_ScriptableObject.StateName;
    }
    public override void Enter()
    {
        _timer = 0;
        base.Enter();
    }
    public override void Update()
    {
        _timer += Time.deltaTime;
        base.Update();
    }
    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public bool _isExit { get => _timer >= _animationClip.length*_exitTime_Normalized; set { } }
    public AnimationClip _animationClip { get; set; }
}
