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
        this.coverPivotPos = pivot;
        this.coverPos = coverPos;
        this.aimPos = aimPos;
    }
}
