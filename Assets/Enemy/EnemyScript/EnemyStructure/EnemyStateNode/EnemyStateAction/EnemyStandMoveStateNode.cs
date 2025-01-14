using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandMoveStateNode : EnemyStateLeafNode
{
    RotateObjectToward objectToward;
    public EnemyStandMoveStateNode(Enemy enemy) : base(enemy)
    {
        this.objectToward = new RotateObjectToward();
    }

    public EnemyStandMoveStateNode(Enemy enemy, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
        this.objectToward = new RotateObjectToward();
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
        Vector3 moveInputDirWorld = enemy.moveInputVelocity_World;
        Animator animator = enemy.animator;

        Vector3 animDir = enemy.transform.InverseTransformDirection(moveInputDirWorld);
        animator.SetFloat("Vertical", animDir.z, 0.5f, Time.deltaTime);
        animator.SetFloat("Horizontal", animDir.x, 0.1f, Time.deltaTime);

        //_enemy.lookRotation = (_enemy.agent.steeringTarget - _enemy.transform.position).normalized;
        objectToward.RotateToward(enemy.lookRotation, enemy.gameObject, enemy.rotateSpeed);

        base.Update();
    }
}
