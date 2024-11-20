using UnityEngine;

public interface IWeaponAdvanceUser
{
    public Animator weaponUserAnimator { get; set; }
    public Weapon currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }//For Hoster Primary Weapon When QuickDraw
    public Vector3 pointingPos { get; set; }
    public WeaponBelt weaponBelt { get; set; }
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public void Initialized_IWeaponAdvanceUser();

}
