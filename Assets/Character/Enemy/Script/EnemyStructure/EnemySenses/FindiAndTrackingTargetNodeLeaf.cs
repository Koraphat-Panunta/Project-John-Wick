using System;
using System.Collections.Generic;
using UnityEngine;


public class FindiAndTrackingTargetNodeLeaf : INodeLeaf
{
    private FieldOfView fieldOfView;
    private LayerMask targetMask { get => this.findingTargetScriptableObject.targetLayer; }
    public float lostSightTimer { get;private set; }
    public bool isSpottingTarget { get; private set; }
    public Vector3 lastSeenPos { get; private set; }
    public Vector3 targetKnewPos { get; private set; }
    public Action<GameObject> OnSpottingTarget;

    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set ; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }

    public Transform rayCastPos { get; protected set; }
    public Transform target;

    protected FindingTargetScriptableObject findingTargetScriptableObject;

    public FindiAndTrackingTargetNodeLeaf(FindingTargetScriptableObject findingTargetScriptableObject,Transform rayCastPos,Func<bool> preCondition)
    {
        this.isReset = new List<Func<bool>>();
        this.preCondition = preCondition;
        this.nodeLeafBehavior = new NodeLeafBehavior();

        this.rayCastPos = rayCastPos;
        this.fieldOfView = new FieldOfView
            (
            findingTargetScriptableObject
            ,this.rayCastPos
            );

        this.findingTargetScriptableObject = findingTargetScriptableObject;
    }
    private bool FindTarget(out GameObject target)
    {
        target = null;

        if (this.fieldOfView.TryFindSingleTarget(this.targetMask, out GameObject spottedTarget))
        {

            this.lastSeenPos = spottedTarget.transform.position;
            this.lostSightTimer = 0;
            this.isSpottingTarget = true;
            target = spottedTarget;

            if(this.OnSpottingTarget != null)
                this.OnSpottingTarget.Invoke(target);

            return true;
        }
        else
        {
            this.lostSightTimer += Time.deltaTime;
            this.isSpottingTarget = false;

            return false;
        }
      
    }

    private float findingTargetTimeInterval = .25f;
    private float findingTargetTimer;
    public void UpdateNode()
    {
        if (this.target != null
           && (Physics.Raycast(this.rayCastPos.position
               , (this.target.transform.position - this.rayCastPos.position).normalized
               , Vector3.Distance(this.rayCastPos.position, this.target.transform.position)
               , LayerMask.GetMask("Default")) == false))
        {

            this.targetKnewPos = this.target.transform.position;
        }



        this.findingTargetTimer += Time.fixedDeltaTime;

        //if (this.findingTargetTimer < this.findingTargetTimeInterval)
        //    return;

        if (this.FindTarget(out GameObject target))
        {
            this.target = target.transform;
        }
        else
        {
            this.target = null;
        }

        this.findingTargetTimer = 0;
    }
    public void Enter()
    {
        targetKnewPos = this.rayCastPos.position + this.rayCastPos.forward;
    }

    public void Exit()
    {
       
    }

    public void FixedUpdateNode()
    {
       
    }

    public bool IsComplete()
    {
        return false;
    }

    public bool IsReset() => this.nodeLeafBehavior.IsReset(this.isReset);
  
    public bool Precondition() => this.preCondition.Invoke();

    public void SetTargetKnowPos(Vector3 pos) => this.targetKnewPos = pos;
    
}

