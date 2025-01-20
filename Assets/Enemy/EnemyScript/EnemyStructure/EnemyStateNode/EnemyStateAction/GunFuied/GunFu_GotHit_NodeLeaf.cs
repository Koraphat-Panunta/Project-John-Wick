using UnityEngine;

public abstract class GunFu_GotHit_NodeLeaf : EnemyStateLeafNode,IGunFuAttackedAbleNode
{
    protected Animator animator;
    protected string stateName;

    protected Vector3 pullBackPos;
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

        pullBackPos = CalculateLerpingKnockBack();
        base.Enter();
    }
    public override void Update()
    {
        _timer += Time.deltaTime;

        LearpingKnockBack();

        base.Update();
    }

    protected void LearpingKnockBack()
    {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, pullBackPos, 20 * Time.deltaTime);
        new RotateObjectToward().RotateTowardsObjectPos(enemy.attackedPos, enemy.gameObject, 20 * Time.deltaTime);
    }

    protected Vector3 CalculateLerpingKnockBack()
    {
        return  enemy.transform.position
            + (enemy.transform.position - enemy.attackedPos).normalized * 0.5f;
    }

    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public bool _isExit { get => _timer >= _animationClip.length*_exitTime_Normalized; set { } }
    public AnimationClip _animationClip { get; set; }
}
