using System.Collections.Generic;
using UnityEngine;

public partial class EnemyDirectedDecision : EnemyDecision
    , IEnemyDirectedAble 
    , IObserverEnemy
    , IObserverEnemyDecision
{
    [SerializeField] public EnemyDirectedDecisionScriptableObject enemyDirectedDecisionScriptableObject;
    public EnemyDecisionContext enemyDecisionContext;

    [SerializeField] private CombatPhase startCombatPhase;
    [SerializeField] private EnemyRoleCommand startRoleCommand;

    public override void Initialized()
    {
        this.enemyDecisionContext = new EnemyDecisionContext(this.startCombatPhase,this.startRoleCommand,this.enemyDirectedDecisionScriptableObject.enemyDecisionContextScriptableObject);
        this._nodeManagerBehavior = new NodeManagerBehavior();
        this.enemy.AddObserver(this);
        this.AddEnemyDecisionObserver(this);
        this.InitailizedNode();
        base.Initialized();
    }

    protected override void Update()
    {
        this.UpdateNode();
        base.Update();
    }
    protected override void FixedUpdate()
    {
        this.FixedUpdateNode();
        base.FixedUpdate();
    }

   

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        EnemyDecisionInjectionEvent.OnHearding(
            noiseMaker
            , this.enemyDecisionContext
            );
    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        EnemyDecisionInjectionEvent.OnSpotingTarget(
            target.transform
            , this.enemyDecisionContext
            );
    }

    public void OnNotify<T>(Enemy enemy, T node)
    {
        
    }

    public void OnNotifyEnemyDecision<T>(EnemyDecision enemyDecision, T var)
    {
        if (var is InsistEnemyActionNodeLeaf insistEnemyActionNodeLeaf
            && insistEnemyActionNodeLeaf == this.insist_Camper_EnemyActionNodeLeaf
            && this.camperPhaseTimerNodeLeaf.IsComplete()
            && this.enemyDecisionContext.roleCommand == EnemyRoleCommand.Camping)
        {
            this.enemyDecisionContext.SetRoleCommand(EnemyRoleCommand.Support);
        }
    }


    #region ImplementDirectedAble

    EnemyRoleCommand IEnemyDirectedAble._curCommandPerforme { get => this.enemyDecisionContext.roleCommand; set => this.enemyDecisionContext.SetRoleCommand(value); }
    public CombatPhase _combatPhase => this.enemyDecisionContext.combatPhase;

    public Enemy _enemy => this.enemy;

    public EnemyCommandAPI _enemyCommandAPI => this.enemyCommand;

    public EnemyDecision _enemyDecision => this;


    #endregion


}

public partial class EnemyDirectedDecision : INodeManager
{
    protected RestNodeLeaf noneDecisionNodeLeaf;

    protected NodeSelector camperBehaviorNodeSelector;
    protected CamperEngageActionNodeLeaf engageTargetEnemyActionNodeLeaf;
    protected HoldSightActionNodeLeaf holdSightActionNodeLeaf;
    protected InsistEnemyActionNodeLeaf insist_Camper_EnemyActionNodeLeaf;

    protected InsistEnemyActionNodeLeaf insist_Chill_EnemyActionNodeLeaf;

    protected MoveToTheZoneEnemyActionNodeLeaf findTargetPositionSuspectNodeLeaf;

    protected NodeSelector supporterBehaviorNodeSelector;
    protected SwarpCombatPositionActionNodeLeaf swarpCombatPositionActionNodeLeaf;
    protected SurroundPlayerPositioningActionNodeLeaf surroundPlayerPositioningActionNodeLeaf;
    protected InsistEnemyActionNodeLeaf insist_Supporter_EnemyActionNodeLeaf;

    protected NodeSelector ambusherBehaviorNodeSelector;
    protected ApprouchingTargetEnemyActionNodeLeaf approuchingTargetEnemyActionNodeLeaf;
    protected InsistEnemyActionNodeLeaf insist_Approucher_EnemyActionNodeLeaf;


    public NodeComponentManager enemyDecisionNodeComponentManager;
    protected PhaseTimerNodeLeaf camperPhaseTimerNodeLeaf;
    protected PhaseTimerNodeLeaf engaingPhaseTimerNodeLeaf;
    public void InitailizedNode()
    {
        this.InitilaizedNodeComponent();

        this.startNodeSelector = new NodeSelector(
            () => true);

        this.noneDecisionNodeLeaf = new RestNodeLeaf(
            ()=>this._enemy.isDead);

        this.InitializedCamper();

        this.insist_Chill_EnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(
            this.enemy
            , this.enemyCommand
            , () => this._combatPhase == CombatPhase.Chill
            , this
            , this.enemyDecisionContext
            );

        this.findTargetPositionSuspectNodeLeaf = new MoveToTheZoneEnemyActionNodeLeaf(
            this.enemy
            ,this._enemyCommandAPI
            ,()=> this._combatPhase == CombatPhase.Suspect
            ,this
            ,this.enemyDecisionContext
            ,this.enemyDecisionContext._targetZone
            );

        this.InitializedSupporter();
        this.InitilaizedAmbuser();

        this.startNodeSelector.AddtoChildNode(this.noneDecisionNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.camperBehaviorNodeSelector);
        this.startNodeSelector.AddtoChildNode(this.insist_Chill_EnemyActionNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.findTargetPositionSuspectNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.supporterBehaviorNodeSelector);
        this.startNodeSelector.AddtoChildNode(this.ambusherBehaviorNodeSelector);

        this.camperBehaviorNodeSelector.AddtoChildNode(this.engageTargetEnemyActionNodeLeaf);
        this.camperBehaviorNodeSelector.AddtoChildNode(this.holdSightActionNodeLeaf);
        this.camperBehaviorNodeSelector.AddtoChildNode(this.insist_Camper_EnemyActionNodeLeaf);

        this.supporterBehaviorNodeSelector.AddtoChildNode(this.swarpCombatPositionActionNodeLeaf);
        this.supporterBehaviorNodeSelector.AddtoChildNode(this.surroundPlayerPositioningActionNodeLeaf);
        this.supporterBehaviorNodeSelector.AddtoChildNode(this.insist_Supporter_EnemyActionNodeLeaf);

        this.ambusherBehaviorNodeSelector.AddtoChildNode(this.approuchingTargetEnemyActionNodeLeaf);
        this.ambusherBehaviorNodeSelector.AddtoChildNode(this.insist_Approucher_EnemyActionNodeLeaf);

        this._nodeManagerBehavior.SearchingNewNode(this);
    }
    
