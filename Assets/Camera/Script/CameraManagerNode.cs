using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerNode:INodeManager,IDebuggedAble
{
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public CameraController cameraController { get;protected set; }
    private Dictionary<CameraThirdPersonControllerViewNodeLeaf, CameraThirdPersonControllerViewScriptableObject> cameraTPPC_ScriptableObject;
    private INodeManager playerStateManager => cameraController.player.playerStateNodeManager;
    private INodeManager playerWeaponManuverStateManager => cameraController.player.weaponAdvanceUser._weaponManuverManager;
    public CameraManagerNode(CameraController cameraController)
    {
        this.cameraController = cameraController;
        this.nodeManagerBehavior = new NodeManagerBehavior();
        this.cameraTPPC_ScriptableObject = new Dictionary<CameraThirdPersonControllerViewNodeLeaf, CameraThirdPersonControllerViewScriptableObject>();

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

    public NodeSelector cameraThirdPersonControllerPlayerBasedSelector { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraTPSStandViewNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraTPSCrouchViewNodeLeaf { get; protected set; }

    public NodeSelector cameraPerformGunFuSelector { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraPerformGunFuWeaponDisarmNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraPerformGunFuExecuteViewNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraPerformGunFuHitViewNodeLeaf { get; protected set; }

    public CameraThirdPersonControllerViewNodeLeaf cameraTPSSprintViewNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraAimDownSightNodeLeaf { get; protected set; }
    public CameraRestNodeLeaf cameraRestNodeLeaf { get; protected set; }
    INodeLeaf INodeManager.curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    protected INodeLeaf curNodeLeaf;

    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        cameraThirdPersonControllerPlayerBasedSelector = new NodeSelector(() => cameraController.isOnPlayerThirdPersonController);
        cameraAimDownSightNodeLeaf = new CameraAimDownSightViewNodeLeaf(cameraController, cameraController.cameraAimDownSightView_SCRP,
            () => 
            {
                if (cameraController.player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0)
                    return true;
                    
                return false;
            });
        this.cameraTPSSprintViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraTPSSprintView_SCRP,
            () => cameraController.isSprint);

        this.cameraPerformGunFuSelector = new NodeSelector(
            () => cameraController.isPerformGunFu);
        this.cameraPerformGunFuWeaponDisarmNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraPerformGunFuWeaponDisarm_SCRP,
            () => cameraController.curGunFuNode != null && cameraController.curGunFuNode is WeaponDisarm_GunFuInteraction_NodeLeaf );
        this.cameraPerformGunFuExecuteViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController , cameraController.cameraExecute_Single_SCRP
           ,() => cameraController.curGunFuNode != null && cameraController.curGunFuNode is IGunFuExecuteNodeLeaf);
        this.cameraPerformGunFuHitViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraPerformGunFuHitView_SCRP,
            () => cameraController.curGunFuNode != null && cameraController.curGunFuNode is GunFuHitNodeLeaf);

        cameraTPSCrouchViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraTPSCrouchView_SCRP,
            () => cameraController.isCrouching);
        cameraTPSStandViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraTPSStandView_SCRP,
            () => true);

        cameraRestNodeLeaf = new CameraRestNodeLeaf(cameraController,()=>true);


        startNodeSelector.AddtoChildNode(cameraThirdPersonControllerPlayerBasedSelector);
        startNodeSelector.AddtoChildNode(cameraRestNodeLeaf);

        cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(cameraPerformGunFuSelector);
        cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(cameraTPSSprintViewNodeLeaf);
        cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(cameraAimDownSightNodeLeaf);
        cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(cameraTPSCrouchViewNodeLeaf);
        cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(cameraTPSStandViewNodeLeaf);

        cameraPerformGunFuSelector.AddtoChildNode(cameraPerformGunFuWeaponDisarmNodeLeaf);
        cameraPerformGunFuSelector.AddtoChildNode(cameraPerformGunFuExecuteViewNodeLeaf);
        cameraPerformGunFuSelector.AddtoChildNode(cameraPerformGunFuHitViewNodeLeaf);

        this.nodeManagerBehavior.SearchingNewNode(this);

    }
    public T Debugged<T>(IDebugger debugger)
    {
        if(debugger is CameraStateManagerDebugger cameraStateDebugger)
        {
            if(cameraStateDebugger.request == CameraStateManagerDebugger.CameraStateManagerDebuggerRequest.curState)
                return (T)(object)curNodeLeaf;
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
