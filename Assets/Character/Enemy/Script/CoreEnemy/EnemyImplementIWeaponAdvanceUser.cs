using UnityEngine;

public partial class Enemy : IWeaponAdvanceUser
{
    [SerializeField] public Transform pointingTransform;

    [SerializeField] private MainHandSocket MainHandSocket;
    [SerializeField] private SecondHandSocket SecondHandSocket;
    [SerializeField] private PrimaryWeaponSocket PrimaryWeaponSocket;
    [SerializeField] private SecondaryWeaponSocket SecondaryWeaponSocket;

    public bool _isPullTriggerCommand { get; set; }
    public bool _isAimingCommand { get; set; }
    public bool _isReloadCommand { get; set; }
    public bool _isDropWeaponCommand { get; set; }
    public bool _isPickingUpWeaponCommand { get; set; }
    public bool _isHolsterWeaponCommand { get; set; }
    public bool _isDrawPrimaryWeaponCommand { get; set; }
    public bool _isDrawSecondaryWeaponCommand { get; set; }

    public MainHandSocket _mainHandSocket { get => this.MainHandSocket; set => this.MainHandSocket = value; }
    public SecondHandSocket _secondHandSocket { get => this.SecondHandSocket; set => this.SecondHandSocket = value; }

    public Animator _weaponUserAnimator { get; set; }
    public Weapon _currentWeapon { get; set; }

    public Vector3 _shootingPos
    {
        get { return enemyGetShootDirection.GetShootingPos(); }
        set { }
    }
    public Vector3 _pointingPos
    {
        get => enemyGetShootDirection.GetPointingPos();
        set { }
    }

    public WeaponBelt _weaponBelt { get; set; }
    public WeaponAfterAction _weaponAfterAction { get; set; }
    public Character _userWeapon => this;

    [SerializeField] AnimatorOverrideController AnimatorOverrideController;
    public AnimatorOverrideController _animatorWeaponAdvanceUserOverride { get; set; }
    public WeaponManuverManager _weaponManuverManager { get; set; }
    public FindingWeaponBehavior _findingWeaponBehavior { get; set; }

    [Range(0,1)]
    public float trackingTargetAccelerate = .002f;
    [Range(0, 1)]
    public float trackingTargetDecelerate = 1;
    [Range(0, 1)]
    public float maxTrackRate = .1f;

    [SerializeField] public float curTrackRate;
    [SerializeField] private bool isSpottingTaget;
    public void Initialized_IWeaponAdvanceUser()
    {
        //pointingTransform.transform.SetParent(null, true);
        
        _weaponUserAnimator = animator;
        _weaponBelt = new WeaponBelt(PrimaryWeaponSocket, SecondaryWeaponSocket, new AmmoProuch(1000, 1000, 1000, 1000
            , 1000, 1000, 1000, 1000));
        _weaponAfterAction = new WeaponAfterActionEnemy(this);
        _findingWeaponBehavior = new FindingWeaponBehavior(this);
        _weaponManuverManager = new EnemyWeaponManuver(this, this);
        _animatorWeaponAdvanceUserOverride = this.AnimatorOverrideController;
    }
}
