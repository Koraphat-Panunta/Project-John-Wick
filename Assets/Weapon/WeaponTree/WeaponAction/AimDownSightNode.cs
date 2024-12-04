using System.Collections.Generic;
using UnityEngine;

public class AimDownSightNode : WeaponActionNode
{
    private WeaponActionNode subCurNode;
    public AimDownSightNode(Weapon weapon) : base(weapon)
    {
       
    }

    public override void Enter()
    {
        subCurNode = this;
    }

    public override void Exit()
    {
        subCurNode = null;
    }

    public override void FixedUpdate()
    {
        //if (subCurNode != null)
            //subCurNode.FixedUpdate();
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override bool IsReset()
    {
        return Weapon.isAiming == false;
    }

    public override bool PreCondition()
    {
        return Weapon.isAiming;
    }

    public override void Update()
    {
        Weapon.userWeapon.weaponAfterAction.AimDownSight(Weapon);
        Weapon.aimingWeight += Weapon.aimDownSight_speed * Time.deltaTime;
        Weapon.aimingWeight = Mathf.Clamp(Weapon.aimingWeight, 0, 1);
        if (
            subCurNode == null
            || subCurNode.IsReset()
            )
        {
            if (subCurNode != null) subCurNode.Exit(); 
            this.Transition(out subCurNode);
            if(subCurNode != null) subCurNode.Enter();
        }
        //if(subCurNode != null)
        //subCurNode.Update();
    }
}
