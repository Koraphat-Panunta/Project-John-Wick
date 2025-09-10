using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class EnemyDodgeRollStateNodeLeaf : EnemyStateLeafNode
{
    IMotionImplusePushAble motionImplusePushAble => enemy._movementCompoent as EnemyMovement;
    EnemyMovement enemyMovement => enemy._movementCompoent as EnemyMovement;

    public INodeManager nodeManager { get => enemy.enemyStateManagerNode; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    float duration = 0.75f;
    float elapesTime;

    public float dodgeRollCoolDown { get; private set; }

    public enum DodgePhase
    {
        pushOut,
        InAir,
        Landing
    }
    private const float pushOutNormalized = 0.23f;
    private const float InAirNormalized = 0.5f;
    private const float LandingNormalized = 1;
    public DodgePhase dodgePhase;
    public EnemyDodgeRollStateNodeLeaf(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }
    public override bool Precondition()
    {

        return base.Precondition();
    }
    public override void Enter()
    {
        elapesTime = 0;
        enemyMovement.AddForcePush(enemy.dodgeImpluseForce * (enemy.moveInputVelocity_WorldCommand + enemy._movementCompoent.curMoveVelocity_World.normalized).normalized, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
        dodgePhase = DodgePhase.pushOut;

        enemy.enemyStance = Stance.stand;
        dodgeRollCoolDown = 2;
        base.Enter();
    }

    public override void Exit()
    {
        enemy.StartCoroutine(CoolDown());
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        if (enemy.isDead)
            return true;

        if(enemy._isPainTrigger)
            return true;

        return IsComplete();
    }

    public override void UpdateNode()
    {
        elapesTime += Time.deltaTime;
        if (elapesTime > duration)
            isComplete = true;

      

        if (dodgePhase == DodgePhase.pushOut)
        {
            float t = (1 / Mathf.Pow(duration * pushOutNormalized, 2)) * (Mathf.Pow(elapesTime, 2));
            enemy._movementCompoent.RotateToDirWorldSlerp(enemy._movementCompoent.curMoveVelocity_World.normalized, t);
            if (elapesTime > duration * pushOutNormalized)
                dodgePhase = DodgePhase.InAir;
        }
        else if (dodgePhase == DodgePhase.InAir)
        {
            enemyMovement.MoveToDirWorld(Vector3.zero, enemy.dodgeInAirStopForce, enemy.dodgeInAirStopForce, MoveMode.MaintainMomentum);
            if (elapesTime > InAirNormalized)
            {
                dodgePhase = DodgePhase.Landing;
            }
        }
        else if (dodgePhase == DodgePhase.Landing)
        {
            enemyMovement.MoveToDirWorld(Vector3.zero, enemy.dodgeOnGroundStopForce, enemy.dodgeOnGroundStopForce, MoveMode.MaintainMomentum);
            //enemyMovement.RotateToDirWorld(player.inputMoveDir_World, player.sprintRotateSpeed);
        }
        base.UpdateNode();
    }

    private IEnumerator CoolDown()
    {
        while(dodgeRollCoolDown > 0)
        {
            dodgeRollCoolDown -= Time.deltaTime;
            yield return null;
        }    

    }
}
