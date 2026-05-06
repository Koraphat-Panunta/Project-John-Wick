using UnityEngine;

public class SecondHandSocket :  WeaponSocket
{
    [SerializeField] private Character character;
    public override Transform weaponAttachingAbleTransform { get { return this.transform; } }
    public override IWeaponAdvanceUser weaponAdvanceUser => character as IWeaponAdvanceUser;

    public override void Attatch(Weapon weapon, Vector3 additionalOffsetPosition, Quaternion additionalOffsetRotation, float attatchingDuration)
    {
        weapon._weaponAttacherComponent.Attach(
                this.weaponAttachingAbleTransform
                , weapon._SecondHandGripTransform
                , additionalOffsetPosition
                , additionalOffsetRotation
                , attatchingDuration);

        base.Attatch(weapon, additionalOffsetPosition, additionalOffsetRotation, attatchingDuration);
    }

    private void OnValidate()
    {
        if (character == null)
            character = GetComponentInParent<Character>();
    }
}
