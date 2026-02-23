using UnityEngine;

public interface IFindingTarget 
{
    public GameObject target { get; set; }
    public FindiAndTrackingTarget findingTargetComponent { get; set; }
    public void InitailizedFindingTarget();
}
