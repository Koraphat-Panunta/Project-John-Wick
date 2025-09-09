using UnityEngine;

public abstract class EnemyActionNodeManager :  INodeManager<EnemyActionNodeLeaf,EnemyActionSelectorNode>
{
    public Enemy enemy;
    public EnemyCommandAPI enemyCommandAPI;
    public IEnemyActionNodeManagerImplementDecision enemyDecision;
    public IEnemyActionNodeManagerImplementDecision.CombatPhase curCombatPhase => enemyDecision._curCombatPhase;
    public ZoneDefine targetZone => enemyDecision._targetZone;
    public bool takeCoverAble => enemyDecision._takeCoverAble;

    
    public EnemyActionNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, IEnemyActionNodeManagerImplementDecision enemyDecision
        ,float minTimeUpdateYingYang,float maxTimeUpdateYingYang)
    {
        this.enemy = enemy;
        this.enemyCommandAPI = enemyCommandAPI;
        this.enemyDecision = enemyDecision;

    }

    public abstract EnemyActionNodeLeaf curNodeLeaf { get ; set ; }
    public abstract EnemyActionSelectorNode startNodeSelector { get; set; }

    

    public virtual void FixedUpdateNode()
    {
        if (curNodeLeaf != null)
            curNodeLeaf.FixedUpdateNode();
    }

    public abstract void InitailizedNode();
   
    public virtual void UpdateNode()
    {
        if (curNodeLeaf.IsReset())
        {
            curNodeLeaf.Exit();
            curNodeLeaf = null;
            startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
            curNodeLeaf = nodeLeaf as EnemyActionNodeLeaf;
            curNodeLeaf.Enter();
        }

        if (curNodeLeaf != null)
            curNodeLeaf.UpdateNode();

    }
    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }

}
