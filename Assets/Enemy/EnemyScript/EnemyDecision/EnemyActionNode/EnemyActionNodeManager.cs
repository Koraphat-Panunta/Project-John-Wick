using UnityEngine;

public abstract class EnemyActionNodeManager :  INodeManager<EnemyActionNodeLeaf,EnemyActionSelectorNode>
{
    public Enemy enemy;
    public EnemyCommandAPI enemyCommandAPI;
    public EnemyDecision enemyDecision;
    public EnemyDecision.CombatPhase curCombatPhase => enemyDecision.curCombatPhase;
    public float pressure => enemyDecision.pressure;
    public EnemyActionNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, EnemyDecision enemyDecision)
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

   

   

    
}
