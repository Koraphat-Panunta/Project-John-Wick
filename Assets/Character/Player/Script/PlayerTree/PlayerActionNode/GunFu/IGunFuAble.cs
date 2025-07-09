using UnityEngine;

public interface IGunFuAble
{
    public Animator _gunFuAnimator { get; }
    public bool _triggerGunFu { get; set; }
    public bool _triggerExecuteGunFu { get; set; }
    //public Transform _gunFuUserTransform { get; set; }
    public Transform _targetAdjustTranform { get; set; }
    public Vector3 _gunFuAimDir { get; set; }
    public GunFuDetectTarget _gunFuDetectTarget { get; set; }
    public LayerMask _layerTarget { get; set; }
    public IGotGunFuAttackedAble attackedAbleGunFu { get; set; }
    public IGotGunFuAttackedAble executedAbleGunFu { get; set; }
    public IGunFuNode curGunFuNode { get; set; }
    public StackGague gunFuExecuteStackGauge { get; set; }
    public Character _character { get; }
    public void InitailizedGunFuComponent();
    public void UpdateDetectingTarget();
}


