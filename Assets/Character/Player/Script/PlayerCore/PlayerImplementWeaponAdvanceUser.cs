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

    [SerializeField] public TransformOffsetSCRP quickSwitchHoldOffset;

    public CrosshairController crosshairController;
    
    public Side curShoulderSide;
    public MainHandSocket _mainHandSocket { get => this.MainHandSocket; set => this.MainHandSocket = value; }
    public SecondHandSocket _secondHandSocket { get => this.SecondHandSocket; set => this.SecondHandSocket = value; }

    [SerializeField] private bool isPullTriggerCommand;
    public bool _isPullTriggerCommand { 
        get 
        {
            if(isPullTriggerCommand)
                return true;

            if(_currentWeapon != null
                && _currentWeapon.fireMode == Weapon.FireMode.Single
                && (_currentWeapon.triggerState == TriggerState.Up || _currentWeapon.triggerState == TriggerState.IsUp)
                && commandBufferManager.TryGetCommand(nameof(_isPullTriggerCommand)))
                return true;

            return false;
        }
        set 
        { 
            this.isPullTriggerCommand = value; 
        }
    }
    public bool _isAimingCommand { get; set; }
    public bool _isReloadCommand { get; set; }
    public bool isSwapShoulder;
    public bool _isPickingUpWeaponCommand { get; set; }
    public bool _isDropWeaponCommand { get; set; }
    public bool _isHolsterWeaponCommand { get; set; }
    public bool _isDrawPrimaryWeaponCommand { get; set; }
    public bool _isDrawSecondaryWeaponCommand { get; set; }
    public float _ReloadDuration 
    {
        get
        {
            try
            {
                if ((this.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<GunFuReloadNodeLeaf>()
                    /*&& this._currentWeapon != null*/)
                {
                    return 1.2f;
                }


                if (this._currentWeapon != null)
                    return this._currentWeapon.reloadTime;

                return 1;
            }
            catch
            {
                if (this._currentWeapon != null)
                    return this._currentWeapon.reloadTime;

                return 1;
            }

        }
    }

    public Weapon _currentWeapon { get => this._mainHandSocket.curWeaponAtSocket; }
    public WeaponBelt _weaponBelt { get; set; }
    public WeaponAfterAction _weaponAfterAction { get; set; }
    public WeaponNodeManuverManager _weaponManuverManager { get; set; }
    public Vector3 _shootingPos
    {
        get
        {
            if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>())
            {
                Ray ray = new Ray(_currentWeapon.bulletSpawner.transform.position, _currentWeapon.bulletSpawner.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, 0))
                    return hitInfo.point;
                else
                    return ray.GetPoint(100);
            }

            Vector3 buttetSpanwToPointingPos = this._pointingPos - this._currentWeapon.bulletSpawner.transform.position;

            float dot = Vector3.Dot(buttetSpanwToPointingPos.normalized, this._currentWeapon.bulletSpawner.transform.forward) ;

            if(dot < 0.98f)
            {
                //Debug.Log("dot not = " + dot);
                Vector3 shootPointPos = this._currentWeapon.bulletSpawner.transform.position + (this._currentWeapon.bulletSpawner.transform.forward * 100);
                //Debug.DrawLine(this._currentWeapon.bulletSpawner.transform.position, shootPointPos, Color.green,5);
                return shootPointPos;             
            }
            //Debug.Log("dot = " + dot);

            Vector3 shootPoint = this.crosshairController.CrosshiarShootpoint.GetShootPointDirection();
            //Debug.DrawLine(this._currentWeapon.bulletSpawner.transform.position, shootPoint, Color.red, 5);
            return shootPoint;
        }
        set { }
    }
    public Vector3 _pointingPos { get => this.crosshairController.CrosshiarShootpoint.GetPointDirection(); set { } }
    public Vector3 _lookingPos => crosshairController.targetAim;
    public Animator _weaponUserAnimator { get; set; }
    public Character _userWeapon { get => this; }
    public FindingWeaponBehavior _findingWeaponBehavior { get; set; }



    public void Initialized_IWeaponAdvanceUser()
    {
        _shootingPos = new Vector3();

        _weaponUserAnimator = animator;
        _findingWeaponBehavior = new FindingWeaponBehavior(this);
        _weaponBelt = new WeaponBelt(PrimaryWeaponSocket, SecondaryWeaponSocket, new AmmoProuch());
        //_weaponBelt.ammoProuch.SetMaximunAmmo(BulletType.rifleAmmo, 1000);
        //_weaponBelt.ammoProuch.SetMaximunAmmo(BulletType.handgunAmmo, 1000);
        //_weaponBelt.ammoProuch.SetAmmo(BulletType.rifleAmmo, 1000);
        //_weaponBelt.ammoProuch.SetAmmo(BulletType.handgunAmmo, 1000);
        _weaponAfterAction = new WeaponAfterActionPlayer(this);

        _weaponManuverManager = new PlayerWeaponManuver(this, this);
    }
    #endregion
}
