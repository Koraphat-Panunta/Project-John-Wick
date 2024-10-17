using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFiringPattern : IEnemyFiringPattern
{
    private EnemyWeaponCommand weaponCommand;
    private Weapon curWeapon;
    private AmmoProuch ammoProuch;
    private double deltaFireTiming = 0;
    private double randomFireTiming = 0;
    private const float MAXRANG_TIMING_FIRE = 1f;
    private const float MINRANG_TIMING_FIRE = 0.3f;
    public NormalFiringPattern(Enemy enemy)
    {
        this.weaponCommand = enemy.enemyWeaponCommand;
        this.curWeapon = enemy.curentWeapon;
        this.ammoProuch = enemy.enemyWeaponCommand.ammoProuch;
        randomFireTiming = MAXRANG_TIMING_FIRE;
    }
    public void Performing()
    {
        
        deltaFireTiming += Time.deltaTime;
        Debug.Log("Normal Firing Pattern Performing deltaFireTiming : " + deltaFireTiming);
        if (deltaFireTiming >= randomFireTiming)
        {
            Debug.Log("DeltaTiming reset");
            if(curWeapon.Magazine_count <= 0&&curWeapon.Chamber_Count<=0)
            {
                weaponCommand.Reload();
                Debug.Log("Enemy Reload");
            }
            else if(curWeapon.weapon_stateManager._currentState != curWeapon.weapon_stateManager.reloadState)
            {
                weaponCommand.Fire();
                Debug.Log("Enemy Firing");
            }
            deltaFireTiming = 0;
            randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);   
        }
    }
}