    private void InitializedCamper()
    {
        this.camperBehaviorNodeSelector = new NodeSelector(
           () => this.enemyDecisionContext.roleCommand == EnemyRoleCommand.Camping
           );
        this.engageTargetEnemyActionNodeLeaf = new CamperEngageActionNodeLeaf(
            this._enemy, this.enemyCommand
            , () => this._combatPhase == CombatPhase.Alert 
            && this.camperPhaseTimerNodeLeaf.IsComplete() == false
            , this
            );
        this.holdSightActionNodeLeaf = new HoldSightActionNodeLeaf(
            this._enemy
            , this._enemyCommandAPI
            , () => this._combatPhase == CombatPhase.Aware
            || this._combatPhase == CombatPhase.Suspect
            , this
            );
        this.insist_Camper_EnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(
            this.enemy
            , this.enemyCommand
            , () => true
            , this
            , this.enemyDecisionContext
            );


    }
    private void InitializedSupporter()
    {
        this.supporterBehaviorNodeSelector = new NodeSelector(
            ()=> this.enemyDecisionContext.roleCommand == EnemyRoleCommand.Support);

        this.swarpCombatPositionActionNodeLeaf = new SwarpCombatPositionActionNodeLeaf(
            this.enemy
            ,this._enemyCommandAPI
            ,()=> this.engaingPhaseTimerNodeLeaf.GetCurrentPhase() > 0
            ,this.enemyDecisionContext
            ,this);

        this.surroundPlayerPositioningActionNodeLeaf = new SurroundPlayerPositioningActionNodeLeaf(
            this.enemy
            , this._enemyCommandAPI
            , () => this.enemyDecisionContext.combatPhase >= CombatPhase.Aware
            , this
            , this.enemyDecisionContext);

        this.insist_Supporter_EnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(
            this.enemy
            , this.enemyCommand
            , () => true
            , this
            , this.enemyDecisionContext);

    }
    private void InitilaizedAmbuser() 
    {
        this.ambusherBehaviorNodeSelector = new NodeSelector(() => this.enemyDecisionContext.roleCommand == EnemyRoleCommand.Ambush);

        this.approuchingTargetEnemyActionNodeLeaf = new ApprouchingTargetEnemyActionNodeLeaf(
            this.enemy
            ,this.enemyCommand
            ,()=> this.engaingPhaseTimerNodeLeaf.GetCurrentPhase() >= 1
            ,this._enemyDecision
            ,this.enemyDecisionContext);

        this.insist_Approucher_EnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(
            this._enemy
            ,this.enemyCommand
            ,()=> true
            ,this
            ,this.enemyDecisionContext);


    }

    private void InitilaizedNodeComponent()
    {
        this.enemyDecisionNodeComponentManager = new NodeComponentManager();

        this.camperPhaseTimerNodeLeaf = new PhaseTimerNodeLeaf(
            ()=> this.enemyDecisionContext.roleCommand == EnemyRoleCommand.Camping && this.enemyDecisionContext.combatPhase == CombatPhase.Alert
            ,this.enemyDirectedDecisionScriptableObject.camperPhaseTimer.duration
            ,this.enemyDirectedDecisionScriptableObject.camperPhaseTimer.phaseThresholds
            ,false);

        this.engaingPhaseTimerNodeLeaf = new PhaseTimerNodeLeaf(
            ()=> 
            (
            this.enemyDecisionContext.roleCommand == EnemyRoleCommand.Support 
            || this.enemyDecisionContext.roleCommand == EnemyRoleCommand.Ambush
            ) 
            && 
            (this.enemyDecisionContext.combatPhase >= CombatPhase.Aware)
            , this.enemyDirectedDecisionScriptableObject.engagingPhaseTimer.duration
            , this.enemyDirectedDecisionScriptableObject.engagingPhaseTimer.phaseThresholds
            , true);

        this.enemyDecisionNodeComponentManager.AddNode(this.camperPhaseTimerNodeLeaf);
        this.enemyDecisionNodeComponentManager.AddNode(this.engaingPhaseTimerNodeLeaf);
    }
    public void UpdateNode()
    {
        this._nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
        this.enemyDecisionNodeComponentManager.Update();
    }
    public void FixedUpdateNode()
    {
        this._nodeManagerBehavior.FixedUpdateNode(this);
        this.enemyDecisionNodeComponentManager.FixedUpdate();

        Debug.Log("Combat phase = " + this.enemyDecisionContext.combatPhase);
        Debug.Log("Role Command = " + this.enemyDecisionContext.roleCommand);
    }

  
    INodeLeaf INodeManager._curNodeLeaf { get; set ; }
    public INodeSelector startNodeSelector { get ; set; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    public List<INodeManager> _parallelNodeManahger { get; set; }
}
