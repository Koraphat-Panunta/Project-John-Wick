using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingTarget 
{
    private FieldOfView fieldOfView;
    private LayerMask targetMask;
    private IFindingTarget userFinding;
    private float lostSightTiming { get; set; }
    public float lostSightTimeSet = 5;
    public bool isLostSighttarget 
    { 
        get {
            if (lostSightTiming <= 0)
                return true;
            return false;
        } 
    }
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
        if(fieldOfView.FindSingleObjectInView(targetMask,new Vector3(0,1.3f,0),out GameObject spottedTarget)){

            lastSeenPos = spottedTarget.transform.position;
            userFinding.targetKnewPos = lastSeenPos;
            lostSightTiming = lostSightTimeSet;
            isSpottingTarget = true;
            target = spottedTarget;
            return true;
        }

        if(lostSightTiming >0)
        lostSightTiming -= Time.deltaTime;

        isSpottingTarget = false;
        return false;
       
    }

}
