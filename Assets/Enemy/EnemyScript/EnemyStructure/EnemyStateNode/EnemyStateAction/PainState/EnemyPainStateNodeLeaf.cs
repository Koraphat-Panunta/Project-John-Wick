using UnityEngine;

public abstract class EnemyPainStateNodeLeaf : EnemyStateLeafNode
{
    protected EnemyPainStateNodeLeaf(Enemy enemy) : base(enemy)
    {
    }
    public override void Enter()
    {
        time = 0;
        enemy._painPart = IPainState.PainPart.None;
        Debug.Log("PainState = " + this.ToString());
        base.Enter();
    }
    public override void Update()
    {
        time += Time.deltaTime;
    }
    public abstract float painDuration { get; set; }
    public float time;
    public abstract IPainState.PainPart painPart { get; set; }
 

    
}
