using UnityEngine;

public class SecondHandSocket : MonoBehaviour, IGrabRangeWeaponAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IRangeWeaponAdvanceUser weaponAdvanceUser => character as IRangeWeaponAdvanceUser;
    public RangeWeapon curRangeWeaponAtSocket { get => IGrabAbleObject.GetCurentGrabAbleObjectAs<RangeWeapon>(this); }

    public IObjectGrabbedAble _currentGrabbedObject { get; private set; }

    Transform IGrabAbleObject._grabSocketTransform => this.weaponAttachingAbleTransform;

    void IGrabAbleObject.GrabAttach(IObjectGrabbedAble grabAble, Vector3 p, Quaternion r, float d)
    {
        this._currentGrabbedObject = grabAble;

        if (grabAble is RangeWeapon rangeWeapon)
            RangeWeaponSocketBehavior.GrabAttach(this.weaponAdvanceUser, this, rangeWeapon, p, r, d);
    }

    void IGrabAbleObject.GrabDetach() 
    { 
        RangeWeaponSocketBehavior.GrabDetach(this); 
        this._currentGrabbedObject = null;
    }

    private void OnValidate()
    {
        if (character == null)
            character = GetComponentInParent<Character>();
    }
}
