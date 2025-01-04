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
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        switch (coverUseable.coverPoint) 
        {
            case CoverPointTallSingleSide coverPointTallSingle: 
                {
                    agent.Move(coverUseable.peekPos);
                }
                break;

            case CoverPointTallDoubleSide coverPointTallDouble: 
                {
                    if(coverUseable.coverPoint.CheckingTargetInCoverView(coverUseable,enemy.targetLayer,coverPointTallDouble.peekPosL,out GameObject target))
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable,coverPointTallDouble.peekPosL);
                        agent.Move(coverUseable.peekPos);
                    }
                    else
                    {
                        coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosR);
                        agent.Move(coverUseable.peekPos);
                    }
                }
                break;
       
            case CoverPointShort coverPointShort: 
                {
                    agent.Move(coverUseable.peekPos);
                }
                break;
        }
        base.Update();
    }
}
