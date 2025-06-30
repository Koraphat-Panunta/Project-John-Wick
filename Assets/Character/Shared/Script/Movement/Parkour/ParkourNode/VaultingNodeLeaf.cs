using System;
using System.Collections.Generic;
using UnityEngine;

public class VaultingNodeLeaf : PlayerStateNodeLeaf, IParkourNodeLeaf
{
    EdgeObstacleDetection IParkourNodeLeaf._edgeObstacleDetection { get => this.edgeObstacleDetection; set => this.edgeObstacleDetection = value; }
    IMovementCompoent IParkourNodeLeaf._movementCompoent { get => movementCompoent; set => movementCompoent = value; }
    private IMovementCompoent movementCompoent;
    private EdgeObstacleDetection edgeObstacleDetection;
    private VaultingParkourScriptableObject vaultingParkourScriptableObject { get; set; }

    private Vector3 ct1;
    private Vector3 ct2;
    private Vector3 exitPos;

    Transform parkourAbleTransform => player.transform;

    public VaultingNodeLeaf(Player player, Func<bool> preCondition,IMovementCompoent movementCompoent,VaultingParkourScriptableObject vaultingParkourScriptableObject) : base(player, preCondition)
    {
        this.movementCompoent = movementCompoent;
        edgeObstacleDetection = new EdgeObstacleDetection();
        vaultingParkourScriptableObject = new VaultingParkourScriptableObject();
    }

    public override bool Precondition()
    {
        if(base.Precondition() == false)
            return false;

        return false;
    }


}
