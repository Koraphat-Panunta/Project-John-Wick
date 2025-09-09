using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDodgeRollStateNodeLeaf : PlayerStateNodeLeaf,INodeLeafTransitionAble
{
    IMotionImplusePushAble motionImplusePushAble => player._movementCompoent as PlayerMovement;
    PlayerMovement playerMovement => player._movementCompoent as PlayerMovement;

    public INodeManager nodeManager { get => player.playerStateNodeManager ; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get ; set; }

    float duration = 0.75f;
    float elapesTime;

    float coolDownTime = 0.5f;
    public bool isCoolDown;
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
    private Vector3 enterDir;
    public PlayerDodgeRollStateNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }
    public override bool Precondition()
    {
        if(isCoolDown)
            return false;

        return base.Precondition();
    }
    public override void Enter()
    {
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        elapesTime = 0;
        enterDir = player.inputMoveDir_World;
        playerMovement.AddForcePush(player.dodgeImpluseForce * (player.inputMoveDir_World + player._movementCompoent.curMoveVelocity_World.normalized).normalized, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
        dodgePhase = DodgePhase.pushOut;
        //player.NotifyObserver(player,this);
        player.playerStance = Stance.stand;
        base.Enter();
    }

    public override void Exit()
    {
        isCoolDown = true;
        _ = CoolDownDodge();
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
        if(player.isDead)
            return true;

       return IsComplete();
    }

    public override void UpdateNode()
    {
        elapesTime += Time.deltaTime;
        if(elapesTime > duration)
            isComplete = true;

        if(TransitioningCheck())
            return;

        if(dodgePhase == DodgePhase.pushOut)
        {
            float t = (1 / Mathf.Pow(duration * pushOutNormalized, 2)) * (Mathf.Pow(elapesTime, 2));
            player._movementCompoent.RotateToDirWorldSlerp(player._movementCompoent.curMoveVelocity_World.normalized, t);
            if(elapesTime > duration*pushOutNormalized)
                dodgePhase = DodgePhase.InAir;
        }
        else if(dodgePhase == DodgePhase.InAir)
        {
            playerMovement.MoveToDirWorld(Vector3.zero, player.dodgeInAirStopForce, player.dodgeInAirStopForce, MoveMode.MaintainMomentum);
            if (elapesTime > InAirNormalized)
            {
                dodgePhase = DodgePhase.Landing;
                nodeLeafTransitionBehavior.TransitionAbleAll(this);
            }
        }
        else if(dodgePhase == DodgePhase.Landing)
        {
            playerMovement.MoveToDirWorld(Vector3.zero, player.dodgeOnGroundStopForce, player.dodgeOnGroundStopForce, MoveMode.MaintainMomentum);
            //enemyMovement.RotateToDirWorld(player.inputMoveDir_World, player.sprintRotateSpeed);
        }
        base.UpdateNode();
    }
    private async Task CoolDownDodge()
    {
        await Task.Delay((int)(coolDownTime * 1000));
        isCoolDown = false;
    }
    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);


    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
   
}
