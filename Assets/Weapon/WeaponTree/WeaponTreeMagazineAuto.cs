using UnityEngine;

public class WeaponTreeMagazineAuto : WeaponTreeManager
{
    public override WeaponNode currentNode { get ; set ; }
    public override WeaponSelector startNode { get ;protected set ; }
    public override WeaponBlackBoard WeaponBlackBoard { get ; set ; }
    public WeaponTreeMagazineAuto(Weapon weapon):base(weapon) 
    {
        this.WeaponBlackBoard = new WeaponBlackBoardMagazineAuto(weapon);
        currentNode = startNode;
    }
    public override void FixedUpdateTree()
    {
        if (currentNode.IsReset())
        {
            (currentNode as WeaponActionNode).Exit();
            currentNode = startNode;
            currentNode.Transition(out WeaponActionNode weaponActionNode);
            currentNode = weaponActionNode;
            (currentNode as WeaponActionNode).Enter();
        }
        currentNode.FixedUpdate();
    }

    public override void InitailizedTree()
    {
        
    }

    public override void UpdateTree()
    {
        if (currentNode.IsReset())
        {
            (currentNode as WeaponActionNode).Exit();
            currentNode = startNode;
            currentNode.Transition(out WeaponActionNode weaponActionNode);
            currentNode = weaponActionNode;
            (currentNode as WeaponActionNode).Enter();
        }
        currentNode.Update();
    }

}
