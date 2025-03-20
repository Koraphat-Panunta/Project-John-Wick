using UnityEngine;

public class PlayerReference : MonoBehaviour,IWeaponAdvanceUser
{
    [SerializeField] private Player player;
    public Animator weaponUserAnimator { get => player.animator; set => player.animator = value; }
    public Weapon currentWeapon { get => player.currentWeapon; set => player.currentWeapon = value; }
    public Transform currentWeaponSocket { get => player.currentWeaponSocket ; set => player.currentWeaponSocket = value; }
    public Transform leftHandSocket { get => player.leftHandSocket; set => player.leftHandSocket = value; }
    public Vector3 shootingPos { get => player.shootingPos; set => player.shootingPos = value; }
    public WeaponBelt weaponBelt { get => player.weaponBelt; set => player.weaponBelt = value; }
    public WeaponAfterAction weaponAfterAction { get => player.weaponAfterAction; set => player.weaponAfterAction = value; }
    public WeaponCommand weaponCommand { get => player.weaponCommand; set => player.weaponCommand = value; }
    public Character userWeapon => player;
    public Vector3 pointingPos { get => player.pointingPos; set => player.pointingPos = value; }
    public WeaponManuverManager weaponManuverManager { get => player.weaponManuverManager; set => player.weaponManuverManager = value; }
    public bool isSwitchWeaponCommand { get => player.isSwitchWeaponCommand; set => isSwitchWeaponCommand = value; }
    public bool isPullTriggerCommand { get => player.isPullTriggerCommand; set => player.isPullTriggerCommand = value; }
    public bool isAimingCommand { get => player.isAimingCommand; set => player.isAimingCommand = value; }
    public bool isReloadCommand { get => player.isReloadCommand; set => player.isReloadCommand = value; }
    public FindingWeaponBehavior findingWeaponBehavior { get => player.findingWeaponBehavior; set => player.findingWeaponBehavior = value; }
    public bool isPickingUpWeaponCommand { get => player.isPickingUpWeaponCommand; set => player.isPickingUpWeaponCommand = value; }
    public bool isDropWeaponCommand { get => player.isDropWeaponCommand; set => player.isDropWeaponCommand = value; }
    public AnimatorOverrideController _animatorOverride { get => player._animatorOverride; set => player._animatorOverride = value; }

    public void Initialized_IWeaponAdvanceUser()
    {
       player.Initialized_IWeaponAdvanceUser();
    }
}
