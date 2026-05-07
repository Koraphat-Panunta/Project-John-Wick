using UnityEngine;

public class SecondaryWeaponSocket : MonoBehaviour, IGrabRangeWeaponAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IRangeWeaponAdvanceUser weaponAdvanceUser => character as IRangeWeaponAdvanceUser;
    public RangeWeapon curRangeWeaponAtSocket { get => (this._currentGrabbedObject is RangeWeapon rangeWeapon?rangeWeapon:null); }



    Transform IGrabAbleObject._grabSocketTransform => this.weaponAttachingAbleTransform;

    public IObjectGrabbedAble _currentGrabbedObject { get; private set; }

    void IGrabAbleObject.GrabAttach(IObjectGrabbedAble grabAble, Vector3 p, Quaternion r, float d)
    {
        this._currentGrabbedObject = grabAble;
        RangeWeaponSocketBehavior.GrabAttach(this.weaponAdvanceUser, this, curRangeWeaponAtSocket, p, r, d); 
    }
    void IGrabAbleObject.GrabDetach() 
    {
        this._currentGrabbedObject = null;
        RangeWeaponSocketBehavior.GrabDetach(this);
    } 

    public void OnValidate()
    {
        if (this.character == null)
            this.character = GetComponentInParent<Character>();
    }
}
