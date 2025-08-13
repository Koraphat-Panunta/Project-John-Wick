using System;
using UnityEngine;

public class HumanThrowGunFuInteractionNodeLeaf : PlayerGunFu_Interaction_NodeLeaf
{
    private readonly float throwDuration = 0.9f;
    private readonly float beforeThrowDuration = 0.335f;
    public enum HumanThrowPhase
    {
        beforeThrow,
        Throwing,
        afterThrow
    }
    public HumanThrowPhase curThrowPhase;
    public HumanThrowGunFuInteractionNodeLeaf(Player player, Func<bool> preCondition, GunFuInteraction_ScriptableObject gunFuInteraction_ScriptableObject) : base(player, preCondition, gunFuInteraction_ScriptableObject)
    {
    }
    public override void Enter()
    {
        Debug.Log("Human Throw Enter");
        gotGunExecutedAble = player.attackedAbleGunFu;
        (player._movementCompoent as MovementCompoent).CancleMomentum();

        curThrowPhase = HumanThrowPhase.beforeThrow;
        gotGunExecutedAble.TakeGunFuAttacked(this, player);

        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("Human Throw Exit");
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        if(_timer >= this.throwDuration)
            isComplete = true;

        switch (curThrowPhase)
        {
            case HumanThrowPhase.beforeThrow:{

                    gotGunExecutedAble._character.transform.position = targetAdjustTransform.position;
                    gotGunExecutedAble._character.transform.rotation = targetAdjustTransform.rotation;

                    if (_timer >= beforeThrowDuration)
                        curThrowPhase = HumanThrowPhase.Throwing;
                }
                break;
            case HumanThrowPhase.Throwing: 
                {
                    curThrowPhase = HumanThrowPhase.afterThrow;
                }
                break;
            case HumanThrowPhase.afterThrow: { }
                break;
        }
      

        
        base.UpdateNode();
    }
}
