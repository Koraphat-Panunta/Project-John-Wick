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
    private const float MAXRANG_TIMING_FIRE = 0.6f;
    private const float MINRANG_TIMING_FIRE = 0.2f;
    private Enemy enemy;
    public NormalFiringPattern(Enemy enemy)
    {
        this.weaponCommand = enemy.enemyWeaponCommand;
        this.curWeapon = enemy.currentWeapon;
        this.ammoProuch = enemy.enemyWeaponCommand.ammoProuch;
        randomFireTiming = MAXRANG_TIMING_FIRE;
        this.enemy = enemy;
    }
    public void Performing()
    {
        
        deltaFireTiming += Time.deltaTime;
        if (deltaFireTiming >= randomFireTiming)
        {
            if(curWeapon.Magazine_count <= 0&&curWeapon.Chamber_Count<=0)
            {
                enemy.weaponCommand.Reload(enemy.weaponBelt.ammoProuch);
            }
            else if(curWeapon.weapon_stateManager._currentState != curWeapon.weapon_stateManager.reloadState)
            {
                Ray ray = new Ray(enemy.rayCastPos.position,(enemy.Target.transform.position- enemy.rayCastPos.position).normalized);
                if (Physics.SphereCast(ray, 0.5f,out RaycastHit hitInfo, Vector3.Distance(enemy.rayCastPos.position, enemy.Target.transform.position), LayerMask.GetMask("Enemy")))
                {
                    if(hitInfo.collider.gameObject.TryGetComponent<BodyPart>(out BodyPart body))
                    {
                        if (body.enemy != enemy)
                        {
                            
                        }
                        else
                        {
                            enemy.weaponCommand.PullTrigger();
                        }
                    }
                }
                else
                {
                    enemy.weaponCommand.PullTrigger();
                }
            }
            deltaFireTiming = 0;
            randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);   
        }
    }
}
