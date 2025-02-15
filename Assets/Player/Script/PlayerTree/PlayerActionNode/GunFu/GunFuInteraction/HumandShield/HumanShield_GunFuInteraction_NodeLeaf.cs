using System;
using UnityEngine;
using UnityEngine.ProBuilder;

public class HumanShield_GunFuInteraction_NodeLeaf : GunFu_Interaction_NodeLeaf
{
    IWeaponAdvanceUser weaponAdvanceUser;

    private string humandShieldEnter = "HumandShieldEnter";
    private string humandShieldStay = "HumandShield";
    private string humandShieldExit = "HumandThrow";

    private float timehumandThrow = 0.95f;
    private float _timerHumandThrow;
    public enum InteractionPhase
    {
        Enter,
        Stay,
        Exit
    }
    public InteractionPhase curIntphase;
    public HumanShield_GunFuInteraction_NodeLeaf(Player player, Func<bool> preCondition,GunFuInteraction_ScriptableObject gunFuInteraction_ScriptableObject) : base(player, preCondition,gunFuInteraction_ScriptableObject)
    {
        weaponAdvanceUser = player;
    }

    public override void Enter()
    {
        curIntphase = InteractionPhase.Enter;

        _timerHumandThrow = 0;
        elaspeTimmerEnter = 0;

        beforeAimConstrainOffset = player._aimConstraint.data.offset;

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

        if(player.playerMovement.isGround == false)
            return true;

        return false;
    }

    private float elaspeEnter = 0.35f;
    private float elaspeTimmerEnter;


    Vector3 beforeAimConstrainOffset;
    public override void UpdateNode()
    {
        switch (curIntphase)
        {
            case InteractionPhase.Enter:
                {
                    elaspeTimmerEnter += Time.deltaTime;

                    attackedAbleGunFu.TakeGunFuAttacked(this, player);
                    player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);

                    if (targetAdjustTransform == null)
                        Debug.Log("targetAdjustTransform == null");

                    attackedAbleGunFu._gunFuHitedAble.position = Vector3.Lerp(
                        attackedAbleGunFu._gunFuHitedAble.position, 
                        targetAdjustTransform.position, 
                        elaspeTimmerEnter / elaspeEnter
                        );

                    attackedAbleGunFu._gunFuHitedAble.rotation = Quaternion.Lerp(
                        attackedAbleGunFu._gunFuHitedAble.rotation,
                        targetAdjustTransform.rotation,
                        elaspeTimmerEnter / elaspeEnter
                        );

                    if (elaspeTimmerEnter >= elaspeEnter)
                    {
                        curIntphase = InteractionPhase.Stay;

                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);
                    }
                }
                break;

            case InteractionPhase.Stay:
                {
                    attackedAbleGunFu._gunFuHitedAble.position = targetAdjustTransform.position;
                    attackedAbleGunFu._gunFuHitedAble.rotation = targetAdjustTransform.rotation;

                    player._aimConstraint.data.offset = new Vector3(12,0,0);
                    player._aimConstraint.weight = 0.5f;

                    player.playerMovement.MoveToDirLocal(player.inputMoveDir_Local, player.StandMoveAccelerate, player.StandMoveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);


                    if ((player.weaponManuverManager.curNodeLeaf is AimDownSightWeaponManuverNodeLeaf) == false)
                    {
                        player._aimConstraint.data.offset = beforeAimConstrainOffset;
                        player._aimConstraint.weight = 1;

                        curIntphase = InteractionPhase.Exit;
                        attackedAbleGunFu.TakeGunFuAttacked(this, player);
                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);
                    }
                }
                break;
            case InteractionPhase.Exit: 
                {
                    _timerHumandThrow += Time.deltaTime;

                    player.playerMovement.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);

                    if (_timerHumandThrow >= timehumandThrow)
                        isComplete  = true;
                }
                break;
        }
        base.UpdateNode();
    }


}
