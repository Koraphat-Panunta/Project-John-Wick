using UnityEngine;

public abstract class WeaponTreeManager 
{
   public abstract WeaponBlackBoard WeaponBlackBoard { get; set; }
    public  WeaponNode currentNode { get; set ; }
    public abstract WeaponSelector startNode { get; set; }
    public Weapon weapon; 
    public WeaponTreeManager(Weapon weapon)
    {
        this.weapon = weapon;
        currentNode = startNode;
    }

    public virtual void FixedUpdateTree()
    {
       
        if(currentNode != null)
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
            if(currentNode is WeaponActionNode)
            (currentNode as WeaponActionNode).Exit();
            currentNode = startNode;
            currentNode.Transition(out WeaponActionNode weaponActionNode);
            currentNode = weaponActionNode;
            (currentNode as WeaponActionNode).Enter();
        }
        currentNode.Update();
    }
    
}
