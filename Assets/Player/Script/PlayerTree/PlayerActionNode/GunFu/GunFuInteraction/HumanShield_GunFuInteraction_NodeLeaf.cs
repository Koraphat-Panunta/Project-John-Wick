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

   
    private float timehumandThrow;
    private float _timerHumandThrow;
    public enum InteractionPhase
    {
        Enter,
        Stay,
        Exit
    }
    private InteractionPhase curIntphase;
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
                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);
                    }
                }
                break;

            case InteractionPhase.Stay:
                {
                    if (player.isAiming == false)
                    {
                        animator.CrossFade(humandShieldExit, 0.1f, 0);
                        curIntphase = InteractionPhase.Exit;
                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);
                    }
                }
                break;
            case InteractionPhase.Exit: 
                {
                    _timerHumandThrow += Time.deltaTime;

                    if(_timerHumandThrow >= timehumandThrow)
                        _isExit = true;
                }
                break;
        }
        base.Update();
    }
}
