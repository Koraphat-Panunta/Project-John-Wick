using Unity.VisualScripting;
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
    public ReloadMagazineFullStage reloadMagazineFullStage { get; private set; }
    public override void InitailizedTree()
    {
        reloadMagazineFullStage = new ReloadMagazineFullStage(this);

        stanceSelector = new WeaponSelector(this, 
            () => { return true;}
            );
        reloadStageSelector =new WeaponSelector(this,
            () => { return weapon.isReloadCommand == true;}
            );

        reloadStageSelector.childNode.Add(reloadMagazineFullStage);
    }

}
