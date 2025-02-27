using System;

using UnityEngine;
using UnityEngine.ProBuilder;

public class HumanShield_GunFuInteraction_NodeLeaf : GunFu_Interaction_NodeLeaf
{
    IWeaponAdvanceUser weaponAdvanceUser;

    public string humandShieldEnter = "HumandShieldEnter";
    public string humandShieldStay = "HumandShield";
    //private string humandShieldExit = "HumandThrow";

    private float EnterDuration = 0.716f;
    private float elaspeTimmerEnter;

    public float elapesTimmerStay { get; private set; }
    public float StayDuration { get; private set; }


    public enum InteractionPhase
    {
        Enter,
        Stay,
        Release
    }
    public InteractionPhase curIntphase;
    public HumanShield_GunFuInteraction_NodeLeaf(Player player, Func<bool> preCondition,GunFuInteraction_ScriptableObject gunFuInteraction_ScriptableObject) : base(player, preCondition,gunFuInteraction_ScriptableObject)
    {
        weaponAdvanceUser = player;
        StayDuration = 5;
    }

    public override void Enter()
    {
        curIntphase = InteractionPhase.Enter;
        elaspeTimmerEnter = 0;
        elapesTimmerStay = 0;
        beforeAimConstrainOffset = player._aimConstraint.data.offset;

        base.Enter();
    }

    public override void Exit()
    {
        player._aimConstraint.data.offset = beforeAimConstrainOffset;
        player._aimConstraint.weight = 1;

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
            case InteractionPhase.Enter:
                {
                    elaspeTimmerEnter += Time.deltaTime;

                    attackedAbleGunFu.TakeGunFuAttacked(this, player);

                    attackedAbleGunFu._gunFuHitedAble.position = Vector3.Lerp(
                        attackedAbleGunFu._gunFuHitedAble.position, 
                        targetAdjustTransform.position, 
                        elaspeTimmerEnter / EnterDuration
                        );

                    attackedAbleGunFu._gunFuHitedAble.rotation = Quaternion.Lerp(
                        attackedAbleGunFu._gunFuHitedAble.rotation,
                        targetAdjustTransform.rotation,
                        elaspeTimmerEnter / EnterDuration
                        );

                    if (elaspeTimmerEnter >= EnterDuration)
                    {
                        curIntphase = InteractionPhase.Stay;
                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuHold);
                    }
                }
                break;

            case InteractionPhase.Stay:
                {
                    elapesTimmerStay += Time.deltaTime;
                    nodeLeafTransitionBehavior.TransitionAbleAll(this);

                    attackedAbleGunFu._gunFuHitedAble.position = targetAdjustTransform.position;
                    attackedAbleGunFu._gunFuHitedAble.rotation = targetAdjustTransform.rotation;

                    player._aimConstraint.data.offset = new Vector3(12,0,0);
                    player._aimConstraint.weight = 0.5f;

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
