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
    public bool isSwitchWeaponCommand { get; set; }
    public bool isPullTriggerCommand { get; set; }
    public bool isAimingCommand { get; set; }
    public bool isReloadCommand { get; set; }
    public void Initialized_IWeaponAdvanceUser();
}
