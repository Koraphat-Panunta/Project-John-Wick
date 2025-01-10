using NUnit.Framework;
using UnityEngine;

public class EnemyTacticDecision : EnemyDecision
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
    public override Enemy enemy { get; set ; }
    public TacticDecision curTacticDecision { get; set ; }
    public SearchingTacticDecision searchingTacticDecision { get; private set; }
    public EncouterTacticDecision encouterTacticDecision { get; private set; }
    public HoldingTacticDecision holdingTacticDecision { get; private set; }
    public TakeCoverTacticDecision takeCoverTacticDecision { get; private set; }

    protected override void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyCommand = GetComponent<EnemyCommandAPI>();

        searchingTacticDecision = new SearchingTacticDecision(enemy,this);
        encouterTacticDecision = new EncouterTacticDecision(enemy,this);
        holdingTacticDecision = new HoldingTacticDecision(enemy, this);
        takeCoverTacticDecision = new TakeCoverTacticDecision(enemy, this);

        curTacticDecision = searchingTacticDecision;
        curTacticDecision.Enter();
        base.Start();
    }

    protected override void Update()
    {
        if(curTacticDecision!=null)
            curTacticDecision.Update();
        base .Update();
    }

    protected override void FixedUpdate()
    {
        if(curTacticDecision!=null)
            curTacticDecision.FixedUpdate();
        base.FixedUpdate();
    }

    public void ChangeTactic(TacticDecision nexttacticDecision)
    {
        if (nexttacticDecision == curTacticDecision)
            return;

        curTacticDecision.Exit();
        curTacticDecision = nexttacticDecision;
        curTacticDecision.Enter();
    }

    private void OnDrawGizmos()
    {
        if(Application.isPlaying)
        for(int i =0;i < encouterTacticDecision.curvePath._markPoint.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(encouterTacticDecision.curvePath._markPoint[i], 0.15f);
        }
    }
}
