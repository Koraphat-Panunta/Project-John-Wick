using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandIdleStateNode : EnemyStateLeafNode
{
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    public float decelerate = 4;
    public EnemyStandIdleStateNode(Enemy enemy) : base(enemy)
    {
        objectToward = new RotateObjectToward();
        agent = enemy.agent;
    }

    public EnemyStandIdleStateNode(Enemy enemy, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
        objectToward = new RotateObjectToward();
        agent = enemy.agent;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {

        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        //Animator animator = enemy.animator;

        //animator.SetFloat("Vertical", Mathf.Lerp(animator.GetFloat("Vertical"), 0, 2 * Time.deltaTime));
        //animator.SetFloat("Horizontal", Mathf.Lerp(animator.GetFloat("Horizontal"), 0, 2 * Time.deltaTime));

        //_enemy.lookRotation = (_enemy.agent.steeringTarget - _enemy.transform.position).normalized;
        enemy.curMoveVelocity_World = Vector3.Lerp(enemy.curMoveVelocity_World, Vector3.zero, decelerate * Time.deltaTime);

        agent.Move(enemy.curMoveVelocity_World * Time.deltaTime);
        objectToward.RotateToward(enemy.lookRotation, enemy.gameObject, enemy._rotateSpeed);

        base.Update();
    }
}
