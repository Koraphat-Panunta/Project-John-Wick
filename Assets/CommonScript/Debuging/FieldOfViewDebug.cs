using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewDebug : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ObjView;
    public static Vector3 Pos;
    public static float Radias;
    public Vector3 DirecviewLeft;
    public Vector3 DirecviewRight;

    void Start()
    {
        Radias = 137;
     
    }

    // Update is called once per frame
    void Update()
    {
        DirecviewLeft = GetVectorView(ObjView.transform.rotation.eulerAngles.y, Radias/2);
        DirecviewRight = GetVectorView(ObjView.transform.rotation.eulerAngles.y, -Radias / 2);
        Pos = new Vector3(ObjView.transform.position.x, ObjView.transform.position.y + 1, ObjView.transform.position.z);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Pos,Pos+DirecviewLeft);
        Gizmos.DrawLine(Pos,Pos+DirecviewRight);

    }
    private Vector3 GetVectorView(float eulerY,float radians)
    {
        radians += eulerY;
        return new Vector3(Mathf.Sin(radians * Mathf.Deg2Rad), 0, Mathf.Cos(radians * Mathf.Deg2Rad))*20;
    }
}
