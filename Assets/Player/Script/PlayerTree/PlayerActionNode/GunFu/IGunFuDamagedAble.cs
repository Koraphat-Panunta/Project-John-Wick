using UnityEngine;

public interface IGunFuDamagedAble 
{
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuHitedAble { get; set; }
    public Vector3 attackedPos { get; set; }
    public HumandShield_GotInteract_NodeLeaf _humandShield_GotInteract_NodeLeaf { get; set; }
    public void TakeGunFuAttacked(GunFuHitNodeLeaf gunFu_NodeLeaf, IGunFuAble gunFuAble);
    public void TakeGunFuAttacked(GunFu_Interaction_NodeLeaf gunFu_Interaction_NodeLeaf,IGunFuAble gunFuAble);
}
