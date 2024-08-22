using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDebuger : MonoBehaviour
{
    public static List<Vector3> _markPos = new List<Vector3>();
    public List<Vector3> Pos = new List<Vector3>();
    public static Vector3 curPos;
    public static Vector3 cp1;
    public static Vector3 cp2;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Pos = _markPos; 
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _markPos.Count-1; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_markPos[i], _markPos[i+1]);
        }
        for(int i = 0; i < _markPos.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_markPos[i], .5f);
        }
        //for(int i =0; i < agent.path.corners.Length; i++)
        //{

        //}
        
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(curPos, .6f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(cp1, .6f);
        Gizmos.DrawSphere(cp2, .6f);
    }
}
