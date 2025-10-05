using UnityEngine;

public class WeaponStartAttacher : MonoBehaviour, IInitializedAble
{
    [SerializeField] Weapon curWeapon;
    [SerializeField] MonoBehaviour weaponUser;
    public void Initialized()
    {
        if(weaponUser.TryGetComponent<IWeaponAdvanceUser>(out IWeaponAdvanceUser weaponAdvanceUser))
        {
            WeaponAttachingBehavior.Attach(curWeapon,weaponAdvanceUser._mainHandSocket);
        }
    }
}
