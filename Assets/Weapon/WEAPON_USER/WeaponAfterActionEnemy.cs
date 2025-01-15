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

    public override void LowReady(Weapon weapon)
    {
    }

    public override void PreLoad(Weapon weapon)
    {
        
    }

    public override void Reload(Weapon weapon, ReloadType reloadType)
    {
        Animator animator = enemy.animator;
        if (reloadType == ReloadType.MAGAZINE_TACTICAL_RELOAD)
        {
            
        }
        else if (reloadType == ReloadType.MAGAZINE_RELOAD)
        {
            
        }
        else if (reloadType == ReloadType.MAGAZINE_RELOAD_SUCCESS)
        {

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
