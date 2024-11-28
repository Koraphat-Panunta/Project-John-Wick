using UnityEngine;

public class WeaponAnimationBasedEvent : MonoBehaviour
{
    private Weapon weapon;
    void Start()
    {
        weapon = GetComponent<Weapon>();
    }

   public void ReloadMagazineOutEvent()
    {
        (weapon as MagazineType).isMagIn = false;
    }
    public void ReloadMagazineInEvent()
    {
        (weapon as MagazineType).isMagIn = true;
        weapon.bulletStore[BulletStackType.Magazine] = weapon.Magazine_capacity;
    }
    public void ChamberLoadMagazineEvent()
    {
        weapon.bulletStore[BulletStackType.Chamber] += 1;
        weapon.bulletStore[BulletStackType.Magazine] -= 1;
    }
    public void ChamberLoadEvent()
    {
        weapon.bulletStore[BulletStackType.Chamber] += 1;
    }
    public void LoadManualEvent()
    {
        weapon.bulletStore[BulletStackType.Magazine] += 1;
    }
}
