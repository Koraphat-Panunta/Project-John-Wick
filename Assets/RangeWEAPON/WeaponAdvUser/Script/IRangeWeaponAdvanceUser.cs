using UnityEngine;

public interface IRangeWeaponAdvanceUser : IWeaponUser
{
    public Animator _weaponUserAnimator { get; set; }
    public RangeWeapon _currentWeapon { get; }
    public MainHandSocket _mainHandSocket { get; set; }
    public SecondHandSocket _secondHandSocket { get; set; }//For Hoster Primary RangeWeapon When QuickDraw
    public Vector3 _shootingPos { get; set; } //Position of bullet destinate
    public Vector3 _pointingPos { get; set; } //Position of aiming
    public WeaponBelt _weaponBelt { get; set; }
    public WeaponAfterAction _weaponAfterAction { get; set; }
    public WeaponNodeManuverManager _weaponManuverManager { get; set; }
    public FindingWeaponBehavior _findingWeaponBehavior { get; set; }
    public bool _isPullTriggerCommand { get; set; }
    public bool _isAimingCommand { get; set; }
    public bool _isReloadCommand { get; set; }
    public bool _isDropWeaponCommand { get; set; }
    public bool _isPickingUpWeaponCommand { get; set; }
    public bool _isHolsterWeaponCommand { get; set; }
    public bool _isDrawPrimaryWeaponCommand { get; set; }
    public bool _isDrawSecondaryWeaponCommand { get; set; }
    public float _ReloadDuration { get; }

    public void Initialized_IWeaponAdvanceUser();

}
public class FindingWeaponBehavior
{
    private IRangeWeaponAdvanceUser weaponAdvanceUser;
    public RangeWeapon weaponFindingSelecting { get;private set; }
    public void SetWeaponFindingSelecting(RangeWeapon weapon) => weaponFindingSelecting = weapon;

    public readonly float findingWeaponRaduisDefault = 1;

    private LayerMask layerMask;
    public FindingWeaponBehavior(IRangeWeaponAdvanceUser weaponAdvanceUser)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.layerMask = LayerMask.GetMask("RangeWeapon");
    }
    public bool FindingWeapon()
    {
        
        if(FindingWeapon(weaponAdvanceUser._character.transform.position, findingWeaponRaduisDefault))
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
                if (collider[i].TryGetComponent<RangeWeapon>(out RangeWeapon weapon))
                {
                    if (weapon.userWeapon == null)
                        weaponFindingSelecting = weapon;
                    continue;
                }
                continue;
            }

            if (Vector3.Distance(weaponAdvanceUser._character.transform.position, weaponFindingSelecting.transform.position) >
                Vector3.Distance(weaponAdvanceUser._character.transform.position, collider[i].transform.position))
            {
                if (collider[i].GetComponent<RangeWeapon>().userWeapon == null)
                    weaponFindingSelecting = collider[i].GetComponent<RangeWeapon>();
            }
        }

        if (weaponFindingSelecting != null)
            return true;



        return false;
    }

}
