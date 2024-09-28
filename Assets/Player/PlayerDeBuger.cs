using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeBuger : MonoBehaviour
{
    public static List<Vector3> sphereCastPos = new List<Vector3>();
    public static Vector3 dirCast;
    public static float sphereRaduis;
    LayerMask layerMask ;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnDrawGizmos()
    {
        if (sphereCastPos.Count > 0) 
        {
            foreach (Vector3 p in sphereCastPos)
            {
               
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(p,sphereRaduis);  
                
            } 
        }
    }
}
