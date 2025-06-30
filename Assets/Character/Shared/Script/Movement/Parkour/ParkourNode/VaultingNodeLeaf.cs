using System;
using System.Collections.Generic;
using UnityEngine;

public class VaultingNodeLeaf : PlayerStateNodeLeaf, IParkourNodeLeaf
{
    EdgeObstacleDetection IParkourNodeLeaf._edgeObstacleDetection { get => this.edgeObstacleDetection; set => this.edgeObstacleDetection = value; }
    IMovementCompoent IParkourNodeLeaf._movementCompoent { get => movementCompoent; set => movementCompoent = value; }
    private IMovementCompoent movementCompoent;
    private EdgeObstacleDetection edgeObstacleDetection;
    public VaultingNodeLeaf(Player player, Func<bool> preCondition,IMovementCompoent movementCompoent) : base(player, preCondition)
    {
        this.movementCompoent = movementCompoent;
        edgeObstacleDetection = new EdgeObstacleDetection();
    }

    public override bool Precondition()
    {

        return base.Precondition();
    }


}
