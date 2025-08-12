using UnityEngine;

public class EnemyWeaponSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Enemy enemy;

    //[SerializeField] Weapon[] weaponsPrefab;
    [SerializeField] private Weapon curWeaponUsed;


    //public enum WeaponType
    //{
    //    AR15,
    //    Glock17
    //}
    //[SerializeField] private WeaponType weaponTypeSelected;
    private void Start()
    {
        new WeaponAttachingBehavior().Attach(curWeaponUsed, enemy._weaponAdvanceUser._mainHandSocket);
    }

    //private void SelectedSpawnWeapon()
    //{
    //    switch (weaponTypeSelected)
    //    {
    //        case WeaponType.AR15:
    //            this.SpawnWeapon<AR15>();
    //            break;
    //        case WeaponType.Glock17:
    //            this.SpawnWeapon<Glock17_9mm>();
    //            break;
    //    }
    //}
    //private void SpawnWeapon<T>() where T : Weapon
    //{
    //    foreach (Weapon weapon in weaponsPrefab)
    //    {
    //        if (weapon is T)
    //        {
    //            curWeaponUsed = GameObject.Instantiate(weapon);
    //            break;
    //        }
    //    }
    //}

}
