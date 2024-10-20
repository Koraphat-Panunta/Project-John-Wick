using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPositionEnemy 
{
    public Vector3 coverPivotPos { get; private set; }
    public Vector3 coverPos { get; private set; }
    public Vector3 aimPos { get; private set; }
    public CoverPositionEnemy(Vector3 pivot,Vector3 coverPos,Vector3 aimPos)
    {
        if(Physics.Raycast(pivot,Vector3.down,out RaycastHit hit,10,LayerMask.GetMask("Default")))
        {
            this.coverPivotPos = hit.point;
        }
        if (Physics.Raycast(coverPos, Vector3.down, out RaycastHit hit1,10, LayerMask.GetMask("Default")))
        {
            this.coverPos = hit1.point;
        }
        if (Physics.Raycast(aimPos, Vector3.down, out RaycastHit hit2,10, LayerMask.GetMask("Default")))
        {
            this.aimPos = hit2.point;
        }
        //this.coverPivotPos = pivot;
        //this.coverPos = coverPos;
        //this.aimPos = aimPos;
    }
}
