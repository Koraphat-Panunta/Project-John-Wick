using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCoverObstacle 
{
    public Collider collider;
    public RaycastHit hitInfo;
    public Vector3[] coverPivotPos = new Vector3[4];
    public Vector3[] coverPos = new Vector3[4];
    public Vector3[] aimPos = new Vector3[4];
    public Vector3 enemyPos;
    public Vector3 normalBound;

    public enum ObstacleType
    {
        BoxCollider,
        Bound
    }
    public ObstacleType obstacleType;
    public EnemyCoverObstacle(Collider collider,RaycastHit hitInfo,Vector3 enemyPos) 
    { 
        this.collider = collider;
        this.hitInfo = hitInfo;
        this.enemyPos = enemyPos;

        

        if (collider.TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
        {
            if(boxCollider.transform.rotation.x%360 != 0 || boxCollider.transform.rotation.z % 360 != 0)
            {
                SetCoverPivotPosition_Bound();
                SetCoverNormalDir_Bound();
                obstacleType = ObstacleType.Bound;
            }
            else
            {
                SetCoverPivotPosition_BoxCollider(boxCollider);
                normalBound = hitInfo.normal.normalized * Vector3.Distance(collider.bounds.center, hitInfo.point);
                obstacleType= ObstacleType.BoxCollider;
            }
        }
        else
        {
            SetCoverPivotPosition_Bound();
            SetCoverNormalDir_Bound();
            obstacleType = ObstacleType.Bound;
        }
        SetAimPos();
        SetCoverPos();
    }
    private void SetCoverPivotPosition_Bound()
    {
        Vector3 Center = new Vector3(this.collider.bounds.center.x, hitInfo.point.y, this.collider.bounds.center.z);
        coverPivotPos[0] = Center + new Vector3(-collider.bounds.extents.x, 0, -collider.bounds.extents.z);
        coverPivotPos[1] = Center + new Vector3(+collider.bounds.extents.x, 0, collider.bounds.extents.z);
        coverPivotPos[2] = Center + new Vector3(-collider.bounds.extents.x, 0, collider.bounds.extents.z);
        coverPivotPos[3] = Center + new Vector3(collider.bounds.extents.x, 0, collider.bounds.extents.z);

        coverPivotPos[0] += (coverPivotPos[0] - Center).normalized * 0.18f;
        coverPivotPos[1] += (coverPivotPos[1] - Center).normalized * 0.18f;
        coverPivotPos[2] += (coverPivotPos[2] - Center).normalized * 0.18f;
        coverPivotPos[3] += (coverPivotPos[3] - Center).normalized * 0.18f;
    }
    private void SetCoverNormalDir_Bound()
    {
        // Find the normal of the bound using raycast
        Ray rayToCenter = new Ray(enemyPos, (collider.bounds.center - enemyPos).normalized);
        if (collider.bounds.IntersectRay(rayToCenter, out float distance))
        {
            Vector3 hitPoint = rayToCenter.GetPoint(distance);
            Debug.Log("Ray hit the bounds of the collider at: " + hitPoint);

            // Calculate the surface normal based on the side of the bounds hit
            Vector3 boundsCenter = collider.bounds.center;
            Vector3 extents = collider.bounds.extents;

            Vector3 normal = Vector3.zero;

            // Compare the hit point with the bounds' sides
            if (Mathf.Approximately(hitPoint.x, boundsCenter.x + extents.x))
                normal = Vector3.right;
            else if (Mathf.Approximately(hitPoint.x, boundsCenter.x - extents.x))
                normal = Vector3.left;
            else if (Mathf.Approximately(hitPoint.z, boundsCenter.z + extents.z))
                normal = Vector3.forward;
            else if (Mathf.Approximately(hitPoint.z, boundsCenter.z - extents.z))
                normal = Vector3.back;
            normalBound = normal;

        }
    }
    private void SetAimPos()
    {
        Vector3 normalVertical = Vector3.Cross(normalBound.normalized,Vector3.up).normalized;
        Vector3 centerPosBound = collider.bounds.center;
        for(int i =0; i< coverPivotPos.Length; i++)
        {
            if (Vector3.Dot(normalVertical, (coverPivotPos[i]-centerPosBound).normalized) > 0)
            {
                aimPos[i] = coverPivotPos[i]+normalVertical*0.5f;
            }
            else
            {
                aimPos[i] = coverPivotPos[i] - normalVertical * 0.5f;
            }
        }

    }
    private void SetCoverPos()
    {
        Vector3 normalVertical = Vector3.Cross(normalBound.normalized, Vector3.up).normalized;
        Vector3 centerPosBound = collider.bounds.center;
        for (int i = 0; i < coverPivotPos.Length; i++)
        {
            if (Vector3.Dot(normalVertical, (coverPivotPos[i] - centerPosBound).normalized) > 0)
            {
                coverPos[i] = coverPivotPos[i] - normalVertical * 0.3f;
            }
            else
            {
                coverPos[i] = coverPivotPos[i] + normalVertical * 0.3f;
            }
        }
    }
    //private void SerAimPos_BoxCollider()
    //{
    //    Vector3 normalVertical = Vector3.Cross(normalBound.normalized, Vector3.up).normalized;
    //    Vector3 centerPosBound = collider.bounds.center;
    //    for (int i = 0; i <= coverPivotPos.Length; i++)
    //    {
    //        if (Vector3.Dot(normalVertical, (coverPivotPos[i] - centerPosBound).normalized) > 0)
    //        {
    //            aimPos[i] = coverPivotPos[i] - normalVertical * 0.3f;
    //        }
    //        else
    //        {
    //            aimPos[i] = coverPivotPos[i] + normalVertical * 0.3f;
    //        }
    //    }
    //}
    //private void SetCoverPos_BoxColiider()
    //{
    //    Vector3 normalVertical = Vector3.Cross(normalBound.normalized, Vector3.up).normalized;
    //    Vector3 centerPosBound = collider.bounds.center;
    //    for (int i = 0; i <= coverPivotPos.Length; i++)
    //    {
    //        if (Vector3.Dot(normalVertical, (coverPivotPos[i] - centerPosBound).normalized) > 0)
    //        {
    //            coverPos[i] = coverPivotPos[i] - normalVertical * 0.3f;
    //        }
    //        else
    //        {
    //            coverPos[i] = coverPivotPos[i] + normalVertical * 0.3f;
    //        }
    //    }
    //}
    private void SetCoverPivotPosition_BoxCollider(BoxCollider boxCollider)
    {
        Vector3 Center = new Vector3(this.collider.bounds.center.x, hitInfo.point.y, this.collider.bounds.center.z);
        coverPivotPos[0] = Center + Vector3.Cross(collider.transform.forward, Vector3.up).normalized * (boxCollider.size.x * boxCollider.transform.localScale.x / 2)
            + Vector3.Cross(collider.transform.right, Vector3.down).normalized * (boxCollider.size.z * boxCollider.transform.localScale.z / 2);

        coverPivotPos[1] = Center + Vector3.Cross(collider.transform.forward, Vector3.down).normalized * (boxCollider.size.x * boxCollider.transform.localScale.x / 2)
            + Vector3.Cross(collider.transform.right, Vector3.down).normalized * (boxCollider.size.z * boxCollider.transform.localScale.z / 2);

        coverPivotPos[2] = Center + Vector3.Cross(collider.transform.forward, Vector3.up).normalized * (boxCollider.size.x * boxCollider.transform.localScale.x / 2)
            + collider.transform.forward.normalized * (boxCollider.size.z * boxCollider.transform.localScale.z / 2);

        coverPivotPos[3] = Center + Vector3.Cross(collider.transform.forward, Vector3.down).normalized * (boxCollider.size.x * boxCollider.transform.localScale.x / 2)
            + collider.transform.forward.normalized * (boxCollider.size.z * boxCollider.transform.localScale.z / 2);

        coverPivotPos[0] += (coverPivotPos[0] - Center).normalized * 0.18f;
        coverPivotPos[1] += (coverPivotPos[1] - Center).normalized * 0.18f;
        coverPivotPos[2] += (coverPivotPos[2] - Center).normalized * 0.18f;
        coverPivotPos[3] += (coverPivotPos[3] - Center).normalized * 0.18f;

    }

}
