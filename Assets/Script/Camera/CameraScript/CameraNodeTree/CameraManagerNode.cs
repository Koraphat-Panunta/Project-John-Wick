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

    public void InitailizedNode()
    {
        startNodeSelector = new CameraSelectorNode(this.cameraController, () => true);

        cameraControlAbleSelector = new CameraSelectorNode(this.cameraController,()=> true);

        startNodeSelector.AddtoChildNode(cameraControlAbleSelector);
    }
}
