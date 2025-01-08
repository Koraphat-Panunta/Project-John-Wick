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
    private WeaponCommand weaponCommand;
    public NormalFiringPattern(EnemyCommandAPI enemyController)
    {
        this.enemy = enemyController.enemy;

        this.curWeapon = enemy.currentWeapon;
        this.ammoProuch = enemy.weaponBelt.ammoProuch;
        randomFireTiming = MAXRANG_TIMING_FIRE;
        this.weaponCommand = enemy.weaponCommand;
    }

    public NormalFiringPattern(Enemy enemy)
    {
        this.enemy = enemy;

        this.curWeapon = enemy.currentWeapon;
        this.ammoProuch = enemy.weaponBelt.ammoProuch;
        randomFireTiming = MAXRANG_TIMING_FIRE;
        this.weaponCommand = enemy.weaponCommand;
    }
    public void Performing()
    {
        if(curWeapon.triggerState == TriggerState.IsDown
            ||curWeapon.triggerState == TriggerState.Down)
            weaponCommand.CancleTrigger();

        deltaFireTiming += Time.deltaTime;

        if (deltaFireTiming < randomFireTiming)
            return;

        if (curWeapon.bulletStore[BulletStackType.Magazine] <= 0 && curWeapon.bulletStore[BulletStackType.Chamber] <= 0)
        {
            weaponCommand.Reload(enemy.weaponBelt.ammoProuch);
            deltaFireTiming = 0;
            randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
            return;
        }

        if (curWeapon.bulletStore[BulletStackType.Chamber] > 0)
        {
            Ray ray = new Ray(enemy.rayCastPos.position, (enemy.targetKnewPos - enemy.rayCastPos.position).normalized);
            if (Physics.SphereCast(ray, 0.5f, out RaycastHit hitInfo, Vector3.Distance(enemy.rayCastPos.position, enemy.targetKnewPos), LayerMask.GetMask("Enemy"))){
                
                if (hitInfo.collider.gameObject.TryGetComponent<BodyPart>(out BodyPart body)){

                    if (body.enemy != enemy){}
                    else{ weaponCommand.PullTrigger(); }
                }
            }
            else
                weaponCommand.PullTrigger();
            
        }
        deltaFireTiming = 0;
        randomFireTiming = Random.Range(MINRANG_TIMING_FIRE, MAXRANG_TIMING_FIRE);
    }
}
