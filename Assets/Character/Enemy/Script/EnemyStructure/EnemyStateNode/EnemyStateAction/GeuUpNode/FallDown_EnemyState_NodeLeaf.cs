using System;
using System.Collections.Generic;
using UnityEngine;

public class FallDown_EnemyState_NodeLeaf : EnemyStateLeafNode,INodeLeafTransitionAble
{

    private Transform _root;
    private Transform _hipsBone;
    private Transform[] _bones;
    private BoneTransform[] _ragdollBoneTransforms;

    private float downTimer;
    public float downDuration = 1;

    public INodeManager nodeManager { get => enemy.enemyStateManagerNode ; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public FallDown_EnemyState_NodeLeaf(Enemy enemy, IRagdollAble fallDownGetUpAble, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

        _root = fallDownGetUpAble._root;
        _hipsBone = fallDownGetUpAble._hipsBone;
        _bones = fallDownGetUpAble._bones;

        _ragdollBoneTransforms = new BoneTransform[_bones.Length];


        for (int i = 0; i < _bones.Length; i++)
        {
            _ragdollBoneTransforms[i] = new BoneTransform();
        }

    }

    public override void Enter()
    {

        downTimer = 0;
        if (enemy.motionControlManager.curMotionState != enemy.motionControlManager.ragdollMotionState)
            enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.ragdollMotionState);
        isComplete = false;

        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }
    public override void FixedUpdateNode()
    {

        this.UpdateDownTimer();
        base.FixedUpdateNode();
    }


    Vector3 beforeRootPos;
    public override void UpdateNode()
    {
        (enemy._movementCompoent).UpdateMoveToDirWorld(Vector3.zero, 2, 7, MoveMode.MaintainMomentum);
        RagdollBoneBehavior.AlignRotationToHips(_hipsBone, enemy.transform);
        RagdollBoneBehavior.AlignPositionToHips(_root, _hipsBone, enemy.transform, _ragdollBoneTransforms[0]);
        if (_hipsBone.transform.position.y < enemy.transform.position.y)
            _hipsBone.transform.position = enemy.transform.position;



        if (downTimer >= downDuration)
        {
            isComplete = true;
            nodeLeafTransitionBehavior.TransitionAbleAll(this);
            TransitioningCheck();
        }

    }
    private Vector3 lastHipPos;

    private void UpdateDownTimer()
    {
        if(downTimer <= 0)
            lastHipPos = _hipsBone.transform.position;

 
        if(Vector3.Distance(this._hipsBone.transform.position,lastHipPos) <= 0.025f)
        {
            downTimer += Time.fixedDeltaTime;
        }


        lastHipPos = _hipsBone.transform.position;
    }
  
    public override bool IsReset()
    {
        if (enemy._triggerHitedGunFu)
            return true;


        if (IsComplete())
            return true;

        if (enemy.isDead)
            return true;

        if (enemy._isPainTrigger)
            return true;

        return false;
    }

    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
  
}


