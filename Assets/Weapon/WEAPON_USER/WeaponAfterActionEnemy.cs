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
       enemy.animator.SetLayerWeight(1, weapon.aimingWeight);
    }

    public override void Firing(Weapon weapon)
    {
        enemy.animator.SetTrigger("Firing");
        enemy.animator.SetLayerWeight(3, 1);
        enemy.StartCoroutine(enemy.RecoveryFiringLayerWeight());
        Debug.Log("Call Back EnemyFiring");
    }

    public override void LowReady(Weapon weapon)
    {
       enemy.animator.SetLayerWeight(1, weapon.aimingWeight);
    }

    public override void PreLoad(Weapon weapon)
    {
        
    }

    public override void Reload(Weapon weapon, ReloadType reloadType)
    {
        Animator animator = enemy.animator;
        if (reloadType == ReloadType.MAGAZINE_TACTICAL_RELOAD)
        {
            animator.SetTrigger("TacticalReload");
            animator.SetLayerWeight(2, 1);
        }
        else if (reloadType == ReloadType.MAGAZINE_RELOAD)
        {
            animator.SetTrigger("Reloading");
            animator.SetLayerWeight(2, 1);
        }
        else if (reloadType == ReloadType.MAGAZINE_RELOAD_SUCCESS)
        {
           enemy.StartCoroutine(enemy.RecoveryReloadLayerWeight(weapon));
        }
    }

    public override void ReloadingMagazine(Weapon weapon)
    {
        
    }

    public override void Reload_ChamberAction(Weapon weapon)
    {
        
    }

    public override void Reload_SingleAction(Weapon weapon)
    {
        
    }

    public override void Tactical_ReloadMagazine(Weapon weapon)
    {
        
    }
}
