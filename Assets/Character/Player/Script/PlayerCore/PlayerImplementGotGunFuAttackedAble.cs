using UnityEngine;

public partial class Player: IGotGunFuAttackedAble
{
    #region InitializedGotAttackedGunFu
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuAttackedAble { get => transform; set { } }
    public Vector3 attackerPos { get => transform.position; set { } }
    public IGunFuNode curAttackerGunFuNode { get; set; }
    public INodeLeaf curNodeLeaf { get => (playerStateNodeManager as INodeManager).GetCurNodeLeaf(); set => (playerStateNodeManager as INodeManager).SetCurNodeLeaf(value); }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IMovementCompoent _movementCompoent { get => playerMovement; set { } }
    public IWeaponAdvanceUser _weaponAdvanceUser { get => this; set { } }
    public IDamageAble _damageAble { get => this; set { } }
    public bool _isDead { get => base.isDead; set { } }
    public bool _isGotAttackedAble
    {
        get
        {
            if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<PlayerBrounceOffGotAttackGunFuNodeLeaf>())
                return false;
            return true;
        }
        set { }
    }
    public bool _isGotExecutedAble { get; set; }

    public Animator _animator => animator;

    public PlayerBrounceOffGotAttackGunFuScriptableObject PlayerBrounceOffGotAttackGunFuScriptableObject;
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble gunFuAble)
    {
        _triggerHitedGunFu = true;
        gunFuAbleAttacker = gunFuAble;
        curAttackerGunFuNode = gunFu_NodeLeaf;
    }
    #endregion
}
