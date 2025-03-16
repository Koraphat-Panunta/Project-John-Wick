using UnityEngine;

public abstract class EnemyRoleBasedDecision : EnemyDecision,INodeManager
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }

    public abstract INodeLeaf curNodeLeaf { get; set ; }
    public abstract INodeSelector startNodeSelector { get ; set ; }
    public abstract NodeManagerBehavior nodeManagerBehavior { get; set; }


    public enum CombatPhase
    {
        Chill,
        Aware,
        Alert
    }
    public CombatPhase curCombatPhase;
    public float pressure;

    protected override void Awake()
    {
        base.Awake();

        enemy.NotifyCommunicate += OnNotifyGetCommunicate;
        enemyCommand = GetComponent<EnemyCommandAPI>();
        curCombatPhase = CombatPhase.Chill;
        pressure = 0;

        InitailizedNode();
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        UpdateNode();
        base.Update();
    }
    protected override void FixedUpdate()
    {
        FixedUpdateNode();  
        base.FixedUpdate();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        if (curCombatPhase == CombatPhase.Alert)
            return;
        curCombatPhase = CombatPhase.Aware;

    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        curCombatPhase = CombatPhase.Alert;
    }

    private void OnNotifyGetCommunicate(Communicator communicator)
    {
        if (communicator is EnemyCommunicator enemyCommunicator == false)
            return;
        switch (enemyCommunicator.enemyCommunicateMassage)
        {
            case EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition:
                {
                    if (curCombatPhase == CombatPhase.Alert)
                        return;
                    curCombatPhase = CombatPhase.Aware;
                }
                break;
        }
    }

    public void UpdateNode() => nodeManagerBehavior.UpdateNode(this);
   
    public void FixedUpdateNode()=>nodeManagerBehavior.FixedUpdateNode(this);

    public abstract void InitailizedNode();
    
}
