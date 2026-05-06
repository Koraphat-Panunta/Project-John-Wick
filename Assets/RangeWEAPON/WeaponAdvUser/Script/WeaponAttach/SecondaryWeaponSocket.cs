using UnityEngine;

public class SecondaryWeaponSocket : MonoBehaviour, IGrabWeaponAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IWeaponAdvanceUser weaponAdvanceUser => character as IWeaponAdvanceUser;
    public Weapon curWeaponAtSocket { get; private set; }

    public void Attatch(Weapon weapon)
        => this.Attatch(weapon, Vector3.zero, Quaternion.identity, 0);

    public void Attatch(Weapon weapon,
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

        this.curWeaponAtSocket = weapon;
        WeaponSocketBehavior.Attatch(this, weapon);
    }

    public void Detach()
    {
        WeaponSocketBehavior.Detach(this);
        this.curWeaponAtSocket = null;
    }

    Transform IObjectGrabbedAble.grabSocketTransform => this.weaponAttachingAbleTransform;
    IGrabAbleObject IObjectGrabbedAble.currentGrabbedObject => this.curWeaponAtSocket as IGrabAbleObject;
    void IObjectGrabbedAble.GrabAttach(IGrabAbleObject grabAble, Vector3 p, Quaternion r, float d)
        => WeaponSocketBehavior.GrabAttach(this, grabAble, p, r, d);
    void IObjectGrabbedAble.GrabDetach() => WeaponSocketBehavior.GrabDetach(this);

    public void OnValidate()
    {
        if (this.character == null)
            this.character = GetComponentInParent<Character>();
    }
}
