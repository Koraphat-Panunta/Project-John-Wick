using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CameraManagerNode : INodeManager
{
    public INodeLeaf curNodeLeaf { get; set; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public CameraController cameraController { get;protected set; }

    public CameraManagerNode(CameraController cameraController)
    {
        this.cameraController = cameraController;
        nodeManagerBehavior = new NodeManagerBehavior();

        InitailizedNode();
    }

    public void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }
    public void UpdateNode()
    {
       nodeManagerBehavior.UpdateNode(this);
    }

    public CameraSelectorNode cameraPlayerBasedSelector { get; protected set; }
    public CameraAimDownSightViewNodeLeaf cameraAimDownSightViewNodeLeaf { get; protected set; }
    public CameraCrouchViewNodeLeaf cameraCrouchViewNodeLeaf { get; protected set; }
    public CameraStandViewNodeLeaf cameraStandViewNodeLeaf { get; protected set; }
    public CameraRestNodeLeaf cameraRestNodeLeaf { get; protected set; }

    public void InitailizedNode()
    {
        startNodeSelector = new CameraSelectorNode(this.cameraController, () => true);

        cameraPlayerBasedSelector = new CameraSelectorNode(this.cameraController,()=> this.cameraController.Player != null);

        cameraAimDownSightViewNodeLeaf = new CameraAimDownSightViewNodeLeaf(this.cameraController,
            ()=> this.cameraController.isZooming);

        cameraCrouchViewNodeLeaf = new CameraCrouchViewNodeLeaf(this.cameraController,
            ()=> this.cameraController.curStance == Player.PlayerStance.crouch);

        cameraStandViewNodeLeaf = new CameraStandViewNodeLeaf(this.cameraController,
            () => this.cameraController.curStance == Player.PlayerStance.stand || true);

        cameraRestNodeLeaf = new CameraRestNodeLeaf(this.cameraController, () => true);

        startNodeSelector.AddtoChildNode(cameraPlayerBasedSelector);
        startNodeSelector.AddtoChildNode(cameraRestNodeLeaf);

        cameraPlayerBasedSelector.AddtoChildNode(cameraAimDownSightViewNodeLeaf);
        cameraPlayerBasedSelector.AddtoChildNode(cameraCrouchViewNodeLeaf);
        cameraPlayerBasedSelector.AddtoChildNode(cameraStandViewNodeLeaf);


        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
    }
}
public class CameraRestNodeLeaf : CameraNodeLeaf
{
    public CameraRestNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
    }
}
