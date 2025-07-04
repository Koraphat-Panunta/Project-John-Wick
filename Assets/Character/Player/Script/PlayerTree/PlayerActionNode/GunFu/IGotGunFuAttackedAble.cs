using UnityEngine;

public interface IGotGunFuAttackedAble
{
    public bool _triggerHitedGunFu { get; set; }
    //public Vector3 attackerPos { get; set; }
    public IGunFuNode curAttackerGunFuNode { get; set; }
    public INodeLeaf curNodeLeaf { get; set; }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IWeaponAdvanceUser _weaponAdvanceUser { get; set; }
    public IDamageAble _damageAble { get; set; }
    public Character _character { get; }
    public bool _isGotAttackedAble { get; set; }
    public bool _isGotExecutedAble { get; set; }
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble gunFuAble);
}
