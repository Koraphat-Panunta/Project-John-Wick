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

    [SerializeField] private AnimationTriggerEventSCRP chamberLoad_timelineSCRP;
    [SerializeField] private AnimationTriggerEventSCRP preload_timelineSCRP;
    [SerializeField] private TimelineTriggerEventScriptableObject quardLoad_timelineSCRP;

    public NodeSelector _shotgunReloadSelector { get; set; }
    public override NodeSelector _reloadSelecotrOverriden => _shotgunReloadSelector;

    public ChamberLoadShotgunNodeLeaf _chamberLoadNodeLeaf { get; set; }
    public PreloadNodeLeaf _preloadNodeLeaf { get; set; }
    public QuardLoadNodeLeaf _quardLoadNodeLeaf { get; set; }

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

        _chamberLoadNodeLeaf = new ChamberLoadShotgunNodeLeaf(
            this,
            chamberLoad_timelineSCRP,
            () => chamber.isLoad == false );

        _preloadNodeLeaf = new PreloadNodeLeaf(
            this,
            preload_timelineSCRP,
            () => curBulletCapacity < maxAmmoCapacity
                && userWeapon != null
                && userWeapon._weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo) > 0);

        _quardLoadNodeLeaf = new QuardLoadNodeLeaf(
            this,
            quardLoad_timelineSCRP,
            () =>  curBulletCapacity < maxAmmoCapacity
                && userWeapon != null
                && userWeapon._weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo) > 0);

        this._chamberLoadNodeLeaf.AddTransitionNode(_preloadNodeLeaf);
        this._preloadNodeLeaf.AddTransitionNode(_quardLoadNodeLeaf);

        _shotgunReloadSelector.AddtoChildNode(_chamberLoadNodeLeaf);
        _shotgunReloadSelector.AddtoChildNode(_preloadNodeLeaf);
        _shotgunReloadSelector.AddtoChildNode(_quardLoadNodeLeaf);
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
