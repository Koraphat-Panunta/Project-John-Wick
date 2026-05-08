using UnityEngine;

public class AttatchGrabAbleObjectEventVirtualEventNode : VirtualEventNode
{
    [SerializeField] GameObject curWeapon;
    [SerializeField] public Character weaponUser;

    public enum AttatchType
    {
        Attatch,
        Holster,
    }

    public AttatchType attachType = AttatchType.Attatch;

    public override void Execute()
    {
        if(curWeapon.TryGetComponent<IObjectGrabbedAble>(out IObjectGrabbedAble grabAbleObject) == false)
        {
            base.Execute();
            return;
        }

        switch (grabAbleObject)
        {
            case RangeWeapon rangeWeapon:
                {
                    if (rangeWeapon != null
                        && weaponUser != null
                        && weaponUser is IRangeWeaponAdvanceUser weaponAdvanceUser)
                    {
                        switch (attachType)
                        {
                            case AttatchType.Attatch:
                                WeaponAttachingBehavior.Attach(rangeWeapon, weaponAdvanceUser._mainHandSocket, 0);
                                break;
                            case AttatchType.Holster:
                                {
                                    if (rangeWeapon is PrimaryWeapon)
                                        WeaponAttachingBehavior.Attach(rangeWeapon, weaponAdvanceUser._weaponBelt.primaryWeaponSocket, 0);
                                    else
                                        WeaponAttachingBehavior.Attach(rangeWeapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket, 0);
                                }
                                break;

                        }
                    }
                    break;
                }
            case MeleeWeapon meleeWeapon:
                {

                    if (this.weaponUser is IMeleeWeaponUserAble meleeWeaponUser == false)
                        break;
                    
                    WeaponAttachingBehavior.Attach(meleeWeapon, meleeWeaponUser._grabMeleeWeaponAble, 0);
                }
                break;
        }
        
        base.Execute();
    }
    protected override void OnDrawGizmos()
    {
        if (isEnableGizmos
            && curWeapon != null
            && weaponUser != null)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(this.transform.position, curWeapon.transform.position);
            Gizmos.DrawLine(this.transform.position, weaponUser.transform.position);
        }
        base.OnDrawGizmos();
    }
}
