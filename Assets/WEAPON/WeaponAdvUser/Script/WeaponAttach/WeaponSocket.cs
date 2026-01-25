using UnityEngine;

public abstract class WeaponSocket : MonoBehaviour 
{
    public abstract Transform weaponAttachingAbleTransform { get; }
    public abstract IWeaponAdvanceUser weaponAdvanceUser { get; }
    public Weapon curWeaponAtSocket { get; protected set; }

    public void Attatch(Weapon weapon) => this.Attatch(weapon,Vector3.zero,Quaternion.identity,0);
    public virtual void Attatch(Weapon weapon
        , Vector3 additionalOffsetPosition
        , Quaternion additionalOffsetRotation
        , float attatchingDuration
        )
    {
        
        this.curWeaponAtSocket = weapon;
        this.curWeaponAtSocket.SetCurAttatchAble(this);
       
       
    }
    public virtual void Detach()
    {
        this.curWeaponAtSocket.SetCurAttatchAble(null);
        this.curWeaponAtSocket._weaponAttacherComponent.Detach();
        this.curWeaponAtSocket = null;

    }
    
}
