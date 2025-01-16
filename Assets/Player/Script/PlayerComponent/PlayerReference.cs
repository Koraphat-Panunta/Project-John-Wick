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

    public bool isAiming { get => player.isAiming; set => player.isAiming = value; }
    public bool isPullTrigger { get => player.isPullTrigger; set => player.isPullTrigger = value; }
    public bool isReload { get => player.isReload; set => player.isReload = value; }
    public bool isSwapShoulder { get => player.isSwapShoulder; set => player.isSwapShoulder = value; }
    public bool isSwitchWeapon { get => player.isSwitchWeapon; set => player.isSwitchWeapon = value; }
    public Vector3 pointingPos { get => player.pointingPos; set => player.pointingPos = value; }

    public void Initialized_IWeaponAdvanceUser()
    {
       
    }
}
