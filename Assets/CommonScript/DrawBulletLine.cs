using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBulletLine : MonoBehaviour
{
    public static List<Vector3> bulletHitPos = new List<Vector3>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(bulletHitPos.Count);
    }
    private void OnDrawGizmos()
    {
        foreach (Vector3 bullet in bulletHitPos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(bullet, 0.1f);
        }
    }
}