using UnityEngine;

public interface IGunFuDamagedAble 
{
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuHitedAble { get; set; }
    public Vector3 attackedPos { get; set; }

    public void TakeGunFuAttacked(GunFuHitNodeLeaf gunFu_NodeLeaf,Vector3 attackerPos);

}
