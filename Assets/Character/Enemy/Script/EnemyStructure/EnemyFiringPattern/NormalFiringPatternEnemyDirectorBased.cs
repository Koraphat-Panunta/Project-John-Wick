using UnityEngine;

public class NormalFiringPatternEnemyDirectorBased : NormalFiringPattern
{
    private EnemyDirector enemyDirector { get; set; }
    private EnemyRoleBasedDecision enemyRoleBasedDecision { get; set; }
    public NormalFiringPatternEnemyDirectorBased(EnemyCommandAPI enemyController,EnemyDirector enemyDirector, EnemyRoleBasedDecision enemyRoleBasedDecision) : base(enemyController)
    {
        this.enemyDirector = enemyDirector;
        this.enemyRoleBasedDecision = enemyRoleBasedDecision;
    }
    protected override void Shoot()
    {
        if(enemyDirector.GetShooterPermission(enemyRoleBasedDecision))
        base.Shoot();
    }


}
