using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static EnemyCoverObstacle;
using static Player;

public class EnemyFindingCover 
{
    private List<EnemyCoverObstacle> enemyCoverObstacles;
    private float raduisDetection = 12;

    public List<CoverPositionEnemy> coverPositionEnemies = new List<CoverPositionEnemy>();
    public CoverPositionEnemy coverPositionEnemy;
    public EnemyFindingCover()
    {
        enemyCoverObstacles = new List<EnemyCoverObstacle>();
        enemyCoverObstacles.Clear();
        coverPositionEnemy = null;
    }
    public bool FindingCover(Enemy enemy )
    {
        // Step 1 เช็คObstacleว่าอยู่ในระยะและมองเห็นได้ //
        enemyCoverObstacles.Clear();
        coverPositionEnemies.Clear();
        Collider[] col = Physics.OverlapSphere(enemy.transform.position, raduisDetection, LayerMask.GetMask("Default"));
        if (col.Length <= 0)
        {
            return false;
        }
        for(int i =0;i<= col.Length - 1; i++)
        {
            if (col[i].gameObject.layer == LayerMask.GetMask("Enemy"))
            {
                continue;
            }
            if (col[i].bounds.size.magnitude < 1.5f)
            {
                continue;
            }
            Vector3 CastDir = (new Vector3(col[i].bounds.center.x, enemy.rayCastPos.position.y, col[i].bounds.center.z)-enemy.rayCastPos.position).normalized;
            if (Physics.Raycast(enemy.rayCastPos.position, CastDir,out RaycastHit hitInfo, Vector3.Distance(col[i].bounds.center, enemy.rayCastPos.position), LayerMask.GetMask("Default")))
            {
                if (hitInfo.collider == col[i] &&Vector3.Dot(Vector3.up, hitInfo.normal) < 0.9f)
                {
                    //Debug.DrawLine(enemy.rayCastPos.position, hitInfo.point,Color.green);
                    if (col[i].CompareTag("IgnoreCover") == false)
                    {
                        enemyCoverObstacles.Add(new EnemyCoverObstacle(col[i], hitInfo, enemy.transform.position));
                    }
                }
            }
        }
        EnemyCoverDebug.enemyCoverObstacle = enemyCoverObstacles;
        if(enemyCoverObstacles.Count <= 0)
        {
            return false;
        }
        
        // Step 2 เช็คจุดกำบังและตำแหน่งTarget
        if (enemyCoverObstacles.Count > 0)
        {
            Vector3 targetPos = enemy.Target.transform.position;
            foreach (EnemyCoverObstacle enemyCoverObstacle in enemyCoverObstacles)
            {
                for (int i = 0; i < enemyCoverObstacle.coverPivotPos.Length; i++)
                {
                    Vector3 CoverPos = enemyCoverObstacle.coverPos[i];
                    Vector3 AimPos = enemyCoverObstacle.aimPos[i];
                    if(Physics.Raycast(CoverPos, (targetPos - CoverPos).normalized,out RaycastHit hit, Vector3.Distance(CoverPos, targetPos), LayerMask.GetMask("Default")))
                    {
                        Debug.DrawLine(CoverPos, hit.point, Color.green);
                        if (Physics.Raycast(AimPos, (targetPos - AimPos).normalized,out RaycastHit hitInfo, Vector3.Distance(AimPos, targetPos), LayerMask.GetMask("Default")+enemy.targetMask))
                        {
                            if (hitInfo.collider.gameObject.layer == enemy.targetMask)
                            {
                                coverPositionEnemies.Add(new CoverPositionEnemy(enemyCoverObstacle.coverPivotPos[i], CoverPos, AimPos));
                                Debug.DrawLine(CoverPos, hit.point, Color.red);
                            }
                            else
                            {
                                Debug.DrawLine(CoverPos, hit.point, Color.red);
                            }
                        }
                        else
                        {
                            coverPositionEnemies.Add(new CoverPositionEnemy(enemyCoverObstacle.coverPivotPos[i], CoverPos, AimPos));
                            Debug.DrawLine(CoverPos, hit.point, Color.red);
                        }
                    }
                    else
                    {
                        //Debug.DrawLine(CoverPos, (targetPos - CoverPos).normalized* Vector3.Distance(CoverPos, targetPos), Color.green);
                    }
                }
            }
        }
        if (coverPositionEnemies.Count <= 0)
        {
            return false;
        }
        Debug.Log("Step 2 coverPositionEnemies.Count " + coverPositionEnemies.Count);
        // Step 3 เช็คCoverPosและAimPosOverlap
        for (int i = 0; i <= coverPositionEnemies.Count-1 ; i++)
        {
            Vector3 CoverPos = coverPositionEnemies[i].coverPos;
            Vector3 AimPos = coverPositionEnemies[i].aimPos;
            Vector3 CoverPosPivot = coverPositionEnemies[i].coverPivotPos;
            Collider[] colnearBy = Physics.OverlapSphere(coverPositionEnemies[i].coverPos, 1f, LayerMask.GetMask("Default"));
            foreach(Collider collider in colnearBy)
            {
                if (coverPositionEnemies.Count > 0)
                {
                    if (collider.TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
                    {
                        if (Convert.ToInt32(boxCollider.transform.rotation.eulerAngles.x) % 360 != 0 ||Convert.ToInt32(boxCollider.transform.rotation.eulerAngles.z) % 360 != 0)
                        {
                            Bounds bounds = collider.bounds;
                            if (bounds.Contains(CoverPos) || bounds.Contains(AimPos)||bounds.Contains(CoverPosPivot))
                            {
                                Debug.Log("I =" + i);
                                Debug.Log("coverPositionEnemies count" + coverPositionEnemies.Count);
                                coverPositionEnemies.RemoveAt(i);
                                break;
                            }
                        }
                        else
                        {
                            Vector3 closestPointToCoverPos = collider.ClosestPoint(CoverPos);
                            Vector3 closestPointToAimPos = collider.ClosestPoint(AimPos);
                            if (closestPointToCoverPos == CoverPos || closestPointToAimPos == AimPos|| closestPointToAimPos == CoverPosPivot)
                            {
                                Debug.Log("I =" + i);
                                Debug.Log("coverPositionEnemies count" + coverPositionEnemies.Count);
                                coverPositionEnemies.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    else
                    {
                        Bounds bounds = collider.bounds;
                        if (bounds.Contains(CoverPos) || bounds.Contains(AimPos) || bounds.Contains(CoverPosPivot))
                        {
                            Debug.Log("I =" + i);
                            Debug.Log("coverPositionEnemies count" + coverPositionEnemies.Count);
                            coverPositionEnemies.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        Debug.Log("Step 3 coverPositionEnemies.Count " + coverPositionEnemies.Count);
        if (coverPositionEnemies.Count <= 0)
        {
            return false;
        }
        EnemyCoverDebug.coverPositionEnemies = coverPositionEnemies;
        // Step 4 เช็คจุดกำบังมีใครไปใช้แล้วหรือ
        for (int i = 0; i <= coverPositionEnemies.Count - 1; i++)
        {
            Collider[] nearColEnemy = Physics.OverlapSphere(coverPositionEnemies[i].coverPos, 1f, LayerMask.GetMask("Enemy"));
            if (nearColEnemy.Length > 0)
            {
                foreach (Collider n in nearColEnemy)
                {
                    Enemy enemyInCol = n.GetComponent<BodyPart>().enemy;
                    if (enemyInCol.GetHP() > 0 && enemyInCol != enemy)
                    {
                        Vector3 coverPos = coverPositionEnemies[i].coverPos;
                        if (Vector3.Distance(enemyInCol.transform.position, new Vector3(coverPos.x, enemyInCol.transform.position.y, coverPos.z)) < 0.13f)
                        {
                            coverPositionEnemies.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
        Debug.Log("Step 4 coverPositionEnemies.Count " + coverPositionEnemies.Count);
        if (coverPositionEnemies.Count <= 0)
        {
            return false ;
        }
        else
        {
            foreach (CoverPositionEnemy coverPositionEnemy in coverPositionEnemies)
            {
                if(this.coverPositionEnemy == null)
                {
                    this.coverPositionEnemy = coverPositionEnemy;
                }
                Vector3 DirTTE = enemy.Target.transform.position.normalized- enemy.transform.position.normalized;
                Vector3 DirTTC = enemy.Target.transform.position.normalized - coverPositionEnemy.coverPos.normalized;

                Vector3 DirTTc = enemy.Target.transform.position.normalized - this.coverPositionEnemy.coverPos.normalized;
                if (Vector3.Dot(DirTTC, DirTTE)>Vector3.Dot(DirTTE,DirTTc))
                {
                    this.coverPositionEnemy = coverPositionEnemy;
                }
            }
            EnemyCoverDebug.CurcoverPositionEnemy = this.coverPositionEnemy;
            return true ;
        }

    }

}
