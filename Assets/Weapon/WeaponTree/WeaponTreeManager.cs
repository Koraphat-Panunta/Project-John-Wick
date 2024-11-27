using UnityEngine;

public abstract class WeaponTreeManager 
{
   public abstract WeaponBlackBoard WeaponBlackBoard { get; set; }
    public abstract WeaponNode currentNode { get; set ; }
    public abstract WeaponSelector startNode { get; protected set; }
    public Weapon weapon; 
    public WeaponTreeManager(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public abstract void FixedUpdateTree();


    public abstract void InitailizedTree();


    public abstract void UpdateTree();
    
}
