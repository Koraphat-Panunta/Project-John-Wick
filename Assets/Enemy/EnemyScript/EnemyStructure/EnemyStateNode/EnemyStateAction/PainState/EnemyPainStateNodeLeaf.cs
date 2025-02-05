using UnityEngine;

public abstract class EnemyPainStateNodeLeaf : EnemyStateLeafNode
{
    protected Animator animator;
    protected abstract string stateName { get; }
    protected EnemyPainStateNodeLeaf(Enemy enemy,Animator animator) : base(enemy)
    {
        this.animator = animator;
    }
    public override void Enter()
    {
        MotionControlManager motionControlManager = enemy.motionControlManager;

        time = 0;
        enemy._painPart = IPainState.PainPart.None;
        motionControlManager.ChangeMotionState(motionControlManager.animationDrivenMotionState);

        animator.CrossFade(stateName, 0.1f, 0);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GotHit);

        base.Enter();
    }
    public override void Update()
    {
        time += Time.deltaTime;
       
    }
    public override void FixedUpdate()
    {
        enemy.enemyMovement.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
        base.FixedUpdate();
    }
    public abstract float painDuration { get; set; }
    public float time;
    public abstract IPainState.PainPart painPart { get; set; }
 

    
}
