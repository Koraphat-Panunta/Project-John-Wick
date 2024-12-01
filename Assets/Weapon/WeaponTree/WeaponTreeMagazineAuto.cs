using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WeaponTreeMagazineAuto : WeaponTreeManager
{
    public override WeaponBlackBoard WeaponBlackBoard { get ; set ; }
    public WeaponTreeMagazineAuto(Weapon weapon):base(weapon) 
    {
        this.WeaponBlackBoard = new WeaponBlackBoardMagazineAuto(weapon);
    }
    public WeaponSelector stanceSelector { get; private set; }
    public WeaponSelector reloadStageSelector { get; private set; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    public ReloadMagazineFullStage reloadMagazineFullStage { get; private set; }
    public TacticalReloadMagazineFullStage tacticalReloadMagazineFullStage { get; private set; }
    public override WeaponSelector startNode { get ; set ; }

    private AimDownSightNode aimDownSight;
    private LowReadyNode lowReady;
    private FiringNode fire;
    private AutoLoadChamberNode autoLoadChamber;
    public override void FixedUpdateTree()
    {
        base.FixedUpdateTree();
    }
    public override void UpdateTree()
    {
        base.UpdateTree();
    }
    public override void InitailizedTree()
    {
        reloadMagazineFullStage = new ReloadMagazineFullStage(this);
        tacticalReloadMagazineFullStage = new TacticalReloadMagazineFullStage(this);
        stanceSelector = new WeaponSelector(this, 
            () => { return true;}
            );
        reloadStageSelector =new WeaponSelector(this,
            () => { return weapon.isReloadCommand == true && WeaponBlackBoard.BulletStack[BulletStackType.Magazine]<weapon.Magazine_capacity;}
            );
        firingAutoLoad = new WeaponSequenceNode(this,
            () => { return WeaponBlackBoard.BulletStack[BulletStackType.Chamber] > 0 && WeaponBlackBoard.TriggerState == TriggerState.Down; }
            );

        aimDownSight = new AimDownSightNode(this);
        lowReady = new LowReadyNode(this);
        fire = new FiringNode(this);
        autoLoadChamber = new AutoLoadChamberNode(this);

        startNode = stanceSelector;

        stanceSelector.AddChildNode(reloadStageSelector);
        stanceSelector.AddChildNode(aimDownSight);
        stanceSelector.AddChildNode(lowReady);

        reloadStageSelector.AddChildNode(reloadMagazineFullStage);
        reloadStageSelector.AddChildNode(tacticalReloadMagazineFullStage);
        
        aimDownSight.AddChildNode(firingAutoLoad);

        firingAutoLoad.AddChildNode(fire);
        firingAutoLoad.AddChildNode(autoLoadChamber);

        currentNode = startNode;
    }

}
