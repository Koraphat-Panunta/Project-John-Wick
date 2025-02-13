using UnityEngine;

public interface IGunFuAble
{
    public bool _triggerGunFu { get; set; }
    public float triggerGunFuBufferTime { get; set; }
    public Transform _gunFuUserTransform { get; set; }
    public Transform _targetAdjustTranform { get; set; }
    public Vector3 _gunFuAimDir { get; set; }
    public GunFuDetectTarget gunFuDetectTarget { get; set; }
    public LayerMask _layerTarget { get; set; }
    public IGunFuGotAttackedAble attackedAbleGunFu { get; set; }
    public void InitailizedGunFuComponent();
    public void UpdateDetectingTarget();
}


