using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoadChamberNode : WeaponLeafNode
{
    private Coroutine reChamber;

    public AutoLoadChamberNode(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
    }

    public override void Enter()
    {
        reChamber = Weapon.StartCoroutine(ReChambering());

    }

    public override void Exit()
    {

    }

    public override void UpdateNode()
    {

    }
    public override void FixedUpdateNode()
    {

    }

    public override bool IsComplete()
    {
        return reChamber == null;
    }
    public IEnumerator ReChambering()
    {
        //Debug.Log("Rechamber");
        yield return new WaitForSeconds((float)(60 / Weapon.rate_of_fire));
        if (Weapon.bulletStore[BulletStackType.Magazine] > 0)
        {
            Weapon.bulletStore[BulletStackType.Chamber] += 1;
            Weapon.bulletStore[BulletStackType.Magazine] -= 1;
        }
        reChamber = null;
    }

}
