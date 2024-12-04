using UnityEngine;

public class LowReadyNode : WeaponActionNode
{
    public LowReadyNode(Weapon weapon) : base(weapon)
    {
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
        return Weapon.isAiming||Weapon.isEquip==false;
    }

    public override bool PreCondition()
    {
        return true;
    }

    public override void Update()
    {
        if(Weapon.userWeapon != null)
        Weapon.userWeapon.weaponAfterAction.LowReady(Weapon);
        Weapon.aimingWeight -= Weapon.aimDownSight_speed * Time.deltaTime;
        Weapon.aimingWeight = Mathf.Clamp(Weapon.aimingWeight, 0, 1);
    }

    
}
