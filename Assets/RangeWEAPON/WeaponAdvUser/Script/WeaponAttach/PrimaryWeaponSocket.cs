using UnityEngine;

public class PrimaryWeaponSocket : MonoBehaviour, IGrabRangeWeaponAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IRangeWeaponAdvanceUser weaponAdvanceUser => character as IRangeWeaponAdvanceUser;
    public RangeWeapon curRangeWeaponAtSocket { get; private set; }

    public void AttatchRangeWeapon(RangeWeapon weapon)
        => this.AttatchRangeWeapon(weapon, Vector3.zero, Quaternion.identity, 0);

    public void AttatchRangeWeapon(RangeWeapon weapon,
        Vector3 additionalOffsetPosition,
        Quaternion additionalOffsetRotation,
        float attatchingDuration)
    {
        weapon._weaponAttacherComponent.Attach(
            this.weaponAttachingAbleTransform,
            weapon._mainHandGripTransform,
            additionalOffsetPosition,
            additionalOffsetRotation,
            attatchingDuration);

        this.curRangeWeaponAtSocket = weapon;
        RangeWeaponSocketBehavior.Attatch(this, weapon);
    }

    public void DetachRangeWeapon()
    {
        RangeWeaponSocketBehavior.Detach(this);
        this.curRangeWeaponAtSocket = null;
    }

    Transform IGrabAbleObject._grabSocketTransform => this.weaponAttachingAbleTransform;
    IObjectGrabbedAble IGrabAbleObject._currentGrabbedObject => this.curRangeWeaponAtSocket as IObjectGrabbedAble;
    void IGrabAbleObject.GrabAttach(IObjectGrabbedAble grabAble, Vector3 p, Quaternion r, float d)
        => RangeWeaponSocketBehavior.GrabAttach(this, grabAble, p, r, d);
    void IGrabAbleObject.GrabDetach() => RangeWeaponSocketBehavior.GrabDetach(this);

    public void OnValidate()
    {
        if (this.character == null)
            this.character = GetComponentInParent<Character>();
    }
}
