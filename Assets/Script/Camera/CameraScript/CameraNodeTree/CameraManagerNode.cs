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

    public CameraSelectorNode cameraControlAbleSelector { get; protected set; }
    public CameraAimDownSightViewNodeLeaf cameraAimDownSightViewNodeLeaf { get; protected set; }
    public CameraCrouchViewNodeLeaf cameraCrouchViewNodeLeaf { get; protected set; }
    public CameraStandViewNodeLeaf cameraStandViewNodeLeaf { get; protected set; }

    public void InitailizedNode()
    {
        startNodeSelector = new CameraSelectorNode(this.cameraController, () => true);

        cameraControlAbleSelector = new CameraSelectorNode(this.cameraController,()=> true);

        cameraAimDownSightViewNodeLeaf = new CameraAimDownSightViewNodeLeaf(this.cameraController,
            ()=> this.cameraController.isZooming);

        cameraCrouchViewNodeLeaf = new CameraCrouchViewNodeLeaf(this.cameraController,
            ()=> this.cameraController.curStance == Player.PlayerStance.crouch);

        cameraStandViewNodeLeaf = new CameraStandViewNodeLeaf(this.cameraController,
            () => this.cameraController.curStance == Player.PlayerStance.stand || true);

        startNodeSelector.AddtoChildNode(cameraControlAbleSelector);

        cameraControlAbleSelector.AddtoChildNode(cameraAimDownSightViewNodeLeaf);
        cameraControlAbleSelector.AddtoChildNode(cameraCrouchViewNodeLeaf);
        cameraControlAbleSelector.AddtoChildNode(cameraStandViewNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
    }
}
