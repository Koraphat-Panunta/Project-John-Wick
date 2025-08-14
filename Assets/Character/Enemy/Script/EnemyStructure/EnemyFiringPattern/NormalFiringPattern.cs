using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFiringPattern : EnemyFiringPattern
{

    private double deltaFireTiming = 0;
    private double randomFireTiming = 0;
    private float reachRoundTime = 0;
    private const float MAXRANG_TIMING_FIRE = 0.6f;
    private const float MINRANG_TIMING_FIRE = 0.25f;
    private const float MAX_REACH_ROUND_TIME = 1.5f;
    private const float MIN_REACH_ROUND_TIME = 0.8f;


    public override bool isReadyToShoot { get => deltaFireTiming >= randomFireTiming; set { } }

    public NormalFiringPattern(EnemyCommandAPI enemyController) : base(enemyController)
    {
        randomFireTiming = MAXRANG_TIMING_FIRE;
        reachRoundTime = MIN_REACH_ROUND_TIME;
    }
    public override void Performing()
    {
        if (curWeapon == null)
            return;
        if(deltaFireTiming >= reachRoundTime)
        {
            deltaFireTiming = 0;
            randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
            reachRoundTime = Random.Range(MIN_REACH_ROUND_TIME,MAX_REACH_ROUND_TIME);
            isCheckShoot = false;
            isWillShoot = false;
        }

        deltaFireTiming += Time.deltaTime;

        if(deltaFireTiming >= randomFireTiming && isCheckShoot == false)
        {
            WillShoot();
            isCheckShoot = true;
        }
            
        if(isReadyToShoot == false)
            return;

        this.ShootingPerform();
    }
    private bool isCheckShoot;
    public bool isWillShoot;
    protected virtual void WillShoot()
    {
        isWillShoot = true;
    }
    protected void ShootingPerform()
    {
        if(isWillShoot == false)
            return;

        if (isShootAble == false)
            return;

        if (curWeapon.bulletStore[BulletStackType.Magazine] <= 0 && curWeapon.bulletStore[BulletStackType.Chamber] <= 0)
        {
            enemyController.Reload();
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
    }
    protected override void Shoot()
    {
        if (enemy._currentWeapon.fireMode == Weapon.FireMode.Single && enemy._currentWeapon.triggerState == TriggerState.Up)
        {
            base.Shoot();
            deltaFireTiming = 0;
            randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
            reachRoundTime = Random.Range(MIN_REACH_ROUND_TIME, MAX_REACH_ROUND_TIME);
            isCheckShoot = false;
            isWillShoot = false;
        }
        else if(enemy._currentWeapon.fireMode == Weapon.FireMode.FullAuto)
            base.Shoot();
    }

}

