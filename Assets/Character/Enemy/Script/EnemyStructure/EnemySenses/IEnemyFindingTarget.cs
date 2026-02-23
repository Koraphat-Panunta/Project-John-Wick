using UnityEngine;

public interface IEnemyFindingTarget 
{
    public FindiAndTrackingTarget enemyLookForPlayer { get; set; }
    public FieldOfView fieldOfView { get; set; }
    public Transform targetPos { get; set; }
    public Transform refPoint { get; set; }
    public LayerMask targetMask { get; set; }
   
    public void InitailizedFindingTargetComponent();
}
