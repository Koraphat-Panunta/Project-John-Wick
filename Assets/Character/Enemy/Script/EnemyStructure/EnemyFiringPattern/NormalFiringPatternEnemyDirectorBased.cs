using UnityEngine;

public class NormalFiringPatternEnemyDirectorBased : NormalFiringPattern
{
    private EnemyDirector enemyDirector { get; set; }
    private IEnemyDirectedAble enemyDirectedAble { get; set; }
    public NormalFiringPatternEnemyDirectorBased(EnemyCommandAPI enemyController,EnemyDirector enemyDirector, IEnemyDirectedAble enemyRoleBasedDecision) : base(enemyController)
    {

        this.enemyDirector = enemyDirector;
        this.enemyDirectedAble = enemyRoleBasedDecision;
    }
    protected override void WillShoot()
    {
        if (this.enemyDirector.GetShooterPermission(this.enemyDirectedAble))
            base.WillShoot();
    }


}
