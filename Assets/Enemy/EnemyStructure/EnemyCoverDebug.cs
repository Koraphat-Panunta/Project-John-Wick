using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverDebug : MonoBehaviour
{
    // Start is called before the first frame update
    public static List<EnemyCoverObstacle> enemyCoverObstacle = new List<EnemyCoverObstacle>();
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
        if(enemyCoverObstacle.Count>0)
        {
            foreach(EnemyCoverObstacle enemyCover in enemyCoverObstacle)
            {
                Gizmos.color = Color.blue;
                foreach(Vector3 CoverPosPivot in enemyCover.coverPivotPos)
                {
                    Gizmos.DrawSphere(CoverPosPivot, sphereRaduis);
                }
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(enemyCover.collider.bounds.center, sphereRaduis);
            }
        }
    }
}
