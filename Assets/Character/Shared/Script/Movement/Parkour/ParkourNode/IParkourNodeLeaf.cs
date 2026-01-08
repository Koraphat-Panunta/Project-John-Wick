using UnityEngine;

public interface IParkourNodeLeaf : INodeLeaf
{
    protected MovementCompoent _movementCompoent { get; set; }
    public const float sphereRaduis = 0.025f;
    public const float sphereDistanceDifferenc = 1;
}
