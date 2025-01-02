using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFiringPattern : IEnemyFiringPattern
{
    private Weapon curWeapon;
    private AmmoProuch ammoProuch;
    private double deltaFireTiming = 0;
    private double randomFireTiming = 0;
    private const float MAXRANG_TIMING_FIRE = 0.6f;
    private const float MINRANG_TIMING_FIRE = 0.2f;
    private Enemy enemy;
    public NormalFiringPattern(EnemyControllerAPI enemyController)
    {
        this.curWeapon = enemyController.currentWeapon;
        this.ammoProuch = enemyController.weaponBelt.ammoProuch;
        randomFireTiming = MAXRANG_TIMING_FIRE;
        this.enemy = enemyController;
    }
    public void Performing()
    {
        if(curWeapon.triggerState == TriggerState.IsDown
            ||curWeapon.triggerState == TriggerState.Down)
            enemy.weaponCommand.CancleTrigger();

        deltaFireTiming += Time.deltaTime;
        if (deltaFireTiming >= randomFireTiming)
        {
            if(curWeapon.bulletStore[BulletStackType.Magazine] <= 0&&curWeapon.bulletStore[BulletStackType.Chamber]<=0)
            {
                enemy.weaponCommand.Reload(enemy.weaponBelt.ammoProuch);
            }
            else if (curWeapon.bulletStore[BulletStackType.Chamber]>0)
            {
                Ray ray = new Ray(enemy.rayCastPos.position,(enemy.targetKnewPos- enemy.rayCastPos.position).normalized);
                if (Physics.SphereCast(ray, 0.5f,out RaycastHit hitInfo, Vector3.Distance(enemy.rayCastPos.position, enemy.targetKnewPos), LayerMask.GetMask("Enemy")))
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
