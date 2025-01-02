using UnityEngine;

public interface ICombatOffensiveInstinct 
{
    public CombatOffensiveInstinct combatOffensiveInstinct { get; set; }
    public FieldOfView fieldOfView { get;}
    public GameObject objInstict { get; set; }
    public LayerMask targetLayer { get; set; }
    public bool isInCombat { get; set; }
    public void InitailizedCombatOffensiveInstinct();
    

}
