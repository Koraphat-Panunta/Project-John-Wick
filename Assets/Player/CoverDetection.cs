using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverDetection 
{
    public Vector3 coverPos { get;private set; }
    public Vector3 aimPos { get; private set; }
    private Vector3 detecEdgeOri;
    private Vector3 detecEdgeDes;
    public Vector3 obstacleSurfaceDir { get; protected set; }
    public CoverDetection() 
    {
        
    }
   
    public bool CheckingObstacleToward(Vector3 oriPos,Vector3 dir)
    {
        LayerMask decObstacle = LayerMask.GetMask("Default");
        if (Physics.Raycast(oriPos,dir,out RaycastHit hit, 1f, decObstacle))
        {
            obstacleSurfaceDir = hit.normal;
            detecEdgeOri = hit.point + (obstacleSurfaceDir * 0.4f);
            Debug.DrawLine(hit.point, detecEdgeOri, Color.gray);
           
            Debug.DrawLine(detecEdgeOri, detecEdgeOri + Vector3.Cross(Vector3.up, obstacleSurfaceDir), Color.blue);
            Debug.DrawLine(detecEdgeOri, detecEdgeOri + Vector3.Cross(Vector3.down, obstacleSurfaceDir), Color.red);
            
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetAimPos(Player.ShoulderSide shoulderSide)
    {
        float sphereCastRaduis = 0.02f;
        List<Vector3> sphereCastPos = new List<Vector3>();
        Vector3 CastDir = obstacleSurfaceDir * -1;
        //SphereCast
        sphereCastPos = GetSphereCast(shoulderSide, sphereCastRaduis, CastDir);
        //Debug
        PlayerDeBuger.dirCast = CastDir;
        PlayerDeBuger.sphereCastPos = sphereCastPos;
        PlayerDeBuger.sphereRaduis = sphereCastRaduis;
        for (int i = 0; i <= sphereCastPos.Count-1; i++)
        {
            Debug.Log("Cast num ="+ i);
            Debug.Log("Cast count =" + sphereCastPos.Count);
            if (i > 0)
            {
                if (Vector3.Distance(sphereCastPos[i], sphereCastPos[i - 1]) > sphereCastRaduis*2+0.1f)
                {
                    if (shoulderSide == Player.ShoulderSide.Left)
                    {
                        coverPos = sphereCastPos[i - 1] - (Vector3.Cross(Vector3.up, obstacleSurfaceDir)).normalized*0.5f;
                        aimPos = sphereCastPos[i-1] - (Vector3.Cross(Vector3.up, obstacleSurfaceDir)).normalized*0.1f;
                        PlayerDeBuger.AimPos = aimPos;
                        PlayerDeBuger.CoverPos = coverPos;
                    }
                    else if(shoulderSide == Player.ShoulderSide.Right)
                    {
                        coverPos = sphereCastPos[i - 1] - (Vector3.Cross(Vector3.down, obstacleSurfaceDir)).normalized * 0.5f;
                        aimPos = sphereCastPos[i - 1] - (Vector3.Cross(Vector3.down, obstacleSurfaceDir)).normalized * 0.1f;
                        PlayerDeBuger.AimPos = aimPos;
                        PlayerDeBuger.CoverPos = coverPos;
                    }
                    return true;
                }
                else
                {
                    Debug.Log("Egde not found");
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
            sphereCast = new ObstacleDetection().GetSphereCast(sphereRaduis, castDir,detecEdgeOri,detecEdgeDes);
        }
        else
        {
            detecEdgeDes = detecEdgeOri + Vector3.Cross(Vector3.down, obstacleSurfaceDir);
            sphereCast = new ObstacleDetection().GetSphereCast(sphereRaduis, castDir, detecEdgeOri, detecEdgeDes);
        }
        return sphereCast;
    }

}
