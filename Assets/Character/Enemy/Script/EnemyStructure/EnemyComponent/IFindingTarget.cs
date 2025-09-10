using UnityEngine;

public interface IFindingTarget 
{
    public GameObject target { get; set; }
    public FindingTarget findingTargetComponent { get; set; }
    public void InitailizedFindingTarget();
}
