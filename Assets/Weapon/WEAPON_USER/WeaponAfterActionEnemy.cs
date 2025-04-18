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

  

    public override void Reload(Weapon weapon, IReloadNode reloadNodePhase)
    {
        AutoRegenAmmo();
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
    private void AutoRegenAmmo()
    {
        if (enemy.weaponBelt.ammoProuch.amountOf_ammo[enemy._currentWeapon.bullet.myType] <= 0)
        {
            enemy.weaponBelt.ammoProuch.AddAmmo(enemy._currentWeapon.bullet.myType, 100);
        }
    }


    public override void Resting(Weapon weapon)
    {

    }

    public override void SwitchingWeapon(Weapon weapon, WeaponManuverLeafNode weaponTransitionNodeLeaf)
    {
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.SwitchWeapon);
    }
}
