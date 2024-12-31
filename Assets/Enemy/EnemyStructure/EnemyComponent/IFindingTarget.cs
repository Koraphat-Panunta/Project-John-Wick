using UnityEngine;

public interface IFindingTarget 
{
   
    public LayerMask targetLayer { get; set; }
    public GameObject userObj { get;  }
    public FieldOfView fieldOfView { get; set; }
    public FindingTarget findingTargetComponent { get; set; }
    public Vector3 targetKnewPos { get; set; }
    public void InitailizedFindingTarget();
}
