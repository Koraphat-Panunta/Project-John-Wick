using UnityEngine;

public class PrimaryWeaponSocket : MonoBehaviour, IWeaponAttachingAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IWeaponAdvanceUser weaponAdvanceUser => character as IWeaponAdvanceUser;
    public Weapon curWeaponAtSocket { get ; set ; }
    public void OnValidate()
    {
        if(this.character == null)
            this.character = GetComponentInParent<Character>();
    }
}
