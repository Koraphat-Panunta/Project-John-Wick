using System;
using UnityEngine;

public abstract class GunFu_GotHit_NodeLeaf : EnemyStateLeafNode,IGunFuAttackedAbleNode
{
    protected Animator animator;
    protected string stateName;

    protected Vector3 pullBackPos;
    public IGunFuAble gunFuAble;
    public GunFu_GotHit_NodeLeaf(Enemy enemy,Func<bool> preCondition,GunFu_GotHit_ScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy,preCondition)
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
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        LearpingKnockBack();

        base.UpdateNode();
    }

    protected void LearpingKnockBack()
    {
        Vector3 lerpingPos = gunFuAble._gunFuUserTransform.position + gunFuAble._gunFuUserTransform.forward * 0.75f;

        enemy.transform.position = Vector3.Lerp(enemy.transform.position,
            new Vector3(lerpingPos.x,enemy.transform.position.y, lerpingPos.z), 
            _timer / _animationClip.length * 0.25f);

        new RotateObjectToward().RotateToward(gunFuAble._targetAdjustTranform.forward * -1, enemy.gameObject, 30 * Time.deltaTime);
    }

    protected Vector3 CalculateLerpingKnockBack()
    {
        return gunFuAble._targetAdjustTranform.position;
    }

    public float _exitTime_Normalized { get; set; }
    public float _timer { get; set; }
    public bool _isExit { get => _timer >= _animationClip.length*_exitTime_Normalized; set { } }
    public AnimationClip _animationClip { get; set; }
}
