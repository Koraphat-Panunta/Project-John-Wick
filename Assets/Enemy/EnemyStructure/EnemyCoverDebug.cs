using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyCoverDebug : MonoBehaviour
{
    // Start is called before the first frame update
    public static List<EnemyCoverObstacle> enemyCoverObstacle = new List<EnemyCoverObstacle>();
    public static List<CoverPositionEnemy> coverPositionEnemies = new List<CoverPositionEnemy>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float sphereRaduis = 0.1f;
    private void OnDrawGizmos()
    {
        if (enemyCoverObstacle.Count > 0)
        {
            foreach (EnemyCoverObstacle enemyCover in enemyCoverObstacle)
            {
                Gizmos.color = Color.blue;
                foreach (Vector3 CoverPosPivot in enemyCover.coverPivotPos)
                {
                    Gizmos.DrawSphere(CoverPosPivot, sphereRaduis);
                }
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(enemyCover.collider.bounds.center, sphereRaduis);
                Gizmos.DrawWireCube(enemyCover.collider.bounds.center, enemyCover.collider.bounds.size);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(enemyCover.collider.bounds.center, enemyCover.collider.bounds.center + enemyCover.normalBound * 3.5f);
                foreach (Vector3 CoverPos in enemyCover.coverPos)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(CoverPos, sphereRaduis);
                }
                foreach (Vector3 AimPos in enemyCover.aimPos)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(AimPos, sphereRaduis);
                }
            }
        }
        if (coverPositionEnemies.Count > 0)
        {
            foreach(CoverPositionEnemy coverPosition in coverPositionEnemies)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(coverPosition.aimPos, sphereRaduis);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(coverPosition.coverPos, sphereRaduis);
            }
        }
    }
}
