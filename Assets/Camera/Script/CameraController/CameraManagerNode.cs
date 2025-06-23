using System;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraController:MonoBehaviour,INodeManager,IDebuggedAble
{
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public CameraController cameraController { get;protected set; }
    public BlackBoard blackBoard = new BlackBoard();
    private Dictionary<CameraThirdPersonControllerViewNodeLeaf, CameraThirdPersonControllerViewScriptableObject> cameraTPPC_ScriptableObject;
    public CameraController(CameraController cameraController)
    {
        this.cameraController = cameraController;
        this.nodeManagerBehavior = new NodeManagerBehavior();
        this.cameraTPPC_ScriptableObject = new Dictionary<CameraThirdPersonControllerViewNodeLeaf, CameraThirdPersonControllerViewScriptableObject>();

        blackBoard.Set<bool>("isAiming",false);
        blackBoard.Set<bool>("isSprinting", false);
        blackBoard.Set<bool>("isPerformGunFu", false);
        blackBoard.Set<bool>("isCrouching", false);
        blackBoard.Set<bool>("isOnPlayerThirdPersonController", false);
        blackBoard.Set<IGunFuNode>("curGunFuNode", null);

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

        cameraThirdPersonControllerPlayerBasedSelector = new NodeSelector(() => this.blackBoard.Get<bool>("isOnPlayerThirdPersonController"));
        cameraAimDownSightNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraAimDownSightView_SCRP,
            () => this.blackBoard.Get<bool>("isAiming"));
        this.cameraTPSSprintViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, cameraTPSSprintView_SCRP,
            () => this.blackBoard.Get<bool>("isSprinting"));

        this.cameraPerformGunFuSelector = new NodeSelector(() => this.blackBoard.Get<bool>("isPerformGunFu"));
        this.cameraPerformGunFuWeaponDisarmNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController,cameraPerformGunFuWeaponDisarm_SCRP,
            () => 
            {
                IGunFuNode curGunFuNode = this.blackBoard.Get<IGunFuNode>("curGunFuNode");

                if(curGunFuNode == null)
                    return false;

                if(curGunFuNode is WeaponDisarm_GunFuInteraction_NodeLeaf)
                    return true;
                return false;
            });
        this.cameraPerformGunFuExecuteViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController,this.cameraPerformGunFuExecuteView_SCRP,
            ()=> {
                IGunFuNode curGunFuNode = this.blackBoard.Get<IGunFuNode>("curGunFuNode");

                if (curGunFuNode == null)
                    return false;

                if (curGunFuNode is GunFuExecuteNodeLeaf)
                    return true;
                return false;
            });
        this.cameraPerformGunFuHitViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, this.cameraPerformGunFuHitView_SCRP,
            () =>
            {
                IGunFuNode curGunFuNode = this.blackBoard.Get<IGunFuNode>("curGunFuNode");

                if (curGunFuNode == null)
                    return false;

                if (curGunFuNode is PlayerGunFuHitNodeLeaf)
                    return true;
                return false;
            });

        cameraTPSCrouchViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, this.cameraTPSCrouchView_SCRP,
            () => this.blackBoard.Get<bool>("isCrouching"));
        cameraTPSStandViewNodeLeaf = new CameraThirdPersonControllerViewNodeLeaf(cameraController, this.cameraTPSStandView_SCRP,
            () => true);

        startNodeSelector.AddtoChildNode(cameraThirdPersonControllerPlayerBasedSelector);
        startNodeSelector.AddtoChildNode(cameraRestNodeLeaf);

        cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(cameraAimDownSightNodeLeaf);
        cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(cameraTPSSprintViewNodeLeaf);
        cameraThirdPersonControllerPlayerBasedSelector.AddtoChildNode(cameraPerformGunFuSelector);
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
public partial class CameraController
{
    [SerializeField] private CameraThirdPersonControllerViewScriptableObject cameraTPSStandView_SCRP;
    [SerializeField] private CameraThirdPersonControllerViewScriptableObject cameraTPSCrouchView_SCRP;
    [SerializeField] private CameraThirdPersonControllerViewScriptableObject cameraPerformGunFuWeaponDisarm_SCRP;
    [SerializeField] private CameraThirdPersonControllerViewScriptableObject cameraPerformGunFuExecuteView_SCRP;
    [SerializeField] private CameraThirdPersonControllerViewScriptableObject cameraPerformGunFuHitView_SCRP;
    [SerializeField] private CameraThirdPersonControllerViewScriptableObject cameraTPSSprintView_SCRP;
    [SerializeField] private CameraThirdPersonControllerViewScriptableObject cameraAimDownSightView_SCRP;
}
public class CameraRestNodeLeaf : CameraNodeLeaf
{
    public CameraRestNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
    }
}
