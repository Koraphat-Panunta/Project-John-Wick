using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFiringPattern : IEnemyFiringPattern
{
    private EnemyWeaponCommand weaponCommand;
    private Weapon curWeapon;
    private AmmoProuch ammoProuch;
    private double deltaFireTiming;
    private double randomFireTiming = 0;
    private const float MAXRANG_TIMING_FIRE = 3;
    private const float MINRANG_TIMING_FIRE = 1;
    public NormalFiringPattern(Enemy enemy)
    {
        this.weaponCommand = enemy.enemyWeaponCommand;
        this.curWeapon = enemy.enemyWeaponCommand.curWeapon;
        this.ammoProuch = enemy.enemyWeaponCommand.ammoProuch;
        randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
    }
    public void Performing()
    {
        
        deltaFireTiming += Time.deltaTime;
        if(deltaFireTiming >= randomFireTiming)
        {
            if(curWeapon.Magazine_count <= 0)
            {
                weaponCommand.Reload();
            }
            else
            {
                weaponCommand.Fire();
            }
            deltaFireTiming = 0;
            randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
            
        }
    }
}
