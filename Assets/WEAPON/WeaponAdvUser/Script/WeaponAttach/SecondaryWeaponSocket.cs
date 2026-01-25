using UnityEngine;

public class SecondaryWeaponSocket :  WeaponSocket
{

    [SerializeField] private Character character;
    public override Transform weaponAttachingAbleTransform { get { return this.transform; } }
    public override IWeaponAdvanceUser weaponAdvanceUser => character as IWeaponAdvanceUser;

    public override void Attatch(Weapon weapon, Vector3 additionalOffsetPosition, Quaternion additionalOffsetRotation, float attatchingDuration)
    {
        weapon._weaponAttacherComponent.Attach(
               this.weaponAttachingAbleTransform
               , weapon._mainHandGripTransform
               , additionalOffsetPosition
               , additionalOffsetRotation
               , attatchingDuration);
        base.Attatch(weapon, additionalOffsetPosition, additionalOffsetRotation, attatchingDuration);
    }
    public void OnValidate()
    {
        if (this.character == null)
            this.character = GetComponentInParent<Character>();
    }
}
