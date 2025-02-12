using UnityEngine;

public class WeaponAfterActionEnemy : WeaponAfterAction
{
    private Enemy enemy;
    public WeaponAfterActionEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public override void AfterFiringSingleAction(Weapon weapon)
    {
       
    }

    public override void AimDownSight(Weapon weapon)
    {
       
    }

    public override void Firing(Weapon weapon)
    {

  
        //Debug.Log("Call Back EnemyFiring");
    }

    public override void HitDamageAble(IBulletDamageAble bulletDamageAble)
    {

    }

    public override void LowReady(Weapon weapon)
    {
    }

  

    public override void Reload(Weapon weapon, IReloadNodePhase reloadNodePhase)
    {
        if (reloadNodePhase is IReloadMagazineNodePhase reloadMagazineNodePhase)
        {
            switch (reloadMagazineNodePhase.curReloadPhase)
            {
                case IReloadMagazineNodePhase.ReloadMagazinePhase.Enter:
                    {
                        if (reloadNodePhase is ReloadMagazineFullStage)
                            enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.ReloadMagazineFullStage);

                        else if (reloadNodePhase is TacticalReloadMagazineFullStage)
                            enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TacticalReloadMagazineFullStage);
                    }
                    break;

                case IReloadMagazineNodePhase.ReloadMagazinePhase.Exit:
                    {
                        if (reloadNodePhase is ReloadMagazineFullStage)
                            enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.ReloadMagazineFullStage);

                        else if (reloadNodePhase is TacticalReloadMagazineFullStage)
                            enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TacticalReloadMagazineFullStage);
                    }
                    break;
            }
        }
      
    }

   

    public override void Resting(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void SwitchingWeapon(Weapon weapon, WeaponTransition weaponTransition)
    {
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.SwitchWeapon);
    }
}
