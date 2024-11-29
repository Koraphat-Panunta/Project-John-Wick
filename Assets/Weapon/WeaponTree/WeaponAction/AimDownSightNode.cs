using System.Collections.Generic;
using UnityEngine;

public class AimDownSightNode : WeaponActionNode
{
    private WeaponActionNode subCurNode;
    private Weapon weapon;
    public AimDownSightNode(WeaponTreeManager weaponTree) : base(weaponTree)
    {
        base.weaponTree = weaponTree;
        weapon = weaponTree.weapon;
    }

    public override void Enter()
    {
        subCurNode = null;
    }

    public override void Exit()
    {
        subCurNode = null;
    }

    public override void FixedUpdate()
    {
        if (subCurNode != null)
            subCurNode.FixedUpdate();
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override bool IsReset()
    {
        return blackBoard.IsAiming == false;
    }

    public override bool PreCondition()
    {
        return blackBoard.IsAiming;
    }

    public override void Update()
    {
        weapon.userWeapon.weaponAfterAction.AimDownSight(weapon);
        weapon.aimingWeight +=weapon.aimDownSight_speed * Time.deltaTime;
        weapon.aimingWeight = Mathf.Clamp(weapon.aimingWeight, 0, 1);
        if (
            subCurNode == null
            || subCurNode.IsReset()
            )
        {
            if (subCurNode != null) subCurNode.Exit(); 
            this.Transition(out subCurNode);
            if(subCurNode != null) subCurNode.Enter();
        }
        if(subCurNode != null)
        subCurNode.Update();
    }
}
