using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.Fire, SpawnBullet);
    }
    private void OnDisable()
    {
        
    }
    private void SpawnBullet(Weapon weapon)
    {
        Transform transform = gameObject.transform;
        Instantiate(weapon.bullet,transform.position,gameObject.transform.rotation);
        Debug.Log("Spawn");
    }
}
