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
    private const float MINRANG_TIMING_FIRE = 0.25f;
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
            if (Physics.SphereCast(ray, 0.5f, out RaycastHit hitInfo, Vector3.Distance(enemy.rayCastPos.position, enemy.targetKnewPos), LayerMask.GetMask("Enemy")))
            {

                if (hitInfo.collider.gameObject.TryGetComponent<IFriendlyFirePreventing>(out IFriendlyFirePreventing freindly))
                {
                    if (freindly.IsFriendlyCheck(enemy) == false)
                        Shoot();
                }
                else
                    Shoot();
            }
            else
                Shoot();


        }
        deltaFireTiming = 0;
        randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
    }
    private void Shoot()
    {
        if (DetectObstacle(1))
       return;
        enemyController.PullTrigger();

    }
    private bool DetectObstacle(float distance)
    {
        int DefaultMask = LayerMask.GetMask("Default");
        int BodyPartMask = LayerMask.GetMask("Enemy");
        int GroundHitMask = LayerMask.GetMask("Ground");

        LayerMask layerMask  = DefaultMask + BodyPartMask + GroundHitMask;
        Ray ray = new Ray(enemy.rayCastPos.position,enemy.rayCastPos.forward);
        Debug.DrawLine(enemy.rayCastPos.position, enemy.rayCastPos.forward*distance);
        if (Physics.SphereCast(ray, 0.0015f, distance, layerMask))
        {
            return true;
        }
        
        return false;
    }
}
