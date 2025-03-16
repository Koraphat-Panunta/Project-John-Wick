using UnityEngine;

public abstract class EnemyActionNodeManager :  INodeManager<EnemyActionNodeLeaf,EnemyActionSelectorNode>
{
    public EnemyActionNodeManager()
    {
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
