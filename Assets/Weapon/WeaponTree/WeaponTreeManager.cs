using UnityEngine;

public abstract class WeaponTreeManager 
{
   public abstract WeaponBlackBoard WeaponBlackBoard { get; set; }
    public  WeaponNode currentNode { get; set ; }
    public WeaponSelector startNode { get; protected set; }
    public Weapon weapon; 
    public WeaponTreeManager(Weapon weapon)
    {
        this.weapon = weapon;
        startNode = new WeaponSelector(this, () => true);
        currentNode = startNode;
    }

    public virtual void FixedUpdateTree()
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
    public virtual void ChangeTreeManualy(WeaponActionNode weaponActionNode)
    {
        (currentNode as WeaponActionNode).Exit();
        currentNode = weaponActionNode;
        (currentNode as WeaponActionNode).Enter();
    }

    public abstract void InitailizedTree();


    public virtual void UpdateTree()
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
