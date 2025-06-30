using UnityEngine;

public interface IParkourNodeLeaf : INodeLeaf
{
    protected EdgeObstacleDetection _edgeObstacleDetection { get; set; }
    protected IMovementCompoent _movementCompoent { get; set; }
}
