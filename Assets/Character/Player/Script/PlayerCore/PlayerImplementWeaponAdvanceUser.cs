using UnityEngine;

public partial class Player: IWeaponAdvanceUser
{
    #region InitailizedWeaponAdvanceUser

    [SerializeField] private MainHandSocket MainHandSocket;
    [SerializeField] private SecondHandSocket SecondHandSocket;
    [SerializeField] private PrimaryWeaponSocket PrimaryWeaponSocket;
    [SerializeField] private SecondaryWeaponSocket SecondaryWeaponSocket;

    [SerializeField] public AnimationTriggerEventSCRP quickSwitchDrawSCRP;
    [SerializeField] public AnimationTriggerEventSCRP quickSiwthcHolsterPrimarySCRP;
    [SerializeField] public AnimationTriggerEventSCRP quickSwitchHoslterSecondarySCRP;

    public CrosshairController crosshairController;
    public enum ShoulderSide
    {
        Left,
        Right
    }
    public ShoulderSide curShoulderSide;
    public MainHandSocket _mainHandSocket { get => this.MainHandSocket; set => this.MainHandSocket = value; }
    public SecondHandSocket _secondHandSocket { get => this.SecondHandSocket; set => this.SecondHandSocket = value; }

    public bool _isPullTriggerCommand { get; set; }
    public bool _isAimingCommand { get; set; }
    public bool _isReloadCommand { get; set; }
    public bool isSwapShoulder;
    public bool _isPickingUpWeaponCommand { get; set; }
    public bool _isDropWeaponCommand { get; set; }
    public bool _isHolsterWeaponCommand { get; set; }
    public bool _isDrawPrimaryWeaponCommand { get; set; }
    public bool _isDrawSecondaryWeaponCommand { get; set; }

    public Weapon _currentWeapon { get; set; }
    public WeaponBelt _weaponBelt { get; set; }
    public WeaponAfterAction _weaponAfterAction { get; set; }
    public WeaponManuverManager _weaponManuverManager { get; set; }
    public Vector3 _shootingPos
    {
        get
        {
            if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>())
            {
                Ray ray = new Ray(_currentWeapon.bulletSpawnerPos.position, _currentWeapon.bulletSpawnerPos.forward);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, 0))
                    return hitInfo.point;
                else
                    return ray.GetPoint(100);
            }
            return crosshairController.CrosshiarShootpoint.GetShootPointDirection();
        }
        set { }
    }
    public Vector3 _pointingPos { get => crosshairController.CrosshiarShootpoint.GetPointDirection(); set { } }
    public Animator _weaponUserAnimator { get; set; }
    public Character _userWeapon { get => this; }
    [SerializeField] private AnimatorOverrideController AnimatorOverrideController;
    public AnimatorOverrideController _animatorWeaponAdvanceUserOverride { get => this.AnimatorOverrideController; set => this.AnimatorOverrideController = value; }
    public FindingWeaponBehavior _findingWeaponBehavior { get; set; }
    public void Initialized_IWeaponAdvanceUser()
    {
        _shootingPos = new Vector3();

        _weaponUserAnimator = animator;
        _findingWeaponBehavior = new FindingWeaponBehavior(this);
        _weaponBelt = new WeaponBelt(PrimaryWeaponSocket, SecondaryWeaponSocket, new AmmoProuch(45, 45, 30, 30
            , 45, 45, 60, 60));
        _weaponAfterAction = new WeaponAfterActionPlayer(this);

        _weaponManuverManager = new PlayerWeaponManuver(this, this);
    }
    #endregion
}
