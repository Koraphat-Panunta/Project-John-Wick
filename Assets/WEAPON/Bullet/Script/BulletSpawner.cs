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
        //GameObject Bullet = Instantiate(weapon.bullet, _transform.position, gameObject._transform.rotation);
        Vector3 shootPos = weapon.userWeapon._shootingPos;
        //Bullet.GetComponent<Bullet>().Execute(_transform.position,shootPos);

        //BulletObj thisBullet = Instantiate(bulletObj, _transform.position, Quaternion.identity);
        //thisBullet.bullet = weapon.bullet;
        //thisBullet.travelDri = (shootPos - _transform.position).normalized ;    
        Vector3 bulletHitPos = weapon.bullet.Shoot(this, shootPos);

       
        StartCoroutine(SpawnTrail(transform.position, bulletHitPos, this.bulletTrail));
        //if(weapon._userWeapon.TryGetComponent<Player>(out Player playerAnimationManager))
        //{
        //    Bullet.GetComponent<Bullet>().Execute(playerAnimationManager.playerWeaponCommand.crosshairController.CrosshiarShootpoint.GetShootPointDirection(gameObject._transform.position));
        //}
        //else if((weapon._userWeapon.TryGetComponent<Enemy>(out Enemy _enemy)))
        //{
        //    Bullet.GetComponent<Bullet>().Execute(_enemy.enemyGetShootDirection.GetShootingPos());
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

        float fadeTime = 0;
        float fadeTimeDuration = 1;

        while (fadeTime >= fadeTimeDuration)
        {
            fadeTime += Time.deltaTime;
            for (int i = 0; i < bulletTrail.colorGradient.alphaKeys.Length; i++) 
            {
                bulletTrail.colorGradient.alphaKeys[i].alpha = Mathf.Lerp(bulletTrail.colorGradient.alphaKeys[i].alpha, 0, fadeTime);
            }
            yield return null;
        }
    }
}
