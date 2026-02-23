using System;
using System.Collections.Generic;
using UnityEngine;

public class FindiAndTrackingTarget : INodeLeaf
{
    private FieldOfView fieldOfView;
    private LayerMask targetMask;
    public float lostSightTiming { get;private set; }
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

    public FindiAndTrackingTarget(LayerMask targetMask,FieldOfView fieldOfView)
    {
        this.fieldOfView = fieldOfView;
        this.targetMask = targetMask;
    }
    public bool FindTarget(out GameObject target)
    {
        target = null;

        if (fieldOfView.TryFindSingleTarget(this.targetMask, out GameObject spottedTarget, new Vector3(0, 1.3f, 0)))
        {

            lastSeenPos = spottedTarget.transform.position;
            lostSightTiming = 0;
            isSpottingTarget = true;
            target = spottedTarget;

            if(OnSpottingTarget != null)
            OnSpottingTarget.Invoke(target);

            return true;
        }
        else
        {
            lostSightTiming += Time.deltaTime;
            isSpottingTarget = false;

            return false;
        }
      
    }

    private float findingTargetTimeInterval = .25f;
    private float findingTargetTimer;
    public void UpdateNode()
    {

        if (this.target != null
            && (Physics.Raycast(rayCastPos.position
                , (this.target.transform.position - rayCastPos.position).normalized
                , Vector3.Distance(rayCastPos.position, this.target.transform.position)
                , LayerMask.GetMask("Default")) == false))
        {

            this.targetKnewPos = this.target.transform.position;
        }



        findingTargetTimer += Time.deltaTime;

        if (findingTargetTimer < findingTargetTimeInterval)
            return;

        if (this.FindTarget(out GameObject target))
            this.target = target.transform;
        else
            this.target = null;

        findingTargetTimer = 0;
    }

    public void Enter()
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        throw new NotImplementedException();
    }

    public void FixedUpdateNode()
    {
        throw new NotImplementedException();
    }

    public bool IsComplete()
    {
        throw new NotImplementedException();
    }

    public bool IsReset()
    {
        throw new NotImplementedException();
    }

    public bool Precondition()
    {
        throw new NotImplementedException();
    }
}

