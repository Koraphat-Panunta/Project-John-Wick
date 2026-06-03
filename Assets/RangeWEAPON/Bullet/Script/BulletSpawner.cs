using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public TrailRenderer bulletTrail;

    int tracerNum = 3;
    int num = 0;

    public void SpawnBullet(Bullet bullet,Vector3 shootPos)
    {
        Transform transform = gameObject.transform;
        //GameObject Bullet = Instantiate(weapon.bullet, _transform.position, gameObject._transform.rotation);
        //Bullet.GetComponent<Bullet>().Execute(_transform.position,shootPos);

        //BulletObj thisBullet = Instantiate(bulletObj, _transform.position, Quaternion.identity);
        //thisBullet.bullet = weapon.bullet;
        //thisBullet.travelDri = (shootPos - _transform.position).normalized ;    
        Vector3 bulletHitPos = bullet.Shoot(this, shootPos);

        //Debug.DrawLine(this.transform.position, shootPos, Color.red, 5);
        //Debug.DrawLine(this.transform.position, bulletHitPos,Color.green,5);

        Vector3 bulletHitDir = bulletHitPos - transform.position;
        Vector3 shootPointDir = shootPos - transform.position;



        if (Vector3.Dot(bulletHitDir, shootPointDir) > .9 )
            StartCoroutine(SpawnTrail(transform.position , bulletHitPos, this.bulletTrail));
        //if(weapon._userWeapon.TryGetComponent<Player>(out Player playerAnimationManager))
        //{
        //    Bullet.GetComponent<Bullet>().Execute(playerAnimationManager.playerWeaponCommand.crosshairController.CrosshiarShootpoint.GetShootPointDirection(gameObject._transform.position));
        //}
        //else if((weapon._userWeapon.TryGetComponent<Enemy>(out Enemy _enemy)))
        //{
        //    Bullet.GetComponent<Bullet>().Execute(_enemy.enemyGetShootDirection.GetShootingPos());
        //}
    }
    public IEnumerator SpawnTrail(Vector3 startPos,Vector3 endPos,TrailRenderer bulletTrail)
    {
        bulletTrail = GameObject.Instantiate(bulletTrail);

        bulletTrail.enabled = true;
        bulletTrail.transform.position = startPos;
        float speedTrail = 360;

        float distance = Vector3.Distance(startPos, endPos);
        float curDistance = 0;
        float t = 0;
        while (t <= 1)
        {
            if (bulletTrail == null) yield break;
            bulletTrail.transform.position = Vector3.Lerp(startPos, endPos, t);
            curDistance += speedTrail * Time.deltaTime;
            t = curDistance / distance;
            yield return null;
        }

        if (bulletTrail == null) yield break;
        bulletTrail.transform.position = endPos;

        float fadeTime = 0;
        float fadeTimeDuration = 1;

        while (fadeTime < fadeTimeDuration)
        {
            if (bulletTrail == null) yield break;
            fadeTime += Time.deltaTime;

            // colorGradient returns a copy — must write the modified gradient back
            Gradient gradient = bulletTrail.colorGradient;
            GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
            for (int i = 0; i < alphaKeys.Length; i++)
                alphaKeys[i].alpha = Mathf.Lerp(alphaKeys[i].alpha, 0, fadeTime / fadeTimeDuration);
            gradient.alphaKeys = alphaKeys;
            bulletTrail.colorGradient = gradient;

            yield return null;
        }

        if (bulletTrail != null)
            Destroy(bulletTrail.gameObject);
    }
}
