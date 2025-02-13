using UnityEngine;

public interface IGunFuGotAttackedAble 
{
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuHitedAble { get; set; }
    public Vector3 attackedPos { get; set; }
    public IGunFuNode curGotAttackedGunFuNode { get; set; }
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble gunFuAble);
}
