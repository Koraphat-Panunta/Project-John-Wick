using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoadChamberNode : WeaponActionNode
{
    private Weapon weapon;
    private Coroutine reChamber;
    public AutoLoadChamberNode(WeaponTreeManager weaponTree):base(weaponTree) 
    {
        weapon = weaponTree.weapon;
    }
    public override void Enter()
    {
        reChamber = weapon.StartCoroutine(ReChambering());
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override bool IsComplete()
    {
        return reChamber == null;
    }

    public override bool IsReset()
    {
        return false;
    }

    public override bool PreCondition()
    {
        return true;
    }

    public override void Update()
    {
    }
    public IEnumerator ReChambering()
    {
        yield return new WaitForSeconds((float)(60 / blackBoard.Rate_of_fire));
        if (weapon.bulletStore[BulletStackType.Magazine] > 0)
        {
            weapon.bulletStore[BulletStackType.Chamber] += 1;
            weapon.bulletStore[BulletStackType.Magazine] -= 1;
        }
        reChamber = null;
    }
}
