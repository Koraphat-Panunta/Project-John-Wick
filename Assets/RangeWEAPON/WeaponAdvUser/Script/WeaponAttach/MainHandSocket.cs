using UnityEngine;

public class MainHandSocket : 
    MonoBehaviour
    , IGrabRangeWeaponAble
    ,IGrabMeleeWeaponAble
{

    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;

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
                    this.weaponAdvanceUser._weaponManuverManager.reloadNodeAttachAbleSelector.AddtoChildNode(rangeWeapon._reloadSelecotrOverriden);
                }
                break;
            case MeleeWeapon meleeWeapon:
                {

                }break;
        }

       
    }
    void IGrabAbleObject.GrabDetach()
    {
        RangeWeaponSocketBehavior.GrabDetach(this);
        this.weaponAdvanceUser._weaponManuverManager.reloadNodeAttachAbleSelector.RemoveNode(this.curRangeWeaponAtSocket._reloadSelecotrOverriden);
        this._currentGrabbedObject = null;
    }


    #region IGrabRangeWeaponAble
    public IRangeWeaponAdvanceUser weaponAdvanceUser => character as IRangeWeaponAdvanceUser;
    public RangeWeapon curRangeWeaponAtSocket { get => IGrabAbleObject.GetCurentGrabAbleObjectAs<RangeWeapon>(this); }

    


    #endregion

    #region IGrabMeleeWeapon
    public IMeleeWeaponUserAble _meleeWeaponUser => character as IMeleeWeaponUserAble;
    public MeleeWeapon curMeleeWeapon => IGrabAbleObject.GetCurentGrabAbleObjectAs<MeleeWeapon>(this);

    #endregion

   

    private void OnValidate()
    {
        if (character == null)
            character = GetComponentInParent<Character>();
    }
}
