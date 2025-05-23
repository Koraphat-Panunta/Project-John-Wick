using System;
using UnityEngine;

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
    public CameraSprintViewNodeLeaf cameraSprintViewNodeLeaf { get; protected set; }
    public CameraCrouchViewNodeLeaf cameraCrouchViewNodeLeaf { get; protected set; }
    public CameraStandViewNodeLeaf cameraStandViewNodeLeaf { get; protected set; }
    public CameraGunFuWeaponDisarmViewNodeLeaf cameraGunFuWeaponDisarmViewNodeLeaf { get; protected set; }
    public CameraGunFuExecuteOnGroundNodeLeaf cameraGunFuExecuteOnGroundNodeLeaf { get; protected set; }
    public CameraGunFuHitViewNodeLeaf cameraGunFuHitViewNodeLeaf { get; protected set; }
    public CameraRestNodeLeaf cameraRestNodeLeaf { get; protected set; }

    public void InitailizedNode()
    {
        startNodeSelector = new CameraSelectorNode(this.cameraController, () => true);

        cameraPlayerBasedSelector = new CameraSelectorNode(this.cameraController,
            ()=> this.cameraController.player != null && this.cameraController.player.gameObject.activeSelf
            );

        cameraAimDownSightViewNodeLeaf = new CameraAimDownSightViewNodeLeaf(this.cameraController,
            ()=> this.cameraController.isZooming);

        Debug.Log("Player = " + cameraController.player);
        Debug.Log("Player ManageNode =" + cameraController.player.playerStateNodeManager);

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

       
        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
        curNodeLeaf.Enter();

    }
}
public class CameraRestNodeLeaf : CameraNodeLeaf
{
    public CameraRestNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
    }
}
