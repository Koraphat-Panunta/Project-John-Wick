using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingTarget 
{
    private FieldOfView fieldOfView;
    private LayerMask targetMask;
    public float lostSightTiming { get;private set; }
    public bool isSpottingTarget { get; private set; }
    public Vector3 lastSeenPos { get; private set; }

    public Action<GameObject> OnSpottingTarget;

    public FindingTarget(LayerMask targetMask,FieldOfView fieldOfView)
    {
        this.fieldOfView = fieldOfView;
        this.targetMask = targetMask;
    }
    public bool FindTarget(out GameObject target)
    {
        target = null;
        if (fieldOfView.FindSingleObjectInView(this.targetMask, new Vector3(0, 1.3f, 0), out GameObject spottedTarget))
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
            isSpottingTarget = false;
            lostSightTiming += Time.deltaTime;

            return false;
        }
       
    }


}

