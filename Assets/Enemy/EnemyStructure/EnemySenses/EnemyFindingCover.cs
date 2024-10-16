using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class EnemyFindingCover 
{
    private List<EnemyCoverObstacle> enemyCoverObstacles;
    public Vector3 CoverPos;
    public Vector3 AimPos;
    private float raduisDetection = 12;
    private float deltaDistance_RayCastPosToTransform;
    public EnemyCoverObstacle coverObstacle;
    public EnemyFindingCover()
    {
        enemyCoverObstacles = new List<EnemyCoverObstacle>();
    }
    public bool FindingCover(Enemy enemy)
    {
        deltaDistance_RayCastPosToTransform = Vector3.Distance(enemy.transform.position, 
            new Vector3(enemy.transform.position.x, enemy.rayCastPos.position.y, enemy.transform.position.z));//จุดห่างระหว่างEnemy Transform กับ RayCastPos ในแกนy

        // Step 1 เช็คObstacleว่าอยู่ในระยะและมองเห็นได้ //
        Collider[] col = Physics.OverlapSphere(enemy.transform.position, raduisDetection, LayerMask.GetMask("Default"));
        if(col.Length <= 0)
        {
            return false;
        }
        for(int i = 0; i <= col.Length - 1; i++)
        {
            if (Physics.Raycast(enemy.rayCastPos.position, (col[i].bounds.center - enemy.rayCastPos.position).normalized, 
                out RaycastHit hitInfo,Vector3.Distance(enemy.rayCastPos.position, col[i].bounds.center),LayerMask.GetMask("Default")))
            {
                if(hitInfo.collider == col[i])
                {
                    enemyCoverObstacles.Add(new EnemyCoverObstacle(col[i], hitInfo));
                }
            }
        }
        if(enemyCoverObstacles.Count <= 0)
        {
            return false;
        }
        ///////////////////////////////////////
        
        // Step 2 เช็คทิศทางของที่กำบัง //
        for(int i = 0; i <= enemyCoverObstacles.Count - 1; i++)
        {
            if (Vector3.Dot(enemyCoverObstacles[i].hitInfo.normal.normalized*-1, 
                (enemy.Target.transform.position - enemyCoverObstacles[i].collider.bounds.center).normalized)>0) 
            {
                
            }
            else
            {
                enemyCoverObstacles.RemoveAt(i);
            }
        }
        if (enemyCoverObstacles.Count <= 0)
        {
            return false;
        }
        ///////////////////////////////////////
        
        // Step 3 Sphere Casr line
        float sphereCastRaduis = 0.02f;
        for (int i = 0; i <= enemyCoverObstacles.Count - 1; i++)
        {
            Vector3 edgeDetecPos = new Vector3(enemyCoverObstacles[i].hitInfo.point.x, enemy.rayCastPos.position.y, enemyCoverObstacles[i].hitInfo.point.z) 
                + enemyCoverObstacles[i].hitInfo.normal*0.4f;
            List<Vector3> sphereCast = new List<Vector3>();

            //Left Side
            bool coverIsFound = false;
            sphereCast = new ObstacleDetection().GetSphereCast(sphereCastRaduis, enemyCoverObstacles[i].hitInfo.normal * -1, edgeDetecPos,
               edgeDetecPos+Vector3.Cross(Vector3.up, enemyCoverObstacles[i].hitInfo.normal) * 9);

            for (int n = 0; n <= sphereCast.Count - 1; n++)
            {
                if (n > 0)
                {
                    if (Vector3.Distance(sphereCast[n], sphereCast[n - 1]) > sphereCastRaduis * 2 + 0.1f)
                    {
                        //Debug.Log("Egde found");
                        Vector3 coverPos;
                        Vector3 aimPos;
                        coverPos = sphereCast[n - 1] - (Vector3.Cross(Vector3.up, enemyCoverObstacles[i].hitInfo.normal)).normalized * 0.4f;
                        aimPos = sphereCast[n - 1] + (Vector3.Cross(Vector3.up, enemyCoverObstacles[i].hitInfo.normal)).normalized * 0.2f;
                        if (Physics.Raycast(coverPos, (enemy.Target.transform.position - coverPos).normalized,out RaycastHit hit,
                            Vector3.Distance(coverPos, enemy.Target.transform.position), LayerMask.GetMask("Default")))
                        {
                            if (Vector3.Distance(hit.point, enemy.Target.transform.position) > 0.1f)
                            {
                                if (Physics.Raycast(aimPos, (enemy.Target.transform.position - aimPos).normalized, out RaycastHit hitAim,
                                    Vector3.Distance(aimPos, enemy.Target.transform.position), LayerMask.GetMask("Default"))) 
                                {
                                    if(Vector3.Distance(hitAim.point, enemy.Target.transform.position) < 0.1f)
                                    {
                                        coverIsFound = true;
                                        enemyCoverObstacles[i].coverPos = coverPos;
                                        enemyCoverObstacles[i].aimPos = aimPos;
                                    }
                                }
                            }
                        }
                        break;
                    }
                    else
                    {
                        Debug.Log("Egde not found");
                    }
                }
            }
            //Right Side
            if(coverIsFound == false)
            {
                sphereCast = new ObstacleDetection().GetSphereCast(sphereCastRaduis, enemyCoverObstacles[i].hitInfo.normal * -1, edgeDetecPos,
                   edgeDetecPos+ Vector3.Cross(Vector3.down, enemyCoverObstacles[i].hitInfo.normal) * 9);

                for (int n = 0; n <= sphereCast.Count - 1; n++)
                {
                    if (n > 0)
                    {
                        if (Vector3.Distance(sphereCast[n], sphereCast[n - 1]) > sphereCastRaduis * 2 + 0.1f)
                        {
                            //Debug.Log("Egde found");
                            Vector3 coverPos;
                            Vector3 aimPos;
                            coverPos = sphereCast[n - 1] - (Vector3.Cross(Vector3.down, enemyCoverObstacles[i].hitInfo.normal)).normalized * 0.4f;
                            aimPos = sphereCast[n - 1] + (Vector3.Cross(Vector3.down, enemyCoverObstacles[i].hitInfo.normal)).normalized * 0.2f;
                            if (Physics.Raycast(coverPos, (enemy.Target.transform.position - coverPos).normalized, out RaycastHit hit,
                            Vector3.Distance(coverPos, enemy.Target.transform.position), LayerMask.GetMask("Default")))
                            {
                                if (Vector3.Distance(hit.point, enemy.Target.transform.position) > 0.1f)
                                {
                                    if (Physics.Raycast(aimPos, (enemy.Target.transform.position - aimPos).normalized, out RaycastHit hitAim,
                                        Vector3.Distance(aimPos, enemy.Target.transform.position), LayerMask.GetMask("Default")))
                                    {
                                        if (Vector3.Distance(hitAim.point, enemy.Target.transform.position) < 0.1f)
                                        {
                                            coverIsFound = true;
                                            enemyCoverObstacles[i].coverPos = coverPos;
                                            enemyCoverObstacles[i].aimPos = aimPos;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        else
                        {
                            Debug.Log("Egde not found");
                        }
                    }
                }
            }
            if(coverIsFound == false)
            {
                enemyCoverObstacles.RemoveAt(i);
            }
        }
        if (enemyCoverObstacles.Count <= 0)
        {
            return false;
        }
        // Step 4 Check there is freindly in Cover //
        Collider[] enemyOther = Physics.OverlapSphere(enemy.transform.position, raduisDetection, LayerMask.GetMask("Enemy"));
        for (int i = 0; i <= enemyCoverObstacles.Count - 1; i++)
        {
            foreach (Collider collider in enemyOther)
            {
                if (collider.TryGetComponent<ChestBodyPart>(out ChestBodyPart e))
                {
                    if (e.enemy.GetHP() > 0 && e.enemy != enemy)
                    {
                        if (Vector3.Distance(e.enemy.transform.position, enemyCoverObstacles[i].coverPos - Vector3.down * deltaDistance_RayCastPosToTransform) < 0.5f
                            || Vector3.Distance(e.enemy.transform.position, enemyCoverObstacles[i].aimPos - Vector3.down * deltaDistance_RayCastPosToTransform) <0.5f)
                        {
                            enemyCoverObstacles.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
        if (enemyCoverObstacles.Count > 0)
        {
            foreach(EnemyCoverObstacle enemyCoverObstacle in enemyCoverObstacles)
            {
                CoverPos = enemyCoverObstacle.coverPos;
                AimPos = enemyCoverObstacle.aimPos;
                this.coverObstacle = enemyCoverObstacle;
                break;
            }
        }
        if (enemyCoverObstacles.Count <= 0)
        {
            return false;
        }
        return false;
    }

}
