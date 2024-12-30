using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
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
        //if(weapon.userWeapon.TryGetComponent<Player>(out Player player))
        //{
        //    Bullet.GetComponent<Bullet>().ShootDirection(player.playerWeaponCommand.crosshairController.CrosshiarShootpoint.GetPointDirection(gameObject.transform.position));
        //}
        //else if((weapon.userWeapon.TryGetComponent<Enemy>(out Enemy enemy)))
        //{
        //    Bullet.GetComponent<Bullet>().ShootDirection(enemy.enemyGetShootDirection.GetDir());
        //}
    }
}
