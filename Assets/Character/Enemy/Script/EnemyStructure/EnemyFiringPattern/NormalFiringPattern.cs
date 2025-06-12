using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFiringPattern : EnemyFiringPattern
{

    private double deltaFireTiming = 0;
    private double randomFireTiming = 0;
    private const float MAXRANG_TIMING_FIRE = 0.6f;
    private const float MINRANG_TIMING_FIRE = 0.25f;

    public override bool isReadyToShoot { get => deltaFireTiming >= randomFireTiming; set { } }

    public NormalFiringPattern(EnemyCommandAPI enemyController) : base(enemyController)
    {
        randomFireTiming = MAXRANG_TIMING_FIRE;
    }
    public override void Performing()
    {
        if (curWeapon == null)
            return;

        deltaFireTiming += Time.deltaTime;

        if(isReadyToShoot == false)
            return;

        if(isShootAble == false)
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
   
}

