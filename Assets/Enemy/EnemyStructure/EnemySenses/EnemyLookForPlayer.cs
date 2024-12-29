using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookForPlayer 
{
    private FieldOfView FieldOfView;
    private LayerMask targetMask;
    public Vector3 _lastSeenPosition = Vector3.zero;
    public EnemyLookForPlayer(LayerMask playerMask,FieldOfView fieldOfView,Transform referencePos)
    {
        this.FieldOfView = fieldOfView;
        this.targetMask = playerMask;
    }
    public bool Recived(out GameObject target)
    {
        target = FieldOfView.FindSingleObjectInView(targetMask, new Vector3(0,1.2f,0));
        if(target != null)
        {
            return true;    
        }
        else
        {
            return false;
        }
    }

}
