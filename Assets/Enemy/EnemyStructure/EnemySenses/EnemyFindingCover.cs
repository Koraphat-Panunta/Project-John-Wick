using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class EnemyFindingCover 
{
    private List<EnemyCoverObstacle> enemyCoverObstacles;
    private float raduisDetection = 12;
    public EnemyCoverObstacle coverObstacle;
    public EnemyFindingCover()
    {
        enemyCoverObstacles = new List<EnemyCoverObstacle>();
    }
    public bool FindingCover(Enemy enemy)
    {
        // Step 1 เช็คObstacleว่าอยู่ในระยะและมองเห็นได้ //
        Collider[] col = Physics.OverlapSphere(enemy.transform.position, raduisDetection, LayerMask.GetMask("Default"));
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
                    Debug.DrawLine(enemy.rayCastPos.position, hitInfo.point,Color.green);
                    enemyCoverObstacles.Add(new EnemyCoverObstacle(col[i], hitInfo));
                }
            }
        }
        if (enemyCoverObstacles.Count > 0)
        {
            EnemyCoverDebug.enemyCoverObstacle.Clear();
            EnemyCoverDebug.enemyCoverObstacle = this.enemyCoverObstacles;
        }
        else
        {
            return false;
        }
        // Step 2 เช็คObstacleว่าอยู่ในจุดที่ถูกต้อง //
        for(int i =0;i<= enemyCoverObstacles.Count - 1; i++)
        {
            foreach(Vector3 CoverPos in enemyCoverObstacles[i].coverPivotPos)
            {
                Ray ray = new Ray(CoverPos, (enemy.rayCastPos.position - CoverPos).normalized);
                
                if (Physics.Raycast(ray,out RaycastHit hitInfo, Vector3.Distance(CoverPos, enemy.rayCastPos.position),LayerMask.GetMask("Default")))
                {
                    Debug.DrawLine(CoverPos, hitInfo.point, Color.gray);
                }
                else
                {
                    Debug.DrawLine(CoverPos, enemy.rayCastPos.position, Color.blue);
                }
            }
        }
        if (enemyCoverObstacles.Count > 0)
        {
            //EnemyCoverDebug.enemyCoverObstacle.Clear();
            //EnemyCoverDebug.enemyCoverObstacle = this.enemyCoverObstacles;
            return true;
        }
        else
        {
            return false;
        }
    }

}
