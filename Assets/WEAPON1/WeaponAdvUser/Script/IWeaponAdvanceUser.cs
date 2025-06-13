using UnityEngine;

public interface IWeaponAdvanceUser
{
    public Animator weaponUserAnimator { get; set; }
    public Weapon _currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }//For Hoster Primary Weapon When QuickDraw
    public Vector3 shootingPos { get; set; } //Position of bullet destinate
    public Vector3 pointingPos { get; set; } //Position of aiming
    public WeaponBelt weaponBelt { get; set; }
    public WeaponAfterAction weaponAfterAction { get; set; }
    public Character userWeapon { get;}
    public WeaponManuverManager weaponManuverManager { get; set; }
    public FindingWeaponBehavior findingWeaponBehavior { get; set; }
    public bool isSwitchWeaponCommand { get; set; }
    public bool isPullTriggerCommand { get; set; }
    public bool isAimingCommand { get; set; }
    public bool isReloadCommand { get; set; }
    public bool isPickingUpWeaponCommand { get; set; }
    public bool isDropWeaponCommand { get; set; }
    public AnimatorOverrideController _animatorOverride{ get; set; }
    public void Initialized_IWeaponAdvanceUser();
}
public class FindingWeaponBehavior
{
    private IWeaponAdvanceUser weaponAdvanceUser;
    public Weapon weaponFindingSelecting { get;private set; }

    public readonly float findingWeaponRaduisDefault = 1;

    private LayerMask layerMask;
    public FindingWeaponBehavior(IWeaponAdvanceUser weaponAdvanceUser)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.layerMask = LayerMask.GetMask("Weapon");
    }
    public bool FindingWeapon()
    {
        
        if(FindingWeapon(weaponAdvanceUser.userWeapon.transform.position, findingWeaponRaduisDefault))
            return true;

        return false;
    }

    public bool FindingWeapon(Vector3 center,float raduis)
    {
        weaponFindingSelecting = null;

        Collider[] collider = Physics.OverlapSphere(center, raduis, layerMask.value, QueryTriggerInteraction.UseGlobal);

        if (collider.Length <= 0)
            return false;

        for (int i = 0; i < collider.Length; i++)
        {
            if (weaponFindingSelecting == null)
            {
                if (collider[i].TryGetComponent<Weapon>(out Weapon weapon))
                {
                    if (weapon.userWeapon == null)
                        weaponFindingSelecting = weapon;
                    continue;
                }
                continue;
            }

            if (Vector3.Distance(weaponAdvanceUser.userWeapon.transform.position, weaponFindingSelecting.transform.position) >
                Vector3.Distance(weaponAdvanceUser.userWeapon.transform.position, collider[i].transform.position))
            {
                if (collider[i].GetComponent<Weapon>().userWeapon == null)
                    weaponFindingSelecting = collider[i].GetComponent<Weapon>();
            }
        }

        if (weaponFindingSelecting != null)
            return true;



        return false;
    }

}
