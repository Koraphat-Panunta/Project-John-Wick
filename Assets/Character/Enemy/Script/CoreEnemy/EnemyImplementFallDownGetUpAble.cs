using System;
using UnityEngine;

public partial class Enemy: IFallDownGetUpAble
{
    #region InitailizedFallDownGetUp Properties
    [SerializeField] private AnimationClip standUpClip;
    [SerializeField] private AnimationClip pushUpClip;
    public AnimationClip _standUpClip => standUpClip;

    public AnimationClip _pushUpClip => pushUpClip;

    Animator IFallDownGetUpAble._animator => animator;

    [SerializeField] private Transform hipsBone;
    public Transform _hipsBone => hipsBone;

    [SerializeField] private Transform rootModel;
    public Transform _root => rootModel;
    public Transform[] _bones => hipsBone.GetComponentsInChildren<Transform>();

    public Rigidbody[] _ragdollRigidbodies => rootModel.GetComponentsInChildren<Rigidbody>();

    public bool _isFallDown { get 
        {
            if(enemyStateManagerNode.TryGetCurNodeLeaf<FallDown_EnemyState_NodeLeaf>())
                return true;
            return false;
        } 
    }

    public bool _isGetUp { get 
        {
            if (enemyStateManagerNode.TryGetCurNodeLeaf<FallDown_EnemyState_NodeLeaf>(out FallDown_EnemyState_NodeLeaf fallDown_EnemyState_NodeLeaf)
                && fallDown_EnemyState_NodeLeaf.curState == FallDown_EnemyState_NodeLeaf.FallDownState.StandingUp)
                return true;
            return false;
        } 
    }

    #endregion
}
