using UnityEngine;

public interface IGunFuGotAttackedAble
{
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuAttackedAble { get; set; }
    public Vector3 attackedPos { get; set; }
    public IGunFuNode curAttackerGunFuNode { get; set; }
    public INodeLeaf curNodeLeaf { get; set; }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IMovementCompoent _movementCompoent { get; set; }
    public IWeaponAdvanceUser _weaponAdvanceUser { get; set; }
    public IDamageAble _damageAble { get; set; }
    public bool _isDead { get; set; }
    public bool _isGotAttackedAble { get; set; }
    public bool _isGotExecutedAble { get; set; }
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble gunFuAble);
}
