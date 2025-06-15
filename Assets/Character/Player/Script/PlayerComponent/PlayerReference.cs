using UnityEngine;

public class PlayerReference : MonoBehaviour,IWeaponAdvanceUser
{
    [SerializeField] private Player player;
    public Animator _weaponUserAnimator { get => player.animator; set => player.animator = value; }
    public Weapon _currentWeapon { get => player._currentWeapon; set => player._currentWeapon = value; }
  
    public Vector3 _shootingPos { get => player._shootingPos; set => player._shootingPos = value; }
    public WeaponBelt _weaponBelt { get => player._weaponBelt; set => player._weaponBelt = value; }
    public WeaponAfterAction _weaponAfterAction { get => player._weaponAfterAction; set => player._weaponAfterAction = value; }
    public Character _userWeapon => player;
    public Vector3 _pointingPos { get => player._pointingPos; set => player._pointingPos = value; }
    public WeaponManuverManager _weaponManuverManager { get => player._weaponManuverManager; set => player._weaponManuverManager = value; }
    public bool _isSwitchWeaponCommand { get => player._isSwitchWeaponCommand; set => _isSwitchWeaponCommand = value; }
    public bool _isPullTriggerCommand { get => player._isPullTriggerCommand; set => player._isPullTriggerCommand = value; }
    public bool _isAimingCommand { get => player._isAimingCommand; set => player._isAimingCommand = value; }
    public bool _isReloadCommand { get => player._isReloadCommand; set => player._isReloadCommand = value; }
    public FindingWeaponBehavior _findingWeaponBehavior { get => player._findingWeaponBehavior; set => player._findingWeaponBehavior = value; }
    public bool _isPickingUpWeaponCommand { get => player._isPickingUpWeaponCommand; set => player._isPickingUpWeaponCommand = value; }
    public bool _isDropWeaponCommand { get => player._isDropWeaponCommand; set => player._isDropWeaponCommand = value; }
    public AnimatorOverrideController _animatorWeaponAdvanceUserOverride { get => player._animatorWeaponAdvanceUserOverride; set => player._animatorWeaponAdvanceUserOverride = value; }
    public MainHandSocket _mainHandSocket { get => player._mainHandSocket; set => player._mainHandSocket = value; }
    public SecondHandSocket _secondHandSocket { get => player._secondHandSocket; set => player._secondHandSocket = value; }

    public void Initialized_IWeaponAdvanceUser()
    {
       player.Initialized_IWeaponAdvanceUser();
    }
}
