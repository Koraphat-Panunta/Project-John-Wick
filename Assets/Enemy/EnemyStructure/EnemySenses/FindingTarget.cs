using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingTarget 
{
    private FieldOfView fieldOfView;
    private LayerMask targetMask;
    private IFindingTarget userFinding;
    public float lostSightTiming { get;private set; }
    public bool isSpottingTarget { get; private set; }
    private Vector3 lastSeenPos { get; /*private*/ set; }
    public FindingTarget(LayerMask playerMask,FieldOfView fieldOfView,IFindingTarget userFinding)
    {
        this.fieldOfView = fieldOfView;
        this.targetMask = playerMask;
        this.userFinding = userFinding;
    }
    public bool FindTarget(out GameObject target)
    {
        target = null;
        if (fieldOfView.FindSingleObjectInView(targetMask, new Vector3(0, 1.3f, 0), out GameObject spottedTarget))
        {

            lastSeenPos = spottedTarget.transform.position;
            userFinding.targetKnewPos = lastSeenPos;
            lostSightTiming = 0;
            isSpottingTarget = true;
            target = spottedTarget;
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
