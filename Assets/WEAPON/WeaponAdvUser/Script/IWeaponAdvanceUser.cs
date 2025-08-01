using UnityEngine;

public interface IWeaponAdvanceUser
{
    public Animator _weaponUserAnimator { get; set; }
    public Weapon _currentWeapon { get; set; }
    public MainHandSocket _mainHandSocket { get; set; }
    public SecondHandSocket _secondHandSocket { get; set; }//For Hoster Primary Weapon When QuickDraw
    public Vector3 _shootingPos { get; set; } //Position of bullet destinate
    public Vector3 _pointingPos { get; set; } //Position of aiming
    public WeaponBelt _weaponBelt { get; set; }
    public WeaponAfterAction _weaponAfterAction { get; set; }
    public Character _userWeapon { get;}
    public WeaponManuverManager _weaponManuverManager { get; set; }
    public FindingWeaponBehavior _findingWeaponBehavior { get; set; }
    public bool _isPullTriggerCommand { get; set; }
    public bool _isAimingCommand { get; set; }
    public bool _isReloadCommand { get; set; }
    public bool _isDropWeaponCommand { get; set; }
    public bool _isPickingUpWeaponCommand { get; set; }
    public bool _isHolsterWeaponCommand { get; set; }
    public bool _isDrawPrimaryWeaponCommand { get; set; }
    public bool _isDrawSecondaryWeaponCommand { get; set; }
    public AnimatorOverrideController _animatorWeaponAdvanceUserOverride{ get; set; }
    public void Initialized_IWeaponAdvanceUser();
}
public class FindingWeaponBehavior
{
    private IWeaponAdvanceUser weaponAdvanceUser;
    public Weapon weaponFindingSelecting { get;private set; }
    public void SetWeaponFindingSelecting(Weapon weapon) => weaponFindingSelecting = weapon;

    public readonly float findingWeaponRaduisDefault = 1;

    private LayerMask layerMask;
    public FindingWeaponBehavior(IWeaponAdvanceUser weaponAdvanceUser)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.layerMask = LayerMask.GetMask("Weapon");
    }
    public bool FindingWeapon()
    {
        
        if(FindingWeapon(weaponAdvanceUser._userWeapon.transform.position, findingWeaponRaduisDefault))
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

            if (Vector3.Distance(weaponAdvanceUser._userWeapon.transform.position, weaponFindingSelecting.transform.position) >
                Vector3.Distance(weaponAdvanceUser._userWeapon.transform.position, collider[i].transform.position))
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
