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
    public float elapesTimmerExit { get; private set; } 
    public float elapesTimmerExitAttacked { get; private set; }
    public float StayDuration { get; private set; }

    private float exitDuration;
    private float exitATKduration;
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
        Exit,
        ExitAttacked
    }
    public HumanShieldInteractionPhase curIntphase;
    public HumanShield_GunFuInteraction_NodeLeaf(Player player, Func<bool> preCondition,GunFuInteraction_ScriptableObject gunFuInteraction_ScriptableObject) : base(player, preCondition,gunFuInteraction_ScriptableObject)
    {
        weaponAdvanceUser = player;
        StayDuration = 6;
    }

    public override void Enter()
    {
        curIntphase = HumanShieldInteractionPhase.Enter;
        elaspeTimmerEnter = 0;
        elapesTimmerStay = 0;
        elapesTimmerExit = 0;
        elapesTimmerExitAttacked = 0;

        base.Enter();
    }

    public override void Exit()
    {
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

        if(player.isDead)
            return true;

        if(player._triggerHitedGunFu)
            return true;

        if(gotGunFuAttackedAble._character.isDead)
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

                    gotGunFuAttackedAble.TakeGunFuAttacked(this, player);

                    gotGunFuAttackedAble._character.transform.position = Vector3.Lerp(
                        gotGunFuAttackedAble._character.transform.position, 
                        targetAdjustTransform.position + (targetAdjustTransform.right*distanceRightOffset) + (targetAdjustTransform.up* distanceUpOffset), 
                        elaspeTimmerEnter / EnterDuration
                        );

                    gotGunFuAttackedAble._character.transform.rotation = Quaternion.Lerp(
                        gotGunFuAttackedAble._character.transform.rotation,
                        targetAdjustTransform.rotation,
                        elaspeTimmerEnter / EnterDuration
                        );

                    if (elaspeTimmerEnter >= EnterDuration)
                    {
                        curIntphase = HumanShieldInteractionPhase.Stay;
                        player.NotifyObserver(player,this);
                    }
                }
                break;

            case HumanShieldInteractionPhase.Stay:
                {
                    elapesTimmerStay += Time.deltaTime;
                    nodeLeafTransitionBehavior.TransitionAbleAll(this);

                    gotGunFuAttackedAble._character.transform.position = targetAdjustTransform.position + (targetAdjustTransform.right * distanceRightOffset) + (targetAdjustTransform.up * distanceUpOffset);
                    gotGunFuAttackedAble._character.transform.rotation = targetAdjustTransform.rotation;

                    player._movementCompoent.MoveToDirLocal(player.inputMoveDir_Local, player.StandMoveAccelerate, player.StandMoveMaxSpeed, MoveMode.MaintainMomentum);

                    Debug.Log("attackedAbleGunFu = " + gotGunFuAttackedAble);

                    if(gotGunFuAttackedAble._character.isDead)
                        isComplete = true;

                    if(elapesTimmerStay >= StayDuration 
                        ||(player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>() == false)
                    {
                        curIntphase = HumanShieldInteractionPhase.Exit;
                        player.NotifyObserver(player,this);
                    }
                       
                }
                break;
            case HumanShieldInteractionPhase.Exit: 
                {
                    elapesTimmerExit += Time.deltaTime;

                    player._movementCompoent.MoveToDirLocal(Vector3.zero, player.breakMaxSpeed, player.breakDecelerate, MoveMode.MaintainMomentum);

                    gotGunFuAttackedAble._character.transform.position = targetAdjustTransform.position + (targetAdjustTransform.right * distanceRightOffset) + (targetAdjustTransform.up * distanceUpOffset);
                    gotGunFuAttackedAble._character.transform.rotation = targetAdjustTransform.rotation;

                    if (elapesTimmerExit >= exitDuration)
                    {
                        curIntphase = HumanShieldInteractionPhase.ExitAttacked;
                        player.NotifyObserver(player,this);
                    }


                }
                break;
            case HumanShieldInteractionPhase.ExitAttacked: 
                {
                    elapesTimmerExitAttacked += Time.deltaTime;

                    player._movementCompoent.MoveToDirLocal(Vector3.zero, player.breakMaxSpeed, player.breakDecelerate, MoveMode.MaintainMomentum);
                    if (elapesTimmerExitAttacked >= exitATKduration)
                    {
                        isComplete = true;
                    }
                }
                break;
        }
        base.UpdateNode();
    }
    public Vector3 humanShieldGetShootDir { get; private set; }
    public void HumanShieldedOpponentGotShoot(Vector3 hitDir)
    {
        humanShieldGetShootDir = hitDir;
    }


}
