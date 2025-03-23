using UnityEngine;

public abstract class EnemyActionNodeManager :  INodeManager<EnemyActionNodeLeaf,EnemyActionSelectorNode>
{
    public Enemy enemy;
    public EnemyCommandAPI enemyCommandAPI;
    public IEnemyActionNodeManagerImplementDecision enemyDecision;
    public IEnemyActionNodeManagerImplementDecision.CombatPhase curCombatPhase => enemyDecision._curCombatPhase;
    public ZoneDefine targetZone => enemyDecision._targetZone;
    public bool takeCoverAble => enemyDecision._takeCoverAble;

    public abstract float yingYangCalculate { get;protected set; }
    public float yingYang { get => enemyDecision._yingYang; set => enemyDecision._yingYang = value; }
    public float enegyWithIn;

    private float timeUpdateYingYang;
    private float minTimeUpdateYingYang;
    private float maxTimeUpdateYingYang;
    private float elapesTimerUpdateYingYang;
    public EnemyActionNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, IEnemyActionNodeManagerImplementDecision enemyDecision
        ,float minTimeUpdateYingYang,float maxTimeUpdateYingYang)
    {
        this.enemy = enemy;
        this.enemyCommandAPI = enemyCommandAPI;
        this.enemyDecision = enemyDecision;

        this.minTimeUpdateYingYang = minTimeUpdateYingYang;
        this.maxTimeUpdateYingYang= maxTimeUpdateYingYang;

        this.timeUpdateYingYang = Random.Range(minTimeUpdateYingYang, maxTimeUpdateYingYang);
        this.enegyWithIn = enemyDecision._yingYang;

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

        UpdateYingYang();
    }

    private void UpdateYingYang()
    {
        elapesTimerUpdateYingYang += Time.deltaTime;
        if(elapesTimerUpdateYingYang >= timeUpdateYingYang)
        {
            enegyWithIn = yingYangCalculate;
            timeUpdateYingYang = Random.Range(minTimeUpdateYingYang, maxTimeUpdateYingYang);
            elapesTimerUpdateYingYang = 0;
        }
    }
}
