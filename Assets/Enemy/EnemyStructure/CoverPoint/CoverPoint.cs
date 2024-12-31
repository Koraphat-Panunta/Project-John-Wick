using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoverPoint : MonoBehaviour
{
    [SerializeField] public Transform coverPos;

    public Vector3 coverDir;
    public FieldOfView fieldOfView;
    public ICoverUseable coverUser;
    public abstract float fovDistance { get; set; }
    public abstract float fovAngleDegrees { get; set; }
    protected virtual void Start()
    {
        this.coverDir = coverPos.transform.forward;
    }
    public abstract void TakeThisCover(ICoverUseable coverUser);
    public abstract void OffThisCover();
    public abstract bool CheckingTargetInCoverView(ICoverUseable coverUser,LayerMask targetMask, Transform peekPos);
   
  
  
}
