using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class EnemyDodgeStateNodeLeaf : EnemyStateLeafNode
{
    IMotionImplusePushAble motionImplusePushAble => enemy._movementCompoent as EnemyMovement;
    EnemyMovement enemyMovement => enemy._movementCompoent as EnemyMovement;

    public INodeManager nodeManager { get => enemy.stateManagerNode; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    private float _duration;
    private float _pushOutNormalized;
    private float _inAirNormalized;
    private float _landingNormalized;
    private float _coolDownTime;
    private Action _onExitCallback;

    private float elapesTime;

    public float dodgeRollCoolDown { get; private set; }

    public enum DodgePhase
    {
        pushOut,
        InAir,
        Landing
    }
    public DodgePhase dodgePhase;

    public EnemyDodgeStateNodeLeaf(
        Enemy enemy,
        Func<bool> preCondition,
        float duration,
        float pushOutNormalized,
        float inAirNormalized,
        float landingNormalized,
        float coolDownTime,
        Action onExitCallback = null) : base(enemy, preCondition)
    {
        _duration = duration;
        _pushOutNormalized = pushOutNormalized;
        _inAirNormalized = inAirNormalized;
        _landingNormalized = landingNormalized;
        _coolDownTime = coolDownTime;
        _onExitCallback = onExitCallback;

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
        enemyMovement.AddForcePushInstantly(enemy.dodgeImpluseForce * (enemy.moveInputVelocity_WorldCommand + enemy._movementCompoent.curMoveVelocity_World.normalized).normalized, IMotionImplusePushAble.PushMode.IgnoreMomentum);
        dodgePhase = DodgePhase.pushOut;

        dodgeRollCoolDown = _coolDownTime;
        base.Enter();
    }

    public override void Exit()
    {
        _onExitCallback?.Invoke();
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

        if (enemy._isPainTrigger)
            return true;

        if (enemy._triggerEnterGotAttacked_OCM)
            return true;

        return IsComplete();
    }

    public override void UpdateNode()
    {
        elapesTime += Time.deltaTime;
        if (elapesTime > _duration)
            isComplete = true;

        if (dodgePhase == DodgePhase.pushOut)
        {
            float t = (1 / Mathf.Pow(_duration * _pushOutNormalized, 2)) * (Mathf.Pow(elapesTime, 2));
            enemy._movementCompoent.SetRotateToDirWorldSlerp(enemy._movementCompoent.curMoveVelocity_World.normalized, t);
            if (elapesTime > _duration * _pushOutNormalized)
                dodgePhase = DodgePhase.InAir;
        }
        else if (dodgePhase == DodgePhase.InAir)
        {
            enemyMovement.UpdateMoveToDirWorld(Vector3.zero, enemy.dodgeInAirStopForce, MoveMode.MaintainMomentumDirection);
            if (elapesTime > _inAirNormalized)
            {
                dodgePhase = DodgePhase.Landing;
            }
        }
        else if (dodgePhase == DodgePhase.Landing)
        {
            enemyMovement.UpdateMoveToDirWorld(Vector3.zero, enemy.dodgeOnGroundStopForce, MoveMode.MaintainMomentumDirection);
        }
        base.UpdateNode();
    }

    private IEnumerator CoolDown()
    {
        while (dodgeRollCoolDown > 0)
        {
            dodgeRollCoolDown -= Time.deltaTime;
            yield return null;
        }
    }
}
