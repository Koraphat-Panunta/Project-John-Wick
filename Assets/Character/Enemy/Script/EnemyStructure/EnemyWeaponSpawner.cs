using UnityEngine;

public class EnemyWeaponSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Enemy enemy;

    [SerializeField] Weapon[] weaponsPrefab;

    public enum WeaponType
    {
        AR15,
        Glock17
    }
    [SerializeField] private WeaponType weaponTypeSelected;
    private void Start()
    {
        switch (weaponTypeSelected)
        {
            case WeaponType.AR15:
                this.SpawnWeapon<AR15>();    
                break;
            case WeaponType.Glock17:
                this.SpawnWeapon<Glock17_9mm>();
                break;
        }
    }
   

    private void SpawnWeapon<T>() where T : Weapon
    {
        foreach (Weapon weapon in weaponsPrefab)
        {
            if(weapon is T)
            {
                T myWeapon =  GameObject.Instantiate(weapon) as T;
                new WeaponAttachingBehavior().Attach(myWeapon, enemy._weaponAdvanceUser._mainHandSocket);
                break;
            }
        }
    }
}
