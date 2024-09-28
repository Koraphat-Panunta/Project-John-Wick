using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverDetection 
{
    public Vector3 coverPos { get;private set; }
    public Vector3 aimPos { get; private set; }
    private GameObject BallAimPos;
    private Vector3 detecEdgeOri;
    private Vector3 detecEdgeDes;
    private Vector3 obstacleSurfaceDir;
    public CoverDetection() 
    {
        //BallAimPos = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //BallAimPos.GetComponent<Collider>().enabled = false;   
        //BallAimPos.transform.localScale = Vector3.one*0.1f;
        //BallAimPos.GetComponent<Material>().color = Color.green;
    }
   
    public bool CheckingObstacleToward(Vector3 oriPos,Vector3 dir)
    {
        LayerMask decObstacle = LayerMask.GetMask("Default");
        if (Physics.Raycast(oriPos,dir,out RaycastHit hit, 1f, decObstacle))
        {
            Debug.Log("DetecObstacle");
            obstacleSurfaceDir = hit.normal;
            detecEdgeOri = hit.point + (obstacleSurfaceDir * 0.4f);
            Debug.DrawLine(hit.point, detecEdgeOri, Color.gray);
           
            Debug.DrawLine(detecEdgeOri, detecEdgeOri + Vector3.Cross(Vector3.up, obstacleSurfaceDir), Color.blue);
            Debug.DrawLine(detecEdgeOri, detecEdgeOri + Vector3.Cross(Vector3.down, obstacleSurfaceDir), Color.red);
            //go.transform.position = hit.point;
            //go.transform.localScale = Vector3.one*0.1f;
            //go.GetComponent<Collider>().enabled = false;
            //coverPos = hit.normal;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetAimPos(Player.ShoulderSide shoulderSide)
    {
        float sphereCastRaduis = 0.08f;
        List<Vector3> sphereCastPos = new List<Vector3>();
        Vector3 CastDir = obstacleSurfaceDir * -1;
        //SphereCast
        sphereCastPos = GetSphereCast(shoulderSide, sphereCastRaduis, CastDir);
        //Debug
        PlayerDeBuger.dirCast = CastDir;
        PlayerDeBuger.sphereCastPos = sphereCastPos;
        PlayerDeBuger.sphereRaduis = sphereCastRaduis;
        for (int i = 0; i <= sphereCastPos.Count; i++)
        {
            Debug.Log("Cast num =" + i);
            if (i > 0)
            {
                if (Vector3.Dot(sphereCastPos[i], sphereCastPos[i - 1]) * Vector3.Distance(sphereCastPos[i], sphereCastPos[i - 1]) > 1.3f)
                {
                    coverPos = sphereCastPos[i];
                    if (shoulderSide == Player.ShoulderSide.Left)
                    {
                        aimPos = sphereCastPos[i] + (Vector3.Cross(Vector3.up, obstacleSurfaceDir)).normalized;
                    }
                    else if(shoulderSide == Player.ShoulderSide.Right)
                    {
                        aimPos = sphereCastPos[i] + (Vector3.Cross(Vector3.down, obstacleSurfaceDir)).normalized;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }
    private List<Vector3> GetSphereCast(Player.ShoulderSide shoulderSide,float sphereRaduis,Vector3 castDir)
    {
        List<Vector3> sphereCast = new List<Vector3>();
        if (shoulderSide == Player.ShoulderSide.Left)
        {
            detecEdgeDes = detecEdgeOri + Vector3.Cross(Vector3.up, obstacleSurfaceDir);
            for (float i = 0; i <= 1; i = i + sphereRaduis*2)
            {
                Vector3 castPos = Vector3.Lerp(detecEdgeOri, detecEdgeDes, i);
                if (Physics.SphereCast(castPos, sphereRaduis, castDir, out RaycastHit hit, 10, LayerMask.GetMask("Default")))
                {   
                    sphereCast.Add(hit.point);
                }
            }
        }
        else
        {
            detecEdgeDes = detecEdgeOri + Vector3.Cross(Vector3.down, obstacleSurfaceDir);
            for (float i = 0; i <= 1; i = i + sphereRaduis*2)
            {
                Vector3 castPos = Vector3.Lerp(detecEdgeOri, detecEdgeDes, i);
                if (Physics.SphereCast(castPos, sphereRaduis, castDir, out RaycastHit hit, 10, LayerMask.GetMask("Default")))
                {
                    sphereCast.Add(hit.point);
                }
            }
        }
        return sphereCast;
    }

}
