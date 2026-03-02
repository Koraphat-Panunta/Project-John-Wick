using System;
using UnityEngine;

public partial class Enemy: IRagdollAble
{
    #region InitailizedFallDownGetUp Properties
    [SerializeField] public AnimationTriggerEventSCRP standUpAnimationTriggerSCRP;
    [SerializeField] public AnimationTriggerEventSCRP pushUpAnimationTriggerSCRP;

    Animator IRagdollAble._animator => animator;

    [SerializeField] private Transform hipsBone;
    public Transform _hipsBone => hipsBone;

    [SerializeField] private Transform rootModel;
    public Transform _root => rootModel;
    public Transform[] _bones => hipsBone.GetComponentsInChildren<Transform>();

    public Rigidbody[] _ragdollRigidbodies => rootModel.GetComponentsInChildren<Rigidbody>();

    public bool _isFallDown { get 
        {
            if(stateManagerNode.TryGetCurNodeLeaf<FallDown_EnemyState_NodeLeaf>())
                return true;
            if(stateManagerNode.TryGetCurNodeLeaf<GetUpStateNodeLeaf>(out GetUpStateNodeLeaf getUpNodeLeaf)
                && getUpNodeLeaf.isStandingComplete == false)
                return true;

            return false;
        } 
    }

    public bool _isGetUp { get 
        {
            if (stateManagerNode.TryGetCurNodeLeaf<FallDown_EnemyState_NodeLeaf>(out FallDown_EnemyState_NodeLeaf fallDown_EnemyState_NodeLeaf)
                ||stateManagerNode.TryGetCurNodeLeaf<GetUpStateNodeLeaf>())
                return true;
            return false;
        } 
    }

    #endregion
}
