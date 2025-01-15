using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeAimStateNode : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    NavMeshAgent agent;
    public EnemyStandTakeAimStateNode(Enemy enemy,ICoverUseable coverUseable) : base(enemy)
    {
        this.coverUseable = coverUseable;
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
        if (enemy.isDead)
            return true;

        if (enemy.isInCover == false)
            return true;
            
        if(enemy.isAiming == false)
            return true;

        if(enemy._isPainTrigger)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        if(enemy.isInCover
            &&enemy.isAiming)
            return true;

        return false;
    }

    public override void Update()
    {
        switch (coverUseable.coverPoint)
        {
            case CoverPointTallSingleSide coverPointTallSingle:
                {
                    Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized * Time.deltaTime * 2;
                    agent.Move(moveDir);
                }
                break;

            case CoverPointTallDoubleSide coverPointTallDouble:
                {
                    if (coverUseable.coverPoint.CheckingTargetInCoverView(coverUseable, enemy.targetLayer, coverPointTallDouble.peekPosL, out GameObject target))
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosL);
                        Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized * Time.deltaTime * 2;
                        agent.Move(moveDir);
                    }
                    else
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosR);
                        Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized * Time.deltaTime * 2;
                        agent.Move(moveDir);
                    }
                }
                break;

            case CoverPointShort coverPointShort:
                {
                    coverPointShort.TakeThisCover(coverUseable);
                    Vector3 moveDir = (coverUseable.peekPos - enemy.transform.position).normalized * Time.deltaTime * 2;
                    agent.Move(moveDir);
                }
                break;
        }

        Vector3 moveInputDirWorld = enemy.moveInputVelocity_World;
        Animator animator = enemy.animator;

        Vector3 animDir = enemy.transform.InverseTransformDirection(moveInputDirWorld);
        animator.SetFloat("Vertical", animDir.z, 0.5f, Time.deltaTime);
        animator.SetFloat("Horizontal", animDir.x, 0.1f, Time.deltaTime);

        new RotateObjectToward().RotateToward(enemy.lookRotation, enemy.gameObject, enemy._rotateSpeed);

        base.Update();
    }
}
