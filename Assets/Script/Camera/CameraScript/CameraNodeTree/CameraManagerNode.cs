using System;
using UnityEngine;

public class CameraManagerNode:IDebuggedAble
{
    public INode curNode { get; protected set; }
    public NodeSelector startNodeSelector { get ; set ; }
    public CameraController cameraController { get;protected set; }

    public CameraManagerNode(CameraController cameraController)
    {
        this.cameraController = cameraController;
        InitailizedNode();
    }

    public void FixedUpdateNode()
    {
        if (curNode != null && curNode is INodeLeaf curNodeLeaf)
            curNodeLeaf.FixedUpdateNode();
    }
    public void UpdateNode()
    {
        if (curNode is INodeLeaf curNodeLeaf 
            && curNodeLeaf.IsReset())
        {
            curNodeLeaf.Exit();
            curNode = null;
            this.SearchingNewNode();
        }

        if (curNode != null && curNode is INodeLeaf nodeLeaf)
            nodeLeaf.UpdateNode();
    }

    public NodeSelector cameraPlayerBasedSelector { get; protected set; }
    public CameraAimDownSightViewNodeLeaf cameraAimDownSightViewNodeLeaf { get; protected set; }
    public CameraSprintViewNodeLeaf cameraSprintViewNodeLeaf { get; protected set; }
    public CameraCrouchViewNodeLeaf cameraCrouchViewNodeLeaf { get; protected set; }
    public CameraStandViewNodeLeaf cameraStandViewNodeLeaf { get; protected set; }
    public CameraGunFuWeaponDisarmViewNodeLeaf cameraGunFuWeaponDisarmViewNodeLeaf { get; protected set; }
    public CameraGunFuExecuteOnGroundNodeLeaf cameraGunFuExecuteOnGroundNodeLeaf { get; protected set; }
    public CameraGunFuHitViewNodeLeaf cameraGunFuHitViewNodeLeaf { get; protected set; }
    public CameraRestNodeLeaf cameraRestNodeLeaf { get; protected set; }

    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true,nameof(startNodeSelector));

        cameraPlayerBasedSelector = new NodeSelector(
            ()=> this.cameraController.player != null && this.cameraController.player.gameObject.activeSelf
            ,nameof(cameraPlayerBasedSelector));

        cameraAimDownSightViewNodeLeaf = new CameraAimDownSightViewNodeLeaf(this.cameraController,
            ()=> this.cameraController.isZooming);

        cameraSprintViewNodeLeaf = new CameraSprintViewNodeLeaf(this.cameraController,
            () => this.cameraController.player.isSprint || this.cameraController.player.playerStateNodeManager.curNodeLeaf is PlayerDodgeRollStateNodeLeaf);

        cameraCrouchViewNodeLeaf = new CameraCrouchViewNodeLeaf(this.cameraController,
            ()=> this.cameraController.curStance == Player.PlayerStance.crouch);
        cameraGunFuWeaponDisarmViewNodeLeaf = new CameraGunFuWeaponDisarmViewNodeLeaf(this.cameraController,
            () => cameraController.isWeaponDisarm);
        cameraGunFuExecuteOnGroundNodeLeaf = new CameraGunFuExecuteOnGroundNodeLeaf(this.cameraController,
           cameraController.cameraExecuteScriptableObject,
            () => this.cameraController.player.playerStateNodeManager.curNodeLeaf is GunFuExecuteNodeLeaf);
        cameraGunFuHitViewNodeLeaf = new CameraGunFuHitViewNodeLeaf(this.cameraController,
            ()=> this.cameraController.gunFuCameraTimer > 0);

        cameraStandViewNodeLeaf = new CameraStandViewNodeLeaf(this.cameraController,
            () => this.cameraController.curStance == Player.PlayerStance.stand || true);

        cameraRestNodeLeaf = new CameraRestNodeLeaf(this.cameraController, () => true);

        startNodeSelector.AddtoChildNode(cameraPlayerBasedSelector);
        startNodeSelector.AddtoChildNode(cameraRestNodeLeaf);

        cameraPlayerBasedSelector.AddtoChildNode(cameraGunFuExecuteOnGroundNodeLeaf);
        cameraPlayerBasedSelector.AddtoChildNode(cameraSprintViewNodeLeaf);
        cameraPlayerBasedSelector.AddtoChildNode(cameraAimDownSightViewNodeLeaf);
        cameraPlayerBasedSelector.AddtoChildNode(cameraCrouchViewNodeLeaf);
        cameraPlayerBasedSelector.AddtoChildNode(cameraGunFuWeaponDisarmViewNodeLeaf);
        cameraPlayerBasedSelector.AddtoChildNode(cameraGunFuHitViewNodeLeaf);
        cameraPlayerBasedSelector.AddtoChildNode(cameraStandViewNodeLeaf);


        this.SearchingNewNode();

    }
    private void SearchingNewNode()
    {
        if (startNodeSelector.FindingNode(out INode node))
        {
            curNode = node;
            (curNode as INodeLeaf).Enter();
        }
        else
        {
            curNode = node;
            throw new Exception(this.GetType().Name + " had state corrupt");
        }

    }

    public T Debugged<T>(IDebugger debugger)
    {
        if(debugger is CameraStateManagerDebugger cameraStateDebugger)
        {
            if(cameraStateDebugger.request == CameraStateManagerDebugger.CameraStateManagerDebuggerRequest.curState)
                return (T)(object)curNode;
        }
        return default;
    }
}
public class CameraRestNodeLeaf : CameraNodeLeaf
{
    public CameraRestNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
    }
}
