using UnityEngine;

public interface IParkourNodeLeaf : INodeLeaf
{
    protected EdgeObstacleDetection _edgeObstacleDetection { get; set; }
    protected MovementCompoent _movementCompoent { get; set; }
    public const float sphereRaduis = 0.05f;
    public const float sphereDistanceDifferenc = 1;
}
