using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverObstacle 
{
    public Collider collider;
    public RaycastHit hitInfo;
    public Vector3[] coverPivotPos = new Vector3[4];
    public Vector3[] coverPos = new Vector3[4];
    public Vector3[] aimPos = new Vector3[4];
    public EnemyCoverObstacle(Collider collider,RaycastHit hitInfo) 
    { 
        this.collider = collider;
        this.hitInfo = hitInfo; 

        if(collider.TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
        {
            if(boxCollider.transform.rotation.x%360 != 0 || boxCollider.transform.rotation.z % 360 != 0)
            {
                Vector3 Center = new Vector3(this.collider.bounds.center.x, hitInfo.point.y, this.collider.bounds.center.z);
                coverPivotPos[0] = Center + new Vector3(-collider.bounds.extents.x, 0, -collider.bounds.extents.z);
                coverPivotPos[1] = Center + new Vector3(+collider.bounds.extents.x, 0, collider.bounds.extents.z);
                coverPivotPos[2] = Center + new Vector3(-collider.bounds.extents.x, 0, collider.bounds.extents.z);
                coverPivotPos[3] = Center + new Vector3(collider.bounds.extents.x, 0, collider.bounds.extents.z);
            }
            else
            {
                Vector3 Center = new Vector3(this.collider.bounds.center.x, hitInfo.point.y, this.collider.bounds.center.z);
                coverPivotPos[0] = Center + Vector3.Cross(collider.transform.forward,Vector3.up).normalized*(boxCollider.size.x * boxCollider.transform.localScale.x / 2)
                    + Vector3.Cross(collider.transform.right, Vector3.down).normalized * (boxCollider.size.z*boxCollider.transform.localScale.z / 2);

                coverPivotPos[1] = Center + Vector3.Cross(collider.transform.forward, Vector3.down).normalized * (boxCollider.size.x * boxCollider.transform.localScale.x / 2)
                    + Vector3.Cross(collider.transform.right, Vector3.down).normalized * (boxCollider.size.z * boxCollider.transform.localScale.z / 2);

                coverPivotPos[2] = Center + Vector3.Cross(collider.transform.forward, Vector3.up).normalized * (boxCollider.size.x * boxCollider.transform.localScale.x / 2)
                    + collider.transform.forward.normalized * (boxCollider.size.z * boxCollider.transform.localScale.z / 2);

                coverPivotPos[3] = Center + Vector3.Cross(collider.transform.forward, Vector3.down).normalized * (boxCollider.size.x * boxCollider.transform.localScale.x / 2)
                    +  collider.transform.forward.normalized * (boxCollider.size.z * boxCollider.transform.localScale.z / 2);
            }
        }
        else
        {
            Vector3 Center = new Vector3(this.collider.bounds.center.x, hitInfo.point.y, this.collider.bounds.center.z);
            coverPivotPos[0] = Center + new Vector3(-collider.bounds.extents.x, 0, -collider.bounds.extents.z);
            coverPivotPos[1] = Center + new Vector3(+collider.bounds.extents.x, 0, collider.bounds.extents.z);
            coverPivotPos[2] = Center + new Vector3(collider.bounds.extents.x, 0, collider.bounds.extents.z);
            coverPivotPos[3] = Center + new Vector3(collider.bounds.extents.x, 0, collider.bounds.extents.z);
        }
    }

}
