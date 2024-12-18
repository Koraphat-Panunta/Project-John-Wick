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
    public ReloadStageSelector reloadStageSelector { get; private set; }
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
        reloadMagazineFullStage = new ReloadMagazineFullStage(weapon);
        tacticalReloadMagazineFullStage = new TacticalReloadMagazineFullStage(weapon);
        startNode = new WeaponSelector(weapon, () => true);

        reloadStageSelector = new ReloadStageSelector(weapon);
        stanceSelector = new WeaponSelector(weapon, 
            () => { return true;}
            );
        firingAutoLoad = new WeaponSequenceNode(weapon,
            () => { return WeaponBlackBoard.BulletStack[BulletStackType.Chamber] > 0 && WeaponBlackBoard.TriggerState == TriggerState.Down; }
            );

        aimDownSight = new AimDownSightNode(weapon);
        lowReady = new LowReadyNode(weapon);
        fire = new FiringNode(weapon);
        autoLoadChamber = new AutoLoadChamberNode(weapon);

        startNode.AddChildNode(stanceSelector);

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
