using UnityEngine;

public interface IWeaponAdvanceUser
{
    public Animator weaponUserAnimator { get; set; }
    public Weapon currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }//For Hoster Primary Weapon When QuickDraw
    public Vector3 shootingPos { get; set; } //Position of bullet destinate
    public Vector3 pointingPos { get; set; } //Position of aiming
    public WeaponBelt weaponBelt { get; set; }
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public Character userWeapon { get;}
    public WeaponManuverManager weaponManuverManager { get; set; }
    public FindingWeaponBehavior findingWeaponBehavior { get; set; }
    public bool isSwitchWeaponCommand { get; set; }
    public bool isPullTriggerCommand { get; set; }
    public bool isAimingCommand { get; set; }
    public bool isReloadCommand { get; set; }
    public bool isPickingUpWeaponCommand { get; set; }
    public void Initialized_IWeaponAdvanceUser();
}
public class FindingWeaponBehavior
{
    private IWeaponAdvanceUser weaponAdvanceUser;
    public Weapon weaponFindingSelecting { get;private set; }

    private LayerMask layerMask;
    public FindingWeaponBehavior(IWeaponAdvanceUser weaponAdvanceUser)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.layerMask = LayerMask.GetMask("Weapon");
    }
    public bool FindingWeapon()
    {
        Debug.Log("FindingWeapon");


        Collider[] collider = Physics.OverlapSphere(weaponAdvanceUser.userWeapon.transform.position,1, layerMask, QueryTriggerInteraction.UseGlobal);

        if (collider.Length <= 0)
            return false;

        for (int i = 0; i < collider.Length; i++)
        {
            Debug.Log("collider weapon =" + collider[i] + " num = "+i);
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
                weaponFindingSelecting = collider[i].GetComponent<Weapon>();
        }

        if (weaponFindingSelecting != null)
            return true;

        return false;
    }

}
