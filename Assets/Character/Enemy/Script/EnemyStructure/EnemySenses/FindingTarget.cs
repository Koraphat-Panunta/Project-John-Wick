using System;
using UnityEngine;

public class FindingTarget 
{
    private FieldOfView fieldOfView;
    private LayerMask targetMask;
    public float lostSightTiming { get;private set; }
    public bool isSpottingTarget { get; private set; }
    public Vector3 lastSeenPos { get; private set; }

    public Action<GameObject> OnSpottingTarget;
    private float checkTimer;
    private float checkTimeInterval = 0.067f;
    public FindingTarget(LayerMask targetMask,FieldOfView fieldOfView)
    {
        this.fieldOfView = fieldOfView;
        this.targetMask = targetMask;
    }
    public bool FindTarget(out GameObject target)
    {
        target = null;

        checkTimer += Time.deltaTime;
        if (checkTimer < checkTimeInterval)
            return false;



        if (fieldOfView.TryFindSingleTarget(this.targetMask, out GameObject spottedTarget, new Vector3(0, 1.3f, 0)))
        {

            lastSeenPos = spottedTarget.transform.position;
            lostSightTiming = 0;
            isSpottingTarget = true;
            target = spottedTarget;

            if(OnSpottingTarget != null)
            OnSpottingTarget.Invoke(target);

            checkTimer = 0;
            return true;
        }
        else
        {
            isSpottingTarget = false;
            lostSightTiming += checkTimer;

            checkTimer = 0;
            return false;
        }
       

    }


}

