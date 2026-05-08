using UnityEngine;

public interface I_OCM_Attack_Able : IMeleeAttackerAble
{
    public Animator _gunFuAnimator { get; }
    public bool _triggerAttack { get; set; }
    public bool _triggerExecute { get; set; }
    //public Transform _gunFuUserTransform { get; set; }
    public Transform _targetAdjustTranform { get; set; }
    public Vector3 _attackAimDir { get; set; }
    public OCM_Offendsive_DetectTarget _gunFuDetectTarget { get; set; }
    public I_Got_OCM_Attacked_Able attackedAbleGunFu { get; set; }
    public I_Got_OCM_Attacked_Able executedAbleGunFu { get; set; }
    public I_OCM_Node curGunFuNode { get; set; }
    public void InitailizedGunFuComponent();
    public void UpdateDetectingTarget();
}


