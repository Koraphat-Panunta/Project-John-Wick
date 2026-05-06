using UnityEngine;

public class AttatchWeaponEventVirtualEventNode : VirtualEventNode
{
    [SerializeField] RangeWeapon curWeapon;
    [SerializeField] public Character weaponUser;

    public enum AttatchType
    {
        Attatch,
        Holster,
    }

    public AttatchType attachType = AttatchType.Attatch;

    public override void Execute()
    {
        if(curWeapon != null 
            && weaponUser != null
            && weaponUser is IWeaponAdvanceUser weaponAdvanceUser)
        {
            switch (attachType)
            {
                case AttatchType.Attatch: 
                    WeaponAttachingBehavior.Attach(curWeapon, weaponAdvanceUser._mainHandSocket, 0);
                    break;
                case AttatchType.Holster:
                    {
                        if(curWeapon is PrimaryWeapon)
                            WeaponAttachingBehavior.Attach(curWeapon, weaponAdvanceUser._weaponBelt.primaryWeaponSocket, 0);
                        else
                            WeaponAttachingBehavior.Attach(curWeapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket, 0);
                    }
                    break;
               
            }
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
