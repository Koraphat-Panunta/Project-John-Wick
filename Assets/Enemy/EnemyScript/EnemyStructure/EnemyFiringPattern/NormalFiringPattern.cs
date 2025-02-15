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
    private EnemyCommandAPI enemyController;
    public NormalFiringPattern(EnemyCommandAPI enemyController)
    {
        this.enemy = enemyController._enemy;
        this.enemyController = enemyController;

        this.curWeapon = enemy.currentWeapon;
        this.ammoProuch = enemy.weaponBelt.ammoProuch;
        randomFireTiming = MAXRANG_TIMING_FIRE;
    }
    public void Performing()
    {
        if(curWeapon.triggerState == TriggerState.IsDown
            ||curWeapon.triggerState == TriggerState.Down)
            enemyController.CancleTrigger();

        deltaFireTiming += Time.deltaTime;

        if (deltaFireTiming < randomFireTiming)
            return;

        if (curWeapon.bulletStore[BulletStackType.Magazine] <= 0 && curWeapon.bulletStore[BulletStackType.Chamber] <= 0)
        {
            enemyController.Reload();
            deltaFireTiming = 0;
            randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
            return;
        }

        if (curWeapon.bulletStore[BulletStackType.Chamber] > 0)
        {
            //CheckFriendltFire
            Ray ray = new Ray(enemy.rayCastPos.position, (enemy.targetKnewPos - enemy.rayCastPos.position).normalized);
            if (Physics.SphereCast(ray, 0.5f, out RaycastHit hitInfo, Vector3.Distance(enemy.rayCastPos.position, enemy.targetKnewPos), LayerMask.GetMask("Enemy"))){
                
                if (hitInfo.collider.gameObject.TryGetComponent<IFriendlyFirePreventing>(out IFriendlyFirePreventing freindly)){

                    Debug.Log("isFriendlyCheck = " + freindly.IsFriendlyCheck(enemy));
                    if (freindly.IsFriendlyCheck(enemy) == false)
                        enemyController.PullTrigger();
                }
                else
                    enemyController.PullTrigger();
            }
            else
                enemyController.PullTrigger();
            
        }
        deltaFireTiming = 0;
        randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
    }
}
