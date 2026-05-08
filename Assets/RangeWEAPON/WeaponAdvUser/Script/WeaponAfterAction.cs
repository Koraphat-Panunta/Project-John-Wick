using UnityEngine;

public abstract class WeaponAfterAction 
{
    public enum WeaponAfterActionSending
    {
        WeaponStateNodeActive,
        HitConfirm
    }
    public abstract void SendFeedBackWeaponAfterAction<T>(WeaponAfterActionSending weaponAfterActionSending,T Var);
}
