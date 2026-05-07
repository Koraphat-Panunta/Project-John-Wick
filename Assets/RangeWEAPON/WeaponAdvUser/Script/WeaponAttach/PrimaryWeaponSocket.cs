using UnityEngine;

public class PrimaryWeaponSocket : MonoBehaviour
    , IGrabRangeWeaponAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IRangeWeaponAdvanceUser weaponAdvanceUser => character as IRangeWeaponAdvanceUser;
    public RangeWeapon curRangeWeaponAtSocket { get => IGrabAbleObject.GetCurentGrabAbleObjectAs<RangeWeapon>(this); }

  

    Transform IGrabAbleObject._grabSocketTransform => this.weaponAttachingAbleTransform;

    public IObjectGrabbedAble _currentGrabbedObject { get; private set; }

    void IGrabAbleObject.GrabAttach(IObjectGrabbedAble grabAble, Vector3 p, Quaternion r, float d)
    {
        this._currentGrabbedObject = grabAble;

        switch (grabAble)
        {
            case RangeWeapon rangeWeapon:
                {
                    RangeWeaponSocketBehavior.GrabAttach(this.weaponAdvanceUser, this, rangeWeapon, p, r, d);
                }
                break;
        }
       
    }
    void IGrabAbleObject.GrabDetach() 
    { 
        RangeWeaponSocketBehavior.GrabDetach(this); 
        this._currentGrabbedObject = null;
    }

    public void OnValidate()
    {
        if (this.character == null)
            this.character = GetComponentInParent<Character>();
    }
}
