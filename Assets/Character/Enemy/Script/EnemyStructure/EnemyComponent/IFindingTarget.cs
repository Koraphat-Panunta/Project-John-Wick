using UnityEngine;

public interface IFindingTarget 
{
    public FindingTarget findingTargetComponent { get; set; }
    public void InitailizedFindingTarget();
}
