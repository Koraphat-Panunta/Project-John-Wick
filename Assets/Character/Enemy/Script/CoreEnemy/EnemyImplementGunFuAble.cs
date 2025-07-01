using System;
using UnityEngine;

public partial class Enemy : IGunFuAble
{
    #region ImplementGunFuAble
    public bool _triggerGunFu { get; set; }
    public bool _triggerExecuteGunFu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float triggerGunFuBufferTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Transform _gunFuUserTransform { get => transform; set { } }
    public Transform _targetAdjustTranform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Vector3 _gunFuAimDir { get => transform.forward; set { } }

    [SerializeField] private GunFuDetectTarget gunFuDetectTarget;
    public GunFuDetectTarget _gunFuDetectTarget { get => gunFuDetectTarget; set => gunFuDetectTarget = value; }
    public LayerMask _layerTarget { get => targetMask; set { } }
    public IGotGunFuAttackedAble attackedAbleGunFu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IGotGunFuAttackedAble executedAbleGunFu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IGunFuNode curGunFuNode
    {
        get
        {
            if (enemyStateManagerNode.GetCurNodeLeaf() is IGunFuNode gunFuNode)
                return gunFuNode;
            return null;

        }
        set { }
    }
    public StackGague gunFuExecuteStackGauge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    Animator IGunFuAble._gunFuAnimator => animator;

    [SerializeField] public EnemySpinKickScriptable EnemySpinKickScriptable;
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
