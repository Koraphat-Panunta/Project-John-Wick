using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] WeaponSingleton weaponSingleton;
    [SerializeField] Transform targerPos;
    private void OnEnable()
    {
        weaponSingleton.FireEvent += SpawnBullet;
    }
    private void OnDisable()
    {
        
    }
    private void SpawnBullet(Weapon weapon)
    {
        Transform transform = gameObject.transform;
        GameObject Bullet = Instantiate(weapon.bullet, transform.position, gameObject.transform.rotation);
        WeaponSingleton weaponSingleton = weapon.GetComponent<WeaponSingleton>();
        if(weaponSingleton.UserWeapon.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            Bullet.GetComponent<Bullet>().ShootDirection(playerController.crosshairController.CrosshiarShootpoint.GetPointDirection(gameObject.transform.position));
        }
        else if(weaponSingleton.UserWeapon.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Bullet.GetComponent<Bullet>().ShootDirection(enemy.enemyGetShootDirection.GetDir());
        }
    }
}
