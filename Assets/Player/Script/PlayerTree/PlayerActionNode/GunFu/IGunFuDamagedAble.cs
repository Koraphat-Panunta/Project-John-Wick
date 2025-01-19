using UnityEngine;

public interface IGunFuDamagedAble 
{
    public bool _triggerHitedGunFu { get; set; }
    public void TakeGunFuAttacked(GunFuHitNodeLeaf gunFuNodeLeaf);
    
}
