using UnityEngine;

public class AutomaticShotgunModel : RangeWeapon, PrimaryWeapon
{
    public Transform slingAnchor { get; set; }
    public Transform forntGripAttachment { get; set; }

    public override int maxAmmoCapacity => weaponStatsScriptableObject.bulletCapacity;
    public override Bullet bullet { get; set; }
    public override Chamber chamber { get; protected set; }
    protected override BulletCapacity bulletCap { get; set; }

    // True while the player is mid-reload sequence (after Preload, before QuardLoad finishes)
    public bool _isReloadStanceEntered { get; set; }

    [SerializeField] private AnimationTriggerEventSCRP chamberLoad_timelineSCRP;
    [SerializeField] private AnimationTriggerEventSCRP preload_timelineSCRP;
    [SerializeField] private TimelineTriggerEventScriptableObject quardLoad_timelineSCRP;

    public NodeSelector _shotgunReloadSelector { get; set; }
    public override NodeSelector _reloadSelecotrOverriden => _shotgunReloadSelector;

    public ChamberLoadShotgunNodeLeaf _chamberLoadNode { get; set; }
    public PreloadNodeLeaf _preloadNode { get; set; }
    public QuardLoadNodeLeaf _quardLoadNode { get; set; }

    public override INodeSelector startNodeSelector { get; set; }
    public AutoLoadChamberNode autoLoadChamber { get; set; }
    public override WeaponRestNodeLeaf restNode { get; set; }

    public override void Initialized()
    {
        bullet = new BuckShotBullet(this);
        bulletCap = new BulletCapacity(bullet, maxAmmoCapacity);
        chamber = new Chamber(bullet, bulletSpawner, this);
        fireMode = FireMode.Single;

        InitializeShotgunReloadSelector();
        base.Initialized();
    }

    private void InitializeShotgunReloadSelector()
    {
        if (chamberLoad_timelineSCRP == null || preload_timelineSCRP == null || quardLoad_timelineSCRP == null)
        {
            Debug.LogError($"[{name}] Shotgun reload timeline SCRPs are not assigned in the Inspector.");
            return;
        }

        _shotgunReloadSelector = new NodeSelector(() =>
        {
            if (userWeapon == null) return false;
            return userWeapon._isReloadCommand
                && userWeapon._weaponManuverManager.isReloadManuverAble
                && (curBulletCapacity < maxAmmoCapacity || chamber.isLoad == false)
                && userWeapon._weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo) > 0;
        });

        _chamberLoadNode = new ChamberLoadShotgunNodeLeaf(
            this,
            chamberLoad_timelineSCRP,
            () => chamber.isLoad == false && curBulletCapacity > 0);

        _preloadNode = new PreloadNodeLeaf(
            this,
            preload_timelineSCRP,
            () => _isReloadStanceEntered == false
                && curBulletCapacity < maxAmmoCapacity
                && userWeapon != null
                && userWeapon._weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo) > 0);

        _quardLoadNode = new QuardLoadNodeLeaf(
            this,
            quardLoad_timelineSCRP,
            () => _isReloadStanceEntered
                && curBulletCapacity < maxAmmoCapacity
                && userWeapon != null
                && userWeapon._weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo) > 0);

        _shotgunReloadSelector.AddtoChildNode(_chamberLoadNode);
        _shotgunReloadSelector.AddtoChildNode(_preloadNode);
        _shotgunReloadSelector.AddtoChildNode(_quardLoadNode);
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        fire = new FiringNode(this, this,
            () => chamber.isReadyShoot
                && (triggerState == TriggerState.Down || triggerState == TriggerState.IsDown));

        autoLoadChamber = new AutoLoadChamberNode(this, () => true);

        restNode = new WeaponRestNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(fire);
        startNodeSelector.AddtoChildNode(restNode);

        fire.AddTransitionNode(autoLoadChamber);

        this._nodeManagerBehavior.SearchingNewNode(this);

        base.InitailizedNode();
    }

    protected override void SetDefaultAttribute()
    {
        _isReloadStanceEntered = false;
        bulletCap = new BulletCapacity(bullet, maxAmmoCapacity);
        chamber = new Chamber(bullet, bulletSpawner, this);
        bulletCap.Load(bullet, maxAmmoCapacity, out _);
        chamber.Load(bullet);
        base.SetDefaultAttribute();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Removes one shell from the tube into the caller's hands
    public bool TryGetShellFromTube(out Bullet shell)
    {
        if (bulletCap == null)
        {
            shell = null;
            return false;
        }
        return bulletCap.GetBulletOut(out shell);
    }

    // Pulls count shells from the ammo pouch and loads them into the tube
    public void LoadShellsIntoTube(int count)
    {
        if (userWeapon == null || bulletCap == null) return;
        userWeapon._weaponBelt.ammoProuch.GetAmmoOut(BulletType.buckShotAmmo, count, out int gotten);
        bulletCap.Load(bullet, gotten, out int leftover);
        if (leftover > 0)
            userWeapon._weaponBelt.ammoProuch.AddAmmo(BulletType.buckShotAmmo, leftover);
    }
}
