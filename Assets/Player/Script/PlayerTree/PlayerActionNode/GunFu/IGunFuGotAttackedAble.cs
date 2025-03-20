using UnityEngine;

public interface IGunFuGotAttackedAble 
{
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuHitedAble { get; set; }
    public Vector3 attackedPos { get; set; }
    public IGunFuNode curAttackerGunFuNode { get; set; }
    public INodeLeaf curNodeLeaf { get; set; }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IMovementCompoent _movementCompoent { get; set; }
    public IWeaponAdvanceUser _weaponAdvanceUser { get; set; }
    public bool _isDead { get; set; }
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble gunFuAble);
}
