using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerNode:INodeManager,IDebuggedAble
{
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    public List<INodeManager> _parallelNodeManahger { get; set; }
    public CameraController cameraController { get;protected set; }
    private Dictionary<CameraThirdPersonControllerViewNodeLeaf, CameraThirdPersonControllerViewScriptableObject> cameraTPPC_ScriptableObject;
    private INodeManager playerStateManager => cameraController.player.playerStateNodeManager;
    private INodeManager playerWeaponManuverStateManager => cameraController.player.weaponAdvanceUser._weaponManuverManager;
    public CameraManagerNode(CameraController cameraController)
    {
        this.cameraController = cameraController;
        this._nodeManagerBehavior = new NodeManagerBehavior();
        this._parallelNodeManahger = new List<INodeManager>();
        this.cameraTPPC_ScriptableObject = new Dictionary<CameraThirdPersonControllerViewNodeLeaf, CameraThirdPersonControllerViewScriptableObject>();

        InitailizedNode();
    }

    public void FixedUpdateNode()
    {
        _nodeManagerBehavior.FixedUpdateNode(this);
    }
    public void UpdateNode()
    {
        _nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
    }
   

    public NodeSelector cameraThirdPersonControllerPlayerBasedSelector { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraTPSStandViewNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraTPSCrouchViewNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraTPSProneViewNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraTPSDodgeViewNodeLeaf { get; protected set; }

    public NodeSelector cameraPerformGunFuSelector { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraPerformGunFuWeaponDisarmNodeLeaf { get; protected set; }
    public CameraThridPersonControllerDynamicTrackingNodeLeaf cameraDynamicTrackingNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraPerformGunFuHitViewNodeLeaf { get; protected set; }

    public CameraThirdPersonControllerViewNodeLeaf cameraTPSSprintViewNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraStandAimDownSightNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraCrouchAimDownSightNodeLeaf { get; protected set; }
    public CameraThirdPersonControllerViewNodeLeaf cameraProneAimDownSightNodeLeaf { get; protected set; }
    public CameraRestNodeLeaf cameraRestNodeLeaf { get; protected set; }
    INodeLeaf INodeManager._curNodeLeaf { get => curNodeLeaf; set => this.curNodeLeaf = value; }


    protected INodeLeaf curNodeLeaf;

    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        cameraThirdPersonControllerPlayerBasedSelector = new NodeSelector(() => cameraController.isOnPlayerThirdPersonController);
        this.cameraStandAimDownSightNodeLeaf = new CameraAimDownSightViewNodeLeaf(cameraController, cameraController.cameraStandAimDownSightView_SCRP,
            cameraController.cameraTPSStandView_SCRP.viewOffsetRight.z,
            () => 
            {
                if (cameraController.player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0
                && cameraController.player.stance == Stance.stand)
                    return true;

                return false;
            });
        this.cameraCrouchAimDownSightNodeLeaf = new CameraAimDownSightViewNodeLeaf(cameraController,cameraController.cameraCrouchAimDownSightView_SCRP,
            cameraController.cameraTPSCrouchView_SCRP.viewOffsetRight.z,
            () => 
            {
                if (cameraController.player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0
                && cameraController.player.stance == Stance.crouch)
                    return true;
                
                return false;
            });
        this.cameraProneAimDownSightNodeLeaf = new CameraAimDownSightViewNodeLeaf(cameraController, cameraController.cameraProneAimDownSightView_SCRP,
            cameraController.cameraTPSProneView_SCRP.viewOffsetRight.z,
            () =>
            {
                if (cameraController.player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0
                && cameraController.player.stance == Stance.prone || playerStateManager.TryGetCurNodeLeaf<PlayerGetUpStateNodeLeaf>())
                    return true;

                return false;
            });
        this.cameraTPSSprintViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraTPSSprintView_SCRP,
            () => 
            cameraController.isSprint );
        this.cameraTPSDodgeViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraTPSDodgeView_SCRP
            , () => playerStateManager.TryGetCurNodeLeaf<PlayerDodgeRollStateNodeLeaf>());

        this.cameraPerformGunFuSelector = new NodeSelector(
            () => cameraController.isPerformGunFu);
        this.cameraPerformGunFuWeaponDisarmNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraPerformGunFuWeaponDisarm_SCRP,
            () => cameraController.curGunFuNode != null && cameraController.curGunFuNode is WeaponDisarm_GunFuInteraction_NodeLeaf );
        this.cameraDynamicTrackingNodeLeaf = new CameraThridPersonControllerDynamicTrackingNodeLeaf(cameraController , cameraController.cameraExecute_Single_SCRP
           ,() => cameraController.curGunFuNode != null 
           && (
           cameraController.curGunFuNode is IGunFuExecuteNodeLeaf
           || this.cameraController.curGunFuNode is GunFuHitDownNodeLeaf)
           );
        this.cameraPerformGunFuHitViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraPerformGunFuHitView_SCRP,
            () => cameraController.curGunFuNode != null && cameraController.curGunFuNode is GunFuHitNodeLeaf);

        this.cameraTPSCrouchViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraTPSCrouchView_SCRP,
            () => cameraController.isCrouching);
        this.cameraTPSProneViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraTPSProneView_SCRP,
            () => this.cameraController.player.stance == Stance.prone);
        this.cameraTPSStandViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraTPSStandView_SCRP,
            () => true);



        this.cameraRestNodeLeaf = new CameraRestNodeLeaf(cameraController,()=>true);


        this.startNodeSelector.AddtoChildNode(this.cameraThirdPersonControllerPlayerBasedSelector);
        this.startNodeSelector.AddtoChildNode(this.cameraRestNodeLeaf);

        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraPerformGunFuSelector);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraTPSSprintViewNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraTPSDodgeViewNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraStandAimDownSightNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraProneAimDownSightNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraCrouchAimDownSightNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraTPSCrouchViewNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraTPSProneViewNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraTPSStandViewNodeLeaf);

        this.cameraPerformGunFuSelector.AddtoChildNode(this.cameraPerformGunFuWeaponDisarmNodeLeaf);
        this.cameraPerformGunFuSelector.AddtoChildNode(this.cameraDynamicTrackingNodeLeaf);
        this.cameraPerformGunFuSelector.AddtoChildNode(this.cameraPerformGunFuHitViewNodeLeaf);

        this._nodeManagerBehavior.SearchingNewNode(this);

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
