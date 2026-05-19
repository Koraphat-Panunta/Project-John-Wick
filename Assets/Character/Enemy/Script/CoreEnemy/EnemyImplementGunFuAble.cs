using System;
using UnityEngine;

public partial class Enemy : I_OCM_Attack_Able
{
    #region ImplementGunFuAble
    public bool _triggerAttack { get; set; }
    public bool _triggerExecute { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float triggerGunFuBufferTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Transform _gunFuUserTransform { get => this.transform; set { } }
    public Transform _targetAdjustTranform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Vector3 _attackAimDir { get => this.transform.forward; set { } }

    [SerializeField] private OCM_Offendsive_DetectTarget gunFuDetectTarget;
    public OCM_Offendsive_DetectTarget _gunFuDetectTarget { get => this.gunFuDetectTarget; set => gunFuDetectTarget = value; }
    public LayerMask _layerTarget { get => this.findingTargetScriptableObject.targetLayer; set { } }
    public I_Got_OCM_Attacked_Able attackedAbleGunFu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public I_Got_OCM_Attacked_Able executedAbleGunFu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public I_OCM_Node curGunFuNode
    {
        get
        {
            if (this.stateManagerNode.GetCurNodeLeaf() is I_OCM_Node gunFuNode)
                return gunFuNode;
            return null;

        }
        set { }
    }

    Animator I_OCM_Attack_Able._gunFuAnimator => animator;

    public Character _character { get => this; }

    [SerializeField] public GunFuHitScriptableObject spinKickScriptable;
    public void InitailizedGunFuComponent()
    {
        _gunFuUserTransform = transform;
    }

    public void UpdateDetectingTarget()
    {
        throw new NotImplementedException();
    }
    #endregion
}
