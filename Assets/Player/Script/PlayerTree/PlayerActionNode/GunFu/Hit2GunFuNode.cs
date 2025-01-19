using UnityEngine;

public class Hit2GunFuNode : GunFuHitNodeLeaf
{
    protected Vector3 targetPos;
    protected IGunFuDamagedAble gunFuDamagedAble;
    private bool isHiting;

    private bool gunFuTriggerBuufer;
    public Hit2GunFuNode(Player player, GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player, gunFuNodeScriptableObject)
    {
    }

    public override void Enter()
    {
        _timer = 0;
        gunFuDamagedAble = null;
        isHiting = false;
        gunFuTriggerBuufer = false;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);

        base.Enter();
    }

    public override void Exit()
    {
        _timer = 0;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuExit);
        player.playerMovement.FreezingCharacter();
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (_isExit)
        {
            if (player.inputMoveDir_Local.magnitude > 0)
                return true;
        }

        if (_isExit)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        if (player._triggerGunFu)
            return true;

        return false;
    }

    public override void Update()
    {

        if (_timer > _animationClip.length * 0.2f
           && player._triggerGunFu)
            gunFuTriggerBuufer = true;



        if (_timer >= _animationClip.length * _transitionAbleTime_Nornalized)
        {

        }
        if (_isTransitionAble &&
            (player._triggerGunFu || gunFuTriggerBuufer))
            player.ChangeNode(player.knockDown_GunFuNode);

        base.Update();
    }
}
