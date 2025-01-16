using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public TrailRenderer bulletTrail;
    public void SpawnBullet(Weapon weapon)
    {
        Transform transform = gameObject.transform;
        //GameObject Bullet = Instantiate(weapon.bullet, transform.position, gameObject.transform.rotation);
        Vector3 shootDir = weapon.userWeapon.pointingPos;
        //Bullet.GetComponent<Bullet>().ShootDirection(transform.position,shootDir);

        //BulletObj thisBullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
        //thisBullet.bullet = weapon.bullet;
        //thisBullet.travelDri = (shootDir - transform.position).normalized ;    

        weapon.bullet.ShootDirection(transform.position, shootDir);
        StartCoroutine(SpawnTrail(transform.position,shootDir,this.bulletTrail));
        //if(weapon.userWeapon.TryGetComponent<Player>(out Player playerAnimationManager))
        //{
        //    Bullet.GetComponent<Bullet>().ShootDirection(playerAnimationManager.playerWeaponCommand.crosshairController.CrosshiarShootpoint.GetPointDirection(gameObject.transform.position));
        //}
        //else if((weapon.userWeapon.TryGetComponent<Enemy>(out Enemy _enemy)))
        //{
        //    Bullet.GetComponent<Bullet>().ShootDirection(_enemy.enemyGetShootDirection.GetDir());
        //}
    }
    private IEnumerator SpawnTrail(Vector3 startPos,Vector3 endPos,TrailRenderer bulletTrail)
    {
        bulletTrail = GameObject.Instantiate(bulletTrail);

        bulletTrail.enabled = true;
        bulletTrail.transform.position = startPos;
        float speedTrail = 360;

        float distance = Vector3.Distance(startPos, endPos);
        float curDistance = 0;
        float t = 0;
        while (t<=1) 
        {
            bulletTrail.transform.position = Vector3.Lerp(startPos, endPos, t);
            curDistance += speedTrail * Time.deltaTime;
            t = curDistance/distance;
            yield return null;
        }
        bulletTrail.transform.position = endPos;

        float fadeTime = 6;
        float saveFadeTime = fadeTime;

        while (fadeTime > 0)
        {
            yield return null;
        }
        bulletTrail.enabled = false;
        Destroy(bulletTrail);
    }
}
