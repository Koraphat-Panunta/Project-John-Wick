using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManuverSelectorNode : WeaponManuverNode
{
    public List<WeaponManuverNode> childNode = new List<WeaponManuverNode>();
    public Dictionary<WeaponManuverNode, Func<bool>> nodePrecon = new Dictionary<WeaponManuverNode, Func<bool>>();

    public WeaponManuverSelectorNode parentNode;
    public WeaponManuverSelectorNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {

    }
    public override bool Precondition()
    {
        return base.Precondition();
    }
    public void PopulatePrecondition(out List<Func<bool>> nodeLeafReset,WeaponManuverNode nodePosition)
    {
        nodeLeafReset = new List<Func<bool>>();

        foreach(WeaponManuverNode weaponManuverNode in nodePrecon.Keys)
        {
            if(weaponManuverNode == nodePosition)
                break;

            nodeLeafReset.Add(weaponManuverNode.preCpndition);
        }

        nodeLeafReset.Add(() => !preCpndition());

        if(parentNode == null)
            return;

        parentNode.PopulatePrecondition(out List<Func<bool>> parentLeafReset,this);

        foreach(Func<bool> pReset in parentLeafReset)
        {
            nodeLeafReset.Add((Func<bool>)pReset);
        }
    }
    public void AddtoChildNode(WeaponManuverNode weaponManuverNode)
    {
        childNode.Add(weaponManuverNode);
        nodePrecon.Add(weaponManuverNode, weaponManuverNode.preCpndition);

        if(weaponManuverNode is WeaponManuverSelectorNode weaponManuverSelector)
            weaponManuverSelector.parentNode = this;

        if (weaponManuverNode is WeaponManuverLeafNode leafNode)
        {
            PopulatePrecondition(out List<Func<bool>> isReset, weaponManuverNode);
            isReset.ForEach(resetCon => leafNode.isReset.Add(resetCon));
        }


    }
    public void RemoveChildNode(WeaponManuverNode weaponManuverNode)
    {
        childNode.Remove(weaponManuverNode);
        nodePrecon.Remove(weaponManuverNode);
    }
    public bool FindingNode(out WeaponManuverLeafNode weaponManuverLeafNode)
    {
        weaponManuverLeafNode = null;

        foreach (WeaponManuverNode weaponManuverNode in childNode)
        {
            if (weaponManuverNode.Precondition() == false) 
            {
                Debug.Log("WeaponManuverNode " + this + " -> " + weaponManuverNode+" is false");
                continue;
            }
               

            Debug.Log("WeaponManuverNode " + this +" -> " + weaponManuverNode);

            if (weaponManuverNode is WeaponManuverLeafNode manuverLeafNode)
            {
                weaponManuverLeafNode = manuverLeafNode;
                return true;
            }

            else if (weaponManuverNode is WeaponManuverSelectorNode weaponManuverSelectorNode)
            {
                weaponManuverSelectorNode.FindingNode(out weaponManuverLeafNode);
                return true;
            }
        }
        return false;
        
    }
}
