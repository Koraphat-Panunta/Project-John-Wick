using System;
using UnityEngine;

public partial class Enemy: IRagdollAble
{
    #region InitailizedFallDownGetUp Properties
    [SerializeField] public AnimationTriggerEventSCRP standUpAnimationTriggerSCRP;
    [SerializeField] public AnimationTriggerEventSCRP pushUpAnimationTriggerSCRP;

    Animator IRagdollAble._animator => animator;


    public Transform _hipsBone => this.humanoidBone.hips;

    public Transform _root => this.gameObject.transform;
    public Transform[] _bones => this.humanoidBone.hips.GetComponentsInChildren<Transform>();

    public Rigidbody[] _ragdollRigidbodies => _root.GetComponentsInChildren<Rigidbody>();

    public bool _isFallDown { get 
        {
            try
            {

                if (stateManagerNode.TryGetCurNodeLeaf<FallDown_EnemyState_NodeLeaf>())
                    return true;
                if (stateManagerNode.TryGetCurNodeLeaf<GetUpStateNodeLeaf>(out GetUpStateNodeLeaf getUpNodeLeaf)
                    && getUpNodeLeaf.isStandingComplete == false)
                    return true;
            }
            catch
            {
                return false;
            }

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
