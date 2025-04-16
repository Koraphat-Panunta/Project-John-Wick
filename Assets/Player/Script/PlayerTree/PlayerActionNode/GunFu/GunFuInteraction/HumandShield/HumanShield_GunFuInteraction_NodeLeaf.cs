using System;

using UnityEngine;
using UnityEngine.ProBuilder;

public class HumanShield_GunFuInteraction_NodeLeaf : PlayerGunFu_Interaction_NodeLeaf
{
    IWeaponAdvanceUser weaponAdvanceUser;

    public string humandShieldEnter = "HumandShieldEnter";
    public string humandShieldStay = "HumanShieldStay";
    //private string humandShieldExit = "HumandThrow";

    private float EnterDuration = 0.716f;
    private float elaspeTimmerEnter;

    public float elapesTimmerStay { get; private set; }
    public float StayDuration { get; private set; }

    public float distanceRightOffset { get 
        {
            if (player.curNodeLeaf is SecondaryWeapon)
                return -0.29f;
            else  /*(player.curNodeLeaf is PrimaryWeapon)*/
                return -0.23f;        
        }
    }
    public float distanceUpOffset = -0.035f;

    public enum HumanShieldInteractionPhase
    {
        Enter,
        Stay,
        Release
    }
    public HumanShieldInteractionPhase curIntphase;
    public HumanShield_GunFuInteraction_NodeLeaf(Player player, Func<bool> preCondition,GunFuInteraction_ScriptableObject gunFuInteraction_ScriptableObject) : base(player, preCondition,gunFuInteraction_ScriptableObject)
    {
        weaponAdvanceUser = player;
        StayDuration = 10;
    }

    public override void Enter()
    {
        curIntphase = HumanShieldInteractionPhase.Enter;
        elaspeTimmerEnter = 0;
        elapesTimmerStay = 0;

        base.Enter();
    }

    public override void Exit()
    {
        curIntphase = HumanShieldInteractionPhase.Release;
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

   


    Vector3 beforeAimConstrainOffset;
    public override void UpdateNode()
    {
        switch (curIntphase)
        {
            case HumanShieldInteractionPhase.Enter:
                {
                    elaspeTimmerEnter += Time.deltaTime;

                    attackedAbleGunFu.TakeGunFuAttacked(this, player);

                    attackedAbleGunFu._gunFuAttackedAble.position = Vector3.Lerp(
                        attackedAbleGunFu._gunFuAttackedAble.position, 
                        targetAdjustTransform.position + (targetAdjustTransform.right*distanceRightOffset) + (targetAdjustTransform.up* distanceUpOffset), 
                        elaspeTimmerEnter / EnterDuration
                        );

                    attackedAbleGunFu._gunFuAttackedAble.rotation = Quaternion.Lerp(
                        attackedAbleGunFu._gunFuAttackedAble.rotation,
                        targetAdjustTransform.rotation,
                        elaspeTimmerEnter / EnterDuration
                        );

                    if (elaspeTimmerEnter >= EnterDuration)
                    {
                        curIntphase = HumanShieldInteractionPhase.Stay;
                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);
                    }
                }
                break;

            case HumanShieldInteractionPhase.Stay:
                {
                    elapesTimmerStay += Time.deltaTime;
                    nodeLeafTransitionBehavior.TransitionAbleAll(this);

                    attackedAbleGunFu._gunFuAttackedAble.position = targetAdjustTransform.position + (targetAdjustTransform.right * distanceRightOffset) + (targetAdjustTransform.up * distanceUpOffset);
                    attackedAbleGunFu._gunFuAttackedAble.rotation = targetAdjustTransform.rotation;

                    player.playerMovement.MoveToDirLocal(player.inputMoveDir_Local, player.StandMoveAccelerate, player.StandMoveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);

                    Debug.Log("attackedAbleGunFu = " + attackedAbleGunFu);

                    if(attackedAbleGunFu._isDead)
                        isComplete = true;

                    if(elapesTimmerStay >= StayDuration)
                        isComplete = true;
                }
                break;
        }
        base.UpdateNode();
    }
    public Vector3 humanShieldGetShootDir { get; private set; }
    public void HumanShieldedOpponentGotShoot(Vector3 hitDir)
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.HumanShieldOpponentGetShoot);
        humanShieldGetShootDir = hitDir;
    }


}
