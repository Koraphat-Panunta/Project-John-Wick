using UnityEngine;

public class LowReadyNode : WeaponActionNode
{
    private Weapon weapon;
    public LowReadyNode(WeaponTreeManager weaponTree) : base(weaponTree)
    {
        this.weapon = weaponTree.weapon;
    }
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override bool IsReset()
    {
        return blackBoard.IsAiming||blackBoard.IsEquip==false;
    }

    public override bool PreCondition()
    {
        return true;
    }

    public override void Update()
    {
        if (weaponTree.weapon.userWeapon == null) Debug.Log("User weapon = null");
        weapon.userWeapon.weaponAfterAction.LowReady(weapon);
        weapon.aimingWeight -= weapon.aimDownSight_speed * Time.deltaTime;
        weapon.aimingWeight = Mathf.Clamp(weapon.aimingWeight, 0, 1);
    }

    
}
