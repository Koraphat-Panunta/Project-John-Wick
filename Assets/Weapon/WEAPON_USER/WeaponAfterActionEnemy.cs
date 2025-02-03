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

  

    public override void Reload(Weapon weapon, ReloadType reloadType)
    {
        Animator animator = enemy.animator;
        if (reloadType == ReloadType.MAGAZINE_TACTICAL_RELOAD)
        {
            enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TacticalReloadMagazineFullStage);
        }
        else if (reloadType == ReloadType.MAGAZINE_RELOAD)
        {
            enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.ReloadMagazineFullStage);
        }
        else if (reloadType == ReloadType.MAGAZINE_RELOAD_SUCCESS)
        {

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
