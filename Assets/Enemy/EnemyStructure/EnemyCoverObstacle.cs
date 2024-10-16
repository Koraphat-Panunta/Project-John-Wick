using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverObstacle 
{
    public Collider collider;
    public RaycastHit hitInfo;
    public Vector3 coverPos;
    public Vector3 aimPos;
    public EnemyCoverObstacle(Collider collider,RaycastHit hitInfo) 
    { 
        this.collider = collider;
        this.hitInfo = hitInfo; 
    }
}
