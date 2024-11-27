using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStage : WeaponActionNode
{
    private WeaponBlackBoardMagazineAuto weaponBlackBoard;
    private Weapon weapon;
    public override List<WeaponNode> SubNode { get; set; }
    public ReloadMagazineFullStage(WeaponTreeManager weaponTree) : base(weaponTree)
    {
        this.weaponBlackBoard = weaponTree.WeaponBlackBoard as WeaponBlackBoardMagazineAuto;
        this.weapon = weaponTree.weapon;
    }
    public override void FixedUpdate()
    {
        
    }

    public override bool IsComplete()
    {
        
    }

    public override bool IsReset()
    {
        
    }

    public override bool PreCondition()
    {
        int chamberCount = weaponBlackBoard.BulletStack[BulletStackType.Chamber];
        int magCount = weaponBlackBoard.BulletStack[BulletStackType.Magazine];
        bool isMagIn = weaponBlackBoard.IsMagin;
       
        if
            (
            isMagIn == true 
            && chamberCount ==0
            && magCount == 0
            )
            return true;
        else
            return false;
    }

    public override void Update()
    {
        
    }

    public override void Enter()
    {
       
    }

    public override void Exit()
    {
        
    }
}
