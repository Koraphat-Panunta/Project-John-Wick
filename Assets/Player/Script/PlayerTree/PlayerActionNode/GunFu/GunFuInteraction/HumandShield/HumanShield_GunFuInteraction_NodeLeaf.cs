using UnityEngine;
using UnityEngine.ProBuilder;

public class HumanShield_GunFuInteraction_NodeLeaf : GunFu_Interaction_NodeLeaf
{
    IWeaponAdvanceUser weaponAdvanceUser;
    public IGunFuDamagedAble gunFuAttackedAble;
    Animator animator;

    IGunFuAble gunFuAble;

    Transform targetAdjustTransform;

    private string humandShieldEnter = "HumandShieldEnter";
    private string humandShieldStay = "HumandShield";
    private string humandShieldExit = "HumandThrow";

   
    private float timehumandThrow = 0.95f;
    private float _timerHumandThrow;

    private HumandShield_GotInteract_NodeLeaf humandShield_GotInteract_NodeLeaf;
    public enum InteractionPhase
    {
        Enter,
        Stay,
        Exit
    }
    public InteractionPhase curIntphase;
    public HumanShield_GunFuInteraction_NodeLeaf(Player player) : base(player)
    {
        weaponAdvanceUser = player;
        this.animator = player.animator;
        gunFuAble = player;

        targetAdjustTransform = gunFuAble._targetAdjustTranform;

        
    }

    public override void Enter()
    {
        curIntphase = InteractionPhase.Enter;

        _timerHumandThrow = 0;
        elaspeTimmerEnter = 0;
        _isExit = false;

        beforeAimConstrainOffset = player._aimConstraint.data.offset;

        humandShield_GotInteract_NodeLeaf = gunFuAttackedAble._humandShield_GotInteract_NodeLeaf;
        base.Enter();
    }

    public override void Exit()
    {
        gunFuAttackedAble = null;
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
       if(_isExit)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    private float elaspeEnter = 0.35f;
    private float elaspeTimmerEnter;

    public override bool _isExit { get  ; set ; }

    Vector3 beforeAimConstrainOffset;
    public override void Update()
    {
        switch (curIntphase)
        {
            case InteractionPhase.Enter:
                {
                    elaspeTimmerEnter += Time.deltaTime;

                    gunFuAttackedAble.TakeGunFuAttacked(this, player);
                    animator.CrossFade(humandShieldEnter, 0.1f, 0);
                    player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);

                    if (targetAdjustTransform == null)
                        Debug.Log("targetAdjustTransform == null");

                    gunFuAttackedAble._gunFuHitedAble.position = Vector3.Lerp(
                        gunFuAttackedAble._gunFuHitedAble.position, 
                        targetAdjustTransform.position, 
                        elaspeTimmerEnter / elaspeEnter
                        );

                    gunFuAttackedAble._gunFuHitedAble.rotation = Quaternion.Lerp(
                        gunFuAttackedAble._gunFuHitedAble.rotation,
                        targetAdjustTransform.rotation,
                        elaspeTimmerEnter / elaspeEnter
                        );

                    if (elaspeTimmerEnter >= elaspeEnter)
                    {
                        animator.CrossFade(humandShieldStay, 0.1f, 0);
                        curIntphase = InteractionPhase.Stay;
                        humandShield_GotInteract_NodeLeaf.StateStay();

                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);
                    }
                }
                break;

            case InteractionPhase.Stay:
                {
                    gunFuAttackedAble._gunFuHitedAble.position = targetAdjustTransform.position;
                    gunFuAttackedAble._gunFuHitedAble.rotation = targetAdjustTransform.rotation;

                    if (weaponAdvanceUser.isAimingCommand)
                        weaponAdvanceUser.weaponCommand.AimDownSight();

                    if(weaponAdvanceUser.isAimingCommand)
                        weaponAdvanceUser.weaponCommand.PullTrigger();
                    else
                        weaponAdvanceUser.weaponCommand.CancleTrigger();

                    player._aimConstraint.data.offset = new Vector3(12,0,0);
                    player._aimConstraint.weight = 0.5f;

                    player.playerMovement.MoveToDirLocal(player.inputMoveDir_Local, player.moveAccelerate, player.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);


                    if ((player.weaponManuverManager.curNodeLeaf is AimDownSightWeaponManuverNodeLeaf) == false)
                    {
                        player._aimConstraint.data.offset = beforeAimConstrainOffset;
                        player._aimConstraint.weight = 1;

                        animator.CrossFade(humandShieldExit, 0.1f, 0);
                        curIntphase = InteractionPhase.Exit;
                        gunFuAttackedAble.TakeGunFuAttacked(this, player);
                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);
                        humandShield_GotInteract_NodeLeaf.StateExit();
                    }
                }
                break;
            case InteractionPhase.Exit: 
                {
                    _timerHumandThrow += Time.deltaTime;

                    player.playerMovement.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);

                    if (_timerHumandThrow >= timehumandThrow)
                        _isExit = true;
                }
                break;
        }
        base.Update();
    }
}
