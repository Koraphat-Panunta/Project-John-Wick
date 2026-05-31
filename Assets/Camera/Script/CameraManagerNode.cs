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
    private INodeManager playerWeaponManuverStateManager => cameraController.player._weaponManuverManager;
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

    public CameraThridPersonControllerDynamicTrackingNodeLeaf cameraTPSSprintViewNodeLeaf { get; protected set; }
    public CameraThridPersonControllerDynamicTrackingNodeLeaf cameraTPSParkourViewNodeLeaf { get; protected set; }

    public CameraRestNodeLeaf cameraRestNodeLeaf { get; protected set; }
    INodeLeaf INodeManager._curNodeLeaf { get => curNodeLeaf; set => this.curNodeLeaf = value; }


    protected INodeLeaf curNodeLeaf;

    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        cameraThirdPersonControllerPlayerBasedSelector = new NodeSelector(() => cameraController.isOnPlayerThirdPersonController);
      
        this.cameraTPSSprintViewNodeLeaf = new CameraThridPersonControllerDynamicTrackingNodeLeaf(
            cameraController, cameraController.cameraTPSSprintView_SCRP,
            () => cameraController.isSprint);

        var cam = cameraController.thirdPersonCinemachineCamera;
 
        this.cameraTPSSprintViewNodeLeaf.SetTrackTransform(
            new Transform[] { cam.targetFollowTarget, cameraController.player.humanoidBone._headBone },
            new float[]     { 0.5f, 0.5f });
        this.cameraTPSSprintViewNodeLeaf.SetLookTransform(
            new Transform[] { cam.targetLookTarget, cameraController.player.humanoidBone._headBone },
            new float[] { 0.5f, 0.5f });

        this.cameraTPSParkourViewNodeLeaf = new CameraThridPersonControllerDynamicTrackingNodeLeaf(
            cameraController, cameraController.cameraTPSParkourView_SCRP,
            () => playerStateManager.TryGetCurNodeLeaf<IParkourNodeLeaf>());
        this.cameraTPSParkourViewNodeLeaf.SetTrackTransform(
            new Transform[] { cam.targetFollowTarget, cameraController.player.humanoidBone.hips },
            new float[]     { 0.5f, 0.5f });
        this.cameraTPSParkourViewNodeLeaf.SetLookTransform(
            new Transform[] { cam.targetLookTarget, cameraController.player.humanoidBone.hips },
            new float[]     { .5f, .5f });

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
           || this.cameraController.curGunFuNode is GunFuHitDownNodeLeaf
           || this.cameraController.curGunFuNode is OCMReloadNodeLeaf
           || this.cameraController.curGunFuNode is OCM_KnockDown_NodeLeaf)
           );
        this.cameraPerformGunFuHitViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraController.cameraPerformGunFuHitView_SCRP,
            () => cameraController.curGunFuNode != null && cameraController.curGunFuNode is GunFuHitNodeLeaf);

        var standLowReadyNode = new CameraThridPersonControllerDynamicTrackingNodeLeaf(
            cameraController, cameraController.cameraTPSStandView_SCRP, () => false);
        standLowReadyNode.SetTrackTransform(
            new Transform[] { cam.targetFollowTarget, cameraController.player.humanoidBone.hips },
            new float[]     { 1, 0.3f });
        standLowReadyNode.SetLookTransform(
            new Transform[] { cam.targetLookTarget, cameraController.player.humanoidBone.hips },
            new float[]     { 1, 0 });

        var crouchLowReadyNode = new CameraThridPersonControllerDynamicTrackingNodeLeaf(
            cameraController, cameraController.cameraTPSCrouchView_SCRP, () => false);
        crouchLowReadyNode.SetTrackTransform(
            new Transform[] { cam.targetFollowTarget, cameraController.player.humanoidBone.hips },
            new float[]     { 1, 0.3f });
        crouchLowReadyNode.SetLookTransform(
            new Transform[] { cam.targetLookTarget, cameraController.player.humanoidBone.hips },
            new float[]     { 1, 0 });

        var proneLowReadyNode = new CameraThirdPersonControllerViewNodeLeaf(
            cameraController, cameraController.cameraTPSProneView_SCRP, () => false);

        this.cameraTPSCrouchViewNodeLeaf = new CameraAimDownSightViewNodeLeaf(
            cameraController,
            cameraController.cameraCrouchAimDownSightView_SCRP,
            crouchLowReadyNode,
            this.cameraController.player,
            () => cameraController.isCrouching);
        this.cameraTPSProneViewNodeLeaf = new CameraAimDownSightViewNodeLeaf(
            cameraController,
            this.cameraController.cameraProneAimDownSightView_SCRP,
            proneLowReadyNode,
            this.cameraController.player,
            () => this.cameraController.player.stance == Stance.prone);
        this.cameraTPSStandViewNodeLeaf = new CameraAimDownSightViewNodeLeaf(
            cameraController,
            this.cameraController.cameraStandAimDownSightView_SCRP,
            standLowReadyNode,
            this.cameraController.player,
            () => true);



        this.cameraRestNodeLeaf = new CameraRestNodeLeaf(cameraController,()=>true);


        this.startNodeSelector.AddtoChildNode(this.cameraThirdPersonControllerPlayerBasedSelector);
        this.startNodeSelector.AddtoChildNode(this.cameraRestNodeLeaf);

        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraPerformGunFuSelector);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraTPSParkourViewNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraTPSSprintViewNodeLeaf);
        this.cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(this.cameraTPSDodgeViewNodeLeaf);
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
